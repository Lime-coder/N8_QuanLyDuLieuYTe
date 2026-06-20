namespace QuanLyYTe.Forms.Doctor
{
    partial class frmDoctor
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlSidebar = new Panel();
            btnLogout = new Button();
            btnNavNotifications = new Button();
            btnNavProfile = new Button();
            btnNavPatient = new Button();
            btnNavPrescription = new Button();
            btnNavHSBA = new Button();
            lblLogo = new Label();
            pnlTop = new Panel();
            lblUserInfo = new Label();
            lblBreadcrumb = new Label();
            lblPageTitle = new Label();
            pnlContent = new Panel();
            pnlSidebar.SuspendLayout();
            pnlTop.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSidebar
            // 
            pnlSidebar.BackColor = Color.FromArgb(31, 31, 31);
            pnlSidebar.Controls.Add(btnLogout);
            pnlSidebar.Controls.Add(btnNavNotifications);
            pnlSidebar.Controls.Add(btnNavProfile);
            pnlSidebar.Controls.Add(btnNavPatient);
            pnlSidebar.Controls.Add(btnNavPrescription);
            pnlSidebar.Controls.Add(btnNavHSBA);
            pnlSidebar.Controls.Add(lblLogo);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 0);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new Size(230, 850);
            pnlSidebar.TabIndex = 0;
            // 
            // btnLogout
            // 
            btnLogout.Dock = DockStyle.Top;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLogout.ForeColor = Color.Tomato;
            btnLogout.Location = new Point(0, 355);
            btnLogout.Name = "btnLogout";
            btnLogout.Padding = new Padding(20, 0, 0, 0);
            btnLogout.Size = new Size(230, 55);
            btnLogout.TabIndex = 6;
            btnLogout.Text = "Đăng xuất";
            btnLogout.TextAlign = ContentAlignment.MiddleLeft;
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click;
            // 
            // btnNavNotifications
            // 
            btnNavNotifications.Dock = DockStyle.Top;
            btnNavNotifications.FlatAppearance.BorderSize = 0;
            btnNavNotifications.FlatAppearance.MouseDownBackColor = Color.FromArgb(50, 50, 58);
            btnNavNotifications.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 46);
            btnNavNotifications.FlatStyle = FlatStyle.Flat;
            btnNavNotifications.ForeColor = Color.FromArgb(190, 190, 200);
            btnNavNotifications.Location = new Point(0, 300);
            btnNavNotifications.Name = "btnNavNotifications";
            btnNavNotifications.Padding = new Padding(20, 0, 0, 0);
            btnNavNotifications.Size = new Size(230, 55);
            btnNavNotifications.TabIndex = 5;
            btnNavNotifications.Text = "Thông báo OLS";
            btnNavNotifications.TextAlign = ContentAlignment.MiddleLeft;
            btnNavNotifications.UseVisualStyleBackColor = true;
            btnNavNotifications.Click += btnNavNotifications_Click;
            // 
            // btnNavProfile
            // 
            btnNavProfile.Dock = DockStyle.Top;
            btnNavProfile.FlatAppearance.BorderSize = 0;
            btnNavProfile.FlatAppearance.MouseDownBackColor = Color.FromArgb(50, 50, 58);
            btnNavProfile.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 46);
            btnNavProfile.FlatStyle = FlatStyle.Flat;
            btnNavProfile.ForeColor = Color.FromArgb(190, 190, 200);
            btnNavProfile.Location = new Point(0, 245);
            btnNavProfile.Name = "btnNavProfile";
            btnNavProfile.Padding = new Padding(20, 0, 0, 0);
            btnNavProfile.Size = new Size(230, 55);
            btnNavProfile.TabIndex = 4;
            btnNavProfile.Text = "Hồ sơ cá nhân";
            btnNavProfile.TextAlign = ContentAlignment.MiddleLeft;
            btnNavProfile.UseVisualStyleBackColor = true;
            btnNavProfile.Click += btnNavProfile_Click;
            // 
            // btnNavPatient
            // 
            btnNavPatient.Dock = DockStyle.Top;
            btnNavPatient.FlatAppearance.BorderSize = 0;
            btnNavPatient.FlatAppearance.MouseDownBackColor = Color.FromArgb(50, 50, 58);
            btnNavPatient.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 46);
            btnNavPatient.FlatStyle = FlatStyle.Flat;
            btnNavPatient.ForeColor = Color.FromArgb(190, 190, 200);
            btnNavPatient.Location = new Point(0, 190);
            btnNavPatient.Name = "btnNavPatient";
            btnNavPatient.Padding = new Padding(20, 0, 0, 0);
            btnNavPatient.Size = new Size(230, 55);
            btnNavPatient.TabIndex = 3;
            btnNavPatient.Text = "Bệnh nhân";
            btnNavPatient.TextAlign = ContentAlignment.MiddleLeft;
            btnNavPatient.UseVisualStyleBackColor = true;
            btnNavPatient.Click += btnNavPatient_Click;
            // 
            // btnNavPrescription
            // 
            btnNavPrescription.Dock = DockStyle.Top;
            btnNavPrescription.FlatAppearance.BorderSize = 0;
            btnNavPrescription.FlatAppearance.MouseDownBackColor = Color.FromArgb(50, 50, 58);
            btnNavPrescription.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 46);
            btnNavPrescription.FlatStyle = FlatStyle.Flat;
            btnNavPrescription.ForeColor = Color.FromArgb(190, 190, 200);
            btnNavPrescription.Location = new Point(0, 135);
            btnNavPrescription.Name = "btnNavPrescription";
            btnNavPrescription.Padding = new Padding(20, 0, 0, 0);
            btnNavPrescription.Size = new Size(230, 55);
            btnNavPrescription.TabIndex = 2;
            btnNavPrescription.Text = "Đơn thuốc";
            btnNavPrescription.TextAlign = ContentAlignment.MiddleLeft;
            btnNavPrescription.UseVisualStyleBackColor = true;
            btnNavPrescription.Click += btnNavPrescription_Click;
            // 
            // btnNavHSBA
            // 
            btnNavHSBA.Dock = DockStyle.Top;
            btnNavHSBA.FlatAppearance.BorderSize = 0;
            btnNavHSBA.FlatAppearance.MouseDownBackColor = Color.FromArgb(50, 50, 58);
            btnNavHSBA.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 46);
            btnNavHSBA.FlatStyle = FlatStyle.Flat;
            btnNavHSBA.ForeColor = Color.FromArgb(190, 190, 200);
            btnNavHSBA.Location = new Point(0, 80);
            btnNavHSBA.Name = "btnNavHSBA";
            btnNavHSBA.Padding = new Padding(20, 0, 0, 0);
            btnNavHSBA.Size = new Size(230, 55);
            btnNavHSBA.TabIndex = 1;
            btnNavHSBA.Text = "Quản lý HSBA";
            btnNavHSBA.TextAlign = ContentAlignment.MiddleLeft;
            btnNavHSBA.UseVisualStyleBackColor = true;
            btnNavHSBA.Click += btnNavHSBA_Click;
            // 
            // lblLogo
            // 
            lblLogo.Dock = DockStyle.Top;
            lblLogo.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblLogo.ForeColor = Color.Orange;
            lblLogo.Location = new Point(0, 0);
            lblLogo.Name = "lblLogo";
            lblLogo.Size = new Size(230, 80);
            lblLogo.TabIndex = 0;
            lblLogo.Text = "HOSPITAL DOCTOR";
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.White;
            pnlTop.Controls.Add(lblUserInfo);
            pnlTop.Controls.Add(lblBreadcrumb);
            pnlTop.Controls.Add(lblPageTitle);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(230, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(1070, 100);
            pnlTop.TabIndex = 1;
            pnlTop.Resize += pnlTop_Resize;
            // 
            // lblUserInfo
            // 
            lblUserInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblUserInfo.AutoSize = true;
            lblUserInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblUserInfo.ForeColor = Color.Orange;
            lblUserInfo.Location = new Point(882, 35);
            lblUserInfo.Name = "lblUserInfo";
            lblUserInfo.Size = new Size(154, 23);
            lblUserInfo.TabIndex = 2;
            lblUserInfo.Text = "Bác sĩ: {username}";
            // 
            // lblBreadcrumb
            // 
            lblBreadcrumb.AutoSize = true;
            lblBreadcrumb.ForeColor = Color.Gray;
            lblBreadcrumb.Location = new Point(25, 55);
            lblBreadcrumb.Name = "lblBreadcrumb";
            lblBreadcrumb.Size = new Size(160, 20);
            lblBreadcrumb.TabIndex = 1;
            lblBreadcrumb.Text = "Dashboard / Trang chủ";
            // 
            // lblPageTitle
            // 
            lblPageTitle.AutoSize = true;
            lblPageTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblPageTitle.Location = new Point(25, 20);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.Size = new Size(138, 32);
            lblPageTitle.TabIndex = 0;
            lblPageTitle.Text = "Dashboard";
            // 
            // pnlContent
            // 
            pnlContent.BackColor = Color.FromArgb(245, 245, 250);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(230, 100);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(1070, 750);
            pnlContent.TabIndex = 2;
            // 
            // frmDoctor
            // 
            ClientSize = new Size(1300, 850);
            Controls.Add(pnlContent);
            Controls.Add(pnlTop);
            Controls.Add(pnlSidebar);
            Name = "frmDoctor";
            Text = "Hospital Management - Doctor";
            Load += frmDoctor_Load;
            pnlSidebar.ResumeLayout(false);
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Button btnNavHSBA;
        private System.Windows.Forms.Button btnNavPrescription;
        private System.Windows.Forms.Button btnNavPatient;
        private System.Windows.Forms.Button btnNavProfile;
        private System.Windows.Forms.Button btnNavNotifications;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Label lblBreadcrumb;
        private System.Windows.Forms.Label lblUserInfo;
        private System.Windows.Forms.Panel pnlContent;
    }
}
