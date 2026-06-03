using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyYTe.Common;

namespace QuanLyYTe.Forms.Doctor
{
    public partial class frmDoctor : Form
    {
        private Panel pnlContent = null!;
        private Label lblPageTitle = null!, lblBreadcrumb = null!, lblUserInfo = null!;

        public frmDoctor()
        {
            this.Size = new Size(1300, 850);
            this.Text = "Hospital Management - Doctor";
            this.WindowState = FormWindowState.Maximized;
            InitUI();
        }

        private void InitUI()
        {
            Panel pnlSidebar = new Panel { Dock = DockStyle.Left, Width = 230, BackColor = Color.FromArgb(31, 31, 31) };
            Label lblLogo = new Label { 
                Text = "HospitalDoctor", 
                ForeColor = Color.Orange, 
                Dock = DockStyle.Top, 
                Height = 80, 
                TextAlign = ContentAlignment.MiddleCenter, 
                Font = new Font("Segoe UI", 16, FontStyle.Bold) 
            };
            pnlSidebar.Controls.Add(lblLogo);

            AddNav(pnlSidebar, "Quản lý HSBA", () => OpenPage("Quản lý HSBA", () => new frmMedicalRecordManagement()));
            AddNav(pnlSidebar, "Dịch vụ cận lâm sàng", () => OpenPage("Dịch vụ CLS", () => new frmServiceManagement()));
            AddNav(pnlSidebar, "Đơn thuốc", () => OpenPage("Đơn thuốc", () => new frmPrescriptionManagement()));
            AddNav(pnlSidebar, "Bệnh nhân", () => OpenPage("Bệnh nhân", () => new frmPatientManagement()));

            Button btnLogout = new Button
            {
                Text = "Đăng xuất",
                Dock = DockStyle.Top,
                Height = 55,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Tomato,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
            };
            btnLogout.FlatAppearance.BorderSize = 0;

            btnLogout.Click += (s, e) => {
                var confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn đăng xuất?", "Đăng xuất",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question
                );

                if (confirm != DialogResult.Yes) return;

                Application.Restart();
            };
            pnlSidebar.Controls.Add(btnLogout);
            btnLogout.BringToFront();

            Panel pnlTop = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.White };
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
                AutoSize = true, 
                BackColor = Color.Transparent 
            };

            lblUserInfo = new Label { 
                Text = "HOSPITAL_DOCTOR  •  " + (AppSession.CurrentUsername), 
                Anchor = AnchorStyles.Right | AnchorStyles.Top, 
                ForeColor = Color.Orange, 
                Font = new Font("Segoe UI", 10, FontStyle.Bold), 
                AutoSize = true 
            };

            pnlTop.Controls.AddRange(new Control[] { lblPageTitle, lblBreadcrumb, lblUserInfo });
            pnlTop.Resize += (s, e) => lblUserInfo.Location = new Point(pnlTop.Width - lblUserInfo.Width - 25, 35);

            pnlContent = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(245, 245, 250) };
            this.Controls.AddRange(new Control[] { pnlContent, pnlTop, pnlSidebar });
            OpenPage("Quản lý HSBA", () => new frmMedicalRecordManagement());
        }

        private void AddNav(Panel p, string t, Action a) { 
            Button b = new Button { 
                Text = t, Dock = DockStyle.Top, Height = 55, 
                FlatStyle = FlatStyle.Flat, 
                ForeColor = Color.White, 
                TextAlign = ContentAlignment.MiddleLeft, 
                Padding = new Padding(20, 0, 0, 0) 
            };

            b.FlatAppearance.BorderSize = 0;
            b.Click += (s, e) => a(); 
            p.Controls.Add(b); b.BringToFront(); 
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
    }
}