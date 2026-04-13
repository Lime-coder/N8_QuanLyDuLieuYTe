using System;
using System.Data;
using System.Windows.Forms;
using QuanLyYTe.DAL;

namespace QuanLyYTe.Forms
{
    public partial class SecurityAdminForm : Form
    {
        private readonly SecurityAdminRepository _repo = new SecurityAdminRepository();

        // ── Constructor ──────────────────────────────────────────────
        public SecurityAdminForm()
        {
            InitializeComponent();
            LoadUsers();
            LoadRoles();
        }

        // ── Data loaders ─────────────────────────────────────────────
        private void LoadUsers()
        {
            try { dgvUsers.DataSource = _repo.GetUsers(); }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void LoadRoles()
        {
            try { dgvRoles.DataSource = _repo.GetRoles(); }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        // ── USER actions ─────────────────────────────────────────────
        private void ShowCreateUserDialog()
        {
            using var dlg = new EditUserDialog("Tạo User mới", isCreate: true);
            if (dlg.ShowDialog() != DialogResult.OK) return;
            try
            {
                string password = dlg.NewPassword ?? "";
                if (string.IsNullOrWhiteSpace(password))
                {
                    ShowError("Mật khẩu không được để trống.");
                    return;
                }
                _repo.CreateUser(dlg.Username, password);
                ShowInfo($"Tạo user '{dlg.Username}' thành công.");
                LoadUsers();
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void ShowEditUserDialog()
        {
            if (!TryGetSelected(dgvUsers, "USERNAME", out string username)) return;

            string currentStatus = dgvUsers.CurrentRow!.Cells["ACCOUNT_STATUS"].Value?.ToString() ?? "";
            bool isLocked = currentStatus.Contains("LOCKED");

            using var dlg = new EditUserDialog($"Sửa User – {username}",
                                               isCreate: false,
                                               fixedUsername: username,
                                               isCurrentlyLocked: isLocked);
            if (dlg.ShowDialog() != DialogResult.OK) return;
            try
            {
                _repo.AlterUser(username, dlg.NewPassword, dlg.LockAction);

                string msg = $"Cập nhật user '{username}' thành công.";
                if (dlg.LockAction == LockAction.Lock) msg += "\n→ Tài khoản đã bị KHÓA.";
                if (dlg.LockAction == LockAction.Unlock) msg += "\n→ Tài khoản đã được MỞ KHÓA.";
                ShowInfo(msg);
                LoadUsers();
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void DropSelectedUser()
        {
            if (!TryGetSelected(dgvUsers, "USERNAME", out string username)) return;
            if (Confirm($"Xác nhận xóa user '{username}'?") != DialogResult.Yes) return;
            try
            {
                _repo.DropUser(username, cascade: true);
                ShowInfo($"Đã xóa user '{username}'.");
                LoadUsers();
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        // ── ROLE actions ─────────────────────────────────────────────
        private void ShowCreateRoleDialog()
        {
            using var dlg = new EditRoleDialog("Tạo Role mới", isCreate: true);
            if (dlg.ShowDialog() != DialogResult.OK) return;
            try
            {
                _repo.CreateRole(dlg.RoleName, dlg.Password);
                ShowInfo($"Tạo role '{dlg.RoleName}' thành công.");
                LoadRoles();
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void ShowEditRoleDialog()
        {
            if (!TryGetSelected(dgvRoles, "ROLE", out string roleName)) return;

            string currentAuth = dgvRoles.CurrentRow!.Cells["PASSWORD_REQUIRED"].Value?.ToString() ?? "NO";
            using var dlg = new EditRoleDialog($"Sửa Role – {roleName}",
                                               isCreate: false,
                                               fixedRoleName: roleName,
                                               hasPassword: currentAuth == "YES");
            if (dlg.ShowDialog() != DialogResult.OK) return;
            if (dlg.KeepPassword) { ShowInfo("Không có thay đổi nào được lưu."); return; }
            try
            {
                _repo.AlterRolePassword(roleName, dlg.Password);
                ShowInfo($"Cập nhật role '{roleName}' thành công.");
                LoadRoles();
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void DropSelectedRole()
        {
            if (!TryGetSelected(dgvRoles, "ROLE", out string roleName)) return;
            if (Confirm($"Xác nhận xóa role '{roleName}'?") != DialogResult.Yes) return;
            try
            {
                _repo.DropRole(roleName);
                ShowInfo($"Đã xóa role '{roleName}'.");
                LoadRoles();
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        // ── Tiny helpers ─────────────────────────────────────────────
        private static bool TryGetSelected(DataGridView dgv, string col, out string value)
        {
            value = string.Empty;
            if (dgv.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một dòng.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            value = dgv.CurrentRow.Cells[col].Value?.ToString() ?? "";
            return true;
        }

        private static void ShowInfo(string msg) =>
            MessageBox.Show(msg, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private static void ShowError(string msg) =>
            MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

        private static DialogResult Confirm(string msg) =>
            MessageBox.Show(msg, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
    }
}
