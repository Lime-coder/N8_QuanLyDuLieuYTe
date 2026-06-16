using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyYTe.Services;

namespace QuanLyYTe.Forms.DBA
{
    public class frmAuditManagement : Form
    {
        private readonly SecurityAdminService _service = new SecurityAdminService();
        private DataTable _dtSource;

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblSearch;
        private TextBox txtSearch;
        private Button btnSearch;
        private ComboBox cboAuditType;
        private Button btnRefresh;
        private DataGridView dgvAudit;
        private Button btnExport;

        public frmAuditManagement()
        {
            InitManualUI();
            LoadData();
        }

        private void InitManualUI()
        {
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.Size = new Size(1150, 700);

            pnlHeader = new Panel { Dock = DockStyle.Top, Height = 120, BackColor = Color.White };

            lblTitle = new Label
            {
                Text = "NHẬT KÝ KIỂM TOÁN (AUDITING)",
                ForeColor = Color.FromArgb(255, 140, 40),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 22),
                AutoSize = true
            };

            lblSearch = new Label
            {
                Text = "Tìm:",
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Location = new Point(20, 75),
                AutoSize = true
            };

            txtSearch = new TextBox
            {
                Width = 300,
                Location = new Point(65, 73),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle
            };

            btnSearch = new Button
            {
                Text = "Tìm",
                Size = new Size(80, 32),
                Location = new Point(375, 73),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += (s, e) => ApplySearch();

            cboAuditType = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 250,
                Location = new Point(480, 73),
                Font = new Font("Segoe UI", 11)
            };
            cboAuditType.Items.AddRange(new object[] {
                "Kiểm toán hệ thống",
                "Cập nhật Đơn thuốc",
                "Cập nhật thông tin HSBA",
                "Thay đổi thông tin Dịch vụ"
            });
            cboAuditType.SelectedIndex = 0;
            cboAuditType.SelectedIndexChanged += (s, e) => LoadData();

            btnRefresh = new Button
            {
                Text = "Làm mới",
                Size = new Size(100, 35),
                Location = new Point(750, 71),
                BackColor = Color.FromArgb(255, 140, 40),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadData();

            dgvAudit = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                EnableHeadersVisualStyles = false,
                GridColor = Color.Black,
            };

            dgvAudit.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvAudit.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvAudit.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvAudit.ColumnHeadersHeight = 40;

            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblSearch, txtSearch, btnSearch, cboAuditType, btnRefresh });
            this.Controls.AddRange(new Control[] { dgvAudit, pnlHeader });

            btnExport = new Button
            {
                Text = "Xuất CSV",
                Size = new Size(100, 35),
                Location = new Point(860, 71),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += (s, e) => ExportToCSV();

            pnlHeader.Controls.Add(btnExport);
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
                    case "STATUS": col.HeaderText = "Trạng thái"; break;
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
    }
}