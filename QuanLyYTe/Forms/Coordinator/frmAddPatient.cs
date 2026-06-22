using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyYTe.Services;

namespace QuanLyYTe.Forms.Coordinator
{
    public partial class frmAddPatient : Form
    {
        private CoordinatorService _coordinatorService;
        private string _usernameDb;

        public frmAddPatient(string usernameDb)
        {
            InitializeComponent();
            _coordinatorService = new CoordinatorService();
            _usernameDb = usernameDb;

            this.BackColor = Color.FromArgb(245, 244, 242);
            btnSave.BackColor = Color.FromArgb(255, 140, 40);
            btnSave.ForeColor = Color.White;
            btnCancel.BackColor = Color.LightGray;

            this.Text = "Thêm bệnh nhân mới";
            string maxId = _coordinatorService.GetMaxPatientId();
            if (maxId.StartsWith("BN") && maxId.Length > 2 && int.TryParse(maxId.Substring(2), out int seq))
            {
                txtPatientId.Text = "BN" + (seq + 1).ToString("D3");
            }
            else
            {
                txtPatientId.Text = "BN001";
            }
            txtPatientId.ReadOnly = true;
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
                    _usernameDb
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

