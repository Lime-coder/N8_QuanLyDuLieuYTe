using System.Data;
using System.Text.RegularExpressions;
using QuanLyYTe.DAL;

namespace QuanLyYTe.Forms
{
    public partial class SecurityAdminForm : Form
    {
        private readonly SecurityAdminRepository _repo = new SecurityAdminRepository();
        private DataTable? _usersDt;
        private DataTable? _rolesDt;

        public SecurityAdminForm()
        {
            InitializeComponent();
            ApplyButtonStyles();
            dgvUsers.CellFormatting += dgvUsers_CellFormatting;
        }

        private void SecurityAdminForm_Load(object sender, EventArgs e)
        {
            RefreshUsers();
            RefreshRoles();
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

            if (upper.Contains("LOCKED"))
            {
                e.CellStyle.ForeColor = Color.FromArgb(183, 28, 28);
                e.CellStyle.SelectionForeColor = Color.White;
                e.CellStyle.SelectionBackColor = Color.FromArgb(244, 67, 54);
            }
            else if (upper.Contains("OPEN"))
            {
                e.CellStyle.ForeColor = Color.FromArgb(27, 94, 32);
                e.CellStyle.SelectionForeColor = Color.White;
                e.CellStyle.SelectionBackColor = Color.FromArgb(76, 175, 80);
            }
        }

        private void RefreshUsers()
        {
            try
            {
                _usersDt = _repo.GetAllUsers();
                _usersDt.CaseSensitive = false;
                dgvUsers.DataSource = _usersDt;
                OracleHelper.FormatGridView(dgvUsers);

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
                MessageBox.Show(ex.Message, "Lỗi load Users");
            }
        }

        private void RefreshRoles()
        {
            try
            {
                _rolesDt = _repo.GetAllRoles();
                _rolesDt.CaseSensitive = false;
                dgvRoles.DataSource = _rolesDt;
                OracleHelper.FormatGridView(dgvRoles);

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
                MessageBox.Show(ex.Message, "Lỗi load Roles");
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
            using var dlg = new EditUserDialog(mode: EditUserDialogMode.Create);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            string username = dlg.Username.Trim().ToUpperInvariant();
            string password = dlg.Password;

            if (!IsValidOracleIdentifier(username))
            {
                MessageBox.Show("Username không hợp lệ. Chỉ dùng chữ/số/_/$/#, bắt đầu bằng chữ, tối đa 30 ký tự.", "Validation");
                return;
            }

            try
            {
                _repo.CreateUser(username, password);
                MessageBox.Show("Tạo user thành công.", "OK");
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

            using var dlg = new EditUserDialog(mode: EditUserDialogMode.EditPassword, presetUsername: username);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                _repo.ChangeUserPassword(username, dlg.Password);
                MessageBox.Show("Cập nhật user thành công.", "OK");
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
                $"Bạn có chắc muốn xóa user `{username}`?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _repo.DropUser(username, cascade: true);
                MessageBox.Show("Xóa user thành công.", "OK");
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
                "Xác nhận Lock",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _repo.LockUser(username);
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
                "Xác nhận Unlock",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _repo.UnlockUser(username);
                RefreshUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void btnRoleCreate_Click(object sender, EventArgs e)
        {
            using var dlg = new EditRoleDialog(mode: EditRoleDialogMode.Create);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            string roleName = dlg.RoleName.Trim().ToUpperInvariant();
            if (!IsValidOracleIdentifier(roleName))
            {
                MessageBox.Show("Role name không hợp lệ. Chỉ dùng chữ/số/_/$/#, bắt đầu bằng chữ, tối đa 30 ký tự.", "Validation");
                return;
            }

            try
            {
                _repo.CreateRole(roleName, dlg.PasswordOrNullForNotIdentified);
                MessageBox.Show("Tạo role thành công.", "OK");
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

            using var dlg = new EditRoleDialog(mode: EditRoleDialogMode.Edit, presetRoleName: roleName);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                _repo.ChangeRolePassword(roleName, dlg.PasswordOrNullForNotIdentified);
                MessageBox.Show("Cập nhật role thành công.", "OK");
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
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _repo.DropRole(roleName);
                MessageBox.Show("Xóa role thành công.", "OK");
                RefreshRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }
    }
}
