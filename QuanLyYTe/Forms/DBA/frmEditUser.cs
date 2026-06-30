using System.Data;
using QuanLyYTe.Services;

namespace QuanLyYTe.Forms.DBA
{
    public enum EditUserDialogMode
    {
        Create,
        Edit
    }

    public partial class frmEditUser : Form
    {
        private readonly EditUserDialogMode _mode;
        private readonly bool _isPatient;
        private readonly SecurityAdminService _service = new SecurityAdminService();

        public string Username => txtUsername.Text;
        public string Password => txtPassword.Text;
        public string FullName => txtFullName.Text;
        public string Gender => cmbGender.SelectedItem?.ToString() ?? "Nam";
        public DateTime Birthdate => dtpBirthdate.Value;
        public string IdCard => txtIdCard.Text;

        // Staff properties
        public string? Phone => _isPatient ? null : txtPhone.Text;
        public string? Hometown => _isPatient ? null : txtHometown.Text;
        public string Role => _isPatient ? "RL_PATIENT" : ((cmbRole.SelectedItem as dynamic)?.Value ?? "");
        public string? DeptId => _isPatient ? null : (cmbDept.SelectedItem as dynamic)?.Value;
        public string? Facility => _isPatient ? null : cmbFacility.SelectedItem?.ToString();

        // Patient properties
        public string? HouseNo => _isPatient ? txtHouseNo.Text : null;
        public string? Street => _isPatient ? txtStreet.Text : null;
        public string? District => _isPatient ? txtDistrict.Text : null;
        public string? CityProvince => _isPatient ? txtCityProvince.Text : null;
        public string? MedicalHistory => _isPatient ? txtMedicalHistory.Text : null;
        public string? FamilyMedicalHistory => _isPatient ? txtFamilyMedicalHistory.Text : null;
        public string? DrugAllergies => _isPatient ? txtDrugAllergies.Text : null;

        public frmEditUser(EditUserDialogMode mode, bool isPatient, string? presetUsername = null)
        {
            _mode = mode;
            _isPatient = isPatient;
            InitializeComponent();

            btnOk.Click += btnOk_Click;
            if (!_isPatient)
            {
                cmbRole.SelectedIndexChanged += cmbRole_SelectedIndexChanged;
            }

            ConfigureFormForMode();
            LoadData();

            if (!string.IsNullOrWhiteSpace(presetUsername))
                txtUsername.Text = presetUsername;

            if (_mode == EditUserDialogMode.Edit)
            {
                txtUsername.ReadOnly = true;
                txtPassword.Enabled = true;
                txtConfirmPassword.Enabled = true;
                Text = "Sửa " + (_isPatient ? "bệnh nhân" : "nhân viên");
                LoadUserInfo(presetUsername!);
            }
            else
            {
                Text = "Tạo " + (_isPatient ? "bệnh nhân" : "nhân viên");
            }
        }
        // Shows/hides patient vs staff controls and adjusts form size accordingly.
        // Called after InitializeComponent to configure the form based on _isPatient.
        private void ConfigureFormForMode()
        {
            int labelX = 20, inputX = 150, inputWidth = 250;

            // Hide/show patient-specific controls
            lblHouseNo.Visible = _isPatient;
            txtHouseNo.Visible = _isPatient;
            lblStreet.Visible = _isPatient;
            txtStreet.Visible = _isPatient;
            lblDistrict.Visible = _isPatient;
            txtDistrict.Visible = _isPatient;
            lblCity.Visible = _isPatient;
            txtCityProvince.Visible = _isPatient;
            lblMedHist.Visible = _isPatient;
            txtMedicalHistory.Visible = _isPatient;
            lblFamHist.Visible = _isPatient;
            txtFamilyMedicalHistory.Visible = _isPatient;
            lblAllergy.Visible = _isPatient;
            txtDrugAllergies.Visible = _isPatient;

            // Hide/show staff-specific controls
            lblPhone.Visible = !_isPatient;
            txtPhone.Visible = !_isPatient;
            lblHometown.Visible = !_isPatient;
            txtHometown.Visible = !_isPatient;
            lblRole.Visible = !_isPatient;
            cmbRole.Visible = !_isPatient;
            lblDept.Visible = !_isPatient;
            cmbDept.Visible = !_isPatient;
            lblFacility.Visible = !_isPatient;
            cmbFacility.Visible = !_isPatient;

            // Calculate the bottom Y based on which set of controls is visible
            int bottomY;
            if (_isPatient)
            {
                // Patient mode: last visible control is txtDrugAllergies
                bottomY = txtDrugAllergies.Location.Y + txtDrugAllergies.Height;
            }
            else
            {
                // Staff mode: last visible control is cmbFacility
                bottomY = cmbFacility.Location.Y + cmbFacility.Height;
            }

            // Position buttons relative to visible content
            btnOk.Location = new Point(inputX + inputWidth - 160, bottomY + 15);
            btnCancel.Location = new Point(inputX + inputWidth - 75, bottomY + 15);

            // Adjust form size
            ClientSize = new Size(labelX + inputX + inputWidth + 20, bottomY + 60);

            if (!_isPatient) UpdateDeptVisibility();
        }

        private void LoadData()
        {
            // Gender
            cmbGender.Items.Add("Nam");
            cmbGender.Items.Add("Nữ");
            cmbGender.SelectedIndex = 0;

            if (!_isPatient)
            {
                // Role
                var roles = new[]
                {
                    new { Text = "Bác sĩ", Value = "RL_DOCTOR" },
                    new { Text = "Điều phối viên", Value = "RL_COORDINATOR" },
                    new { Text = "Kỹ thuật viên", Value = "RL_TECHNICIAN" }
                };
                cmbRole.DisplayMember = "Text";
                cmbRole.ValueMember = "Value";
                cmbRole.DataSource = roles;

                // Dept
                try
                {
                    DataTable dt = _service.GetAllDepartments();
                    var depts = dt.AsEnumerable().Select(r => new { 
                        Text = r.Field<string>("DEPT_NAME"), 
                        Value = r.Field<string>("DEPT_ID") 
                    }).ToList();
                    cmbDept.DisplayMember = "Text";
                    cmbDept.ValueMember = "Value";
                    cmbDept.DataSource = depts;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể tải danh sách phòng ban: " + ex.Message);
                }

                // Facility
                cmbFacility.Items.Add("Hà Nội");
                cmbFacility.Items.Add("Hải Phòng");
                cmbFacility.Items.Add("Hồ Chí Minh");
                cmbFacility.SelectedIndex = 0;
            }
        }

        private void LoadUserInfo(string username)
        {
            try
            {
                DataTable dt = _service.GetUserInfo(username);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtFullName.Text = row["FULL_NAME"]?.ToString();
                    cmbGender.SelectedItem = row["GENDER"]?.ToString();
                    if (row["BIRTHDATE"] != DBNull.Value)
                        dtpBirthdate.Value = Convert.ToDateTime(row["BIRTHDATE"]);
                    txtIdCard.Text = row["ID_CARD"]?.ToString();

                    if (_isPatient)
                    {
                        txtHouseNo.Text = row["HOUSE_NO"]?.ToString();
                        txtStreet.Text = row["STREET"]?.ToString();
                        txtDistrict.Text = row["DISTRICT"]?.ToString();
                        txtCityProvince.Text = row["CITY_PROVINCE"]?.ToString();
                        txtMedicalHistory.Text = row["MEDICAL_HISTORY"]?.ToString();
                        txtFamilyMedicalHistory.Text = row["FAMILY_MEDICAL_HISTORY"]?.ToString();
                        txtDrugAllergies.Text = row["DRUG_ALLERGIES"]?.ToString();
                    }
                    else
                    {
                        txtPhone.Text = row["PHONE"]?.ToString();
                        txtHometown.Text = row["HOMETOWN"]?.ToString();
                        
                        string role = row["ROLE"]?.ToString() ?? "";
                        foreach (var item in cmbRole.Items)
                        {
                            if ((item as dynamic).Value == role)
                            {
                                cmbRole.SelectedItem = item;
                                break;
                            }
                        }

                        string? deptId = row["DEPT_ID"]?.ToString();
                        if (!string.IsNullOrEmpty(deptId))
                        {
                            foreach (var item in cmbDept.Items)
                            {
                                if ((item as dynamic).Value == deptId)
                                {
                                    cmbDept.SelectedItem = item;
                                    break;
                                }
                            }
                        }

                        string? facility = row["FACILITY"]?.ToString();
                        if (!string.IsNullOrEmpty(facility))
                        {
                            foreach (var item in cmbFacility.Items)
                            {
                                if (item.ToString() == facility)
                                {
                                    cmbFacility.SelectedItem = item;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin người dùng: " + ex.Message);
            }
        }

        private void UpdateDeptVisibility()
        {
            if (_isPatient) return;
            lblDept.Visible = true;
            cmbDept.Visible = true;
        }

        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDeptVisibility();
        }

        private void btnOk_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text)) { MessageBox.Show("Vui lòng nhập tên đăng nhập."); return; }
            
            if (_mode == EditUserDialogMode.Create)
            {
                if (string.IsNullOrWhiteSpace(txtPassword.Text)) { MessageBox.Show("Vui lòng nhập mật khẩu."); return; }
                if (txtPassword.Text != txtConfirmPassword.Text) { MessageBox.Show("Mật khẩu xác nhận không khớp."); return; }
            }
            else // Edit mode
            {
                if (!string.IsNullOrEmpty(txtPassword.Text))
                {
                    if (txtPassword.Text != txtConfirmPassword.Text) { MessageBox.Show("Mật khẩu xác nhận không khớp."); return; }
                }
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text)) { MessageBox.Show("Vui lòng nhập họ và tên."); return; }
            if (string.IsNullOrWhiteSpace(txtIdCard.Text)) { MessageBox.Show("Vui lòng nhập CCCD."); return; }

            if (!_isPatient)
            {
                if (string.IsNullOrWhiteSpace(txtPhone.Text)) { MessageBox.Show("Vui lòng nhập số điện thoại."); return; }
                if (string.IsNullOrWhiteSpace(txtHometown.Text)) { MessageBox.Show("Vui lòng nhập quê quán."); return; }
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
