using System.Collections.Generic;

namespace QuanLyYTe.Forms.Doctor
{
    public class frmPatientManagement : frmDoctorBase
    {
        public frmPatientManagement() { btnA.Visible = btnD.Visible = false; LoadD(); }
        protected override void LoadD()
        {
            Dgv.DataSource = Svc.GetPatients(TxtS.Text);
            if (Dgv.Columns.Contains("PATIENT_ID")) Dgv.Columns["PATIENT_ID"].HeaderText = "Mã Bệnh Nhân";
            if (Dgv.Columns.Contains("FULL_NAME")) Dgv.Columns["FULL_NAME"].HeaderText = "Họ tên";
            if (Dgv.Columns.Contains("GENDER")) Dgv.Columns["GENDER"].HeaderText = "Giới tính";
            if (Dgv.Columns.Contains("BIRTHDATE")) Dgv.Columns["BIRTHDATE"].HeaderText = "Ngày sinh";
            if (Dgv.Columns.Contains("MEDICAL_HISTORY")) Dgv.Columns["MEDICAL_HISTORY"].HeaderText = "Tiền sử bệnh";
            if (Dgv.Columns.Contains("MEDICAL_HISTORY")) Dgv.Columns["FAMILY_MEDICAL_HISTORY"].HeaderText = "Tiền sử bệnh của gia đình";
            if (Dgv.Columns.Contains("MEDICAL_HISTORY")) Dgv.Columns["DRUG_ALLERGIES"].HeaderText = "Dị ứng thuốc";
            Helpers.GridViewStyler.Format(Dgv);
        }
        protected override void FormE()
        {
            if (Dgv.CurrentRow == null) return;
            var r = Dgv.CurrentRow;
            var f = new Dictionary<string, string> { 
                { "Mã BN (Không sửa)", r.Cells["PATIENT_ID"].Value.ToString() }, 
                { "Tiền sử bệnh lý", r.Cells["MEDICAL_HISTORY"].Value.ToString() }, 
                { "Tiền sử gia đình", r.Cells["FAMILY_MEDICAL_HISTORY"].Value.ToString() }, 
                { "Dị ứng thuốc", r.Cells["DRUG_ALLERGIES"].Value.ToString() } 
            };

            ShowDialog("Cập nhật thông tin bệnh lý", f, res => Svc.SavePatient(res["Mã BN (Không sửa)"], res["Tiền sử bệnh lý"], res["Tiền sử gia đình"], res["Dị ứng thuốc"]));
        }
    }
}