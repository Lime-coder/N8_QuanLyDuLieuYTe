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
            // ── Controls ──────────────────────────────────────────────
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pnlLogo = new System.Windows.Forms.Panel();
            this.lblAppTitle = new System.Windows.Forms.Label();
            this.lblAppSub = new System.Windows.Forms.Label();
            this.pnlVersion = new System.Windows.Forms.Panel();
            this.lblVersion = new System.Windows.Forms.Label();

            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlDivider = new System.Windows.Forms.Panel();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.pnlCard = new System.Windows.Forms.Panel();

            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();

            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();

            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();

            this.lblDataSource = new System.Windows.Forms.Label();
            this.txtDataSource = new System.Windows.Forms.TextBox();

            this.lblError = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();

            this.pnlSidebar.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlCard.SuspendLayout();
            this.SuspendLayout();

            // ── Sidebar ───────────────────────────────────────────────
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(24, 24, 28);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Width = 220;
            this.pnlSidebar.Controls.Add(this.pnlVersion);
            this.pnlSidebar.Controls.Add(this.pnlLogo);

            // Logo
            this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogo.Height = 80;
            this.pnlLogo.BackColor = System.Drawing.Color.FromArgb(30, 30, 35);
            this.pnlLogo.Controls.Add(this.lblAppSub);
            this.pnlLogo.Controls.Add(this.lblAppTitle);

            this.lblAppTitle.Text = "OracleAdmin";
            this.lblAppTitle.Font = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold);
            this.lblAppTitle.ForeColor = System.Drawing.Color.FromArgb(255, 140, 40);
            this.lblAppTitle.AutoSize = true;
            this.lblAppTitle.Location = new System.Drawing.Point(20, 16);

            this.lblAppSub.Text = "DBA Management Console";
            this.lblAppSub.Font = new System.Drawing.Font("Segoe UI", 7.5f);
            this.lblAppSub.ForeColor = System.Drawing.Color.FromArgb(120, 120, 130);
            this.lblAppSub.AutoSize = true;
            this.lblAppSub.Location = new System.Drawing.Point(22, 44);

            // Version footer
            this.pnlVersion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlVersion.Height = 40;
            this.pnlVersion.BackColor = System.Drawing.Color.FromArgb(24, 24, 28);
            this.pnlVersion.Controls.Add(this.lblVersion);

            this.lblVersion.Text = "Phân Hệ 1 · v1.0";
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 7.5f);
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(80, 80, 90);
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(20, 12);

            // ── Main ──────────────────────────────────────────────────
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(245, 244, 242);
            this.pnlMain.Controls.Add(this.pnlCenter);
            this.pnlMain.Controls.Add(this.pnlDivider);

            // Orange top stripe
            this.pnlDivider.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDivider.Height = 3;
            this.pnlDivider.BackColor = System.Drawing.Color.FromArgb(255, 140, 40);

            // Center wrapper (fills remaining space, centers the card)
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.BackColor = System.Drawing.Color.Transparent;
            this.pnlCenter.Controls.Add(this.pnlCard);
            this.pnlCenter.Resize += new System.EventHandler(this.pnlCenter_Resize);

            // ── Login Card ────────────────────────────────────────────
            this.pnlCard.Size = new System.Drawing.Size(310, 340);
            this.pnlCard.BackColor = System.Drawing.Color.White;
            this.pnlCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCard.Controls.Add(this.btnConnect);
            this.pnlCard.Controls.Add(this.lblError);
            this.pnlCard.Controls.Add(this.txtDataSource);
            this.pnlCard.Controls.Add(this.lblDataSource);
            this.pnlCard.Controls.Add(this.txtPassword);
            this.pnlCard.Controls.Add(this.lblPassword);
            this.pnlCard.Controls.Add(this.txtUsername);
            this.pnlCard.Controls.Add(this.lblUsername);
            this.pnlCard.Controls.Add(this.lblSubtitle);
            this.pnlCard.Controls.Add(this.lblTitle);
            this.pnlCard.Padding = new System.Windows.Forms.Padding(28);

            int x = 28, y = 24, w = 254;

            // Title
            this.lblTitle.Text = "Đăng nhập";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15f, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(28, 28, 32);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(x, y);

            // Subtitle
            this.lblSubtitle.Text = "Kết nối trực tiếp Oracle Database";
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 8.5f);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(140, 140, 150);
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Location = new System.Drawing.Point(x, y + 32);

            // Username
            SetupFieldLabel(this.lblUsername, "Tên người dùng", x, y + 68);
            SetupTextBox(this.txtUsername, "sys", x, y + 84, w);

            // Password
            SetupFieldLabel(this.lblPassword, "Mật khẩu", x, y + 120);
            SetupTextBox(this.txtPassword, "", x, y + 136, w);
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);

            // Data Source
            SetupFieldLabel(this.lblDataSource, "Data Source (TNS / Host:Port/SID)", x, y + 172);
            SetupTextBox(this.txtDataSource, "localhost:1521/ORCLPDB1", x, y + 188, w);

            // Error label
            this.lblError.Text = "";
            this.lblError.Font = new System.Drawing.Font("Segoe UI", 8f);
            this.lblError.ForeColor = System.Drawing.Color.FromArgb(180, 40, 40);
            this.lblError.AutoSize = false;
            this.lblError.Size = new System.Drawing.Size(w, 28);
            this.lblError.Location = new System.Drawing.Point(x, y + 224);
            this.lblError.Visible = false;

            // Connect button
            this.btnConnect.Text = "Kết nối";
            this.btnConnect.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
            this.btnConnect.ForeColor = System.Drawing.Color.White;
            this.btnConnect.BackColor = System.Drawing.Color.FromArgb(255, 140, 40);
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.FlatAppearance.BorderSize = 0;
            this.btnConnect.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(230, 120, 20);
            this.btnConnect.Size = new System.Drawing.Size(w, 38);
            this.btnConnect.Location = new System.Drawing.Point(x, y + 258);
            this.btnConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);

            // ── Form root ─────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 420);
            this.MinimumSize = new System.Drawing.Size(580, 380);
            this.Text = "Oracle DBA — Đăng nhập";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Font = new System.Drawing.Font("Segoe UI", 9f);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlSidebar);

            this.pnlSidebar.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlCard.ResumeLayout(false);
            this.pnlCard.PerformLayout();
            this.ResumeLayout(false);
        }

        private void SetupFieldLabel(System.Windows.Forms.Label lbl, string text, int x, int y)
        {
            lbl.Text = text;
            lbl.Font = new System.Drawing.Font("Segoe UI", 8.5f);
            lbl.ForeColor = System.Drawing.Color.FromArgb(100, 100, 110);
            lbl.AutoSize = true;
            lbl.Location = new System.Drawing.Point(x, y);
        }

        private void SetupTextBox(System.Windows.Forms.TextBox txt, string placeholder, int x, int y, int w)
        {
            txt.Font = new System.Drawing.Font("Segoe UI", 9.5f);
            txt.Text = placeholder;
            txt.Size = new System.Drawing.Size(w, 28);
            txt.Location = new System.Drawing.Point(x, y);
            txt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txt.BackColor = System.Drawing.Color.FromArgb(250, 249, 248);
        }

        // ── Declarations ──────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlSidebar, pnlLogo, pnlVersion;
        private System.Windows.Forms.Panel pnlMain, pnlDivider, pnlCenter, pnlCard;
        private System.Windows.Forms.Label lblAppTitle, lblAppSub, lblVersion;
        private System.Windows.Forms.Label lblTitle, lblSubtitle;
        private System.Windows.Forms.Label lblUsername, lblPassword, lblDataSource, lblError;
        private System.Windows.Forms.TextBox txtUsername, txtPassword, txtDataSource;
        private System.Windows.Forms.Button btnConnect;
    }
}