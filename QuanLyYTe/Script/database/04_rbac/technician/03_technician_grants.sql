-- ==============================================================================
-- 03_technician_grants.sql
-- Run as: hospital (or sysdba with CURRENT_SCHEMA=hospital)
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

GRANT SELECT ON hospital.V_TECHNICIAN_SERVICE_RECORD TO rl_technician;
GRANT UPDATE (SERVICE_RESULT) ON hospital.V_TECHNICIAN_SERVICE_RECORD TO rl_technician;

GRANT EXECUTE ON hospital.GET_TECHNICIAN_SERVICE_RECORDS TO rl_technician;
GRANT EXECUTE ON hospital.UPDATE_TECHNICIAN_SERVICE_RESULT TO rl_technician;
GRANT EXECUTE ON hospital.GET_TECHNICIAN_PERSONAL_INFO TO rl_technician;
GRANT EXECUTE ON hospital.UPDATE_TECHNICIAN_PERSONAL_INFO TO rl_technician;

-- Access to lookup staff via procedure requires SELECT on STAFF
GRANT SELECT ON hospital.STAFF TO rl_technician;
