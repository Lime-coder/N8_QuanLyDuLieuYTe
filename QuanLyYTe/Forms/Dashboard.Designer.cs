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
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pnlSidebarFooter = new System.Windows.Forms.Panel();
            this.lblVersion = new System.Windows.Forms.Label();
            this.pnlNavItems = new System.Windows.Forms.Panel();
            this.btnNavUsers = new System.Windows.Forms.Button();
            this.btnNavRoles = new System.Windows.Forms.Button();
            this.btnNavGrant = new System.Windows.Forms.Button();
            this.btnNavRevoke = new System.Windows.Forms.Button();
            this.btnNavPermView = new System.Windows.Forms.Button();
            this.pnlLogo = new System.Windows.Forms.Panel();
            this.lblAppTitle = new System.Windows.Forms.Label();
            this.lblAppSubtitle = new System.Windows.Forms.Label();

            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlCards = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlWelcome = new System.Windows.Forms.Panel();
            this.lblWelcomeDesc = new System.Windows.Forms.Label();
            this.lblWelcomeTitle = new System.Windows.Forms.Label();
            this.pnlDivider = new System.Windows.Forms.Panel();
            this.pnlTopbar = new System.Windows.Forms.Panel();
            this.lblPageBreadcrumb = new System.Windows.Forms.Label();
            this.lblPageTitle = new System.Windows.Forms.Label();

            this.pnlSidebar.SuspendLayout();
            this.pnlNavItems.SuspendLayout();
            this.pnlLogo.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlWelcome.SuspendLayout();
            this.pnlTopbar.SuspendLayout();
            this.SuspendLayout();

            // ── Sidebar ──────────────────────────────────────────────────────────
            // IMPORTANT: For DockStyle, Controls must be added in REVERSE order
            // (Bottom-docked first, then Top-docked last = Top renders at top)
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(24, 24, 28);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Width = 240;
            // Add order: Footer first (Bottom), NavItems second (Top), Logo last (Top)
            // WinForms processes dock in reverse Controls order: last added Top = topmost
            this.pnlSidebar.Controls.Add(this.pnlSidebarFooter); // Bottom → renders bottom
            this.pnlSidebar.Controls.Add(this.pnlNavItems);       // Top    → renders below logo
            this.pnlSidebar.Controls.Add(this.pnlLogo);           // Top    → renders at top

            // ── Sidebar Footer ───────────────────────────────────────────────────
            this.pnlSidebarFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSidebarFooter.Height = 40;
            this.pnlSidebarFooter.BackColor = System.Drawing.Color.FromArgb(24, 24, 28);
            this.pnlSidebarFooter.Controls.Add(this.lblVersion);

            this.lblVersion.Text = "Phân Hệ 1 · v1.0";
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 7.5f);
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(80, 80, 90);
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(20, 12);

            // ── Nav Items ────────────────────────────────────────────────────────
            this.pnlNavItems.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNavItems.Height = 290;
            this.pnlNavItems.BackColor = System.Drawing.Color.FromArgb(24, 24, 28);
            this.pnlNavItems.Controls.Add(this.btnNavUsers);
            this.pnlNavItems.Controls.Add(this.btnNavRoles);
            this.pnlNavItems.Controls.Add(this.btnNavGrant);
            this.pnlNavItems.Controls.Add(this.btnNavRevoke);
            this.pnlNavItems.Controls.Add(this.btnNavPermView);

            SetupNavButton(this.btnNavUsers, "  Quản lý User", 0);
            SetupNavButton(this.btnNavRoles, "  Quản lý Role", 1);
            SetupNavButton(this.btnNavGrant, "  Cấp Quyền", 2);
            SetupNavButton(this.btnNavRevoke, "  Thu Hồi Quyền", 3);
            SetupNavButton(this.btnNavPermView, "  Xem Quyền", 4);

            // ── Logo ─────────────────────────────────────────────────────────────
            this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogo.Height = 80;
            this.pnlLogo.BackColor = System.Drawing.Color.FromArgb(30, 30, 35);
            this.pnlLogo.Controls.Add(this.lblAppSubtitle);
            this.pnlLogo.Controls.Add(this.lblAppTitle);

            this.lblAppTitle.Text = "OracleAdmin";
            this.lblAppTitle.Font = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold);
            this.lblAppTitle.ForeColor = System.Drawing.Color.FromArgb(255, 140, 40);
            this.lblAppTitle.AutoSize = true;
            this.lblAppTitle.Location = new System.Drawing.Point(20, 16);

            this.lblAppSubtitle.Text = "DBA Management Console";
            this.lblAppSubtitle.Font = new System.Drawing.Font("Segoe UI", 7.5f);
            this.lblAppSubtitle.ForeColor = System.Drawing.Color.FromArgb(120, 120, 130);
            this.lblAppSubtitle.AutoSize = true;
            this.lblAppSubtitle.Location = new System.Drawing.Point(22, 44);

            // ── Main Panel ───────────────────────────────────────────────────────
            // Add order: Content first (Fill), Divider second (Top), Topbar last (Top)
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(245, 244, 242);
            this.pnlMain.Controls.Add(this.pnlContent);  // Fill → takes remaining space
            this.pnlMain.Controls.Add(this.pnlDivider);  // Top  → just below topbar
            this.pnlMain.Controls.Add(this.pnlTopbar);   // Top  → renders at very top

            // ── Topbar ───────────────────────────────────────────────────────────
            this.pnlTopbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopbar.Height = 68;
            this.pnlTopbar.BackColor = System.Drawing.Color.White;
            this.pnlTopbar.Controls.Add(this.lblPageBreadcrumb);
            this.pnlTopbar.Controls.Add(this.lblPageTitle);

            this.lblPageTitle.Text = "Dashboard";
            this.lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 15f, System.Drawing.FontStyle.Bold);
            this.lblPageTitle.ForeColor = System.Drawing.Color.FromArgb(28, 28, 32);
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.Location = new System.Drawing.Point(28, 10);

            this.lblPageBreadcrumb.Text = "Trang chủ";
            this.lblPageBreadcrumb.Font = new System.Drawing.Font("Segoe UI", 8.5f);
            this.lblPageBreadcrumb.ForeColor = System.Drawing.Color.FromArgb(160, 160, 170);
            this.lblPageBreadcrumb.AutoSize = true;
            this.lblPageBreadcrumb.Location = new System.Drawing.Point(30, 42);

            // ── Orange divider ───────────────────────────────────────────────────
            this.pnlDivider.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDivider.Height = 3;
            this.pnlDivider.BackColor = System.Drawing.Color.FromArgb(255, 140, 40);

            // ── Content Panel ────────────────────────────────────────────────────
            // Add order: pnlCards first (Fill), pnlWelcome last (Top)
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Padding = new System.Windows.Forms.Padding(24, 20, 24, 24);
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(245, 244, 242);
            this.pnlContent.AutoScroll = true;
            this.pnlContent.Controls.Add(this.pnlCards);    // Fill → below welcome
            this.pnlContent.Controls.Add(this.pnlWelcome);  // Top  → renders at top

            // ── Welcome Banner ───────────────────────────────────────────────────
            this.pnlWelcome.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlWelcome.Height = 80;
            this.pnlWelcome.BackColor = System.Drawing.Color.Transparent;
            this.pnlWelcome.Controls.Add(this.lblWelcomeDesc);
            this.pnlWelcome.Controls.Add(this.lblWelcomeTitle);

            this.lblWelcomeTitle.Text = "Chào mừng đến với Oracle DBA Console";
            this.lblWelcomeTitle.Font = new System.Drawing.Font("Segoe UI", 13f, System.Drawing.FontStyle.Bold);
            this.lblWelcomeTitle.ForeColor = System.Drawing.Color.FromArgb(28, 28, 32);
            this.lblWelcomeTitle.AutoSize = true;
            this.lblWelcomeTitle.Location = new System.Drawing.Point(0, 8);

            this.lblWelcomeDesc.Text = "Chọn một chức năng từ menu bên trái để bắt đầu.";
            this.lblWelcomeDesc.Font = new System.Drawing.Font("Segoe UI", 9.5f);
            this.lblWelcomeDesc.ForeColor = System.Drawing.Color.FromArgb(120, 120, 130);
            this.lblWelcomeDesc.AutoSize = true;
            this.lblWelcomeDesc.Location = new System.Drawing.Point(0, 40);

            // ── Cards FlowLayout ─────────────────────────────────────────────────
            // Use FlowLayoutPanel so cards wrap naturally — NO DockStyle.Fill here
            // because Fill inside a panel with AutoScroll conflicts with FlowLayout
            this.pnlCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCards.AutoSize = true;       // expands to fit its children
            this.pnlCards.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlCards.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.pnlCards.WrapContents = true;
            this.pnlCards.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.pnlCards.BackColor = System.Drawing.Color.Transparent;
            // Cards are added at runtime in Dashboard.cs → BuildCards()

            // ── Form root ────────────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 660);
            this.MinimumSize = new System.Drawing.Size(900, 560);
            this.Text = "Oracle DBA Management Console";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(245, 244, 242);
            this.Font = new System.Drawing.Font("Segoe UI", 9f);

            // Add sidebar before main so sidebar docks Left correctly
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlSidebar);

            this.pnlSidebar.ResumeLayout(false);
            this.pnlNavItems.ResumeLayout(false);
            this.pnlLogo.ResumeLayout(false);
            this.pnlLogo.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlTopbar.ResumeLayout(false);
            this.pnlTopbar.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.pnlWelcome.ResumeLayout(false);
            this.pnlWelcome.PerformLayout();
            this.ResumeLayout(false);
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

        // ── Control declarations ─────────────────────────────────────
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlLogo;
        private System.Windows.Forms.Label lblAppTitle;
        private System.Windows.Forms.Label lblAppSubtitle;
        private System.Windows.Forms.Panel pnlNavItems;
        private System.Windows.Forms.Button btnNavUsers;
        private System.Windows.Forms.Button btnNavRoles;
        private System.Windows.Forms.Button btnNavGrant;
        private System.Windows.Forms.Button btnNavRevoke;
        private System.Windows.Forms.Button btnNavPermView;
        private System.Windows.Forms.Panel pnlSidebarFooter;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlTopbar;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Label lblPageBreadcrumb;
        private System.Windows.Forms.Panel pnlDivider;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlWelcome;
        private System.Windows.Forms.Label lblWelcomeTitle;
        private System.Windows.Forms.Label lblWelcomeDesc;
        private System.Windows.Forms.FlowLayoutPanel pnlCards;

        #endregion
    }
}