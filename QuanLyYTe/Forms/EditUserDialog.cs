namespace QuanLyYTe.Forms
{
    public enum EditUserDialogMode
    {
        Create,
        EditPassword
    }

    public class EditUserDialog : Form
    {
        private readonly EditUserDialogMode _mode;

        private TextBox txtUsername = null!;
        private TextBox txtPassword = null!;
        private TextBox txtConfirmPassword = null!;
        private Button btnOk = null!;
        private Button btnCancel = null!;

        public string Username => txtUsername.Text;
        public string Password => txtPassword.Text;

        public EditUserDialog(EditUserDialogMode mode, string? presetUsername = null)
        {
            _mode = mode;
            InitializeComponent();

            if (!string.IsNullOrWhiteSpace(presetUsername))
                txtUsername.Text = presetUsername;

            if (_mode == EditUserDialogMode.EditPassword)
            {
                txtUsername.ReadOnly = true;
                Text = "Edit user (change password)";
            }
            else
            {
                Text = "Create user";
            }
        }

        private void InitializeComponent()
        {
            var lblUsername = new Label();
            var lblPassword = new Label();
            var lblConfirm = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            txtConfirmPassword = new TextBox();
            btnOk = new Button();
            btnCancel = new Button();

            SuspendLayout();

            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(16, 18);
            lblUsername.Text = "Username";

            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(16, 60);
            lblPassword.Text = "Password";

            lblConfirm.AutoSize = true;
            lblConfirm.Location = new Point(16, 102);
            lblConfirm.Text = "Confirm";

            txtUsername.Location = new Point(110, 14);
            txtUsername.Size = new Size(260, 27);

            txtPassword.Location = new Point(110, 56);
            txtPassword.Size = new Size(260, 27);
            txtPassword.UseSystemPasswordChar = true;

            txtConfirmPassword.Location = new Point(110, 98);
            txtConfirmPassword.Size = new Size(260, 27);
            txtConfirmPassword.UseSystemPasswordChar = true;

            btnOk.Location = new Point(214, 146);
            btnOk.Size = new Size(75, 30);
            btnOk.Text = "OK";
            btnOk.Click += (_, __) =>
            {
                // Require confirm password when setting/changing password
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show(this, "Password không được để trống.", "Validation");
                    txtPassword.Focus();
                    return;
                }

                if (!string.Equals(txtPassword.Text, txtConfirmPassword.Text, StringComparison.Ordinal))
                {
                    MessageBox.Show(this, "Password và Confirm không khớp.", "Validation");
                    txtConfirmPassword.Focus();
                    txtConfirmPassword.SelectAll();
                    return;
                }

                DialogResult = DialogResult.OK;
                Close();
            };

            btnCancel.Location = new Point(295, 146);
            btnCancel.Size = new Size(75, 30);
            btnCancel.Text = "Cancel";
            btnCancel.DialogResult = DialogResult.Cancel;

            AcceptButton = btnOk;
            CancelButton = btnCancel;
            ClientSize = new Size(392, 196);
            Controls.Add(lblUsername);
            Controls.Add(lblPassword);
            Controls.Add(lblConfirm);
            Controls.Add(txtUsername);
            Controls.Add(txtPassword);
            Controls.Add(txtConfirmPassword);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            // Keep confirm password visible for both Create and EditPassword

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
