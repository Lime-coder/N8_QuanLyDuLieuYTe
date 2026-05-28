using System.Data;
using QuanLyYTe.Common;
using QuanLyYTe.Helpers;
using QuanLyYTe.Services;

namespace QuanLyYTe.Forms.Patient
{
    public partial class frmPatient : Form
    {
        private readonly PatientService _service = new PatientService();

        private TabControl tabMain = null!;

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
        private Button btnViewDetail = null!;

        private Label lblSelectedRecord = null!;
        private DataGridView dgvPrescriptions = null!;
        private DataGridView dgvServices = null!;

        public frmPatient()
        {
            InitializeComponent();
            LoadProfile();
            LoadMedicalRecords();
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            Text = $"Hồ sơ bệnh nhân — {AppSession.CurrentUsername}";
            ClientSize = new Size(900, 620);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            tabMain = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10)
            };

            tabMain.TabPages.Add(BuildProfileTab());
            tabMain.TabPages.Add(BuildRecordsTab());
            tabMain.TabPages.Add(BuildDetailTab());

            Controls.Add(tabMain);
            ResumeLayout(false);
        }

        private TabPage BuildProfileTab()
        {
            var tab = new TabPage("Hồ sơ cá nhân") { Padding = new Padding(12) };

            int labelX = 20, valueX = 200, y = 20, spacing = 32;

            Label Lbl(string text) =>
                new Label { Text = text, Location = new Point(labelX, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };

            Label Val(string name)
            {
                var l = new Label { Name = name, Text = "—", Location = new Point(valueX, y), AutoSize = true, Font = new Font("Segoe UI", 9) };
                return l;
            }

            tab.Controls.Add(Lbl("Mã bệnh nhân:")); lblPatientId = Val("lblPatientId"); tab.Controls.Add(lblPatientId); y += spacing;
            tab.Controls.Add(Lbl("Họ và tên:")); lblFullName = Val("lblFullName"); tab.Controls.Add(lblFullName); y += spacing;
            tab.Controls.Add(Lbl("Giới tính:")); lblGender = Val("lblGender"); tab.Controls.Add(lblGender); y += spacing;
            tab.Controls.Add(Lbl("Ngày sinh:")); lblBirthdate = Val("lblBirthdate"); tab.Controls.Add(lblBirthdate); y += spacing;
            tab.Controls.Add(Lbl("CCCD:")); lblIdCard = Val("lblIdCard"); tab.Controls.Add(lblIdCard); y += spacing;

            y += 8;
            tab.Controls.Add(new Label { Text = "Tiền sử bệnh lý:", Location = new Point(labelX, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) }); y += 20;
            txtMedicalHistory = new TextBox { Location = new Point(labelX, y), Size = new Size(840, 48), Font = new Font("Segoe UI", 9), Multiline = true, ScrollBars = ScrollBars.Vertical };
            tab.Controls.Add(txtMedicalHistory); y += 52;

            tab.Controls.Add(new Label { Text = "Bệnh lý gia đình:", Location = new Point(labelX, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) }); y += 20;
            txtFamilyMedicalHistory = new TextBox { Location = new Point(labelX, y), Size = new Size(840, 48), Font = new Font("Segoe UI", 9), Multiline = true, ScrollBars = ScrollBars.Vertical };
            tab.Controls.Add(txtFamilyMedicalHistory); y += 52;

            tab.Controls.Add(new Label { Text = "Dị ứng thuốc:", Location = new Point(labelX, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) }); y += 20;
            txtDrugAllergies = new TextBox { Location = new Point(labelX, y), Size = new Size(840, 32), Font = new Font("Segoe UI", 9), Multiline = true, ScrollBars = ScrollBars.Vertical };
            tab.Controls.Add(txtDrugAllergies); y += 44;

            y += 8;
            var sep = new Label { Text = "── Địa chỉ liên lạc (có thể chỉnh sửa) ──────────────────────────────────────", Location = new Point(labelX, y), AutoSize = true, ForeColor = Color.Gray };
            tab.Controls.Add(sep); y += 28;

            int inputW = 260;
            void AddInput(string label, ref TextBox box, int col)
            {
                int px = labelX + col * 430;
                tab.Controls.Add(new Label { Text = label, Location = new Point(px, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) });
                box = new TextBox { Location = new Point(px, y + 20), Width = inputW, Font = new Font("Segoe UI", 9) };
                tab.Controls.Add(box);
            }

            AddInput("Số nhà:", ref txtHouseNo, 0);
            AddInput("Tên đường:", ref txtStreet, 1);
            y += 56;
            AddInput("Quận/Huyện:", ref txtDistrict, 0);
            AddInput("Tỉnh/Thành phố:", ref txtCityProvince, 1);
            y += 56;

            btnSaveContact = new Button
            {
                Text = "Lưu thông tin",
                Location = new Point(labelX, y),
                Size = new Size(140, 32),
                Font = new Font("Segoe UI", 9)
            };
            btnSaveContact.Click += BtnSaveContact_Click;
            tab.Controls.Add(btnSaveContact);

            return tab;
        }

        private TabPage BuildRecordsTab()
        {
            var tab = new TabPage("Lịch sử khám") { Padding = new Padding(12) };

            dgvRecords = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9),
                ColumnHeadersDefaultCellStyle = { Font = new Font("Segoe UI", 9, FontStyle.Bold) },
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvRecords.SelectionChanged += DgvRecords_SelectionChanged;

            var panel = new Panel { Dock = DockStyle.Bottom, Height = 46, Padding = new Padding(0, 8, 0, 0) };
            btnViewDetail = new Button
            {
                Text = "Xem đơn thuốc & dịch vụ",
                Size = new Size(200, 30),
                Location = new Point(0, 8),
                Font = new Font("Segoe UI", 9),
                Enabled = false
            };
            btnViewDetail.Click += BtnViewDetail_Click;
            panel.Controls.Add(btnViewDetail);

            tab.Controls.Add(dgvRecords);
            tab.Controls.Add(panel);
            return tab;
        }

        private TabPage BuildDetailTab()
        {
            var tab = new TabPage("Đơn thuốc & Dịch vụ") { Padding = new Padding(12) };

            lblSelectedRecord = new Label
            {
                Text = "Chưa chọn hồ sơ nào. Vào tab Lịch sử khám và nhấn \"Xem đơn thuốc & dịch vụ\".",
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.Gray
            };

            var lblPresc = new Label { Text = "Đơn thuốc", Dock = DockStyle.Top, Height = 24, Font = new Font("Segoe UI", 9, FontStyle.Bold) };

            dgvPrescriptions = new DataGridView
            {
                Height = 180,
                Dock = DockStyle.Top,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9),
                ColumnHeadersDefaultCellStyle = { Font = new Font("Segoe UI", 9, FontStyle.Bold) },
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            var lblSvc = new Label { Text = "Dịch vụ y tế", Dock = DockStyle.Top, Height = 24, Font = new Font("Segoe UI", 9, FontStyle.Bold) };

            dgvServices = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9),
                ColumnHeadersDefaultCellStyle = { Font = new Font("Segoe UI", 9, FontStyle.Bold) },
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            tab.Controls.Add(dgvServices);
            tab.Controls.Add(lblSvc);
            tab.Controls.Add(dgvPrescriptions);
            tab.Controls.Add(lblPresc);
            tab.Controls.Add(lblSelectedRecord);
            return tab;
        }

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
                lblBirthdate.Text = row["BIRTHDATE"] != DBNull.Value
                    ? Convert.ToDateTime(row["BIRTHDATE"]).ToString("dd/MM/yyyy")
                    : "—";
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
            btnViewDetail.Enabled = dgvRecords.SelectedRows.Count > 0;
        }

        private void BtnViewDetail_Click(object? sender, EventArgs e)
        {
            if (dgvRecords.SelectedRows.Count == 0) return;
            string recordId = dgvRecords.SelectedRows[0].Cells["Mã hồ sơ"].Value?.ToString() ?? "";
            LoadDetail(recordId);
            tabMain.SelectedTab = tabMain.TabPages[2];
        }

        private void LoadDetail(string recordId)
        {
            try
            {
                lblSelectedRecord.Text = $"Hồ sơ: {recordId}";

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
