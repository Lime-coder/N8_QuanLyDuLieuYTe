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

    PROCEDURE USP_RESTORE_AUDITED_ROW_BY_SCN(
        p_table_name IN VARCHAR2,
        p_audit_scn  IN NUMBER,
        p_key1       IN VARCHAR2,
        p_key2       IN VARCHAR2 DEFAULT NULL,
        p_key3       IN VARCHAR2 DEFAULT NULL
    );
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
        SELECT hospital.SEQ_RECOVERY_ID.NEXTVAL
        INTO v_recovery_id
        FROM dual;

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
            value  => 'IN (''STATISTICS'', ''PROCEDURE'', ''FUNCTION'', ''PACKAGE'', ''VIEW'', ''GRANT'', ''TYPE'', ''TRIGGER'')'
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

    PROCEDURE USP_RESTORE_AUDITED_ROW_BY_SCN(
        p_table_name IN VARCHAR2,
        p_audit_scn  IN NUMBER,
        p_key1       IN VARCHAR2,
        p_key2       IN VARCHAR2 DEFAULT NULL,
        p_key3       IN VARCHAR2 DEFAULT NULL
    ) AS
        v_table_name     VARCHAR2(128) := UPPER(TRIM(p_table_name));
        v_flashback_scn  NUMBER;
        v_audit_time     TIMESTAMP;
        v_flashback_time TIMESTAMP;
        v_past_count     NUMBER;
        v_now_count      NUMBER;
        v_target_key     VARCHAR2(1000);
    BEGIN
        IF p_audit_scn IS NULL OR p_audit_scn <= 1 THEN
            RAISE_APPLICATION_ERROR(-20001, 'p_audit_scn must be greater than 1.');
        END IF;

        IF p_key1 IS NULL THEN
            RAISE_APPLICATION_ERROR(-20002, 'p_key1 must not be null.');
        END IF;

        v_flashback_scn := p_audit_scn - 1;
        v_target_key := p_key1;

        BEGIN v_audit_time := SCN_TO_TIMESTAMP(p_audit_scn); EXCEPTION WHEN OTHERS THEN v_audit_time := NULL; END;
        BEGIN v_flashback_time := SCN_TO_TIMESTAMP(v_flashback_scn); EXCEPTION WHEN OTHERS THEN v_flashback_time := NULL; END;

        IF v_table_name = 'PATIENT' THEN
            SELECT COUNT(*) INTO v_past_count FROM hospital.PATIENT AS OF SCN v_flashback_scn WHERE PATIENT_ID = p_key1;
            SELECT COUNT(*) INTO v_now_count FROM hospital.PATIENT WHERE PATIENT_ID = p_key1;

            IF v_past_count = 1 AND v_now_count = 1 THEN
                FOR rec IN (SELECT * FROM hospital.PATIENT AS OF SCN v_flashback_scn WHERE PATIENT_ID = p_key1) LOOP
                    UPDATE hospital.PATIENT
                    SET FULL_NAME = rec.FULL_NAME,
                        GENDER = rec.GENDER,
                        BIRTHDATE = rec.BIRTHDATE,
                        ID_CARD = rec.ID_CARD,
                        HOUSE_NO = rec.HOUSE_NO,
                        STREET = rec.STREET,
                        DISTRICT = rec.DISTRICT,
                        CITY_PROVINCE = rec.CITY_PROVINCE,
                        MEDICAL_HISTORY = rec.MEDICAL_HISTORY,
                        FAMILY_MEDICAL_HISTORY = rec.FAMILY_MEDICAL_HISTORY,
                        DRUG_ALLERGIES = rec.DRUG_ALLERGIES,
                        USERNAME_DB = rec.USERNAME_DB,
                        IS_ACTIVE = rec.IS_ACTIVE
                    WHERE PATIENT_ID = rec.PATIENT_ID;
                END LOOP;
            ELSIF v_past_count = 1 AND v_now_count = 0 THEN
                FOR rec IN (SELECT * FROM hospital.PATIENT AS OF SCN v_flashback_scn WHERE PATIENT_ID = p_key1) LOOP
                    INSERT INTO hospital.PATIENT(
                        PATIENT_ID, FULL_NAME, GENDER, BIRTHDATE, ID_CARD,
                        HOUSE_NO, STREET, DISTRICT, CITY_PROVINCE,
                        MEDICAL_HISTORY, FAMILY_MEDICAL_HISTORY, DRUG_ALLERGIES,
                        USERNAME_DB, IS_ACTIVE
                    ) VALUES (
                        rec.PATIENT_ID, rec.FULL_NAME, rec.GENDER, rec.BIRTHDATE, rec.ID_CARD,
                        rec.HOUSE_NO, rec.STREET, rec.DISTRICT, rec.CITY_PROVINCE,
                        rec.MEDICAL_HISTORY, rec.FAMILY_MEDICAL_HISTORY, rec.DRUG_ALLERGIES,
                        rec.USERNAME_DB, rec.IS_ACTIVE
                    );
                END LOOP;
            ELSIF v_past_count = 0 AND v_now_count = 1 THEN
                DELETE FROM hospital.PATIENT WHERE PATIENT_ID = p_key1;
            ELSE
                RAISE_APPLICATION_ERROR(-20003, 'No current or flashback row found for PATIENT key ' || p_key1);
            END IF;

        ELSIF v_table_name = 'MEDICAL_RECORD' THEN
            SELECT COUNT(*) INTO v_past_count FROM hospital.MEDICAL_RECORD AS OF SCN v_flashback_scn WHERE RECORD_ID = p_key1;
            SELECT COUNT(*) INTO v_now_count FROM hospital.MEDICAL_RECORD WHERE RECORD_ID = p_key1;

            IF v_past_count = 1 AND v_now_count = 1 THEN
                FOR rec IN (SELECT * FROM hospital.MEDICAL_RECORD AS OF SCN v_flashback_scn WHERE RECORD_ID = p_key1) LOOP
                    UPDATE hospital.MEDICAL_RECORD
                    SET PATIENT_ID = rec.PATIENT_ID,
                        RECORD_DATE = rec.RECORD_DATE,
                        DIAGNOSIS = rec.DIAGNOSIS,
                        TREATMENT_PLAN = rec.TREATMENT_PLAN,
                        DOCTOR_ID = rec.DOCTOR_ID,
                        DEPT_ID = rec.DEPT_ID,
                        CONCLUSION = rec.CONCLUSION
                    WHERE RECORD_ID = rec.RECORD_ID;
                END LOOP;
            ELSIF v_past_count = 1 AND v_now_count = 0 THEN
                FOR rec IN (SELECT * FROM hospital.MEDICAL_RECORD AS OF SCN v_flashback_scn WHERE RECORD_ID = p_key1) LOOP
                    INSERT INTO hospital.MEDICAL_RECORD(
                        RECORD_ID, PATIENT_ID, RECORD_DATE, DIAGNOSIS,
                        TREATMENT_PLAN, DOCTOR_ID, DEPT_ID, CONCLUSION
                    ) VALUES (
                        rec.RECORD_ID, rec.PATIENT_ID, rec.RECORD_DATE, rec.DIAGNOSIS,
                        rec.TREATMENT_PLAN, rec.DOCTOR_ID, rec.DEPT_ID, rec.CONCLUSION
                    );
                END LOOP;
            ELSIF v_past_count = 0 AND v_now_count = 1 THEN
                DELETE FROM hospital.MEDICAL_RECORD WHERE RECORD_ID = p_key1;
            ELSE
                RAISE_APPLICATION_ERROR(-20003, 'No current or flashback row found for MEDICAL_RECORD key ' || p_key1);
            END IF;

        ELSE
            RAISE_APPLICATION_ERROR(-20006, 'Unsupported audited table for Flashback demo. Use PATIENT or MEDICAL_RECORD: ' || v_table_name);
        END IF;

        COMMIT;

        LOG_RECOVERY(
            p_target_table     => v_table_name,
            p_target_key       => v_target_key,
            p_audit_event_time => v_audit_time,
            p_flashback_time   => v_flashback_time,
            p_status           => 'SUCCESS',
            p_note             => 'Restored one audited row by Flashback Query AS OF SCN.'
        );

    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            LOG_RECOVERY(
                p_target_table     => NVL(v_table_name, 'UNKNOWN'),
                p_target_key       => v_target_key,
                p_audit_event_time => v_audit_time,
                p_flashback_time   => v_flashback_time,
                p_status           => 'FAILED',
                p_note             => 'Audited row Flashback SCN recovery failed.',
                p_error_message    => SQLERRM
            );
            RAISE;
    END USP_RESTORE_AUDITED_ROW_BY_SCN;

END PKG_BACKUP_RECOVERY;
/

SHOW ERRORS PACKAGE BODY hospital.PKG_BACKUP_RECOVERY;


-- ==============================================================================
-- 6. DATA PUMP IMPORT FOR WINFORMS
-- ==============================================================================
-- This procedure is owned by HOSPITAL_DBA because REMAP_SCHEMA import requires
-- the active DATAPUMP_IMP_FULL_DATABASE role. The target schema is fixed to
-- HOSPITAL_RESTORE so the UI cannot drop or overwrite an arbitrary schema.
-- ==============================================================================
CREATE OR REPLACE PROCEDURE hospital_dba.USP_IMPORT_DATAPUMP_TO_RESTORE(
    p_dump_file           IN  VARCHAR2,
    p_job_state           OUT VARCHAR2,
    p_log_file            OUT VARCHAR2,
    p_table_count         OUT NUMBER,
    p_prescription_count  OUT NUMBER
) AUTHID CURRENT_USER AS
    v_handle      NUMBER;
    v_job_name    VARCHAR2(30);
    v_user_count  NUMBER;
BEGIN
    p_job_state := 'NOT_STARTED';
    p_table_count := 0;
    p_prescription_count := 0;

    IF p_dump_file IS NULL
       OR LENGTH(p_dump_file) > 255
       OR NOT REGEXP_LIKE(p_dump_file, '^[A-Za-z0-9][A-Za-z0-9_.-]*[.]dmp$', 'i')
       OR INSTR(p_dump_file, '..') > 0 THEN
        RAISE_APPLICATION_ERROR(-20020, 'Invalid dump file name. Enter only a .dmp file name.');
    END IF;

    SELECT COUNT(*)
    INTO v_user_count
    FROM DBA_USERS
    WHERE USERNAME = 'HOSPITAL_RESTORE';

    IF v_user_count > 0 THEN
        EXECUTE IMMEDIATE 'DROP USER HOSPITAL_RESTORE CASCADE';
    END IF;

    EXECUTE IMMEDIATE '
        CREATE USER HOSPITAL_RESTORE IDENTIFIED BY "Restore#2026"
        DEFAULT TABLESPACE SYSTEM
        TEMPORARY TABLESPACE TEMP
        QUOTA UNLIMITED ON SYSTEM';

    EXECUTE IMMEDIATE '
        GRANT CREATE SESSION, CREATE TABLE, CREATE VIEW, CREATE SEQUENCE,
              CREATE PROCEDURE, CREATE TRIGGER
        TO HOSPITAL_RESTORE';

    v_job_name := 'HOSP_IMP_' || TO_CHAR(SYSTIMESTAMP, 'YYYYMMDDHH24MISSFF3');
    p_log_file := 'import_gui_' || TO_CHAR(SYSTIMESTAMP, 'YYYYMMDD_HH24MISS') || '.log';

    v_handle := DBMS_DATAPUMP.OPEN(
        operation => 'IMPORT',
        job_mode  => 'SCHEMA',
        job_name  => v_job_name
    );

    DBMS_DATAPUMP.ADD_FILE(
        handle    => v_handle,
        filename  => p_dump_file,
        directory => 'HOSPITAL_BACKUP_DIR',
        filetype  => DBMS_DATAPUMP.KU$_FILE_TYPE_DUMP_FILE
    );

    DBMS_DATAPUMP.ADD_FILE(
        handle    => v_handle,
        filename  => p_log_file,
        directory => 'HOSPITAL_BACKUP_DIR',
        filetype  => DBMS_DATAPUMP.KU$_FILE_TYPE_LOG_FILE
    );

    DBMS_DATAPUMP.METADATA_REMAP(
        handle    => v_handle,
        name      => 'REMAP_SCHEMA',
        old_value => 'HOSPITAL',
        value     => 'HOSPITAL_RESTORE'
    );

    -- No REMAP_TABLESPACE needed since both source and target use SYSTEM tablespace.
    DBMS_DATAPUMP.METADATA_FILTER(
        handle => v_handle,
        name   => 'EXCLUDE_PATH_EXPR',
        value  => 'IN (''USER'', ''SYSTEM_GRANT'', ''ROLE_GRANT'', ''DEFAULT_ROLE'', ''OBJECT_GRANT'', ''PROCACT_SCHEMA'')'
    );

    DBMS_DATAPUMP.SET_PARAMETER(
        handle => v_handle,
        name   => 'TABLE_EXISTS_ACTION',
        value  => 'REPLACE'
    );

    DBMS_DATAPUMP.START_JOB(v_handle);
    DBMS_DATAPUMP.WAIT_FOR_JOB(v_handle, p_job_state);
    v_handle := NULL;

    IF p_job_state <> 'COMPLETED' THEN
        RAISE_APPLICATION_ERROR(-20021, 'Data Pump import ended with state ' || p_job_state);
    END IF;

    SELECT COUNT(*)
    INTO p_table_count
    FROM ALL_TABLES
    WHERE OWNER = 'HOSPITAL_RESTORE';

    EXECUTE IMMEDIATE
        'SELECT COUNT(*) FROM HOSPITAL_RESTORE.PRESCRIPTION'
        INTO p_prescription_count;
EXCEPTION
    WHEN OTHERS THEN
        BEGIN
            IF v_handle IS NOT NULL THEN
                DBMS_DATAPUMP.DETACH(v_handle);
            END IF;
        EXCEPTION
            WHEN OTHERS THEN NULL;
        END;
        RAISE;
END;
/
SHOW ERRORS PROCEDURE hospital_dba.USP_IMPORT_DATAPUMP_TO_RESTORE;


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
        job_type        => 'STORED_PROCEDURE',
        job_action      => 'hospital.PKG_BACKUP_RECOVERY.USP_AUTO_BACKUP',
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

