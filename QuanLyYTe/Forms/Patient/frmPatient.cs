using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyYTe.Common;
using QuanLyYTe.Services;

namespace QuanLyYTe.Forms.Patient
{
    public partial class frmPatient : Form
    {
        private readonly PatientService _service = new PatientService();

        // ── Styling Constants ─────────────────────────────────────────
        private static readonly Color Orange = Color.FromArgb(255, 140, 40);
        private static readonly Color OrangeHov = Color.FromArgb(255, 165, 80);
        private static readonly Color ActiveBg = Color.FromArgb(45, 255, 140, 40);
        private static readonly Color DarkBg = Color.FromArgb(24, 24, 28);
        private static readonly Color ContentBg = Color.FromArgb(245, 244, 242);

        // ── Core Layout ───────────────────────────────────────────────
        private Panel pnlSidebar = null!;
        private Panel pnlTopbar = null!;
        private Panel pnlContent = null!;
        private Label lblPageTitle = null!;
        private Label lblPageBreadcrumb = null!;
        private Button _activeNavBtn = null!;

        private Panel pnlProfile = null!;
        private Panel pnlRecords = null!;

        // ── Profile Controls ──────────────────────────────────────────
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

        // ── Records Controls ──────────────────────────────────────────
        private DataGridView dgvRecords = null!;
        private DataGridView dgvPrescriptions = null!;
        private DataGridView dgvServices = null!;
        private Label lblDetailTitle = null!;

        public frmPatient()
        {
            InitializeComponent();
            LoadProfile();
            LoadMedicalRecords();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            Text = "Hospital - Patient Portal";
            ClientSize = new Size(1100, 700);
            MinimumSize = new Size(950, 600);
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            BackColor = ContentBg;
            Font = new Font("Segoe UI", 9f);

            // ── Sidebar ───────────────────────────────────────────────
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

            // ── Topbar ────────────────────────────────────────────────
            pnlTopbar = new Panel { Dock = DockStyle.Top, Height = 68, BackColor = Color.White };
            lblPageTitle = new Label { Font = new Font("Segoe UI", 15f, FontStyle.Bold), ForeColor = Color.FromArgb(28, 28, 32), AutoSize = true, Location = new Point(28, 10) };
            lblPageBreadcrumb = new Label { Font = new Font("Segoe UI", 8.5f), ForeColor = Color.FromArgb(160, 160, 170), AutoSize = true, Location = new Point(30, 42) };
            var lblUserInfo = new Label { Text = $"{AppSession.CurrentUsername.ToUpper()}  ·  PATIENT", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Orange, AutoSize = false, Size = new Size(300, 20), TextAlign = ContentAlignment.MiddleRight, Anchor = AnchorStyles.Top | AnchorStyles.Right, Location = new Point(pnlTopbar.Width - 324, 24) };
            
            pnlTopbar.Controls.AddRange(new Control[] { lblPageTitle, lblPageBreadcrumb, lblUserInfo });

            var pnlDivider = new Panel { Dock = DockStyle.Top, Height = 3, BackColor = Orange };

            // ── Content ───────────────────────────────────────────────
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

            // Default state
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

        private void SetActiveNav(Button btn)
        {
            if (_activeNavBtn != null)
            {
                _activeNavBtn.ForeColor = Color.FromArgb(190, 190, 200);
                _activeNavBtn.BackColor = Color.Transparent;
            }
            btn.ForeColor = Orange;
            btn.BackColor = ActiveBg;
            _activeNavBtn = btn;
        }

        private void ShowPanel(Panel pnl, string title, string breadcrumb)
        {
            pnlProfile.Visible = false;
            pnlRecords.Visible = false;
            pnl.Visible = true;
            lblPageTitle.Text = title;
            lblPageBreadcrumb.Text = breadcrumb;
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                new AuthService().Logout();
                Application.Restart();
            }
        }

        // ── Panel Profile ─────────────────────────────────────────────
        private void BuildProfilePanel()
        {
            pnlProfile = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            pnlProfile.Padding = new Padding(24);

            int labelX = 24, valueX = 180, y = 24, spacing = 32;

            Label Lbl(string text) => new Label { Text = text, Location = new Point(labelX, y), AutoSize = true, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), ForeColor = Color.FromArgb(80, 80, 90) };
            Label Val(string name) => new Label { Name = name, Text = "—", Location = new Point(valueX, y), AutoSize = true, Font = new Font("Segoe UI", 9.5f), ForeColor = Color.FromArgb(28, 28, 32) };

            pnlProfile.Controls.Add(Lbl("Mã bệnh nhân:")); lblPatientId = Val("lblPatientId"); pnlProfile.Controls.Add(lblPatientId); y += spacing;
            pnlProfile.Controls.Add(Lbl("Họ và tên:")); lblFullName = Val("lblFullName"); pnlProfile.Controls.Add(lblFullName); y += spacing;
            pnlProfile.Controls.Add(Lbl("Giới tính:")); lblGender = Val("lblGender"); pnlProfile.Controls.Add(lblGender); y += spacing;
            pnlProfile.Controls.Add(Lbl("Ngày sinh:")); lblBirthdate = Val("lblBirthdate"); pnlProfile.Controls.Add(lblBirthdate); y += spacing;
            pnlProfile.Controls.Add(Lbl("CCCD:")); lblIdCard = Val("lblIdCard"); pnlProfile.Controls.Add(lblIdCard); y += spacing;

            y += 12;
            pnlProfile.Controls.Add(new Label { Text = "Tiền sử bệnh lý:", Location = new Point(labelX, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) }); y += 22;
            txtMedicalHistory = new TextBox { Location = new Point(labelX, y), Height = 48, Font = new Font("Segoe UI", 9.5f), Multiline = true, ScrollBars = ScrollBars.Vertical, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, Width = pnlContent.Width - 48 };
            pnlProfile.Controls.Add(txtMedicalHistory); y += 60;

            pnlProfile.Controls.Add(new Label { Text = "Bệnh lý gia đình:", Location = new Point(labelX, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) }); y += 22;
            txtFamilyMedicalHistory = new TextBox { Location = new Point(labelX, y), Height = 48, Font = new Font("Segoe UI", 9.5f), Multiline = true, ScrollBars = ScrollBars.Vertical, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, Width = pnlContent.Width - 48 };
            pnlProfile.Controls.Add(txtFamilyMedicalHistory); y += 60;

            pnlProfile.Controls.Add(new Label { Text = "Dị ứng thuốc:", Location = new Point(labelX, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) }); y += 22;
            txtDrugAllergies = new TextBox { Location = new Point(labelX, y), Height = 36, Font = new Font("Segoe UI", 9.5f), Multiline = true, ScrollBars = ScrollBars.Vertical, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, Width = pnlContent.Width - 48 };
            pnlProfile.Controls.Add(txtDrugAllergies); y += 52;

            y += 8;
            var sep = new Label { Text = "ĐỊA CHỈ LIÊN LẠC", Location = new Point(labelX, y), AutoSize = true, Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Orange };
            pnlProfile.Controls.Add(sep); y += 28;

            TableLayoutPanel tlpAddress = new TableLayoutPanel
            {
                Location = new Point(labelX, y),
                Height = 140, // Increased height to prevent cutoff
                ColumnCount = 2,
                RowCount = 2,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Width = pnlContent.Width - 48
            };
            tlpAddress.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpAddress.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpAddress.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpAddress.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            void AddCell(string label, ref TextBox box, int col, int row)
            {
                var pnl = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 20, 0) };
                pnl.Controls.Add(new Label { Text = label, Location = new Point(0, 0), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) });
                box = new TextBox { Location = new Point(0, 22), Width = pnl.Width, Font = new Font("Segoe UI", 9.5f), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
                pnl.Controls.Add(box);
                tlpAddress.Controls.Add(pnl, col, row);
            }

            AddCell("Số nhà:", ref txtHouseNo, 0, 0);
            AddCell("Tên đường:", ref txtStreet, 1, 0);
            AddCell("Quận/Huyện:", ref txtDistrict, 0, 1);
            AddCell("Tỉnh/Thành phố:", ref txtCityProvince, 1, 1);
            pnlProfile.Controls.Add(tlpAddress);
            y += 150;

            btnSaveContact = new Button
            {
                Text = "LƯU THÔNG TIN",
                Location = new Point(labelX, y),
                Size = new Size(160, 36),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSaveContact.FlatAppearance.BorderSize = 0;
            btnSaveContact.Click += BtnSaveContact_Click;
            pnlProfile.Controls.Add(btnSaveContact);
        }

        // ── Panel Records ─────────────────────────────────────────────
        private void BuildRecordsPanel()
        {
            pnlRecords = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };

            var splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 250,
                SplitterWidth = 6,
                BackColor = ContentBg
            };

            // Top: Medical Records
            var pnlTop = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12) };
            var lblTopTitle = new Label { Text = "DANH SÁCH HỒ SƠ KHÁM", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Orange, Dock = DockStyle.Top, Height = 24 };
            dgvRecords = CreateGridView();
            dgvRecords.SelectionChanged += DgvRecords_SelectionChanged;
            pnlTop.Controls.Add(dgvRecords);
            pnlTop.Controls.Add(lblTopTitle);

            // Bottom: Details (Prescriptions & Services)
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

        // ── Logic ─────────────────────────────────────────────────────
        private void LoadProfile()
        {
            try
            {
                DataTable dt = _service.GetProfile();
                if (dt.Rows.Count == 0) return;
                DataRow row = dt.Rows[0];

                lblPatientId.Text = row["PATIENT_ID"]?.ToString();
                lblFullName.Text = row["FULL_NAME"]?.ToString();
                lblGender.Text = row["GENDER"]?.ToString();
                lblBirthdate.Text = row["BIRTHDATE"] != DBNull.Value ? Convert.ToDateTime(row["BIRTHDATE"]).ToString("dd/MM/yyyy") : "—";
                lblIdCard.Text = row["ID_CARD"]?.ToString();
                
                txtMedicalHistory.Text = row["MEDICAL_HISTORY"]?.ToString() ?? "";
                txtFamilyMedicalHistory.Text = row["FAMILY_MEDICAL_HISTORY"]?.ToString() ?? "";
                txtDrugAllergies.Text = row["DRUG_ALLERGIES"]?.ToString() ?? "";
                txtHouseNo.Text = row["HOUSE_NO"]?.ToString();
                txtStreet.Text = row["STREET"]?.ToString();
                txtDistrict.Text = row["DISTRICT"]?.ToString();
                txtCityProvince.Text = row["CITY_PROVINCE"]?.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải hồ sơ: " + ex.Message);
            }
        }

        private void LoadMedicalRecords()
        {
            try
            {
                DataTable dt = _service.GetMedicalRecords();
                if (dt.Columns["RECORD_ID"]      != null) dt.Columns["RECORD_ID"]!.ColumnName      = "Mã hồ sơ";
                if (dt.Columns["RECORD_DATE"]    != null) dt.Columns["RECORD_DATE"]!.ColumnName    = "Ngày khám";
                if (dt.Columns["DIAGNOSIS"]      != null) dt.Columns["DIAGNOSIS"]!.ColumnName      = "Chẩn đoán";
                if (dt.Columns["TREATMENT_PLAN"] != null) dt.Columns["TREATMENT_PLAN"]!.ColumnName = "Phác đồ điều trị";
                if (dt.Columns["CONCLUSION"]     != null) dt.Columns["CONCLUSION"]!.ColumnName     = "Kết luận";
                if (dt.Columns["DOCTOR_NAME"]    != null) dt.Columns["DOCTOR_NAME"]!.ColumnName    = "Bác sĩ";
                if (dt.Columns["DEPT_NAME"]      != null) dt.Columns["DEPT_NAME"]!.ColumnName      = "Khoa";

                dgvRecords.DataSource = dt;
                if (dgvRecords.Columns.Contains("Ngày khám"))
                    dgvRecords.Columns["Ngày khám"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch sử khám: " + ex.Message);
            }
        }

        private void DgvRecords_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvRecords.SelectedRows.Count == 0) return;
            string recordId = dgvRecords.SelectedRows[0].Cells["Mã hồ sơ"].Value?.ToString() ?? "";
            LoadDetail(recordId);
        }

        private void LoadDetail(string recordId)
        {
            try
            {
                lblDetailTitle.Text = $"CHI TIẾT HỒ SƠ: {recordId}";

                DataTable dtPresc = _service.GetPrescriptions(recordId);
                if (dtPresc.Columns["RECORD_ID"]          != null) dtPresc.Columns["RECORD_ID"]!.ColumnName          = "Mã hồ sơ";
                if (dtPresc.Columns["PRESCRIPTION_DATE"]  != null) dtPresc.Columns["PRESCRIPTION_DATE"]!.ColumnName  = "Ngày kê";
                if (dtPresc.Columns["MEDICINE_NAME"]      != null) dtPresc.Columns["MEDICINE_NAME"]!.ColumnName      = "Tên thuốc";
                if (dtPresc.Columns["DOSAGE"]             != null) dtPresc.Columns["DOSAGE"]!.ColumnName             = "Liều dùng";
                dgvPrescriptions.DataSource = dtPresc;
                if (dgvPrescriptions.Columns.Contains("Ngày kê"))
                    dgvPrescriptions.Columns["Ngày kê"].DefaultCellStyle.Format = "dd/MM/yyyy";

                DataTable dtSvc = _service.GetServices(recordId);
                if (dtSvc.Columns["RECORD_ID"]       != null) dtSvc.Columns["RECORD_ID"]!.ColumnName       = "Mã hồ sơ";
                if (dtSvc.Columns["SERVICE_TYPE"]    != null) dtSvc.Columns["SERVICE_TYPE"]!.ColumnName    = "Loại dịch vụ";
                if (dtSvc.Columns["SERVICE_DATE"]    != null) dtSvc.Columns["SERVICE_DATE"]!.ColumnName    = "Ngày thực hiện";
                if (dtSvc.Columns["SERVICE_RESULT"]  != null) dtSvc.Columns["SERVICE_RESULT"]!.ColumnName  = "Kết quả";
                if (dtSvc.Columns["TECHNICIAN_NAME"] != null) dtSvc.Columns["TECHNICIAN_NAME"]!.ColumnName = "Kỹ thuật viên";
                dgvServices.DataSource = dtSvc;
                if (dgvServices.Columns.Contains("Ngày thực hiện"))
                    dgvServices.Columns["Ngày thực hiện"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết hồ sơ: " + ex.Message);
            }
        }

        private void BtnSaveContact_Click(object? sender, EventArgs e)
        {
            try
            {
                _service.UpdateContact(
                    txtHouseNo.Text.Trim(),
                    txtStreet.Text.Trim(),
                    txtDistrict.Text.Trim(),
                    txtCityProvince.Text.Trim(),
                    txtMedicalHistory.Text.Trim(),
                    txtFamilyMedicalHistory.Text.Trim(),
                    txtDrugAllergies.Text.Trim()
                );
                MessageBox.Show("Cập nhật thông tin thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
