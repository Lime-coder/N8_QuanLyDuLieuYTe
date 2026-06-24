-- ==============================================================================
-- AIO_01_SYSDBA.sql
-- Run as: SYS AS SYSDBA
-- Container: PDB_QLYT (unless specified otherwise)
-- ==============================================================================

-- ==============================================================================
-- Source: 03_Audit\PhanQuyen_1.sql
-- ==============================================================================

-- Warning: This script switches to CDB$ROOT
-- Run as: SYSDBA
-- Chuyển về vùng root
ALTER SESSION SET CONTAINER = CDB$ROOT;

-- Kích hoạt kiểm toán hệ thống (Yêu cầu 3.1)
ALTER SYSTEM SET audit_trail = DB, EXTENDED SCOPE = SPFILE;


-- Restore container back to PDB_QLYT for subsequent scripts
ALTER SESSION SET CONTAINER = PDB_QLYT;


-- ==============================================================================
-- Source: 01_Combined_RQ1\00_SYSDBA_Prereq_For_All_Roles.sql
-- ==============================================================================

-- ==============================================================================
-- Run as: SYS AS SYSDB
-- Connect to: PDB_QLYT
-- ===============================================================================

SET SERVEROUTPUT ON;

PROMPT ==============================================================================
PROMPT START: SYSDBA prereq for all-role setup
PROMPT ==============================================================================

-- ==============================================================================
-- 1. Allow HOSPITAL_DBA to manage VPD/RLS policies
-- ==============================================================================
GRANT EXECUTE ON SYS.DBMS_RLS TO HOSPITAL_DBA;

-- ==============================================================================
-- 2. Allow HOSPITAL_DBA to create/drop/alter objects across schemas used by setup
-- ==============================================================================
GRANT CREATE ANY TABLE     TO HOSPITAL_DBA;
GRANT CREATE ANY VIEW      TO HOSPITAL_DBA;
GRANT CREATE ANY PROCEDURE TO HOSPITAL_DBA;
GRANT CREATE ANY TRIGGER   TO HOSPITAL_DBA;

GRANT DROP ANY TABLE       TO HOSPITAL_DBA;
GRANT DROP ANY VIEW        TO HOSPITAL_DBA;
GRANT DROP ANY PROCEDURE   TO HOSPITAL_DBA;
GRANT DROP ANY TRIGGER     TO HOSPITAL_DBA;

GRANT ALTER ANY TABLE      TO HOSPITAL_DBA;
GRANT GRANT ANY OBJECT PRIVILEGE TO HOSPITAL_DBA;

-- ==============================================================================
-- 3. Direct table grants needed by views/procedures owned by HOSPITAL_DBA
--    and by definer-rights compilation.
--    Roles are not enough for stored procedure compilation in Oracle.
-- ==============================================================================
GRANT SELECT, INSERT, UPDATE, DELETE ON HOSPITAL.PATIENT        TO HOSPITAL_DBA WITH GRANT OPTION;
GRANT SELECT, INSERT, UPDATE, DELETE ON HOSPITAL.MEDICAL_RECORD TO HOSPITAL_DBA WITH GRANT OPTION;
GRANT SELECT, INSERT, UPDATE, DELETE ON HOSPITAL.SERVICE_RECORD TO HOSPITAL_DBA WITH GRANT OPTION;
GRANT SELECT, INSERT, UPDATE, DELETE ON HOSPITAL.PRESCRIPTION   TO HOSPITAL_DBA WITH GRANT OPTION;

GRANT SELECT, UPDATE ON HOSPITAL.STAFF      TO HOSPITAL_DBA WITH GRANT OPTION;
GRANT SELECT         ON HOSPITAL.DEPARTMENT TO HOSPITAL_DBA WITH GRANT OPTION;

-- Needed because coordinator setup may modify SERVICE_RECORD.TECHNICIAN_ID NULL.
GRANT ALTER ON HOSPITAL.SERVICE_RECORD TO HOSPITAL_DBA;

-- ==============================================================================
-- 4. Quick privilege sanity check
-- ==============================================================================
COLUMN grantee FORMAT A20
COLUMN privilege FORMAT A35
COLUMN owner FORMAT A20
COLUMN table_name FORMAT A35

PROMPT System privileges granted to HOSPITAL_DBA relevant to setup:
SELECT grantee, privilege
FROM dba_sys_privs
WHERE grantee = 'HOSPITAL_DBA'
  AND privilege IN (
    'CREATE ANY TABLE', 'CREATE ANY VIEW', 'CREATE ANY PROCEDURE', 'CREATE ANY TRIGGER',
    'DROP ANY TABLE', 'DROP ANY VIEW', 'DROP ANY PROCEDURE', 'DROP ANY TRIGGER',
    'ALTER ANY TABLE', 'GRANT ANY OBJECT PRIVILEGE'
  )
ORDER BY privilege;

PROMPT Direct object privileges from HOSPITAL to HOSPITAL_DBA relevant to setup:
SELECT grantee, owner, table_name, privilege, grantable
FROM dba_tab_privs
WHERE grantee = 'HOSPITAL_DBA'
  AND owner = 'HOSPITAL'
  AND table_name IN ('PATIENT', 'MEDICAL_RECORD', 'SERVICE_RECORD', 'PRESCRIPTION', 'STAFF', 'DEPARTMENT')
ORDER BY table_name, privilege;

PROMPT ==============================================================================
PROMPT DONE: SYSDBA prereq finished
PROMPT ==============================================================================



-- ==============================================================================
-- Source: 02_OLS\OLS_AIO_1_SYSDBA.sql
-- ==============================================================================

-- ==============================================================================
-- File: OLS_AIO_1_SYSDBA.sql
-- Run as: SYS AS SYSDBA 
-- ==============================================================================
SET SERVEROUTPUT ON;

PROMPT ==============================================================================
PROMPT 1. Switch to CDB$ROOT to unlock global LBACSYS account
PROMPT ==============================================================================
ALTER SESSION SET CONTAINER = CDB$ROOT;
ALTER USER lbacsys IDENTIFIED BY Lbacsys#2026 ACCOUNT UNLOCK CONTAINER=ALL;

PROMPT ==============================================================================
PROMPT 2. Switch to PDB_QLYT to configure OLS for the project
PROMPT ==============================================================================
ALTER SESSION SET CONTAINER = PDB_QLYT;

GRANT LBAC_DBA TO hospital_dba;
GRANT EXECUTE ON sa_sysdba TO hospital_dba;
GRANT EXECUTE ON sa_components TO hospital_dba;
GRANT EXECUTE ON sa_label_admin TO hospital_dba;
GRANT EXECUTE ON sa_policy_admin TO hospital_dba;
GRANT EXECUTE ON sa_user_admin TO hospital_dba;

-- Allow hospital_dba to read user OLS privilege
GRANT SELECT ON DBA_SA_USERS TO hospital_dba;

PROMPT ==============================================================================
PROMPT 3. Enable OLS Kernel and Restart PDB
PROMPT ==============================================================================
EXEC LBACSYS.CONFIGURE_OLS;
EXEC LBACSYS.OLS_ENFORCEMENT.ENABLE_OLS;

ALTER PLUGGABLE DATABASE CLOSE IMMEDIATE;
ALTER PLUGGABLE DATABASE OPEN;

PROMPT ==============================================================================
PROMPT DONE: SYSDBA setup complete. 
PROMPT Disconnect and connect as HOSPITAL_DBA for the next script.
PROMPT ==============================================================================


-- ==============================================================================
-- Source: 04_BackupRecovery\BackupRecoveryGrants.sql
-- ==============================================================================

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

GRANT READ, WRITE ON DIRECTORY HOSPITAL_BACKUP_DIR TO HOSPITAL;
GRANT READ, WRITE ON DIRECTORY HOSPITAL_BACKUP_DIR TO HOSPITAL_DBA;


-- ==============================================================================
-- 2. BASIC SESSION AND TABLESPACE PRIVILEGES
-- ==============================================================================

GRANT CREATE SESSION TO HOSPITAL;
GRANT CREATE TABLE TO HOSPITAL;
GRANT UNLIMITED TABLESPACE TO HOSPITAL;

GRANT CREATE SESSION TO HOSPITAL_DBA;
GRANT UNLIMITED TABLESPACE TO HOSPITAL_DBA;


-- ==============================================================================
-- 3. PRIVILEGES FOR CREATING REQUIREMENT 4 OBJECTS UNDER HOSPITAL SCHEMA
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

GRANT EXECUTE ON SYS.DBMS_DATAPUMP TO HOSPITAL;
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
    GRANT_ROLE_IF_EXISTS('EXP_FULL_DATABASE', 'HOSPITAL');
    GRANT_ROLE_IF_EXISTS('DATAPUMP_EXP_FULL_DATABASE', 'HOSPITAL');

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

GRANT FLASHBACK ANY TABLE TO HOSPITAL;
GRANT FLASHBACK ANY TABLE TO HOSPITAL_DBA;


-- ==============================================================================
-- 9. UNIFIED AUDIT PRIVILEGES
-- ==============================================================================

GRANT AUDIT_ADMIN TO HOSPITAL_DBA;
GRANT AUDIT_VIEWER TO HOSPITAL_DBA;
GRANT AUDIT_VIEWER TO HOSPITAL;

-- Direct SELECT grant is needed because stored PL/SQL does not use privileges
-- obtained only through roles.
GRANT SELECT ON UNIFIED_AUDIT_TRAIL TO HOSPITAL_DBA;


-- ==============================================================================
-- 10. UNIFIED AUDIT POLICY FOR ROW-LEVEL FLASHBACK RECOVERY
-- ==============================================================================
-- This section requires the audited HOSPITAL tables to already exist.
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
    INSERT ON HOSPITAL.PATIENT,
    UPDATE ON HOSPITAL.PATIENT,
    DELETE ON HOSPITAL.PATIENT,
    INSERT ON HOSPITAL.MEDICAL_RECORD,
    UPDATE ON HOSPITAL.MEDICAL_RECORD,
    DELETE ON HOSPITAL.MEDICAL_RECORD;

AUDIT POLICY audit_row_recovery_policy WHENEVER SUCCESSFUL;



