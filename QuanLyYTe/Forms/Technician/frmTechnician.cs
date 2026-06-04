using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyYTe.Services;

namespace QuanLyYTe.Forms.Technician
{
    public partial class frmTechnician : Form
    {
        private readonly TechnicianService _service = new TechnicianService();
        public frmTechnician()
        {
            InitializeComponent();
            this.Load += FrmTechnician_Load;
        }

        private void FrmTechnician_Load(object? sender, EventArgs e)
        {
            // Let grid auto-generate columns from the DataTable returned by the SP
            dgvServices.Columns.Clear();
            dgvServices.AutoGenerateColumns = true;
            LoadServices();
        }

        private void btnNewService_Click(object sender, EventArgs e)
        {
            if (dgvServices.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng để cập nhật kết quả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = dgvServices.SelectedRows[0];
            object idObj = row.Cells["record_id"].Value;
            object typeObj = row.Cells["service_type"].Value;
            object dateObj = row.Cells["service_date"].Value;

            string? recordId = idObj?.ToString();
            string? serviceType = typeObj?.ToString();

            if (string.IsNullOrEmpty(recordId) || string.IsNullOrEmpty(serviceType))
            {
                MessageBox.Show("Dữ liệu dịch vụ không hợp lệ. Vui lòng chọn một dòng hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime serviceDate;
            if (dateObj == null || dateObj == DBNull.Value)
            {
                MessageBox.Show("Ngày dịch vụ bị thiếu. Không thể cập nhật.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dateObj is DateTime dt)
            {
                serviceDate = dt;
            }
            else if (!DateTime.TryParse(dateObj.ToString(), out serviceDate))
            {
                MessageBox.Show("Không thể chuyển đổi trường ngày dịch vụ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string? newResult = Prompt.ShowDialog("Nhập kết quả dịch vụ:", "Cập nhật kết quả");
            if (string.IsNullOrEmpty(newResult)) return;

            try
            {
                _service.SaveServiceResult(recordId, serviceType, serviceDate, newResult);
                MessageBox.Show("Cập nhật kết quả thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadServices();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadServices();
        }

        private void LoadServices()
        {
            try
            {
                DataTable dt = _service.LoadAssignedServices();
                dgvServices.AutoGenerateColumns = true;
                dgvServices.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Simple input prompt
        private static class Prompt
        {
            public static string? ShowDialog(string text, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 500,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterParent
                };
                Label textLabel = new Label() { Left = 10, Top = 10, Text = text, AutoSize = true };
                TextBox inputBox = new TextBox() { Left = 10, Top = 40, Width = 460 };
                Button confirmation = new Button() { Text = "OK", Left = 380, Width = 90, Top = 70, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(inputBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? inputBox.Text : null;
            }
        }
    }
}
