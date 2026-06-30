using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyYTe.Services;

namespace QuanLyYTe.Forms.Technician
{
    public partial class frmTechnician : Form
    {
        private readonly TechnicianService _service = new TechnicianService();
        private DateTime _currentServiceDate;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool HideCaret(IntPtr hWnd);

        public frmTechnician()
        {
            InitializeComponent();
            AttachReadOnlyFocusHandler(this);
            this.Load += FrmTechnician_Load;
        }

        private void AttachReadOnlyFocusHandler(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox tb)
                {
                    tb.Enter += (s, e) =>
                    {
                        if (tb.ReadOnly)
                        {
                            tb.Parent?.Focus();
                            HideCaret(tb.Handle);
                        }
                    };
                    tb.MouseDown += (s, e) =>
                    {
                        if (tb.ReadOnly)
                            HideCaret(tb.Handle);
                    };
                }
                
                if (c.HasChildren)
                {
                    AttachReadOnlyFocusHandler(c);
                }
            }
        }

        private void FrmTechnician_Load(object? sender, EventArgs e)
        {
            dgvServices.Columns.Clear();
            dgvServices.AutoGenerateColumns = true;
            txtSearchRecord.TextChanged += (s, ev) => FilterServices();
            LoadServices();
            try { LoadPersonalInfo(); } catch { /* ignore if not available */ }

            // Add OLS Notification Tab
            TabPage tabOls = new TabPage("Thông báo OLS");
            QuanLyYTe.Forms.Common.frmNotifications frm = new QuanLyYTe.Forms.Common.frmNotifications();
            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            tabOls.Controls.Add(frm);
            tabControlMain.TabPages.Add(tabOls);
            frm.Show();
        }

        // ─────────────────────────────────────────────────────────────
        //  TAB: DỊCH VỤ CỦA TÔI
        // ─────────────────────────────────────────────────────────────

        private void LoadServices()
        {
            try
            {
                int selectedIndex = dgvServices.CurrentRow?.Index ?? -1;
                int scrollIndex = dgvServices.FirstDisplayedScrollingRowIndex;

                DataTable dt = _service.LoadAssignedServices();
                dgvServices.AutoGenerateColumns = true;
                dgvServices.DataSource = dt;
                
                // Re-apply filter if user is searching
                FilterServices();
                
                FormatServiceGrid();

                // Restore previous selection
                if (selectedIndex >= 0 && selectedIndex < dgvServices.Rows.Count)
                {
                    dgvServices.ClearSelection();
                    dgvServices.CurrentCell = dgvServices.Rows[selectedIndex].Cells[0];
                    dgvServices.Rows[selectedIndex].Selected = true;
                    
                    if (scrollIndex >= 0 && scrollIndex < dgvServices.Rows.Count)
                    {
                        dgvServices.FirstDisplayedScrollingRowIndex = scrollIndex;
                    }
                }

                UpdateDetailPanelFromSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterServices()
        {
            try
            {
                if (dgvServices.DataSource is DataTable dt)
                {
                    string kw = txtSearchRecord.Text?.Trim() ?? string.Empty;
                    if (string.IsNullOrEmpty(kw))
                    {
                        dt.DefaultView.RowFilter = string.Empty;
                    }
                    else
                    {
                        string safeKw = kw.Replace("'", "''");
                        // DataView LIKE is case-insensitive.
                        dt.DefaultView.RowFilter = $"RECORD_ID LIKE '%{safeKw}%' OR SERVICE_TYPE LIKE '%{safeKw}%'";
                    }
                    UpdateDetailPanelFromSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lọc dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FilterServices();
        }

        private void txtSearchRecord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Ngăn tiếng "bíp" khi nhấn Enter
                btnSearch_Click(sender, e);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearchRecord.Clear();
            LoadServices();
        }

        private void dgvServices_SelectionChanged(object? sender, EventArgs e)
        {
            UpdateDetailPanelFromSelection();
        }

        private void UpdateDetailPanelFromSelection()
        {
            try
            {
                if (dgvServices.CurrentRow == null)
                {
                    txtDetailRecordId.Text = string.Empty;
                    txtDetailServiceType.Text = string.Empty;
                    txtDetailServiceDate.Text = string.Empty;
                    txtCurrentResult.Text = string.Empty;
                    return;
                }

                DataGridViewRow row = dgvServices.CurrentRow;
                txtDetailRecordId.Text = GetCellText(row, "RECORD_ID", "record_id");
                txtDetailServiceType.Text = GetCellText(row, "SERVICE_TYPE", "service_type");

                string rawDate = GetCellText(row, "SERVICE_DATE", "service_date");
                if (row.DataGridView != null && row.DataGridView.Columns.Contains("SERVICE_DATE") && row.Cells["SERVICE_DATE"].Value is DateTime dtVal)
                {
                    _currentServiceDate = dtVal;
                    txtDetailServiceDate.Text = dtVal.ToString("dd/MM/yyyy");
                }
                else if (DateTime.TryParse(rawDate, out DateTime parsedDate))
                {
                    _currentServiceDate = parsedDate;
                    txtDetailServiceDate.Text = parsedDate.ToString("dd/MM/yyyy");
                }
                else
                {
                    _currentServiceDate = DateTime.MinValue;
                    txtDetailServiceDate.Text = rawDate;
                }

                txtCurrentResult.Text = GetCellText(row, "SERVICE_RESULT", "service_result", "kết_quả");

                // Reset inline edit state
                ResetResultEditState();
            }
            catch { /* ignore selection sync issues */ }
        }

        private void ResetResultEditState()
        {
            txtCurrentResult.ReadOnly = true;
            txtCurrentResult.BackColor = Color.FromArgb(229, 231, 235);
            btnUpdateResult.Visible = true;
            btnSaveResult.Visible = false;
            btnCancelResult.Visible = false;
        }

        private void btnUpdateResult_Click(object sender, EventArgs e)
        {
            if (dgvServices.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng để cập nhật kết quả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = dgvServices.SelectedRows[0];
            string recordId   = GetRowText(row, "RECORD_ID",    "record_id");
            string serviceType = GetRowText(row, "SERVICE_TYPE", "service_type");

            if (string.IsNullOrEmpty(recordId) || string.IsNullOrEmpty(serviceType))
            {
                MessageBox.Show("Dữ liệu dịch vụ không hợp lệ. Vui lòng chọn một dòng hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_currentServiceDate == DateTime.MinValue)
            {
                MessageBox.Show("Không thể lấy ngày dịch vụ gốc hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Enable inline editing
            txtCurrentResult.ReadOnly = false;
            txtCurrentResult.BackColor = Color.White;
            btnUpdateResult.Visible = false;
            btnSaveResult.Visible = true;
            btnCancelResult.Visible = true;
            txtCurrentResult.Focus();
        }

        private void btnCancelResult_Click(object sender, EventArgs e)
        {
            UpdateDetailPanelFromSelection(); // Reloads the original text and resets state
        }

        private void btnSaveResult_Click(object? sender, EventArgs e)
        {
            string recordId = txtDetailRecordId.Text.Trim();
            string serviceType = txtDetailServiceType.Text.Trim();
            
            if (string.IsNullOrEmpty(recordId)) return;

            try
            {
                _service.SaveServiceResult(recordId, serviceType, _currentServiceDate, txtCurrentResult.Text.Trim());
                MessageBox.Show("Cập nhật kết quả thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                ResetResultEditState();
                LoadServices();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatServiceGrid()
        {
            try
            {
                if (dgvServices.DataSource == null) return;

                void HideIfExists(string col)
                {
                    if (dgvServices.Columns.Contains(col))
                        dgvServices.Columns[col].Visible = false;
                }

                // Hide technician/status columns — not shown to the KTV
                HideIfExists("TECHNICIAN_ID");
                HideIfExists("KTV_ID");
                HideIfExists("MA_KTV");
                HideIfExists("TECH_ID");
                HideIfExists("STATUS");
                HideIfExists("SERVICE_STATUS");
                HideIfExists("TRANG_THAI");

                // Rename visible columns
                if (dgvServices.Columns.Contains("RECORD_ID"))
                {
                    dgvServices.Columns["RECORD_ID"].HeaderText    = "Mã HSBA";
                    dgvServices.Columns["RECORD_ID"].ReadOnly      = true;
                    dgvServices.Columns["RECORD_ID"].DisplayIndex  = 0;
                    dgvServices.Columns["RECORD_ID"].Width         = 110;
                    dgvServices.Columns["RECORD_ID"].AutoSizeMode  = DataGridViewAutoSizeColumnMode.None;
                }
                if (dgvServices.Columns.Contains("SERVICE_TYPE"))
                {
                    dgvServices.Columns["SERVICE_TYPE"].HeaderText   = "Loại dịch vụ";
                    dgvServices.Columns["SERVICE_TYPE"].DisplayIndex = 1;
                }
                if (dgvServices.Columns.Contains("SERVICE_DATE"))
                {
                    dgvServices.Columns["SERVICE_DATE"].HeaderText              = "Ngày thực hiện";
                    dgvServices.Columns["SERVICE_DATE"].DefaultCellStyle.Format = "yyyy-MM-dd";
                    dgvServices.Columns["SERVICE_DATE"].DisplayIndex            = 2;
                    dgvServices.Columns["SERVICE_DATE"].Width                   = 145;
                    dgvServices.Columns["SERVICE_DATE"].AutoSizeMode            = DataGridViewAutoSizeColumnMode.None;
                }
                if (dgvServices.Columns.Contains("SERVICE_RESULT"))
                {
                    dgvServices.Columns["SERVICE_RESULT"].HeaderText    = "Kết quả";
                    dgvServices.Columns["SERVICE_RESULT"].DisplayIndex  = 3;
                    dgvServices.Columns["SERVICE_RESULT"].AutoSizeMode  = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch { /* ignore formatting errors */ }
        }

        // ─────────────────────────────────────────────────────────────
        //  TAB: THÔNG TIN CỦA TÔI
        // ─────────────────────────────────────────────────────────────

        private void LoadPersonalInfo()
        {
            try
            {
                // Fallback / Initial load from AppSession
                if (!string.IsNullOrEmpty(QuanLyYTe.Common.AppSession.CurrentUserId))
                    txtUserId.Text = QuanLyYTe.Common.AppSession.CurrentUserId;
                if (!string.IsNullOrEmpty(QuanLyYTe.Common.AppSession.CurrentFullName))
                    txtFullName.Text = QuanLyYTe.Common.AppSession.CurrentFullName;

                DataTable dt = _service.LoadPersonalInfo();
                if (dt.Rows.Count == 0) return;
                var r = dt.Rows[0];

                txtUserId.Text   = r.Table.Columns.Contains("STAFF_ID")   ? r["STAFF_ID"]?.ToString()   : txtUserId.Text;
                txtFullName.Text = r.Table.Columns.Contains("FULL_NAME")  ? r["FULL_NAME"]?.ToString()  : txtFullName.Text;
                txtGender.Text   = r.Table.Columns.Contains("GENDER")     ? r["GENDER"]?.ToString()     : string.Empty;
                if (r.Table.Columns.Contains("BIRTHDATE") && DateTime.TryParse(r["BIRTHDATE"]?.ToString(), out DateTime bd))
                    dtpBirthdate.Value = bd;
                txtPhone.Text    = r.Table.Columns.Contains("PHONE")      ? r["PHONE"]?.ToString()      : string.Empty;
                txtHometown.Text = r.Table.Columns.Contains("HOMETOWN")   ? r["HOMETOWN"]?.ToString()   : string.Empty;
                txtDept.Text     = r.Table.Columns.Contains("DEPT_NAME")  ? r["DEPT_NAME"]?.ToString()  : string.Empty;
                txtFacility.Text = r.Table.Columns.Contains("FACILITY")   ? r["FACILITY"]?.ToString()   : string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin cá nhân: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool _isEditing = false;

        private void btnSaveInfo_Click(object sender, EventArgs e)
        {
            if (!_isEditing)
            {
                // First click: enter edit mode
                _isEditing = true;
                txtPhone.ReadOnly     = false;
                txtHometown.ReadOnly  = false;
                txtPhone.BackColor    = Color.White;
                txtHometown.BackColor = Color.White;
                btnSaveInfo.Text      = "Lưu";
                btnSaveInfo.BackColor = Color.FromArgb(22, 163, 74);
                btnCancelEdit.Enabled = true;
            }
            else
            {
                // Second click: save changes
                try
                {
                    _service.UpdatePersonalInfo(txtPhone.Text, txtHometown.Text);
                    MessageBox.Show("Cập nhật thông tin cá nhân thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ExitEditMode();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu thông tin: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelEdit_Click(object sender, EventArgs e) => ExitEditMode();

        private void ExitEditMode()
        {
            _isEditing = false;
            LoadPersonalInfo();
            txtPhone.ReadOnly     = true;
            txtHometown.ReadOnly  = true;
            txtPhone.BackColor    = Color.FromArgb(229, 231, 235);
            txtHometown.BackColor = Color.FromArgb(229, 231, 235);
            btnSaveInfo.Text      = "Cập nhật";
            btnSaveInfo.BackColor = Color.FromArgb(255, 140, 40);
            btnCancelEdit.Enabled = false;
        }

        private void btnEditInfo_Click(object sender, EventArgs e) { /* unused – kept for Designer compat */ }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất?", "Đăng xuất",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                new Services.AuthService().Logout();
                Application.Restart();
                Environment.Exit(0);
            }
        }

        // ─────────────────────────────────────────────────────────────
        //  HELPERS
        // ─────────────────────────────────────────────────────────────

        private static string GetCellText(DataGridViewRow row, params string[] columnNames)
        {
            foreach (string col in columnNames)
            {
                if (row.DataGridView != null && row.DataGridView.Columns.Contains(col))
                {
                    object? val = row.Cells[row.DataGridView.Columns[col].Index].Value;
                    if (val != null && val != DBNull.Value)
                        return val.ToString() ?? string.Empty;
                }
            }
            return string.Empty;
        }

        private static string GetRowText(DataGridViewRow row, params string[] columnNames)
            => GetCellText(row, columnNames);
    }

}
