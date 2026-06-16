using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyYTe.Services;

namespace QuanLyYTe.Forms.Coordinator
{
    public partial class frmCoordinator : Form
    {
    	private readonly CoordinatorService _coordinatorService = new CoordinatorService();
    
    	private string _username;
    
    	private DataTable dtPatients;
    
    	private DataTable dtMedicalRecords;
    
    	private DataTable dtServiceRecords;
    
    	private IContainer components = null;
    
    	private TabControl tabCoordinator;
    
    	private TabPage tabPatients;
    
    	private TabPage tabCreateMR;
    
    	private TabPage tabAssignDoctor;
    
    	private TabPage tabAssignTech;
    
    	private TabPage tabProfile;
    
    	private Label lblTitle;
    
    	private SplitContainer splitContainer1;
    
    	private DataGridView dgvPatients;
    
    	private Panel panel1;
    
    	private Button btnRefreshPatient;
    
    	private Button btnSearchPatient;
    
    	private TextBox txtSearchPatient;
    
    	private Label lblSearch;
    
    	private GroupBox grpPatientDetails;
    
    	private TextBox txtPatientId;
    
    	private Label lblPatientId;
    
    	private TextBox txtPatientGender;
    
    	private Label lblPatientGender;
    
    	private TextBox txtPatientFullName;
    
    	private Label lblPatientFullName;
    
    	private TextBox txtPatientBirthDate;
    
    	private Label lblPatientBirthDate;
    
    	private TextBox txtPatientIdCard;
    
    	private Label lblPatientIdCard;
    
    	private TextBox txtPatientUsernameDb;
    
    	private Label lblPatientUsernameDb;
    
    	private TextBox txtPatientDrugAllergies;
    
    	private Label lblPatientDrugAllergies;
    
    	private TextBox txtPatientFamilyMedicalHistory;
    
    	private Label lblPatientFamilyMedicalHistory;
    
    	private TextBox txtPatientMedicalHistory;
    
    	private Label lblPatientMedicalHistory;
    
    	private TextBox txtPatientCityProvince;
    
    	private Label lblPatientCityProvince;
    
    	private TextBox txtPatientDistrict;
    
    	private Label lblPatientDistrict;
    
    	private TextBox txtPatientStreet;
    
    	private Label lblPatientStreet;
    
    	private TextBox txtPatientHouseNo;
    
    	private Label lblPatientHouseNo;
    
    	private Button btnPatientClear;
    
    	private Button btnPatientUpdate;
    
    	private Button btnPatientAdd;
    
    	private GroupBox grpCreateMR;
    
    	private Label lblSectionInfo;
    
    	private TextBox txtMRRecordId;
    
    	private Label lblMRRecordId;
    
    	private DateTimePicker dtpMRRecordDate;
    
    	private Label lblMRRecordDate;
    
    	private ComboBox cmbMRPatientId;
    
    	private Label lblMRPatientId;
    
    	private ComboBox cmbMRDoctorId;
    
    	private Label lblMRDoctorId;
    
    	private ComboBox cmbMRDeptId;
    
    	private Label lblMRDeptId;
    
    	private Label lblSectionDoctor;
    
    	private TextBox txtMRServiceResult;
    
    	private Label lblMRServiceResult;
    
    	private TextBox txtMRConclusion;
    
    	private Label lblMRConclusion;
    
    	private TextBox txtMRTreatmentPlan;
    
    	private Label lblMRTreatmentPlan;
    
    	private TextBox txtMRDiagnosis;
    
    	private Label lblMRDiagnosis;
    
    	private Label lblSectionSpecialty;
    
    	private Button btnMRCreate;
    
    	private Label label5;
    
    	private SplitContainer splitContainer2;
    
    	private DataGridView dgvMedicalRecords;
    
    	private GroupBox groupBox1;
    
    	private TextBox txtAssignMRId;
    
    	private Label label6;
    
    	private TextBox txtAssignMRDate;
    
    	private Label label8;
    
    	private TextBox txtAssignMRPatient;
    
    	private Label label7;
    
    	private Button btnUpdateAssignDoctor;
    
    	private ComboBox cmbAssignDoctor;
    
    	private Label label9;
    
    	private ComboBox cmbAssignDept;
    
    	private Label label10;
    
    	private SplitContainer splitContainer3;
    
    	private DataGridView dgvServiceRecords;
    
    	private Label lblTechNotice;
    
    	private GroupBox groupBox2;
    
    	private Button btnUpdateAssignTech;
    
    	private ComboBox cmbAssignTech;
    
    	private Label label15;
    
    	private TextBox txtAssignSRDate;
    
    	private Label label13;
    
    	private TextBox txtAssignSRType;
    
    	private Label label12;
    
    	private TextBox txtAssignSRId;
    
    	private Label label11;
    
    	private GroupBox groupBox3;
    
    	private TextBox txtSelfStaffId;
    
    	private Label label16;
    
    	private TextBox txtSelfRole;
    
    	private Label label19;
    
    	private TextBox txtSelfFullName;
    
    	private Label label17;
    
    	private Button btnUpdateProfile;
    
    	private TextBox txtSelfHometown;
    
    	private Label label20;
    
    	private TextBox txtSelfPhone;
    
    	private Label label21;
    
    	private TextBox txtSelfSpecialty;
    
    	private Label label18;
    
    	public frmCoordinator()
    	{
    		InitializeComponent();
    	}
    
    	public frmCoordinator(string username)
    		: this()
    	{
    		_username = username;
    	}
    
    	private void frmCoordinator_Load(object sender, EventArgs e)
    	{
    		this.DoubleBuffered = true;
    		EnableDoubleBuffered(dgvPatients);
    		EnableDoubleBuffered(dgvMedicalRecords);
    		EnableDoubleBuffered(dgvServiceRecords);
    		
    		txtPatientUsernameDb.ReadOnly = true;
    		txtMRRecordId.ReadOnly = true;
    		LoadData();

            // Add OLS Notification Tab
            TabPage tabOls = new TabPage("Thông báo OLS");
            QuanLyYTe.Forms.Common.frmNotifications frm = new QuanLyYTe.Forms.Common.frmNotifications();
            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            tabOls.Controls.Add(frm);
            tabCoordinator.TabPages.Add(tabOls);
            frm.Show();
    	}

    	private void EnableDoubleBuffered(DataGridView dgv)
    	{
    		typeof(DataGridView).InvokeMember("DoubleBuffered", 
    			System.Reflection.BindingFlags.NonPublic | 
    			System.Reflection.BindingFlags.Instance | 
    			System.Reflection.BindingFlags.SetProperty, 
    			null, dgv, new object[] { true });
    	}
    
    	private void LoadData()
    	{
    		try
    		{
    			dtPatients = _coordinatorService.GetAllPatients();
    			dtPatients.Columns.Add("display_name", typeof(string), "patient_id + ' - ' + full_name");
    			dgvPatients.DataSource = dtPatients;
    			if (dgvPatients.Columns.Contains("patient_id")) dgvPatients.Columns["patient_id"].HeaderText = "Mã bệnh nhân";
    			if (dgvPatients.Columns.Contains("full_name")) dgvPatients.Columns["full_name"].HeaderText = "Tên bệnh nhân";
    			if (dgvPatients.Columns.Contains("gender")) dgvPatients.Columns["gender"].HeaderText = "Giới tính";
    			if (dgvPatients.Columns.Contains("birthdate")) dgvPatients.Columns["birthdate"].HeaderText = "Ngày sinh";
    			if (dgvPatients.Columns.Contains("medical_history")) dgvPatients.Columns["medical_history"].HeaderText = "Tiền sử bệnh";
    			if (dgvPatients.Columns.Contains("family_medical_history")) dgvPatients.Columns["family_medical_history"].HeaderText = "Tiền sử bệnh gia đình";
    			if (dgvPatients.Columns.Contains("drug_allergies")) dgvPatients.Columns["drug_allergies"].HeaderText = "Dị ứng thuốc";
    			if (dgvPatients.Columns.Contains("display_name")) dgvPatients.Columns["display_name"].Visible = false;
    			if (dgvPatients.Columns.Contains("id_card")) dgvPatients.Columns["id_card"].Visible = false;
    			if (dgvPatients.Columns.Contains("house_no")) dgvPatients.Columns["house_no"].Visible = false;
    			if (dgvPatients.Columns.Contains("street")) dgvPatients.Columns["street"].Visible = false;
    			if (dgvPatients.Columns.Contains("district")) dgvPatients.Columns["district"].Visible = false;
    			if (dgvPatients.Columns.Contains("city_province")) dgvPatients.Columns["city_province"].Visible = false;
    			if (dgvPatients.Columns.Contains("username_db")) dgvPatients.Columns["username_db"].Visible = false;
    			cmbMRPatientId.DataSource = dtPatients.Copy();
    			cmbMRPatientId.DisplayMember = "display_name";
    			cmbMRPatientId.ValueMember = "patient_id";
    			DataTable departments = _coordinatorService.GetDepartments();
    			cmbMRDeptId.DataSource = departments;
    			cmbMRDeptId.DisplayMember = "dept_name";
    			cmbMRDeptId.ValueMember = "dept_id";
    			cmbAssignDept.DataSource = departments.Copy();
    			cmbAssignDept.DisplayMember = "dept_name";
    			cmbAssignDept.ValueMember = "dept_id";
    			dtMedicalRecords = _coordinatorService.GetAllMedicalRecords();
    			dgvMedicalRecords.DataSource = dtMedicalRecords;
    			if (dgvMedicalRecords.Columns.Contains("RECORD_ID")) dgvMedicalRecords.Columns["RECORD_ID"].HeaderText = "Mã HSBA";
    			if (dgvMedicalRecords.Columns.Contains("PATIENT_ID")) dgvMedicalRecords.Columns["PATIENT_ID"].HeaderText = "Mã Bệnh nhân";
    			if (dgvMedicalRecords.Columns.Contains("RECORD_DATE")) dgvMedicalRecords.Columns["RECORD_DATE"].HeaderText = "Ngày lập";
    			if (dgvMedicalRecords.Columns.Contains("DIAGNOSIS")) dgvMedicalRecords.Columns["DIAGNOSIS"].HeaderText = "Chẩn đoán";
    			if (dgvMedicalRecords.Columns.Contains("TREATMENT_PLAN")) dgvMedicalRecords.Columns["TREATMENT_PLAN"].HeaderText = "Hướng điều trị";
    			if (dgvMedicalRecords.Columns.Contains("DOCTOR_ID")) dgvMedicalRecords.Columns["DOCTOR_ID"].HeaderText = "Mã Bác sĩ";
    			if (dgvMedicalRecords.Columns.Contains("DEPT_ID")) dgvMedicalRecords.Columns["DEPT_ID"].HeaderText = "Mã Khoa";
    			if (dgvMedicalRecords.Columns.Contains("CONCLUSION")) dgvMedicalRecords.Columns["CONCLUSION"].HeaderText = "Kết luận";
    			txtMRRecordId.Text = GenerateNewMRId();
    			dtServiceRecords = _coordinatorService.GetAllServiceAssignments();
    			if (dtServiceRecords != null)
    			{
    				dgvServiceRecords.DataSource = dtServiceRecords;
    				if (dgvServiceRecords.Columns.Contains("KETQUA"))
    				{
    					dgvServiceRecords.Columns["KETQUA"].Visible = false;
    				}
    				if (dgvServiceRecords.Columns.Contains("MAHSBA"))
    				{
    					dgvServiceRecords.Columns["MAHSBA"].HeaderText = "Ma HSBA";
    				}
    				if (dgvServiceRecords.Columns.Contains("LOAIDV"))
    				{
    					dgvServiceRecords.Columns["LOAIDV"].HeaderText = "Loại dịch vụ";
    				}
    				if (dgvServiceRecords.Columns.Contains("NGAYDV"))
    				{
    					dgvServiceRecords.Columns["NGAYDV"].HeaderText = "Ngày dịch vụ";
    				}
    				if (dgvServiceRecords.Columns.Contains("MAKTV"))
    				{
    					dgvServiceRecords.Columns["MAKTV"].HeaderText = "Kỹ thuật viên";
    				}
    			}
    			DataTable techniciansForAssignment = _coordinatorService.GetTechniciansForAssignment();
    			cmbAssignTech.DataSource = techniciansForAssignment;
    			cmbAssignTech.DisplayMember = "display_name";
    			cmbAssignTech.ValueMember = "staff_id";
    			LoadProfile();
    		}
    		catch (Exception ex)
    		{
    			ShowError(ex);
    		}
    	}
    
    	private void ShowError(Exception ex)
    	{
    		MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    	}
    
    	private void btnRefreshPatient_Click(object sender, EventArgs e)
    	{
    		txtSearchPatient.Clear();
    		LoadData();
    	}
    
    	private void btnSearchPatient_Click(object sender, EventArgs e)
    	{
    		if (dtPatients != null)
    		{
    			string rowFilter = "";
    			string value = txtSearchPatient.Text.Trim();
    			if (!string.IsNullOrEmpty(value))
    			{
    				rowFilter = $"patient_id LIKE '%{value}%' OR id_card LIKE '%{value}%' OR full_name LIKE '%{value}%'";
    			}
    			dtPatients.DefaultView.RowFilter = rowFilter;
    		}
    	}
    
    	private void dgvPatients_CellClick(object sender, DataGridViewCellEventArgs e)
    	{
    		if (e.RowIndex >= 0)
    		{
    			DataGridViewRow dataGridViewRow = dgvPatients.Rows[e.RowIndex];
    			txtPatientId.Text = dataGridViewRow.Cells["patient_id"].Value?.ToString();
    			txtPatientFullName.Text = dataGridViewRow.Cells["full_name"].Value?.ToString();
    			txtPatientGender.Text = dataGridViewRow.Cells["gender"].Value?.ToString();
    			txtPatientBirthDate.Text = Convert.ToDateTime(dataGridViewRow.Cells["birthdate"].Value).ToString("dd/MM/yyyy");
    			txtPatientIdCard.Text = dataGridViewRow.Cells["id_card"].Value?.ToString();
    			txtPatientHouseNo.Text = dataGridViewRow.Cells["house_no"].Value?.ToString();
    			txtPatientStreet.Text = dataGridViewRow.Cells["street"].Value?.ToString();
    			txtPatientDistrict.Text = dataGridViewRow.Cells["district"].Value?.ToString();
    			txtPatientCityProvince.Text = dataGridViewRow.Cells["city_province"].Value?.ToString();
    			txtPatientMedicalHistory.Text = dataGridViewRow.Cells["medical_history"].Value?.ToString();
    			txtPatientFamilyMedicalHistory.Text = dataGridViewRow.Cells["family_medical_history"].Value?.ToString();
    			txtPatientDrugAllergies.Text = dataGridViewRow.Cells["drug_allergies"].Value?.ToString();
    			txtPatientUsernameDb.Text = dataGridViewRow.Cells["username_db"].Value?.ToString();
    		}
    	}
    
    	private void btnPatientAdd_Click(object sender, EventArgs e)
    	{
    		try
    		{
    			string patientId = txtPatientId.Text.Trim();
    			if (string.IsNullOrEmpty(patientId))
    			{
    				MessageBox.Show("Vui lòng nhập Mã bệnh nhân để thêm mới!");
    				return;
    			}
    			string fullName = txtPatientFullName.Text.Trim();
    			string gender = txtPatientGender.Text.Trim();
    			DateTime birthDate = DateTime.ParseExact(txtPatientBirthDate.Text.Trim(), "dd/MM/yyyy", null);
    			string idCard = txtPatientIdCard.Text.Trim();
    			// Khi thêm mới, username_db thường được đặt giống mã bệnh nhân
    			string usernameDb = patientId; 
    			_coordinatorService.InsertPatient(patientId, fullName, gender, birthDate, idCard, txtPatientHouseNo.Text, txtPatientStreet.Text, txtPatientDistrict.Text, txtPatientCityProvince.Text, txtPatientMedicalHistory.Text, txtPatientFamilyMedicalHistory.Text, txtPatientDrugAllergies.Text, usernameDb);
    			MessageBox.Show("Thêm mới bệnh nhân thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    			LoadData();
    		}
    		catch (Exception ex)
    		{
    			ShowError(ex);
    		}
    	}
    
    	private void btnPatientUpdate_Click(object sender, EventArgs e)
    	{
    		try
    		{
    			string text = txtPatientId.Text.Trim();
    			if (string.IsNullOrEmpty(text))
    			{
    				MessageBox.Show("Vui lòng chọn bệnh nhân cần cập nhật!");
    				return;
    			}
    			DataRow[] array = dtPatients.Select("patient_id = '" + text + "'");
    			if (array.Length != 0)
    			{
    				string fullName = txtPatientFullName.Text.Trim();
    				string gender = txtPatientGender.Text.Trim();
    				DateTime birthDate = DateTime.ParseExact(txtPatientBirthDate.Text.Trim(), "dd/MM/yyyy", null);
    				string idCard = txtPatientIdCard.Text.Trim();
    				string usernameDb = array[0]["username_db"].ToString() ?? "";
    				_coordinatorService.UpdatePatient(text, fullName, gender, birthDate, idCard, txtPatientHouseNo.Text, txtPatientStreet.Text, txtPatientDistrict.Text, txtPatientCityProvince.Text, txtPatientMedicalHistory.Text, txtPatientFamilyMedicalHistory.Text, txtPatientDrugAllergies.Text, usernameDb);
    				MessageBox.Show("Cập nhật thông tin bệnh nhân thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    				LoadData();
    			}
    		}
    		catch (Exception ex)
    		{
    			ShowError(ex);
    		}
    	}
    
    	private void btnPatientClear_Click(object sender, EventArgs e)
    	{
    		txtPatientId.Clear();
    		txtPatientFullName.Clear();
    		txtPatientGender.Clear();
    		txtPatientBirthDate.Clear();
    		txtPatientIdCard.Clear();
    		txtPatientHouseNo.Clear();
    		txtPatientStreet.Clear();
    		txtPatientDistrict.Clear();
    		txtPatientCityProvince.Clear();
    		txtPatientMedicalHistory.Clear();
    		txtPatientFamilyMedicalHistory.Clear();
    		txtPatientDrugAllergies.Clear();
    		txtPatientUsernameDb.Clear();
    	}
    
    	private void cmbUnifiedDept_SelectedIndexChanged(object sender, EventArgs e)
    	{
    		if (cmbMRDeptId.SelectedValue != null && cmbMRDeptId.SelectedValue is string deptId)
    		{
    			try
    			{
    				cmbMRDoctorId.DataSource = _coordinatorService.GetDoctorsByDepartment(deptId);
    				cmbMRDoctorId.DisplayMember = "display_name";
    				cmbMRDoctorId.ValueMember = "staff_id";
    			}
    			catch (Exception ex)
    			{
    				ShowError(ex);
    			}
    		}
    	}
    
    	private void btnMRCreate_Click(object sender, EventArgs e)
    	{
    		try
    		{
    			string text = txtMRRecordId.Text.Trim();
    			string text2 = "";
    			if (cmbMRPatientId.SelectedItem is DataRowView drvPat) text2 = drvPat["patient_id"].ToString();
    			
    			string text3 = "";
    			if (cmbMRDeptId.SelectedItem is DataRowView drvDept) text3 = drvDept["dept_id"].ToString();
    			
    			string text4 = "";
    			if (cmbMRDoctorId.SelectedItem is DataRowView drvDoc) text4 = drvDoc["staff_id"].ToString();

    			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2) || string.IsNullOrEmpty(text3) || string.IsNullOrEmpty(text4))
    			{
    				MessageBox.Show("Vui lòng chọn hợp lệ từ danh sách thả xuống (Bệnh nhân, Khoa, Bác sĩ).", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    				return;
    			}
    			_coordinatorService.InsertMedicalRecord(text, text2, dtpMRRecordDate.Value, text4, text3);
    			MessageBox.Show("Đã tạo HSBA " + text + " và phân công Bác sĩ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    			LoadData();
    		}
    		catch (Exception ex)
    		{
    			ShowError(ex);
    		}
    	}
    
    	private void cmbAssignDept_SelectedIndexChanged(object sender, EventArgs e)
    	{
    		if (cmbAssignDept.SelectedValue != null && cmbAssignDept.SelectedValue is string deptId)
    		{
    			try
    			{
    				cmbAssignDoctor.DataSource = _coordinatorService.GetDoctorsByDepartment(deptId);
    				cmbAssignDoctor.DisplayMember = "display_name";
    				cmbAssignDoctor.ValueMember = "staff_id";
    			}
    			catch (Exception ex)
    			{
    				ShowError(ex);
    			}
    		}
    	}
    
    	private void dgvMedicalRecords_CellClick(object sender, DataGridViewCellEventArgs e)
    	{
    		if (e.RowIndex >= 0)
    		{
    			DataGridViewRow dataGridViewRow = dgvMedicalRecords.Rows[e.RowIndex];
    			txtAssignMRId.Text = dataGridViewRow.Cells["record_id"].Value?.ToString();
    			txtAssignMRPatient.Text = dataGridViewRow.Cells["patient_id"].Value?.ToString();
    			if (dataGridViewRow.Cells["record_date"].Value != DBNull.Value)
    			{
    				txtAssignMRDate.Text = Convert.ToDateTime(dataGridViewRow.Cells["record_date"].Value).ToString("dd/MM/yyyy");
    			}
    			string text = dataGridViewRow.Cells["dept_id"].Value?.ToString();
    			if (!string.IsNullOrEmpty(text))
    			{
    				cmbAssignDept.SelectedValue = text;
    			}
    		}
    	}
    
    	private void btnUpdateAssignDoctor_Click(object sender, EventArgs e)
    	{
    		try
    		{
    			string text = txtAssignMRId.Text.Trim();
    			if (string.IsNullOrEmpty(text))
    			{
    				MessageBox.Show("Vui lòng chọn HSBA từ danh sách trước khi cập nhật.");
    				return;
    			}
    			string text2 = "";
    			if (cmbAssignDoctor.SelectedItem is DataRowView drvDoc) text2 = drvDoc["staff_id"].ToString();
    			
    			string text3 = "";
    			if (cmbAssignDept.SelectedItem is DataRowView drvDept) text3 = drvDept["dept_id"].ToString();
    			
    			if (string.IsNullOrEmpty(text2) || string.IsNullOrEmpty(text3))
    			{
    				MessageBox.Show("Vui lòng chọn hợp lệ từ danh sách thả xuống (Khoa, Bác sĩ).", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    				return;
    			}
    			
    			if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
    			{
    				_coordinatorService.UpdateCoordinatorFields(text, text2, text3);
    				MessageBox.Show("Cập nhật phân công Bác si thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    				LoadData();
    			}
    		}
    		catch (Exception ex)
    		{
    			ShowError(ex);
    		}
    	}
    
    	private void dgvServiceRecords_CellClick(object sender, DataGridViewCellEventArgs e)
    	{
    		if (e.RowIndex >= 0)
    		{
    			DataGridViewRow dataGridViewRow = dgvServiceRecords.Rows[e.RowIndex];
    			txtAssignSRId.Text = dataGridViewRow.Cells["MAHSBA"].Value?.ToString();
    			txtAssignSRType.Text = dataGridViewRow.Cells["LOAIDV"].Value?.ToString();
    			if (dataGridViewRow.Cells["NGAYDV"].Value != DBNull.Value)
    			{
    				DateTime originalDate = Convert.ToDateTime(dataGridViewRow.Cells["NGAYDV"].Value);
    				txtAssignSRDate.Text = originalDate.ToString("dd/MM/yyyy");
    				txtAssignSRDate.Tag = originalDate;
    			}
    		}
    	}
    
    	private void dgvServiceRecords_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    	{
    		if (e.ColumnIndex >= 0 && dgvServiceRecords.Columns[e.ColumnIndex].Name == "MAKTV" && (e.Value == DBNull.Value || e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString())))
    		{
    			e.Value = "Chua phân công";
    			e.FormattingApplied = true;
    		}
    	}
    
    	private void btnUpdateAssignTech_Click(object sender, EventArgs e)
    	{
    		try
    		{
    			string text = txtAssignSRId.Text.Trim();
    			string text2 = txtAssignSRType.Text.Trim();
    			string text3 = txtAssignSRDate.Text.Trim();
    			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2) || string.IsNullOrEmpty(text3))
    			{
    				MessageBox.Show("Vui lòng chọn Dịch vụ từ danh sách trước khi cập nhật.");
    				return;
    			}
    			DateTime serviceDate;
    			if (txtAssignSRDate.Tag != null)
    			{
    				serviceDate = (DateTime)txtAssignSRDate.Tag;
    			}
    			else
    			{
    				serviceDate = DateTime.ParseExact(text3, "dd/MM/yyyy", null);
    			}
    			
    			string text4 = "";
    			if (cmbAssignTech.SelectedItem is DataRowView drvTech) text4 = drvTech["staff_id"].ToString();
    			
    			if (string.IsNullOrEmpty(text4))
    			{
    				MessageBox.Show("Vui lòng chọn Kỹ thuật viên hợp lệ từ danh sách.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    				return;
    			}

    			if (!string.IsNullOrEmpty(text4))
    			{
    				_coordinatorService.UpdateTechnician(text, text2, serviceDate, text4);
    				MessageBox.Show("Phân công Kỹ thuật viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    				LoadData();
    			}
    		}
    		catch (Exception ex)
    		{
    			ShowError(ex);
    		}
    	}
    
    	private void LoadProfile()
    	{
    		try
    		{
    			DataTable selfStaffInfo = _coordinatorService.GetSelfStaffInfo();
    			if (selfStaffInfo.Rows.Count > 0)
    			{
    				DataRow dataRow = selfStaffInfo.Rows[0];
    				txtSelfStaffId.Text = dataRow["staff_id"].ToString();
    				txtSelfFullName.Text = dataRow["full_name"].ToString();
    				txtSelfRole.Text = dataRow["staff_role"].ToString();
    				txtSelfSpecialty.Text = dataRow["specialty"].ToString();
    				txtSelfPhone.Text = dataRow["phone"].ToString();
    				txtSelfHometown.Text = dataRow["hometown"].ToString();
    			}
    		}
    		catch (Exception ex)
    		{
    			ShowError(ex);
    		}
    	}
    
    	private void btnUpdateProfile_Click(object sender, EventArgs e)
    	{
    		try
    		{
    			_coordinatorService.UpdateSelfStaffInfo(txtSelfPhone.Text, txtSelfHometown.Text);
    			MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    			LoadProfile();
    		}
    		catch (Exception ex)
    		{
    			ShowError(ex);
    		}
    	}
    
    	private void tabCoordinator_SelectedIndexChanged(object sender, EventArgs e)
    	{
    		if (tabCoordinator.SelectedTab == tabProfile)
    		{
    			LoadProfile();
    		}
    	}
    
    	private string GenerateNewMRId()
    	{
    		if (dtMedicalRecords == null || dtMedicalRecords.Rows.Count == 0)
    			return "HSBA001";
    
    		int maxId = 0;
    		string prefix = "HSBA";
    		int numLength = 3;
    
    		foreach (DataRow row in dtMedicalRecords.Rows)
    		{
    			string idStr = row["record_id"].ToString();
    			var match = System.Text.RegularExpressions.Regex.Match(idStr, @"^([A-Za-z]+)(\d+)$");
    			if (match.Success)
    			{
    				prefix = match.Groups[1].Value;
    				string numStr = match.Groups[2].Value;
    				if (numStr.Length > numLength) numLength = numStr.Length;
    				if (int.TryParse(numStr, out int num))
    				{
    					if (num > maxId) maxId = num;
    				}
    			}
    			else if (int.TryParse(idStr, out int num2))
    			{
    				prefix = "";
    				if (idStr.Length > numLength) numLength = idStr.Length;
    				if (num2 > maxId) maxId = num2;
    			}
    		}
    		
    		if (string.IsNullOrEmpty(prefix)) prefix = "HSBA";
    		return $"{prefix}{(maxId + 1).ToString().PadLeft(numLength, '0')}";
    	}
    }
}


