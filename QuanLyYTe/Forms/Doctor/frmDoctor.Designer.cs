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
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.btnNavHSBA = new System.Windows.Forms.Button();
            this.btnNavPrescription = new System.Windows.Forms.Button();
            this.btnNavPatient = new System.Windows.Forms.Button();
            this.btnNavProfile = new System.Windows.Forms.Button();
            this.btnNavNotifications = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.lblBreadcrumb = new System.Windows.Forms.Label();
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlSidebar.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pnlSidebar.Controls.Add(this.btnLogout);
            this.pnlSidebar.Controls.Add(this.btnNavNotifications);
            this.pnlSidebar.Controls.Add(this.btnNavProfile);
            this.pnlSidebar.Controls.Add(this.btnNavPatient);
            this.pnlSidebar.Controls.Add(this.btnNavPrescription);
            this.pnlSidebar.Controls.Add(this.btnNavHSBA);
            this.pnlSidebar.Controls.Add(this.lblLogo);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(230, 850);
            this.pnlSidebar.TabIndex = 0;
            // 
            // lblLogo
            // 
            this.lblLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(40)))));
            this.lblLogo.Location = new System.Drawing.Point(0, 0);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(230, 80);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "HOSPITAL DOCTOR";
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNavHSBA
            // 
            this.btnNavHSBA.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavHSBA.FlatAppearance.BorderSize = 0;
            this.btnNavHSBA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavHSBA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.btnNavHSBA.Location = new System.Drawing.Point(0, 80);
            this.btnNavHSBA.Name = "btnNavHSBA";
            this.btnNavHSBA.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnNavHSBA.Size = new System.Drawing.Size(230, 55);
            this.btnNavHSBA.TabIndex = 1;
            this.btnNavHSBA.Text = "Quản lý HSBA";
            this.btnNavHSBA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavHSBA.UseVisualStyleBackColor = true;
            this.btnNavHSBA.Click += new System.EventHandler(this.btnNavHSBA_Click);
            // 
            // btnNavPrescription
            // 
            this.btnNavPrescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavPrescription.FlatAppearance.BorderSize = 0;
            this.btnNavPrescription.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavPrescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.btnNavPrescription.Location = new System.Drawing.Point(0, 135);
            this.btnNavPrescription.Name = "btnNavPrescription";
            this.btnNavPrescription.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnNavPrescription.Size = new System.Drawing.Size(230, 55);
            this.btnNavPrescription.TabIndex = 2;
            this.btnNavPrescription.Text = "Đơn thuốc";
            this.btnNavPrescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavPrescription.UseVisualStyleBackColor = true;
            this.btnNavPrescription.Click += new System.EventHandler(this.btnNavPrescription_Click);
            // 
            // btnNavPatient
            // 
            this.btnNavPatient.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavPatient.FlatAppearance.BorderSize = 0;
            this.btnNavPatient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavPatient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.btnNavPatient.Location = new System.Drawing.Point(0, 190);
            this.btnNavPatient.Name = "btnNavPatient";
            this.btnNavPatient.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnNavPatient.Size = new System.Drawing.Size(230, 55);
            this.btnNavPatient.TabIndex = 3;
            this.btnNavPatient.Text = "Bệnh nhân";
            this.btnNavPatient.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavPatient.UseVisualStyleBackColor = true;
            this.btnNavPatient.Click += new System.EventHandler(this.btnNavPatient_Click);
            // 
            // btnNavProfile
            // 
            this.btnNavProfile.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavProfile.FlatAppearance.BorderSize = 0;
            this.btnNavProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavProfile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.btnNavProfile.Location = new System.Drawing.Point(0, 245);
            this.btnNavProfile.Name = "btnNavProfile";
            this.btnNavProfile.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnNavProfile.Size = new System.Drawing.Size(230, 55);
            this.btnNavProfile.TabIndex = 4;
            this.btnNavProfile.Text = "Hồ sơ cá nhân";
            this.btnNavProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavProfile.UseVisualStyleBackColor = true;
            this.btnNavProfile.Click += new System.EventHandler(this.btnNavProfile_Click);
            // 
            // btnNavNotifications
            // 
            this.btnNavNotifications.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNavNotifications.FlatAppearance.BorderSize = 0;
            this.btnNavNotifications.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavNotifications.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.btnNavNotifications.Location = new System.Drawing.Point(0, 300);
            this.btnNavNotifications.Name = "btnNavNotifications";
            this.btnNavNotifications.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnNavNotifications.Size = new System.Drawing.Size(230, 55);
            this.btnNavNotifications.TabIndex = 5;
            this.btnNavNotifications.Text = "Thông báo OLS";
            this.btnNavNotifications.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavNotifications.UseVisualStyleBackColor = true;
            this.btnNavNotifications.Click += new System.EventHandler(this.btnNavNotifications_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnLogout.Location = new System.Drawing.Point(0, 355);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnLogout.Size = new System.Drawing.Size(230, 55);
            this.btnLogout.TabIndex = 6;
            this.btnLogout.Text = "Đăng xuất";
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.lblUserInfo);
            this.pnlTop.Controls.Add(this.lblBreadcrumb);
            this.pnlTop.Controls.Add(this.lblPageTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(230, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1070, 100);
            this.pnlTop.TabIndex = 1;
            this.pnlTop.Resize += new System.EventHandler(this.pnlTop_Resize);
            // 
            // lblPageTitle
            // 
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblPageTitle.Location = new System.Drawing.Point(25, 20);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(109, 25);
            this.lblPageTitle.TabIndex = 0;
            this.lblPageTitle.Text = "Dashboard";
            // 
            // lblBreadcrumb
            // 
            this.lblBreadcrumb.AutoSize = true;
            this.lblBreadcrumb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(170)))));
            this.lblBreadcrumb.Location = new System.Drawing.Point(25, 55);
            this.lblBreadcrumb.Name = "lblBreadcrumb";
            this.lblBreadcrumb.Size = new System.Drawing.Size(126, 13);
            this.lblBreadcrumb.TabIndex = 1;
            this.lblBreadcrumb.Text = "Dashboard / Trang chủ";
            // 
            // lblUserInfo
            // 
            this.lblUserInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserInfo.AutoSize = true;
            this.lblUserInfo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUserInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(40)))));
            this.lblUserInfo.Location = new System.Drawing.Point(882, 35);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(163, 19);
            this.lblUserInfo.TabIndex = 2;
            this.lblUserInfo.Text = "Bác sĩ: {username}";
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(230, 100);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1070, 750);
            this.pnlContent.TabIndex = 2;
            // 
            // frmDoctor
            // 
            this.ClientSize = new System.Drawing.Size(1300, 850);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlSidebar);
            this.Name = "frmDoctor";
            this.Text = "Hospital Management - Doctor";
            this.Load += new System.EventHandler(this.frmDoctor_Load);
            this.pnlSidebar.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);

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
