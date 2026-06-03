namespace QuanLyYTe.Forms.Doctor
{
    public class frmServiceManagement : frmDoctorBase
    {
        public frmServiceManagement()
        {
            LoadD();
            btnE.Visible = false;
        }

        protected override void LoadD() { 
            Dgv.DataSource = Svc.GetServices(TxtS.Text); 
            Helpers.GridViewStyler.Format(Dgv); 
        }

        protected override void FormA()
        {
            var fields = new Dictionary<string, string> {
                { "Mã HSBA", "" },
                { "Loại dịch vụ", "" },
                { "Kết quả", "" }
            };
            ShowDialog("Thêm dịch vụ CLS", fields, r => Svc.CreateService(r["Mã HSBA"], r["Loại dịch vụ"], r["Kết quả"]));
        }

        protected override void FormD()
        {
            if (Dgv.CurrentRow == null) return;

            string id = Dgv.CurrentRow.Cells["RECORD_ID"].Value.ToString();
            string type = Dgv.CurrentRow.Cells["SERVICE_TYPE"].Value.ToString();

            
            string dateStr = Dgv.CurrentRow.Cells["SERVICE_DATE"].Value.ToString();
            DateTime date = DateTime.ParseExact(dateStr, "dd/MM/yyyy", null);

            if (MessageBox.Show($"Xác nhận xóa dịch vụ '{type}'?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    Svc.RemoveService(id, type, date);
                    MessageBox.Show("Xóa thành công!"); LoadD();
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            }
        }
    }
}