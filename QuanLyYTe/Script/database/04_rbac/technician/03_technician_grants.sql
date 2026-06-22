-- ==============================================================================
-- 03_technician_grants.sql
-- ChÃ¡ÂºÂ¡y dÃ†Â°Ã¡Â»â€ºi quyÃ¡Â»Ân: hospital (or sysdba with CURRENT_SCHEMA=hospital)
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

GRANT SELECT ON hospital_dba.V_TECHNICIAN_SERVICE_RECORD TO rl_technician;
GRANT UPDATE (SERVICE_RESULT) ON hospital_dba.V_TECHNICIAN_SERVICE_RECORD TO rl_technician;

GRANT EXECUTE ON hospital_dba.GET_TECHNICIAN_SERVICE_RECORDS TO rl_technician;
GRANT EXECUTE ON hospital_dba.UPDATE_TECHNICIAN_SERVICE_RESULT TO rl_technician;
GRANT EXECUTE ON hospital_dba.GET_TECHNICIAN_PERSONAL_INFO TO rl_technician;
GRANT EXECUTE ON hospital_dba.UPDATE_TECHNICIAN_PERSONAL_INFO TO rl_technician;

-- Access to lookup staff via procedure requires SELECT on STAFF
GRANT SELECT ON hospital.STAFF TO rl_technician;
GRANT UPDATE (hometown, phone) ON hospital.staff TO rl_technician;
GRANT SELECT ON hospital.DEPARTMENT TO rl_technician;

