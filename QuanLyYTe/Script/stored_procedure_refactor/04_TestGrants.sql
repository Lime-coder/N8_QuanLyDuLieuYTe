-- Run as: hospital_dba | Container: PDB_QLYT
-- Creates Oracle users matching mock staff and sets up roles and privileges for testing
ALTER SESSION SET CONTAINER = PDB_QLYT_Nhap;

BEGIN
    FOR u IN (SELECT username FROM all_users
              WHERE username IN ('NV001','NV002','NV003','NV004')) LOOP
        EXECUTE IMMEDIATE 'DROP USER ' || u.username || ' CASCADE';
    END LOOP;
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

CREATE USER NV001 IDENTIFIED BY Abc123456;
CREATE USER NV002 IDENTIFIED BY Abc123456;
CREATE USER NV003 IDENTIFIED BY Abc123456;
CREATE USER NV004 IDENTIFIED BY Abc123456;

-- System privileges
GRANT CREATE SESSION                   TO NV001;
GRANT CREATE SESSION, CREATE VIEW      TO NV002;
GRANT CREATE SESSION                   TO NV003;
GRANT CREATE SESSION, CREATE SEQUENCE  TO NV004;

-- Business role assignments
GRANT rl_doctor      TO NV001;
GRANT rl_coordinator TO NV002;
GRANT rl_technician  TO NV003;
GRANT rl_doctor      TO NV004;

-- RL_DOCTOR: read/write medical records and prescriptions, read-only on supporting tables
GRANT SELECT, INSERT, UPDATE ON hospital.medical_record TO rl_doctor;
GRANT SELECT, INSERT         ON hospital.prescription   TO rl_doctor;
GRANT SELECT                 ON hospital.patient        TO rl_doctor;
GRANT SELECT                 ON hospital.department     TO rl_doctor;
GRANT SELECT                 ON hospital.staff          TO rl_doctor;

-- RL_COORDINATOR: full access on staff and departments, read-only on patient and records
GRANT SELECT, INSERT, UPDATE, DELETE ON hospital.staff          TO rl_coordinator;
GRANT SELECT, INSERT, UPDATE, DELETE ON hospital.department     TO rl_coordinator;
GRANT SELECT                         ON hospital.patient        TO rl_coordinator;
GRANT SELECT                         ON hospital.medical_record TO rl_coordinator;

-- RL_TECHNICIAN: read records, write service results
GRANT SELECT                 ON hospital.medical_record TO rl_technician;
GRANT SELECT, INSERT, UPDATE ON hospital.service_record TO rl_technician;
GRANT SELECT                 ON hospital.staff          TO rl_technician;

-- RL_PATIENT: read-only on own relevant data
GRANT SELECT ON hospital.patient        TO rl_patient;
GRANT SELECT ON hospital.medical_record TO rl_patient;
GRANT SELECT ON hospital.prescription   TO rl_patient;

-- Column-level privileges
CREATE OR REPLACE VIEW hospital.v_patient_limited AS
SELECT patient_id, full_name, gender, birthdate 
FROM hospital.patient;
GRANT SELECT ON hospital.v_patient_limited TO NV002;

GRANT UPDATE (conclusion, treatment_plan)               ON hospital.medical_record TO NV001;

-- Grant with propagation right
GRANT SELECT ON hospital.department TO NV002 WITH GRANT OPTION;

COMMIT;
