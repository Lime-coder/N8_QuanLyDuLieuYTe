using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.Common;
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

        private void ShowOperationError(string operation, Exception ex, MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            Logger.LogError(ex, $"BackupRecovery.{operation}");

            MessageBox.Show(
                $"{operation} that bai: {ex.Message}\n\n" +
                $"App log: {Logger.LogFilePath}\n" +
                "Data Pump logs: C:\\OracleBackups",
                "Loi",
                MessageBoxButtons.OK,
                icon);
        }

        private void OpenErrorLogFolder()
        {
            try
            {
                Directory.CreateDirectory(Logger.LogDirectoryPath);
                Process.Start(new ProcessStartInfo
                {
                    FileName = Logger.LogDirectoryPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                ShowOperationError("Mo thu muc log", ex);
            }
        }

        private void frmBackupRecovery_Load(object sender, EventArgs e)
        {
            cboInterval.SelectedIndex = 0; // Default to 1 minute
            LoadStandardAuditLogs();
            LoadBackupHistory();
            LoadAuditRecoveryPoints();
            UpdateSchedulerStatusUI();
            LoadTableData();
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
            StyleNeutral(btnOpenErrorLog);
            StyleBlue(btnBackupNow);
            StyleGreen(btnEnableAutoBackup);
            StyleDanger(btnDisableAutoBackup);

            StyleNeutral(btnLoadAudit);
            StylePrimary(btnBackupRestore);
            StyleBlue(btnImportDataPump);
        }

        private void btnRefreshBackup_Click(object sender, EventArgs e)
        {
            LoadBackupHistory();
            LoadAuditRecoveryPoints();
            UpdateSchedulerStatusUI();
        }
        // Tải nhật ký kiểm toán hệ thống (Standard Audit)
        private void LoadStandardAuditLogs()
        {
            try
            {
                DataTable dt = _service.GetStandardAuditLogs();
                dgvStandardAudit.DataSource = dt;
                lblTotalRows.Text = $"Tổng số dòng: {dt.Rows.Count}";
                GridViewStyler.Format(dgvStandardAudit);

                if (dgvStandardAudit.Columns.Contains("USERNAME")) dgvStandardAudit.Columns["USERNAME"].HeaderText = "Người dùng";
                if (dgvStandardAudit.Columns.Contains("OBJECT")) dgvStandardAudit.Columns["OBJECT"].HeaderText = "Đối tượng";
                if (dgvStandardAudit.Columns.Contains("ACTION")) dgvStandardAudit.Columns["ACTION"].HeaderText = "Hành động";
                if (dgvStandardAudit.Columns.Contains("TIMESTAMP")) dgvStandardAudit.Columns["TIMESTAMP"].HeaderText = "Thời gian ghi nhận";
            }
            catch (Exception ex)
            {
                ShowOperationError("Tai nhat ky Standard Audit", ex);
            }
        }

        private void LoadTableData()
        {
            try
            {
                dgvPrescription.DataSource = _service.GetPrescriptions();
                GridViewStyler.Format(dgvPrescription);
                if (dgvPrescription.Columns.Contains("RECORD_ID")) dgvPrescription.Columns["RECORD_ID"].HeaderText = "Mã bệnh án";
                if (dgvPrescription.Columns.Contains("PRESCRIPTION_DATE")) dgvPrescription.Columns["PRESCRIPTION_DATE"].HeaderText = "Ngày kê đơn";
                if (dgvPrescription.Columns.Contains("MEDICINE_NAME")) dgvPrescription.Columns["MEDICINE_NAME"].HeaderText = "Tên thuốc";
                if (dgvPrescription.Columns.Contains("DOSAGE")) dgvPrescription.Columns["DOSAGE"].HeaderText = "Liều lượng";

                dgvMedicalRecord.DataSource = _service.GetMedicalRecords();
                GridViewStyler.Format(dgvMedicalRecord);
                dgvMedicalRecord.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                if (dgvMedicalRecord.Columns.Contains("RECORD_ID")) dgvMedicalRecord.Columns["RECORD_ID"].HeaderText = "Mã bệnh án";
                if (dgvMedicalRecord.Columns.Contains("PATIENT_ID")) dgvMedicalRecord.Columns["PATIENT_ID"].HeaderText = "Mã BN";
                if (dgvMedicalRecord.Columns.Contains("RECORD_DATE")) dgvMedicalRecord.Columns["RECORD_DATE"].HeaderText = "Ngày khám";
                if (dgvMedicalRecord.Columns.Contains("DIAGNOSIS")) dgvMedicalRecord.Columns["DIAGNOSIS"].HeaderText = "Chẩn đoán";
                if (dgvMedicalRecord.Columns.Contains("TREATMENT_PLAN")) dgvMedicalRecord.Columns["TREATMENT_PLAN"].HeaderText = "Hướng điều trị";
                if (dgvMedicalRecord.Columns.Contains("DOCTOR_ID")) dgvMedicalRecord.Columns["DOCTOR_ID"].HeaderText = "Mã BS";
                if (dgvMedicalRecord.Columns.Contains("DEPT_ID")) dgvMedicalRecord.Columns["DEPT_ID"].HeaderText = "Mã khoa";
                if (dgvMedicalRecord.Columns.Contains("CONCLUSION")) dgvMedicalRecord.Columns["CONCLUSION"].HeaderText = "Kết luận";

                dgvServiceRecord.DataSource = _service.GetServiceRecords();
                GridViewStyler.Format(dgvServiceRecord);
                dgvServiceRecord.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                if (dgvServiceRecord.Columns.Contains("RECORD_ID")) dgvServiceRecord.Columns["RECORD_ID"].HeaderText = "Mã bệnh án";
                if (dgvServiceRecord.Columns.Contains("SERVICE_TYPE")) dgvServiceRecord.Columns["SERVICE_TYPE"].HeaderText = "Tên dịch vụ";
                if (dgvServiceRecord.Columns.Contains("SERVICE_DATE")) dgvServiceRecord.Columns["SERVICE_DATE"].HeaderText = "Ngày thực hiện";
                if (dgvServiceRecord.Columns.Contains("TECHNICIAN_ID")) dgvServiceRecord.Columns["TECHNICIAN_ID"].HeaderText = "Mã KTV";
                if (dgvServiceRecord.Columns.Contains("SERVICE_RESULT")) dgvServiceRecord.Columns["SERVICE_RESULT"].HeaderText = "Kết quả";
            }
            catch (Exception ex)
            {
                ShowOperationError("Tai du lieu cac bang", ex);
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
                ShowOperationError("Tai lich su sao luu", ex);
            }
        }

        private void dgvBackupHistory_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvBackupHistory.CurrentRow == null ||
                !dgvBackupHistory.Columns.Contains("DUMP_FILE"))
            {
                return;
            }

            object value = dgvBackupHistory.CurrentRow.Cells["DUMP_FILE"].Value;
            if (value != null && value != DBNull.Value)
            {
                txtImportDumpFile.Text = value.ToString();
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
                        DateTime time = DateTime.ParseExact(row["AUDIT_TIME_STR"].ToString(), "yyyy-MM-dd HH:mm:ss", null);
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
                Logger.LogError(ex, "BackupRecovery.LoadAuditRecoveryPoints");
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
                Logger.LogError(ex, "BackupRecovery.UpdateSchedulerStatusUI");
                lblSchedulerStatus.Text = "Lỗi trạng thái: " + ex.Message;
                lblSchedulerStatus.ForeColor = Color.OrangeRed;
            }
        }
        // Nạp nhật ký kiểm toán chi tiết (Fine-Grained Audit)
        private void LoadFgaAuditLogs()
        {
            try
            {
                DataTable dt = _service.GetFgaAuditLogs();
                dgvFgaAudit.DataSource = dt;
                GridViewStyler.Format(dgvFgaAudit);

                if (dgvFgaAudit.Columns.Contains("USERNAME")) dgvFgaAudit.Columns["USERNAME"].HeaderText = "Người dùng";
                if (dgvFgaAudit.Columns.Contains("TIMESTAMP")) dgvFgaAudit.Columns["TIMESTAMP"].HeaderText = "Thời điểm";
                if (dgvFgaAudit.Columns.Contains("ACTION")) dgvFgaAudit.Columns["ACTION"].HeaderText = "Hành động";
                if (dgvFgaAudit.Columns.Contains("OBJECT")) dgvFgaAudit.Columns["OBJECT"].HeaderText = "Đối tượng";
                if (dgvFgaAudit.Columns.Contains("SQL_TEXT")) dgvFgaAudit.Columns["SQL_TEXT"].HeaderText = "Câu lệnh SQL";
            }
            catch (Exception ex)
            {
                ShowOperationError("Tai nhat ky kiem toan FGA", ex, MessageBoxIcon.Warning);
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
                            ShowOperationError("Sao luu thu cong", ex);
                            btnBackupNow.Enabled = true;
                            btnBackupNow.Text = "Sao lưu chủ động";
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                ShowOperationError("Khoi dong sao luu thu cong", ex);
                btnBackupNow.Enabled = true;
                btnBackupNow.Text = "Sao lưu chủ động";
            }
        }
        // Kích hoạt sao lưu tự động dùng DBMS_SCHEDULER thông qua Service
        private async void EnableAutoBackup()
        {
            if (cboInterval.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn chu kỳ để kích hoạt.", "Yêu cầu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string intervalSel = cboInterval.SelectedItem.ToString();
            btnEnableAutoBackup.Enabled = false;
            btnEnableAutoBackup.Text = "Đang kích hoạt...";

            try
            {
                DateTime startTime = DateTime.Now;
                await System.Threading.Tasks.Task.Run(() => _service.EnableAutoBackup(intervalSel));
                MessageBox.Show($"Kích hoạt sao lưu tự động thành công! Chu kỳ: {intervalSel}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                UpdateSchedulerStatusUI();
                LoadBackupHistory();

                // Bắt đầu kiểm tra ngầm để tự động tải lại bảng lịch sử khi tiến trình sao lưu chạy xong
                StartBackupPolling(startTime);
            }
            catch (Exception ex)
            {
                ShowOperationError("Bat sao luu tu dong", ex);
            }
            finally
            {
                btnEnableAutoBackup.Enabled = true;
                btnEnableAutoBackup.Text = "Bật sao lưu Tự động";
            }
        }

        private async void StartBackupPolling(DateTime startTime)
        {
            // Kiểm tra mỗi 3 giây, tối đa 10 lần (30 giây)
            for (int i = 0; i < 10; i++)
            {
                await System.Threading.Tasks.Task.Delay(3000);

                if (this.IsDisposed) return;

                LoadBackupHistory();
                LoadAuditRecoveryPoints();

                if (HasNewAutoBackup(startTime))
                {
                    break;
                }
            }
        }

        private bool HasNewAutoBackup(DateTime startTime)
        {
            if (dgvBackupHistory.DataSource is DataTable dt)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["BACKUP_TYPE"]?.ToString() == "AUTO" && 
                        row["BACKUP_TIME"] != DBNull.Value)
                    {
                        DateTime backupTime = Convert.ToDateTime(row["BACKUP_TIME"]);
                        if (backupTime >= startTime.AddSeconds(-5))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        // Tắt sao lưu tự động thông qua Service
        private async void DisableAutoBackup()
        {
            btnDisableAutoBackup.Enabled = false;
            btnDisableAutoBackup.Text = "Đang tắt...";
            lblSchedulerStatus.Text = "Trạng thái Backup Auto: 🟡 Đang chờ tắt...";
            lblSchedulerStatus.ForeColor = Color.Orange;

            try
            {
                await System.Threading.Tasks.Task.Run(async () =>
                {
                    while (true)
                    {
                        if (this.IsDisposed) return;
                        try
                        {
                            _service.DisableAutoBackup();
                            break; // Success!
                        }
                        catch (OracleException ex) when (ex.Number == 27478)
                        {
                            // Job is running, wait 2 seconds and retry
                            await System.Threading.Tasks.Task.Delay(2000);
                        }
                    }
                });

                if (this.IsDisposed) return;

                MessageBox.Show("Đã dừng sao lưu tự động (Job disabled).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateSchedulerStatusUI();
            }
            catch (Exception ex)
            {
                if (this.IsDisposed) return;
                ShowOperationError("Tat sao luu tu dong", ex);
                UpdateSchedulerStatusUI();
            }
            finally
            {
                if (!this.IsDisposed)
                {
                    btnDisableAutoBackup.Enabled = true;
                    btnDisableAutoBackup.Text = "Tắt sao lưu Tự động";
                }
            }
        }

        // Thực hiện khôi phục dữ liệu cho bản ghi
        private void BackupRestore()
        {
            if (cboBackupVersion.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn một thời điểm kiểm toán hợp lệ để khôi phục.", "Yêu cầu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime auditTime = (DateTime)cboBackupVersion.SelectedValue;

            string recordId = Prompt.ShowDialog("Nhập mã bệnh án (RECORD_ID) cần khôi phục:\n(Ví dụ: BA001)", "Nhập mã bệnh án");
            if (string.IsNullOrWhiteSpace(recordId))
            {
                return;
            }
            recordId = recordId.Trim().ToUpper();

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn khôi phục dữ liệu {recordId} về trạng thái ngay trước thời điểm {auditTime:HH:mm:ss dd/MM/yyyy}?", "Khôi phục dữ liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _service.BackupRestoreByAudit(recordId, auditTime);
                MessageBox.Show($"Khôi phục dữ liệu thành công cho {recordId} bằng Flashback Query!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadStandardAuditLogs();
                LoadAuditRecoveryPoints();
                LoadFgaAuditLogs();
                LoadTableData();
            }
            catch (Exception ex)
            {
                ShowOperationError("Khoi phuc Flashback", ex);
            }
        }

        private async System.Threading.Tasks.Task ImportDataPumpAsync()
        {
            string dumpFile = txtImportDumpFile.Text.Trim();
            if (string.IsNullOrWhiteSpace(dumpFile))
            {
                MessageBox.Show("Vui long nhap ten file dump.", "Yeu cau", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Import {dumpFile} vao HOSPITAL_RESTORE? Du lieu restore cu se bi thay the.",
                "Xac nhan Data Pump import",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            btnImportDataPump.Enabled = false;
            btnImportDataPump.Text = "Đang import...";

            try
            {
                string result = await System.Threading.Tasks.Task.Run(
                    () => _service.ImportDataPumpToRestore(dumpFile));

                MessageBox.Show(
                    "Import vao HOSPITAL_RESTORE thanh cong.\n\n" + result,
                    "Data Pump import",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                LoadTableData();
            }
            catch (Exception ex)
            {
                ShowOperationError("Data Pump import", ex);
            }
            finally
            {
                btnImportDataPump.Enabled = true;
                btnImportDataPump.Text = "Import dump";
            }
        }

        // --- Event Handlers ---
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStandardAuditLogs();
            LoadTableData();
        }
        private void btnBackupNow_Click(object sender, EventArgs e) => ManualBackup();
        private void btnEnableAutoBackup_Click(object sender, EventArgs e) => EnableAutoBackup();
        private void btnDisableAutoBackup_Click(object sender, EventArgs e) => DisableAutoBackup();

        private void btnLoadAudit_Click(object sender, EventArgs e) => LoadFgaAuditLogs();
        private void btnBackupRestore_Click(object sender, EventArgs e) => BackupRestore();
        private async void btnImportDataPump_Click(object sender, EventArgs e) => await ImportDataPumpAsync();
        private void btnOpenErrorLog_Click(object sender, EventArgs e) => OpenErrorLogFolder();
    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 350,
                Height = 160,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                MinimizeBox = false,
                MaximizeBox = false
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = text, AutoSize = true };
            TextBox textBox = new TextBox() { Left = 20, Top = 60, Width = 290 };
            Button confirmation = new Button() { Text = "Xác nhận", Left = 210, Width = 100, Top = 90, DialogResult = DialogResult.OK };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
