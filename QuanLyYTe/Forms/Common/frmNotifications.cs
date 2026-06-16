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
                if (dgvNotifications.Columns.Contains("ID_THONGBAO"))
                {
                    dgvNotifications.Columns["ID_THONGBAO"].HeaderText = "Mã TB";
                    dgvNotifications.Columns["ID_THONGBAO"].Width = 80;
                }
                if (dgvNotifications.Columns.Contains("NOIDUNG"))
                {
                    dgvNotifications.Columns["NOIDUNG"].HeaderText = "Nội dung";
                    dgvNotifications.Columns["NOIDUNG"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                if (dgvNotifications.Columns.Contains("NGAYGIO"))
                {
                    dgvNotifications.Columns["NGAYGIO"].HeaderText = "Ngày giờ";
                    dgvNotifications.Columns["NGAYGIO"].Width = 150;
                }
                if (dgvNotifications.Columns.Contains("DIADIEM"))
                {
                    dgvNotifications.Columns["DIADIEM"].HeaderText = "Địa điểm";
                    dgvNotifications.Columns["DIADIEM"].Width = 200;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tải thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
