using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QuanLyYTe.Helpers;

namespace QuanLyYTe.Forms.Doctor
{
    public class frmPrescriptionManagement : frmDoctorBase
    {
        public frmPrescriptionManagement()
        {
            this.Text = "Quản lý Đơn thuốc";
            Dgv.BringToFront();
            LoadD();
        }

        protected override void LoadD()
        {
            try
            {
                Dgv.DataSource = Svc.GetPrescriptions(TxtS.Text);
                GridViewStyler.Format(Dgv);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi hiển thị danh sách: " + ex.Message); }
        }

        protected override void FormA()
        {
            var fields = new Dictionary<string, string> {
                { "Mã HSBA", "" },
                { "Tên thuốc", "" },
                { "Liều dùng", "" }
            };

            ShowDialog("Kê đơn thuốc mới", fields, r => {
                Svc.SavePrescription("INSERT", r["Mã HSBA"], r["Tên thuốc"], r["Liều dùng"], null);
            });
        }

        protected override void FormE()
        {
            if (Dgv.CurrentRow == null) return;

            string id = Dgv.CurrentRow.Cells["RECORD_ID"].Value.ToString();
            string med = Dgv.CurrentRow.Cells["MEDICINE_NAME"].Value.ToString();
            string oldDosage = Dgv.CurrentRow.Cells["DOSAGE"].Value.ToString();

            
            string dateStr = Dgv.CurrentRow.Cells["PRESCRIPTION_DATE"].Value.ToString();
            DateTime originalDate = DateTime.ParseExact(dateStr, "dd/MM/yyyy", null);

            var fields = new Dictionary<string, string> {
                { "Mã HSBA (Không sửa)", id },
                { "Tên thuốc", med },
                { "Liều dùng mới", oldDosage }
            };

            ShowDialog("Cập nhật đơn thuốc", fields, r => {
                Svc.SavePrescription("UPDATE", id, r["Tên thuốc"], r["Liều dùng mới"], originalDate, med);
            });
        }

        protected override void FormD()
        {
            if (Dgv.CurrentRow == null) return;
            string id = Dgv.CurrentRow.Cells["RECORD_ID"].Value.ToString();
            string med = Dgv.CurrentRow.Cells["MEDICINE_NAME"].Value.ToString();

            string dateStr = Dgv.CurrentRow.Cells["PRESCRIPTION_DATE"].Value.ToString();
            DateTime date = DateTime.ParseExact(dateStr, "dd/MM/yyyy", null);

            if (MessageBox.Show("Xóa thuốc này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    Svc.SavePrescription("DELETE", id, med, "", date, med);
                    MessageBox.Show("Đã xóa!"); LoadD();
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            }
        }
    }
}