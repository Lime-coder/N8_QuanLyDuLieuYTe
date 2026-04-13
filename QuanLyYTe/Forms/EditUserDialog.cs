using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyYTe.DAL;

namespace QuanLyYTe.Forms
{
    /// <summary>
    /// Dialog để tạo hoặc sửa User
    /// </summary>
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
}
