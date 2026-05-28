using System.Data;
using QuanLyYTe.Services;

namespace QuanLyYTe.Forms.DBA
{
    public enum EditUserDialogMode
    {
        Create,
        Edit
    }

    public class frmEditUser : Form
    {
        private readonly EditUserDialogMode _mode;
        private readonly bool _isPatient;
        private readonly SecurityAdminService _service = new SecurityAdminService();

        // Common Fields
        private TextBox txtUsername = null!;
        private TextBox txtPassword = null!;
        private TextBox txtConfirmPassword = null!;
        private TextBox txtFullName = null!;
        private ComboBox cmbGender = null!;
        private DateTimePicker dtpBirthdate = null!;
        private TextBox txtIdCard = null!;

        // Staff Fields
        private TextBox txtPhone = null!;
        private TextBox txtHometown = null!;
        private ComboBox cmbRole = null!;
        private ComboBox cmbDept = null!;
        private Label lblDept = null!;

        // Patient Fields
        private TextBox txtHouseNo = null!;
        private TextBox txtStreet = null!;
        private TextBox txtDistrict = null!;
        private TextBox txtCityProvince = null!;
        private TextBox txtMedicalHistory = null!;
        private TextBox txtFamilyMedicalHistory = null!;
        private TextBox txtDrugAllergies = null!;

        private Button btnOk = null!;
        private Button btnCancel = null!;

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
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin người dùng: " + ex.Message);
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            int labelX = 20, inputX = 150, startY = 20, spacing = 35, inputWidth = 250;

            Label AddLabel(string text, int y) {
                var l = new Label { Text = text, Location = new Point(labelX, y), AutoSize = true };
                Controls.Add(l);
                return l;
            }

            AddLabel("Tên đăng nhập:", startY);
            txtUsername = new TextBox { Location = new Point(inputX, startY), Width = inputWidth };
            
            AddLabel("Mật khẩu:", startY += spacing);
            txtPassword = new TextBox { Location = new Point(inputX, startY), Width = inputWidth, UseSystemPasswordChar = true };

            AddLabel("Xác nhận MK:", startY += spacing);
            txtConfirmPassword = new TextBox { Location = new Point(inputX, startY), Width = inputWidth, UseSystemPasswordChar = true };

            AddLabel("Họ và tên:", startY += spacing);
            txtFullName = new TextBox { Location = new Point(inputX, startY), Width = inputWidth };

            AddLabel("Giới tính:", startY += spacing);
            cmbGender = new ComboBox { Location = new Point(inputX, startY), Width = inputWidth, DropDownStyle = ComboBoxStyle.DropDownList };

            AddLabel("Ngày sinh:", startY += spacing);
            dtpBirthdate = new DateTimePicker { Location = new Point(inputX, startY), Width = inputWidth, Format = DateTimePickerFormat.Short };

            AddLabel("CCCD:", startY += spacing);
            txtIdCard = new TextBox { Location = new Point(inputX, startY), Width = inputWidth };

            Controls.AddRange(new Control[] { txtUsername, txtPassword, txtConfirmPassword, txtFullName, cmbGender, dtpBirthdate, txtIdCard });

            if (_isPatient)
            {
                AddLabel("Số nhà:", startY += spacing);
                txtHouseNo = new TextBox { Location = new Point(inputX, startY), Width = inputWidth };

                AddLabel("Tên đường:", startY += spacing);
                txtStreet = new TextBox { Location = new Point(inputX, startY), Width = inputWidth };

                AddLabel("Quận/Huyện:", startY += spacing);
                txtDistrict = new TextBox { Location = new Point(inputX, startY), Width = inputWidth };

                AddLabel("Tỉnh/Thành phố:", startY += spacing);
                txtCityProvince = new TextBox { Location = new Point(inputX, startY), Width = inputWidth };

                AddLabel("Tiền sử bệnh lý:", startY += spacing);
                txtMedicalHistory = new TextBox { Location = new Point(inputX, startY), Width = inputWidth, Multiline = true, Height = 50 };
                startY += 30; // Extra space for multiline

                AddLabel("Bệnh lý gia đình:", startY += spacing);
                txtFamilyMedicalHistory = new TextBox { Location = new Point(inputX, startY), Width = inputWidth, Multiline = true, Height = 50 };
                startY += 30;

                AddLabel("Dị ứng thuốc:", startY += spacing);
                txtDrugAllergies = new TextBox { Location = new Point(inputX, startY), Width = inputWidth, Multiline = true, Height = 50 };
                startY += 30;

                Controls.AddRange(new Control[] { txtHouseNo, txtStreet, txtDistrict, txtCityProvince, txtMedicalHistory, txtFamilyMedicalHistory, txtDrugAllergies });
            }
            else
            {
                AddLabel("Số điện thoại:", startY += spacing);
                txtPhone = new TextBox { Location = new Point(inputX, startY), Width = inputWidth };

                AddLabel("Quê quán:", startY += spacing);
                txtHometown = new TextBox { Location = new Point(inputX, startY), Width = inputWidth };

                AddLabel("Vai trò:", startY += spacing);
                cmbRole = new ComboBox { Location = new Point(inputX, startY), Width = inputWidth, DropDownStyle = ComboBoxStyle.DropDownList };
                cmbRole.SelectedIndexChanged += (s, e) => UpdateDeptVisibility();

                lblDept = AddLabel("Chuyên khoa:", startY += spacing);
                cmbDept = new ComboBox { Location = new Point(inputX, startY), Width = inputWidth, DropDownStyle = ComboBoxStyle.DropDownList };

                Controls.AddRange(new Control[] { txtPhone, txtHometown, cmbRole, cmbDept });
            }

            btnOk = new Button { Text = "Đồng ý", Location = new Point(inputX + inputWidth - 160, startY + 45), Size = new Size(75, 30) };
            btnOk.Click += btnOk_Click;

            btnCancel = new Button { Text = "Hủy", Location = new Point(inputX + inputWidth - 75, startY + 45), Size = new Size(75, 30), DialogResult = DialogResult.Cancel };

            Controls.AddRange(new Control[] { btnOk, btnCancel });

            ClientSize = new Size(labelX + inputX + inputWidth + 20, startY + 90);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;

            if (!_isPatient) UpdateDeptVisibility();
            
            ResumeLayout(false);
        }

        private void UpdateDeptVisibility()
        {
            if (_isPatient) return;
            bool isDoctor = Role == "RL_DOCTOR";
            lblDept.Visible = isDoctor;
            cmbDept.Visible = isDoctor;
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

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
