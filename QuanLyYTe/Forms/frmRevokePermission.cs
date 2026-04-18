using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyYTe.DAL;

namespace QuanLyYTe.Forms
{
    public partial class frmRevokePermission : Form
    {
        private readonly PrivilegeRepository _repo = new PrivilegeRepository();
        private DataTable _dtPrivileges;

        public frmRevokePermission()
        {
            InitializeComponent();
        }

        private void FrmPrivilege_Load(object sender, EventArgs e)
        {
            ApplyGridStyle(dgvPrivilege);
            cbSearchType.SelectedIndex = 0; // Default to "User"
            cbFilterType.SelectedIndex = 0; // Default to "Tất cả"
        }

        // ================================================================
        // COMBOBOX DYNAMIC LOADING
        // ================================================================
        private void cbSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = cbSearchType.Text;
            try
            {
                if (type == "User")
                {
                    lblSearchName.Text = "Tên User";
                    DataTable dt = _repo.GetUsers();
                    cbSearchName.DataSource = dt;
                    cbSearchName.DisplayMember = "USERNAME";
                    cbSearchName.ValueMember = "USERNAME";
                }
                else if (type == "Role")
                {
                    lblSearchName.Text = "Tên Role";
                    DataTable dt = _repo.GetRoles();
                    cbSearchName.DataSource = dt;
                    cbSearchName.DisplayMember = "ROLENAME";
                    cbSearchName.ValueMember = "ROLENAME";
                }
                else if (type == "Đối tượng")
                {
                    lblSearchName.Text = "Tên Đối tượng";
                    string sql = "SELECT object_name FROM ALL_OBJECTS WHERE OWNER = 'HOSPITAL' " +
                                 "AND object_type IN ('TABLE', 'VIEW') ORDER BY object_name";
                    DataTable dtObj = OracleHelper.ExecuteQuery(sql);
                    if (dtObj != null && dtObj.Rows.Count > 0)
                    {
                        cbSearchName.DataSource = dtObj;
                        cbSearchName.DisplayMember = "object_name";
                        cbSearchName.ValueMember = "object_name";
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Lỗi tải danh sách", ex.Message);
            }
        }

        // ================================================================
        // ACTIONS
        // ================================================================
        private void btnView_Click(object sender, EventArgs e)
        {
            string searchType = cbSearchType.Text;
            string searchName = cbSearchName.Text.Trim();

            if (string.IsNullOrEmpty(searchName))
            {
                ShowWarn($"Vui lòng nhập hoặc chọn {searchType}!");
                return;
            }

            if (searchType == "Đối tượng")
                LoadByObject(searchName);
            else
                LoadByGrantee(searchName);
        }

        private void LoadByGrantee(string grantee)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                _dtPrivileges = _repo.GetAllPrivileges(grantee);
                BindGrid(_dtPrivileges);
                RenameColumns_ByGrantee();
                UpdateSummaryLabel(grantee, false);
                FilterGrid(); 
            }
            catch (Exception ex) { ShowError("Lỗi xem quyền", ex.Message); }
            finally { Cursor = Cursors.Default; }
        }

        private void LoadByObject(string objName)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                _dtPrivileges = _repo.GetPrivsOnObject("HOSPITAL", objName);
                BindGrid(_dtPrivileges);
                RenameColumns_ByObject();
                UpdateSummaryLabel(objName, true);
                FilterGrid();
            }
            catch (Exception ex) { ShowError("Lỗi xem quyền đối tượng", ex.Message); }
            finally { Cursor = Cursors.Default; }
        }

        private void btnRevoke_Click(object sender, EventArgs e)
        {
            if (dgvPrivilege.SelectedRows.Count == 0)
            {
                ShowWarn("Vui lòng chọn 1 dòng quyền bên trên để thu hồi.");
                return;
            }

            DataGridViewRow row = dgvPrivilege.SelectedRows[0];

            string type = GetCell(row, "LOAI_QUYEN");
            string privilege = GetCell(row, "QUYEN");
            string owner = GetCell(row, "CHU_SO_HUU");
            string obj = GetCell(row, "DOI_TUONG");
            string column = GetCell(row, "COT");
            
            // Tìm grantee
            string grantee = "";
            if (cbSearchType.Text == "Đối tượng")
                grantee = GetCell(row, "NGUOI_NHAN"); // NGUOI_NHAN là cột trả về từ USP_GET_PRIVS_ON_OBJ
            else
                grantee = cbSearchName.Text.Trim();

            if (string.IsNullOrEmpty(grantee)) return;

            // KIỂM TRA ĐIỀU KIỆN: Không cho DBA tự thu hồi chính mình.
            if (grantee.Trim().ToUpper() == "HOSPITAL_DBA")
            {
                ShowWarn("Tuyệt đối không thể tự thu hồi quyền của chính tài khoản Database Administrator (HOSPITAL_DBA).");
                return;
            }

            string info = $"Người nhận: {grantee}\nLoại đối tượng: {type}\nQuyền: {privilege}";
            if (!string.IsNullOrEmpty(obj)) info += $"\nĐối tượng: {owner}.{obj}";
            if (!string.IsNullOrEmpty(column)) info += $"\nCột: {column}";

            if (MessageBox.Show($"Bạn có chắc chắn muốn thu hồi quyền sau?\n\n{info}", "Xác nhận Thu hồi",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                _repo.RevokePrivilege(type, privilege, owner, obj, column, grantee);
                MessageBox.Show("Thu hồi quyền thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnView_Click(null, null); // Reload grid
            }
            catch (Exception ex) { ShowError("Lỗi thu hồi", ex.Message); }
            finally { Cursor = Cursors.Default; }
        }

        // ================================================================
        // FILTER
        // ================================================================
        private void cbFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterGrid();
        }

        private void txtKeyword_TextChanged(object sender, EventArgs e)
        {
            FilterGrid();
        }

        private void FilterGrid()
        {
            if (_dtPrivileges == null || dgvPrivilege.DataSource == null) return;

            string type = cbFilterType.Text;
            DataView dv = _dtPrivileges.DefaultView;

            // Xây dựng điều kiện lọc theo Type Combobox
            string typeFilter = "";
            if (_dtPrivileges.Columns.Contains("LOAI_QUYEN")) 
            {
                if (type == "Quyền đối tượng")
                    typeFilter = "LOAI_QUYEN IN ('TABLE', 'VIEW', 'PROCEDURE', 'FUNCTION')";
                else if (type == "Quyền theo cột")
                    typeFilter = "LOAI_QUYEN = 'COLUMN'";
                else if (type == "Quyền hệ thống")
                    typeFilter = "LOAI_QUYEN = 'SYSTEM'";
                else if (type == "Role được cấp")
                    typeFilter = "LOAI_QUYEN = 'ROLE'";
            }

            // Xây dựng điều kiện quét Keyword (cột Kiểu, Quyền, Schema, Đối tượng)
            string keyword = txtKeyword.Text.Trim();
            string keywordFilter = "";
            if (!string.IsNullOrEmpty(keyword))
            {
                string kw = keyword.Replace("'", "''"); // Tránh lỗi cú pháp RowFilter
                System.Collections.Generic.List<string> kwList = new System.Collections.Generic.List<string>();
                
                if (_dtPrivileges.Columns.Contains("LOAI_QUYEN")) kwList.Add($"LOAI_QUYEN LIKE '%{kw}%'");
                if (_dtPrivileges.Columns.Contains("QUYEN"))      kwList.Add($"QUYEN LIKE '%{kw}%'");
                if (_dtPrivileges.Columns.Contains("CHU_SO_HUU")) kwList.Add($"CHU_SO_HUU LIKE '%{kw}%'");
                if (_dtPrivileges.Columns.Contains("DOI_TUONG"))  kwList.Add($"DOI_TUONG LIKE '%{kw}%'");
                if (_dtPrivileges.Columns.Contains("NGUOI_NHAN")) kwList.Add($"NGUOI_NHAN LIKE '%{kw}%'");

                if (kwList.Count > 0)
                    keywordFilter = "(" + string.Join(" OR ", kwList) + ")";
            }

            // Gộp cả 2 điều kiện lại (nếu có)
            string finalFilter = "";
            if (!string.IsNullOrEmpty(typeFilter) && !string.IsNullOrEmpty(keywordFilter))
                finalFilter = $"{typeFilter} AND {keywordFilter}";
            else if (!string.IsNullOrEmpty(typeFilter))
                finalFilter = typeFilter;
            else if (!string.IsNullOrEmpty(keywordFilter))
                finalFilter = keywordFilter;

            dv.RowFilter = finalFilter;
            FitColumns();
        }

        // ================================================================
        // HELPERS
        // ================================================================
        private void BindGrid(DataTable dt)
        {
            dgvPrivilege.DataSource = dt;
            FitColumns();
        }

        private void UpdateSummaryLabel(string targetName, bool isObjectMode)
        {
            if (_dtPrivileges == null) return;
            
            int total = _dtPrivileges.Rows.Count;
            // Tính số lượng cho từng loại (khi ở chế độ xem user/role)
            int dtCount = 0, cotCount = 0, sysCount = 0, roleCount = 0;
            
            if (!isObjectMode)
            {
                foreach(DataRow row in _dtPrivileges.Rows)
                {
                    string loai = row["LOAI_QUYEN"]?.ToString() ?? "";
                    if (loai == "COLUMN") cotCount++;
                    else if (loai == "SYSTEM") sysCount++;
                    else if (loai == "ROLE") roleCount++;
                    else dtCount++; // Table, View, Procedure
                }
                lblSummary.Text = $"{targetName}: {dtCount} quyền đối tượng | {cotCount} quyền cột | {sysCount} quyền hệ thống | {roleCount} role (Tổng: {total})";
            }
            else
            {
                lblSummary.Text = $"Đối tượng [HOSPITAL.{targetName}]: Có tổng cộng {total} quyền được cấp phát.";
            }
        }

        private void FitColumns()
        {
            if (dgvPrivilege.Columns.Count == 0 || dgvPrivilege.Rows.Count == 0) return;
            dgvPrivilege.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void ApplyGridStyle(DataGridView dgv)
        {
            dgv.SelectionMode              = DataGridViewSelectionMode.FullRowSelect;
            dgv.ReadOnly                   = true;
            dgv.AllowUserToAddRows         = false;
            dgv.AllowUserToDeleteRows      = false;
            dgv.RowHeadersVisible          = false;
            dgv.MultiSelect                = false;
            dgv.BackgroundColor            = Color.White;
            dgv.BorderStyle                = BorderStyle.None;
            dgv.GridColor                  = Color.FromArgb(220, 220, 220);

            // Bám sát màu sắc trong hình feedback
            dgv.ColumnHeadersDefaultCellStyle.BackColor  = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor  = Color.FromArgb(40, 40, 40);
            dgv.ColumnHeadersDefaultCellStyle.Font       = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment  = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersHeight = 36;
            dgv.EnableHeadersVisualStyles = false;

            dgv.DefaultCellStyle.Font               = new Font("Segoe UI", 9.5f);
            dgv.DefaultCellStyle.SelectionBackColor  = Color.FromArgb(50, 100, 200);
            dgv.DefaultCellStyle.SelectionForeColor  = Color.White;
            dgv.DefaultCellStyle.Padding             = new Padding(4, 0, 4, 0);

            // dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(235, 242, 255);
            dgv.RowTemplate.Height  = 30;
        }

        private static string GetCell(DataGridViewRow row, string colName)
        {
            if (row.DataGridView.Columns.Contains(colName))
                return row.Cells[colName].Value?.ToString() ?? "";
            return "";
        }

        private void RenameColumns_ByGrantee()
        {
            Rename("LOAI_QUYEN",     "Kiểu");
            Rename("QUYEN",          "Quyền");
            Rename("CHU_SO_HUU",     "Schema");
            Rename("DOI_TUONG",      "Đối tượng");
            
            // Ẩn các cột thừa theo yêu cầu
            if (dgvPrivilege.Columns.Contains("COT")) dgvPrivilege.Columns["COT"].Visible = false;
            if (dgvPrivilege.Columns.Contains("CO_THE_CAP_LAI")) dgvPrivilege.Columns["CO_THE_CAP_LAI"].Visible = false;
            if (dgvPrivilege.Columns.Contains("CO_THE_HIERARCHY")) dgvPrivilege.Columns["CO_THE_HIERARCHY"].Visible = false;
        }

        private void RenameColumns_ByObject()
        {
            Rename("NGUOI_NHAN",       "User / Role nhận");
            Rename("CHU_SO_HUU",       "Schema");
            Rename("DOI_TUONG",        "Đối tượng");
            Rename("QUYEN",            "Quyền");
            Rename("LOAI_QUYEN",       "Kiểu");

            // Ẩn các cột thừa theo yêu cầu
            if (dgvPrivilege.Columns.Contains("COT")) dgvPrivilege.Columns["COT"].Visible = false;
            if (dgvPrivilege.Columns.Contains("CO_THE_CAP_LAI")) dgvPrivilege.Columns["CO_THE_CAP_LAI"].Visible = false;
            if (dgvPrivilege.Columns.Contains("CO_THE_HIERARCHY")) dgvPrivilege.Columns["CO_THE_HIERARCHY"].Visible = false;
        }

        private void Rename(string colName, string header)
        {
            if (dgvPrivilege.Columns.Contains(colName))
                dgvPrivilege.Columns[colName].HeaderText = header;
        }

        private void ShowWarn(string msg) => MessageBox.Show(msg, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        private void ShowError(string title, string msg) => MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);

        private void dgvPrivilege_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvPrivilege.Rows[e.RowIndex];
            if (row.Selected) return;

            Color rowColor;

            // Try to get LOAI_QUYEN (User / Role mode)
            if (dgvPrivilege.Columns.Contains("LOAI_QUYEN"))
            {
                string kieuCell = row.Cells["LOAI_QUYEN"].Value?.ToString()?.Trim().ToUpper() ?? "";

                if (kieuCell == "SYSTEM")
                    rowColor = Color.FromArgb(255, 220, 180);
                else if (kieuCell == "ROLE")
                    rowColor = Color.FromArgb(255, 237, 210);
                else if (kieuCell is "TABLE" or "VIEW" or "PROCEDURE" or "FUNCTION")
                    rowColor = Color.FromArgb(255, 248, 235);
                else if (kieuCell == "COLUMN")
                    rowColor = Color.FromArgb(220, 240, 255);
                else
                    rowColor = e.RowIndex % 2 == 0 ? Color.White : Color.FromArgb(245, 245, 245);
            }
            // Đối tượng mode — no LOAI_QUYEN column, use NGUOI_NHAN
            else if (dgvPrivilege.Columns.Contains("NGUOI_NHAN"))
            {
                string nguoiNhan = row.Cells["NGUOI_NHAN"].Value?.ToString() ?? "";

                rowColor = nguoiNhan.StartsWith("RL_", StringComparison.OrdinalIgnoreCase)
                    ? Color.FromArgb(255, 248, 235)   // orange tint — role grantee
                    : Color.FromArgb(235, 248, 255);  // blue tint — user grantee
            }
            else
            {
                rowColor = e.RowIndex % 2 == 0 ? Color.White : Color.FromArgb(245, 245, 245);
            }

            e.CellStyle.BackColor = rowColor;
            e.CellStyle.ForeColor = Color.FromArgb(40, 40, 40);
        }
    }
}
