using System;
using System.Windows.Forms;

namespace QuanLyYTe.Forms.DBA
{
    partial class frmEditUser
    {
        // Required designer variable.
        private System.ComponentModel.IContainer components = null;
        // Clean up any resources being used.
        // <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        // Required method for Designer support - do not modify
        // the contents of this method with the code editor.
        private void InitializeComponent()
        {
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblGender = new System.Windows.Forms.Label();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.lblBirthdate = new System.Windows.Forms.Label();
            this.dtpBirthdate = new System.Windows.Forms.DateTimePicker();
            this.lblIdCard = new System.Windows.Forms.Label();
            this.txtIdCard = new System.Windows.Forms.TextBox();
            this.lblHouseNo = new System.Windows.Forms.Label();
            this.txtHouseNo = new System.Windows.Forms.TextBox();
            this.lblStreet = new System.Windows.Forms.Label();
            this.txtStreet = new System.Windows.Forms.TextBox();
            this.lblDistrict = new System.Windows.Forms.Label();
            this.txtDistrict = new System.Windows.Forms.TextBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.txtCityProvince = new System.Windows.Forms.TextBox();
            this.lblMedHist = new System.Windows.Forms.Label();
            this.txtMedicalHistory = new System.Windows.Forms.TextBox();
            this.lblFamHist = new System.Windows.Forms.Label();
            this.txtFamilyMedicalHistory = new System.Windows.Forms.TextBox();
            this.lblAllergy = new System.Windows.Forms.Label();
            this.txtDrugAllergies = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblHometown = new System.Windows.Forms.Label();
            this.txtHometown = new System.Windows.Forms.TextBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.lblDept = new System.Windows.Forms.Label();
            this.cmbDept = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(20, 20);
            this.lblUsername.Text = "Tên đăng nhập:";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(150, 20);
            this.txtUsername.Width = 250;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(20, 55);
            this.lblPassword.Text = "Mật khẩu:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(150, 55);
            this.txtPassword.Width = 250;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblConfirmPassword
            // 
            this.lblConfirmPassword.AutoSize = true;
            this.lblConfirmPassword.Location = new System.Drawing.Point(20, 90);
            this.lblConfirmPassword.Text = "Xác nhận MK:";
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.Location = new System.Drawing.Point(150, 90);
            this.txtConfirmPassword.Width = 250;
            this.txtConfirmPassword.UseSystemPasswordChar = true;
            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Location = new System.Drawing.Point(20, 125);
            this.lblFullName.Text = "Họ và tên:";
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(150, 125);
            this.txtFullName.Width = 250;
            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Location = new System.Drawing.Point(20, 160);
            this.lblGender.Text = "Giới tính:";
            // 
            // cmbGender
            // 
            this.cmbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGender.Location = new System.Drawing.Point(150, 160);
            this.cmbGender.Width = 250;
            // 
            // lblBirthdate
            // 
            this.lblBirthdate.AutoSize = true;
            this.lblBirthdate.Location = new System.Drawing.Point(20, 195);
            this.lblBirthdate.Text = "Ngày sinh:";
            // 
            // dtpBirthdate
            // 
            this.dtpBirthdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBirthdate.Location = new System.Drawing.Point(150, 195);
            this.dtpBirthdate.Width = 250;
            // 
            // lblIdCard
            // 
            this.lblIdCard.AutoSize = true;
            this.lblIdCard.Location = new System.Drawing.Point(20, 230);
            this.lblIdCard.Text = "CCCD:";
            // 
            // txtIdCard
            // 
            this.txtIdCard.Location = new System.Drawing.Point(150, 230);
            this.txtIdCard.Width = 250;
            // 
            // lblHouseNo
            // 
            this.lblHouseNo.AutoSize = true;
            this.lblHouseNo.Location = new System.Drawing.Point(20, 265);
            this.lblHouseNo.Text = "Số nhà:";
            // 
            // txtHouseNo
            // 
            this.txtHouseNo.Location = new System.Drawing.Point(150, 265);
            this.txtHouseNo.Width = 250;
            // 
            // lblStreet
            // 
            this.lblStreet.AutoSize = true;
            this.lblStreet.Location = new System.Drawing.Point(20, 300);
            this.lblStreet.Text = "Tên đường:";
            // 
            // txtStreet
            // 
            this.txtStreet.Location = new System.Drawing.Point(150, 300);
            this.txtStreet.Width = 250;
            // 
            // lblDistrict
            // 
            this.lblDistrict.AutoSize = true;
            this.lblDistrict.Location = new System.Drawing.Point(20, 335);
            this.lblDistrict.Text = "Quận/Huyện:";
            // 
            // txtDistrict
            // 
            this.txtDistrict.Location = new System.Drawing.Point(150, 335);
            this.txtDistrict.Width = 250;
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(20, 370);
            this.lblCity.Text = "Tỉnh/Thành phố:";
            // 
            // txtCityProvince
            // 
            this.txtCityProvince.Location = new System.Drawing.Point(150, 370);
            this.txtCityProvince.Width = 250;
            // 
            // lblMedHist
            // 
            this.lblMedHist.AutoSize = true;
            this.lblMedHist.Location = new System.Drawing.Point(20, 405);
            this.lblMedHist.Text = "Tiền sử bệnh lý:";
            // 
            // txtMedicalHistory
            // 
            this.txtMedicalHistory.Location = new System.Drawing.Point(150, 405);
            this.txtMedicalHistory.Width = 250;
            this.txtMedicalHistory.Multiline = true;
            this.txtMedicalHistory.Height = 50;
            // 
            // lblFamHist
            // 
            this.lblFamHist.AutoSize = true;
            this.lblFamHist.Location = new System.Drawing.Point(20, 470);
            this.lblFamHist.Text = "Bệnh lý gia đình:";
            // 
            // txtFamilyMedicalHistory
            // 
            this.txtFamilyMedicalHistory.Location = new System.Drawing.Point(150, 470);
            this.txtFamilyMedicalHistory.Width = 250;
            this.txtFamilyMedicalHistory.Multiline = true;
            this.txtFamilyMedicalHistory.Height = 50;
            // 
            // lblAllergy
            // 
            this.lblAllergy.AutoSize = true;
            this.lblAllergy.Location = new System.Drawing.Point(20, 535);
            this.lblAllergy.Text = "Dị ứng thuốc:";
            // 
            // txtDrugAllergies
            // 
            this.txtDrugAllergies.Location = new System.Drawing.Point(150, 535);
            this.txtDrugAllergies.Width = 250;
            this.txtDrugAllergies.Multiline = true;
            this.txtDrugAllergies.Height = 50;
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(20, 265);
            this.lblPhone.Text = "Số điện thoại:";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(150, 265);
            this.txtPhone.Width = 250;
            // 
            // lblHometown
            // 
            this.lblHometown.AutoSize = true;
            this.lblHometown.Location = new System.Drawing.Point(20, 300);
            this.lblHometown.Text = "Quê quán:";
            // 
            // txtHometown
            // 
            this.txtHometown.Location = new System.Drawing.Point(150, 300);
            this.txtHometown.Width = 250;
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Location = new System.Drawing.Point(20, 335);
            this.lblRole.Text = "Vai trò:";
            // 
            // cmbRole
            // 
            this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRole.Location = new System.Drawing.Point(150, 335);
            this.cmbRole.Width = 250;
            // 
            // lblDept
            // 
            this.lblDept.AutoSize = true;
            this.lblDept.Location = new System.Drawing.Point(20, 370);
            this.lblDept.Text = "Chuyên khoa:";
            // 
            // cmbDept
            // 
            this.cmbDept.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDept.Location = new System.Drawing.Point(150, 370);
            this.cmbDept.Width = 250;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(150, 600);
            this.btnOk.Size = new System.Drawing.Size(75, 30);
            this.btnOk.Text = "Đồng ý";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(240, 600);
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.Text = "Hủy";
            // 
            // frmEditUser
            // 
            this.ClientSize = new System.Drawing.Size(430, 660);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblConfirmPassword);
            this.Controls.Add(this.txtConfirmPassword);
            this.Controls.Add(this.lblFullName);
            this.Controls.Add(this.txtFullName);
            this.Controls.Add(this.lblGender);
            this.Controls.Add(this.cmbGender);
            this.Controls.Add(this.lblBirthdate);
            this.Controls.Add(this.dtpBirthdate);
            this.Controls.Add(this.lblIdCard);
            this.Controls.Add(this.txtIdCard);
            this.Controls.Add(this.lblHouseNo);
            this.Controls.Add(this.txtHouseNo);
            this.Controls.Add(this.lblStreet);
            this.Controls.Add(this.txtStreet);
            this.Controls.Add(this.lblDistrict);
            this.Controls.Add(this.txtDistrict);
            this.Controls.Add(this.lblCity);
            this.Controls.Add(this.txtCityProvince);
            this.Controls.Add(this.lblMedHist);
            this.Controls.Add(this.txtMedicalHistory);
            this.Controls.Add(this.lblFamHist);
            this.Controls.Add(this.txtFamilyMedicalHistory);
            this.Controls.Add(this.lblAllergy);
            this.Controls.Add(this.txtDrugAllergies);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.lblHometown);
            this.Controls.Add(this.txtHometown);
            this.Controls.Add(this.lblRole);
            this.Controls.Add(this.cmbRole);
            this.Controls.Add(this.lblDept);
            this.Controls.Add(this.cmbDept);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmEditUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        // Common controls
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.Label lblBirthdate;
        private System.Windows.Forms.Label lblIdCard;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.ComboBox cmbGender;
        private System.Windows.Forms.DateTimePicker dtpBirthdate;
        private System.Windows.Forms.TextBox txtIdCard;

        // Staff controls
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblHometown;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.Label lblDept;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtHometown;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.ComboBox cmbDept;

        // Patient controls
        private System.Windows.Forms.Label lblHouseNo;
        private System.Windows.Forms.Label lblStreet;
        private System.Windows.Forms.Label lblDistrict;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.Label lblMedHist;
        private System.Windows.Forms.Label lblFamHist;
        private System.Windows.Forms.Label lblAllergy;
        private System.Windows.Forms.TextBox txtHouseNo;
        private System.Windows.Forms.TextBox txtStreet;
        private System.Windows.Forms.TextBox txtDistrict;
        private System.Windows.Forms.TextBox txtCityProvince;
        private System.Windows.Forms.TextBox txtMedicalHistory;
        private System.Windows.Forms.TextBox txtFamilyMedicalHistory;
        private System.Windows.Forms.TextBox txtDrugAllergies;

        // Buttons
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}
