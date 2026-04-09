using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyYTe.DAL;

namespace QuanLyYTe.Forms
{
    public class SecurityAdminForm : Form
    {
        // ── Controls ─────────────────────────────────────────────────
        private TabControl tabMain;
        private TabPage tabUsers, tabRoles;
        private DataGridView dgvUsers, dgvRoles;

        private Button btnCreateUser, btnEditUser, btnDropUser, btnRefreshUsers;
        private Button btnCreateRole, btnEditRole, btnDropRole, btnRefreshRoles;

        private readonly SecurityAdminRepository _repo = new SecurityAdminRepository();

        // ── Constructor ──────────────────────────────────────────────
        public SecurityAdminForm()
        {
            InitializeComponent();
            LoadUsers();
            LoadRoles();
        }

        // ── UI Builder ───────────────────────────────────────────────
        private void InitializeComponent()
        {
            Text = "Quản trị CSDL Oracle – Users & Roles";
            Size = new Size(920, 580);
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(700, 450);

            tabMain = new TabControl { Dock = DockStyle.Fill };
            tabUsers = new TabPage("👤  Người dùng (Users)");
            tabRoles = new TabPage("🔑  Vai trò (Roles)");
            tabMain.TabPages.AddRange(new[] { tabUsers, tabRoles });
            Controls.Add(tabMain);

            BuildUserTab();
            BuildRoleTab();
        }

        // ─────────────── TAB USERS ───────────────────────────────────
        private void BuildUserTab()
        {
            dgvUsers = CreateGrid();
            dgvUsers.Dock = DockStyle.Fill;

            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 46,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(6, 6, 0, 0)
            };

            btnCreateUser = MakeBtn("➕ Tạo User", Color.FromArgb(40, 167, 69));
            btnEditUser = MakeBtn("✏️ Sửa User", Color.FromArgb(0, 123, 255));
            btnDropUser = MakeBtn("🗑️ Xóa User", Color.FromArgb(220, 53, 69));
            btnRefreshUsers = MakeBtn("🔄 Làm mới", Color.FromArgb(108, 117, 125));

            btnCreateUser.Click += (s, e) => ShowCreateUserDialog();
            btnEditUser.Click += (s, e) => ShowEditUserDialog();
            btnDropUser.Click += (s, e) => DropSelectedUser();
            btnRefreshUsers.Click += (s, e) => LoadUsers();

            panel.Controls.AddRange(new Control[]
                { btnCreateUser, btnEditUser, btnDropUser, btnRefreshUsers });

            tabUsers.Controls.Add(dgvUsers);
            tabUsers.Controls.Add(panel);
        }

        // ─────────────── TAB ROLES ───────────────────────────────────
        private void BuildRoleTab()
        {
            dgvRoles = CreateGrid();
            dgvRoles.Dock = DockStyle.Fill;

            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 46,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(6, 6, 0, 0)
            };

            btnCreateRole = MakeBtn("➕ Tạo Role", Color.FromArgb(40, 167, 69));
            btnEditRole = MakeBtn("✏️ Sửa Role", Color.FromArgb(0, 123, 255));
            btnDropRole = MakeBtn("🗑️ Xóa Role", Color.FromArgb(220, 53, 69));
            btnRefreshRoles = MakeBtn("🔄 Làm mới", Color.FromArgb(108, 117, 125));

            btnCreateRole.Click += (s, e) => ShowCreateRoleDialog();
            btnEditRole.Click += (s, e) => ShowEditRoleDialog();
            btnDropRole.Click += (s, e) => DropSelectedRole();
            btnRefreshRoles.Click += (s, e) => LoadRoles();

            panel.Controls.AddRange(new Control[]
                { btnCreateRole, btnEditRole, btnDropRole, btnRefreshRoles });

            tabRoles.Controls.Add(dgvRoles);
            tabRoles.Controls.Add(panel);
        }

        // ── Factory helpers ──────────────────────────────────────────
        private static DataGridView CreateGrid() => new DataGridView
        {
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            RowHeadersVisible = false,
            Font = new Font("Segoe UI", 9.5f)
        };

        private static Button MakeBtn(string text, Color back) => new Button
        {
            Text = text,
            Height = 30,
            Width = 148,
            BackColor = back,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9f, FontStyle.Bold),
            Cursor = Cursors.Hand
        };

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

    // ════════════════════════════════════════════════════════════════
    //  Dialog: Tạo / Sửa User
    // ════════════════════════════════════════════════════════════════
    internal class EditUserDialog : Form
    {
        public string Username => txtUsername.Text.Trim();
        // Khi tạo user: bắt buộc phải có mật khẩu
        // Khi sửa user: nếu checkbox đổi mật khẩu được chọn thì return mật khẩu, nếu không return null
        public string? NewPassword => _isCreate 
            ? txtPassword.Text  // Tạo user: bắt buộc
            : (chkChangePw.Checked ? txtPassword.Text : null);  // Sửa user: tuỳ chọn
        public LockAction LockAction { get; private set; } = LockAction.None;

        private readonly TextBox txtUsername;
        private readonly TextBox txtPassword;
        private readonly TextBox txtConfirm;
        private readonly CheckBox chkChangePw;
        private readonly RadioButton rdoNoChange, rdoLock, rdoUnlock;
        private readonly bool _isCreate;

        public EditUserDialog(string title,
                              bool isCreate,
                              string? fixedUsername = null,
                              bool isCurrentlyLocked = false)
        {
            _isCreate = isCreate;
            Text = title;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false; MinimizeBox = false;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Padding = new Padding(16);

            txtUsername = new TextBox { Width = 230 };
            txtPassword = new TextBox { Width = 230, UseSystemPasswordChar = true };
            txtConfirm = new TextBox { Width = 230, UseSystemPasswordChar = true };
            chkChangePw = new CheckBox { Text = "Đổi mật khẩu", AutoSize = true };
            rdoNoChange = new RadioButton { Text = "Giữ nguyên", AutoSize = true, Checked = true };
            rdoLock = new RadioButton { Text = "🔒 Khóa tài khoản", AutoSize = true };
            rdoUnlock = new RadioButton { Text = "🔓 Mở khóa tài khoản", AutoSize = true };

            var tbl = new TableLayoutPanel
            {
                ColumnCount = 2,
                AutoSize = true,
                Padding = new Padding(8, 8, 8, 4)
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            int r = 0;

            // Username
            AddRow(tbl, r++, "Username:", txtUsername);
            if (fixedUsername != null) { txtUsername.Text = fixedUsername; txtUsername.Enabled = false; }

            if (isCreate)
            {
                // Tạo mới: bắt buộc nhập mật khẩu
                AddRow(tbl, r++, "Mật khẩu *:", txtPassword);
                AddRow(tbl, r++, "Xác nhận *:", txtConfirm);
            }
            else
            {
                // Sửa: checkbox đổi mật khẩu (tuỳ chọn)
                chkChangePw.CheckedChanged += (s, e) =>
                {
                    txtPassword.Enabled = chkChangePw.Checked;
                    txtConfirm.Enabled = chkChangePw.Checked;
                };
                txtPassword.Enabled = false;
                txtConfirm.Enabled = false;

                tbl.SetColumnSpan(chkChangePw, 2);
                tbl.Controls.Add(chkChangePw, 0, r++);
                AddRow(tbl, r++, "Mật khẩu mới:", txtPassword);
                AddRow(tbl, r++, "Xác nhận:", txtConfirm);

                // GroupBox khoá / mở khoá
                var grpLock = new GroupBox
                {
                    Text = "Trạng thái tài khoản",
                    AutoSize = true,
                    Padding = new Padding(8)
                };

                // Chỉ bật radio phù hợp với trạng thái hiện tại
                rdoLock.Enabled = !isCurrentlyLocked;
                rdoUnlock.Enabled = isCurrentlyLocked;

                var lockFlow = new FlowLayoutPanel
                { FlowDirection = FlowDirection.TopDown, AutoSize = true };
                lockFlow.Controls.AddRange(new Control[] { rdoNoChange, rdoLock, rdoUnlock });
                grpLock.Controls.Add(lockFlow);

                tbl.SetColumnSpan(grpLock, 2);
                tbl.Controls.Add(grpLock, 0, r++);
            }

            // Nút
            var btnOK = new Button { Text = "✔ Lưu", Width = 90, Height = 30, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "✖ Hủy", Width = 90, Height = 30, DialogResult = DialogResult.Cancel };
            btnOK.Click += (s, e) => ValidateAndApply();

            var btnPanel = new FlowLayoutPanel
            { AutoSize = true, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(0, 6, 0, 0) };
            btnPanel.Controls.AddRange(new Control[] { btnCancel, btnOK });
            tbl.SetColumnSpan(btnPanel, 2);
            tbl.Controls.Add(btnPanel, 0, r);

            AcceptButton = btnOK;
            CancelButton = btnCancel;
            Controls.Add(tbl);
        }

        private void ValidateAndApply()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            { Warn("Username không được để trống."); return; }

            bool changingPw = _isCreate || chkChangePw.Checked;
            if (changingPw)
            {
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                { Warn("Mật khẩu không được để trống."); return; }
                if (txtPassword.Text != txtConfirm.Text)
                { Warn("Mật khẩu xác nhận không khớp."); return; }
            }

            if (!_isCreate)
            {
                if (rdoLock.Checked) LockAction = LockAction.Lock;
                if (rdoUnlock.Checked) LockAction = LockAction.Unlock;
            }

            DialogResult = DialogResult.OK;
        }

        private static void Warn(string m) =>
            MessageBox.Show(m, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private static void AddRow(TableLayoutPanel tbl, int row, string label, Control ctrl)
        {
            tbl.Controls.Add(new Label { Text = label, TextAlign = ContentAlignment.MiddleLeft, AutoSize = true }, 0, row);
            tbl.Controls.Add(ctrl, 1, row);
        }
    }

    // ════════════════════════════════════════════════════════════════
    //  Dialog: Tạo / Sửa Role
    // ════════════════════════════════════════════════════════════════
    internal class EditRoleDialog : Form
    {
        public string RoleName => txtRole.Text.Trim();

        // null  → NOT IDENTIFIED (xoá mật khẩu)
        // value → đặt / đổi mật khẩu
        // nếu người dùng không chọn gì → giữ nguyên (trả về sentinel qua _keepPw)
        public string? Password { get; private set; }
        private bool _keepPw = true;   // true = không thay đổi gì với password

        private readonly TextBox txtRole = new TextBox { Width = 230 };
        private readonly TextBox txtPassword = new TextBox { Width = 230, UseSystemPasswordChar = true };
        private readonly CheckBox chkSetPw = new CheckBox { AutoSize = true };
        private readonly CheckBox chkRemovePw = new CheckBox { Text = "Xóa mật khẩu (NOT IDENTIFIED)", AutoSize = true };
        private readonly bool _isCreate;

        public EditRoleDialog(string title,
                              bool isCreate,
                              string? fixedRoleName = null,
                              bool hasPassword = false)
        {
            _isCreate = isCreate;
            Text = title;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false; MinimizeBox = false;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Padding = new Padding(16);

            var tbl = new TableLayoutPanel
            {
                ColumnCount = 2,
                AutoSize = true,
                Padding = new Padding(8)
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            int r = 0;

            // Role name
            AddRow(tbl, r++, "Role name:", txtRole);
            if (fixedRoleName != null) { txtRole.Text = fixedRoleName; txtRole.Enabled = false; }

            if (isCreate)
            {
                chkSetPw.Text = "Đặt mật khẩu cho role";
                chkSetPw.CheckedChanged += (s, e) => txtPassword.Enabled = chkSetPw.Checked;
                txtPassword.Enabled = false;

                tbl.SetColumnSpan(chkSetPw, 2);
                tbl.Controls.Add(chkSetPw, 0, r++);
                AddRow(tbl, r++, "Mật khẩu:", txtPassword);
            }
            else
            {
                // Hiển thị trạng thái hiện tại
                var lblStatus = new Label
                {
                    Text = hasPassword ? "Hiện tại: Có mật khẩu 🔐" : "Hiện tại: Không có mật khẩu 🔓",
                    AutoSize = true,
                    ForeColor = hasPassword ? Color.DarkOrange : Color.SeaGreen,
                    Font = new Font("Segoe UI", 9f, FontStyle.Italic)
                };
                tbl.SetColumnSpan(lblStatus, 2);
                tbl.Controls.Add(lblStatus, 0, r++);

                chkSetPw.Text = hasPassword ? "Đổi mật khẩu mới" : "Thêm mật khẩu";
                chkSetPw.CheckedChanged += (s, e) =>
                {
                    txtPassword.Enabled = chkSetPw.Checked;
                    if (chkSetPw.Checked) chkRemovePw.Checked = false;
                    chkRemovePw.Enabled = !chkSetPw.Checked && hasPassword;
                };
                chkRemovePw.CheckedChanged += (s, e) =>
                {
                    if (chkRemovePw.Checked) chkSetPw.Checked = false;
                    chkSetPw.Enabled = !chkRemovePw.Checked;
                    txtPassword.Enabled = false;
                };
                txtPassword.Enabled = false;
                chkRemovePw.Enabled = hasPassword;

                tbl.SetColumnSpan(chkSetPw, 2);
                tbl.Controls.Add(chkSetPw, 0, r++);
                AddRow(tbl, r++, "Mật khẩu:", txtPassword);

                tbl.SetColumnSpan(chkRemovePw, 2);
                tbl.Controls.Add(chkRemovePw, 0, r++);
            }

            // Nút
            var btnOK = new Button { Text = "✔ Lưu", Width = 90, Height = 30, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "✖ Hủy", Width = 90, Height = 30, DialogResult = DialogResult.Cancel };
            btnOK.Click += (s, e) => ValidateAndApply();

            var btnPanel = new FlowLayoutPanel
            { AutoSize = true, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(0, 6, 0, 0) };
            btnPanel.Controls.AddRange(new Control[] { btnCancel, btnOK });
            tbl.SetColumnSpan(btnPanel, 2);
            tbl.Controls.Add(btnPanel, 0, r);

            AcceptButton = btnOK;
            CancelButton = btnCancel;
            Controls.Add(tbl);
        }

        private void ValidateAndApply()
        {
            if (string.IsNullOrWhiteSpace(txtRole.Text))
            { Warn("Tên role không được để trống."); return; }

            if (chkSetPw.Checked)
            {
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                { Warn("Vui lòng nhập mật khẩu hoặc bỏ chọn."); return; }
                Password = txtPassword.Text;
                _keepPw = false;
            }
            else if (chkRemovePw.Checked)
            {
                Password = null;    // → NOT IDENTIFIED
                _keepPw = false;
            }
            // else: _keepPw = true, Password = null → form chính biết không cần gọi AlterRolePassword

            DialogResult = DialogResult.OK;
        }

        /// <summary>True nếu người dùng không thay đổi gì về mật khẩu role.</summary>
        public bool KeepPassword => _keepPw;

        private static void Warn(string m) =>
            MessageBox.Show(m, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private static void AddRow(TableLayoutPanel tbl, int row, string label, Control ctrl)
        {
            tbl.Controls.Add(new Label { Text = label, TextAlign = ContentAlignment.MiddleLeft, AutoSize = true }, 0, row);
            tbl.Controls.Add(ctrl, 1, row);
        }
    }
}