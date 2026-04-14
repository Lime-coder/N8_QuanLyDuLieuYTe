using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyYTe.DAL;

namespace QuanLyYTe.Forms
{
    /// <summary>
    /// Form quản lý quyền:
    ///   - Câu 4: Thu hồi quyền từ user hoặc role
    ///   - Câu 5: Xem thông tin về quyền của mỗi user/role trên các đối tượng dữ liệu
    /// </summary>
    public partial class FrmPrivilege : Form
    {
        private readonly PrivilegeRepository _repo = new PrivilegeRepository();

        public FrmPrivilege()
        {
            InitializeComponent();
        }

        // ================================================================
        // LOAD FORM
        // ================================================================
        private void FrmPrivilege_Load(object sender, EventArgs e)
        {
            LoadGranteeCombo();
            ApplyGridStyle(dgvPrivilege);

            // Mặc định chọn "Theo User/Role"
            rbByGrantee.Checked = true;
        }

        // ----------------------------------------------------------------
        // Nạp User + Role vào ComboBox
        // ----------------------------------------------------------------
        private void LoadGranteeCombo()
        {
            try
            {
                DataTable dtUsers = _repo.GetUsers();
                DataTable dtRoles = _repo.GetRoles();

                var dt = new DataTable();
                dt.Columns.Add("DISPLAY",  typeof(string));
                dt.Columns.Add("GRANTEE",  typeof(string));

                foreach (DataRow r in dtUsers.Rows)
                {
                    string name = r["USERNAME"].ToString()!;
                    dt.Rows.Add($"[User]  {name}", name);
                }
                foreach (DataRow r in dtRoles.Rows)
                {
                    string name = r["ROLENAME"].ToString()!;
                    dt.Rows.Add($"[Role]  {name}", name);
                }

                cbGrantee.DataSource    = dt;
                cbGrantee.DisplayMember = "DISPLAY";
                cbGrantee.ValueMember   = "GRANTEE";

                if (cbGrantee.Items.Count > 0) cbGrantee.SelectedIndex = 0;
            }
            catch (Exception ex) { ShowError("Lỗi tải danh sách", ex.Message); }
        }

        // ================================================================
        // SỰ KIỆN: RadioButton chuyển chế độ xem
        // ================================================================
        private void rbByGrantee_CheckedChanged(object sender, EventArgs e)
        {
            bool byGrantee = rbByGrantee.Checked;
            pnlGrantee.Visible = byGrantee;
            pnlObject.Visible  = !byGrantee;
            ClearGrid();
        }

        // ================================================================
        // NÚT "XEM QUYỀN"
        // ================================================================
        private void btnView_Click(object sender, EventArgs e)
        {
            if (rbByGrantee.Checked)
                LoadByGrantee();
            else
                LoadByObject();
        }

        private void LoadByGrantee()
        {
            if (cbGrantee.SelectedIndex < 0) { ShowWarn("Vui lòng chọn User / Role."); return; }

            string grantee = cbGrantee.SelectedValue?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(grantee)) { ShowWarn("Grantee không hợp lệ."); return; }

            try
            {
                Cursor = Cursors.WaitCursor;
                DataTable dt = _repo.GetAllPrivileges(grantee);
                BindGrid(dt);
                RenameColumns_ByGrantee();
                lblCount.Text = $"Tổng: {dt.Rows.Count} quyền  |  Grantee: {grantee}";
            }
            catch (Exception ex) { ShowError("Lỗi xem quyền", ex.Message); }
            finally { Cursor = Cursors.Default; }
        }

        private void LoadByObject()
        {
            string owner  = txtOwner.Text.Trim();
            string objName = txtObject.Text.Trim();
            if (string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(objName))
            { ShowWarn("Vui lòng nhập Schema và Tên đối tượng."); return; }

            try
            {
                Cursor = Cursors.WaitCursor;
                DataTable dt = _repo.GetPrivsOnObject(owner, objName);
                BindGrid(dt);
                RenameColumns_ByObject();
                lblCount.Text = $"Tổng: {dt.Rows.Count} bản ghi  |  Đối tượng: {owner}.{objName}";
            }
            catch (Exception ex) { ShowError("Lỗi xem quyền đối tượng", ex.Message); }
            finally { Cursor = Cursors.Default; }
        }

        // ================================================================
        // NÚT "THU HỒI QUYỀN" (Câu 4)
        // ================================================================
        private void btnRevoke_Click(object sender, EventArgs e)
        {
            // Chỉ cho phép thu hồi khi đang xem theo Grantee
            if (rbByObject.Checked)
            {
                ShowWarn("Để thu hồi quyền, hãy chọn chế độ\n\"Theo User / Role\", tải danh sách rồi chọn dòng cần thu hồi.");
                return;
            }

            if (dgvPrivilege.SelectedRows.Count == 0)
            {
                ShowWarn("Vui lòng chọn dòng quyền cần thu hồi.");
                return;
            }

            DataGridViewRow row = dgvPrivilege.SelectedRows[0];

            // Đọc dữ liệu từ dòng đang chọn (dùng tên cột gốc của Oracle)
            string type      = GetCell(row, "LOAI_QUYEN");
            string privilege = GetCell(row, "QUYEN");
            string owner     = GetCell(row, "CHU_SO_HUU");
            string obj       = GetCell(row, "DOI_TUONG");
            string column    = GetCell(row, "COT");
            string grantee   = cbGrantee.SelectedValue?.ToString() ?? "";

            if (string.IsNullOrEmpty(grantee))
            { ShowWarn("Không xác định được Grantee. Vui lòng tải lại danh sách."); return; }

            // Thông tin xác nhận
            string info =
                $"  Grantee   : {grantee}\n" +
                $"  Loại      : {type}\n" +
                $"  Quyền     : {privilege}" +
                (string.IsNullOrEmpty(obj)    ? "" : $"\n  Đối tượng : {owner}.{obj}") +
                (string.IsNullOrEmpty(column) ? "" : $"\n  Cột       : {column}");

            var confirm = MessageBox.Show(
                $"Xác nhận thu hồi quyền sau?\n\n{info}",
                "Xác nhận Thu hồi",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
            if (confirm == DialogResult.No) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                _repo.RevokePrivilege(type, privilege, owner, obj, column, grantee);
                MessageBox.Show("Thu hồi quyền thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadByGrantee(); // reload
            }
            catch (Exception ex) { ShowError("Lỗi thu hồi quyền", ex.Message); }
            finally { Cursor = Cursors.Default; }
        }

        // ================================================================
        // HELPERS
        // ================================================================

        private void BindGrid(DataTable dt)
        {
            dgvPrivilege.DataSource = null;
            dgvPrivilege.DataSource = dt;
            FitColumns();
        }

        private void ClearGrid()
        {
            dgvPrivilege.DataSource = null;
            lblCount.Text = "";
        }

        /// <summary>
        /// Tự động căn cột vừa nội dung nhưng không vượt quá maxWidth,
        /// và tổng chiều rộng không vượt quá DataGridView.
        /// </summary>
        private void FitColumns()
        {
            if (dgvPrivilege.Columns.Count == 0) return;

            // Bước 1: Cho mỗi cột tự co giãn theo nội dung
            dgvPrivilege.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // Bước 2: Lấy chiều rộng theo AllCells, gán cố định, rồi tắt AutoSize
            int totalWidth = 0;
            foreach (DataGridViewColumn col in dgvPrivilege.Columns)
            {
                int w = col.Width;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                col.Width = w;
                totalWidth += w;
            }

            dgvPrivilege.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // Bước 3: Nếu tổng cột < chiều rộng grid → dùng Fill để căn khít
            int gridWidth = dgvPrivilege.ClientSize.Width;
            if (totalWidth < gridWidth - 20)
            {
                dgvPrivilege.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            // Nếu tổng cột > grid → giữ nguyên kích thước AllCells (có scrollbar ngang)
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

            dgv.ColumnHeadersDefaultCellStyle.BackColor  = Color.FromArgb(30, 100, 160);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor  = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font       = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment  = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersHeight = 34;

            dgv.DefaultCellStyle.Font               = new Font("Segoe UI", 9.5f);
            dgv.DefaultCellStyle.SelectionBackColor  = Color.FromArgb(52, 152, 219);
            dgv.DefaultCellStyle.SelectionForeColor  = Color.White;
            dgv.DefaultCellStyle.Padding             = new Padding(4, 0, 4, 0);

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 252);
            dgv.RowTemplate.Height  = 28;
            dgv.EnableHeadersVisualStyles = false;
        }

        private static string GetCell(DataGridViewRow row, string colName)
        {
            if (row.DataGridView.Columns.Contains(colName))
                return row.Cells[colName].Value?.ToString() ?? "";
            return "";
        }

        private void RenameColumns_ByGrantee()
        {
            Rename("LOAI_QUYEN",     "Loại quyền");
            Rename("QUYEN",          "Quyền / Role được cấp");
            Rename("CHU_SO_HUU",     "Schema");
            Rename("DOI_TUONG",      "Đối tượng");
            Rename("COT",            "Cột");
            Rename("CO_THE_CAP_LAI", "Cấp lại?");
        }

        private void RenameColumns_ByObject()
        {
            Rename("NGUOI_NHAN",       "User / Role nhận");
            Rename("CHU_SO_HUU",       "Schema");
            Rename("DOI_TUONG",        "Đối tượng");
            Rename("COT",              "Cột");
            Rename("QUYEN",            "Quyền");
            Rename("CO_THE_CAP_LAI",   "Cấp lại?");
            Rename("CO_THE_HIERARCHY", "Hierarchy?");
        }

        private void Rename(string colName, string header)
        {
            if (dgvPrivilege.Columns.Contains(colName))
                dgvPrivilege.Columns[colName].HeaderText = header;
        }

        private void ShowWarn(string msg)
            => MessageBox.Show(msg, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private void ShowError(string title, string msg)
            => MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
