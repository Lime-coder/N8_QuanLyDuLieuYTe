-- ==============================================================================
-- 02_coordinator_vpd_functions.sql
-- Cháº¡y dÆ°á»›i quyá»n: hospital_dba
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

CREATE OR REPLACE FUNCTION FN_VPD_STAFF_SELF (
    p_schema VARCHAR2,
    p_obj    VARCHAR2
)
RETURN VARCHAR2
AS
    v_current_user VARCHAR2(100);
BEGIN
    v_current_user := SYS_CONTEXT('USERENV', 'SESSION_USER');
    
    IF SYS_CONTEXT('USERENV', 'CURRENT_USER') IN ('HOSPITAL', 'HOSPITAL_DBA') THEN
        RETURN '1=1';
    END IF;

    RETURN 'username_db = SYS_CONTEXT(''USERENV'', ''SESSION_USER'')';

EXCEPTION
    WHEN OTHERS THEN
        RETURN '1=0';
END;
/
