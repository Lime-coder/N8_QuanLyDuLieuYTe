-- ==============================================================================
-- 01_create_roles.sql
-- Cháº¡y dÆ°á»›i quyá»n: sysdba
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;

BEGIN
    FOR r IN (
        SELECT role FROM dba_roles
        WHERE role IN ('RL_COORDINATOR','RL_DOCTOR','RL_TECHNICIAN','RL_PATIENT')
    ) LOOP
        EXECUTE IMMEDIATE 'DROP ROLE ' || r.role;
    END LOOP;
EXCEPTION 
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE(SQLERRM);
END;
/

CREATE ROLE rl_coordinator;
CREATE ROLE rl_doctor;
CREATE ROLE rl_technician;
CREATE ROLE rl_patient;
