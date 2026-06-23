using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DataProvider;

namespace QuanLyYTe.Repositories
{
    public class BackupRecoveryRepository : BaseRepository
    {

        // Lấy dữ liệu hiện tại của bảng PRESCRIPTION
        public DataTable GetCurrentData()
        {
            string sql = "SELECT RECORD_ID, PRESCRIPTION_DATE, MEDICINE_NAME, DOSAGE FROM hospital.PRESCRIPTION ORDER BY RECORD_ID, MEDICINE_NAME";
            return _dbProvider.ExecuteQuery(sql);
        }

        // Lấy lịch sử sao lưu từ BACKUP_HISTORY
        public DataTable GetBackupHistory()
        {
            string sql = @"
                SELECT
                    BACKUP_ID,
                    BACKUP_TIME,
                    BACKUP_TYPE,
                    DUMP_FILE,
                    STATUS,
                    ERROR_MESSAGE
                FROM hospital.BACKUP_HISTORY
                ORDER BY BACKUP_TIME DESC";
            return _dbProvider.ExecuteQuery(sql);
        }

        // Lấy các điểm phục hồi từ UNIFIED_AUDIT_TRAIL
        public DataTable GetAuditRecoveryPoints()
        {
            string sql = @"
                SELECT EVENT_TIMESTAMP AS AUDIT_TIME, ACTION_NAME 
                FROM UNIFIED_AUDIT_TRAIL 
                WHERE OBJECT_SCHEMA = 'HOSPITAL' 
                  AND OBJECT_NAME = 'PRESCRIPTION' 
                  AND ACTION_NAME IN ('UPDATE', 'DELETE')
                ORDER BY EVENT_TIMESTAMP DESC";
            return _dbProvider.ExecuteQuery(sql);
        }

        // Lấy nhật ký Audit của PRESCRIPTION
        public DataTable GetAuditLogs()
        {
            string sql = @"
                SELECT 
                    CAST(DBUSERNAME AS VARCHAR2(128)) as USERNAME, 
                    TO_CHAR(EVENT_TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') as TIMESTAMP, 
                    CAST(OBJECT_NAME AS VARCHAR2(128)) as OBJECT, 
                    CAST(ACTION_NAME AS VARCHAR2(128)) as ACTION, 
                    'Success' AS STATUS, 
                    CAST(SQL_TEXT AS VARCHAR2(4000)) as SQL_TEXT
                FROM UNIFIED_AUDIT_TRAIL 
                WHERE OBJECT_SCHEMA = 'HOSPITAL' 
                  AND OBJECT_NAME = 'PRESCRIPTION' 
                  AND ACTION_NAME IN ('UPDATE', 'DELETE')
                ORDER BY EVENT_TIMESTAMP DESC";
            return _dbProvider.ExecuteQuery(sql);
        }

        // Kiểm tra trạng thái của AUTO_BACKUP_JOB
        public DataTable GetJobState()
        {
            string sql = "SELECT STATE, REPEAT_INTERVAL FROM ALL_SCHEDULER_JOBS WHERE OWNER = 'HOSPITAL' AND JOB_NAME = 'AUTO_BACKUP_JOB'";
            return _dbProvider.ExecuteQuery(sql);
        }

        // Thực hiện sao lưu thủ công (SP: USP_MANUAL_BACKUP)
        public void ManualBackup()
        {
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            {
                using (OracleCommand cmd = new OracleCommand("hospital.PKG_BACKUP_RECOVERY.USP_MANUAL_BACKUP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Kích hoạt sao lưu tự động với chu kỳ lặp lại
        public string ImportDataPumpToRestore(string dumpFile)
        {
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            using (OracleCommand cmd = new OracleCommand("hospital_dba.USP_IMPORT_DATAPUMP_TO_RESTORE", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.BindByName = true;
                cmd.CommandTimeout = 0;

                cmd.Parameters.Add("p_dump_file", OracleDbType.Varchar2, 255).Value = dumpFile;

                OracleParameter stateParam = cmd.Parameters.Add("p_job_state", OracleDbType.Varchar2, 30);
                stateParam.Direction = ParameterDirection.Output;

                OracleParameter logParam = cmd.Parameters.Add("p_log_file", OracleDbType.Varchar2, 255);
                logParam.Direction = ParameterDirection.Output;

                OracleParameter tableCountParam = cmd.Parameters.Add("p_table_count", OracleDbType.Int32);
                tableCountParam.Direction = ParameterDirection.Output;

                OracleParameter prescriptionCountParam = cmd.Parameters.Add("p_prescription_count", OracleDbType.Int32);
                prescriptionCountParam.Direction = ParameterDirection.Output;

                conn.Open();
                cmd.ExecuteNonQuery();

                return $"State: {stateParam.Value}; Log: {logParam.Value}; " +
                       $"Tables: {tableCountParam.Value}; PRESCRIPTION rows: {prescriptionCountParam.Value}";
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
                    job_action      => 'hospital.PKG_BACKUP_RECOVERY.USP_AUTO_BACKUP',
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

        // Vô hiệu hóa sao lưu tự động
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

        // Giả lập xóa dữ liệu (SP: USP_SIMULATE_DELETE)
        public int SimulateDelete()
        {
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            {
                using (OracleCommand cmd = new OracleCommand("hospital.PKG_BACKUP_RECOVERY.USP_SIMULATE_DELETE", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
            }
        }

        // Giả lập cập nhật sai dữ liệu (SP: USP_SIMULATE_WRONG_UPDATE)
        public int SimulateWrongUpdate()
        {
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            {
                using (OracleCommand cmd = new OracleCommand("hospital.PKG_BACKUP_RECOVERY.USP_SIMULATE_WRONG_UPDATE", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
            }
        }

        // Phục hồi dữ liệu dựa trên Audit (SP: USP_RESTORE_PRESCRIPTION_BY_AUDIT)
        public void BackupRestoreByAudit(string recordId, DateTime auditTime)
        {
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            {
                using (OracleCommand cmd = new OracleCommand("hospital.PKG_BACKUP_RECOVERY.USP_RESTORE_PRESCRIPTION_BY_AUDIT", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new OracleParameter("p_record_id", OracleDbType.Varchar2) { Value = recordId });
                    cmd.Parameters.Add(new OracleParameter("p_audit_event_time", OracleDbType.TimeStamp) { Value = auditTime });
                    cmd.Parameters.Add(new OracleParameter("p_seconds_before", OracleDbType.Int32) { Value = 0 });
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

