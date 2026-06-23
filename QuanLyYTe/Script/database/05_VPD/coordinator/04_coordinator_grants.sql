-- ==============================================================================
-- 05_coordinator_tests.sql
-- Cháº¡y dÆ°á»›i quyá»n: hospital_dba
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;

-- GRANTS
GRANT SELECT, INSERT, UPDATE ON hospital.patient TO rl_coordinator;
GRANT SELECT, INSERT ON hospital.medical_record TO rl_coordinator;
GRANT UPDATE (doctor_id, dept_id) ON hospital.medical_record TO rl_coordinator;
GRANT SELECT ON hospital.service_record TO rl_coordinator;
GRANT UPDATE (technician_id) ON hospital.service_record TO rl_coordinator;
GRANT SELECT ON hospital.department TO rl_coordinator;
GRANT SELECT ON hospital.staff TO rl_coordinator;
GRANT UPDATE (phone, hometown) ON hospital.staff TO rl_coordinator;
GRANT SELECT ON hospital.VW_COORD_DOCTORS TO rl_coordinator;
GRANT SELECT ON hospital.VW_COORD_TECHNICIANS TO rl_coordinator;

GRANT EXECUTE ON hospital.SP_COORD_GET_DOCTORS TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_DOC_DEPT TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_TECHS TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_DEPTS TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_UPD_MED_REC TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_SELF TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_UPD_SELF TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_PATS TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_SEARCH_PATS TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_CHK_PAT_ID TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_CHK_IDCARD TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_CHK_USER TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_INS_PAT TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_UPD_PAT TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_ALL_MED TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_INS_MED TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_SRV_ASS TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_UPD_TECH TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_PATS_PAGED TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_ALL_MED_PAGED TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_SRV_ASS_PAGED TO rl_coordinator;
