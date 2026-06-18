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
        private Button _activeNavBtn = null!;
        private DataTable _allRecordsTable = null!;

        public frmPatient()
        {
            InitializeComponent();
            LoadProfile();
            LoadMedicalRecords();
            
            SetActiveNav(btnNavProfile);
            ShowPanel(pnlProfile, "Hồ sơ cá nhân", "Patient / Profile");
        }

        private void SetActiveNav(Button btn)
        {
            if (_activeNavBtn != null)
            {
                _activeNavBtn.ForeColor = Color.FromArgb(190, 190, 200);
                _activeNavBtn.BackColor = Color.Transparent;
            }
            btn.ForeColor = Color.FromArgb(255, 140, 40); // Orange
            btn.BackColor = Color.FromArgb(45, 255, 140, 40); // ActiveBg
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

        private void btnNavProfile_Click(object? sender, EventArgs e)
        {
            SetActiveNav(btnNavProfile);
            ShowPanel(pnlProfile, "Hồ sơ cá nhân", "Patient / Profile");
        }

        private void btnNavRecords_Click(object? sender, EventArgs e)
        {
            SetActiveNav(btnNavRecords);
            ShowPanel(pnlRecords, "Lịch sử khám", "Patient / Medical Records");
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                new AuthService().Logout();
                Application.Restart();
            }
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

        private void FilterRecords_Event(object? sender, EventArgs e)
        {
            FilterRecords();
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

        private void BtnClearFilter_Click(object? sender, EventArgs e)
        {
            txtSearchRecords.Clear();
            dtpFrom.Value = DateTime.Now.AddMonths(-6);
            dtpTo.Value = DateTime.Now;
            if (cboDept.Items.Count > 0) cboDept.SelectedIndex = 0;
            if (cboDoctor.Items.Count > 0) cboDoctor.SelectedIndex = 0;
            FilterRecords();
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
