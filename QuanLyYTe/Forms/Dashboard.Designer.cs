namespace QuanLyYTe.Forms
{
    partial class Dashboard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            // ── Instantiate all controls ──────────────────────────────
            pnlSidebar = new System.Windows.Forms.Panel();
            pnlSidebarFooter = new System.Windows.Forms.Panel();
            lblVersion = new System.Windows.Forms.Label();
            pnlNavItems = new System.Windows.Forms.Panel();
            btnNavUsers = new System.Windows.Forms.Button();
            // btnNavRoles = new System.Windows.Forms.Button();
            btnNavGrant = new System.Windows.Forms.Button();
            btnNavRevoke = new System.Windows.Forms.Button();
            // btnNavPermView = new System.Windows.Forms.Button();
            pnlLogo = new System.Windows.Forms.Panel();
            lblAppSubtitle = new System.Windows.Forms.Label();
            lblAppTitle = new System.Windows.Forms.Label();
            pnlMain = new System.Windows.Forms.Panel();
            pnlContent = new System.Windows.Forms.Panel();
            pnlCards = new System.Windows.Forms.FlowLayoutPanel();
            pnlWelcome = new System.Windows.Forms.Panel();
            lblWelcomeDesc = new System.Windows.Forms.Label();
            lblWelcomeTitle = new System.Windows.Forms.Label();
            pnlDivider = new System.Windows.Forms.Panel();
            pnlTopbar = new System.Windows.Forms.Panel();
            lblPageBreadcrumb = new System.Windows.Forms.Label();
            lblPageTitle = new System.Windows.Forms.Label();
            lblUserInfo = new System.Windows.Forms.Label();

            pnlSidebar.SuspendLayout();
            pnlSidebarFooter.SuspendLayout();
            pnlNavItems.SuspendLayout();
            pnlLogo.SuspendLayout();
            pnlMain.SuspendLayout();
            pnlContent.SuspendLayout();
            pnlWelcome.SuspendLayout();
            pnlTopbar.SuspendLayout();
            SuspendLayout();

            // ── Sidebar ───────────────────────────────────────────────
            pnlSidebar.BackColor = System.Drawing.Color.FromArgb(24, 24, 28);
            pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            pnlSidebar.Width = 240;
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.TabIndex = 1;
            // Add order: Footer(Bottom) first, then NavItems(Top), then Logo(Top)
            // WinForms dock: last-added Top control renders topmost
            pnlSidebar.Controls.Add(pnlSidebarFooter);
            pnlSidebar.Controls.Add(pnlNavItems);
            pnlSidebar.Controls.Add(pnlLogo);

            // ── Sidebar Footer ────────────────────────────────────────
            pnlSidebarFooter.BackColor = System.Drawing.Color.FromArgb(24, 24, 28);
            pnlSidebarFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlSidebarFooter.Height = 44;
            pnlSidebarFooter.Name = "pnlSidebarFooter";
            pnlSidebarFooter.TabIndex = 0;
            pnlSidebarFooter.Controls.Add(lblVersion);

            lblVersion.Text = "Phân Hệ 1 · v1.0";
            lblVersion.Font = new System.Drawing.Font("Segoe UI", 7.5f);
            lblVersion.ForeColor = System.Drawing.Color.FromArgb(80, 80, 90);
            lblVersion.AutoSize = true;
            lblVersion.Location = new System.Drawing.Point(20, 14);
            lblVersion.Name = "lblVersion";
            lblVersion.TabIndex = 0;

            // ── Nav Items ─────────────────────────────────────────────
            pnlNavItems.BackColor = System.Drawing.Color.FromArgb(24, 24, 28);
            pnlNavItems.Dock = System.Windows.Forms.DockStyle.Top;
            pnlNavItems.Height = 332;
            pnlNavItems.Name = "pnlNavItems";
            pnlNavItems.TabIndex = 1;
            pnlNavItems.Controls.Add(btnNavUsers);
            // pnlNavItems.Controls.Add(btnNavRoles);
            pnlNavItems.Controls.Add(btnNavGrant);
            pnlNavItems.Controls.Add(btnNavRevoke);
            // pnlNavItems.Controls.Add(btnNavPermView);

            // Setup each nav button with proper position/style
            SetupNavButton(btnNavUsers, "  Quản lý User", 0);
            // SetupNavButton(btnNavRoles, "  Quản lý Role", 1);
            SetupNavButton(btnNavGrant, "  Cấp Quyền", 1);
            SetupNavButton(btnNavRevoke, "  Xem và thu Hồi Quyền", 2);
            // SetupNavButton(btnNavPermView, "  Xem Quyền", 4);

            btnNavUsers.Name = "btnNavUsers"; btnNavUsers.TabIndex = 0;
            // btnNavRoles.Name = "btnNavRoles"; btnNavRoles.TabIndex = 1;
            btnNavGrant.Name = "btnNavGrant"; btnNavGrant.TabIndex = 2;
            btnNavRevoke.Name = "btnNavRevoke"; btnNavRevoke.TabIndex = 3;
            // btnNavPermView.Name = "btnNavPermView"; btnNavPermView.TabIndex = 4;

            btnLogout = new System.Windows.Forms.Button();
            pnlNavItems.Controls.Add(btnLogout);
            btnLogout.Text = "  Đăng xuất";
            btnLogout.Font = new System.Drawing.Font("Segoe UI", 9.5f);
            btnLogout.ForeColor = System.Drawing.Color.FromArgb(220, 80, 80);
            btnLogout.BackColor = System.Drawing.Color.Transparent;
            btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(60, 220, 80, 80);
            btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnLogout.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            btnLogout.Size = new System.Drawing.Size(216, 46);
            btnLogout.Location = new System.Drawing.Point(12, 10 + 3 * 52); // index 3, below revoke
            btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            btnLogout.Name = "btnLogout";
            btnLogout.TabIndex = 3;
            btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // ── Logo ──────────────────────────────────────────────────
            pnlLogo.BackColor = System.Drawing.Color.FromArgb(30, 30, 35);
            pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            pnlLogo.Height = 88;
            pnlLogo.Name = "pnlLogo";
            pnlLogo.TabIndex = 2;
            pnlLogo.Controls.Add(lblAppSubtitle);
            pnlLogo.Controls.Add(lblAppTitle);

            lblAppTitle.Text = "HospitalAdmin";
            lblAppTitle.Font = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold);
            lblAppTitle.ForeColor = System.Drawing.Color.FromArgb(255, 140, 40);
            lblAppTitle.AutoSize = true;
            lblAppTitle.Location = new System.Drawing.Point(20, 18);
            lblAppTitle.Name = "lblAppTitle";
            lblAppTitle.TabIndex = 1;
            lblAppTitle.Cursor = System.Windows.Forms.Cursors.Hand;
            lblAppTitle.Click += new System.EventHandler(this.lblAppTitle_Click);

            lblAppSubtitle.Text = "DBA Management Console";
            lblAppSubtitle.Font = new System.Drawing.Font("Segoe UI", 7.5f);
            lblAppSubtitle.ForeColor = System.Drawing.Color.FromArgb(120, 120, 130);
            lblAppSubtitle.AutoSize = true;
            lblAppSubtitle.Location = new System.Drawing.Point(22, 52);
            lblAppSubtitle.Name = "lblAppSubtitle";
            lblAppSubtitle.TabIndex = 0;

            // ── Main Panel ────────────────────────────────────────────
            // Add order: Content(Fill) first, Divider(Top), Topbar(Top) last
            pnlMain.BackColor = System.Drawing.Color.FromArgb(245, 244, 242);
            pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlMain.Name = "pnlMain";
            pnlMain.TabIndex = 0;
            pnlMain.Controls.Add(pnlContent);
            pnlMain.Controls.Add(pnlDivider);
            pnlMain.Controls.Add(pnlTopbar);

            // ── Topbar ────────────────────────────────────────────────
            pnlTopbar.BackColor = System.Drawing.Color.White;
            pnlTopbar.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTopbar.Height = 68;
            pnlTopbar.Name = "pnlTopbar";
            pnlTopbar.TabIndex = 2;
            // Add lblUserInfo BEFORE ResumeLayout
            pnlTopbar.Controls.Add(lblPageBreadcrumb);
            pnlTopbar.Controls.Add(lblPageTitle);
            pnlTopbar.Controls.Add(lblUserInfo);

            lblPageTitle.Text = "Dashboard";
            lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 15f, System.Drawing.FontStyle.Bold);
            lblPageTitle.ForeColor = System.Drawing.Color.FromArgb(28, 28, 32);
            lblPageTitle.AutoSize = true;
            lblPageTitle.Location = new System.Drawing.Point(28, 10);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.TabIndex = 1;

            lblPageBreadcrumb.Text = "Trang chủ";
            lblPageBreadcrumb.Font = new System.Drawing.Font("Segoe UI", 8.5f);
            lblPageBreadcrumb.ForeColor = System.Drawing.Color.FromArgb(160, 160, 170);
            lblPageBreadcrumb.AutoSize = true;
            lblPageBreadcrumb.Location = new System.Drawing.Point(30, 42);
            lblPageBreadcrumb.Name = "lblPageBreadcrumb";
            lblPageBreadcrumb.TabIndex = 0;

            // lblUserInfo — right side of topbar, positioned at runtime in Dashboard_Load
            lblUserInfo.AutoSize = false;
            lblUserInfo.Size = new System.Drawing.Size(240, 20);
            lblUserInfo.Font = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Bold);
            lblUserInfo.ForeColor = System.Drawing.Color.FromArgb(255, 140, 40);
            lblUserInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            lblUserInfo.Anchor = System.Windows.Forms.AnchorStyles.Top
                               | System.Windows.Forms.AnchorStyles.Right;
            lblUserInfo.Name = "lblUserInfo";
            lblUserInfo.TabIndex = 3;
            // X position set in Dashboard_Load once layout is complete

            // ── Orange divider ────────────────────────────────────────
            pnlDivider.BackColor = System.Drawing.Color.FromArgb(255, 140, 40);
            pnlDivider.Dock = System.Windows.Forms.DockStyle.Top;
            pnlDivider.Height = 3;
            pnlDivider.Name = "pnlDivider";
            pnlDivider.TabIndex = 1;

            // ── Content Panel ─────────────────────────────────────────
            // Add order: Cards(Top/AutoSize) first, Welcome(Top) last
            pnlContent.AutoScroll = false;
            pnlContent.BackColor = System.Drawing.Color.FromArgb(245, 244, 242);
            pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlContent.Padding = new System.Windows.Forms.Padding(24, 20, 24, 24);
            pnlContent.Name = "pnlContent";
            pnlContent.TabIndex = 0;
            pnlContent.Controls.Add(pnlCards);
            pnlContent.Controls.Add(pnlWelcome);

            // ── Welcome Banner ────────────────────────────────────────
            pnlWelcome.BackColor = System.Drawing.Color.Transparent;
            pnlWelcome.Dock = System.Windows.Forms.DockStyle.Top;
            pnlWelcome.Height = 80;
            pnlWelcome.Name = "pnlWelcome";
            pnlWelcome.TabIndex = 1;
            pnlWelcome.Controls.Add(lblWelcomeDesc);
            pnlWelcome.Controls.Add(lblWelcomeTitle);

            lblWelcomeTitle.Text = "Chào mừng đến với Hospital DBA Console";
            lblWelcomeTitle.Font = new System.Drawing.Font("Segoe UI", 13f, System.Drawing.FontStyle.Bold);
            lblWelcomeTitle.ForeColor = System.Drawing.Color.FromArgb(28, 28, 32);
            lblWelcomeTitle.AutoSize = true;
            lblWelcomeTitle.Location = new System.Drawing.Point(0, 8);
            lblWelcomeTitle.Name = "lblWelcomeTitle";
            lblWelcomeTitle.TabIndex = 1;

            lblWelcomeDesc.Text = "Chọn một chức năng từ menu bên trái để bắt đầu.";
            lblWelcomeDesc.Font = new System.Drawing.Font("Segoe UI", 9.5f);
            lblWelcomeDesc.ForeColor = System.Drawing.Color.FromArgb(120, 120, 130);
            lblWelcomeDesc.AutoSize = true;
            lblWelcomeDesc.Location = new System.Drawing.Point(0, 44);
            lblWelcomeDesc.Name = "lblWelcomeDesc";
            lblWelcomeDesc.TabIndex = 0;

            // ── Cards FlowLayout ──────────────────────────────────────
            pnlCards.Dock = System.Windows.Forms.DockStyle.Top;
            pnlCards.AutoSize = true;
            pnlCards.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            pnlCards.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            pnlCards.WrapContents = true;
            pnlCards.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            pnlCards.BackColor = System.Drawing.Color.Transparent;
            pnlCards.Name = "pnlCards";
            pnlCards.TabIndex = 0;

            // ── Form Root ─────────────────────────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1100, 660);
            MinimumSize = new System.Drawing.Size(900, 560);
            BackColor = System.Drawing.Color.FromArgb(245, 244, 242);
            Font = new System.Drawing.Font("Segoe UI", 9f);
            Text = "HospitalAdmin";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Name = "Dashboard";
            // Sidebar must be added AFTER pnlMain so Left dock works correctly
            Controls.Add(pnlMain);
            Controls.Add(pnlSidebar);
            Load += new System.EventHandler(this.Dashboard_Load);

            pnlSidebar.ResumeLayout(false);
            pnlSidebarFooter.ResumeLayout(false);
            pnlSidebarFooter.PerformLayout();
            pnlNavItems.ResumeLayout(false);
            pnlLogo.ResumeLayout(false);
            pnlLogo.PerformLayout();
            pnlMain.ResumeLayout(false);
            pnlTopbar.ResumeLayout(false);
            pnlTopbar.PerformLayout();
            pnlContent.ResumeLayout(false);
            pnlContent.PerformLayout();
            pnlWelcome.ResumeLayout(false);
            pnlWelcome.PerformLayout();
            ResumeLayout(false);
        }

        private void SetupNavButton(System.Windows.Forms.Button btn, string text, int index)
        {
            btn.Text = text;
            btn.Font = new System.Drawing.Font("Segoe UI", 9.5f);
            btn.ForeColor = System.Drawing.Color.FromArgb(190, 190, 200);
            btn.BackColor = System.Drawing.Color.Transparent;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(40, 40, 46);
            btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(50, 50, 58);
            btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btn.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            btn.Size = new System.Drawing.Size(216, 46);
            btn.Location = new System.Drawing.Point(12, 10 + index * 52);
            btn.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        // ── Control declarations ──────────────────────────────────────
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlLogo;
        private System.Windows.Forms.Label lblAppTitle;
        private System.Windows.Forms.Label lblAppSubtitle;
        private System.Windows.Forms.Panel pnlNavItems;
        private System.Windows.Forms.Button btnNavUsers;
        // private System.Windows.Forms.Button btnNavRoles;
        private System.Windows.Forms.Button btnNavGrant;
        private System.Windows.Forms.Button btnNavRevoke;
        // private System.Windows.Forms.Button btnNavPermView;
        private System.Windows.Forms.Panel pnlSidebarFooter;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlTopbar;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Label lblPageBreadcrumb;
        private System.Windows.Forms.Label lblUserInfo;
        private System.Windows.Forms.Panel pnlDivider;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlWelcome;
        private System.Windows.Forms.Label lblWelcomeTitle;
        private System.Windows.Forms.Label lblWelcomeDesc;
        private System.Windows.Forms.FlowLayoutPanel pnlCards;
        private System.Windows.Forms.Button btnLogout;

        #endregion
    }
}