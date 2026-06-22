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
            this.btnNavGrant = new System.Windows.Forms.Button();
            this.btnNavRevoke = new System.Windows.Forms.Button();
            this.btnNavOls = new System.Windows.Forms.Button();
            this.btnNavAddOls = new System.Windows.Forms.Button();
            this.btnNavAudit = new System.Windows.Forms.Button();
            this.btnNavBackup = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.pnlLogo = new System.Windows.Forms.Panel();
            this.lblAppSubtitle = new System.Windows.Forms.Label();
            this.lblAppTitle = new System.Windows.Forms.Label();
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
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.pnlSidebar.SuspendLayout();
            this.pnlSidebarFooter.SuspendLayout();
            this.pnlNavItems.SuspendLayout();
            this.pnlLogo.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlWelcome.SuspendLayout();
            this.pnlTopbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(28)))));
            this.pnlSidebar.Controls.Add(this.pnlSidebarFooter);
            this.pnlSidebar.Controls.Add(this.pnlNavItems);
            this.pnlSidebar.Controls.Add(this.pnlLogo);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(240, 660);
            this.pnlSidebar.TabIndex = 1;
            // 
            // pnlSidebarFooter
            // 
            this.pnlSidebarFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(28)))));
            this.pnlSidebarFooter.Controls.Add(this.lblVersion);
            this.pnlSidebarFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSidebarFooter.Location = new System.Drawing.Point(0, 616);
            this.pnlSidebarFooter.Name = "pnlSidebarFooter";
            this.pnlSidebarFooter.Size = new System.Drawing.Size(240, 44);
            this.pnlSidebarFooter.TabIndex = 0;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(90)))));
            this.lblVersion.Location = new System.Drawing.Point(20, 14);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(91, 12);
            this.lblVersion.TabIndex = 0;
            this.lblVersion.Text = "Phân Hệ 1 · v1.0";
            // 
            // pnlNavItems
            // 
            this.pnlNavItems.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(28)))));
            this.pnlNavItems.Controls.Add(this.btnNavUsers);
            this.pnlNavItems.Controls.Add(this.btnNavGrant);
            this.pnlNavItems.Controls.Add(this.btnNavRevoke);
            this.pnlNavItems.Controls.Add(this.btnNavOls);
            this.pnlNavItems.Controls.Add(this.btnNavAddOls);
            this.pnlNavItems.Controls.Add(this.btnNavAudit);
            this.pnlNavItems.Controls.Add(this.btnNavBackup);
            this.pnlNavItems.Controls.Add(this.btnLogout);
            this.pnlNavItems.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNavItems.Location = new System.Drawing.Point(0, 88);
            this.pnlNavItems.Name = "pnlNavItems";
            this.pnlNavItems.Size = new System.Drawing.Size(240, 450);
            this.pnlNavItems.TabIndex = 1;
            // 
            // btnNavUsers
            // 
            this.btnNavUsers.BackColor = System.Drawing.Color.Transparent;
            this.btnNavUsers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavUsers.FlatAppearance.BorderSize = 0;
            this.btnNavUsers.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(58)))));
            this.btnNavUsers.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.btnNavUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavUsers.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnNavUsers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.btnNavUsers.Location = new System.Drawing.Point(12, 10);
            this.btnNavUsers.Name = "btnNavUsers";
            this.btnNavUsers.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnNavUsers.Size = new System.Drawing.Size(216, 46);
            this.btnNavUsers.TabIndex = 0;
            this.btnNavUsers.Text = "  Quản lý user và role";
            this.btnNavUsers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavUsers.UseVisualStyleBackColor = false;
            // 
            // btnNavGrant
            // 
            this.btnNavGrant.BackColor = System.Drawing.Color.Transparent;
            this.btnNavGrant.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavGrant.FlatAppearance.BorderSize = 0;
            this.btnNavGrant.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(58)))));
            this.btnNavGrant.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.btnNavGrant.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavGrant.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnNavGrant.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.btnNavGrant.Location = new System.Drawing.Point(12, 62);
            this.btnNavGrant.Name = "btnNavGrant";
            this.btnNavGrant.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnNavGrant.Size = new System.Drawing.Size(216, 46);
            this.btnNavGrant.TabIndex = 2;
            this.btnNavGrant.Text = "  Cấp Quyền";
            this.btnNavGrant.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavGrant.UseVisualStyleBackColor = false;
            // 
            // btnNavRevoke
            // 
            this.btnNavRevoke.BackColor = System.Drawing.Color.Transparent;
            this.btnNavRevoke.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavRevoke.FlatAppearance.BorderSize = 0;
            this.btnNavRevoke.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(58)))));
            this.btnNavRevoke.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.btnNavRevoke.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavRevoke.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnNavRevoke.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.btnNavRevoke.Location = new System.Drawing.Point(12, 114);
            this.btnNavRevoke.Name = "btnNavRevoke";
            this.btnNavRevoke.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnNavRevoke.Size = new System.Drawing.Size(216, 46);
            this.btnNavRevoke.TabIndex = 3;
            this.btnNavRevoke.Text = "  Xem và thu hồi quyền";
            this.btnNavRevoke.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavRevoke.UseVisualStyleBackColor = false;
            // 
            // btnNavOls
            // 
            this.btnNavOls.BackColor = System.Drawing.Color.Transparent;
            this.btnNavOls.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavOls.FlatAppearance.BorderSize = 0;
            this.btnNavOls.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(58)))));
            this.btnNavOls.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.btnNavOls.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavOls.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnNavOls.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.btnNavOls.Location = new System.Drawing.Point(12, 166);
            this.btnNavOls.Name = "btnNavOls";
            this.btnNavOls.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnNavOls.Size = new System.Drawing.Size(216, 46);
            this.btnNavOls.TabIndex = 4;
            this.btnNavOls.Text = "  Thông báo OLS";
            this.btnNavOls.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavOls.UseVisualStyleBackColor = false;
            // 
            // btnNavAddOls
            // 
            this.btnNavAddOls.BackColor = System.Drawing.Color.Transparent;
            this.btnNavAddOls.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavAddOls.FlatAppearance.BorderSize = 0;
            this.btnNavAddOls.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(58)))));
            this.btnNavAddOls.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.btnNavAddOls.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavAddOls.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnNavAddOls.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.btnNavAddOls.Location = new System.Drawing.Point(12, 218);
            this.btnNavAddOls.Name = "btnNavAddOls";
            this.btnNavAddOls.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnNavAddOls.Size = new System.Drawing.Size(216, 46);
            this.btnNavAddOls.TabIndex = 5;
            this.btnNavAddOls.Text = "  Tạo thông báo OLS";
            this.btnNavAddOls.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavAddOls.UseVisualStyleBackColor = false;
            // 
            // btnNavAudit
            // 
            this.btnNavAudit.BackColor = System.Drawing.Color.Transparent;
            this.btnNavAudit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavAudit.FlatAppearance.BorderSize = 0;
            this.btnNavAudit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(58)))));
            this.btnNavAudit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.btnNavAudit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavAudit.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnNavAudit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.btnNavAudit.Location = new System.Drawing.Point(12, 270);
            this.btnNavAudit.Name = "btnNavAudit";
            this.btnNavAudit.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnNavAudit.Size = new System.Drawing.Size(216, 46);
            this.btnNavAudit.TabIndex = 6;
            this.btnNavAudit.Text = "  Nhật ký kiểm toán";
            this.btnNavAudit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavAudit.UseVisualStyleBackColor = false;
            // 
            // btnNavBackup
            // 
            this.btnNavBackup.BackColor = System.Drawing.Color.Transparent;
            this.btnNavBackup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavBackup.FlatAppearance.BorderSize = 0;
            this.btnNavBackup.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(58)))));
            this.btnNavBackup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.btnNavBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavBackup.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnNavBackup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.btnNavBackup.Location = new System.Drawing.Point(12, 322);
            this.btnNavBackup.Name = "btnNavBackup";
            this.btnNavBackup.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnNavBackup.Size = new System.Drawing.Size(216, 46);
            this.btnNavBackup.TabIndex = 7;
            this.btnNavBackup.Text = "  Sao lưu và phục hồi";
            this.btnNavBackup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavBackup.UseVisualStyleBackColor = false;
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.Transparent;
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(220)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnLogout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnLogout.Location = new System.Drawing.Point(12, 374);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnLogout.Size = new System.Drawing.Size(216, 46);
            this.btnLogout.TabIndex = 6;
            this.btnLogout.Text = "  Đăng xuất";
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // pnlLogo
            // 
            this.pnlLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(35)))));
            this.pnlLogo.Controls.Add(this.lblAppSubtitle);
            this.pnlLogo.Controls.Add(this.lblAppTitle);
            this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogo.Location = new System.Drawing.Point(0, 0);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(240, 88);
            this.pnlLogo.TabIndex = 2;
            // 
            // lblAppSubtitle
            // 
            this.lblAppSubtitle.AutoSize = true;
            this.lblAppSubtitle.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this.lblAppSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(130)))));
            this.lblAppSubtitle.Location = new System.Drawing.Point(22, 52);
            this.lblAppSubtitle.Name = "lblAppSubtitle";
            this.lblAppSubtitle.Size = new System.Drawing.Size(125, 12);
            this.lblAppSubtitle.TabIndex = 0;
            this.lblAppSubtitle.Text = "DBA Management Console";
            // 
            // lblAppTitle
            // 
            this.lblAppTitle.AutoSize = true;
            this.lblAppTitle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblAppTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblAppTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(40)))));
            this.lblAppTitle.Location = new System.Drawing.Point(20, 18);
            this.lblAppTitle.Name = "lblAppTitle";
            this.lblAppTitle.Size = new System.Drawing.Size(142, 25);
            this.lblAppTitle.TabIndex = 1;
            this.lblAppTitle.Text = "HospitalAdmin";
            this.lblAppTitle.Click += new System.EventHandler(this.lblAppTitle_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(244)))), ((int)(((byte)(242)))));
            this.pnlMain.Controls.Add(this.pnlContent);
            this.pnlMain.Controls.Add(this.pnlDivider);
            this.pnlMain.Controls.Add(this.pnlTopbar);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(240, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(860, 660);
            this.pnlMain.TabIndex = 0;
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(244)))), ((int)(((byte)(242)))));
            this.pnlContent.Controls.Add(this.pnlCards);
            this.pnlContent.Controls.Add(this.pnlWelcome);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 71);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(24, 20, 24, 24);
            this.pnlContent.Size = new System.Drawing.Size(860, 589);
            this.pnlContent.TabIndex = 0;
            // 
            // pnlCards
            // 
            this.pnlCards.AutoSize = true;
            this.pnlCards.BackColor = System.Drawing.Color.Transparent;
            this.pnlCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCards.Location = new System.Drawing.Point(24, 100);
            this.pnlCards.Name = "pnlCards";
            this.pnlCards.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.pnlCards.Size = new System.Drawing.Size(812, 4);
            this.pnlCards.TabIndex = 0;
            // 
            // pnlWelcome
            // 
            this.pnlWelcome.BackColor = System.Drawing.Color.Transparent;
            this.pnlWelcome.Controls.Add(this.lblWelcomeDesc);
            this.pnlWelcome.Controls.Add(this.lblWelcomeTitle);
            this.pnlWelcome.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlWelcome.Location = new System.Drawing.Point(24, 20);
            this.pnlWelcome.Name = "pnlWelcome";
            this.pnlWelcome.Size = new System.Drawing.Size(812, 80);
            this.pnlWelcome.TabIndex = 1;
            // 
            // lblWelcomeDesc
            // 
            this.lblWelcomeDesc.AutoSize = true;
            this.lblWelcomeDesc.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblWelcomeDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(130)))));
            this.lblWelcomeDesc.Location = new System.Drawing.Point(0, 44);
            this.lblWelcomeDesc.Name = "lblWelcomeDesc";
            this.lblWelcomeDesc.Size = new System.Drawing.Size(306, 17);
            this.lblWelcomeDesc.TabIndex = 0;
            this.lblWelcomeDesc.Text = "Chọn một chức năng từ menu bên trái để bắt đầu.";
            // 
            // lblWelcomeTitle
            // 
            this.lblWelcomeTitle.AutoSize = true;
            this.lblWelcomeTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblWelcomeTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.lblWelcomeTitle.Location = new System.Drawing.Point(0, 8);
            this.lblWelcomeTitle.Name = "lblWelcomeTitle";
            this.lblWelcomeTitle.Size = new System.Drawing.Size(359, 25);
            this.lblWelcomeTitle.TabIndex = 1;
            this.lblWelcomeTitle.Text = "Chào mừng đến với Hospital DBA Console";
            // 
            // pnlDivider
            // 
            this.pnlDivider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(40)))));
            this.pnlDivider.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDivider.Location = new System.Drawing.Point(0, 68);
            this.pnlDivider.Name = "pnlDivider";
            this.pnlDivider.Size = new System.Drawing.Size(860, 3);
            this.pnlDivider.TabIndex = 1;
            // 
            // pnlTopbar
            // 
            this.pnlTopbar.BackColor = System.Drawing.Color.White;
            this.pnlTopbar.Controls.Add(this.lblPageBreadcrumb);
            this.pnlTopbar.Controls.Add(this.lblPageTitle);
            this.pnlTopbar.Controls.Add(this.lblUserInfo);
            this.pnlTopbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopbar.Location = new System.Drawing.Point(0, 0);
            this.pnlTopbar.Name = "pnlTopbar";
            this.pnlTopbar.Size = new System.Drawing.Size(860, 68);
            this.pnlTopbar.TabIndex = 2;
            // 
            // lblPageBreadcrumb
            // 
            this.lblPageBreadcrumb.AutoSize = true;
            this.lblPageBreadcrumb.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblPageBreadcrumb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(170)))));
            this.lblPageBreadcrumb.Location = new System.Drawing.Point(30, 42);
            this.lblPageBreadcrumb.Name = "lblPageBreadcrumb";
            this.lblPageBreadcrumb.Size = new System.Drawing.Size(60, 15);
            this.lblPageBreadcrumb.TabIndex = 0;
            this.lblPageBreadcrumb.Text = "Trang chủ";
            // 
            // lblPageTitle
            // 
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblPageTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.lblPageTitle.Location = new System.Drawing.Point(28, 10);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(113, 28);
            this.lblPageTitle.TabIndex = 1;
            this.lblPageTitle.Text = "Dashboard";
            // 
            // lblUserInfo
            // 
            this.lblUserInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserInfo.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblUserInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(40)))));
            this.lblUserInfo.Location = new System.Drawing.Point(620, 0);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(240, 20);
            this.lblUserInfo.TabIndex = 3;
            this.lblUserInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(244)))), ((int)(((byte)(242)))));
            this.ClientSize = new System.Drawing.Size(1100, 660);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlSidebar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(900, 560);
            this.Name = "Dashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HospitalAdmin";
            this.Load += new System.EventHandler(this.Dashboard_Load);
            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebarFooter.ResumeLayout(false);
            this.pnlSidebarFooter.PerformLayout();
            this.pnlNavItems.ResumeLayout(false);
            this.pnlLogo.ResumeLayout(false);
            this.pnlLogo.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.pnlWelcome.ResumeLayout(false);
            this.pnlWelcome.PerformLayout();
            this.pnlTopbar.ResumeLayout(false);
            this.pnlTopbar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlLogo;
        private System.Windows.Forms.Label lblAppTitle;
        private System.Windows.Forms.Label lblAppSubtitle;
        private System.Windows.Forms.Panel pnlNavItems;
        private System.Windows.Forms.Button btnNavUsers;
        private System.Windows.Forms.Button btnNavGrant;
        private System.Windows.Forms.Button btnNavRevoke;
        private System.Windows.Forms.Button btnNavOls;
        private System.Windows.Forms.Button btnNavAddOls;
        private System.Windows.Forms.Button btnNavAudit;
        private System.Windows.Forms.Button btnNavBackup;
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
    }
}