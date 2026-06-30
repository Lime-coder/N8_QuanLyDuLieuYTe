-- ==============================================================================
-- 05_coordinator_tests.sql
-- Chạy dưới quyền: hospital_dba
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;

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
GRANT EXECUTE ON hospital.SP_COORD_GET_MAX_PAT_ID TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_INS_PAT TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_UPD_PAT TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_ALL_MED TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_INS_MED TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_SRV_ASS TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_UPD_TECH TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_PATS_PAGED TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_ALL_MED_PAGED TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_SRV_ASS_PAGED TO rl_coordinator;
