using System;
using System.Data;
using System.IO;
using QuanLyYTe.Repositories;

namespace QuanLyYTe.Services
{
    public class BackupRecoveryService
    {
        private readonly BackupRecoveryRepository _repo = new BackupRecoveryRepository();

        public DataTable GetStandardAuditLogs()
        {
            return _repo.GetStandardAuditLogs();
        }

        public DataTable GetBackupHistory()
        {
            return _repo.GetBackupHistory();
        }

        public DataTable GetJobState()
        {
            return _repo.GetJobState();
        }

        public DataTable GetFgaAuditLogs()
        {
            return _repo.GetFgaAuditLogs();
        }

        public DataTable GetFlashbackAuditLogs(string tableName)
        {
            return _repo.GetFlashbackAuditLogs(tableName);
        }

        public DataTable GetCurrentAuditedRow(string tableName, string key1, string key2, string key3)
        {
            return _repo.GetCurrentAuditedRow(tableName, key1, key2, key3);
        }

        public DataTable GetFlashbackAuditedRow(string tableName, decimal auditScn, string key1, string key2, string key3)
        {
            return _repo.GetFlashbackAuditedRow(tableName, auditScn, key1, key2, key3);
        }

        public void ManualBackup()
        {
            _repo.ManualBackup();
        }

        public string ImportDataPumpToRestore(string dumpFile)
        {
            string normalized = dumpFile?.Trim();
            if (string.IsNullOrWhiteSpace(normalized))
                throw new ArgumentException("Vui long nhap ten file dump.");

            if (!string.Equals(Path.GetFileName(normalized), normalized, StringComparison.Ordinal) ||
                !string.Equals(Path.GetExtension(normalized), ".dmp", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Chi nhap ten file .dmp, khong nhap duong dan.");
            }

            return _repo.ImportDataPumpToRestore(normalized);
        }

        public void EnableAutoBackup(string intervalText)
        {
            if (string.IsNullOrWhiteSpace(intervalText))
                throw new ArgumentException("Chu kỳ sao lưu không hợp lệ.");

            string repeatInterval = intervalText == "1 phút" ? "FREQ=MINUTELY;INTERVAL=1" : "FREQ=DAILY;INTERVAL=1";
            _repo.EnableAutoBackup(repeatInterval);
        }

        public void DisableAutoBackup()
        {
            _repo.DisableAutoBackup();
        }



        public void BackupRestoreAuditedRow(string tableName, decimal auditScn, string key1, string key2, string key3)
        {
            _repo.BackupRestoreAuditedRow(tableName, auditScn, key1, key2, key3);
        }

        public DataTable GetPrescriptions()
        {
            return _repo.GetPrescriptions();
        }

        public DataTable GetPatients()
        {
            return _repo.GetPatients();
        }

        public DataTable GetMedicalRecords()
        {
            return _repo.GetMedicalRecords();
        }

        public DataTable GetServiceRecords()
        {
            return _repo.GetServiceRecords();
        }
    }
}
