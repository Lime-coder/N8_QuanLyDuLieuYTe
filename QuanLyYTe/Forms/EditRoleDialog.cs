using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyYTe.Forms
{
    /// <summary>
    /// Dialog để tạo hoặc sửa Role
    /// </summary>
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
