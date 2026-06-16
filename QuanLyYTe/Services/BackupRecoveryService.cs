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

        /// <summary>
        /// Lấy nhật ký kiểm toán (Audit Logs) theo thứ tự ưu tiên:
        /// 1. Tìm bảng kiểm toán từ Yêu cầu 3 (AUDIT_LOG hoặc NHATKY_KIEMTOAN).
        /// 2. Nếu không tìm thấy, sử dụng UNIFIED_AUDIT_TRAIL.
        /// </summary>
        public DataTable GetAuditLogs()
        {
            try
            {
                // Ưu tiên 1a: Bảng AUDIT_LOG
                return _repo.GetCustomAuditLogs("AUDIT_LOG");
            }
            catch
            {
                try
                {
                    // Ưu tiên 1b: Bảng NHATKY_KIEMTOAN
                    return _repo.GetCustomAuditLogs("NHATKY_KIEMTOAN");
                }
                catch
                {
                    // Ưu tiên 2: Hệ thống UNIFIED_AUDIT_TRAIL
                    return _repo.GetUnifiedAuditLogs();
                }
            }
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
