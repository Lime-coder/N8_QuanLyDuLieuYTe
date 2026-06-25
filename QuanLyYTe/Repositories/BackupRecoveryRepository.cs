using System;
using System.Data;
using System.Globalization;
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
                WHERE OWNER IN ('HOSPITAL_DBA', 'HOSPITAL_DBA')
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
                FROM hospital_dba.BACKUP_HISTORY
                ORDER BY BACKUP_TIME DESC";
            return _dbProvider.ExecuteQuery(sql);
        }

        // Hien thi audit schema co SCN cho cac bang co the phuc hoi.
        public DataTable GetFgaAuditLogs()
        {
            try { _dbProvider.ExecuteQuery("BEGIN DBMS_AUDIT_MGMT.FLUSH_UNIFIED_AUDIT_TRAIL; END;"); } catch { }

            string sql = @"
                SELECT
                    USERNAME,
                    OBJECT_NAME AS OBJECT,
                    ACTION_NAME AS ACTION,
                    TO_CHAR(AUDIT_TIME, 'DD/MM/YYYY HH24:MI:SS') AS TIMESTAMP,
                    SCN,
                    SOURCE,
                    SQL_TEXT
                FROM (
                    SELECT
                        DBUSERNAME AS USERNAME,
                        OBJECT_NAME,
                        ACTION_NAME,
                        EVENT_TIMESTAMP AS AUDIT_TIME,
                        SCN,
                        'UNIFIED' AS SOURCE,
                        CAST(SQL_TEXT AS VARCHAR2(4000)) AS SQL_TEXT
                    FROM UNIFIED_AUDIT_TRAIL
                    WHERE OBJECT_SCHEMA = 'HOSPITAL_DBA'
                      AND OBJECT_NAME IN ('PRESCRIPTION', 'MEDICAL_RECORD', 'SERVICE_RECORD', 'PATIENT')
                      AND ACTION_NAME IN ('INSERT', 'UPDATE', 'DELETE')
                      AND RETURN_CODE = 0
                    UNION ALL
                    SELECT
                        DB_USER AS USERNAME,
                        OBJECT_NAME,
                        STATEMENT_TYPE AS ACTION_NAME,
                        TIMESTAMP AS AUDIT_TIME,
                        SCN,
                        'FGA' AS SOURCE,
                        CAST(SQL_TEXT AS VARCHAR2(4000)) AS SQL_TEXT
                    FROM DBA_FGA_AUDIT_TRAIL
                    WHERE OBJECT_SCHEMA = 'HOSPITAL_DBA'
                      AND OBJECT_NAME IN ('PRESCRIPTION', 'MEDICAL_RECORD', 'SERVICE_RECORD', 'PATIENT')
                      AND STATEMENT_TYPE IN ('INSERT', 'UPDATE', 'DELETE')
                )
                WHERE SCN IS NOT NULL
                ORDER BY AUDIT_TIME DESC";
            return _dbProvider.ExecuteQuery(sql);
        }

        public DataTable GetFlashbackAuditLogs(string tableName)
        {
            string safeTable = NormalizeAuditedTableName(tableName);
            try { _dbProvider.ExecuteQuery("BEGIN DBMS_AUDIT_MGMT.FLUSH_UNIFIED_AUDIT_TRAIL; END;"); } catch { }

            string sql = $@"
                SELECT
                    TO_CHAR(AUDIT_TIME, 'DD/MM/YYYY HH24:MI:SS') AS TIMESTAMP,
                    SCN,
                    USERNAME,
                    ACTION_NAME AS ACTION,
                    OBJECT_NAME AS OBJECT,
                    SOURCE,
                    SQL_TEXT
                FROM (
                    SELECT
                        EVENT_TIMESTAMP AS AUDIT_TIME,
                        SCN,
                        DBUSERNAME AS USERNAME,
                        ACTION_NAME,
                        OBJECT_NAME,
                        'UNIFIED' AS SOURCE,
                        CAST(SQL_TEXT AS VARCHAR2(4000)) AS SQL_TEXT
                    FROM UNIFIED_AUDIT_TRAIL
                    WHERE OBJECT_SCHEMA = 'HOSPITAL_DBA'
                      AND OBJECT_NAME = '{safeTable}'
                      AND ACTION_NAME IN ('INSERT', 'UPDATE', 'DELETE')
                      AND RETURN_CODE = 0
                    UNION ALL
                    SELECT
                        TIMESTAMP AS AUDIT_TIME,
                        SCN,
                        DB_USER AS USERNAME,
                        STATEMENT_TYPE AS ACTION_NAME,
                        OBJECT_NAME,
                        'FGA' AS SOURCE,
                        CAST(SQL_TEXT AS VARCHAR2(4000)) AS SQL_TEXT
                    FROM DBA_FGA_AUDIT_TRAIL
                    WHERE OBJECT_SCHEMA = 'HOSPITAL_DBA'
                      AND OBJECT_NAME = '{safeTable}'
                      AND STATEMENT_TYPE IN ('INSERT', 'UPDATE', 'DELETE')
                )
                WHERE SCN IS NOT NULL
                ORDER BY AUDIT_TIME DESC";
            return _dbProvider.ExecuteQuery(sql);
        }

        public DataTable GetCurrentAuditedRow(string tableName, string key1, string key2, string key3)
        {
            return GetAuditedRow(tableName, null, key1, key2, key3);
        }

        public DataTable GetFlashbackAuditedRow(string tableName, decimal auditScn, string key1, string key2, string key3)
        {
            if (auditScn <= 1)
                throw new ArgumentException("SCN phai lon hon 1.");

            return GetAuditedRow(tableName, auditScn - 1, key1, key2, key3);
        }

        private DataTable GetAuditedRow(string tableName, decimal? flashbackScn, string key1, string key2, string key3)
        {
            string safeTable = NormalizeAuditedTableName(tableName);
            string flashbackClause = flashbackScn.HasValue ? $" AS OF SCN {flashbackScn.Value.ToString(CultureInfo.InvariantCulture)}" : string.Empty;
            string sql;

            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            using (OracleCommand cmd = conn.CreateCommand())
            {
                cmd.BindByName = true;

                switch (safeTable)
                {
                    case "PATIENT":
                        sql = $"SELECT PATIENT_ID, FULL_NAME, GENDER, BIRTHDATE, ID_CARD FROM hospital_dba.PATIENT{flashbackClause} WHERE PATIENT_ID = :key1";
                        cmd.Parameters.Add("key1", OracleDbType.Varchar2).Value = key1;
                        break;

                    case "MEDICAL_RECORD":
                        sql = $"SELECT * FROM hospital_dba.MEDICAL_RECORD{flashbackClause} WHERE RECORD_ID = :key1";
                        cmd.Parameters.Add("key1", OracleDbType.Varchar2).Value = key1;
                        break;

                    case "PRESCRIPTION":
                        sql = $"SELECT * FROM hospital_dba.PRESCRIPTION{flashbackClause} " +
                              "WHERE RECORD_ID = :key1 AND TRUNC(PRESCRIPTION_DATE) = TRUNC(:key2) AND MEDICINE_NAME = :key3";
                        cmd.Parameters.Add("key1", OracleDbType.Varchar2).Value = key1;
                        cmd.Parameters.Add("key2", OracleDbType.Date).Value = ParseDateKey(key2);
                        cmd.Parameters.Add("key3", OracleDbType.NVarchar2).Value = key3;
                        break;

                    case "SERVICE_RECORD":
                        sql = $"SELECT * FROM hospital_dba.SERVICE_RECORD{flashbackClause} " +
                              "WHERE RECORD_ID = :key1 AND SERVICE_TYPE = :key2 AND TRUNC(SERVICE_DATE) = TRUNC(:key3)";
                        cmd.Parameters.Add("key1", OracleDbType.Varchar2).Value = key1;
                        cmd.Parameters.Add("key2", OracleDbType.NVarchar2).Value = key2;
                        cmd.Parameters.Add("key3", OracleDbType.Date).Value = ParseDateKey(key3);
                        break;

                    default:
                        throw new ArgumentException("Bang khong duoc ho tro Flashback.");
                }

                cmd.CommandText = sql;
                using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                {
                    DataTable result = new DataTable();
                    conn.Open();
                    adapter.Fill(result);
                    return result;
                }
            }
        }

        private static string NormalizeAuditedTableName(string tableName)
        {
            string safeTable = (tableName ?? string.Empty).Trim().ToUpperInvariant();
            switch (safeTable)
            {
                case "PATIENT":
                case "MEDICAL_RECORD":
                case "PRESCRIPTION":
                case "SERVICE_RECORD":
                    return safeTable;
                default:
                    throw new ArgumentException("Bang khong duoc ho tro Flashback.");
            }
        }

        private static DateTime ParseDateKey(string value)
        {
            string[] formats =
            {
                "yyyy-MM-dd HH:mm:ss",
                "yyyy-MM-dd",
                "dd/MM/yyyy HH:mm:ss",
                "dd/MM/yyyy",
                "MM/dd/yyyy HH:mm:ss",
                "MM/dd/yyyy"
            };

            if (DateTime.TryParseExact(value?.Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime exact))
                return exact;

            if (DateTime.TryParse(value, out DateTime parsed))
                return parsed;

            throw new ArgumentException("Gia tri ngay khong hop le: " + value);
        }

        // Kiểm tra trạng thái của AUTO_BACKUP_JOB
        public DataTable GetJobState()
        {
            string sql = "SELECT STATE, ENABLED, REPEAT_INTERVAL FROM ALL_SCHEDULER_JOBS WHERE OWNER = 'HOSPITAL_DBA' AND JOB_NAME = 'AUTO_BACKUP_JOB'";
            return _dbProvider.ExecuteQuery(sql);
        }

        // Thực hiện sao lưu thủ công (SP: USP_MANUAL_BACKUP)
        public void ManualBackup()
        {
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            {
                using (OracleCommand cmd = new OracleCommand("hospital_dba.PKG_BACKUP_RECOVERY.USP_MANUAL_BACKUP", conn))
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
            DECLARE
                v_count NUMBER;
            BEGIN
                SELECT COUNT(*) INTO v_count FROM ALL_SCHEDULER_JOBS WHERE JOB_NAME = 'AUTO_BACKUP_JOB' AND OWNER = 'HOSPITAL_DBA';
                IF v_count = 0 THEN
                    DBMS_SCHEDULER.CREATE_JOB (
                        job_name        => 'hospital_dba.AUTO_BACKUP_JOB',
                        job_type        => 'STORED_PROCEDURE',
                        job_action      => 'hospital_dba.PKG_BACKUP_RECOVERY.USP_AUTO_BACKUP',
                        start_date      => SYSTIMESTAMP,
                        repeat_interval => '{repeatInterval}',
                        enabled         => TRUE,
                        comments        => 'Automatic backup job for PRESCRIPTION table'
                    );
                ELSE
                    DBMS_SCHEDULER.DISABLE('hospital_dba.AUTO_BACKUP_JOB', force => FALSE);
                    DBMS_SCHEDULER.SET_ATTRIBUTE('hospital_dba.AUTO_BACKUP_JOB', 'job_type', 'STORED_PROCEDURE');
                    DBMS_SCHEDULER.SET_ATTRIBUTE('hospital_dba.AUTO_BACKUP_JOB', 'job_action', 'hospital_dba.PKG_BACKUP_RECOVERY.USP_AUTO_BACKUP');
                    DBMS_SCHEDULER.SET_ATTRIBUTE('hospital_dba.AUTO_BACKUP_JOB', 'repeat_interval', '{repeatInterval}');
                    DBMS_SCHEDULER.SET_ATTRIBUTE('hospital_dba.AUTO_BACKUP_JOB', 'start_date', SYSTIMESTAMP);
                    DBMS_SCHEDULER.SET_ATTRIBUTE_NULL('hospital_dba.AUTO_BACKUP_JOB', 'end_date');
                    DBMS_SCHEDULER.ENABLE('hospital_dba.AUTO_BACKUP_JOB');
                END IF;

                BEGIN
                    DBMS_SCHEDULER.RUN_JOB('hospital_dba.AUTO_BACKUP_JOB', use_current_session => FALSE);
                EXCEPTION WHEN OTHERS THEN
                    NULL;
                END;
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
                -- Stop any currently running instance first
                BEGIN
                    DBMS_SCHEDULER.STOP_JOB('hospital_dba.AUTO_BACKUP_JOB', force => TRUE);
                EXCEPTION WHEN OTHERS THEN NULL; -- OK if not running
                END;
                -- Then disable to prevent rescheduling
                DBMS_SCHEDULER.DISABLE('hospital_dba.AUTO_BACKUP_JOB', force => TRUE);
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

        // Phuc hoi dung mot dong cua bang duoc audit dua tren SCN.
        public void BackupRestoreAuditedRow(string tableName, decimal auditScn, string key1, string key2, string key3)
        {
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            {
                using (OracleCommand cmd = new OracleCommand("hospital_dba.PKG_BACKUP_RECOVERY.USP_RESTORE_AUDITED_ROW_BY_SCN", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.BindByName = true;
                    cmd.Parameters.Add("p_table_name", OracleDbType.Varchar2, 128).Value = tableName;
                    cmd.Parameters.Add("p_audit_scn", OracleDbType.Decimal).Value = auditScn;
                    cmd.Parameters.Add("p_key1", OracleDbType.Varchar2, 400).Value = key1;
                    cmd.Parameters.Add("p_key2", OracleDbType.Varchar2, 400).Value = string.IsNullOrWhiteSpace(key2) ? (object)DBNull.Value : key2;
                    cmd.Parameters.Add("p_key3", OracleDbType.Varchar2, 400).Value = string.IsNullOrWhiteSpace(key3) ? (object)DBNull.Value : key3;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetPrescriptions()
        {
            return _dbProvider.ExecuteQuery("SELECT * FROM hospital_dba.PRESCRIPTION");
        }

        public DataTable GetPatients()
        {
            return _dbProvider.ExecuteQuery("SELECT PATIENT_ID, FULL_NAME, GENDER, BIRTHDATE, ID_CARD FROM hospital_dba.PATIENT");
        }

        public DataTable GetMedicalRecords()
        {
            return _dbProvider.ExecuteQuery("SELECT * FROM hospital_dba.MEDICAL_RECORD");
        }

        public DataTable GetServiceRecords()
        {
            return _dbProvider.ExecuteQuery("SELECT * FROM hospital_dba.SERVICE_RECORD");
        }
    }
}

