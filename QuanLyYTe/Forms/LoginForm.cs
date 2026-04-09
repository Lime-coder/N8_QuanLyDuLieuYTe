using System;
using System.Drawing;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace QuanLyYTe.Forms
{
    public partial class LoginForm : Form
    {
        public string ConnectionString { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string dataSource = txtDataSource.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(dataSource))
            {
                lblError.Text = "Vui lòng nhập tên người dùng và Data Source.";
                lblError.Visible = true;
                return;
            }

            string cs = $"User Id={user};Password={password};Data Source={dataSource};";

            try
            {
                using var conn = new OracleConnection(cs);
                conn.Open(); // throws if credentials are wrong
                ConnectionString = cs;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (OracleException ex)
            {
                lblError.Text = $"Lỗi kết nối: {ex.Message}";
                lblError.Visible = true;
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btnConnect_Click(sender, e);
        }
    }
}