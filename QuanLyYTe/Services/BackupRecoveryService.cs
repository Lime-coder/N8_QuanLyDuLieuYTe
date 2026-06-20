using System;
using System.Data;
using QuanLyYTe.Repositories;

namespace QuanLyYTe.Services
{
    public class BackupRecoveryService
    {
        private readonly BackupRecoveryRepository _repo = new BackupRecoveryRepository();

        public DataTable GetCurrentData()
        {
            return _repo.GetCurrentData();
        }

        public DataTable GetBackupHistory()
        {
            return _repo.GetBackupHistory();
        }

        public DataTable GetAuditRecoveryPoints()
        {
            return _repo.GetAuditRecoveryPoints();
        }

        public DataTable GetJobState()
        {
            return _repo.GetJobState();
        }

        public DataTable GetAuditLogs()
        {
            return _repo.GetAuditLogs();
        }

        public void ManualBackup()
        {
            _repo.ManualBackup();
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

        public int SimulateDelete()
        {
            return _repo.SimulateDelete();
        }

        public int SimulateWrongUpdate()
        {
            return _repo.SimulateWrongUpdate();
        }

        public void BackupRestoreByAudit(DateTime auditTime)
        {
            _repo.BackupRestoreByAudit("BA000001", auditTime);
        }
    }
}
