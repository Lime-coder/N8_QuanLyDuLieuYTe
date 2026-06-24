-- ==============================================================================
-- 03_technician_grants.sql
-- Mục đích: Cấp quyền (GRANT) cho Role rl_technician.
-- Người thực thi: hospital_dba (hoặc SYS)
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

GRANT SELECT ON hospital_dba.V_TECHNICIAN_SERVICE_RECORD TO rl_technician;
GRANT UPDATE (SERVICE_RESULT) ON hospital_dba.V_TECHNICIAN_SERVICE_RECORD TO rl_technician;

GRANT SELECT ON hospital_dba.V_TECHNICIAN_PERSONAL_INFO TO rl_technician;
GRANT UPDATE (hometown, phone) ON hospital_dba.V_TECHNICIAN_PERSONAL_INFO TO rl_technician;

GRANT EXECUTE ON hospital_dba.USP_GET_TECHNICIAN_SERVICE_RECORDS TO rl_technician;
GRANT EXECUTE ON hospital_dba.USP_UPDATE_TECHNICIAN_SERVICE_RESULT TO rl_technician;
GRANT EXECUTE ON hospital_dba.USP_GET_TECHNICIAN_PERSONAL_INFO TO rl_technician;
GRANT EXECUTE ON hospital_dba.USP_UPDATE_TECHNICIAN_PERSONAL_INFO TO rl_technician;

