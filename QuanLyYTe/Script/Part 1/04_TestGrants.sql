-- Run as: hospital_dba | Container: PDB_QLYT
-- Creates Oracle users matching mock staff and sets up roles and privileges for testing
-- ALTER SESSION SET CONTAINER = PDB_QLYT;

BEGIN
    FOR u IN (SELECT username FROM all_users
              WHERE username IN ('NV000021','NV000001','NV000121','BN000001')) LOOP
        EXECUTE IMMEDIATE 'DROP USER ' || u.username || ' CASCADE';
    END LOOP;
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

CREATE USER NV000021 IDENTIFIED BY Abc123456;
CREATE USER NV000001 IDENTIFIED BY Abc123456;
CREATE USER NV000121 IDENTIFIED BY Abc123456;
CREATE USER BN000001 IDENTIFIED BY Abc123456;

GRANT EXECUTE ON hospital_dba.USP_GET_SESSION_ROLE TO NV000021;
GRANT EXECUTE ON hospital_dba.USP_GET_SESSION_ROLE TO NV000001;
GRANT EXECUTE ON hospital_dba.USP_GET_SESSION_ROLE TO NV000121;
GRANT EXECUTE ON hospital_dba.USP_GET_SESSION_ROLE TO BN000001;

GRANT EXECUTE ON hospital_dba.USP_GET_USER_ID TO NV000021;
GRANT EXECUTE ON hospital_dba.USP_GET_USER_ID TO NV000001;
GRANT EXECUTE ON hospital_dba.USP_GET_USER_ID TO NV000121;
GRANT EXECUTE ON hospital_dba.USP_GET_USER_ID TO BN000001;

-- System privileges
GRANT CREATE SESSION                   TO NV000021;
GRANT CREATE SESSION, CREATE VIEW      TO NV000001;
GRANT CREATE SESSION                   TO NV000121;
GRANT CREATE SESSION                   TO BN000001;

-- Business role assignments
GRANT rl_doctor      TO NV000021;
GRANT rl_coordinator TO NV000001;
GRANT rl_technician  TO NV000121;
GRANT rl_patient     TO BN000001;

-- Column-level privileges
GRANT UPDATE (patient_id, full_name, gender, birthdate) ON hospital.patient TO NV000001;
GRANT UPDATE (conclusion, treatment_plan)               ON hospital.medical_record TO NV000021;

-- Grant with propagation right
GRANT SELECT ON hospital.department TO NV000001 WITH GRANT OPTION;

COMMIT;

