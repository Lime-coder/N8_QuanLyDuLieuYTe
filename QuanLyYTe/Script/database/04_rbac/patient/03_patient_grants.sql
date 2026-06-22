-- ==============================================================================
-- 03_patient_grants.sql
-- Cháº¡y dÆ°á»›i quyá»n: hospital (or sysdba with CURRENT_SCHEMA=hospital)
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

GRANT SELECT ON hospital_dba.V_PATIENT_SELF TO rl_patient;
GRANT UPDATE (house_no, street, district, city_province, medical_history, family_medical_history, drug_allergies) ON hospital_dba.V_PATIENT_SELF TO rl_patient;
GRANT SELECT ON hospital_dba.V_MEDICAL_RECORD_PATIENT TO rl_patient;
GRANT SELECT ON hospital_dba.V_PRESCRIPTION_PATIENT TO rl_patient;
GRANT SELECT ON hospital_dba.V_SERVICE_RECORD_PATIENT TO rl_patient;

GRANT EXECUTE ON hospital_dba.USP_GET_PATIENT_PROFILE TO rl_patient;
GRANT EXECUTE ON hospital_dba.USP_GET_PATIENT_RECORDS TO rl_patient;
GRANT EXECUTE ON hospital_dba.USP_GET_PATIENT_PRESCRIPTIONS TO rl_patient;
GRANT EXECUTE ON hospital_dba.USP_GET_PATIENT_SERVICES TO rl_patient;
GRANT EXECUTE ON hospital_dba.USP_UPDATE_PATIENT_CONTACT TO rl_patient;

GRANT SELECT ON hospital.staff TO rl_patient;
GRANT SELECT ON hospital.department TO rl_patient;

