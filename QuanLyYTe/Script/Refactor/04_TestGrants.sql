-- Run as: hospital_dba | Container: PDB_QLYT
-- Creates Oracle users matching mock staff and sets up roles and privileges for testing
-- ALTER SESSION SET CONTAINER = PDB_QLYT;

BEGIN
    FOR u IN (SELECT username FROM all_users
              WHERE username IN ('NV001','NV002','NV003','BN001')) LOOP
        EXECUTE IMMEDIATE 'DROP USER ' || u.username || ' CASCADE';
    END LOOP;
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

CREATE USER NV001 IDENTIFIED BY Abc123456;
CREATE USER NV002 IDENTIFIED BY Abc123456;
CREATE USER NV003 IDENTIFIED BY Abc123456;
CREATE USER BN001 IDENTIFIED BY Abc123456;

GRANT EXECUTE ON hospital_dba.USP_GET_SESSION_ROLE TO NV001;
GRANT EXECUTE ON hospital_dba.USP_GET_SESSION_ROLE TO NV002;
GRANT EXECUTE ON hospital_dba.USP_GET_SESSION_ROLE TO NV003;
GRANT EXECUTE ON hospital_dba.USP_GET_SESSION_ROLE TO BN001;

GRANT EXECUTE ON hospital_dba.USP_GET_USER_ID TO NV001;
GRANT EXECUTE ON hospital_dba.USP_GET_USER_ID TO NV002;
GRANT EXECUTE ON hospital_dba.USP_GET_USER_ID TO NV003;
GRANT EXECUTE ON hospital_dba.USP_GET_USER_ID TO BN001;

-- System privileges
GRANT CREATE SESSION                   TO NV001;
GRANT CREATE SESSION, CREATE VIEW      TO NV002;
GRANT CREATE SESSION                   TO NV003;
GRANT CREATE SESSION                   TO BN001;

-- Business role assignments
GRANT rl_doctor      TO NV001;
GRANT rl_coordinator TO NV002;
GRANT rl_technician  TO NV003;
GRANT rl_patient     TO BN001;

-- Column-level privileges
GRANT UPDATE (patient_id, full_name, gender, birthdate) ON hospital.patient TO NV002;
GRANT UPDATE (conclusion, treatment_plan)               ON hospital.medical_record TO NV001;

-- Grant with propagation right
GRANT SELECT ON hospital.department TO NV002 WITH GRANT OPTION;

COMMIT;
