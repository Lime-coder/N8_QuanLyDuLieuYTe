-- ==============================================================================
-- BackupRecoveryGrants.sql
-- Run as: SYS AS SYSDBA
-- Container: PDB_QLYT
--
-- Purpose:
--   Prepare Oracle privileges and the Data Pump directory for Requirement 4.
--
-- Windows prerequisite:
--   Create the physical folder first and grant write permission to Oracle service:
--     New-Item -ItemType Directory -Force C:\OracleBackups
--     icacls "C:\OracleBackups" /grant "NT SERVICE\OracleServiceXE:(OI)(CI)F"
-- ==============================================================================

-- Data Pump directory object. The path must exist on the database server machine.
CREATE OR REPLACE DIRECTORY HOSPITAL_BACKUP_DIR AS 'C:\OracleBackups';

GRANT READ, WRITE ON DIRECTORY HOSPITAL_BACKUP_DIR TO HOSPITAL;
GRANT READ, WRITE ON DIRECTORY HOSPITAL_BACKUP_DIR TO HOSPITAL_DBA;

-- DBMS_DATAPUMP is used by HOSPITAL.PKG_BACKUP_RECOVERY for export.
GRANT EXECUTE ON SYS.DBMS_DATAPUMP TO HOSPITAL;
GRANT EXECUTE ON SYS.DBMS_DATAPUMP TO HOSPITAL_DBA;

-- Data Pump command-line roles for expdp/impdp scripts.
-- Oracle versions expose either EXP/IMP_FULL_DATABASE or DATAPUMP_* roles.
-- Grant every available role so REMAP_SCHEMA import can run as HOSPITAL_DBA.
DECLARE
    PROCEDURE GRANT_ROLE_IF_EXISTS(p_role IN VARCHAR2, p_grantee IN VARCHAR2) AS
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

-- Scheduler privileges for auto backup jobs created from the DBA UI.
GRANT CREATE JOB TO HOSPITAL_DBA;
GRANT CREATE ANY JOB TO HOSPITAL_DBA;
GRANT MANAGE SCHEDULER TO HOSPITAL_DBA;

-- HOSPITAL_DBA creates Requirement 4 objects under schema HOSPITAL.
GRANT CREATE ANY TABLE TO HOSPITAL_DBA;
GRANT CREATE ANY SEQUENCE TO HOSPITAL_DBA;
GRANT CREATE ANY PROCEDURE TO HOSPITAL_DBA;

-- HOSPITAL_DBA creates and resets HOSPITAL_RESTORE during Data Pump import tests.
GRANT CREATE USER TO HOSPITAL_DBA;
GRANT ALTER USER TO HOSPITAL_DBA;
GRANT DROP USER TO HOSPITAL_DBA;
GRANT CREATE TABLESPACE TO HOSPITAL_DBA;

-- Object access used by the WinForms DBA module and restore verification scripts.
GRANT SELECT ANY TABLE TO HOSPITAL_DBA;
GRANT INSERT ANY TABLE TO HOSPITAL_DBA;
GRANT UPDATE ANY TABLE TO HOSPITAL_DBA;
GRANT DELETE ANY TABLE TO HOSPITAL_DBA;
GRANT SELECT ANY SEQUENCE TO HOSPITAL_DBA;
GRANT EXECUTE ANY PROCEDURE TO HOSPITAL_DBA;

-- Tablespace quota for owner schemas.
GRANT CREATE SESSION TO HOSPITAL;
GRANT CREATE TABLE TO HOSPITAL;
GRANT UNLIMITED TABLESPACE TO HOSPITAL;
GRANT UNLIMITED TABLESPACE TO HOSPITAL_DBA;

-- Flashback Query recovery demo.
GRANT FLASHBACK ANY TABLE TO HOSPITAL;

-- Unified Audit access/policy management. The policy is created in BackupRecoverySetup.sql.
GRANT AUDIT_ADMIN TO HOSPITAL_DBA;
GRANT AUDIT_VIEWER TO HOSPITAL_DBA;
GRANT AUDIT_VIEWER TO HOSPITAL;

PROMPT [OK] BackupRecovery privileges and HOSPITAL_BACKUP_DIR are ready.
