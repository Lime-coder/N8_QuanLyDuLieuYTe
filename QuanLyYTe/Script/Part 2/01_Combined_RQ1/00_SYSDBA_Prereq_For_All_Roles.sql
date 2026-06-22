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
