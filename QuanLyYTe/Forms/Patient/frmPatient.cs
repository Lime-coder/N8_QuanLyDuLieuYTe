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

        

        public frmPatient()
        {
            InitializeComponent();
            InitializeCustomUI();
            LoadProfile();
            LoadMedicalRecords();
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
                Height = 140,
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

                _allRecordsTable = dt;
                dgvRecords.DataSource = _allRecordsTable.DefaultView;
                if (dgvRecords.Columns.Contains("Ngày khám"))
                    dgvRecords.Columns["Ngày khám"].DefaultCellStyle.Format = "dd/MM/yyyy";

                var depts = new System.Collections.Generic.List<string> { "Tất cả" };
                var doctors = new System.Collections.Generic.List<string> { "Tất cả" };

                foreach (DataRow row in dt.Rows)
                {
                    string dept = row["Khoa"]?.ToString() ?? "";
                    string doctor = row["Bác sĩ"]?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(dept) && !depts.Contains(dept)) depts.Add(dept);
                    if (!string.IsNullOrEmpty(doctor) && !doctors.Contains(doctor)) doctors.Add(doctor);
                }

                cboDept.DataSource = depts;
                cboDoctor.DataSource = doctors;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch sử khám: " + ex.Message);
            }
        }

        private void FilterRecords()
        {
            if (_allRecordsTable == null) return;

            string keyword = txtSearchRecords.Text.Trim().ToLower();
            string selectedDept = cboDept.SelectedItem?.ToString() ?? "Tất cả";
            string selectedDoctor = cboDoctor.SelectedItem?.ToString() ?? "Tất cả";

            DateTime from = dtpFrom.Value.Date;
            DateTime to = dtpTo.Value.Date.AddDays(1).AddSeconds(-1);

            _allRecordsTable.DefaultView.RowFilter = "";

            var parts = new System.Collections.Generic.List<string>();

            parts.Add($"[Ngày khám] >= #{from:MM/dd/yyyy}# AND [Ngày khám] <= #{to:MM/dd/yyyy HH:mm:ss}#");

            if (selectedDept != "Tất cả")
            {
                string escDept = selectedDept.Replace("'", "''");
                parts.Add($"[Khoa] = '{escDept}'");
            }

            if (selectedDoctor != "Tất cả")
            {
                string escDoc = selectedDoctor.Replace("'", "''");
                parts.Add($"[Bác sĩ] = '{escDoc}'");
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                string esc = keyword.Replace("'", "''");
                parts.Add($"(CONVERT([Chẩn đoán], 'System.String') LIKE '%{esc}%' OR " +
                           $"CONVERT([Kết luận], 'System.String') LIKE '%{esc}%')");
            }

            _allRecordsTable.DefaultView.RowFilter = string.Join(" AND ", parts);
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
