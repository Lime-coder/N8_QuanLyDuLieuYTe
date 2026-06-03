using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QuanLyYTe.Helpers;

namespace QuanLyYTe.Forms.Doctor
{
    public class frmPatientManagement : frmDoctorBase
    {
        public frmPatientManagement()
        {
            this.Text = "Danh sách Bệnh nhân đã điều trị";

            btnA.Visible = false; // Hide button "Tạo"
            btnD.Visible = false; // Hide button "Xóa"

            Dgv.BringToFront();
            LoadD();
        }

        protected override void LoadD()
        {
            try
            {
                // Get Patients List
                Dgv.DataSource = Svc.GetPatients(TxtS.Text);
                GridViewStyler.Format(Dgv);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi hiển thị: " + ex.Message); }
        }

        protected override void FormE()
        {
            if (Dgv.CurrentRow == null) return;

            string id = Dgv.CurrentRow.Cells["PATIENT_ID"].Value.ToString();
            string name = Dgv.CurrentRow.Cells["FULL_NAME"].Value.ToString();

            var fields = new Dictionary<string, string> {
                { "Bệnh nhân (Không sửa)", name }, // "Không sửa" -> Will be lock
                { "Tiền sử bệnh", Dgv.CurrentRow.Cells["MEDICAL_HISTORY"].Value?.ToString() ?? "" }, // Multiline
                { "Tiền sử gia đình", Dgv.CurrentRow.Cells["FAMILY_MEDICAL_HISTORY"].Value?.ToString() ?? "" },
                { "Dị ứng thuốc", Dgv.CurrentRow.Cells["DRUG_ALLERGIES"].Value?.ToString() ?? "" }
            };

            ShowDialog("Cập nhật thông tin bệnh lý", fields, r => {
                Svc.SavePatient(id, r["Tiền sử bệnh"], r["Tiền sử gia đình"], r["Dị ứng thuốc"]);
            });
        }
    }
}