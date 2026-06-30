-- ==============================================================================
-- 03_patient_grants.sql
-- Chạy dưới quyền: hospital (or sysdba with CURRENT_SCHEMA=hospital)
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;



GRANT EXECUTE ON hospital_dba.USP_GET_PATIENT_PROFILE TO rl_patient;
GRANT EXECUTE ON hospital_dba.USP_GET_PATIENT_RECORDS TO rl_patient;
GRANT EXECUTE ON hospital_dba.USP_GET_PATIENT_PRESCRIPTIONS TO rl_patient;
GRANT EXECUTE ON hospital_dba.USP_GET_PATIENT_SERVICES TO rl_patient;
GRANT EXECUTE ON hospital_dba.USP_UPDATE_PATIENT_CONTACT TO rl_patient;

