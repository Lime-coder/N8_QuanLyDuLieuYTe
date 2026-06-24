using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DataProvider;

namespace QuanLyYTe.Repositories
{
    public class BackupRecoveryRepository : BaseRepository
    {

        // Lấy nhật ký kiểm toán hệ thống (Standard Audit)
        public DataTable GetStandardAuditLogs()
        {
            string sql = @"
                SELECT 
                    CAST(USERNAME AS VARCHAR2(128)) as USERNAME, 
                    CAST(OBJ_NAME AS VARCHAR2(128)) as OBJECT, 
                    CAST(ACTION_NAME AS VARCHAR2(128)) as ACTION, 
                    TO_CHAR(TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') as TIMESTAMP
                FROM DBA_AUDIT_TRAIL 
                WHERE OWNER IN ('HOSPITAL', 'HOSPITAL_DBA')
                  AND RETURNCODE = 0
                ORDER BY TO_DATE(TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') DESC";
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

        // Lấy các điểm phục hồi từ UNIFIED_AUDIT_TRAIL và FGA (Chỉ lấy thành công)
        public DataTable GetAuditRecoveryPoints()
        {
            try { _dbProvider.ExecuteQuery("BEGIN DBMS_AUDIT_MGMT.FLUSH_UNIFIED_AUDIT_TRAIL; END;"); } catch { }

            string sql = @"
                SELECT TO_CHAR(AUDIT_TIME, 'YYYY-MM-DD HH24:MI:SS') AS AUDIT_TIME_STR, ACTION_NAME, SOURCE 
                FROM (
                    SELECT EVENT_TIMESTAMP AS AUDIT_TIME, ACTION_NAME, 'Hệ thống' AS SOURCE 
                    FROM UNIFIED_AUDIT_TRAIL 
                    WHERE OBJECT_SCHEMA = 'HOSPITAL' 
                      AND OBJECT_NAME IN ('PRESCRIPTION', 'MEDICAL_RECORD', 'SERVICE_RECORD', 'PATIENT')
                      AND ACTION_NAME IN ('UPDATE', 'DELETE')
                      AND RETURN_CODE = 0
                    UNION ALL
                    SELECT TIMESTAMP AS AUDIT_TIME, STATEMENT_TYPE AS ACTION_NAME, 'Chi tiết' AS SOURCE
                    FROM DBA_FGA_AUDIT_TRAIL
                    WHERE OBJECT_SCHEMA = 'HOSPITAL'
                      AND OBJECT_NAME IN ('PRESCRIPTION', 'MEDICAL_RECORD', 'SERVICE_RECORD')
                      AND STATEMENT_TYPE IN ('UPDATE', 'DELETE', 'INSERT')
                    UNION ALL
                    SELECT CAST(TIMESTAMP AS TIMESTAMP) AS AUDIT_TIME, ACTION_NAME, 'Hệ thống' AS SOURCE
                    FROM DBA_AUDIT_TRAIL
                    WHERE OWNER = 'HOSPITAL' 
                      AND OBJ_NAME = 'PATIENT'
                      AND ACTION_NAME IN ('UPDATE', 'DELETE')
                      AND RETURNCODE = 0
                )
                ORDER BY AUDIT_TIME DESC";
            return _dbProvider.ExecuteQuery(sql);
        }

        // Lấy nhật ký kiểm toán chi tiết (Fine-Grained Audit)
        public DataTable GetFgaAuditLogs()
        {
            try { _dbProvider.ExecuteQuery("BEGIN DBMS_AUDIT_MGMT.FLUSH_UNIFIED_AUDIT_TRAIL; END;"); } catch { }

            string sql = @"
                SELECT 
                    CAST(DB_USER AS VARCHAR2(128)) as USERNAME, 
                    CAST(OBJECT_NAME AS VARCHAR2(128)) as OBJECT, 
                    CAST(STATEMENT_TYPE AS VARCHAR2(128)) as ACTION, 
                    TO_CHAR(TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') as TIMESTAMP, 
                    CAST(SQL_TEXT AS VARCHAR2(4000)) as SQL_TEXT
                FROM DBA_FGA_AUDIT_TRAIL 
                WHERE OBJECT_SCHEMA = 'HOSPITAL'
                ORDER BY TO_DATE(TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') DESC";
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

        // Phục hồi dữ liệu dựa trên Audit (SP: USP_RESTORE_PRESCRIPTION_BY_AUDIT)
        public void BackupRestoreByAudit(string recordId, DateTime auditTime)
        {
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            {
                using (OracleCommand cmd = new OracleCommand("hospital.PKG_BACKUP_RECOVERY.USP_RESTORE_ALL_RECORDS_BY_AUDIT", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new OracleParameter("p_record_id", OracleDbType.Varchar2) { Value = recordId });
                    cmd.Parameters.Add(new OracleParameter("p_audit_event_time", OracleDbType.TimeStamp) { Value = auditTime });
                    cmd.Parameters.Add(new OracleParameter("p_seconds_before", OracleDbType.Int32) { Value = 5 });
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetPrescriptions()
        {
            return _dbProvider.ExecuteQuery("SELECT * FROM hospital.PRESCRIPTION");
        }

        public DataTable GetMedicalRecords()
        {
            return _dbProvider.ExecuteQuery("SELECT * FROM hospital.MEDICAL_RECORD");
        }

        public DataTable GetServiceRecords()
        {
            return _dbProvider.ExecuteQuery("SELECT * FROM hospital.SERVICE_RECORD");
        }
    }
}
