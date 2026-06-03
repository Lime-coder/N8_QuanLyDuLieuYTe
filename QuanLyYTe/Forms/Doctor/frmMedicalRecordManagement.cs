namespace QuanLyYTe.Forms.Doctor
{
    public class frmMedicalRecordManagement : frmDoctorBase
    {
        public frmMedicalRecordManagement()
        {
            LoadD();
            btnA.Visible = false;
            btnD.Visible = false;
        }

        protected override void LoadD() { 
            Dgv.DataSource = Svc.GetMedicalRecords(TxtS.Text); 
            Helpers.GridViewStyler.Format(Dgv); 
        }

        protected override void FormE()
        {
            if (Dgv.CurrentRow == null) return;
            var fields = new Dictionary<string, string> {
                { "Mã MedicalRecord (Không sửa)", Dgv.CurrentRow.Cells["RECORD_ID"].Value.ToString() },
                { "Chẩn đoán", Dgv.CurrentRow.Cells["DIAGNOSIS"].Value.ToString() },
                { "Điều trị", Dgv.CurrentRow.Cells["TREATMENT_PLAN"].Value.ToString() },
                { "Kết luận", Dgv.CurrentRow.Cells["CONCLUSION"].Value.ToString() }
            };

            ShowDialog("Cập nhật bệnh án", fields, r => Svc.SaveMedicalRecord(Dgv.CurrentRow.Cells["RECORD_ID"].Value.ToString(), r["Chẩn đoán"], r["Điều trị"], r["Kết luận"]));
        }
    }
}