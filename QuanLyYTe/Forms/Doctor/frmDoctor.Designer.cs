using QuanLyYTe.Common;

namespace QuanLyYTe.Forms.Doctor
{
    partial class frmDoctor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Panel pnlSidebar = new Panel { 
                Dock = DockStyle.Left, 
                Width = 230, 
                BackColor = Color.FromArgb(31, 31, 31) 
            };

            Label lblLogo = new Label { 
                Text = "HOSPITAL DOCTOR", 
                ForeColor = Color.Orange, 
                Dock = DockStyle.Top, 
                Height = 80, 
                TextAlign = ContentAlignment.MiddleCenter, 
                Font = new Font("Segoe UI", 15, FontStyle.Bold) 
            };

            pnlSidebar.Controls.Add(lblLogo);

            // Các Tab điều hướng
            AddNav(pnlSidebar, "Quản lý HSBA", () => OpenPage("Quản lý HSBA", () => new frmMedicalRecordManagement()));
            AddNav(pnlSidebar, "Đơn thuốc", () => OpenPage("Đơn thuốc", () => new frmPrescriptionManagement()));
            AddNav(pnlSidebar, "Bệnh nhân", () => OpenPage("Bệnh nhân", () => new frmPatientManagement()));
            AddNav(pnlSidebar, "Hồ sơ cá nhân", () => OpenPage("Hồ sơ cá nhân", () => new frmDoctorProfile()));
            AddNav(pnlSidebar, "Thông báo OLS", () => OpenPage("Thông báo OLS", () => new QuanLyYTe.Forms.Common.frmNotifications()));

            Button btnLogout = new Button
            {
                Text = "Đăng xuất",
                Dock = DockStyle.Top,
                Height = 55,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Tomato,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (s, e) => {
                var confirm = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes) Application.Restart();
            };

            pnlSidebar.Controls.Add(btnLogout);
            btnLogout.BringToFront();

            Panel pnlTop = new Panel { 
                Dock = DockStyle.Top, 
                Height = 100, 
                BackColor = Color.White 
            };

            lblPageTitle = new Label { 
                Text = "Dashboard", 
                Location = new Point(25, 20), 
                Font = new Font("Segoe UI", 14, FontStyle.Bold), 
                AutoSize = true 
            };

            lblBreadcrumb = new Label { 
                Text = "Dashboard / Trang chủ", 
                Location = new Point(25, 55), 
                ForeColor = Color.Gray, 
                AutoSize = true 
            };

            lblUserInfo = new Label { 
                Text = "Bác sĩ: " + (AppSession.CurrentUsername), 
                Anchor = AnchorStyles.Right | AnchorStyles.Top, 
                ForeColor = Color.Orange, 
                Font = new Font("Segoe UI", 10, FontStyle.Bold), 
                AutoSize = true 
            };

            pnlTop.Controls.AddRange(new Control[] { lblPageTitle, lblBreadcrumb, lblUserInfo });

            pnlTop.Resize += (s, e) => lblUserInfo.Location = new Point(pnlTop.Width - lblUserInfo.Width - 25, 35);

            pnlContent = new Panel { 
                Dock = DockStyle.Fill, 
                BackColor = Color.FromArgb(245, 245, 250) 
            };

            this.Controls.AddRange(new Control[] { pnlContent, pnlTop, pnlSidebar });
        }

        #endregion

        private Panel pnlContent;
        private Label lblPageTitle;
        private Label lblBreadcrumb;
        private Label lblUserInfo;
    }
}
