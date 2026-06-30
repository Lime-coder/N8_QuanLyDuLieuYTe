-- Run as: SYSDBA | Container: PDB_QLYT
-- ALTER SESSION SET CONTAINER = PDB_QLYT;
-- Only run after executed the table
GRANT SELECT ON hospital.department     TO hospital_dba WITH GRANT OPTION;
GRANT SELECT ON hospital.staff          TO hospital_dba WITH GRANT OPTION;
GRANT SELECT ON hospital.patient        TO hospital_dba WITH GRANT OPTION;
GRANT SELECT ON hospital.medical_record TO hospital_dba WITH GRANT OPTION;
GRANT SELECT ON hospital.service_record TO hospital_dba WITH GRANT OPTION;
GRANT SELECT ON hospital.prescription   TO hospital_dba WITH GRANT OPTION;
GRANT INSERT, UPDATE, DELETE ON hospital.staff          TO hospital_dba;
GRANT INSERT, UPDATE, DELETE ON hospital.service_record TO hospital_dba;
GRANT INSERT, UPDATE, DELETE ON hospital.patient        TO hospital_dba WITH GRANT OPTION;

GRANT SELECT ON hospital.SEQ_STAFF_ID    TO hospital_dba;
GRANT SELECT ON hospital.SEQ_PATIENT_ID  TO hospital_dba;

-- After executing the above code, disconnect then reconnect into hospital_dba