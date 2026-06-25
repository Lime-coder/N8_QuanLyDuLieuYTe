-- ==============================================================================
-- 04_patient_tests.sql
-- Chạy dưới quyền: a patient user
-- ==============================================================================

-- Connect as a patient (e.g., BN000001)
-- Check their own profile
-- SELECT * FROM hospital_dba.V_PATIENT_SELF;

-- Run procedure
-- VAR cur REFCURSOR;
-- EXEC hospital_dba.USP_GET_PATIENT_PROFILE(:cur);
-- PRINT cur;

