-- ==============================================================================
-- 04_patient_tests.sql
-- Run as: a patient user
-- ==============================================================================

-- Connect as a patient (e.g., BN000001)
-- Check their own profile
-- SELECT * FROM hospital.V_PATIENT_SELF;

-- Run procedure
-- VAR cur REFCURSOR;
-- EXEC hospital.USP_GET_PATIENT_PROFILE(:cur);
-- PRINT cur;
