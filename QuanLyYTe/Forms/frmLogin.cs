using System;
using System.Configuration;
using System.Windows.Forms;
using System.Xml.Linq;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DAL;
using QuanLyYTe.Helper;

namespace QuanLyYTe.Forms
{
    public partial class frmLogin : Form
    {
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

            if (string.IsNullOrEmpty(user))
            {
                ShowError("Vui lòng nhập tên người dùng.");
                return;
            }
            if (string.IsNullOrEmpty(dataSource))
            {
                ShowError("Vui lòng nhập Data Source.");
                return;
            }

            string connStr = $"User Id={user}; Password={password}; Data Source={dataSource};";
            try
            {
                btnConnect.Enabled = false;
                btnConnect.Text = "Đang kết nối...";
                lblError.Visible = false;

                OracleHelper.TestConnection(connStr);
                OracleHelper.SetConnectionString(connStr);

                // ↓ replaces: var dashboard = new Dashboard(user);
                Form mainForm = RoleRouter.Resolve(user);
                mainForm.Show();
                this.Hide();
                mainForm.FormClosed += (s, args) => this.Close();
            }
            catch (OracleException ex)
            {
                ShowError(OraErrToVietnamese(ex.Number, ex.Message));
            }
            catch (InvalidOperationException ex)   // unknown / null role from RoleRouter
            {
                ShowError(ex.Message);
            }
            finally
            {
                btnConnect.Enabled = true;
                btnConnect.Text = "Kết nối";
            }
        }

        // Ensure the Enter key logic points to the right textbox
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

        // Map common Oracle error codes to friendly Vietnamese messages
        private string OraErrToVietnamese(int code, string fallback)
        {
            return code switch
            {
                1017 => "Sai tên người dùng hoặc mật khẩu.",
                28000 => "Tài khoản đã bị khóa.",
                28001 => "Mật khẩu đã hết hạn, vui lòng đổi mật khẩu.",
                12541 => "Không thể kết nối đến Oracle Listener.",
                12514 => "Service name không tồn tại trên server.",
                _ => $"Lỗi Oracle ({code}): {fallback}"
            };
        }
    }
}