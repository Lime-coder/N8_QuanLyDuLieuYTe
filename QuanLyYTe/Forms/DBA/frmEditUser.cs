namespace QuanLyYTe.Forms.DBA
{
    public enum EditUserDialogMode
    {
        Create,
        EditPassword
    }

    public class frmEditUser : Form
    {
        private readonly EditUserDialogMode _mode;

        private TextBox txtUsername = null!;
        private TextBox txtPassword = null!;
        private TextBox txtConfirmPassword = null!;
        private Button btnOk = null!;
        private Button btnCancel = null!;

        public string Username => txtUsername.Text;
        public string Password => txtPassword.Text;

        public frmEditUser()
        {
            InitializeComponent();
        }

        public frmEditUser(EditUserDialogMode mode, string? presetUsername = null)
        {
            _mode = mode;
            InitializeComponent();

            if (!string.IsNullOrWhiteSpace(presetUsername))
                txtUsername.Text = presetUsername;

            if (_mode == EditUserDialogMode.EditPassword)
            {
                txtUsername.ReadOnly = true;
                Text = "Sửa user (đổi mật khẩu)";
            }
            else
            {
                Text = "Tạo user";
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
            lblUsername.Text = "Tên user";

            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(16, 60);
            lblPassword.Text = "Mật khẩu";

            lblConfirm.AutoSize = true;
            lblConfirm.Location = new Point(16, 102);
            lblConfirm.Text = "Xác nhận mật khẩu";

            txtUsername.Location = new Point(145, 14);
            txtUsername.Size = new Size(225, 27);

            txtPassword.Location = new Point(145, 56);
            txtPassword.Size = new Size(225, 27);
            txtPassword.UseSystemPasswordChar = true;

            txtConfirmPassword.Location = new Point(145, 98);
            txtConfirmPassword.Size = new Size(225, 27);
            txtConfirmPassword.UseSystemPasswordChar = true;

            btnOk.Location = new Point(214, 146);
            btnOk.Size = new Size(75, 30);
            btnOk.Text = "Đồng ý";
            btnOk.Click += new System.EventHandler(this.btnOk_Click);

            btnCancel.Location = new Point(295, 146);
            btnCancel.Size = new Size(75, 30);
            btnCancel.Text = "Hủy";
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

            ResumeLayout(false);
            PerformLayout();
        }

        private void btnOk_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show(this, "Mật khẩu không được để trống.", "Kiểm tra");
                txtPassword.Focus();
                return;
            }

            if (!string.Equals(txtPassword.Text, txtConfirmPassword.Text, StringComparison.Ordinal))
            {
                MessageBox.Show(this, "Hai lần nhập mật khẩu không khớp.", "Kiểm tra");
                txtConfirmPassword.Focus();
                txtConfirmPassword.SelectAll();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
