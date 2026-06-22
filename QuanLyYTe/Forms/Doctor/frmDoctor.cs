using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyYTe.Common;

namespace QuanLyYTe.Forms.Doctor
{
    public partial class frmDoctor : Form
    {
        public frmDoctor()
        {
            this.Size = new Size(1300, 850);
            this.Text = "Hospital Management - Doctor";
            this.WindowState = FormWindowState.Maximized;
            InitializeComponent();
            OpenPage("Quản lý HSBA", () => new frmMedicalRecordManagement());
        }

        private void frmDoctor_Load(object sender, EventArgs e)
        {
            try
            {
                var svc = new QuanLyYTe.Services.DoctorService();
                var dt = svc.GetSelfInfo();
                if (dt.Rows.Count > 0)
                {
                    AppSession.CurrentFullName = dt.Rows[0]["FULL_NAME"]?.ToString();
                }
            }
            catch { }

            lblUserInfo.Text = $"Bác sĩ  ·  {(AppSession.CurrentFullName ?? AppSession.CurrentUsername).ToUpper()}  ·  {AppSession.CurrentUserId}";
            PositionUserInfo();
        }

        private void pnlTop_Resize(object sender, EventArgs e)
        {
            PositionUserInfo();
        }

        private void PositionUserInfo()
        {
            lblUserInfo.Location = new Point(pnlTop.Width - lblUserInfo.Width - 25, 35);
        }

        private void OpenPage(string t, Func<Form> f)
        {
            lblPageTitle.Text = t; 
            lblBreadcrumb.Text = "Dashboard / " + t;
            pnlContent.Controls.Clear();
            Form c = f(); 
            c.TopLevel = false; 
            c.FormBorderStyle = FormBorderStyle.None; 
            c.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(c); 
            c.Show();
        }

        private void btnNavHSBA_Click(object sender, EventArgs e) => OpenPage("Quản lý HSBA", () => new frmMedicalRecordManagement());
        private void btnNavPrescription_Click(object sender, EventArgs e) => OpenPage("Đơn thuốc", () => new frmPrescriptionManagement());
        private void btnNavPatient_Click(object sender, EventArgs e) => OpenPage("Bệnh nhân", () => new frmPatientManagement());
        private void btnNavProfile_Click(object sender, EventArgs e) => OpenPage("Hồ sơ cá nhân", () => new frmDoctorProfile());
        private void btnNavNotifications_Click(object sender, EventArgs e) => OpenPage("Thông báo OLS", () => new QuanLyYTe.Forms.Common.frmNotifications());

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes) Application.Restart();
        }
    }
}