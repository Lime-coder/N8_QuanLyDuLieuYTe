using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyYTe.Services;

namespace QuanLyYTe.Forms.DBA
{
    public partial class frmAuditManagement : Form
    {
        private readonly SecurityAdminService _service = new SecurityAdminService();
        private DataTable _dtSource;

        public frmAuditManagement()
        {
            InitializeComponent();
            cboAuditType.SelectedIndex = 0;
            LoadData();
        }

        private void LoadData()
        {
            int type = cboAuditType.SelectedIndex;
            if (type == 0) _dtSource = _service.GetStandardAudit();
            else if (type == 1) _dtSource = _service.GetPrescriptionAudit();
            else if (type == 2) _dtSource = _service.GetMedicalInfoAudit();
            else if (type == 3) _dtSource = _service.GetServiceRecordAudit();

            dgvAudit.DataSource = _dtSource.DefaultView;

            txtSearch.Clear();
            TranslateHeaders();
        }

        private void ApplySearch()
        {
            if (_dtSource == null) return;

            // Lấy từ khóa và xử lý ký tự đặc biệt
            string filterValue = txtSearch.Text.Trim().Replace("'", "''");

            if (string.IsNullOrEmpty(filterValue))
            {
                _dtSource.DefaultView.RowFilter = "";
                return;
            }

            // Xây dựng chuỗi lọc: chỉ tìm trên các cột kiểu chuỗi để tránh lỗi kiểu dữ liệu
            string filterExpr = "";
            foreach (DataColumn col in _dtSource.Columns)
            {
                // Chỉ lọc trên các cột chứa văn bản (string)
                if (col.DataType == typeof(string))
                {
                    if (filterExpr.Length > 0) filterExpr += " OR ";
                    filterExpr += string.Format("[{0}] LIKE '%{1}%'", col.ColumnName, filterValue);
                }
            }

            // Ép Grid cập nhật lại theo bộ lọc
            _dtSource.DefaultView.RowFilter = filterExpr;
        }

        private void TranslateHeaders()
        {
            foreach (DataGridViewColumn col in dgvAudit.Columns)
            {
                switch (col.Name.ToUpper())
                {
                    case "USERNAME": col.HeaderText = "Người dùng"; break;
                    case "TIMESTAMP": col.HeaderText = "Thời điểm"; break;
                    case "OBJECT": col.HeaderText = "Đối tượng"; break;
                    case "ACTION": col.HeaderText = "Hành động"; break;
                    case "RETURNCODE": col.HeaderText = "Mã trả về"; break;
                    case "SQL_TEXT": col.HeaderText = "Câu lệnh SQL"; break;
                    case "OBJECT_NAME": col.HeaderText = "Tên bảng"; break;
                    case "POLICY_NAME": col.HeaderText = "Tên chính sách"; break;
                    case "STATEMENT_TYPE": col.HeaderText = "Loại lệnh"; break;
                }
            }
        }

        private void ExportToCSV()
        {
            if (dgvAudit.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = "NhatKyKiemToan_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    string[] columnNames = new string[dgvAudit.Columns.Count];

                    for (int i = 0; i < dgvAudit.Columns.Count; i++)
                        columnNames[i] = dgvAudit.Columns[i].HeaderText;
                    sb.AppendLine(string.Join(",", columnNames));

                    foreach (DataGridViewRow row in dgvAudit.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            string[] cells = new string[dgvAudit.Columns.Count];
                            for (int i = 0; i < dgvAudit.Columns.Count; i++)
                                cells[i] = row.Cells[i].Value?.ToString().Replace(",", " "); // Tránh lỗi dấu phẩy trong CSV
                            sb.AppendLine(string.Join(",", cells));
                        }
                    }

                    System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), System.Text.Encoding.UTF8);
                    MessageBox.Show("Xuất dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi");
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e) => ApplySearch();
        private void cboAuditType_SelectedIndexChanged(object sender, EventArgs e) => LoadData();
        private void btnRefresh_Click(object sender, EventArgs e) => LoadData();
        private void btnExport_Click(object sender, EventArgs e) => ExportToCSV();
    }
}