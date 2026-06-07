using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyYTe.Common;

namespace QuanLyYTe.Forms.Patient
{
    partial class frmPatient
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
            this.SuspendLayout();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Name = "frmPatient";
            this.Text = "Hồ Sơ Bệnh Nhân";
            this.ResumeLayout(false);
        }

        private static readonly Color Orange = Color.FromArgb(255, 140, 40);
        private static readonly Color OrangeHov = Color.FromArgb(255, 165, 80);
        private static readonly Color ActiveBg = Color.FromArgb(45, 255, 140, 40);
        private static readonly Color DarkBg = Color.FromArgb(24, 24, 28);
        private static readonly Color ContentBg = Color.FromArgb(245, 244, 242);

        private Panel pnlSidebar = null!;
        private Panel pnlTopbar = null!;
        private Panel pnlContent = null!;
        private Label lblPageTitle = null!;
        private Label lblPageBreadcrumb = null!;
        private Button _activeNavBtn = null!;

        private Panel pnlProfile = null!;
        private Panel pnlRecords = null!;

        private Label lblPatientId = null!;
        private Label lblFullName = null!;
        private Label lblGender = null!;
        private Label lblBirthdate = null!;
        private Label lblIdCard = null!;
        private TextBox txtMedicalHistory = null!;
        private TextBox txtFamilyMedicalHistory = null!;
        private TextBox txtDrugAllergies = null!;
        private TextBox txtHouseNo = null!;
        private TextBox txtStreet = null!;
        private TextBox txtDistrict = null!;
        private TextBox txtCityProvince = null!;
        private Button btnSaveContact = null!;

        private DataGridView dgvRecords = null!;
        private DataGridView dgvPrescriptions = null!;
        private DataGridView dgvServices = null!;

        private DateTimePicker dtpFrom = null!;
        private DateTimePicker dtpTo = null!;
        private ComboBox cboDept = null!;
        private ComboBox cboDoctor = null!;
        private TextBox txtSearchRecords = null!;
        private Button btnClearFilter = null!;
        private DataTable _allRecordsTable = null!;

        private Label lblDetailTitle = null!;

        private void InitializeCustomUI()
        {
            SuspendLayout();
            Text = "Hospital - Patient Portal";
            ClientSize = new Size(1100, 700);
            MinimumSize = new Size(950, 600);
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            BackColor = ContentBg;
            Font = new Font("Segoe UI", 9f);

            pnlSidebar = new Panel { Dock = DockStyle.Left, Width = 240, BackColor = DarkBg };

            var pnlLogo = new Panel { Dock = DockStyle.Top, Height = 88, BackColor = Color.FromArgb(30, 30, 35) };
            pnlLogo.Controls.Add(new Label { Text = "Hospital Portal", Font = new Font("Segoe UI", 14f, FontStyle.Bold), ForeColor = Orange, AutoSize = true, Location = new Point(20, 18) });
            pnlLogo.Controls.Add(new Label { Text = "Patient Management", Font = new Font("Segoe UI", 7.5f), ForeColor = Color.FromArgb(120, 120, 130), AutoSize = true, Location = new Point(22, 52) });

            var pnlNav = new Panel { Dock = DockStyle.Fill };
            Button btnNavProfile = CreateNavButton("  Hồ sơ cá nhân", 0);
            Button btnNavRecords = CreateNavButton("  Lịch sử khám", 1);
            Button btnNavLogout = CreateNavButton("  Đăng xuất", 2);
            btnNavLogout.ForeColor = Color.FromArgb(220, 80, 80);
            
            btnNavProfile.Click += (s, e) => { SetActiveNav(btnNavProfile); ShowPanel(pnlProfile, "Hồ sơ cá nhân", "Patient / Profile"); };
            btnNavRecords.Click += (s, e) => { SetActiveNav(btnNavRecords); ShowPanel(pnlRecords, "Lịch sử khám", "Patient / Medical Records"); };
            btnNavLogout.Click += BtnLogout_Click;

            pnlNav.Controls.AddRange(new Control[] { btnNavProfile, btnNavRecords, btnNavLogout });
            pnlSidebar.Controls.Add(pnlNav);
            pnlSidebar.Controls.Add(pnlLogo);

            pnlTopbar = new Panel { Dock = DockStyle.Top, Height = 68, BackColor = Color.White };
            lblPageTitle = new Label { Font = new Font("Segoe UI", 15f, FontStyle.Bold), ForeColor = Color.FromArgb(28, 28, 32), AutoSize = true, Location = new Point(28, 10) };
            lblPageBreadcrumb = new Label { Font = new Font("Segoe UI", 8.5f), ForeColor = Color.FromArgb(160, 160, 170), AutoSize = true, Location = new Point(30, 42) };
            var lblUserInfo = new Label { Text = $"{AppSession.CurrentUsername.ToUpper()}  ·  PATIENT", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Orange, AutoSize = false, Size = new Size(300, 20), TextAlign = ContentAlignment.MiddleRight, Anchor = AnchorStyles.Top | AnchorStyles.Right, Location = new Point(pnlTopbar.Width - 324, 24) };
            
            pnlTopbar.Controls.AddRange(new Control[] { lblPageTitle, lblPageBreadcrumb, lblUserInfo });

            var pnlDivider = new Panel { Dock = DockStyle.Top, Height = 3, BackColor = Orange };

            pnlContent = new Panel { Dock = DockStyle.Fill, Padding = new Padding(24) };

            BuildProfilePanel();
            BuildRecordsPanel();

            pnlContent.Controls.Add(pnlProfile);
            pnlContent.Controls.Add(pnlRecords);

            var pnlMain = new Panel { Dock = DockStyle.Fill };
            pnlMain.Controls.Add(pnlContent);
            pnlMain.Controls.Add(pnlDivider);
            pnlMain.Controls.Add(pnlTopbar);

            Controls.Add(pnlMain);
            Controls.Add(pnlSidebar);

            SetActiveNav(btnNavProfile);
            ShowPanel(pnlProfile, "Hồ sơ cá nhân", "Patient / Profile");
            ResumeLayout(false);
        }

        
private Button CreateNavButton(string text, int index)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(190, 190, 200),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(16, 0, 0, 0),
                Size = new Size(216, 46),
                Location = new Point(12, 10 + index * 52),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 46);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(50, 50, 58);
            return btn;
        }

        

        private void BuildRecordsPanel()
        {
            pnlRecords = new Panel { Dock = DockStyle.Fill, BackColor = ContentBg };

            var splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 350,
                SplitterWidth = 8,
                BackColor = ContentBg
            };

            var pnlTop = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12) };
            var lblTopTitle = new Label { Text = "DANH SÁCH HỒ SƠ KHÁM", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Orange, Dock = DockStyle.Top, Height = 24 };

            var pnlFilterBar = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = Color.FromArgb(250, 250, 252),
                ColumnCount = 10,
                RowCount = 1,
                Padding = new Padding(4, 8, 4, 0)
            };
            
            pnlFilterBar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            pnlFilterBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            pnlFilterBar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            pnlFilterBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            pnlFilterBar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            pnlFilterBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            pnlFilterBar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            pnlFilterBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            pnlFilterBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            pnlFilterBar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            Label CreateLbl(string text) => new Label { Text = text, AutoSize = true, Font = new Font("Segoe UI", 8.5f), ForeColor = Color.FromArgb(80, 80, 90), Margin = new Padding(0, 5, 0, 0) };

            dtpFrom = new DateTimePicker { Format = DateTimePickerFormat.Short, Font = new Font("Segoe UI", 9f), Anchor = AnchorStyles.Left | AnchorStyles.Right, Margin = new Padding(4, 0, 8, 0), Value = DateTime.Now.AddMonths(-6) };
            dtpTo = new DateTimePicker { Format = DateTimePickerFormat.Short, Font = new Font("Segoe UI", 9f), Anchor = AnchorStyles.Left | AnchorStyles.Right, Margin = new Padding(4, 0, 8, 0), Value = DateTime.Now };

            cboDept = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9f), Anchor = AnchorStyles.Left | AnchorStyles.Right, Margin = new Padding(4, 0, 8, 0) };
            cboDoctor = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9f), Anchor = AnchorStyles.Left | AnchorStyles.Right, Margin = new Padding(4, 0, 8, 0) };

            txtSearchRecords = new TextBox
            {
                PlaceholderText = "Tìm chẩn đoán, kết luận...",
                Font = new Font("Segoe UI", 9f),
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(4, 0, 8, 0),
                Height = 27
            };

            btnClearFilter = new Button
            {
                Text = "✕ Xóa",
                Font = new Font("Segoe UI", 8.5f),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(64, 25),
                BackColor = Color.FromArgb(245, 245, 250),
                ForeColor = Color.FromArgb(80, 80, 90),
                Cursor = Cursors.Hand,
                Margin = new Padding(4, 6, 0, 0)
            };
            btnClearFilter.FlatAppearance.BorderColor = Color.FromArgb(210, 210, 220);

            pnlFilterBar.Controls.Add(CreateLbl("Từ ngày:"), 0, 0);
            pnlFilterBar.Controls.Add(dtpFrom, 1, 0);
            pnlFilterBar.Controls.Add(CreateLbl("Đến:"), 2, 0);
            pnlFilterBar.Controls.Add(dtpTo, 3, 0);
            pnlFilterBar.Controls.Add(CreateLbl("Khoa:"), 4, 0);
            pnlFilterBar.Controls.Add(cboDept, 5, 0);
            pnlFilterBar.Controls.Add(CreateLbl("Bác sĩ:"), 6, 0);
            pnlFilterBar.Controls.Add(cboDoctor, 7, 0);
            pnlFilterBar.Controls.Add(txtSearchRecords, 8, 0);
            pnlFilterBar.Controls.Add(btnClearFilter, 9, 0);

            dgvRecords = CreateGridView();
            dgvRecords.SelectionChanged += DgvRecords_SelectionChanged;

            dtpFrom.ValueChanged += (s, e) => FilterRecords();
            dtpTo.ValueChanged += (s, e) => FilterRecords();
            cboDept.SelectedIndexChanged += (s, e) => FilterRecords();
            cboDoctor.SelectedIndexChanged += (s, e) => FilterRecords();
            txtSearchRecords.TextChanged += (s, e) => FilterRecords();
            btnClearFilter.Click += (s, e) =>
            {
                txtSearchRecords.Clear();
                dtpFrom.Value = DateTime.Now.AddMonths(-6);
                dtpTo.Value = DateTime.Now;
                if (cboDept.Items.Count > 0) cboDept.SelectedIndex = 0;
                if (cboDoctor.Items.Count > 0) cboDoctor.SelectedIndex = 0;
                FilterRecords();
            };

            pnlTop.Controls.Add(dgvRecords);
            pnlTop.Controls.Add(pnlFilterBar);
            pnlTop.Controls.Add(lblTopTitle);

            var pnlBottom = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12) };
            lblDetailTitle = new Label { Text = "CHI TIẾT HỒ SƠ: Chưa chọn", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Orange, Dock = DockStyle.Top, Height = 24 };
            
            var splitBottom = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, BackColor = Color.White };
            splitBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            splitBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            
            var pnlLeft = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 0, 8, 0) };
            var lblLeft = new Label { Text = "Đơn thuốc", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), Dock = DockStyle.Top, Height = 20 };
            dgvPrescriptions = CreateGridView();
            pnlLeft.Controls.Add(dgvPrescriptions);
            pnlLeft.Controls.Add(lblLeft);

            var pnlRight = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8, 0, 0, 0) };
            var lblRight = new Label { Text = "Dịch vụ y tế", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), Dock = DockStyle.Top, Height = 20 };
            dgvServices = CreateGridView();
            pnlRight.Controls.Add(dgvServices);
            pnlRight.Controls.Add(lblRight);

            splitBottom.Controls.Add(pnlLeft, 0, 0);
            splitBottom.Controls.Add(pnlRight, 1, 0);
            
            pnlBottom.Controls.Add(splitBottom);
            pnlBottom.Controls.Add(lblDetailTitle);

            splitContainer.Panel1.Controls.Add(pnlTop);
            splitContainer.Panel2.Controls.Add(pnlBottom);
            pnlRecords.Controls.Add(splitContainer);
        }
private DataGridView CreateGridView()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9f),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 40,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                    BackColor = Color.FromArgb(245, 245, 250),
                    ForeColor = Color.FromArgb(60, 60, 70),
                    SelectionBackColor = Color.FromArgb(245, 245, 250),
                    Padding = new Padding(4, 8, 4, 8)
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Padding = new Padding(4, 6, 4, 6),
                    SelectionBackColor = Color.FromArgb(255, 240, 230),
                    SelectionForeColor = Color.FromArgb(28, 28, 32)
                },
                RowTemplate = { Height = 36 },
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
        }

        

        #endregion
    }
}
