using System;
using System.Configuration;
using System.Windows.Forms;
using System.Xml.Linq;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DAL;

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

            // Grab the full data source string directly from the UI
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

            // Drop the UI input directly into the Data Source parameter
            string connStr = $"User Id={user}; Password={password}; Data Source={dataSource};";

            try
            {
                btnConnect.Enabled = false;
                btnConnect.Text = "Đang kết nối...";
                lblError.Visible = false;

                OracleHelper.TestConnection(connStr);
                OracleHelper.SetConnectionString(connStr);

                var dashboard = new Dashboard(user);
                dashboard.Show();
                this.Hide();
                dashboard.FormClosed += (s, args) => this.Close();
            }
            catch (OracleException ex)
            {
                ShowError(OraErrToVietnamese(ex.Number, ex.Message));
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