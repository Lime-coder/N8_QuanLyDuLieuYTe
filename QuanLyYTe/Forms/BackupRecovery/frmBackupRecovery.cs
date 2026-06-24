using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
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
            StyleNeutral(btnPreviewFlashback);
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
                dgvRestoreHistory.DataSource = dt;
                GridViewStyler.Format(dgvBackupHistory);
                GridViewStyler.Format(dgvRestoreHistory);

                if (dgvBackupHistory.Columns.Contains("BACKUP_ID")) dgvBackupHistory.Columns["BACKUP_ID"].HeaderText = "ID";
                if (dgvBackupHistory.Columns.Contains("BACKUP_TIME")) dgvBackupHistory.Columns["BACKUP_TIME"].HeaderText = "Thời gian";
                if (dgvBackupHistory.Columns.Contains("BACKUP_TYPE")) dgvBackupHistory.Columns["BACKUP_TYPE"].HeaderText = "Loại sao lưu";
                if (dgvBackupHistory.Columns.Contains("STATUS")) dgvBackupHistory.Columns["STATUS"].HeaderText = "Trạng thái";
                if (dgvRestoreHistory.Columns.Contains("BACKUP_ID")) dgvRestoreHistory.Columns["BACKUP_ID"].HeaderText = "ID";
                if (dgvRestoreHistory.Columns.Contains("BACKUP_TIME")) dgvRestoreHistory.Columns["BACKUP_TIME"].HeaderText = "Thời gian";
                if (dgvRestoreHistory.Columns.Contains("DUMP_FILE")) dgvRestoreHistory.Columns["DUMP_FILE"].HeaderText = "File dump";
                if (dgvRestoreHistory.Columns.Contains("STATUS")) dgvRestoreHistory.Columns["STATUS"].HeaderText = "Trạng thái";
            }
            catch (Exception ex)
            {
                ShowOperationError("Tai lich su sao luu", ex);
            }
        }

        private void dgvBackupHistory_SelectionChanged(object sender, EventArgs e)
        {
            var grid = sender as DataGridView ?? dgvBackupHistory;
            if (grid.CurrentRow == null ||
                !grid.Columns.Contains("DUMP_FILE"))
            {
                return;
            }

            object value = grid.CurrentRow.Cells["DUMP_FILE"].Value;
            if (value != null && value != DBNull.Value)
            {
                txtImportDumpFile.Text = value.ToString();
            }
        }
        // Tải audit DML theo bảng đang chọn trong tab Flashback.
        private void LoadAuditRecoveryPoints()
        {
            LoadFlashbackAuditForSelectedTable();
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
        // Nạp nhật ký DML phục vụ Flashback theo bảng.
        private void LoadFgaAuditLogs()
        {
            LoadFlashbackAuditForSelectedTable();
        }

        private string GetSelectedFlashbackTable()
        {
            switch (tabFlashbackTables.SelectedIndex)
            {
                case 0:
                    return "PATIENT";
                case 1:
                    return "MEDICAL_RECORD";
                default:
                    return "PATIENT";
            }
        }

        private void LoadFlashbackAuditForSelectedTable()
        {
            try
            {
                string tableName = GetSelectedFlashbackTable();
                DataTable dt = _service.GetFlashbackAuditLogs(tableName);
                AddParseColumns(dt, tableName);
                dgvFgaAudit.DataSource = dt;
                GridViewStyler.Format(dgvFgaAudit);

                if (dgvFgaAudit.Columns.Contains("USERNAME")) dgvFgaAudit.Columns["USERNAME"].HeaderText = "Người dùng";
                if (dgvFgaAudit.Columns.Contains("TIMESTAMP")) dgvFgaAudit.Columns["TIMESTAMP"].HeaderText = "Thời điểm";
                if (dgvFgaAudit.Columns.Contains("ACTION")) dgvFgaAudit.Columns["ACTION"].HeaderText = "Hành động";
                if (dgvFgaAudit.Columns.Contains("OBJECT")) dgvFgaAudit.Columns["OBJECT"].HeaderText = "Đối tượng";
                if (dgvFgaAudit.Columns.Contains("SCN")) dgvFgaAudit.Columns["SCN"].HeaderText = "SCN";
                if (dgvFgaAudit.Columns.Contains("KEY_TEXT")) dgvFgaAudit.Columns["KEY_TEXT"].HeaderText = "Khóa dòng";
                if (dgvFgaAudit.Columns.Contains("PARSE_STATUS")) dgvFgaAudit.Columns["PARSE_STATUS"].HeaderText = "Trạng thái parse";
                if (dgvFgaAudit.Columns.Contains("SOURCE")) dgvFgaAudit.Columns["SOURCE"].HeaderText = "Nguồn";
                if (dgvFgaAudit.Columns.Contains("SQL_TEXT")) dgvFgaAudit.Columns["SQL_TEXT"].HeaderText = "Câu lệnh SQL";
                if (dgvFgaAudit.Columns.Contains("CAN_RESTORE")) dgvFgaAudit.Columns["CAN_RESTORE"].Visible = false;

                dgvCurrentRow.DataSource = null;
                dgvFlashbackRow.DataSource = null;
                lblSelectedAudit.Text = $"Đang xem {tableName}: {dt.Rows.Count} audit DML.";
            }
            catch (Exception ex)
            {
                ShowOperationError("Tai audit Flashback", ex, MessageBoxIcon.Warning);
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

        private static string GetAuditValue(DataRowView row, string columnName)
        {
            if (row == null || row.Row == null || !row.Row.Table.Columns.Contains(columnName))
                return string.Empty;

            object value = row[columnName];
            return value == null || value == DBNull.Value ? string.Empty : value.ToString();
        }

        private static string[] GetPrimaryKeyColumns(string tableName)
        {
            switch ((tableName ?? string.Empty).Trim().ToUpperInvariant())
            {
                case "PATIENT":
                    return new[] { "PATIENT_ID" };
                case "MEDICAL_RECORD":
                    return new[] { "RECORD_ID" };
                default:
                    return Array.Empty<string>();
            }
        }

        private static string ExtractSqlValue(string sqlText, string columnName)
        {
            if (string.IsNullOrWhiteSpace(sqlText) || string.IsNullOrWhiteSpace(columnName))
                return string.Empty;

            string col = Regex.Escape(columnName);
            string[] patterns =
            {
                @"\b" + col + @"\b\s*=\s*DATE\s*'((?:''|[^'])*)'",
                @"\b" + col + @"\b\s*=\s*TO_DATE\s*\(\s*'((?:''|[^'])*)'",
                @"\b" + col + @"\b\s*=\s*N?'((?:''|[^'])*)'"
            };

            foreach (string pattern in patterns)
            {
                Match match = Regex.Match(sqlText, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                    return match.Groups[1].Value.Replace("''", "'").Trim();
            }

            return string.Empty;
        }

        private static Dictionary<string, string> ExtractAuditKeys(string tableName, string sqlText)
        {
            var keys = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (string columnName in GetPrimaryKeyColumns(tableName))
            {
                string value = ExtractSqlValue(sqlText, columnName);
                if (!string.IsNullOrWhiteSpace(value))
                    keys[columnName] = value;
            }

            return keys;
        }

        private static bool HasAllKeys(string tableName, Dictionary<string, string> keys)
        {
            foreach (string columnName in GetPrimaryKeyColumns(tableName))
            {
                if (!keys.TryGetValue(columnName, out string existing) || string.IsNullOrWhiteSpace(existing))
                    return false;
            }

            return true;
        }

        private static void AddParseColumns(DataTable dt, string tableName)
        {
            if (!dt.Columns.Contains("KEY_TEXT")) dt.Columns.Add("KEY_TEXT", typeof(string));
            if (!dt.Columns.Contains("PARSE_STATUS")) dt.Columns.Add("PARSE_STATUS", typeof(string));
            if (!dt.Columns.Contains("CAN_RESTORE")) dt.Columns.Add("CAN_RESTORE", typeof(bool));

            foreach (DataRow row in dt.Rows)
            {
                string sqlText = dt.Columns.Contains("SQL_TEXT") && row["SQL_TEXT"] != DBNull.Value ? row["SQL_TEXT"].ToString() : string.Empty;
                Dictionary<string, string> keys = ExtractAuditKeys(tableName, sqlText);
                bool canRestore = HasAllKeys(tableName, keys);

                row["KEY_TEXT"] = canRestore ? BuildKeySummary(tableName, keys) : "";
                row["PARSE_STATUS"] = canRestore ? "OK" : "Can nhap khoa thu cong";
                row["CAN_RESTORE"] = canRestore;
            }
        }

        private bool FillMissingKeysManually(string tableName, Dictionary<string, string> keys)
        {
            foreach (string columnName in GetPrimaryKeyColumns(tableName))
            {
                if (keys.TryGetValue(columnName, out string existing) && !string.IsNullOrWhiteSpace(existing))
                    continue;

                string hint = columnName.EndsWith("_DATE", StringComparison.OrdinalIgnoreCase)
                    ? "Nhap ngay theo dang YYYY-MM-DD hoac DD/MM/YYYY."
                    : "Nhap khoa chinh cua dong can khoi phuc.";

                string input = Prompt.ShowDialog(
                    $"Audit dang dung bind/procedure nen khong co gia tri {columnName} trong SQL_TEXT.\n{hint}",
                    $"Nhap {columnName}");

                if (string.IsNullOrWhiteSpace(input))
                    return false;

                keys[columnName] = input.Trim();
            }

            return true;
        }

        private static string GetProcedureKey(Dictionary<string, string> keys, string tableName, int index)
        {
            string[] columns = GetPrimaryKeyColumns(tableName);
            if (index >= columns.Length)
                return null;

            return keys.TryGetValue(columns[index], out string value) ? value : null;
        }

        private static string BuildKeySummary(string tableName, Dictionary<string, string> keys)
        {
            var parts = new List<string>();
            foreach (string columnName in GetPrimaryKeyColumns(tableName))
            {
                keys.TryGetValue(columnName, out string value);
                parts.Add($"{columnName}={value}");
            }

            return string.Join(", ", parts);
        }

        private bool TryGetSelectedFlashbackEvent(bool allowManualKeyEntry, out string tableName, out decimal auditScn, out Dictionary<string, string> keys, out string keySummary, out string actionName, out string auditTime)
        {
            tableName = GetSelectedFlashbackTable();
            auditScn = 0;
            keys = null;
            keySummary = string.Empty;
            actionName = string.Empty;
            auditTime = string.Empty;

            if (dgvFgaAudit.CurrentRow == null)
            {
                return false;
            }

            object scnValue = dgvFgaAudit.CurrentRow.Cells["SCN"].Value;
            if (scnValue == null || scnValue == DBNull.Value || !decimal.TryParse(scnValue.ToString(), out auditScn) || auditScn <= 1)
            {
                return false;
            }

            string sqlText = dgvFgaAudit.Columns.Contains("SQL_TEXT") && dgvFgaAudit.CurrentRow.Cells["SQL_TEXT"].Value != DBNull.Value
                ? dgvFgaAudit.CurrentRow.Cells["SQL_TEXT"].Value.ToString()
                : string.Empty;
            keys = ExtractAuditKeys(tableName, sqlText);
            if (!HasAllKeys(tableName, keys))
            {
                if (!allowManualKeyEntry || !FillMissingKeysManually(tableName, keys))
                    return false;
            }

            keySummary = BuildKeySummary(tableName, keys);
            actionName = dgvFgaAudit.Columns.Contains("ACTION") && dgvFgaAudit.CurrentRow.Cells["ACTION"].Value != DBNull.Value
                ? dgvFgaAudit.CurrentRow.Cells["ACTION"].Value.ToString()
                : string.Empty;
            auditTime = dgvFgaAudit.Columns.Contains("TIMESTAMP") && dgvFgaAudit.CurrentRow.Cells["TIMESTAMP"].Value != DBNull.Value
                ? dgvFgaAudit.CurrentRow.Cells["TIMESTAMP"].Value.ToString()
                : string.Empty;
            return true;
        }

        private void PreviewFlashback()
        {
            if (!TryGetSelectedFlashbackEvent(true, out string tableName, out decimal auditScn, out Dictionary<string, string> keys, out string keySummary, out _, out _))
            {
                MessageBox.Show("Chua co du khoa dong de preview.", "Flashback", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                dgvCurrentRow.DataSource = _service.GetCurrentAuditedRow(
                    tableName,
                    GetProcedureKey(keys, tableName, 0),
                    GetProcedureKey(keys, tableName, 1),
                    GetProcedureKey(keys, tableName, 2));
                dgvFlashbackRow.DataSource = _service.GetFlashbackAuditedRow(
                    tableName,
                    auditScn,
                    GetProcedureKey(keys, tableName, 0),
                    GetProcedureKey(keys, tableName, 1),
                    GetProcedureKey(keys, tableName, 2));
                GridViewStyler.Format(dgvCurrentRow);
                GridViewStyler.Format(dgvFlashbackRow);
                dgvCurrentRow.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvFlashbackRow.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                lblCurrentRow.Text = $"Hien tai: {tableName} ({keySummary})";
                lblFlashbackRow.Text = $"AS OF SCN {auditScn - 1}";
            }
            catch (Exception ex)
            {
                ShowOperationError("Preview Flashback", ex);
            }
        }

        // Thực hiện khôi phục dữ liệu cho bản ghi
        private void BackupRestore()
        {
            if (!TryGetSelectedFlashbackEvent(true, out string tableName, out decimal auditScn, out Dictionary<string, string> keys, out string keySummary, out string actionName, out string auditTime))
            {
                MessageBox.Show("Chua co du khoa dong de restore.", "Flashback", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Khoi phuc dong trong bang {tableName}\n{keySummary}\n\n" +
                $"Audit: {actionName} | SCN={auditScn} | {auditTime}\n" +
                $"Du lieu se duoc dua ve trang thai tai SCN {auditScn - 1}.",
                "Khoi phuc bang Flashback SCN",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _service.BackupRestoreAuditedRow(
                    tableName,
                    auditScn,
                    GetProcedureKey(keys, tableName, 0),
                    GetProcedureKey(keys, tableName, 1),
                    GetProcedureKey(keys, tableName, 2));

                MessageBox.Show($"Khoi phuc thanh cong dong {tableName}: {keySummary}", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void btnPreviewFlashback_Click(object sender, EventArgs e) => PreviewFlashback();
        private void btnBackupRestore_Click(object sender, EventArgs e) => BackupRestore();
        private async void btnImportDataPump_Click(object sender, EventArgs e) => await ImportDataPumpAsync();
        private void btnOpenErrorLog_Click(object sender, EventArgs e) => OpenErrorLogFolder();
        private void tabFlashbackTables_SelectedIndexChanged(object sender, EventArgs e) => LoadFlashbackAuditForSelectedTable();
        private void dgvFgaAudit_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvFgaAudit.CurrentRow == null)
            {
                lblSelectedAudit.Text = "Chon audit co parse key hop le de preview/restore.";
                return;
            }

            string status = dgvFgaAudit.Columns.Contains("PARSE_STATUS") && dgvFgaAudit.CurrentRow.Cells["PARSE_STATUS"].Value != null
                ? dgvFgaAudit.CurrentRow.Cells["PARSE_STATUS"].Value.ToString()
                : "";
            string key = dgvFgaAudit.Columns.Contains("KEY_TEXT") && dgvFgaAudit.CurrentRow.Cells["KEY_TEXT"].Value != null
                ? dgvFgaAudit.CurrentRow.Cells["KEY_TEXT"].Value.ToString()
                : "";

            lblSelectedAudit.Text = string.IsNullOrWhiteSpace(key)
                ? $"{status}. Bam Preview/Restore de nhap khoa thu cong."
                : $"{status}: {key}";
        }
    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 560,
                Height = 230,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                MinimizeBox = false,
                MaximizeBox = false
            };
            Label textLabel = new Label()
            {
                Left = 20,
                Top = 20,
                Width = 500,
                Height = 80,
                Text = text,
                AutoSize = false
            };
            TextBox textBox = new TextBox() { Left = 20, Top = 110, Width = 500 };
            Button confirmation = new Button() { Text = "Xác nhận", Left = 420, Width = 100, Top = 150, DialogResult = DialogResult.OK };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
