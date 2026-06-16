using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System;
using System.Globalization;

namespace QuanLyYTe.Forms.Doctor
{
    public class frmMedicalRecordManagement : frmDoctorBase
    {
        private DataGridView dgvSub = new DataGridView { 
            Dock = DockStyle.Fill, 
            BackgroundColor = Color.White, 
            BorderStyle = BorderStyle.None, 
            AllowUserToAddRows = false, 
            ReadOnly = true, 
            SelectionMode = DataGridViewSelectionMode.FullRowSelect 
        };

        private Label lblSubTitle = new Label { 
            Text = "▶ CHI TIẾT DỊCH VỤ CẬN LÂM SÀNG", 
            Dock = DockStyle.Left, 
            Width = 300, 
            Font = new Font("Segoe UI", 9, FontStyle.Bold), 
            ForeColor = Color.FromArgb(0, 120, 215), 
            TextAlign = ContentAlignment.MiddleLeft, 
            Padding = new Padding(10, 0, 0, 0) 
        };

        public frmMedicalRecordManagement()
        {
            this.Controls.Remove(Dgv);
            SplitContainer split = new SplitContainer { 
                Dock = DockStyle.Fill, 
                Orientation = Orientation.Horizontal, 
                SplitterDistance = 450, 
                SplitterWidth = 5, 
                BackColor = Color.FromArgb(200, 200, 200) 
            };
            split.Panel1.Controls.Add(Dgv);

            Panel pnlSubAction = new Panel { 
                Dock = DockStyle.Top, 
                Height = 40, 
                BackColor = Color.FromArgb(235, 235, 235) 
            };

            Button btnSubAdd = CreateBtn("Thêm", Color.DodgerBlue, 310); 
            btnSubAdd.Size = new Size(80, 30); 
            btnSubAdd.Location = new Point(310, 5);

            Button btnSubDel = CreateBtn("Xóa", Color.Crimson, 400); 
            btnSubDel.Size = new Size(80, 30); 
            btnSubDel.Location = new Point(400, 5);

            btnSubAdd.Click += (s, e) => AddSubService();
            btnSubDel.Click += (s, e) => DeleteSubService();

            pnlSubAction.Controls.AddRange(new Control[] { lblSubTitle, btnSubAdd, btnSubDel });
            split.Panel2.Controls.AddRange(new Control[] { dgvSub, pnlSubAction });

            this.Controls.Add(split);
            split.BringToFront();

            LoadD();
            btnA.Visible = btnD.Visible = false;
            Dgv.SelectionChanged += (s, e) => LoadSubData();
        }

        private void LoadSubData()
        {
            if (Dgv.CurrentRow == null) { dgvSub.DataSource = null; return; }
            string rid = Dgv.CurrentRow.Cells["RECORD_ID"].Value.ToString();
            dgvSub.DataSource = Svc.GetServices(rid);
            if (dgvSub.Columns.Contains("RECORD_ID")) dgvSub.Columns["RECORD_ID"].Visible = false;
            if (dgvSub.Columns.Contains("SERVICE_TYPE")) dgvSub.Columns["SERVICE_TYPE"].HeaderText = "Loại dịch vụ";
            if (dgvSub.Columns.Contains("SERVICE_DATE")) dgvSub.Columns["SERVICE_DATE"].HeaderText = "Ngày thực hiện";
            if (dgvSub.Columns.Contains("TECHNICIAN_ID")) dgvSub.Columns["TECHNICIAN_ID"].HeaderText = "Mã KTV";
            if (dgvSub.Columns.Contains("SERVICE_RESULT")) dgvSub.Columns["SERVICE_RESULT"].HeaderText = "Kết quả";
            Helpers.GridViewStyler.Format(dgvSub);
        }

        private void AddSubService()
        {
            if (Dgv.CurrentRow == null) return;
            string rid = Dgv.CurrentRow.Cells["RECORD_ID"].Value.ToString().Trim();
            var f = new Dictionary<string, string> { { "Mã HSBA (Không sửa)", rid }, { "Loại dịch vụ", "" } };
            ShowDialog("Thêm dịch vụ mới", f, res => {
                Svc.CreateService(rid, res["Loại dịch vụ"]);
                LoadSubData();
            });
        }

        private void DeleteSubService()
        {
            if (dgvSub.CurrentRow == null) return;

            string rid = dgvSub.CurrentRow.Cells["RECORD_ID"].Value.ToString().Trim();
            string type = dgvSub.CurrentRow.Cells["SERVICE_TYPE"].Value.ToString();
            DateTime dt = DateTime.ParseExact(dgvSub.CurrentRow.Cells["SERVICE_DATE"].Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (MessageBox.Show($"Xác nhận xóa dịch vụ '{type}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Svc.RemoveService(rid, type, dt);
                    LoadSubData();
                    MessageBox.Show("Xóa dịch vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        protected override void LoadD()
        {
            Dgv.DataSource = Svc.GetMedicalRecords(TxtS.Text);
            if (Dgv.Columns.Contains("RECORD_ID")) Dgv.Columns["RECORD_ID"].HeaderText = "Mã HSBA";
            if (Dgv.Columns.Contains("PATIENT_ID")) Dgv.Columns["PATIENT_ID"].HeaderText = "Mã Bệnh nhân";
            if (Dgv.Columns.Contains("FULL_NAME")) Dgv.Columns["FULL_NAME"].HeaderText = "Tên Bệnh nhân";
            if (Dgv.Columns.Contains("RECORD_DATE")) Dgv.Columns["RECORD_DATE"].HeaderText = "Ngày lập";
            if (Dgv.Columns.Contains("DIAGNOSIS")) Dgv.Columns["DIAGNOSIS"].HeaderText = "Chẩn đoán";
            if (Dgv.Columns.Contains("TREATMENT_PLAN")) Dgv.Columns["TREATMENT_PLAN"].HeaderText = "Điều trị";
            if (Dgv.Columns.Contains("CONCLUSION")) Dgv.Columns["CONCLUSION"].HeaderText = "Kết luận";
            if (Dgv.Columns.Contains("DOCTOR_ID")) Dgv.Columns["DOCTOR_ID"].Visible = false;
            if (Dgv.Columns.Contains("DEPT_ID")) Dgv.Columns["DEPT_ID"].Visible = false;
            Helpers.GridViewStyler.Format(Dgv);
        }

        protected override void FormE()
        {
            if (Dgv.CurrentRow == null) return;
            var r = Dgv.CurrentRow;
            string rid = r.Cells["RECORD_ID"].Value.ToString().Trim();
            var f = new Dictionary<string, string> { 
                { "Mã HSBA (Không sửa)", rid }, 
                { "Chẩn đoán", r.Cells["DIAGNOSIS"].Value.ToString() }, 
                { "Điều trị", r.Cells["TREATMENT_PLAN"].Value.ToString() }, 
                { "Kết luận", r.Cells["CONCLUSION"].Value.ToString() } 
            };
            ShowDialog("Cập nhật bệnh án", f, res => Svc.SaveMedicalRecord(rid, res["Chẩn đoán"], res["Điều trị"], res["Kết luận"]));
        }
    }
}
