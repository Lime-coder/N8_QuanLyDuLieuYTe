namespace QuanLyYTe.Forms.Coordinator
{
    partial class frmAddPatient
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblPatientId = new System.Windows.Forms.Label();
            this.txtPatientId = new System.Windows.Forms.TextBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblGender = new System.Windows.Forms.Label();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.lblDOB = new System.Windows.Forms.Label();
            this.dtpDOB = new System.Windows.Forms.DateTimePicker();
            this.lblIdCard = new System.Windows.Forms.Label();
            this.txtIdCard = new System.Windows.Forms.TextBox();
            this.lblHouseNo = new System.Windows.Forms.Label();
            this.txtHouseNo = new System.Windows.Forms.TextBox();
            this.lblStreet = new System.Windows.Forms.Label();
            this.txtStreet = new System.Windows.Forms.TextBox();
            this.lblDistrict = new System.Windows.Forms.Label();
            this.txtDistrict = new System.Windows.Forms.TextBox();
            this.lblCityProvince = new System.Windows.Forms.Label();
            this.txtCityProvince = new System.Windows.Forms.TextBox();
            this.lblMedicalHistory = new System.Windows.Forms.Label();
            this.txtMedicalHistory = new System.Windows.Forms.TextBox();
            this.lblFamilyHistory = new System.Windows.Forms.Label();
            this.txtFamilyHistory = new System.Windows.Forms.TextBox();
            this.lblDrugAllergies = new System.Windows.Forms.Label();
            this.txtDrugAllergies = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblPatientId
            // 
            this.lblPatientId.AutoSize = true;
            this.lblPatientId.Location = new System.Drawing.Point(30, 20);
            this.lblPatientId.Name = "lblPatientId";
            this.lblPatientId.Size = new System.Drawing.Size(109, 20);
            this.lblPatientId.TabIndex = 0;
            this.lblPatientId.Text = "Mã bệnh nhân:";
            // 
            // txtPatientId
            // 
            this.txtPatientId.Location = new System.Drawing.Point(150, 20);
            this.txtPatientId.Name = "txtPatientId";
            this.txtPatientId.Size = new System.Drawing.Size(200, 27);
            this.txtPatientId.TabIndex = 1;
            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Location = new System.Drawing.Point(30, 60);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(59, 20);
            this.lblFullName.TabIndex = 2;
            this.lblFullName.Text = "Họ tên:";
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(150, 60);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(200, 27);
            this.txtFullName.TabIndex = 3;
            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Location = new System.Drawing.Point(30, 100);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(68, 20);
            this.lblGender.TabIndex = 4;
            this.lblGender.Text = "Giới tính:";
            // 
            // cmbGender
            // 
            this.cmbGender.FormattingEnabled = true;
            this.cmbGender.Items.AddRange(new object[] {
            "Nam",
            "Nữ"});
            this.cmbGender.Location = new System.Drawing.Point(150, 100);
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(200, 28);
            this.cmbGender.TabIndex = 5;
            // 
            // lblDOB
            // 
            this.lblDOB.AutoSize = true;
            this.lblDOB.Location = new System.Drawing.Point(30, 140);
            this.lblDOB.Name = "lblDOB";
            this.lblDOB.Size = new System.Drawing.Size(77, 20);
            this.lblDOB.TabIndex = 6;
            this.lblDOB.Text = "Ngày sinh:";
            // 
            // dtpDOB
            // 
            this.dtpDOB.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDOB.Location = new System.Drawing.Point(150, 140);
            this.dtpDOB.Name = "dtpDOB";
            this.dtpDOB.Size = new System.Drawing.Size(200, 27);
            this.dtpDOB.TabIndex = 7;
            // 
            // lblIdCard
            // 
            this.lblIdCard.AutoSize = true;
            this.lblIdCard.Location = new System.Drawing.Point(30, 180);
            this.lblIdCard.Name = "lblIdCard";
            this.lblIdCard.Size = new System.Drawing.Size(50, 20);
            this.lblIdCard.TabIndex = 8;
            this.lblIdCard.Text = "CCCD:";
            // 
            // txtIdCard
            // 
            this.txtIdCard.Location = new System.Drawing.Point(150, 180);
            this.txtIdCard.Name = "txtIdCard";
            this.txtIdCard.Size = new System.Drawing.Size(200, 27);
            this.txtIdCard.TabIndex = 9;
            // 
            // lblHouseNo
            // 
            this.lblHouseNo.AutoSize = true;
            this.lblHouseNo.Location = new System.Drawing.Point(30, 220);
            this.lblHouseNo.Name = "lblHouseNo";
            this.lblHouseNo.Size = new System.Drawing.Size(57, 20);
            this.lblHouseNo.TabIndex = 10;
            this.lblHouseNo.Text = "Số nhà:";
            // 
            // txtHouseNo
            // 
            this.txtHouseNo.Location = new System.Drawing.Point(150, 220);
            this.txtHouseNo.Name = "txtHouseNo";
            this.txtHouseNo.Size = new System.Drawing.Size(200, 27);
            this.txtHouseNo.TabIndex = 11;
            // 
            // lblStreet
            // 
            this.lblStreet.AutoSize = true;
            this.lblStreet.Location = new System.Drawing.Point(30, 260);
            this.lblStreet.Name = "lblStreet";
            this.lblStreet.Size = new System.Drawing.Size(57, 20);
            this.lblStreet.TabIndex = 12;
            this.lblStreet.Text = "Đường:";
            // 
            // txtStreet
            // 
            this.txtStreet.Location = new System.Drawing.Point(150, 260);
            this.txtStreet.Name = "txtStreet";
            this.txtStreet.Size = new System.Drawing.Size(200, 27);
            this.txtStreet.TabIndex = 13;
            // 
            // lblDistrict
            // 
            this.lblDistrict.AutoSize = true;
            this.lblDistrict.Location = new System.Drawing.Point(30, 300);
            this.lblDistrict.Name = "lblDistrict";
            this.lblDistrict.Size = new System.Drawing.Size(92, 20);
            this.lblDistrict.TabIndex = 14;
            this.lblDistrict.Text = "Quận/Huyện:";
            // 
            // txtDistrict
            // 
            this.txtDistrict.Location = new System.Drawing.Point(150, 300);
            this.txtDistrict.Name = "txtDistrict";
            this.txtDistrict.Size = new System.Drawing.Size(200, 27);
            this.txtDistrict.TabIndex = 15;
            // 
            // lblCityProvince
            // 
            this.lblCityProvince.AutoSize = true;
            this.lblCityProvince.Location = new System.Drawing.Point(30, 340);
            this.lblCityProvince.Name = "lblCityProvince";
            this.lblCityProvince.Size = new System.Drawing.Size(115, 20);
            this.lblCityProvince.TabIndex = 16;
            this.lblCityProvince.Text = "Tỉnh/Thành phố:";
            // 
            // txtCityProvince
            // 
            this.txtCityProvince.Location = new System.Drawing.Point(150, 340);
            this.txtCityProvince.Name = "txtCityProvince";
            this.txtCityProvince.Size = new System.Drawing.Size(200, 27);
            this.txtCityProvince.TabIndex = 17;
            // 
            // lblMedicalHistory
            // 
            this.lblMedicalHistory.AutoSize = true;
            this.lblMedicalHistory.Location = new System.Drawing.Point(400, 20);
            this.lblMedicalHistory.Name = "lblMedicalHistory";
            this.lblMedicalHistory.Size = new System.Drawing.Size(96, 20);
            this.lblMedicalHistory.TabIndex = 18;
            this.lblMedicalHistory.Text = "Tiền sử bệnh:";
            // 
            // txtMedicalHistory
            // 
            this.txtMedicalHistory.Location = new System.Drawing.Point(520, 20);
            this.txtMedicalHistory.Multiline = true;
            this.txtMedicalHistory.Name = "txtMedicalHistory";
            this.txtMedicalHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMedicalHistory.Size = new System.Drawing.Size(250, 80);
            this.txtMedicalHistory.TabIndex = 19;
            // 
            // lblFamilyHistory
            // 
            this.lblFamilyHistory.AutoSize = true;
            this.lblFamilyHistory.Location = new System.Drawing.Point(400, 120);
            this.lblFamilyHistory.Name = "lblFamilyHistory";
            this.lblFamilyHistory.Size = new System.Drawing.Size(117, 20);
            this.lblFamilyHistory.TabIndex = 20;
            this.lblFamilyHistory.Text = "Tiền sử gia đình:";
            // 
            // txtFamilyHistory
            // 
            this.txtFamilyHistory.Location = new System.Drawing.Point(520, 120);
            this.txtFamilyHistory.Multiline = true;
            this.txtFamilyHistory.Name = "txtFamilyHistory";
            this.txtFamilyHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFamilyHistory.Size = new System.Drawing.Size(250, 80);
            this.txtFamilyHistory.TabIndex = 21;
            // 
            // lblDrugAllergies
            // 
            this.lblDrugAllergies.AutoSize = true;
            this.lblDrugAllergies.Location = new System.Drawing.Point(400, 220);
            this.lblDrugAllergies.Name = "lblDrugAllergies";
            this.lblDrugAllergies.Size = new System.Drawing.Size(95, 20);
            this.lblDrugAllergies.TabIndex = 22;
            this.lblDrugAllergies.Text = "Dị ứng thuốc:";
            // 
            // txtDrugAllergies
            // 
            this.txtDrugAllergies.Location = new System.Drawing.Point(520, 220);
            this.txtDrugAllergies.Multiline = true;
            this.txtDrugAllergies.Name = "txtDrugAllergies";
            this.txtDrugAllergies.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDrugAllergies.Size = new System.Drawing.Size(250, 80);
            this.txtDrugAllergies.TabIndex = 23;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(550, 400);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.TabIndex = 24;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(670, 400);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmAddPatient
            // 
            this.ClientSize = new System.Drawing.Size(820, 460);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtDrugAllergies);
            this.Controls.Add(this.lblDrugAllergies);
            this.Controls.Add(this.txtFamilyHistory);
            this.Controls.Add(this.lblFamilyHistory);
            this.Controls.Add(this.txtMedicalHistory);
            this.Controls.Add(this.lblMedicalHistory);
            this.Controls.Add(this.txtCityProvince);
            this.Controls.Add(this.lblCityProvince);
            this.Controls.Add(this.txtDistrict);
            this.Controls.Add(this.lblDistrict);
            this.Controls.Add(this.txtStreet);
            this.Controls.Add(this.lblStreet);
            this.Controls.Add(this.txtHouseNo);
            this.Controls.Add(this.lblHouseNo);
            this.Controls.Add(this.txtIdCard);
            this.Controls.Add(this.lblIdCard);
            this.Controls.Add(this.dtpDOB);
            this.Controls.Add(this.lblDOB);
            this.Controls.Add(this.cmbGender);
            this.Controls.Add(this.lblGender);
            this.Controls.Add(this.txtFullName);
            this.Controls.Add(this.lblFullName);
            this.Controls.Add(this.txtPatientId);
            this.Controls.Add(this.lblPatientId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddPatient";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thông tin bệnh nhân";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblPatientId;
        private System.Windows.Forms.TextBox txtPatientId;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.ComboBox cmbGender;
        private System.Windows.Forms.Label lblDOB;
        private System.Windows.Forms.DateTimePicker dtpDOB;
        private System.Windows.Forms.Label lblIdCard;
        private System.Windows.Forms.TextBox txtIdCard;
        private System.Windows.Forms.Label lblHouseNo;
        private System.Windows.Forms.TextBox txtHouseNo;
        private System.Windows.Forms.Label lblStreet;
        private System.Windows.Forms.TextBox txtStreet;
        private System.Windows.Forms.Label lblDistrict;
        private System.Windows.Forms.TextBox txtDistrict;
        private System.Windows.Forms.Label lblCityProvince;
        private System.Windows.Forms.TextBox txtCityProvince;
        private System.Windows.Forms.Label lblMedicalHistory;
        private System.Windows.Forms.TextBox txtMedicalHistory;
        private System.Windows.Forms.Label lblFamilyHistory;
        private System.Windows.Forms.TextBox txtFamilyHistory;
        private System.Windows.Forms.Label lblDrugAllergies;
        private System.Windows.Forms.TextBox txtDrugAllergies;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
