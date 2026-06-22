using System;
using System.Windows.Forms;
using QuanLyYTe.Common;
using QuanLyYTe.Repositories;

namespace QuanLyYTe.Forms.Common
{
    public partial class frmNotifications : Form
    {
        private readonly NotificationRepository _repo = new NotificationRepository();

        public frmNotifications()
        {
            InitializeComponent();
        }

        private void frmNotifications_Load(object sender, EventArgs e)
        {
            if (AppSession.CurrentUserRole == "RL_OLS_TESTER" || AppSession.CurrentUsername.StartsWith("U"))
            {
                lblTestMode.Text = $"(Chế độ Kiểm thử OLS - Tài khoản: {AppSession.CurrentUsername})";
            }
            LoadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                dgvNotifications.DataSource = _repo.GetNotifications();
                
                // Customize columns if they exist
                if (dgvNotifications.Columns.Contains("NOTIFICATION_ID"))
                {
                    dgvNotifications.Columns["NOTIFICATION_ID"].HeaderText = "Mã TB";
                    dgvNotifications.Columns["NOTIFICATION_ID"].Width = 80;
                }
                if (dgvNotifications.Columns.Contains("DESCRIPTION"))
                {
                    dgvNotifications.Columns["DESCRIPTION"].HeaderText = "Nội dung";
                    dgvNotifications.Columns["DESCRIPTION"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                if (dgvNotifications.Columns.Contains("POSTED_DATE"))
                {
                    dgvNotifications.Columns["POSTED_DATE"].HeaderText = "Ngày giờ";
                    dgvNotifications.Columns["POSTED_DATE"].Width = 150;
                }
                if (dgvNotifications.Columns.Contains("LOCATION"))
                {
                    dgvNotifications.Columns["LOCATION"].HeaderText = "Địa điểm";
                    dgvNotifications.Columns["LOCATION"].Width = 200;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tải thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
