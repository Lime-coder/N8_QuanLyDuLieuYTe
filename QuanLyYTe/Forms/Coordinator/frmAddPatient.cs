using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyYTe.Services;

namespace QuanLyYTe.Forms.Coordinator
{
    public partial class frmAddPatient : Form
    {
        private CoordinatorService _coordinatorService;

        public frmAddPatient()
        {
            InitializeComponent();
            _coordinatorService = new CoordinatorService();

            this.BackColor = Color.FromArgb(245, 244, 242);
            btnSave.BackColor = Color.FromArgb(255, 140, 40);
            btnSave.ForeColor = Color.White;
            btnCancel.BackColor = Color.LightGray;

            this.Text = "Thêm bệnh nhân mới";
            string maxId = _coordinatorService.GetMaxPatientId();
            if (maxId.StartsWith("BN") && maxId.Length > 2 && int.TryParse(maxId.Substring(2), out int seq))
            {
                txtPatientId.Text = "BN" + (seq + 1).ToString("D6");
            }
            else
            {
                txtPatientId.Text = "BN000001";
            }
            txtPatientId.ReadOnly = true;
            cmbGender.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGender.SelectedIndex = 0; // Default to 'Nam'
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtPatientId.Text))
                {
                    MessageBox.Show("Vui lòng nhập Mã bệnh nhân!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _coordinatorService.InsertPatient(
                    txtPatientId.Text.Trim(),
                    txtFullName.Text.Trim(),
                    cmbGender.Text,
                    dtpDOB.Value,
                    txtIdCard.Text.Trim(),
                    txtHouseNo.Text.Trim(),
                    txtStreet.Text.Trim(),
                    txtDistrict.Text.Trim(),
                    txtCityProvince.Text.Trim(),
                    txtMedicalHistory.Text.Trim(),
                    txtFamilyHistory.Text.Trim(),
                    txtDrugAllergies.Text.Trim(),
                    txtPatientId.Text.Trim()
                );
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

