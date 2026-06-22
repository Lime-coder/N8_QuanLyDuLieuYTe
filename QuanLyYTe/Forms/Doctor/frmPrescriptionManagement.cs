using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization; // Thêm thư viện này

namespace QuanLyYTe.Forms.Doctor
{
    public partial class frmPrescriptionManagement : frmDoctorBase
    {
        public frmPrescriptionManagement() { LoadD(); }
        protected override void LoadD()
        {
            Dgv.DataSource = Svc.GetPrescriptions(TxtS.Text);
            if (Dgv.Columns.Contains("RECORD_ID")) Dgv.Columns["RECORD_ID"].HeaderText = "Mã HSBA";
            if (Dgv.Columns.Contains("PRESCRIPTION_DATE")) Dgv.Columns["PRESCRIPTION_DATE"].HeaderText = "Ngày kê đơn";
            if (Dgv.Columns.Contains("MEDICINE_NAME")) Dgv.Columns["MEDICINE_NAME"].HeaderText = "Tên thuốc";
            if (Dgv.Columns.Contains("DOSAGE")) Dgv.Columns["DOSAGE"].HeaderText = "Liều dùng";
            Helpers.GridViewStyler.Format(Dgv);
        }
        protected override void FormA()
        {
            ShowDialog("Kê đơn thuốc mới", new Dictionary<string, string> {
                { "Mã HSBA", "" },
                { "Tên thuốc", "" },
                { "Liều dùng", "" }
            }, r => Svc.SavePrescription("INSERT", r["Mã HSBA"].Trim(), r["Tên thuốc"], r["Liều dùng"]));
        }

        protected override void FormE()
        {
            if (Dgv.CurrentRow == null) return;
            var r = Dgv.CurrentRow;
            string rid = r.Cells["RECORD_ID"].Value.ToString().Trim();
            string oldMed = r.Cells["MEDICINE_NAME"].Value.ToString();
            DateTime date = DateTime.ParseExact(r.Cells["PRESCRIPTION_DATE"].Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var f = new Dictionary<string, string> {
                { "Mã HSBA (Không sửa)", rid },
                { "Tên thuốc", oldMed },
                { "Liều dùng mới", r.Cells["DOSAGE"].Value.ToString() }
            };

            ShowDialog("Cập nhật đơn thuốc", f, res =>
            {
                Svc.SavePrescription("UPDATE", rid, res["Tên thuốc"], res["Liều dùng mới"], date, oldMed);
                LoadD();
            });
        }

        protected override void FormD()
        {
            if (Dgv.CurrentRow == null) return;

            string rid = Dgv.CurrentRow.Cells["RECORD_ID"].Value.ToString().Trim();
            string med = Dgv.CurrentRow.Cells["MEDICINE_NAME"].Value.ToString();
            DateTime date = DateTime.ParseExact(Dgv.CurrentRow.Cells["PRESCRIPTION_DATE"].Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (MessageBox.Show($"Xác nhận xóa thuốc '{med}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    Svc.SavePrescription("DELETE", rid, med, "", date, med);
                    LoadD();
                    MessageBox.Show("Đã xóa thuốc khỏi đơn thuốc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}