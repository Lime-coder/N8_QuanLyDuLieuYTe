-- ==============================================================================
-- BackupRecoverySetup_Revised.sql
-- Run as: HOSPITAL_DBA
-- Container: PDB_QLYT
-- Purpose: Requirement 4 - Data Pump + DBMS_SCHEDULER + Flashback Query based on Audit Log
-- ==============================================================================
-- Notes:
-- 1. This script replaces the previous table-copy backup approach.
-- 2. Backup mechanism: Oracle Data Pump export of schema HOSPITAL.
-- 3. Auto backup: DBMS_SCHEDULER.
-- 4. Logical recovery: Unified Audit identifies incident timestamp, Flashback Query restores data before incident.
-- 5. Demo recovery is implemented for HOSPITAL.PRESCRIPTION because Requirement 3 audits prescription changes.
-- ==============================================================================

ALTER SESSION SET CURRENT_SCHEMA = hospital;
SET SERVEROUTPUT ON;

-- ==============================================================================
-- 1. BACKUP HISTORY TABLE
-- ==============================================================================
BEGIN
    EXECUTE IMMEDIATE '
        CREATE TABLE hospital.BACKUP_HISTORY (
            BACKUP_ID       NUMBER PRIMARY KEY,
            BACKUP_TIME     TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
            BACKUP_TYPE     VARCHAR2(20) NOT NULL,
            METHOD          VARCHAR2(30) DEFAULT ''DATA_PUMP'' NOT NULL,
            DIRECTORY_NAME  VARCHAR2(128),
            DUMP_FILE       VARCHAR2(255),
            LOG_FILE        VARCHAR2(255),
            STATUS          VARCHAR2(20) NOT NULL,
            NOTE            VARCHAR2(1000),
            ERROR_MESSAGE   VARCHAR2(4000)
        )';
    DBMS_OUTPUT.PUT_LINE('[OK] Created BACKUP_HISTORY');
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE = -955 THEN
            DBMS_OUTPUT.PUT_LINE('[SKIP] BACKUP_HISTORY already exists');
        ELSE
            RAISE;
        END IF;
END;
/

-- ==============================================================================
-- 2. RECOVERY HISTORY TABLE
-- ==============================================================================
BEGIN
    EXECUTE IMMEDIATE '
        CREATE TABLE hospital.RECOVERY_HISTORY (
            RECOVERY_ID          NUMBER PRIMARY KEY,
            RECOVERY_TIME        TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
            TARGET_TABLE         VARCHAR2(128) NOT NULL,
            TARGET_KEY           VARCHAR2(200),
            AUDIT_EVENT_TIME     TIMESTAMP,
            FLASHBACK_TIME       TIMESTAMP,
            STATUS               VARCHAR2(20) NOT NULL,
            NOTE                 VARCHAR2(1000),
            ERROR_MESSAGE        VARCHAR2(4000)
        )';
    DBMS_OUTPUT.PUT_LINE('[OK] Created RECOVERY_HISTORY');
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE = -955 THEN
            DBMS_OUTPUT.PUT_LINE('[SKIP] RECOVERY_HISTORY already exists');
        ELSE
            RAISE;
        END IF;
END;
/

-- ==============================================================================
-- 3. SEQUENCES
-- ==============================================================================
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE hospital.SEQ_BACKUP_ID START WITH 1 INCREMENT BY 1 NOCACHE NOCYCLE';
    DBMS_OUTPUT.PUT_LINE('[OK] SEQ_BACKUP_ID created');
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE = -955 THEN
            DBMS_OUTPUT.PUT_LINE('[SKIP] SEQ_BACKUP_ID already exists');
        ELSE
            RAISE;
        END IF;
END;
/

BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE hospital.SEQ_RECOVERY_ID START WITH 1 INCREMENT BY 1 NOCACHE NOCYCLE';
    DBMS_OUTPUT.PUT_LINE('[OK] SEQ_RECOVERY_ID created');
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE = -955 THEN
            DBMS_OUTPUT.PUT_LINE('[SKIP] SEQ_RECOVERY_ID already exists');
        ELSE
            RAISE;
        END IF;
END;
/

-- ==============================================================================
-- 4. PACKAGE: DATA PUMP BACKUP + FLASHBACK RECOVERY
-- ==============================================================================
CREATE OR REPLACE PACKAGE hospital.PKG_BACKUP_RECOVERY AS
    PROCEDURE USP_BACKUP_DATAPUMP(
        p_backup_type    IN VARCHAR2 DEFAULT 'MANUAL',
        p_directory_name IN VARCHAR2 DEFAULT 'HOSPITAL_BACKUP_DIR'
    );

    PROCEDURE USP_MANUAL_BACKUP;

    PROCEDURE USP_AUTO_BACKUP;

    PROCEDURE USP_RESTORE_PRESCRIPTION_BY_AUDIT(
        p_record_id         IN VARCHAR2,
        p_audit_event_time  IN TIMESTAMP DEFAULT NULL,
        p_seconds_before    IN NUMBER DEFAULT 1
    );

    PROCEDURE USP_SIMULATE_WRONG_UPDATE;
    PROCEDURE USP_SIMULATE_DELETE;
END PKG_BACKUP_RECOVERY;
/

SHOW ERRORS PACKAGE hospital.PKG_BACKUP_RECOVERY;

CREATE OR REPLACE PACKAGE BODY hospital.PKG_BACKUP_RECOVERY AS

    PROCEDURE LOG_BACKUP(
        p_backup_id      IN NUMBER,
        p_backup_time    IN TIMESTAMP,
        p_backup_type    IN VARCHAR2,
        p_directory_name IN VARCHAR2,
        p_dump_file      IN VARCHAR2,
        p_log_file       IN VARCHAR2,
        p_status         IN VARCHAR2,
        p_note           IN VARCHAR2,
        p_error_message  IN VARCHAR2 DEFAULT NULL
    ) AS
        PRAGMA AUTONOMOUS_TRANSACTION;
    BEGIN
        INSERT INTO hospital.BACKUP_HISTORY(
            BACKUP_ID, BACKUP_TIME, BACKUP_TYPE, METHOD,
            DIRECTORY_NAME, DUMP_FILE, LOG_FILE, STATUS, NOTE, ERROR_MESSAGE
        ) VALUES (
            p_backup_id, p_backup_time, UPPER(p_backup_type), 'DATA_PUMP',
            p_directory_name, p_dump_file, p_log_file, p_status, p_note, p_error_message
        );
        COMMIT;
    END LOG_BACKUP;

    PROCEDURE LOG_RECOVERY(
        p_target_table      IN VARCHAR2,
        p_target_key        IN VARCHAR2,
        p_audit_event_time  IN TIMESTAMP,
        p_flashback_time    IN TIMESTAMP,
        p_status            IN VARCHAR2,
        p_note              IN VARCHAR2,
        p_error_message     IN VARCHAR2 DEFAULT NULL
    ) AS
        PRAGMA AUTONOMOUS_TRANSACTION;
        v_recovery_id NUMBER;
    BEGIN
        /*
           Use dynamic SQL here to avoid compile-time ORA-00942 when this package
           is created by HOSPITAL_DBA inside schema HOSPITAL.
           Runtime object is still HOSPITAL.RECOVERY_HISTORY.
        */
        EXECUTE IMMEDIATE
            'SELECT NVL(MAX(RECOVERY_ID), 0) + 1 FROM hospital.RECOVERY_HISTORY'
            INTO v_recovery_id;

        EXECUTE IMMEDIATE '
            INSERT INTO hospital.RECOVERY_HISTORY(
                RECOVERY_ID, RECOVERY_TIME, TARGET_TABLE, TARGET_KEY,
                AUDIT_EVENT_TIME, FLASHBACK_TIME, STATUS, NOTE, ERROR_MESSAGE
            ) VALUES (
                :1, SYSTIMESTAMP, :2, :3, :4, :5, :6, :7, :8
            )'
        USING
            v_recovery_id, p_target_table, p_target_key,
            p_audit_event_time, p_flashback_time, p_status, p_note, p_error_message;

        COMMIT;
    END LOG_RECOVERY;

    PROCEDURE USP_BACKUP_DATAPUMP(
        p_backup_type    IN VARCHAR2 DEFAULT 'MANUAL',
        p_directory_name IN VARCHAR2 DEFAULT 'HOSPITAL_BACKUP_DIR'
    ) AS
        v_handle      NUMBER;
        v_job_state   VARCHAR2(30);
        v_backup_id   NUMBER;
        v_time        TIMESTAMP;
        v_stamp       VARCHAR2(30);
        v_dump_file   VARCHAR2(255);
        v_log_file    VARCHAR2(255);
    BEGIN
        v_time := SYSTIMESTAMP;
        SELECT hospital.SEQ_BACKUP_ID.NEXTVAL INTO v_backup_id FROM dual;

        v_stamp := TO_CHAR(SYSDATE, 'YYYYMMDD_HH24MISS');
        v_dump_file := 'hospital_schema_' || v_backup_id || '_' || v_stamp || '.dmp';
        v_log_file  := 'hospital_schema_' || v_backup_id || '_' || v_stamp || '.log';

        v_handle := DBMS_DATAPUMP.OPEN(
            operation => 'EXPORT',
            job_mode  => 'SCHEMA',
            job_name  => 'HOSPITAL_EXP_' || v_backup_id
        );

        DBMS_DATAPUMP.ADD_FILE(
            handle    => v_handle,
            filename  => v_dump_file,
            directory => p_directory_name,
            filetype  => DBMS_DATAPUMP.KU$_FILE_TYPE_DUMP_FILE
        );

        DBMS_DATAPUMP.ADD_FILE(
            handle    => v_handle,
            filename  => v_log_file,
            directory => p_directory_name,
            filetype  => DBMS_DATAPUMP.KU$_FILE_TYPE_LOG_FILE
        );

        DBMS_DATAPUMP.METADATA_FILTER(
            handle => v_handle,
            name   => 'SCHEMA_EXPR',
            value  => '= ''HOSPITAL'''
        );

        -- Exclude statistics and PL/SQL metadata to make the backup extremely fast
        DBMS_DATAPUMP.METADATA_FILTER(
            handle => v_handle,
            name   => 'EXCLUDE_PATH_EXPR',
            value  => 'IN (''STATISTICS'', ''PROCEDURE'', ''FUNCTION'', ''PACKAGE'', ''VIEW'', ''GRANT'', ''TYPE'')'
        );

        DBMS_DATAPUMP.START_JOB(v_handle);
        DBMS_DATAPUMP.WAIT_FOR_JOB(v_handle, v_job_state);

        LOG_BACKUP(
            p_backup_id      => v_backup_id,
            p_backup_time    => v_time,
            p_backup_type    => p_backup_type,
            p_directory_name => p_directory_name,
            p_dump_file      => v_dump_file,
            p_log_file       => v_log_file,
            p_status         => CASE WHEN v_job_state = 'COMPLETED' THEN 'SUCCESS' ELSE 'WARNING' END,
            p_note           => 'Data Pump schema export finished. Job state = ' || v_job_state
        );

    EXCEPTION
        WHEN OTHERS THEN
            BEGIN
                IF v_handle IS NOT NULL THEN
                    DBMS_DATAPUMP.DETACH(v_handle);
                END IF;
            EXCEPTION
                WHEN OTHERS THEN NULL;
            END;

            IF v_backup_id IS NULL THEN
                SELECT hospital.SEQ_BACKUP_ID.NEXTVAL INTO v_backup_id FROM dual;
            END IF;

            LOG_BACKUP(
                p_backup_id      => v_backup_id,
                p_backup_time    => NVL(v_time, SYSTIMESTAMP),
                p_backup_type    => p_backup_type,
                p_directory_name => p_directory_name,
                p_dump_file      => v_dump_file,
                p_log_file       => v_log_file,
                p_status         => 'FAILED',
                p_note           => 'Data Pump schema export failed.',
                p_error_message  => SQLERRM
            );
            RAISE;
    END USP_BACKUP_DATAPUMP;

    PROCEDURE USP_MANUAL_BACKUP AS
    BEGIN
        USP_BACKUP_DATAPUMP('MANUAL', 'HOSPITAL_BACKUP_DIR');
    END USP_MANUAL_BACKUP;

    PROCEDURE USP_AUTO_BACKUP AS
    BEGIN
        USP_BACKUP_DATAPUMP('AUTO', 'HOSPITAL_BACKUP_DIR');
    END USP_AUTO_BACKUP;

    PROCEDURE USP_RESTORE_PRESCRIPTION_BY_AUDIT(
        p_record_id         IN VARCHAR2,
        p_audit_event_time  IN TIMESTAMP DEFAULT NULL,
        p_seconds_before    IN NUMBER DEFAULT 1
    ) AS
        v_audit_time      TIMESTAMP;
        v_flashback_time  TIMESTAMP;
        v_old_count       NUMBER;
    BEGIN
        IF p_record_id IS NULL THEN
            RAISE_APPLICATION_ERROR(-20001, 'p_record_id must not be null.');
        END IF;

        -- If GUI passes a selected audit timestamp, use it.
        -- Otherwise, use the latest Unified Audit record for UPDATE/DELETE on HOSPITAL.PRESCRIPTION.
        IF p_audit_event_time IS NOT NULL THEN
            v_audit_time := p_audit_event_time;
        ELSE
            EXECUTE IMMEDIATE '
                SELECT MAX(event_timestamp)
                FROM unified_audit_trail
                WHERE object_schema = ''HOSPITAL''
                  AND object_name = ''PRESCRIPTION''
                  AND action_name IN (''UPDATE'', ''DELETE'')'
            INTO v_audit_time;
        END IF;

        IF v_audit_time IS NULL THEN
            RAISE_APPLICATION_ERROR(-20002, 'No Unified Audit record found for HOSPITAL.PRESCRIPTION UPDATE/DELETE.');
        END IF;

        v_flashback_time := v_audit_time - NUMTODSINTERVAL(NVL(p_seconds_before, 1), 'SECOND');

        SELECT COUNT(*)
        INTO v_old_count
        FROM hospital.PRESCRIPTION AS OF TIMESTAMP v_flashback_time
        WHERE RECORD_ID = p_record_id;

        IF v_old_count = 0 THEN
            RAISE_APPLICATION_ERROR(
                -20003,
                'No flashback data found for RECORD_ID = ' || p_record_id ||
                ' at ' || TO_CHAR(v_flashback_time, 'YYYY-MM-DD HH24:MI:SS')
            );
        END IF;

        -- Restore the full prescription set of this medical record to the state before incident.
        DELETE FROM hospital.PRESCRIPTION
        WHERE RECORD_ID = p_record_id;

        INSERT INTO hospital.PRESCRIPTION(
            RECORD_ID,
            PRESCRIPTION_DATE,
            MEDICINE_NAME,
            DOSAGE
        )
        SELECT
            RECORD_ID,
            PRESCRIPTION_DATE,
            MEDICINE_NAME,
            DOSAGE
        FROM hospital.PRESCRIPTION AS OF TIMESTAMP v_flashback_time
        WHERE RECORD_ID = p_record_id;

        COMMIT;

        LOG_RECOVERY(
            p_target_table     => 'PRESCRIPTION',
            p_target_key       => p_record_id,
            p_audit_event_time => v_audit_time,
            p_flashback_time   => v_flashback_time,
            p_status           => 'SUCCESS',
            p_note             => 'Restored by Flashback Query based on Unified Audit timestamp.'
        );

    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            LOG_RECOVERY(
                p_target_table     => 'PRESCRIPTION',
                p_target_key       => p_record_id,
                p_audit_event_time => v_audit_time,
                p_flashback_time   => v_flashback_time,
                p_status           => 'FAILED',
                p_note             => 'Flashback recovery failed. Use nearest Data Pump dump file if UNDO is no longer available.',
                p_error_message    => SQLERRM
            );
            RAISE;
    END USP_RESTORE_PRESCRIPTION_BY_AUDIT;

    PROCEDURE USP_SIMULATE_WRONG_UPDATE AS
    BEGIN
        UPDATE hospital.PRESCRIPTION
        SET MEDICINE_NAME = N'ERROR_DATA',
            DOSAGE        = N'999 VIEN/NGAY'
        WHERE RECORD_ID = 'BA000001';
        COMMIT;
    END USP_SIMULATE_WRONG_UPDATE;

    PROCEDURE USP_SIMULATE_DELETE AS
    BEGIN
        DELETE FROM hospital.PRESCRIPTION
        WHERE RECORD_ID = 'BA000001';
        COMMIT;
    END USP_SIMULATE_DELETE;

END PKG_BACKUP_RECOVERY;
/

SHOW ERRORS PACKAGE BODY hospital.PKG_BACKUP_RECOVERY;

-- ==============================================================================
-- 5. COMPATIBILITY WRAPPERS FOR WINFORMS
-- ==============================================================================
CREATE OR REPLACE PROCEDURE hospital.USP_MANUAL_BACKUP AS
BEGIN
    hospital.PKG_BACKUP_RECOVERY.USP_MANUAL_BACKUP;
END;
/
SHOW ERRORS PROCEDURE hospital.USP_MANUAL_BACKUP;

CREATE OR REPLACE PROCEDURE hospital.USP_AUTO_BACKUP AS
BEGIN
    hospital.PKG_BACKUP_RECOVERY.USP_AUTO_BACKUP;
END;
/
SHOW ERRORS PROCEDURE hospital.USP_AUTO_BACKUP;

CREATE OR REPLACE PROCEDURE hospital.USP_RESTORE_PRESCRIPTION_BY_AUDIT(
    p_record_id         IN VARCHAR2,
    p_audit_event_time  IN TIMESTAMP DEFAULT NULL,
    p_seconds_before    IN NUMBER DEFAULT 1
) AS
BEGIN
    hospital.PKG_BACKUP_RECOVERY.USP_RESTORE_PRESCRIPTION_BY_AUDIT(
        p_record_id        => p_record_id,
        p_audit_event_time => p_audit_event_time,
        p_seconds_before   => p_seconds_before
    );
END;
/
SHOW ERRORS PROCEDURE hospital.USP_RESTORE_PRESCRIPTION_BY_AUDIT;

CREATE OR REPLACE PROCEDURE hospital.USP_SIMULATE_WRONG_UPDATE AS
BEGIN
    hospital.PKG_BACKUP_RECOVERY.USP_SIMULATE_WRONG_UPDATE;
END;
/
SHOW ERRORS PROCEDURE hospital.USP_SIMULATE_WRONG_UPDATE;

CREATE OR REPLACE PROCEDURE hospital.USP_SIMULATE_DELETE AS
BEGIN
    hospital.PKG_BACKUP_RECOVERY.USP_SIMULATE_DELETE;
END;
/
SHOW ERRORS PROCEDURE hospital.USP_SIMULATE_DELETE;

-- ==============================================================================
-- 6. UNIFIED AUDIT POLICY FOR REQUIREMENT 3 + RECOVERY TIMESTAMP
-- ==============================================================================
BEGIN
    EXECUTE IMMEDIATE 'NOAUDIT POLICY audit_prescription_recovery_policy';
EXCEPTION
    WHEN OTHERS THEN NULL;
END;
/

BEGIN
    EXECUTE IMMEDIATE 'DROP AUDIT POLICY audit_prescription_recovery_policy';
EXCEPTION
    WHEN OTHERS THEN NULL;
END;
/

BEGIN
    EXECUTE IMMEDIATE '
        CREATE AUDIT POLICY audit_prescription_recovery_policy
        ACTIONS UPDATE ON hospital.PRESCRIPTION,
                DELETE ON hospital.PRESCRIPTION';

    EXECUTE IMMEDIATE 'AUDIT POLICY audit_prescription_recovery_policy';

    DBMS_OUTPUT.PUT_LINE('[OK] Unified Audit Policy enabled for PRESCRIPTION UPDATE/DELETE');
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('[WARNING] Unified Audit Policy setup failed: ' || SQLERRM);
END;
/

-- Optional: audit HSBA and HSBA_DV illegal changes if your Requirement 3 scripts have not created them yet.
-- Keep this separated from the core recovery demo.

-- ==============================================================================
-- 7. DBMS_SCHEDULER AUTO BACKUP JOB
-- ==============================================================================
BEGIN
    DBMS_SCHEDULER.DROP_JOB(
        job_name => 'HOSPITAL.AUTO_BACKUP_JOB',
        force    => TRUE
    );
EXCEPTION
    WHEN OTHERS THEN NULL;
END;
/

BEGIN
    DBMS_SCHEDULER.CREATE_JOB(
        job_name        => 'HOSPITAL.AUTO_BACKUP_JOB',
        job_type        => 'PLSQL_BLOCK',
        job_action      => 'BEGIN hospital.USP_AUTO_BACKUP; END;',
        start_date      => SYSTIMESTAMP,
        repeat_interval => 'FREQ=DAILY;BYHOUR=2;BYMINUTE=0;BYSECOND=0',
        enabled         => FALSE,
        comments        => 'Automatic Data Pump schema backup for HOSPITAL. Enable when demo environment is ready.'
    );

    DBMS_OUTPUT.PUT_LINE('[OK] AUTO_BACKUP_JOB created. Default status: DISABLED.');
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('[WARNING] Cannot create AUTO_BACKUP_JOB: ' || SQLERRM);
END;
/

-- For demo only, enable manually when needed:
-- BEGIN DBMS_SCHEDULER.ENABLE('HOSPITAL.AUTO_BACKUP_JOB'); END;
-- /
-- BEGIN DBMS_SCHEDULER.RUN_JOB('HOSPITAL.AUTO_BACKUP_JOB'); END;
-- /

