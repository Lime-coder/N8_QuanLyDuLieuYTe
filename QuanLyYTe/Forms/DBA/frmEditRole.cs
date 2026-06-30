namespace QuanLyYTe.Forms.DBA
{
    public enum EditRoleDialogMode
    {
        Create,
        Edit
    }

    public partial class frmEditRole : Form
    {
        private readonly EditRoleDialogMode _mode;

        public string RoleName => txtRole.Text;

        public string? PasswordOrNullForNotIdentified => chkNotIdentified.Checked ? null : txtPassword.Text;

        public frmEditRole()
        {
            InitializeComponent();
        }

        public frmEditRole(EditRoleDialogMode mode, string? presetRoleName = null)
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

        private void TogglePasswordEnabled()
        {
            txtPassword.Enabled = !chkNotIdentified.Checked;
            if (chkNotIdentified.Checked) txtPassword.Text = string.Empty;
        }

        private void chkNotIdentified_CheckedChanged(object? sender, EventArgs e)
        {
            TogglePasswordEnabled();
        }
    }
}
