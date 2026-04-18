namespace QuanLyYTe.Forms
{
    public enum EditRoleDialogMode
    {
        Create,
        Edit
    }

    public class EditRoleDialog : Form
    {
        private readonly EditRoleDialogMode _mode;

        private TextBox txtRole = null!;
        private CheckBox chkNotIdentified = null!;
        private TextBox txtPassword = null!;
        private Button btnOk = null!;
        private Button btnCancel = null!;

        public string RoleName => txtRole.Text;

        /// <summary>
        /// Null => NOT IDENTIFIED (không password). Non-null => IDENTIFIED BY "password"
        /// </summary>
        public string? PasswordOrNullForNotIdentified => chkNotIdentified.Checked ? null : txtPassword.Text;

        public EditRoleDialog(EditRoleDialogMode mode, string? presetRoleName = null)
        {
            _mode = mode;
            InitializeComponent();

            if (!string.IsNullOrWhiteSpace(presetRoleName))
                txtRole.Text = presetRoleName;

            if (_mode == EditRoleDialogMode.Edit)
            {
                txtRole.ReadOnly = true;
                Text = "Sửa role";
            }
            else
            {
                Text = "Tạo role";
            }

            TogglePasswordEnabled();
        }

        private void InitializeComponent()
        {
            var lblRole = new Label();
            var lblPassword = new Label();

            txtRole = new TextBox();
            chkNotIdentified = new CheckBox();
            txtPassword = new TextBox();
            btnOk = new Button();
            btnCancel = new Button();

            SuspendLayout();

            lblRole.AutoSize = true;
            lblRole.Location = new Point(16, 18);
            lblRole.Text = "Tên role";

            txtRole.Location = new Point(110, 14);
            txtRole.Size = new Size(260, 27);

            chkNotIdentified.AutoSize = true;
            chkNotIdentified.Location = new Point(110, 54);
            chkNotIdentified.Text = "NOT IDENTIFIED (không mật khẩu)";
            chkNotIdentified.CheckedChanged += (_, __) => TogglePasswordEnabled();

            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(16, 90);
            lblPassword.Text = "Mật khẩu";

            txtPassword.Location = new Point(110, 86);
            txtPassword.Size = new Size(260, 27);
            txtPassword.UseSystemPasswordChar = true;

            btnOk.Location = new Point(214, 130);
            btnOk.Size = new Size(75, 30);
            btnOk.Text = "Đồng ý";
            btnOk.DialogResult = DialogResult.OK;

            btnCancel.Location = new Point(295, 130);
            btnCancel.Size = new Size(75, 30);
            btnCancel.Text = "Hủy";
            btnCancel.DialogResult = DialogResult.Cancel;

            AcceptButton = btnOk;
            CancelButton = btnCancel;
            ClientSize = new Size(392, 178);
            Controls.Add(lblRole);
            Controls.Add(txtRole);
            Controls.Add(chkNotIdentified);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            ResumeLayout(false);
            PerformLayout();
        }

        private void TogglePasswordEnabled()
        {
            txtPassword.Enabled = !chkNotIdentified.Checked;
            if (chkNotIdentified.Checked) txtPassword.Text = string.Empty;
        }
    }
}
