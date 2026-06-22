using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.Helpers;

namespace QuanLyYTe.Forms.BackupRecovery
{
    public partial class frmBackupRecovery : Form
    {
        private readonly Services.BackupRecoveryService _service = new Services.BackupRecoveryService();

        public frmBackupRecovery()
        {
            InitializeComponent();
            ApplyButtonStyles();
        }

        private void frmBackupRecovery_Load(object sender, EventArgs e)
        {
            cboInterval.SelectedIndex = 0; // Default to 1 minute
            LoadCurrentData();
            LoadBackupHistory();
            LoadAuditRecoveryPoints();
            UpdateSchedulerStatusUI();
        }
        // Áp dụng các kiểu nút hiện đại tương tự các Form quản trị khác trong hệ thống
        private void ApplyButtonStyles()
        {
            void StylePrimary(Button b)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.BackColor = Color.FromArgb(255, 140, 40); // Orange Accent
                b.ForeColor = Color.White;
                b.Cursor = Cursors.Hand;
            }

            void StyleDanger(Button b)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.BackColor = Color.FromArgb(244, 67, 54); // Red Accent
                b.ForeColor = Color.White;
                b.Cursor = Cursors.Hand;
            }

            void StyleNeutral(Button b)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.BackColor = Color.FromArgb(96, 125, 139); // Gray Accent
                b.ForeColor = Color.White;
                b.Cursor = Cursors.Hand;
            }

            void StyleBlue(Button b)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.BackColor = Color.FromArgb(33, 150, 243); // Blue Accent
                b.ForeColor = Color.White;
                b.Cursor = Cursors.Hand;
            }

            void StyleGreen(Button b)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.BackColor = Color.FromArgb(76, 175, 80); // Green Accent
                b.ForeColor = Color.White;
                b.Cursor = Cursors.Hand;
            }

            StyleNeutral(btnRefresh);
            StyleNeutral(btnRefreshBackup);
            StyleBlue(btnBackupNow);
            StyleGreen(btnEnableAutoBackup);
            StyleDanger(btnDisableAutoBackup);
            StyleDanger(btnSimulateDelete);
            StyleDanger(btnSimulateWrongUpdate);
            StyleNeutral(btnLoadAudit);
            StylePrimary(btnBackupRestore);
        }

        private void btnRefreshBackup_Click(object sender, EventArgs e)
        {
            LoadBackupHistory();
            LoadAuditRecoveryPoints();
            UpdateSchedulerStatusUI();
        }
        // Tải dữ liệu hiện tại của bảng PRESCRIPTION
        private void LoadCurrentData()
        {
            try
            {
                DataTable dt = _service.GetCurrentData();
                dgvDonThuoc.DataSource = dt;
                lblTotalRows.Text = $"Tổng số dòng: {dt.Rows.Count}";
                GridViewStyler.Format(dgvDonThuoc);

                if (dgvDonThuoc.Columns.Contains("RECORD_ID")) dgvDonThuoc.Columns["RECORD_ID"].HeaderText = "Mã HSBA";
                if (dgvDonThuoc.Columns.Contains("PRESCRIPTION_DATE")) dgvDonThuoc.Columns["PRESCRIPTION_DATE"].HeaderText = "Ngày kê toa";
                if (dgvDonThuoc.Columns.Contains("MEDICINE_NAME")) dgvDonThuoc.Columns["MEDICINE_NAME"].HeaderText = "Tên thuốc";
                if (dgvDonThuoc.Columns.Contains("DOSAGE")) dgvDonThuoc.Columns["DOSAGE"].HeaderText = "Liều dùng";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu bảng PRESCRIPTION: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Tải lịch sử các phiên bản sao lưu từ BACKUP_HISTORY
        private void LoadBackupHistory()
        {
            try
            {
                DataTable dt = _service.GetBackupHistory();
                dgvBackupHistory.DataSource = dt;
                GridViewStyler.Format(dgvBackupHistory);

                if (dgvBackupHistory.Columns.Contains("BACKUP_ID")) dgvBackupHistory.Columns["BACKUP_ID"].HeaderText = "ID";
                if (dgvBackupHistory.Columns.Contains("BACKUP_TIME")) dgvBackupHistory.Columns["BACKUP_TIME"].HeaderText = "Thời gian";
                if (dgvBackupHistory.Columns.Contains("BACKUP_TYPE")) dgvBackupHistory.Columns["BACKUP_TYPE"].HeaderText = "Loại sao lưu";
                if (dgvBackupHistory.Columns.Contains("STATUS")) dgvBackupHistory.Columns["STATUS"].HeaderText = "Trạng thái";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch sử sao lưu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Tải danh sách các thời điểm kiểm toán khả dụng cho ComboBox phục hồi
        private void LoadAuditRecoveryPoints()
        {
            try
            {
                DataTable dt = _service.GetAuditRecoveryPoints();
                
                cboBackupVersion.DataSource = null;
                cboBackupVersion.Items.Clear();

                if (dt.Rows.Count > 0)
                {
                    var points = new List<KeyValuePair<DateTime, string>>();
                    foreach (DataRow row in dt.Rows)
                    {
                        DateTime time = Convert.ToDateTime(row["AUDIT_TIME"]);
                        string action = row["ACTION_NAME"].ToString();
                        points.Add(new KeyValuePair<DateTime, string>(time, $"{action} lúc {time:dd/MM/yyyy HH:mm:ss}"));
                    }
                    cboBackupVersion.DataSource = points;
                    cboBackupVersion.DisplayMember = "Value";
                    cboBackupVersion.ValueMember = "Key";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi tải danh sách điểm khôi phục: " + ex.Message);
            }
        }
        // Cập nhật nhãn trạng thái của Oracle Job (DBMS_SCHEDULER)
        private void UpdateSchedulerStatusUI()
        {
            try
            {
                DataTable dt = _service.GetJobState();
                lblSchedulerStatus.Visible = true;
                if (dt.Rows.Count > 0)
                {
                    string state = dt.Rows[0]["STATE"].ToString();
                    if (state == "SCHEDULED" || state == "RUNNING" || state == "RETRY RUNNING")
                    {
                        lblSchedulerStatus.Text = "Trạng thái Backup Auto: 🟢 Đang bật";
                        lblSchedulerStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblSchedulerStatus.Text = "Trạng thái Backup Auto: 🔴 Đã tắt";
                        lblSchedulerStatus.ForeColor = Color.Red;
                    }
                }
                else
                {
                    lblSchedulerStatus.Text = "Trạng thái Backup Auto: 🔴 Đã tắt";
                    lblSchedulerStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblSchedulerStatus.Text = "Lỗi trạng thái: " + ex.Message;
                lblSchedulerStatus.ForeColor = Color.OrangeRed;
            }
        }
        // Nạp nhật ký kiểm toán (Audit Log) từ Service
        private void LoadAuditLogs()
        {
            try
            {
                DataTable dt = _service.GetAuditLogs();
                dgvAudit.DataSource = dt;
                GridViewStyler.Format(dgvAudit);

                if (dgvAudit.Columns.Contains("USERNAME")) dgvAudit.Columns["USERNAME"].HeaderText = "Người dùng";
                if (dgvAudit.Columns.Contains("TIMESTAMP")) dgvAudit.Columns["TIMESTAMP"].HeaderText = "Thời điểm";
                if (dgvAudit.Columns.Contains("ACTION")) dgvAudit.Columns["ACTION"].HeaderText = "Hành động";
                if (dgvAudit.Columns.Contains("OBJECT")) dgvAudit.Columns["OBJECT"].HeaderText = "Đối tượng";
                if (dgvAudit.Columns.Contains("SQL_TEXT")) dgvAudit.Columns["SQL_TEXT"].HeaderText = "Câu lệnh SQL";
                if (dgvAudit.Columns.Contains("STATUS")) dgvAudit.Columns["STATUS"].HeaderText = "Trạng thái";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể nạp nhật ký kiểm toán. Vui lòng kiểm tra lại quyền truy cập hoặc cấu hình Audit của hệ thống.\nChi tiết: " + ex.Message, "Lỗi tải Nhật ký", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        // Thực hiện sao lưu thủ công thông qua Service
        private void ManualBackup()
        {
            try
            {
                btnBackupNow.Enabled = false;
                btnBackupNow.Text = "Đang chạy ngầm...";

                _ = System.Threading.Tasks.Task.Run(() =>
                {
                    try
                    {
                        _service.ManualBackup();
                        this.Invoke((MethodInvoker)delegate
                        {
                            LoadBackupHistory();
                            LoadAuditRecoveryPoints();
                            btnBackupNow.Enabled = true;
                            btnBackupNow.Text = "Sao lưu chủ động";
                        });
                    }
                    catch (Exception ex)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            MessageBox.Show("Sao lưu thủ công thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            btnBackupNow.Enabled = true;
                            btnBackupNow.Text = "Sao lưu chủ động";
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnBackupNow.Enabled = true;
                btnBackupNow.Text = "Sao lưu chủ động";
            }
        }
        // Kích hoạt sao lưu tự động dùng DBMS_SCHEDULER thông qua Service
        private void EnableAutoBackup()
        {
            if (cboInterval.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn chu kỳ để kích hoạt.", "Yêu cầu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string intervalSel = cboInterval.SelectedItem.ToString();

            try
            {
                _service.EnableAutoBackup(intervalSel);
                MessageBox.Show($"Kích hoạt sao lưu tự động thành công! Chu kỳ: {intervalSel}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                UpdateSchedulerStatusUI();
                LoadBackupHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kích hoạt sao lưu tự động thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Tắt sao lưu tự động thông qua Service
        private void DisableAutoBackup()
        {
            try
            {
                _service.DisableAutoBackup();
                MessageBox.Show("Đã dừng sao lưu tự động (Job disabled).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                UpdateSchedulerStatusUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tắt sao lưu tự động thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Giả lập sự cố: Xóa bản ghi BA000001
        private void SimulateDelete()
        {
            try
            {
                int rows = _service.SimulateDelete();
                MessageBox.Show($"Đã giả lập sự cố: Xóa thành công {rows} bản ghi của BA000001 khỏi bảng PRESCRIPTION.", "Giả lập sự cố", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadCurrentData();
                LoadAuditRecoveryPoints();
                LoadAuditLogs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi giả lập sự cố xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Giả lập sự cố: Cập nhật sai thông tin bản ghi BA000001
        private void SimulateWrongUpdate()
        {
            try
            {
                int rows = _service.SimulateWrongUpdate();
                MessageBox.Show($"Đã giả lập sự cố: Cập nhật sai thông tin của {rows} bản ghi BA000001 thành công.", "Giả lập sự cố", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadCurrentData();
                LoadAuditRecoveryPoints();
                LoadAuditLogs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi giả lập sự cố cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Thực hiện khôi phục dữ liệu cho bản ghi BA000001
        private void BackupRestore()
        {
            if (cboBackupVersion.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn một thời điểm kiểm toán hợp lệ để khôi phục.", "Yêu cầu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime auditTime = (DateTime)cboBackupVersion.SelectedValue;

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn khôi phục dữ liệu BA000001 về trạng thái ngay trước thời điểm {auditTime:HH:mm:ss dd/MM/yyyy}?", "Khôi phục dữ liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _service.BackupRestoreByAudit(auditTime);
                MessageBox.Show($"Khôi phục dữ liệu thành công cho BA000001 bằng Flashback Query!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCurrentData();
                LoadAuditRecoveryPoints();
                LoadAuditLogs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Khôi phục dữ liệu thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Event Handlers ---
        private void btnRefresh_Click(object sender, EventArgs e) => LoadCurrentData();
        private void btnBackupNow_Click(object sender, EventArgs e) => ManualBackup();
        private void btnEnableAutoBackup_Click(object sender, EventArgs e) => EnableAutoBackup();
        private void btnDisableAutoBackup_Click(object sender, EventArgs e) => DisableAutoBackup();
        private void btnSimulateDelete_Click(object sender, EventArgs e) => SimulateDelete();
        private void btnSimulateWrongUpdate_Click(object sender, EventArgs e) => SimulateWrongUpdate();
        private void btnLoadAudit_Click(object sender, EventArgs e) => LoadAuditLogs();
        private void btnBackupRestore_Click(object sender, EventArgs e) => BackupRestore();
    }
}
