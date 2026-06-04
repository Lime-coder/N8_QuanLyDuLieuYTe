using System;
using System.Configuration;
using System.Windows.Forms;
using System.Xml.Linq;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.Helpers;
using QuanLyYTe.Services;

namespace QuanLyYTe.Forms
{
    public partial class frmLogin : Form
    {
        private readonly AuthService _authService = new AuthService();

        public frmLogin()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            pnlCenter_Resize(sender, e);
            txtUsername.Focus();
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string dataSource = txtDataSource.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(dataSource))
            {
                ShowError("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            try
            {
                btnConnect.Enabled = false;
                btnConnect.Text = "Đang kết nối...";
                lblError.Visible = false;

                _authService.Login(user, password, dataSource);

                Form mainForm = RoleRouter.Resolve(user);
                mainForm.Show();
                this.Hide();
                mainForm.FormClosed += (s, args) => this.Close();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                btnConnect.Enabled = true;
                btnConnect.Text = "Kết nối";
            }
        }

        private void txtDataSource_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnConnect_Click(sender, e);
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btnConnect_Click(sender, e);
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }
    }
}