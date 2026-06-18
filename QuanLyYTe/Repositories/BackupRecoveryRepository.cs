using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DataProvider;

namespace QuanLyYTe.Repositories
{
    public class BackupRecoveryRepository
    {
        private readonly OracleDbProvider _dbProvider = new OracleDbProvider();

        public DataTable GetCurrentData()
        {
            string sql = "SELECT RECORD_ID, PRESCRIPTION_DATE, MEDICINE_NAME, DOSAGE FROM hospital.PRESCRIPTION ORDER BY RECORD_ID, MEDICINE_NAME";
            return _dbProvider.ExecuteQuery(sql);
        }

        public DataTable GetBackupHistory()
        {
            string sql = @"
                SELECT
                    BACKUP_ID,
                    BACKUP_TIME,
                    BACKUP_TYPE,
                    METHOD,
                    DIRECTORY_NAME,
                    DUMP_FILE,
                    LOG_FILE,
                    STATUS,
                    NOTE,
                    ERROR_MESSAGE
                FROM hospital.BACKUP_HISTORY
                ORDER BY BACKUP_TIME DESC";
            return _dbProvider.ExecuteQuery(sql);
        }

        public DataTable GetAuditRecoveryPoints()
        {
            string sql = @"
                SELECT EVENT_TIMESTAMP AS AUDIT_TIME, ACTION_NAME 
                FROM UNIFIED_AUDIT_TRAIL 
                WHERE OBJECT_SCHEMA = 'HOSPITAL' AND OBJECT_NAME = 'PRESCRIPTION' AND ACTION_NAME IN ('UPDATE', 'DELETE')
                ORDER BY EVENT_TIMESTAMP DESC";
            return _dbProvider.ExecuteQuery(sql);
        }

        public DataTable GetCustomAuditLogs(string tableName)
        {
            string sql = $"SELECT DBUSERNAME, EVENT_TIMESTAMP, ACTION_NAME, OBJECT_NAME FROM hospital.{tableName} WHERE OBJECT_NAME = 'PRESCRIPTION' ORDER BY EVENT_TIMESTAMP DESC";
            return _dbProvider.ExecuteQuery(sql);
        }

        public DataTable GetUnifiedAuditLogs()
        {
            string sql = "SELECT DBUSERNAME, EVENT_TIMESTAMP, ACTION_NAME, OBJECT_NAME FROM UNIFIED_AUDIT_TRAIL WHERE OBJECT_SCHEMA = 'HOSPITAL' AND OBJECT_NAME = 'PRESCRIPTION' ORDER BY EVENT_TIMESTAMP DESC";
            return _dbProvider.ExecuteQuery(sql);
        }

        public DataTable GetJobState()
        {
            string sql = "SELECT STATE, REPEAT_INTERVAL FROM ALL_SCHEDULER_JOBS WHERE OWNER = 'HOSPITAL' AND JOB_NAME = 'AUTO_BACKUP_JOB'";
            return _dbProvider.ExecuteQuery(sql);
        }

        public void ManualBackup()
        {
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            {
                using (OracleCommand cmd = new OracleCommand("hospital.USP_MANUAL_BACKUP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EnableAutoBackup(string repeatInterval)
        {
            string sql = $@"
            BEGIN
                -- Disable job cũ nếu đang chạy
                BEGIN
                    DBMS_SCHEDULER.DISABLE('hospital.AUTO_BACKUP_JOB', force => TRUE);
                EXCEPTION WHEN OTHERS THEN NULL;
                END;

                -- Xóa job cũ
                BEGIN
                    DBMS_SCHEDULER.DROP_JOB('hospital.AUTO_BACKUP_JOB', force => TRUE);
                EXCEPTION WHEN OTHERS THEN NULL;
                END;

                -- Tạo và kích hoạt job mới
                DBMS_SCHEDULER.CREATE_JOB (
                    job_name        => 'hospital.AUTO_BACKUP_JOB',
                    job_type        => 'STORED_PROCEDURE',
                    job_action      => 'hospital.USP_AUTO_BACKUP',
                    start_date      => SYSTIMESTAMP,
                    repeat_interval => '{repeatInterval}',
                    enabled         => TRUE,
                    comments        => 'Automatic backup job for PRESCRIPTION table'
                );
            END;";

            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            {
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DisableAutoBackup()
        {
            string sql = @"
            BEGIN
                DBMS_SCHEDULER.DISABLE('hospital.AUTO_BACKUP_JOB', force => TRUE);
            END;";

            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            {
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int SimulateDelete()
        {
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            {
                using (OracleCommand cmd = new OracleCommand("hospital.USP_SIMULATE_DELETE", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
            }
        }

        public int SimulateWrongUpdate()
        {
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            {
                using (OracleCommand cmd = new OracleCommand("hospital.USP_SIMULATE_WRONG_UPDATE", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
            }
        }

        public void BackupRestoreByAudit(string recordId, DateTime auditTime)
        {
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            {
                using (OracleCommand cmd = new OracleCommand("hospital.USP_RESTORE_PRESCRIPTION_BY_AUDIT", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new OracleParameter("p_record_id", OracleDbType.Varchar2) { Value = recordId });
                    cmd.Parameters.Add(new OracleParameter("p_audit_event_time", OracleDbType.TimeStamp) { Value = auditTime });
                    cmd.Parameters.Add(new OracleParameter("p_seconds_before", OracleDbType.Int32) { Value = 1 });
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
