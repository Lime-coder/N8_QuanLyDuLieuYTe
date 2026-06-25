-- ==============================================================================
-- BackupRecoveryGrants.sql
-- Run as     : SYS AS SYSDBA
-- Container  : PDB_QLYT
-- Purpose    : Prepare privileges, Data Pump directory, Scheduler privileges,
--              Flashback Query access, and Unified Audit policy for Requirement 4.
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
SET SERVEROUTPUT ON;

-- ==============================================================================
-- 1. DATA PUMP DIRECTORY
-- ==============================================================================
-- Windows prerequisite:
-- Create the physical folder first and grant write permission to Oracle service:
--   New-Item -ItemType Directory -Force C:\OracleBackups
--   icacls "C:\OracleBackups" /grant "NT SERVICE\OracleServiceXE:(OI)(CI)F"
-- ==============================================================================

CREATE OR REPLACE DIRECTORY HOSPITAL_BACKUP_DIR AS 'C:\OracleBackups';

GRANT READ, WRITE ON DIRECTORY HOSPITAL_BACKUP_DIR TO HOSPITAL_DBA;
GRANT READ, WRITE ON DIRECTORY HOSPITAL_BACKUP_DIR TO HOSPITAL_DBA;


-- ==============================================================================
-- 2. BASIC SESSION AND TABLESPACE PRIVILEGES
-- ==============================================================================

GRANT CREATE SESSION TO HOSPITAL_DBA;
GRANT CREATE TABLE TO HOSPITAL_DBA;
GRANT UNLIMITED TABLESPACE TO HOSPITAL_DBA;

GRANT CREATE SESSION TO HOSPITAL_DBA;
GRANT UNLIMITED TABLESPACE TO HOSPITAL_DBA;


-- ==============================================================================
-- 3. PRIVILEGES FOR CREATING REQUIREMENT 4 OBJECTS UNDER HOSPITAL_DBA SCHEMA
-- ==============================================================================

GRANT CREATE ANY TABLE TO HOSPITAL_DBA;
GRANT CREATE ANY SEQUENCE TO HOSPITAL_DBA;
GRANT CREATE ANY PROCEDURE TO HOSPITAL_DBA;
GRANT ALTER ANY PROCEDURE TO HOSPITAL_DBA;


-- ==============================================================================
-- 4. OBJECT ACCESS FOR DBA MODULE AND RESTORE VERIFICATION
-- ==============================================================================

GRANT SELECT ANY TABLE TO HOSPITAL_DBA;
GRANT INSERT ANY TABLE TO HOSPITAL_DBA;
GRANT UPDATE ANY TABLE TO HOSPITAL_DBA;
GRANT DELETE ANY TABLE TO HOSPITAL_DBA;

GRANT SELECT ANY SEQUENCE TO HOSPITAL_DBA;
GRANT EXECUTE ANY PROCEDURE TO HOSPITAL_DBA;


-- ==============================================================================
-- 5. DBMS_DATAPUMP AND DATA PUMP ROLES
-- ==============================================================================

GRANT EXECUTE ON SYS.DBMS_DATAPUMP TO HOSPITAL_DBA;
GRANT EXECUTE ON SYS.DBMS_DATAPUMP TO HOSPITAL_DBA;

DECLARE
    PROCEDURE GRANT_ROLE_IF_EXISTS(
        p_role    IN VARCHAR2,
        p_grantee IN VARCHAR2
    ) AS
        v_count NUMBER;
    BEGIN
        SELECT COUNT(*)
        INTO v_count
        FROM DBA_ROLES
        WHERE ROLE = UPPER(p_role);

        IF v_count > 0 THEN
            EXECUTE IMMEDIATE 'GRANT ' || p_role || ' TO ' || p_grantee;
            DBMS_OUTPUT.PUT_LINE('[OK] Granted ' || p_role || ' to ' || p_grantee);
        ELSE
            DBMS_OUTPUT.PUT_LINE('[SKIP] Role does not exist: ' || p_role);
        END IF;
    END;
BEGIN
    GRANT_ROLE_IF_EXISTS('EXP_FULL_DATABASE', 'HOSPITAL_DBA');
    GRANT_ROLE_IF_EXISTS('DATAPUMP_EXP_FULL_DATABASE', 'HOSPITAL_DBA');

    GRANT_ROLE_IF_EXISTS('EXP_FULL_DATABASE', 'HOSPITAL_DBA');
    GRANT_ROLE_IF_EXISTS('IMP_FULL_DATABASE', 'HOSPITAL_DBA');
    GRANT_ROLE_IF_EXISTS('DATAPUMP_EXP_FULL_DATABASE', 'HOSPITAL_DBA');
    GRANT_ROLE_IF_EXISTS('DATAPUMP_IMP_FULL_DATABASE', 'HOSPITAL_DBA');
END;
/

ALTER USER HOSPITAL_DBA DEFAULT ROLE ALL;


-- ==============================================================================
-- 6. DBMS_SCHEDULER PRIVILEGES FOR AUTOMATIC BACKUP JOBS
-- ==============================================================================

GRANT CREATE JOB TO HOSPITAL_DBA;
GRANT CREATE ANY JOB TO HOSPITAL_DBA;
GRANT MANAGE SCHEDULER TO HOSPITAL_DBA;


-- ==============================================================================
-- 7. USER MANAGEMENT PRIVILEGES FOR DATA PUMP IMPORT TESTS
-- ==============================================================================

GRANT CREATE USER TO HOSPITAL_DBA;
GRANT ALTER USER TO HOSPITAL_DBA;
GRANT DROP USER TO HOSPITAL_DBA;
GRANT CREATE TABLESPACE TO HOSPITAL_DBA;


-- ==============================================================================
-- 8. FLASHBACK QUERY PRIVILEGES
-- ==============================================================================

GRANT FLASHBACK ANY TABLE TO HOSPITAL_DBA;
GRANT FLASHBACK ANY TABLE TO HOSPITAL_DBA;


-- ==============================================================================
-- 9. UNIFIED AUDIT PRIVILEGES
-- ==============================================================================

GRANT AUDIT_ADMIN TO HOSPITAL_DBA;
GRANT AUDIT_VIEWER TO HOSPITAL_DBA;
GRANT AUDIT_VIEWER TO HOSPITAL_DBA;

-- Direct SELECT grant is needed because stored PL/SQL does not use privileges
-- obtained only through roles.
GRANT SELECT ON UNIFIED_AUDIT_TRAIL TO HOSPITAL_DBA;


-- ==============================================================================
-- 10. UNIFIED AUDIT POLICY FOR ROW-LEVEL FLASHBACK RECOVERY
-- ==============================================================================
-- This section requires the audited HOSPITAL_DBA tables to already exist.
-- It audits successful DML so recovery can identify SCN and restore one row.
-- ==============================================================================

BEGIN
    EXECUTE IMMEDIATE 'NOAUDIT POLICY audit_prescription_recovery_policy';
EXCEPTION
    WHEN OTHERS THEN NULL;
END;
/

BEGIN
    EXECUTE IMMEDIATE 'NOAUDIT POLICY audit_row_recovery_policy';
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
    EXECUTE IMMEDIATE 'DROP AUDIT POLICY audit_row_recovery_policy';
EXCEPTION
    WHEN OTHERS THEN NULL;
END;
/

CREATE AUDIT POLICY audit_row_recovery_policy
ACTIONS
    INSERT ON hospital_dba.PATIENT,
    UPDATE ON hospital_dba.PATIENT,
    DELETE ON hospital_dba.PATIENT,
    INSERT ON hospital_dba.MEDICAL_RECORD,
    UPDATE ON hospital_dba.MEDICAL_RECORD,
    DELETE ON hospital_dba.MEDICAL_RECORD,
    INSERT ON hospital_dba.PRESCRIPTION,
    UPDATE ON hospital_dba.PRESCRIPTION,
    DELETE ON hospital_dba.PRESCRIPTION,
    INSERT ON hospital_dba.SERVICE_RECORD,
    UPDATE ON hospital_dba.SERVICE_RECORD,
    DELETE ON hospital_dba.SERVICE_RECORD;

AUDIT POLICY audit_row_recovery_policy WHENEVER SUCCESSFUL;



