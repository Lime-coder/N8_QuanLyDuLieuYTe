namespace QuanLyYTe.Forms
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void pnlCenter_Resize(object sender, System.EventArgs e)
        {
            this.pnlCard.Location = new System.Drawing.Point(
                (this.pnlCenter.Width - this.pnlCard.Width) / 2,
                (this.pnlCenter.Height - this.pnlCard.Height) / 2);
        }

        private void InitializeComponent()
        {
            pnlSidebar = new Panel();
            pnlVersion = new Panel();
            lblVersion = new Label();
            pnlLogo = new Panel();
            lblAppSub = new Label();
            lblAppTitle = new Label();
            pnlMain = new Panel();
            pnlCenter = new Panel();
            pnlCard = new Panel();
            btnConnect = new Button();
            lblError = new Label();
            txtPassword = new TextBox();
            lblPassword = new Label();
            txtUsername = new TextBox();
            lblUsername = new Label();
            lblSubtitle = new Label();
            lblTitle = new Label();
            pnlDivider = new Panel();
            pnlSidebar.SuspendLayout();
            pnlVersion.SuspendLayout();
            pnlLogo.SuspendLayout();
            pnlMain.SuspendLayout();
            pnlCenter.SuspendLayout();
            pnlCard.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSidebar
            // 
            pnlSidebar.BackColor = Color.FromArgb(24, 24, 28);
            pnlSidebar.Controls.Add(pnlVersion);
            pnlSidebar.Controls.Add(pnlLogo);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 0);
            pnlSidebar.Margin = new Padding(3, 4, 3, 4);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new Size(274, 667);
            pnlSidebar.TabIndex = 1;
            // 
            // pnlVersion
            // 
            pnlVersion.BackColor = Color.FromArgb(24, 24, 28);
            pnlVersion.Controls.Add(lblVersion);
            pnlVersion.Dock = DockStyle.Bottom;
            pnlVersion.Location = new Point(0, 608);
            pnlVersion.Margin = new Padding(3, 4, 3, 4);
            pnlVersion.Name = "pnlVersion";
            pnlVersion.Size = new Size(274, 59);
            pnlVersion.TabIndex = 0;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Font = new Font("Segoe UI", 7.5F);
            lblVersion.ForeColor = Color.FromArgb(80, 80, 90);
            lblVersion.Location = new Point(23, 19);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(101, 17);
            lblVersion.TabIndex = 0;
            lblVersion.Text = "Phân Hệ 1 · v1.0";
            // 
            // pnlLogo
            // 
            pnlLogo.BackColor = Color.FromArgb(30, 30, 35);
            pnlLogo.Controls.Add(lblAppSub);
            pnlLogo.Controls.Add(lblAppTitle);
            pnlLogo.Dock = DockStyle.Top;
            pnlLogo.Location = new Point(0, 0);
            pnlLogo.Margin = new Padding(3, 4, 3, 4);
            pnlLogo.Name = "pnlLogo";
            pnlLogo.Size = new Size(274, 117);
            pnlLogo.TabIndex = 1;
            // 
            // lblAppSub
            // 
            lblAppSub.AutoSize = true;
            lblAppSub.Font = new Font("Segoe UI", 7.5F);
            lblAppSub.ForeColor = Color.FromArgb(120, 120, 130);
            lblAppSub.Location = new Point(25, 69);
            lblAppSub.Name = "lblAppSub";
            lblAppSub.Size = new Size(164, 17);
            lblAppSub.TabIndex = 0;
            lblAppSub.Text = "DBA Management Console";
            // 
            // lblAppTitle
            // 
            lblAppTitle.AutoSize = true;
            lblAppTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblAppTitle.ForeColor = Color.FromArgb(255, 140, 40);
            lblAppTitle.Location = new Point(23, 24);
            lblAppTitle.Name = "lblAppTitle";
            lblAppTitle.Size = new Size(163, 32);
            lblAppTitle.TabIndex = 1;
            lblAppTitle.Text = "HospitalAdmin";
            // 
            // pnlMain
            // 
            pnlMain.BackColor = Color.FromArgb(245, 244, 242);
            pnlMain.Controls.Add(pnlCenter);
            pnlMain.Controls.Add(pnlDivider);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(274, 0);
            pnlMain.Margin = new Padding(3, 4, 3, 4);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(572, 667);
            pnlMain.TabIndex = 0;
            // 
            // pnlCenter
            // 
            pnlCenter.BackColor = Color.Transparent;
            pnlCenter.Controls.Add(pnlCard);
            pnlCenter.Dock = DockStyle.Fill;
            pnlCenter.Location = new Point(0, 4);
            pnlCenter.Margin = new Padding(3, 4, 3, 4);
            pnlCenter.Name = "pnlCenter";
            pnlCenter.Size = new Size(572, 663);
            pnlCenter.TabIndex = 0;
            pnlCenter.Resize += pnlCenter_Resize;
            // 
            // pnlCard
            // 
            pnlCard.BackColor = Color.White;
            pnlCard.BorderStyle = BorderStyle.FixedSingle;
            pnlCard.Controls.Add(btnConnect);
            pnlCard.Controls.Add(lblError);
            pnlCard.Controls.Add(txtPassword);
            pnlCard.Controls.Add(lblPassword);
            pnlCard.Controls.Add(txtUsername);
            pnlCard.Controls.Add(lblUsername);
            pnlCard.Controls.Add(lblSubtitle);
            pnlCard.Controls.Add(lblTitle);
            pnlCard.Location = new Point(0, 0);
            pnlCard.Margin = new Padding(3, 4, 3, 4);
            pnlCard.Name = "pnlCard";
            pnlCard.Size = new Size(365, 370);
            pnlCard.TabIndex = 0;
            // 
            // btnConnect
            // 
            btnConnect.BackColor = Color.FromArgb(255, 140, 40);
            btnConnect.Cursor = Cursors.Hand;
            btnConnect.FlatAppearance.BorderSize = 0;
            btnConnect.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 120, 20);
            btnConnect.FlatStyle = FlatStyle.Flat;
            btnConnect.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnConnect.ForeColor = Color.White;
            btnConnect.Location = new Point(32, 299);
            btnConnect.Margin = new Padding(3, 4, 3, 4);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(302, 51);
            btnConnect.TabIndex = 3;
            btnConnect.Text = "Kết nối";
            btnConnect.UseVisualStyleBackColor = false;
            btnConnect.Click += btnConnect_Click;
            // 
            // lblError
            // 
            lblError.Font = new Font("Segoe UI", 8F);
            lblError.ForeColor = Color.FromArgb(180, 40, 40);
            lblError.Location = new Point(32, 264);
            lblError.Name = "lblError";
            lblError.Size = new Size(302, 27);
            lblError.TabIndex = 4;
            lblError.Visible = false;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.FromArgb(250, 249, 248);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Segoe UI", 9.5F);
            txtPassword.Location = new Point(32, 219);
            txtPassword.Margin = new Padding(3, 4, 3, 4);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(301, 29);
            txtPassword.TabIndex = 2;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.KeyDown += txtPassword_KeyDown;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI", 8.5F);
            lblPassword.ForeColor = Color.FromArgb(100, 100, 110);
            lblPassword.Location = new Point(32, 195);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(70, 20);
            lblPassword.TabIndex = 5;
            lblPassword.Text = "Mật khẩu";
            // 
            // txtUsername
            // 
            txtUsername.BackColor = Color.FromArgb(250, 249, 248);
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Font = new Font("Segoe UI", 9.5F);
            txtUsername.Location = new Point(32, 144);
            txtUsername.Margin = new Padding(3, 4, 3, 4);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(301, 29);
            txtUsername.TabIndex = 1;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI", 8.5F);
            lblUsername.ForeColor = Color.FromArgb(100, 100, 110);
            lblUsername.Location = new Point(32, 120);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(113, 20);
            lblUsername.TabIndex = 6;
            lblUsername.Text = "Tên người dùng";
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 8.5F);
            lblSubtitle.ForeColor = Color.FromArgb(140, 140, 150);
            lblSubtitle.Location = new Point(32, 77);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(170, 20);
            lblSubtitle.TabIndex = 7;
            lblSubtitle.Text = "Kết nối Hospital Database";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(28, 28, 32);
            lblTitle.Location = new Point(32, 32);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(142, 35);
            lblTitle.TabIndex = 8;
            lblTitle.Text = "Đăng nhập";
            // 
            // pnlDivider
            // 
            pnlDivider.BackColor = Color.FromArgb(255, 140, 40);
            pnlDivider.Dock = DockStyle.Top;
            pnlDivider.Location = new Point(0, 0);
            pnlDivider.Margin = new Padding(3, 4, 3, 4);
            pnlDivider.Name = "pnlDivider";
            pnlDivider.Size = new Size(572, 4);
            pnlDivider.TabIndex = 1;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(846, 667);
            Controls.Add(pnlMain);
            Controls.Add(pnlSidebar);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimumSize = new Size(683, 571);
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "HospitalAdmin — Đăng nhập";
            Load += new System.EventHandler(this.LoginForm_Load);
            pnlSidebar.ResumeLayout(false);
            pnlVersion.ResumeLayout(false);
            pnlVersion.PerformLayout();
            pnlLogo.ResumeLayout(false);
            pnlLogo.PerformLayout();
            pnlMain.ResumeLayout(false);
            pnlCenter.ResumeLayout(false);
            pnlCard.ResumeLayout(false);
            pnlCard.PerformLayout();
            ResumeLayout(false);
        }

        // ── Declarations ─────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlSidebar, pnlLogo, pnlVersion;
        private System.Windows.Forms.Panel pnlMain, pnlDivider, pnlCenter, pnlCard;
        private System.Windows.Forms.Label lblAppTitle, lblAppSub, lblVersion;
        private System.Windows.Forms.Label lblTitle, lblSubtitle;
        private System.Windows.Forms.Label lblUsername, lblPassword, lblError;
        private System.Windows.Forms.TextBox txtUsername, txtPassword;
        private System.Windows.Forms.Button btnConnect;
    }
}