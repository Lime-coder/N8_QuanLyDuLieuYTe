-- ==============================================================================
-- 03_grant_roles_to_users.sql
-- Cháº¡y dÆ°á»›i quyá»n: sysdba
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;

GRANT CREATE SESSION TO rl_coordinator, rl_doctor, rl_technician, rl_patient;

-- NOTE: Individual users get their roles granted dynamically in USP_CREATE_USER_LINKED
