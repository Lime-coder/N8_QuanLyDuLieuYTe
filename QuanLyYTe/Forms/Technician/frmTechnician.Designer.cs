namespace QuanLyYTe.Forms.Technician
{
    partial class frmTechnician
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
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
            // ── top-level controls ───────────────────────────────────────
            panelHeader    = new Panel();
            btnLogout      = new Button();
            lblAppTitle    = new Label();
            lblAppSubtitle = new Label();
            tabControlMain = new TabControl();
            tabServices    = new TabPage();
            tabPersonal    = new TabPage();

            // ── Tab: Dịch vụ của tôi ─────────────────────────────────────
            pnlSearchBar      = new Panel();
            lblSearchRecord   = new Label();
            txtSearchRecord   = new TextBox();
            btnSearch         = new Button();
            btnRefresh        = new Button();
            splitServices     = new SplitContainer();
            dgvServices       = new DataGridView();
            tabDetailView     = new TabControl();
            tabDetailInfo     = new TabPage();
            tabDetailResult   = new TabPage();

            // detail – Chi tiết dịch vụ
            lblDetailRecordId    = new Label();
            txtDetailRecordId    = new TextBox();
            lblDetailServiceType = new Label();
            txtDetailServiceType = new TextBox();
            lblDetailServiceDate = new Label();
            txtDetailServiceDate = new TextBox();

            // detail – Kết quả hiện tại
            pnlResultNote    = new Panel();
            lblResultNote    = new Label();
            lblCurrentResult = new Label();
            txtCurrentResult = new TextBox();
            btnUpdateResult  = new Button();

            // ── Tab: Thông tin của tôi ────────────────────────────────────
            pnlInfoNote   = new Panel();
            lblInfoNote   = new Label();
            lblUserId     = new Label();
            txtUserId     = new TextBox();
            lblFullName   = new Label();
            txtFullName   = new TextBox();
            lblGender     = new Label();
            txtGender     = new TextBox();
            lblBirthdate  = new Label();
            dtpBirthdate  = new DateTimePicker();
            lblPhone      = new Label();
            txtPhone      = new TextBox();
            lblHometown   = new Label();
            txtHometown   = new TextBox();
            btnEditInfo   = new Button();
            btnSaveInfo   = new Button();
            btnCancelEdit = new Button();

            ((System.ComponentModel.ISupportInitialize)dgvServices).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitServices).BeginInit();
            splitServices.Panel1.SuspendLayout();
            splitServices.Panel2.SuspendLayout();
            panelHeader.SuspendLayout();
            SuspendLayout();

            // ════════════════════════════════════════════════════════════
            //  panelHeader
            // ════════════════════════════════════════════════════════════
            panelHeader.BackColor = Color.FromArgb(30, 41, 59);
            panelHeader.Controls.Add(btnLogout);
            panelHeader.Controls.Add(lblAppSubtitle);
            panelHeader.Controls.Add(lblAppTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1000, 80);

            lblAppTitle.AutoSize = true;
            lblAppTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            lblAppTitle.ForeColor = Color.White;
            lblAppTitle.Location = new Point(18, 12);
            lblAppTitle.Name = "lblAppTitle";
            lblAppTitle.Text = "Kỹ thuật viên";

            lblAppSubtitle.AutoSize = true;
            lblAppSubtitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblAppSubtitle.ForeColor = Color.Gainsboro;
            lblAppSubtitle.Location = new Point(21, 46);
            lblAppSubtitle.Name = "lblAppSubtitle";
            lblAppSubtitle.Text = "Quản lý dịch vụ được phân công và thông tin cá nhân";

            btnLogout.Text = "Đăng xuất";
            btnLogout.Location = new Point(880, 23);
            btnLogout.Size = new Size(100, 34);
            btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.BackColor = Color.FromArgb(220, 38, 38);
            btnLogout.ForeColor = Color.White;
            btnLogout.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.Click += new EventHandler(btnLogout_Click);
            btnLogout.Name = "btnLogout";

            // ════════════════════════════════════════════════════════════
            //  tabControlMain
            // ════════════════════════════════════════════════════════════
            tabControlMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControlMain.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tabControlMain.Location = new Point(14, 96);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.Size = new Size(972, 530);
            tabControlMain.TabIndex = 1;
            tabControlMain.Controls.Add(tabServices);
            tabControlMain.Controls.Add(tabPersonal);

            // ════════════════════════════════════════════════════════════
            //  tabServices  — "Dịch vụ của tôi"
            // ════════════════════════════════════════════════════════════
            tabServices.Text = "Dịch vụ của tôi";
            tabServices.Padding = new Padding(6);
            tabServices.UseVisualStyleBackColor = true;

            // ── search bar ────────────────────────────────────────────
            pnlSearchBar.Dock = DockStyle.Top;
            pnlSearchBar.Height = 50;
            pnlSearchBar.Padding = new Padding(6, 8, 6, 6);
            pnlSearchBar.BackColor = Color.FromArgb(248, 250, 252);

            lblSearchRecord.AutoSize = true;
            lblSearchRecord.Text = "Tìm mã HSBA/dịch vụ:";
            lblSearchRecord.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            lblSearchRecord.ForeColor = Color.FromArgb(51, 65, 85);
            lblSearchRecord.Location = new Point(6, 15);

            txtSearchRecord.Location = new Point(190, 11);
            txtSearchRecord.Width = 260;
            txtSearchRecord.Height = 28;
            txtSearchRecord.Font = new Font("Segoe UI", 10F);
            txtSearchRecord.PlaceholderText = "Nhập mã HSBA/dịch vụ cần tìm";
            txtSearchRecord.Name = "txtSearchRecord";
            txtSearchRecord.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) btnSearch_Click(s, e); };

            btnSearch.Location = new Point(460, 10);
            btnSearch.Size = new Size(80, 30);
            btnSearch.Text = "Tìm";
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.BackColor = Color.FromArgb(255, 140, 40);
            btnSearch.ForeColor = Color.White;
            btnSearch.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnSearch.Name = "btnSearch";

            btnRefresh.Location = new Point(550, 10);
            btnRefresh.Size = new Size(90, 30);
            btnRefresh.Text = "Làm mới";
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.BackColor = Color.FromArgb(15, 118, 110);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += new EventHandler(btnRefresh_Click);
            btnRefresh.Name = "btnRefresh";

            pnlSearchBar.Controls.Add(lblSearchRecord);
            pnlSearchBar.Controls.Add(txtSearchRecord);
            pnlSearchBar.Controls.Add(btnSearch);
            pnlSearchBar.Controls.Add(btnRefresh);

            // ── service grid ──────────────────────────────────────────
            dgvServices.AllowUserToAddRows = false;
            dgvServices.AllowUserToDeleteRows = false;
            dgvServices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvServices.BackgroundColor = Color.White;
            dgvServices.BorderStyle = BorderStyle.None;
            dgvServices.ColumnHeadersHeight = 36;
            dgvServices.EnableHeadersVisualStyles = false;
            dgvServices.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            dgvServices.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 41, 59);
            dgvServices.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvServices.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            dgvServices.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgvServices.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 41, 59);
            dgvServices.GridColor = Color.FromArgb(226, 232, 240);
            dgvServices.Name = "dgvServices";
            dgvServices.ReadOnly = true;
            dgvServices.RowHeadersVisible = false;
            dgvServices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvServices.Dock = DockStyle.Fill;
            dgvServices.TabIndex = 1;
            dgvServices.SelectionChanged += new EventHandler(dgvServices_SelectionChanged);

            // ── SplitContainer: top = grid, bottom = detail ───────────
            splitServices.Dock = DockStyle.Fill;
            splitServices.Orientation = Orientation.Horizontal;
            splitServices.SplitterDistance = 260;  // initial height of top panel (grid)
            splitServices.SplitterWidth = 8;       // thickness of the draggable gap
            splitServices.BackColor = Color.FromArgb(203, 213, 225); // gap colour
            splitServices.Panel1MinSize = 80;
            splitServices.Panel2MinSize = 120;
            splitServices.Name = "splitServices";

            // Panel1 (top): search bar + grid
            splitServices.Panel1.Controls.Add(dgvServices);   // Fill
            splitServices.Panel1.Controls.Add(pnlSearchBar);  // Top

            // Panel2 (bottom): detail tabs — with a small top padding for breathing room
            splitServices.Panel2.Padding = new Padding(0, 4, 0, 0);

            // ── detail tabs ───────────────────────────────────────────
            tabDetailView.Dock = DockStyle.Fill;
            tabDetailView.Name = "tabDetailView";
            tabDetailView.Font = new Font("Segoe UI", 9.5f);
            tabDetailView.Controls.Add(tabDetailInfo);
            tabDetailView.Controls.Add(tabDetailResult);
            splitServices.Panel2.Controls.Add(tabDetailView);

            // Chi tiết dịch vụ
            tabDetailInfo.Text = "Chi tiết dịch vụ";
            tabDetailInfo.Padding = new Padding(10);
            tabDetailInfo.UseVisualStyleBackColor = true;

            SetupDetailLabel(lblDetailRecordId,    14, 16,  "Mã HSBA");
            SetupDetailBox(txtDetailRecordId,      200, 12, 240);
            SetupDetailLabel(lblDetailServiceType, 14, 52,  "Loại dịch vụ");
            SetupDetailBox(txtDetailServiceType,   200, 48, 340);
            SetupDetailLabel(lblDetailServiceDate, 14, 88,  "Ngày thực hiện DV");
            SetupDetailBox(txtDetailServiceDate,   200, 84, 160);

            tabDetailInfo.Controls.AddRange(new Control[]
            {
                lblDetailRecordId, txtDetailRecordId,
                lblDetailServiceType, txtDetailServiceType,
                lblDetailServiceDate, txtDetailServiceDate
            });

            // Kết quả hiện tại
            tabDetailResult.Text = "Kết quả hiện tại";
            tabDetailResult.Padding = new Padding(10);
            tabDetailResult.UseVisualStyleBackColor = true;

            lblCurrentResult.AutoSize = true;
            lblCurrentResult.Location = new Point(10, 14);
            lblCurrentResult.Text = "Kết quả hiện tại";
            lblCurrentResult.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            lblCurrentResult.ForeColor = Color.FromArgb(30, 41, 59);

            txtCurrentResult.Location = new Point(150, 10);
            txtCurrentResult.Size = new Size(560, 24);
            txtCurrentResult.ReadOnly = true;
            txtCurrentResult.Font = new Font("Segoe UI", 9.5f);
            txtCurrentResult.ForeColor = Color.FromArgb(30, 41, 59);
            txtCurrentResult.BackColor = Color.FromArgb(229, 231, 235);
            txtCurrentResult.Name = "txtCurrentResult";

            btnUpdateResult.Location = new Point(10, 46);
            btnUpdateResult.Size = new Size(160, 34);
            btnUpdateResult.Text = "Cập nhật";
            btnUpdateResult.FlatStyle = FlatStyle.Flat;
            btnUpdateResult.BackColor = Color.FromArgb(255, 140, 40);
            btnUpdateResult.ForeColor = Color.White;
            btnUpdateResult.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnUpdateResult.FlatAppearance.BorderSize = 0;
            btnUpdateResult.Click += new EventHandler(btnUpdateResult_Click);
            btnUpdateResult.Name = "btnUpdateResult";

            btnSaveResult = new Button();
            btnSaveResult.Location = new Point(180, 46);
            btnSaveResult.Size = new Size(100, 34);
            btnSaveResult.Text = "Lưu";
            btnSaveResult.FlatStyle = FlatStyle.Flat;
            btnSaveResult.BackColor = Color.FromArgb(16, 185, 129); // Emerald green
            btnSaveResult.ForeColor = Color.White;
            btnSaveResult.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnSaveResult.FlatAppearance.BorderSize = 0;
            btnSaveResult.Visible = false; // Hidden until Edit is clicked
            btnSaveResult.Click += new EventHandler(btnSaveResult_Click);
            btnSaveResult.Name = "btnSaveResult";

            btnCancelResult = new Button();
            btnCancelResult.Location = new Point(100, 46); // Place next to btnUpdateResult (which we'll reposition)
            btnCancelResult.Size = new Size(80, 34);
            btnCancelResult.Text = "Hủy";
            btnCancelResult.FlatStyle = FlatStyle.Flat;
            btnCancelResult.BackColor = Color.FromArgb(100, 116, 139); // Gray
            btnCancelResult.ForeColor = Color.White;
            btnCancelResult.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnCancelResult.FlatAppearance.BorderSize = 0;
            btnCancelResult.Visible = false; // Hidden until Edit is clicked
            btnCancelResult.Click += new EventHandler(btnCancelResult_Click);
            btnCancelResult.Name = "btnCancelResult";

            // Adjust locations: when editing, "Lưu" is at X=10, "Hủy" is at X=100
            btnUpdateResult.Location = new Point(10, 46);
            btnUpdateResult.Size = new Size(130, 34);
            btnUpdateResult.Text = "Cập nhật";
            btnSaveResult.Location = new Point(10, 46);
            btnSaveResult.Size = new Size(80, 34);

            tabDetailResult.Controls.AddRange(new Control[]
            {
                lblCurrentResult, txtCurrentResult, btnUpdateResult, btnSaveResult, btnCancelResult
            });

            // Wire up tabServices — SplitContainer fills the entire tab
            tabServices.Controls.Add(splitServices);

            // ════════════════════════════════════════════════════════════
            //  tabPersonal  — "Thông tin của tôi"
            // ════════════════════════════════════════════════════════════
            tabPersonal.Text = "Thông tin của tôi";
            tabPersonal.Padding = new Padding(8);
            tabPersonal.UseVisualStyleBackColor = true;

            TableLayoutPanel tlpPersonalCenter = new TableLayoutPanel();
            tlpPersonalCenter.Dock = DockStyle.Fill;
            tlpPersonalCenter.ColumnCount = 3;
            tlpPersonalCenter.RowCount = 3;
            tlpPersonalCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpPersonalCenter.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpPersonalCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpPersonalCenter.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPersonalCenter.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpPersonalCenter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Panel pnlPersonalContent = new Panel();
            pnlPersonalContent.Size = new Size(560, 360);
            tlpPersonalCenter.Controls.Add(pnlPersonalContent, 1, 1);

            // Layout helper
            int lx = 14, fx = 200, fw = 340, rowH = 48;
            int row = 0; // reset row to 0 since it's now inside the Panel

            SetupInfoRow(lblUserId,    "Mã nhân viên",  lx, row);
            SetupInfoBox(txtUserId,    fx, row, fw, true);   row += rowH;

            SetupInfoRow(lblFullName,  "Họ tên",        lx, row);
            SetupInfoBox(txtFullName,  fx, row, fw, true);   row += rowH;

            SetupInfoRow(lblGender,    "Phái",          lx, row);
            SetupInfoBox(txtGender,    fx, row, 120, true);  row += rowH;

            SetupInfoRow(lblBirthdate, "Ngày sinh",     lx, row);
            dtpBirthdate.Location = new Point(fx, row);
            dtpBirthdate.Width = 200;
            dtpBirthdate.Enabled = false;
            dtpBirthdate.Font = new Font("Segoe UI", 10F);
            dtpBirthdate.Format = DateTimePickerFormat.Custom;
            dtpBirthdate.CustomFormat = "dd/MM/yyyy";
            dtpBirthdate.Name = "dtpBirthdate";
            row += rowH;

            // ── Chuyên khoa (ROLE) intentionally omitted ──

            SetupInfoRow(lblPhone,    "Số điện thoại", lx, row);
            SetupInfoBox(txtPhone,    fx, row, fw, true);    row += rowH;

            SetupInfoRow(lblHometown, "Quê quán",      lx, row);
            SetupInfoBox(txtHometown, fx, row, fw, true);    row += rowH;

            // Read-only info fields — light gray background
            txtUserId.BackColor   = Color.FromArgb(229, 231, 235);
            txtFullName.BackColor = Color.FromArgb(229, 231, 235);
            txtGender.BackColor   = Color.FromArgb(229, 231, 235);
            // Phone & Hometown start read-only but are editable after Save
            txtPhone.BackColor    = Color.FromArgb(229, 231, 235);
            txtHometown.BackColor = Color.FromArgb(229, 231, 235);

            btnSaveInfo.Location = new Point(14, row + 10);
            btnSaveInfo.Size = new Size(140, 34);
            btnSaveInfo.Text = "Cập nhật";
            btnSaveInfo.FlatStyle = FlatStyle.Flat;
            btnSaveInfo.BackColor = Color.FromArgb(255, 140, 40);
            btnSaveInfo.ForeColor = Color.White;
            btnSaveInfo.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnSaveInfo.FlatAppearance.BorderSize = 0;
            btnSaveInfo.Click += new EventHandler(btnSaveInfo_Click);
            btnSaveInfo.Name = "btnSaveInfo";

            btnCancelEdit.Location = new Point(162, row + 10);
            btnCancelEdit.Size = new Size(80, 34);
            btnCancelEdit.Text = "Hủy";
            btnCancelEdit.FlatStyle = FlatStyle.Flat;
            btnCancelEdit.BackColor = Color.FromArgb(100, 116, 139);
            btnCancelEdit.ForeColor = Color.White;
            btnCancelEdit.Font = new Font("Segoe UI", 9.5f);
            btnCancelEdit.FlatAppearance.BorderSize = 0;
            btnCancelEdit.Enabled = false;
            btnCancelEdit.Click += new EventHandler(btnCancelEdit_Click);
            btnCancelEdit.Name = "btnCancelEdit";

            pnlPersonalContent.Controls.AddRange(new Control[]
            {
                lblUserId, txtUserId,
                lblFullName, txtFullName,
                lblGender, txtGender,
                lblBirthdate, dtpBirthdate,
                lblPhone, txtPhone,
                lblHometown, txtHometown,
                btnSaveInfo, btnCancelEdit
            });
            tabPersonal.Controls.Add(tlpPersonalCenter);

            // ════════════════════════════════════════════════════════════
            //  frmTechnician
            // ════════════════════════════════════════════════════════════
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(241, 245, 249);
            ClientSize = new Size(1000, 640);
            Controls.Add(tabControlMain);
            Controls.Add(panelHeader);
            Name = "frmTechnician";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kỹ thuật viên";

            ((System.ComponentModel.ISupportInitialize)dgvServices).EndInit();
            splitServices.Panel1.ResumeLayout(false);
            splitServices.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitServices).EndInit();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        // ── layout helpers ───────────────────────────────────────────────
        private static void SetupDetailLabel(Label lbl, int x, int y, string text)
        {
            lbl.AutoSize = true;
            lbl.Location = new Point(x, y + 4);
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(71, 85, 105);
        }

        private static void SetupDetailBox(TextBox txt, int x, int y, int width)
        {
            txt.Location = new Point(x, y);
            txt.Width = width;
            txt.ReadOnly = true;
            txt.Font = new Font("Segoe UI", 9.5f);
            txt.BackColor = Color.FromArgb(248, 250, 252);
        }

        private static void SetupInfoRow(Label lbl, string text, int x, int y)
        {
            lbl.AutoSize = true;
            lbl.Location = new Point(x, y + 6);
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 10f);
            lbl.ForeColor = Color.FromArgb(51, 65, 85);
        }

        private static void SetupInfoBox(TextBox txt, int x, int y, int width, bool readOnly)
        {
            txt.Location = new Point(x, y);
            txt.Width = width;
            txt.Height = 28;
            txt.Font = new Font("Segoe UI", 10F);
            txt.ReadOnly = readOnly;
            txt.BackColor = readOnly ? Color.FromArgb(248, 250, 252) : Color.White;
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
    }
}