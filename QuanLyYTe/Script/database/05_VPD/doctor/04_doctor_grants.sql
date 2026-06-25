-- ==============================================================================
-- 04_doctor_grants.sql
-- Chạy dưới quyền: hospital_dba (for grants), then test as doctor
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;

GRANT EXECUTE ON hospital_dba.USP_GET_MEDICAL_RECORD TO rl_doctor;
GRANT EXECUTE ON hospital_dba.USP_UPDATE_MEDICAL_RECORD TO rl_doctor;

GRANT EXECUTE ON hospital_dba.USP_GET_SERVICES TO rl_doctor;
GRANT EXECUTE ON hospital_dba.USP_ADD_SERVICE TO rl_doctor;
GRANT EXECUTE ON hospital_dba.USP_DELETE_SERVICE TO rl_doctor;

GRANT EXECUTE ON hospital_dba.USP_GET_PRESCRIPTION TO rl_doctor;
GRANT EXECUTE ON hospital_dba.USP_MANAGE_PRESCRIPTION TO rl_doctor;

GRANT EXECUTE ON hospital_dba.USP_GET_PATIENTS TO rl_doctor;
GRANT EXECUTE ON hospital_dba.USP_UPDATE_PATIENT TO rl_doctor;

GRANT EXECUTE ON hospital_dba.USP_GET_SELF_INFO TO rl_doctor;
GRANT EXECUTE ON hospital_dba.USP_UPDATE_SELF_INFO TO rl_doctor;

