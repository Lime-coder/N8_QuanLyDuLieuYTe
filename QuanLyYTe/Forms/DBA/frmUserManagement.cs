using System.Data;
using System.Text.RegularExpressions;
using QuanLyYTe.Forms.DBA;
using QuanLyYTe.Services;
using QuanLyYTe.Helpers;

namespace QuanLyYTe.Forms
{
    public partial class frmUserManagement : Form
    {
        private readonly SecurityAdminService _service = new SecurityAdminService();
        private DataTable? _usersDt;
        private DataTable? _rolesDt;
        private Font? _statusFont;

        public frmUserManagement()
        {
            InitializeComponent();
            ApplyButtonStyles();
            dgvUsers.CellFormatting += dgvUsers_CellFormatting;
            _statusFont = new Font(dgvUsers.Font, FontStyle.Bold);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _statusFont?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void SecurityAdminForm_Load(object sender, EventArgs e)
        {
            RefreshUsers();
            RefreshRoles();
        }

        private void ApplyUsersGridColumnHeaders()
        {
            void Set(string columnName, string headerText)
            {
                if (dgvUsers.Columns.Contains(columnName))
                    dgvUsers.Columns[columnName].HeaderText = headerText;
            }

            Set("USERNAME", "Tên user");
            Set("ACCOUNT_STATUS", "Trạng thái hoạt động");
            Set("ORACLE_MAINTAINED", "Hệ thống");
            Set("LOCK_DATE", "Ngày khóa");
            Set("CREATED", "Ngày tạo");
        }

        private void ApplyRolesGridColumnHeaders()
        {
            void Set(string columnName, string headerText)
            {
                if (dgvRoles.Columns.Contains(columnName))
                    dgvRoles.Columns[columnName].HeaderText = headerText;
            }

            Set("ROLE", "Tên role");
            Set("PASSWORD_REQUIRED", "Yêu cầu mật khẩu");
            Set("AUTHENTICATION_TYPE", "Kiểu xác thực");
            Set("COMMON", "Chung (CDB)");
            Set("ORACLE_MAINTAINED", "Hệ thống");
        }

        private void ApplyButtonStyles()
        {
            void StylePrimary(Button b)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.BackColor = Color.FromArgb(33, 150, 243);
                b.ForeColor = Color.White;
            }

            void StyleWarning(Button b)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.BackColor = Color.FromArgb(255, 193, 7);
                b.ForeColor = Color.Black;
            }

            void StyleSuccess(Button b)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.BackColor = Color.FromArgb(76, 175, 80);
                b.ForeColor = Color.White;
            }

            void StyleDanger(Button b)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.BackColor = Color.FromArgb(244, 67, 54);
                b.ForeColor = Color.White;
            }

            void StyleNeutral(Button b)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.BackColor = Color.FromArgb(96, 125, 139);
                b.ForeColor = Color.White;
            }

            void StyleDefault(Button b)
            {
                b.FlatStyle = FlatStyle.Standard;
                b.UseVisualStyleBackColor = true;
            }

            StyleNeutral(btnUserRefresh);
            StylePrimary(btnUserCreate);
            StyleWarning(btnUserEdit);
            StyleDanger(btnUserDelete);
            StyleDefault(btnUserLock);
            StyleSuccess(btnUserUnlock);

            StyleNeutral(btnRoleRefresh);
            StylePrimary(btnRoleCreate);
            StyleWarning(btnRoleEdit);
            StyleDanger(btnRoleDelete);
        }

        private void dgvUsers_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvUsers.Columns.Count == 0) return;
            if (!dgvUsers.Columns.Contains("ACCOUNT_STATUS")) return;

            int colIdx = dgvUsers.Columns["ACCOUNT_STATUS"].Index;
            if (e.ColumnIndex != colIdx) return;

            string status = Convert.ToString(e.Value) ?? string.Empty;
            string upper = status.ToUpperInvariant();

            if (_statusFont != null)
                e.CellStyle.Font = _statusFont;

            if (upper.Contains("OPEN"))
            {
                e.Value = "ACTIVE";
                e.CellStyle.ForeColor = Color.Green;
                e.CellStyle.SelectionForeColor = Color.LightGreen;
            }
            else if (upper.Contains("LOCKED"))
            {
                e.Value = "LOCK";
                e.CellStyle.ForeColor = Color.Red;
                e.CellStyle.SelectionForeColor = Color.Salmon;
            }
            else
            {
                e.Value = "INACTIVE";
                e.CellStyle.ForeColor = Color.Gray;
                e.CellStyle.SelectionForeColor = Color.LightGray;
            }
        }

        private void RefreshUsers()
        {
            try
            {
                _usersDt = _service.GetAllUsers();
                _usersDt.CaseSensitive = false;
                dgvUsers.DataSource = _usersDt;
                GridViewStyler.Format(dgvUsers);
                ApplyUsersGridColumnHeaders();

                if (_usersDt.Columns.Count == 0)
                {
                    MessageBox.Show(
                        "Không lấy được dữ liệu Users (DataTable không có cột).\n" +
                        "Khả năng cao: Stored Procedure chưa tạo/chưa đúng schema, hoặc thiếu quyền SELECT trên DBA_USERS.",
                        "Chẩn đoán");
                }

                ApplyUserFilter();
                UpdateUserButtonsState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tải danh sách user");
            }
        }

        private void RefreshRoles()
        {
            try
            {
                _rolesDt = _service.GetAllRoles();
                _rolesDt.CaseSensitive = false;
                dgvRoles.DataSource = _rolesDt;
                GridViewStyler.Format(dgvRoles);
                ApplyRolesGridColumnHeaders();

                if (_rolesDt.Columns.Count == 0)
                {
                    MessageBox.Show(
                        "Không lấy được dữ liệu Roles (DataTable không có cột).\n" +
                        "Khả năng cao: Stored Procedure chưa tạo/chưa đúng schema, hoặc thiếu quyền SELECT trên DBA_ROLES.",
                        "Chẩn đoán");
                }

                ApplyRoleFilter();
                UpdateRoleButtonsState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tải danh sách role");
            }
        }

        private static string EscapeForRowFilterLike(string s)
        {
            return s
                .Replace("'", "''")
                .Replace("[", "[[]")
                .Replace("]", "[]]")
                .Replace("%", "[%]")
                .Replace("*", "[*]");
        }

        private void ApplyUserFilter()
        {
            if (_usersDt == null) return;
            string term = (txtUserSearch?.Text ?? string.Empty).Trim();

            var view = _usersDt.DefaultView;
            if (string.IsNullOrWhiteSpace(term) || !_usersDt.Columns.Contains("USERNAME"))
            {
                view.RowFilter = string.Empty;
                return;
            }

            string safe = EscapeForRowFilterLike(term);
            view.RowFilter = $"CONVERT(USERNAME, 'System.String') LIKE '%{safe}%'";
        }

        private void ApplyRoleFilter()
        {
            if (_rolesDt == null) return;
            string term = (txtRoleSearch?.Text ?? string.Empty).Trim();

            var view = _rolesDt.DefaultView;
            if (string.IsNullOrWhiteSpace(term) || !_rolesDt.Columns.Contains("ROLE"))
            {
                view.RowFilter = string.Empty;
                return;
            }

            string safe = EscapeForRowFilterLike(term);
            view.RowFilter = $"CONVERT(ROLE, 'System.String') LIKE '%{safe}%'";
        }

        private static bool IsValidOracleIdentifier(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            s = s.Trim();
            return Regex.IsMatch(s, @"^[A-Za-z][A-Za-z0-9_$#]{0,29}$");
        }

        private string? GetSelectedUsername()
        {
            if (dgvUsers.CurrentRow == null) return null;
            if (!dgvUsers.Columns.Contains("USERNAME")) return null;
            return Convert.ToString(dgvUsers.CurrentRow.Cells["USERNAME"].Value);
        }

        private string? GetSelectedUserStatus()
        {
            if (dgvUsers.CurrentRow == null) return null;
            if (!dgvUsers.Columns.Contains("ACCOUNT_STATUS")) return null;
            return Convert.ToString(dgvUsers.CurrentRow.Cells["ACCOUNT_STATUS"].Value);
        }

        private string? GetSelectedUserOracleMaintained()
        {
            if (dgvUsers.CurrentRow == null) return null;
            if (!dgvUsers.Columns.Contains("ORACLE_MAINTAINED")) return null;
            return Convert.ToString(dgvUsers.CurrentRow.Cells["ORACLE_MAINTAINED"].Value);
        }

        private string? GetSelectedRoleName()
        {
            if (dgvRoles.CurrentRow == null) return null;
            if (!dgvRoles.Columns.Contains("ROLE")) return null;
            return Convert.ToString(dgvRoles.CurrentRow.Cells["ROLE"].Value);
        }

        private string? GetSelectedRoleOracleMaintained()
        {
            if (dgvRoles.CurrentRow == null) return null;
            if (!dgvRoles.Columns.Contains("ORACLE_MAINTAINED")) return null;
            return Convert.ToString(dgvRoles.CurrentRow.Cells["ORACLE_MAINTAINED"].Value);
        }

        private void UpdateUserButtonsState()
        {
            bool hasRow = dgvUsers.CurrentRow != null;
            btnUserEdit.Enabled = hasRow;
            btnUserDelete.Enabled = hasRow;
            btnUserLock.Enabled = hasRow;
            btnUserUnlock.Enabled = hasRow;

            if (!hasRow)
            {
                return;
            }

            bool isSystemUser = string.Equals(GetSelectedUserOracleMaintained(), "Y", StringComparison.OrdinalIgnoreCase);
            if (isSystemUser)
            {
                btnUserEdit.Enabled = false;
                btnUserDelete.Enabled = false;
                btnUserLock.Enabled = false;
                btnUserUnlock.Enabled = false;
                return;
            }

            string? status = GetSelectedUserStatus();
            bool locked = !string.IsNullOrWhiteSpace(status) && status.ToUpperInvariant().Contains("LOCKED");
            btnUserLock.Enabled = !locked;
            btnUserUnlock.Enabled = locked;
        }

        private void UpdateRoleButtonsState()
        {
            bool hasRow = dgvRoles.CurrentRow != null;
            btnRoleEdit.Enabled = hasRow;
            btnRoleDelete.Enabled = hasRow;

            if (!hasRow) return;
            bool isSystemRole = string.Equals(GetSelectedRoleOracleMaintained(), "Y", StringComparison.OrdinalIgnoreCase);
            if (isSystemRole)
            {
                btnRoleEdit.Enabled = false;
                btnRoleDelete.Enabled = false;
            }
        }

        private void btnUserRefresh_Click(object sender, EventArgs e) => RefreshUsers();
        private void btnRoleRefresh_Click(object sender, EventArgs e) => RefreshRoles();

        private void dgvUsers_SelectionChanged(object sender, EventArgs e) => UpdateUserButtonsState();
        private void dgvRoles_SelectionChanged(object sender, EventArgs e) => UpdateRoleButtonsState();

        private void txtUserSearch_TextChanged(object sender, EventArgs e) => ApplyUserFilter();
        private void txtRoleSearch_TextChanged(object sender, EventArgs e) => ApplyRoleFilter();

        private void dgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            btnUserEdit.PerformClick();
        }

        private void dgvRoles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            btnRoleEdit.PerformClick();
        }

        private void btnUserCreate_Click(object sender, EventArgs e)
        {
            using var selectionDlg = new Form { 
                Text = "Chọn loại người dùng", 
                ClientSize = new Size(320, 120), 
                StartPosition = FormStartPosition.CenterParent, 
                FormBorderStyle = FormBorderStyle.FixedDialog, 
                MaximizeBox = false, 
                MinimizeBox = false 
            };
            
            var lbl = new Label { 
                Text = "Bạn muốn tạo loại người dùng nào?", 
                AutoSize = false, 
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(320, 30), 
                Location = new Point(0, 15) 
            };
            var btnStaff = new Button { Text = "Nhân viên", Location = new Point(45, 60), Size = new Size(105, 35), DialogResult = DialogResult.Yes };
            var btnPatient = new Button { Text = "Bệnh nhân", Location = new Point(170, 60), Size = new Size(105, 35), DialogResult = DialogResult.No };
            selectionDlg.Controls.AddRange(new Control[] { lbl, btnStaff, btnPatient });
            
            var result = selectionDlg.ShowDialog(this);
            if (result != DialogResult.Yes && result != DialogResult.No) return;
            
            bool isPatient = result == DialogResult.No;

            using var dlg = new frmEditUser(mode: EditUserDialogMode.Create, isPatient: isPatient);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                _service.CreateUser(dlg.Username, dlg.Password, dlg.FullName, dlg.Gender, dlg.Birthdate, dlg.IdCard, dlg.Role, 
                    dlg.Phone, dlg.Hometown, dlg.DeptId, dlg.Facility,
                    dlg.HouseNo, dlg.Street, dlg.District, dlg.CityProvince, 
                    dlg.MedicalHistory, dlg.FamilyMedicalHistory, dlg.DrugAllergies);
                MessageBox.Show("Tạo user thành công.", "Thông báo");
                RefreshUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void btnUserEdit_Click(object sender, EventArgs e)
        {
            if (string.Equals(GetSelectedUserOracleMaintained(), "Y", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, "Đây là User hệ thống, bạn không có quyền thay đổi.", "Thông báo");
                return;
            }

            string? username = GetSelectedUsername();
            if (string.IsNullOrWhiteSpace(username)) return;

            DataTable dtInfo;
            try { dtInfo = _service.GetUserInfo(username); }
            catch (Exception ex) { MessageBox.Show("Lỗi lấy thông tin user: " + ex.Message); return; }

            if (dtInfo.Rows.Count == 0) { MessageBox.Show("Không tìm thấy thông tin user trong các bảng dữ liệu."); return; }
            
            string role = dtInfo.Rows[0]["ROLE"]?.ToString() ?? "";
            bool isPatient = role == "RL_PATIENT";

            using var dlg = new frmEditUser(mode: EditUserDialogMode.Edit, isPatient: isPatient, presetUsername: username);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                _service.UpdateUser(username, dlg.FullName, dlg.Gender, dlg.Birthdate, dlg.IdCard, dlg.Role, 
                    dlg.Phone, dlg.Hometown, dlg.DeptId, dlg.Facility,
                    dlg.HouseNo, dlg.Street, dlg.District, dlg.CityProvince, 
                    dlg.MedicalHistory, dlg.FamilyMedicalHistory, dlg.DrugAllergies);
                
                // Reset password if provided
                if (!string.IsNullOrEmpty(dlg.Password))
                {
                    _service.ChangeUserPassword(username, dlg.Password);
                }

                MessageBox.Show("Cập nhật user thành công.", "Thông báo");
                RefreshUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void btnUserDelete_Click(object sender, EventArgs e)
        {
            if (string.Equals(GetSelectedUserOracleMaintained(), "Y", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, "Đây là User hệ thống, bạn không có quyền thay đổi.", "Thông báo");
                return;
            }

            string? username = GetSelectedUsername();
            if (string.IsNullOrWhiteSpace(username)) return;

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn vô hiệu hóa user `{username}`?\n\n" +
                "Tài khoản sẽ bị khóa và đánh dấu không hoạt động.\n" +
                "Dữ liệu y tế lịch sử sẽ được bảo toàn.",
                "Xác nhận vô hiệu hóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _service.DeactivateUser(username);
                MessageBox.Show("Vô hiệu hóa user thành công.", "Thông báo");
                RefreshUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void btnUserLock_Click(object sender, EventArgs e)
        {
            if (string.Equals(GetSelectedUserOracleMaintained(), "Y", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, "Đây là User hệ thống, bạn không có quyền thay đổi.", "Thông báo");
                return;
            }

            string? username = GetSelectedUsername();
            if (string.IsNullOrWhiteSpace(username)) return;

            var confirm = MessageBox.Show(
                this,
                $"Bạn có chắc muốn khóa user `{username}`?",
                "Xác nhận khóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _service.LockUser(username);
                RefreshUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void btnUserUnlock_Click(object sender, EventArgs e)
        {
            if (string.Equals(GetSelectedUserOracleMaintained(), "Y", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, "Đây là User hệ thống, bạn không có quyền thay đổi.", "Thông báo");
                return;
            }

            string? username = GetSelectedUsername();
            if (string.IsNullOrWhiteSpace(username)) return;

            var confirm = MessageBox.Show(
                this,
                $"Bạn có chắc muốn mở khóa user `{username}`?",
                "Xác nhận mở khóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _service.UnlockUser(username);
                RefreshUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void btnRoleCreate_Click(object sender, EventArgs e)
        {
            using var dlg = new frmEditRole(mode: EditRoleDialogMode.Create);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            string roleName = dlg.RoleName.Trim().ToUpperInvariant();
            if (!IsValidOracleIdentifier(roleName))
            {
                MessageBox.Show("Tên role không hợp lệ. Chỉ dùng chữ/số/_/$/#, bắt đầu bằng chữ, tối đa 30 ký tự.", "Kiểm tra");
                return;
            }

            try
            {
                _service.CreateRole(roleName, dlg.PasswordOrNullForNotIdentified);
                MessageBox.Show("Tạo role thành công.", "Thông báo");
                RefreshRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void btnRoleEdit_Click(object sender, EventArgs e)
        {
            if (string.Equals(GetSelectedRoleOracleMaintained(), "Y", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, "Đây là Role hệ thống, bạn không có quyền thay đổi.", "Thông báo");
                return;
            }

            string? roleName = GetSelectedRoleName();
            if (string.IsNullOrWhiteSpace(roleName)) return;

            using var dlg = new frmEditRole(mode: EditRoleDialogMode.Edit, presetRoleName: roleName);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                _service.ChangeRolePassword(roleName, dlg.PasswordOrNullForNotIdentified);
                MessageBox.Show("Cập nhật role thành công.", "Thông báo");
                RefreshRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void btnRoleDelete_Click(object sender, EventArgs e)
        {
            if (string.Equals(GetSelectedRoleOracleMaintained(), "Y", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, "Đây là Role hệ thống, bạn không có quyền thay đổi.", "Thông báo");
                return;
            }

            string? roleName = GetSelectedRoleName();
            if (string.IsNullOrWhiteSpace(roleName)) return;

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa role `{roleName}`?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _service.DropRole(roleName);
                MessageBox.Show("Xóa role thành công.", "Thông báo");
                RefreshRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }
    }
}
