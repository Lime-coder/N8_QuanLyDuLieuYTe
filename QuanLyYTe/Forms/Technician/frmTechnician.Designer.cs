namespace QuanLyYTe.Forms.Technician
{
    partial class frmTechnician
    {
        //  Required designer variable.
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            panelHeader = new Panel();
            btnLogout = new Button();
            lblAppSubtitle = new Label();
            lblAppTitle = new Label();
            tabControlMain = new TabControl();
            tabServices = new TabPage();
            splitServices = new SplitContainer();
            dgvServices = new DataGridView();
            pnlSearchBar = new Panel();
            lblSearchRecord = new Label();
            txtSearchRecord = new TextBox();
            btnSearch = new Button();
            btnRefresh = new Button();
            tabDetailView = new TabControl();
            tabDetailInfo = new TabPage();
            lblDetailRecordId = new Label();
            txtDetailRecordId = new TextBox();
            lblDetailServiceType = new Label();
            txtDetailServiceType = new TextBox();
            lblDetailServiceDate = new Label();
            txtDetailServiceDate = new TextBox();
            tabDetailResult = new TabPage();
            lblCurrentResult = new Label();
            txtCurrentResult = new TextBox();
            btnUpdateResult = new Button();
            btnSaveResult = new Button();
            btnCancelResult = new Button();
            tabPersonal = new TabPage();
            tlpPersonalCenter = new TableLayoutPanel();
            pnlPersonalContent = new Panel();
            lblUserId = new Label();
            txtUserId = new TextBox();
            lblFullName = new Label();
            txtFullName = new TextBox();
            lblGender = new Label();
            txtGender = new TextBox();
            lblBirthdate = new Label();
            dtpBirthdate = new DateTimePicker();
            lblPhone = new Label();
            txtPhone = new TextBox();
            lblHometown = new Label();
            txtHometown = new TextBox();
            btnSaveInfo = new Button();
            btnCancelEdit = new Button();
            pnlResultNote = new Panel();
            lblResultNote = new Label();
            pnlInfoNote = new Panel();
            lblInfoNote = new Label();
            btnEditInfo = new Button();
            panelHeader.SuspendLayout();
            tabControlMain.SuspendLayout();
            tabServices.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitServices).BeginInit();
            splitServices.Panel1.SuspendLayout();
            splitServices.Panel2.SuspendLayout();
            splitServices.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvServices).BeginInit();
            pnlSearchBar.SuspendLayout();
            tabDetailView.SuspendLayout();
            tabDetailInfo.SuspendLayout();
            tabDetailResult.SuspendLayout();
            tabPersonal.SuspendLayout();
            tlpPersonalCenter.SuspendLayout();
            pnlPersonalContent.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(30, 41, 59);
            panelHeader.Controls.Add(btnLogout);
            panelHeader.Controls.Add(lblAppSubtitle);
            panelHeader.Controls.Add(lblAppTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1000, 80);
            panelHeader.TabIndex = 2;
            // 
            // btnLogout
            // 
            btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLogout.BackColor = Color.FromArgb(220, 38, 38);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(880, 23);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(100, 34);
            btnLogout.TabIndex = 0;
            btnLogout.Text = "Đăng xuất";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // lblAppSubtitle
            // 
            lblAppSubtitle.AutoSize = true;
            lblAppSubtitle.Font = new Font("Segoe UI", 9F);
            lblAppSubtitle.ForeColor = Color.Gainsboro;
            lblAppSubtitle.Location = new Point(21, 46);
            lblAppSubtitle.Name = "lblAppSubtitle";
            lblAppSubtitle.Size = new Size(369, 20);
            lblAppSubtitle.TabIndex = 1;
            lblAppSubtitle.Text = "Thực hiện dịch vụ được phân công và ghi nhận kết quả";
            // 
            // lblAppTitle
            // 
            lblAppTitle.AutoSize = true;
            lblAppTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblAppTitle.ForeColor = Color.White;
            lblAppTitle.Location = new Point(18, 12);
            lblAppTitle.Name = "lblAppTitle";
            lblAppTitle.Size = new Size(187, 37);
            lblAppTitle.TabIndex = 2;
            lblAppTitle.Text = "Kỹ thuật viên";
            // 
            // tabControlMain
            // 
            tabControlMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControlMain.Controls.Add(tabServices);
            tabControlMain.Controls.Add(tabPersonal);
            tabControlMain.Font = new Font("Segoe UI", 10F);
            tabControlMain.Location = new Point(14, 96);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new Size(972, 530);
            tabControlMain.TabIndex = 1;
            // 
            // tabServices
            // 
            tabServices.Controls.Add(splitServices);
            tabServices.Location = new Point(4, 32);
            tabServices.Name = "tabServices";
            tabServices.Padding = new Padding(6);
            tabServices.Size = new Size(964, 494);
            tabServices.TabIndex = 0;
            tabServices.Text = "Dịch vụ của tôi";
            tabServices.UseVisualStyleBackColor = true;
            // 
            // splitServices
            // 
            splitServices.BackColor = Color.FromArgb(203, 213, 225);
            splitServices.Dock = DockStyle.Fill;
            splitServices.Location = new Point(6, 6);
            splitServices.Name = "splitServices";
            splitServices.Orientation = Orientation.Horizontal;
            // 
            // splitServices.Panel1
            // 
            splitServices.Panel1.Controls.Add(dgvServices);
            splitServices.Panel1.Controls.Add(pnlSearchBar);
            splitServices.Panel1MinSize = 80;
            // 
            // splitServices.Panel2
            // 
            splitServices.Panel2.Controls.Add(tabDetailView);
            splitServices.Panel2.Padding = new Padding(0, 4, 0, 0);
            splitServices.Panel2MinSize = 180;
            splitServices.Size = new Size(952, 482);
            splitServices.SplitterDistance = 298;
            splitServices.TabIndex = 0;
            // 
            // dgvServices
            // 
            dgvServices.AllowUserToAddRows = false;
            dgvServices.AllowUserToDeleteRows = false;
            dgvServices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvServices.BackgroundColor = Color.White;
            dgvServices.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(241, 245, 249);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvServices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvServices.ColumnHeadersHeight = 36;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9.5F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvServices.DefaultCellStyle = dataGridViewCellStyle2;
            dgvServices.Dock = DockStyle.Fill;
            dgvServices.EnableHeadersVisualStyles = false;
            dgvServices.GridColor = Color.FromArgb(226, 232, 240);
            dgvServices.Location = new Point(0, 50);
            dgvServices.Name = "dgvServices";
            dgvServices.ReadOnly = true;
            dgvServices.RowHeadersVisible = false;
            dgvServices.RowHeadersWidth = 51;
            dgvServices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvServices.Size = new Size(952, 248);
            dgvServices.TabIndex = 1;
            dgvServices.SelectionChanged += dgvServices_SelectionChanged;
            // 
            // pnlSearchBar
            // 
            pnlSearchBar.BackColor = Color.FromArgb(248, 250, 252);
            pnlSearchBar.Controls.Add(lblSearchRecord);
            pnlSearchBar.Controls.Add(txtSearchRecord);
            pnlSearchBar.Controls.Add(btnSearch);
            pnlSearchBar.Controls.Add(btnRefresh);
            pnlSearchBar.Dock = DockStyle.Top;
            pnlSearchBar.Location = new Point(0, 0);
            pnlSearchBar.Name = "pnlSearchBar";
            pnlSearchBar.Padding = new Padding(6, 8, 6, 6);
            pnlSearchBar.Size = new Size(952, 50);
            pnlSearchBar.TabIndex = 2;
            // 
            // lblSearchRecord
            // 
            lblSearchRecord.AutoSize = true;
            lblSearchRecord.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblSearchRecord.ForeColor = Color.FromArgb(51, 65, 85);
            lblSearchRecord.Location = new Point(6, 15);
            lblSearchRecord.Name = "lblSearchRecord";
            lblSearchRecord.Size = new Size(180, 21);
            lblSearchRecord.TabIndex = 0;
            lblSearchRecord.Text = "Tìm mã HSBA/dịch vụ:";
            // 
            // txtSearchRecord
            // 
            txtSearchRecord.Font = new Font("Segoe UI", 10F);
            txtSearchRecord.Location = new Point(190, 11);
            txtSearchRecord.Name = "txtSearchRecord";
            txtSearchRecord.PlaceholderText = "Nhập mã HSBA/dịch vụ cần tìm";
            txtSearchRecord.Size = new Size(260, 30);
            txtSearchRecord.TabIndex = 1;
            txtSearchRecord.KeyDown += txtSearchRecord_KeyDown;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(255, 140, 40);
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(460, 10);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(80, 30);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "Tìm";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(15, 118, 110);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(550, 10);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(90, 30);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // tabDetailView
            // 
            tabDetailView.Controls.Add(tabDetailInfo);
            tabDetailView.Controls.Add(tabDetailResult);
            tabDetailView.Dock = DockStyle.Fill;
            tabDetailView.Font = new Font("Segoe UI", 9.5F);
            tabDetailView.Location = new Point(0, 4);
            tabDetailView.Name = "tabDetailView";
            tabDetailView.SelectedIndex = 0;
            tabDetailView.Size = new Size(952, 176);
            tabDetailView.TabIndex = 0;
            // 
            // tabDetailInfo
            // 
            tabDetailInfo.Controls.Add(lblDetailRecordId);
            tabDetailInfo.Controls.Add(txtDetailRecordId);
            tabDetailInfo.Controls.Add(lblDetailServiceType);
            tabDetailInfo.Controls.Add(txtDetailServiceType);
            tabDetailInfo.Controls.Add(lblDetailServiceDate);
            tabDetailInfo.Controls.Add(txtDetailServiceDate);
            tabDetailInfo.Location = new Point(4, 30);
            tabDetailInfo.Name = "tabDetailInfo";
            tabDetailInfo.Padding = new Padding(10);
            tabDetailInfo.Size = new Size(944, 142);
            tabDetailInfo.TabIndex = 0;
            tabDetailInfo.Text = "Chi tiết dịch vụ";
            tabDetailInfo.UseVisualStyleBackColor = true;
            // 
            // lblDetailRecordId
            // 
            lblDetailRecordId.AutoSize = true;
            lblDetailRecordId.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblDetailRecordId.ForeColor = Color.FromArgb(71, 85, 105);
            lblDetailRecordId.Location = new Point(14, 20);
            lblDetailRecordId.Name = "lblDetailRecordId";
            lblDetailRecordId.Size = new Size(80, 21);
            lblDetailRecordId.TabIndex = 0;
            lblDetailRecordId.Text = "Mã HSBA";
            // 
            // txtDetailRecordId
            // 
            txtDetailRecordId.BackColor = Color.FromArgb(248, 250, 252);
            txtDetailRecordId.Font = new Font("Segoe UI", 9.5F);
            txtDetailRecordId.Location = new Point(200, 12);
            txtDetailRecordId.Name = "txtDetailRecordId";
            txtDetailRecordId.ReadOnly = true;
            txtDetailRecordId.Size = new Size(240, 29);
            txtDetailRecordId.TabIndex = 1;
            // 
            // lblDetailServiceType
            // 
            lblDetailServiceType.AutoSize = true;
            lblDetailServiceType.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblDetailServiceType.ForeColor = Color.FromArgb(71, 85, 105);
            lblDetailServiceType.Location = new Point(14, 56);
            lblDetailServiceType.Name = "lblDetailServiceType";
            lblDetailServiceType.Size = new Size(102, 21);
            lblDetailServiceType.TabIndex = 2;
            lblDetailServiceType.Text = "Loại dịch vụ";
            // 
            // txtDetailServiceType
            // 
            txtDetailServiceType.BackColor = Color.FromArgb(248, 250, 252);
            txtDetailServiceType.Font = new Font("Segoe UI", 9.5F);
            txtDetailServiceType.Location = new Point(200, 48);
            txtDetailServiceType.Name = "txtDetailServiceType";
            txtDetailServiceType.ReadOnly = true;
            txtDetailServiceType.Size = new Size(340, 29);
            txtDetailServiceType.TabIndex = 3;
            // 
            // lblDetailServiceDate
            // 
            lblDetailServiceDate.AutoSize = true;
            lblDetailServiceDate.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblDetailServiceDate.ForeColor = Color.FromArgb(71, 85, 105);
            lblDetailServiceDate.Location = new Point(14, 92);
            lblDetailServiceDate.Name = "lblDetailServiceDate";
            lblDetailServiceDate.Size = new Size(155, 21);
            lblDetailServiceDate.TabIndex = 4;
            lblDetailServiceDate.Text = "Ngày thực hiện DV";
            // 
            // txtDetailServiceDate
            // 
            txtDetailServiceDate.BackColor = Color.FromArgb(248, 250, 252);
            txtDetailServiceDate.Font = new Font("Segoe UI", 9.5F);
            txtDetailServiceDate.Location = new Point(200, 84);
            txtDetailServiceDate.Name = "txtDetailServiceDate";
            txtDetailServiceDate.ReadOnly = true;
            txtDetailServiceDate.Size = new Size(160, 29);
            txtDetailServiceDate.TabIndex = 5;
            // 
            // tabDetailResult
            // 
            tabDetailResult.Controls.Add(lblCurrentResult);
            tabDetailResult.Controls.Add(txtCurrentResult);
            tabDetailResult.Controls.Add(btnUpdateResult);
            tabDetailResult.Controls.Add(btnSaveResult);
            tabDetailResult.Controls.Add(btnCancelResult);
            tabDetailResult.Location = new Point(4, 30);
            tabDetailResult.Name = "tabDetailResult";
            tabDetailResult.Padding = new Padding(10);
            tabDetailResult.Size = new Size(944, 142);
            tabDetailResult.TabIndex = 1;
            tabDetailResult.Text = "Kết quả dịch vụ";
            tabDetailResult.UseVisualStyleBackColor = true;
            // 
            // lblCurrentResult
            // 
            lblCurrentResult.AutoSize = true;
            lblCurrentResult.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblCurrentResult.ForeColor = Color.FromArgb(30, 41, 59);
            lblCurrentResult.Location = new Point(10, 16);
            lblCurrentResult.Name = "lblCurrentResult";
            lblCurrentResult.Size = new Size(132, 21);
            lblCurrentResult.TabIndex = 0;
            lblCurrentResult.Text = "Kết quả dịch vụ:";
            // 
            // txtCurrentResult
            // 
            txtCurrentResult.BackColor = Color.FromArgb(229, 231, 235);
            txtCurrentResult.Font = new Font("Segoe UI", 9.5F);
            txtCurrentResult.ForeColor = Color.FromArgb(30, 41, 59);
            txtCurrentResult.Location = new Point(150, 10);
            txtCurrentResult.Multiline = true;
            txtCurrentResult.Name = "txtCurrentResult";
            txtCurrentResult.ReadOnly = true;
            txtCurrentResult.ScrollBars = ScrollBars.Vertical;
            txtCurrentResult.Size = new Size(530, 70);
            txtCurrentResult.TabIndex = 1;
            // 
            // btnUpdateResult
            // 
            btnUpdateResult.BackColor = Color.FromArgb(255, 140, 40);
            btnUpdateResult.FlatAppearance.BorderSize = 0;
            btnUpdateResult.FlatStyle = FlatStyle.Flat;
            btnUpdateResult.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnUpdateResult.ForeColor = Color.White;
            btnUpdateResult.Location = new Point(150, 90);
            btnUpdateResult.Name = "btnUpdateResult";
            btnUpdateResult.Size = new Size(100, 34);
            btnUpdateResult.TabIndex = 2;
            btnUpdateResult.Text = "Cập nhật";
            btnUpdateResult.UseVisualStyleBackColor = false;
            btnUpdateResult.Click += btnUpdateResult_Click;
            // 
            // btnSaveResult
            // 
            btnSaveResult.BackColor = Color.FromArgb(16, 185, 129);
            btnSaveResult.FlatAppearance.BorderSize = 0;
            btnSaveResult.FlatStyle = FlatStyle.Flat;
            btnSaveResult.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnSaveResult.ForeColor = Color.White;
            btnSaveResult.Location = new Point(150, 90);
            btnSaveResult.Name = "btnSaveResult";
            btnSaveResult.Size = new Size(100, 34);
            btnSaveResult.TabIndex = 3;
            btnSaveResult.Text = "Lưu";
            btnSaveResult.UseVisualStyleBackColor = false;
            btnSaveResult.Visible = false;
            btnSaveResult.Click += btnSaveResult_Click;
            // 
            // btnCancelResult
            // 
            btnCancelResult.BackColor = Color.FromArgb(100, 116, 139);
            btnCancelResult.FlatAppearance.BorderSize = 0;
            btnCancelResult.FlatStyle = FlatStyle.Flat;
            btnCancelResult.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnCancelResult.ForeColor = Color.White;
            btnCancelResult.Location = new Point(260, 90);
            btnCancelResult.Name = "btnCancelResult";
            btnCancelResult.Size = new Size(80, 34);
            btnCancelResult.TabIndex = 4;
            btnCancelResult.Text = "Hủy";
            btnCancelResult.UseVisualStyleBackColor = false;
            btnCancelResult.Visible = false;
            btnCancelResult.Click += btnCancelResult_Click;
            // 
            // tabPersonal
            // 
            tabPersonal.Controls.Add(tlpPersonalCenter);
            tabPersonal.Location = new Point(4, 32);
            tabPersonal.Name = "tabPersonal";
            tabPersonal.Padding = new Padding(8);
            tabPersonal.Size = new Size(964, 494);
            tabPersonal.TabIndex = 1;
            tabPersonal.Text = "Thông tin của tôi";
            tabPersonal.UseVisualStyleBackColor = true;
            // 
            // tlpPersonalCenter
            // 
            tlpPersonalCenter.ColumnCount = 3;
            tlpPersonalCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpPersonalCenter.ColumnStyles.Add(new ColumnStyle());
            tlpPersonalCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpPersonalCenter.Controls.Add(pnlPersonalContent, 1, 1);
            tlpPersonalCenter.Dock = DockStyle.Fill;
            tlpPersonalCenter.Location = new Point(8, 8);
            tlpPersonalCenter.Name = "tlpPersonalCenter";
            tlpPersonalCenter.RowCount = 3;
            tlpPersonalCenter.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPersonalCenter.RowStyles.Add(new RowStyle());
            tlpPersonalCenter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPersonalCenter.Size = new Size(948, 478);
            tlpPersonalCenter.TabIndex = 0;
            // 
            // pnlPersonalContent
            // 
            pnlPersonalContent.Controls.Add(lblUserId);
            pnlPersonalContent.Controls.Add(txtUserId);
            pnlPersonalContent.Controls.Add(lblFullName);
            pnlPersonalContent.Controls.Add(txtFullName);
            pnlPersonalContent.Controls.Add(lblGender);
            pnlPersonalContent.Controls.Add(txtGender);
            pnlPersonalContent.Controls.Add(lblBirthdate);
            pnlPersonalContent.Controls.Add(dtpBirthdate);
            pnlPersonalContent.Controls.Add(lblPhone);
            pnlPersonalContent.Controls.Add(txtPhone);
            pnlPersonalContent.Controls.Add(lblHometown);
            pnlPersonalContent.Controls.Add(txtHometown);
            pnlPersonalContent.Controls.Add(btnSaveInfo);
            pnlPersonalContent.Controls.Add(btnCancelEdit);
            pnlPersonalContent.Location = new Point(194, 43);
            pnlPersonalContent.Name = "pnlPersonalContent";
            pnlPersonalContent.Size = new Size(560, 360);
            pnlPersonalContent.TabIndex = 0;
            // 
            // lblUserId
            // 
            lblUserId.AutoSize = true;
            lblUserId.Font = new Font("Segoe UI", 10F);
            lblUserId.ForeColor = Color.FromArgb(51, 65, 85);
            lblUserId.Location = new Point(14, 6);
            lblUserId.Name = "lblUserId";
            lblUserId.Size = new Size(114, 23);
            lblUserId.TabIndex = 0;
            lblUserId.Text = "Mã nhân viên";
            // 
            // txtUserId
            // 
            txtUserId.BackColor = Color.FromArgb(229, 231, 235);
            txtUserId.Font = new Font("Segoe UI", 10F);
            txtUserId.Location = new Point(200, 0);
            txtUserId.Name = "txtUserId";
            txtUserId.ReadOnly = true;
            txtUserId.Size = new Size(340, 30);
            txtUserId.TabIndex = 1;
            // 
            // lblFullName
            // 
            lblFullName.AutoSize = true;
            lblFullName.Font = new Font("Segoe UI", 10F);
            lblFullName.ForeColor = Color.FromArgb(51, 65, 85);
            lblFullName.Location = new Point(14, 54);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(62, 23);
            lblFullName.TabIndex = 2;
            lblFullName.Text = "Họ tên";
            // 
            // txtFullName
            // 
            txtFullName.BackColor = Color.FromArgb(229, 231, 235);
            txtFullName.Font = new Font("Segoe UI", 10F);
            txtFullName.Location = new Point(200, 48);
            txtFullName.Name = "txtFullName";
            txtFullName.ReadOnly = true;
            txtFullName.Size = new Size(340, 30);
            txtFullName.TabIndex = 3;
            // 
            // lblGender
            // 
            lblGender.AutoSize = true;
            lblGender.Font = new Font("Segoe UI", 10F);
            lblGender.ForeColor = Color.FromArgb(51, 65, 85);
            lblGender.Location = new Point(14, 102);
            lblGender.Name = "lblGender";
            lblGender.Size = new Size(43, 23);
            lblGender.TabIndex = 4;
            lblGender.Text = "Phái";
            // 
            // txtGender
            // 
            txtGender.BackColor = Color.FromArgb(229, 231, 235);
            txtGender.Font = new Font("Segoe UI", 10F);
            txtGender.Location = new Point(200, 96);
            txtGender.Name = "txtGender";
            txtGender.ReadOnly = true;
            txtGender.Size = new Size(120, 30);
            txtGender.TabIndex = 5;
            // 
            // lblBirthdate
            // 
            lblBirthdate.AutoSize = true;
            lblBirthdate.Font = new Font("Segoe UI", 10F);
            lblBirthdate.ForeColor = Color.FromArgb(51, 65, 85);
            lblBirthdate.Location = new Point(14, 150);
            lblBirthdate.Name = "lblBirthdate";
            lblBirthdate.Size = new Size(86, 23);
            lblBirthdate.TabIndex = 6;
            lblBirthdate.Text = "Ngày sinh";
            // 
            // dtpBirthdate
            // 
            dtpBirthdate.CustomFormat = "dd/MM/yyyy";
            dtpBirthdate.Enabled = false;
            dtpBirthdate.Font = new Font("Segoe UI", 10F);
            dtpBirthdate.Format = DateTimePickerFormat.Custom;
            dtpBirthdate.Location = new Point(200, 144);
            dtpBirthdate.Name = "dtpBirthdate";
            dtpBirthdate.Size = new Size(200, 30);
            dtpBirthdate.TabIndex = 7;
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.Font = new Font("Segoe UI", 10F);
            lblPhone.ForeColor = Color.FromArgb(51, 65, 85);
            lblPhone.Location = new Point(14, 198);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(111, 23);
            lblPhone.TabIndex = 8;
            lblPhone.Text = "Số điện thoại";
            // 
            // txtPhone
            // 
            txtPhone.BackColor = Color.FromArgb(229, 231, 235);
            txtPhone.Font = new Font("Segoe UI", 10F);
            txtPhone.Location = new Point(200, 192);
            txtPhone.Name = "txtPhone";
            txtPhone.ReadOnly = true;
            txtPhone.Size = new Size(340, 30);
            txtPhone.TabIndex = 9;
            // 
            // lblHometown
            // 
            lblHometown.AutoSize = true;
            lblHometown.Font = new Font("Segoe UI", 10F);
            lblHometown.ForeColor = Color.FromArgb(51, 65, 85);
            lblHometown.Location = new Point(14, 246);
            lblHometown.Name = "lblHometown";
            lblHometown.Size = new Size(86, 23);
            lblHometown.TabIndex = 10;
            lblHometown.Text = "Quê quán";
            // 
            // txtHometown
            // 
            txtHometown.BackColor = Color.FromArgb(229, 231, 235);
            txtHometown.Font = new Font("Segoe UI", 10F);
            txtHometown.Location = new Point(200, 240);
            txtHometown.Name = "txtHometown";
            txtHometown.ReadOnly = true;
            txtHometown.Size = new Size(340, 30);
            txtHometown.TabIndex = 11;
            // 
            // btnSaveInfo
            // 
            btnSaveInfo.BackColor = Color.FromArgb(255, 140, 40);
            btnSaveInfo.FlatAppearance.BorderSize = 0;
            btnSaveInfo.FlatStyle = FlatStyle.Flat;
            btnSaveInfo.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnSaveInfo.ForeColor = Color.White;
            btnSaveInfo.Location = new Point(14, 298);
            btnSaveInfo.Name = "btnSaveInfo";
            btnSaveInfo.Size = new Size(140, 34);
            btnSaveInfo.TabIndex = 12;
            btnSaveInfo.Text = "Cập nhật";
            btnSaveInfo.UseVisualStyleBackColor = false;
            btnSaveInfo.Click += btnSaveInfo_Click;
            // 
            // btnCancelEdit
            // 
            btnCancelEdit.BackColor = Color.FromArgb(100, 116, 139);
            btnCancelEdit.Enabled = false;
            btnCancelEdit.FlatAppearance.BorderSize = 0;
            btnCancelEdit.FlatStyle = FlatStyle.Flat;
            btnCancelEdit.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnCancelEdit.ForeColor = Color.White;
            btnCancelEdit.Location = new Point(162, 298);
            btnCancelEdit.Name = "btnCancelEdit";
            btnCancelEdit.Size = new Size(80, 34);
            btnCancelEdit.TabIndex = 13;
            btnCancelEdit.Text = "Hủy";
            btnCancelEdit.UseVisualStyleBackColor = false;
            btnCancelEdit.Click += btnCancelEdit_Click;
            // 
            // pnlResultNote
            // 
            pnlResultNote.Location = new Point(0, 0);
            pnlResultNote.Name = "pnlResultNote";
            pnlResultNote.Size = new Size(200, 100);
            pnlResultNote.TabIndex = 0;
            // 
            // lblResultNote
            // 
            lblResultNote.Location = new Point(0, 0);
            lblResultNote.Name = "lblResultNote";
            lblResultNote.Size = new Size(100, 23);
            lblResultNote.TabIndex = 0;
            // 
            // pnlInfoNote
            // 
            pnlInfoNote.Location = new Point(0, 0);
            pnlInfoNote.Name = "pnlInfoNote";
            pnlInfoNote.Size = new Size(200, 100);
            pnlInfoNote.TabIndex = 0;
            // 
            // lblInfoNote
            // 
            lblInfoNote.Location = new Point(0, 0);
            lblInfoNote.Name = "lblInfoNote";
            lblInfoNote.Size = new Size(100, 23);
            lblInfoNote.TabIndex = 0;
            // 
            // btnEditInfo
            // 
            btnEditInfo.Location = new Point(0, 0);
            btnEditInfo.Name = "btnEditInfo";
            btnEditInfo.Size = new Size(75, 23);
            btnEditInfo.TabIndex = 0;
            // 
            // frmTechnician
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(241, 245, 249);
            ClientSize = new Size(1000, 640);
            Controls.Add(tabControlMain);
            Controls.Add(panelHeader);
            Name = "frmTechnician";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kỹ thuật viên";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            tabControlMain.ResumeLayout(false);
            tabServices.ResumeLayout(false);
            splitServices.Panel1.ResumeLayout(false);
            splitServices.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitServices).EndInit();
            splitServices.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvServices).EndInit();
            pnlSearchBar.ResumeLayout(false);
            pnlSearchBar.PerformLayout();
            tabDetailView.ResumeLayout(false);
            tabDetailInfo.ResumeLayout(false);
            tabDetailInfo.PerformLayout();
            tabDetailResult.ResumeLayout(false);
            tabDetailResult.PerformLayout();
            tabPersonal.ResumeLayout(false);
            tlpPersonalCenter.ResumeLayout(false);
            pnlPersonalContent.ResumeLayout(false);
            pnlPersonalContent.PerformLayout();
            ResumeLayout(false);
        }



        #endregion

        // ── field declarations ───────────────────────────────────────────
        private Panel       panelHeader;
        private Label       lblAppTitle;
        private Label       lblAppSubtitle;
        private Button      btnLogout;
        private TabControl  tabControlMain;

        // Tab Services
        private TabPage      tabServices;
        private Panel        pnlSearchBar;
        private Label        lblSearchRecord;
        private TextBox      txtSearchRecord;
        private Button       btnSearch;
        private Button       btnRefresh;
        private SplitContainer splitServices;
        private DataGridView dgvServices;
        private TabControl   tabDetailView;
        private TabPage      tabDetailInfo;
        private TabPage      tabDetailResult;
        private Label        lblDetailRecordId;
        private TextBox      txtDetailRecordId;
        private Label        lblDetailServiceType;
        private TextBox      txtDetailServiceType;
        private Label        lblDetailServiceDate;
        private TextBox      txtDetailServiceDate;
        private Label        lblCurrentResult;
        private TextBox      txtCurrentResult;
        private Button       btnUpdateResult;
        private Button       btnSaveResult;
        private Button       btnCancelResult;
        private Panel        pnlResultNote;
        private Label        lblResultNote;

        // Tab Personal
        private TabPage       tabPersonal;
        private Panel         pnlInfoNote;
        private Label         lblInfoNote;
        private Label         lblUserId;
        private TextBox       txtUserId;
        private Label         lblFullName;
        private TextBox       txtFullName;
        private Label         lblGender;
        private TextBox       txtGender;
        private Label         lblBirthdate;
        private DateTimePicker dtpBirthdate;
        private Label         lblPhone;
        private TextBox       txtPhone;
        private Label         lblHometown;
        private TextBox       txtHometown;
        private Button        btnEditInfo;
        private Button        btnSaveInfo;
        private Button        btnCancelEdit;
        private TableLayoutPanel tlpPersonalCenter;
        private Panel pnlPersonalContent;
    }
}