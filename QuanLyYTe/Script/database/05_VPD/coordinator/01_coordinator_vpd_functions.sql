-- ==============================================================================
-- 02_coordinator_vpd_functions.sql
-- Chạy dưới quyền: hospital_dba
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

CREATE OR REPLACE FUNCTION FN_VPD_STAFF_SELF (
    p_schema VARCHAR2,
    p_obj    VARCHAR2
)
RETURN VARCHAR2
AS
BEGIN
    IF SYS_CONTEXT('USERENV', 'SESSION_USER') IN ('HOSPITAL', 'HOSPITAL_DBA') THEN
        RETURN '1=1';
    END IF;

    RETURN 'UPPER(username_db) = SYS_CONTEXT(''USERENV'', ''SESSION_USER'')';

EXCEPTION
    WHEN OTHERS THEN
        RETURN '1=0';
END;
/
