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