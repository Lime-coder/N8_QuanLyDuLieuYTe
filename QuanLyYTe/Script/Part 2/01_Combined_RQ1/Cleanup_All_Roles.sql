-- ==============================================================================
-- Cleanup_All_Roles.sql
-- Purpose: One-shot cleanup for Doctor + Coordinator + Patient + Technician setup.
--
-- Run as: HOSPITAL_DBA
-- ==============================================================================

-- ==============================================================================
-- Cleanup_Doctor_Legacy.sql
-- Purpose: Remove doctor legacy objects/grants created by:
--   - GrantPrivileges_Doctor.sql
--   - StoreProcedure_Doctor.sql
--   - VPD_Doctor.sql
--
-- Run as: HOSPITAL_DBA or another user with enough privileges in PDB_QLYT.
-- Connect to: PDB_QLYT before running this script.
-- If you are connected to CDB$ROOT as a privileged common user, uncomment this line:
-- ALTER SESSION SET CONTAINER = PDB_QLYT;
--
-- Notes:
--   1. This script is intentionally idempotent: missing objects/grants are skipped.
--   2. It removes the doctor VPD policies/functions, doctor stored procedures,
--      and grants to rl_doctor that were added by the three doctor files above.
--   3. It does NOT drop rl_doctor itself.
--   4. It does NOT drop hospital_dba.USP_GET_SESSION_ROLE or
--      hospital_dba.USP_GET_GRANTED_ROLE because those look like shared helper
--      procedures; it only revokes rl_doctor's EXECUTE privilege on them.
--   5. If the old VPD_Doctor.sql already deleted other roles' VPD policies/functions
--      because of LIKE 'POL_VPD_%' / LIKE 'FN_VPD_%', this cleanup cannot restore
--      them. Re-run the correct coordinator/patient/technician setup after cleanup.
-- ==============================================================================

SET SERVEROUTPUT ON;

PROMPT ==============================================================================
PROMPT START: Cleanup Doctor legacy setup
PROMPT ==============================================================================

-- ==============================================================================
-- 1. Revoke doctor grants
-- ==============================================================================
DECLARE
    PROCEDURE try_exec(p_sql IN VARCHAR2) IS
    BEGIN
        EXECUTE IMMEDIATE p_sql;
        DBMS_OUTPUT.PUT_LINE('[OK]   ' || p_sql);
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('[SKIP] ' || p_sql || ' -> ' || SQLERRM);
    END;
BEGIN
    DBMS_OUTPUT.PUT_LINE('Revoking doctor helper procedure grants from hospital_dba...');

    try_exec('REVOKE EXECUTE ON hospital_dba.USP_GET_SESSION_ROLE FROM rl_doctor');
    try_exec('REVOKE EXECUTE ON hospital_dba.USP_GET_GRANTED_ROLE FROM rl_doctor');

    DBMS_OUTPUT.PUT_LINE('Revoking doctor base-table grants from hospital...');

    try_exec('REVOKE SELECT ON hospital.patient FROM rl_doctor');
    try_exec('REVOKE UPDATE ON hospital.patient FROM rl_doctor');

    try_exec('REVOKE SELECT ON hospital.medical_record FROM rl_doctor');
    try_exec('REVOKE UPDATE ON hospital.medical_record FROM rl_doctor');

    try_exec('REVOKE SELECT ON hospital.service_record FROM rl_doctor');
    try_exec('REVOKE INSERT ON hospital.service_record FROM rl_doctor');
    try_exec('REVOKE DELETE ON hospital.service_record FROM rl_doctor');

    try_exec('REVOKE SELECT ON hospital.prescription FROM rl_doctor');
    try_exec('REVOKE INSERT ON hospital.prescription FROM rl_doctor');
    try_exec('REVOKE UPDATE ON hospital.prescription FROM rl_doctor');
    try_exec('REVOKE DELETE ON hospital.prescription FROM rl_doctor');

    try_exec('REVOKE SELECT ON hospital.staff FROM rl_doctor');

    DBMS_OUTPUT.PUT_LINE('Revoking doctor stored procedure grants...');

    FOR p IN (
        SELECT column_value AS proc_name
        FROM TABLE(sys.odcivarchar2list(
            'USP_GET_MEDICAL_RECORD',
            'USP_UPDATE_MEDICAL_RECORD',
            'USP_GET_SERVICES',
            'USP_ADD_SERVICE',
            'USP_DELETE_SERVICE',
            'USP_GET_PRESCRIPTION',
            'USP_MANAGE_PRESCRIPTION',
            'USP_GET_PATIENTS',
            'USP_UPDATE_PATIENT',
            'USP_GET_SELF_INFO',
            'USP_UPDATE_SELF_INFO'
        ))
    ) LOOP
        try_exec('REVOKE EXECUTE ON hospital.' || p.proc_name || ' FROM rl_doctor');
    END LOOP;
END;
/

-- ==============================================================================
-- 2. Drop doctor VPD policies
-- ============================================================================== 
BEGIN
    DBMS_RLS.DROP_POLICY('HOSPITAL', 'MEDICAL_RECORD', 'POL_VPD_MEDICAL_RECORD_READ');
    DBMS_OUTPUT.PUT_LINE('[OK]   Dropped policy HOSPITAL.MEDICAL_RECORD.POL_VPD_MEDICAL_RECORD_READ');
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('[SKIP] Drop policy POL_VPD_MEDICAL_RECORD_READ -> ' || SQLERRM);
END;
/

BEGIN
    DBMS_RLS.DROP_POLICY('HOSPITAL', 'MEDICAL_RECORD', 'POL_VPD_MEDICAL_RECORD_UPDATE');
    DBMS_OUTPUT.PUT_LINE('[OK]   Dropped policy HOSPITAL.MEDICAL_RECORD.POL_VPD_MEDICAL_RECORD_UPDATE');
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('[SKIP] Drop policy POL_VPD_MEDICAL_RECORD_UPDATE -> ' || SQLERRM);
END;
/

BEGIN
    DBMS_RLS.DROP_POLICY('HOSPITAL', 'PATIENT', 'POL_VPD_PATIENT_READ');
    DBMS_OUTPUT.PUT_LINE('[OK]   Dropped policy HOSPITAL.PATIENT.POL_VPD_PATIENT_READ');
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('[SKIP] Drop policy POL_VPD_PATIENT_READ -> ' || SQLERRM);
END;
/

BEGIN
    DBMS_RLS.DROP_POLICY('HOSPITAL', 'PATIENT', 'POL_VPD_PATIENT_UPDATE');
    DBMS_OUTPUT.PUT_LINE('[OK]   Dropped policy HOSPITAL.PATIENT.POL_VPD_PATIENT_UPDATE');
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('[SKIP] Drop policy POL_VPD_PATIENT_UPDATE -> ' || SQLERRM);
END;
/

BEGIN
    DBMS_RLS.DROP_POLICY('HOSPITAL', 'SERVICE_RECORD', 'POL_VPD_SERVICE_RECORD_READ_DELETE');
    DBMS_OUTPUT.PUT_LINE('[OK]   Dropped policy HOSPITAL.SERVICE_RECORD.POL_VPD_SERVICE_RECORD_READ_DELETE');
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('[SKIP] Drop policy POL_VPD_SERVICE_RECORD_READ_DELETE -> ' || SQLERRM);
END;
/

BEGIN
    DBMS_RLS.DROP_POLICY('HOSPITAL', 'SERVICE_RECORD', 'POL_VPD_SERVICE_RECORD_INSERT');
    DBMS_OUTPUT.PUT_LINE('[OK]   Dropped policy HOSPITAL.SERVICE_RECORD.POL_VPD_SERVICE_RECORD_INSERT');
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('[SKIP] Drop policy POL_VPD_SERVICE_RECORD_INSERT -> ' || SQLERRM);
END;
/

BEGIN
    DBMS_RLS.DROP_POLICY('HOSPITAL', 'PRESCRIPTION', 'POL_VPD_PRESCRIPTION_READ_DELETE');
    DBMS_OUTPUT.PUT_LINE('[OK]   Dropped policy HOSPITAL.PRESCRIPTION.POL_VPD_PRESCRIPTION_READ_DELETE');
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('[SKIP] Drop policy POL_VPD_PRESCRIPTION_READ_DELETE -> ' || SQLERRM);
END;
/

BEGIN
    DBMS_RLS.DROP_POLICY('HOSPITAL', 'PRESCRIPTION', 'POL_VPD_PRESCRIPTION_UPDATE');
    DBMS_OUTPUT.PUT_LINE('[OK]   Dropped policy HOSPITAL.PRESCRIPTION.POL_VPD_PRESCRIPTION_UPDATE');
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('[SKIP] Drop policy POL_VPD_PRESCRIPTION_UPDATE -> ' || SQLERRM);
END;
/

-- ==============================================================================

-- ============================================================================== 
-- 2B. Drop fixed doctor VPD policies created by Combined_Doctor_Fixed.sql
-- ============================================================================== 
DECLARE
    PROCEDURE drop_policy_safe(p_object_name VARCHAR2, p_policy_name VARCHAR2) IS
    BEGIN
        DBMS_RLS.DROP_POLICY(
            object_schema => 'HOSPITAL',
            object_name   => p_object_name,
            policy_name   => p_policy_name
        );
        DBMS_OUTPUT.PUT_LINE('[OK]   Dropped policy HOSPITAL.' || p_object_name || '.' || p_policy_name);
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('[SKIP] Drop policy ' || p_policy_name || ' -> ' || SQLERRM);
    END;
BEGIN
    drop_policy_safe('MEDICAL_RECORD', 'POL_DOC_MEDICAL_RECORD_SEL');
    drop_policy_safe('MEDICAL_RECORD', 'POL_DOC_MEDICAL_RECORD_UPD');
    drop_policy_safe('PATIENT',        'POL_DOC_PATIENT_SEL');
    drop_policy_safe('PATIENT',        'POL_DOC_PATIENT_UPD');
    drop_policy_safe('SERVICE_RECORD', 'POL_DOC_SERVICE_RECORD_SEL_DEL');
    drop_policy_safe('SERVICE_RECORD', 'POL_DOC_SERVICE_RECORD_INS');
    drop_policy_safe('PRESCRIPTION',   'POL_DOC_PRESCRIPTION_SEL_DEL');
    drop_policy_safe('PRESCRIPTION',   'POL_DOC_PRESCRIPTION_INS_UPD');
END;
/

-- 3. Drop doctor stored procedures
-- ==============================================================================
DECLARE
    PROCEDURE try_exec(p_sql IN VARCHAR2) IS
    BEGIN
        EXECUTE IMMEDIATE p_sql;
        DBMS_OUTPUT.PUT_LINE('[OK]   ' || p_sql);
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('[SKIP] ' || p_sql || ' -> ' || SQLERRM);
    END;
BEGIN
    DBMS_OUTPUT.PUT_LINE('Dropping known doctor stored procedures...');

    FOR p IN (
        SELECT column_value AS proc_name
        FROM TABLE(sys.odcivarchar2list(
            'USP_GET_MEDICAL_RECORD',
            'USP_UPDATE_MEDICAL_RECORD',
            'USP_GET_SERVICES',
            'USP_ADD_SERVICE',
            'USP_DELETE_SERVICE',
            'USP_GET_PRESCRIPTION',
            'USP_MANAGE_PRESCRIPTION',
            'USP_GET_PATIENTS',
            'USP_UPDATE_PATIENT',
            'USP_GET_SELF_INFO',
            'USP_UPDATE_SELF_INFO'
        ))
    ) LOOP
        try_exec('DROP PROCEDURE hospital.' || p.proc_name);
    END LOOP;

    DBMS_OUTPUT.PUT_LINE('Dropping legacy doctor procedures matching USP_DOCTOR_%...');

    FOR p IN (
        SELECT object_name
        FROM all_objects
        WHERE owner = 'HOSPITAL'
          AND object_type = 'PROCEDURE'
          AND object_name LIKE 'USP_DOCTOR_%'
        ORDER BY object_name
    ) LOOP
        try_exec('DROP PROCEDURE hospital.' || p.object_name);
    END LOOP;
END;
/

-- ==============================================================================
-- 4. Drop doctor VPD functions
-- ==============================================================================
DECLARE
    PROCEDURE try_exec(p_sql IN VARCHAR2) IS
    BEGIN
        EXECUTE IMMEDIATE p_sql;
        DBMS_OUTPUT.PUT_LINE('[OK]   ' || p_sql);
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('[SKIP] ' || p_sql || ' -> ' || SQLERRM);
    END;
BEGIN
    DBMS_OUTPUT.PUT_LINE('Dropping doctor VPD functions...');

    try_exec('DROP FUNCTION hospital.FN_VPD_MEDICAL_RECORD_DOCTOR');
    try_exec('DROP FUNCTION hospital.FN_VPD_PATIENT_DOCTOR');
    try_exec('DROP FUNCTION hospital.FN_VPD_RECORD_DETAIL_DOCTOR');
END;
/

-- ==============================================================================
-- 5. Verification queries
-- ==============================================================================
PROMPT ==============================================================================
PROMPT Verification: remaining doctor legacy objects/policies
PROMPT ==============================================================================

SELECT owner, object_name, object_type, status
FROM all_objects
WHERE owner = 'HOSPITAL'
  AND (
        object_name IN (
            'FN_VPD_MEDICAL_RECORD_DOCTOR',
            'FN_VPD_PATIENT_DOCTOR',
            'FN_VPD_RECORD_DETAIL_DOCTOR',
            'USP_GET_MEDICAL_RECORD',
            'USP_UPDATE_MEDICAL_RECORD',
            'USP_GET_SERVICES',
            'USP_ADD_SERVICE',
            'USP_DELETE_SERVICE',
            'USP_GET_PRESCRIPTION',
            'USP_MANAGE_PRESCRIPTION',
            'USP_GET_PATIENTS',
            'USP_UPDATE_PATIENT',
            'USP_GET_SELF_INFO',
            'USP_UPDATE_SELF_INFO'
        )
        OR object_name LIKE 'USP_DOCTOR_%'
      )
ORDER BY owner, object_type, object_name;

SELECT object_owner, object_name, policy_name, pf_owner, function
FROM all_policies
WHERE object_owner = 'HOSPITAL'
  AND policy_name IN (
        'POL_VPD_MEDICAL_RECORD_READ',
        'POL_VPD_MEDICAL_RECORD_UPDATE',
        'POL_VPD_PATIENT_READ',
        'POL_VPD_PATIENT_UPDATE',
        'POL_VPD_SERVICE_RECORD_READ_DELETE',
        'POL_VPD_SERVICE_RECORD_INSERT',
        'POL_VPD_PRESCRIPTION_READ_DELETE',
        'POL_VPD_PRESCRIPTION_UPDATE'
  )
ORDER BY object_name, policy_name;

SELECT owner, table_name, privilege, grantee
FROM all_tab_privs
WHERE grantee = 'RL_DOCTOR'
  AND owner IN ('HOSPITAL', 'HOSPITAL_DBA')
  AND (
        table_name IN (
            'PATIENT',
            'MEDICAL_RECORD',
            'SERVICE_RECORD',
            'PRESCRIPTION',
            'STAFF',
            'USP_GET_SESSION_ROLE',
            'USP_GET_GRANTED_ROLE',
            'USP_GET_MEDICAL_RECORD',
            'USP_UPDATE_MEDICAL_RECORD',
            'USP_GET_SERVICES',
            'USP_ADD_SERVICE',
            'USP_DELETE_SERVICE',
            'USP_GET_PRESCRIPTION',
            'USP_MANAGE_PRESCRIPTION',
            'USP_GET_PATIENTS',
            'USP_UPDATE_PATIENT',
            'USP_GET_SELF_INFO',
            'USP_UPDATE_SELF_INFO'
        )
        OR table_name LIKE 'USP_DOCTOR_%'
      )
ORDER BY owner, table_name, privilege;

COMMIT;

PROMPT ==============================================================================
PROMPT DONE: Cleanup Doctor legacy setup
PROMPT ==============================================================================


PROMPT ==============================================================================
PROMPT NEXT: Cleanup Coordinator + Patient + Technician
PROMPT ==============================================================================

-- ==============================================================================
-- Cleanup_Coordinator_Patient_Technician.sql
-- Purpose: Remove objects/grants created by Combined_Coordinator_Patient_Technician.sql
-- Excludes doctor files/objects:
--   - GrantPrivileges_Doctor.sql
--   - StoreProcedure_Doctor.sql
--   - VPD_Doctor.sql
--
-- Run as: HOSPITAL_DBA or another user with enough privileges in PDB_QLYT.
-- Connect to: PDB_QLYT before running this script.
-- If you are connected to CDB$ROOT as a privileged common user, uncomment this line:
-- ALTER SESSION SET CONTAINER = PDB_QLYT;
--
-- Notes:
--   1. This script is intentionally idempotent: missing objects/grants are skipped.
--   2. It drops coordinator/patient/technician procedures, views, the coordinator helper
--      table, and the coordinator STAFF VPD policies/function.
--      It does not drop TRG_VALIDATE_SERVICE_RECORD by default because that trigger
--      may have existed before the coordinator script replaced it.
--   3. It DOES NOT automatically revert data/schema changes such as:
--        - hospital.TRG_VALIDATE_SERVICE_RECORD overwritten by VPD_Coordinator.sql
--        - hospital.service_record.technician_id changed to NULL-able
--        - sample unassigned service_record inserted by VPD_Coordinator.sql
--      because reverting those blindly can delete real data or fail if NULL values exist.
--      See the optional manual-review section near the bottom.
-- ==============================================================================

SET SERVEROUTPUT ON;

PROMPT ==============================================================================
PROMPT START: Cleanup Coordinator + Patient + Technician setup
PROMPT ==============================================================================

-- ==============================================================================
-- Helper: execute DDL/DCL and skip errors so the script can be re-run safely.
-- ==============================================================================
DECLARE
    PROCEDURE try_exec(p_sql IN VARCHAR2) IS
    BEGIN
        EXECUTE IMMEDIATE p_sql;
        DBMS_OUTPUT.PUT_LINE('[OK]   ' || p_sql);
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('[SKIP] ' || p_sql || ' -> ' || SQLERRM);
    END;
BEGIN
    DBMS_OUTPUT.PUT_LINE('Cleanup helper initialized.');
END;
/

-- ==============================================================================
-- 1. Revoke role grants from coordinator/patient/technician roles
--    Dropping objects would remove many grants automatically, but explicit revoke
--    makes base-table grants easier to clean up.
-- ==============================================================================
DECLARE
    PROCEDURE try_exec(p_sql IN VARCHAR2) IS
    BEGIN
        EXECUTE IMMEDIATE p_sql;
        DBMS_OUTPUT.PUT_LINE('[OK]   ' || p_sql);
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('[SKIP] ' || p_sql || ' -> ' || SQLERRM);
    END;
BEGIN
    DBMS_OUTPUT.PUT_LINE('Revoking coordinator grants...');

    try_exec('REVOKE SELECT ON hospital.patient FROM rl_coordinator');
    try_exec('REVOKE INSERT ON hospital.patient FROM rl_coordinator');
    try_exec('REVOKE UPDATE ON hospital.patient FROM rl_coordinator');
    try_exec('REVOKE SELECT ON hospital.medical_record FROM rl_coordinator');
    try_exec('REVOKE INSERT ON hospital.medical_record FROM rl_coordinator');
    try_exec('REVOKE UPDATE ON hospital.medical_record FROM rl_coordinator');
    try_exec('REVOKE SELECT ON hospital.service_record FROM rl_coordinator');
    try_exec('REVOKE UPDATE ON hospital.service_record FROM rl_coordinator');
    try_exec('REVOKE SELECT ON hospital.department FROM rl_coordinator');
    try_exec('REVOKE SELECT ON hospital.staff FROM rl_coordinator');
    try_exec('REVOKE UPDATE ON hospital.staff FROM rl_coordinator');
    try_exec('REVOKE SELECT ON hospital.VW_COORD_DOCTORS FROM rl_coordinator');
    try_exec('REVOKE SELECT ON hospital.VW_COORD_TECHNICIANS FROM rl_coordinator');

    FOR p IN (
        SELECT column_value AS proc_name
        FROM TABLE(sys.odcivarchar2list(
            'SP_COORD_GET_DOCTORS',
            'SP_COORD_GET_DOC_DEPT',
            'SP_COORD_GET_TECHS',
            'SP_COORD_GET_DEPTS',
            'SP_COORD_GET_SELF',
            'SP_COORD_UPD_SELF',
            'SP_COORD_GET_PATS',
            'SP_COORD_SEARCH_PATS',
            'SP_COORD_CHK_PAT_ID',
            'SP_COORD_CHK_IDCARD',
            'SP_COORD_CHK_USER',
            'SP_COORD_INS_PAT',
            'SP_COORD_UPD_PAT',
            'SP_COORD_GET_ALL_MED',
            'SP_COORD_INS_MED',
            'SP_COORD_UPD_MED_REC',
            'SP_COORD_GET_SRV_ASS',
            'SP_COORD_UPD_SRV_REC',
            'SP_COORD_UPD_TECH'
        ))
    ) LOOP
        try_exec('REVOKE EXECUTE ON hospital.' || p.proc_name || ' FROM rl_coordinator');
    END LOOP;

    DBMS_OUTPUT.PUT_LINE('Revoking patient grants...');

    try_exec('REVOKE SELECT ON hospital_dba.V_PATIENT_SELF FROM rl_patient');
    try_exec('REVOKE UPDATE ON hospital_dba.V_PATIENT_SELF FROM rl_patient');
    try_exec('REVOKE SELECT ON hospital_dba.V_MEDICAL_RECORD_PATIENT FROM rl_patient');
    try_exec('REVOKE SELECT ON hospital_dba.V_PRESCRIPTION_PATIENT FROM rl_patient');
    try_exec('REVOKE SELECT ON hospital_dba.V_SERVICE_RECORD_PATIENT FROM rl_patient');
    try_exec('REVOKE SELECT ON hospital.staff FROM rl_patient');
    try_exec('REVOKE SELECT ON hospital.department FROM rl_patient');

    FOR p IN (
        SELECT column_value AS proc_name
        FROM TABLE(sys.odcivarchar2list(
            'USP_GET_PATIENT_PROFILE',
            'USP_GET_PATIENT_RECORDS',
            'USP_GET_PATIENT_PRESCRIPTIONS',
            'USP_GET_PATIENT_SERVICES',
            'USP_UPDATE_PATIENT_CONTACT'
        ))
    ) LOOP
        try_exec('REVOKE EXECUTE ON hospital_dba.' || p.proc_name || ' FROM rl_patient');
    END LOOP;

    DBMS_OUTPUT.PUT_LINE('Revoking technician grants...');

    try_exec('REVOKE SELECT ON hospital_dba.V_TECHNICIAN_SERVICE_RECORD FROM rl_technician');

    FOR p IN (
        SELECT column_value AS proc_name
        FROM TABLE(sys.odcivarchar2list(
            'GET_TECHNICIAN_SERVICE_RECORDS',
            'UPDATE_TECHNICIAN_SERVICE_RESULT',
            'GET_TECHNICIAN_PERSONAL_INFO',
            'UPDATE_TECHNICIAN_PERSONAL_INFO'
        ))
    ) LOOP
        try_exec('REVOKE EXECUTE ON hospital_dba.' || p.proc_name || ' FROM rl_technician');
    END LOOP;
END;
/

-- ==============================================================================
-- 2. Drop coordinator VPD policies on STAFF
-- ==============================================================================
BEGIN
    DBMS_RLS.DROP_POLICY('HOSPITAL', 'STAFF', 'POL_VPD_STAFF_SELF_SELECT');
    DBMS_OUTPUT.PUT_LINE('[OK]   Dropped policy HOSPITAL.STAFF.POL_VPD_STAFF_SELF_SELECT');
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('[SKIP] Drop policy POL_VPD_STAFF_SELF_SELECT -> ' || SQLERRM);
END;
/

BEGIN
    DBMS_RLS.DROP_POLICY('HOSPITAL', 'STAFF', 'POL_VPD_STAFF_SELF_UPDATE');
    DBMS_OUTPUT.PUT_LINE('[OK]   Dropped policy HOSPITAL.STAFF.POL_VPD_STAFF_SELF_UPDATE');
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('[SKIP] Drop policy POL_VPD_STAFF_SELF_UPDATE -> ' || SQLERRM);
END;
/

-- ==============================================================================
-- 3. Drop procedures
-- ==============================================================================
DECLARE
    PROCEDURE try_exec(p_sql IN VARCHAR2) IS
    BEGIN
        EXECUTE IMMEDIATE p_sql;
        DBMS_OUTPUT.PUT_LINE('[OK]   ' || p_sql);
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('[SKIP] ' || p_sql || ' -> ' || SQLERRM);
    END;
BEGIN
    DBMS_OUTPUT.PUT_LINE('Dropping coordinator procedures...');

    FOR p IN (
        SELECT column_value AS proc_name
        FROM TABLE(sys.odcivarchar2list(
            'SP_COORD_GET_DOCTORS',
            'SP_COORD_GET_DOC_DEPT',
            'SP_COORD_GET_TECHS',
            'SP_COORD_GET_DEPTS',
            'SP_COORD_GET_SELF',
            'SP_COORD_UPD_SELF',
            'SP_COORD_GET_PATS',
            'SP_COORD_SEARCH_PATS',
            'SP_COORD_CHK_PAT_ID',
            'SP_COORD_CHK_IDCARD',
            'SP_COORD_CHK_USER',
            'SP_COORD_INS_PAT',
            'SP_COORD_UPD_PAT',
            'SP_COORD_GET_ALL_MED',
            'SP_COORD_INS_MED',
            'SP_COORD_UPD_MED_REC',
            'SP_COORD_GET_SRV_ASS',
            'SP_COORD_UPD_SRV_REC',
            'SP_COORD_UPD_TECH'
        ))
    ) LOOP
        try_exec('DROP PROCEDURE hospital.' || p.proc_name);
    END LOOP;

    DBMS_OUTPUT.PUT_LINE('Dropping patient procedures...');

    FOR p IN (
        SELECT column_value AS proc_name
        FROM TABLE(sys.odcivarchar2list(
            'USP_GET_PATIENT_PROFILE',
            'USP_GET_PATIENT_RECORDS',
            'USP_GET_PATIENT_PRESCRIPTIONS',
            'USP_GET_PATIENT_SERVICES',
            'USP_UPDATE_PATIENT_CONTACT'
        ))
    ) LOOP
        try_exec('DROP PROCEDURE hospital_dba.' || p.proc_name);
    END LOOP;

    DBMS_OUTPUT.PUT_LINE('Dropping technician procedures...');

    FOR p IN (
        SELECT column_value AS proc_name
        FROM TABLE(sys.odcivarchar2list(
            'GET_TECHNICIAN_SERVICE_RECORDS',
            'UPDATE_TECHNICIAN_SERVICE_RESULT',
            'GET_TECHNICIAN_PERSONAL_INFO',
            'UPDATE_TECHNICIAN_PERSONAL_INFO'
        ))
    ) LOOP
        try_exec('DROP PROCEDURE hospital_dba.' || p.proc_name);
    END LOOP;
END;
/

-- ==============================================================================
-- 4. Drop views, trigger, VPD function, and helper table
-- ==============================================================================
DECLARE
    PROCEDURE try_exec(p_sql IN VARCHAR2) IS
    BEGIN
        EXECUTE IMMEDIATE p_sql;
        DBMS_OUTPUT.PUT_LINE('[OK]   ' || p_sql);
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('[SKIP] ' || p_sql || ' -> ' || SQLERRM);
    END;
BEGIN
    DBMS_OUTPUT.PUT_LINE('Dropping views...');

    try_exec('DROP VIEW hospital.VW_COORD_DOCTORS');
    try_exec('DROP VIEW hospital.VW_COORD_TECHNICIANS');

    try_exec('DROP VIEW hospital_dba.V_PATIENT_SELF');
    try_exec('DROP VIEW hospital_dba.V_MEDICAL_RECORD_PATIENT');
    try_exec('DROP VIEW hospital_dba.V_PRESCRIPTION_PATIENT');
    try_exec('DROP VIEW hospital_dba.V_SERVICE_RECORD_PATIENT');

    try_exec('DROP VIEW hospital_dba.V_TECHNICIAN_SERVICE_RECORD');

    DBMS_OUTPUT.PUT_LINE('Dropping coordinator VPD function/helper table...');

    -- Not dropped automatically: hospital.TRG_VALIDATE_SERVICE_RECORD
    -- Reason: VPD_Coordinator.sql used CREATE OR REPLACE on this trigger, so an
    -- original trigger may have existed before the combined script. Cleanup cannot
    -- restore that original trigger safely. See optional section at the bottom.
    try_exec('DROP FUNCTION hospital.FN_VPD_STAFF_SELF');
    try_exec('DROP TABLE hospital.COORD_ASSIGNMENT_STAFF PURGE');
END;
/

-- ==============================================================================
-- 5. Verification queries
-- ==============================================================================
PROMPT ==============================================================================
PROMPT Verification: remaining non-doctor objects from this combined script
PROMPT ==============================================================================

SELECT owner, object_name, object_type, status
FROM all_objects
WHERE (owner = 'HOSPITAL' AND object_name IN (
        'FN_VPD_STAFF_SELF',
        'VW_COORD_DOCTORS',
        'VW_COORD_TECHNICIANS',
        'COORD_ASSIGNMENT_STAFF',
        'SP_COORD_GET_DOCTORS',
        'SP_COORD_GET_DOC_DEPT',
        'SP_COORD_GET_TECHS',
        'SP_COORD_GET_DEPTS',
        'SP_COORD_GET_SELF',
        'SP_COORD_UPD_SELF',
        'SP_COORD_GET_PATS',
        'SP_COORD_SEARCH_PATS',
        'SP_COORD_CHK_PAT_ID',
        'SP_COORD_CHK_IDCARD',
        'SP_COORD_CHK_USER',
        'SP_COORD_INS_PAT',
        'SP_COORD_UPD_PAT',
        'SP_COORD_GET_ALL_MED',
        'SP_COORD_INS_MED',
        'SP_COORD_UPD_MED_REC',
        'SP_COORD_GET_SRV_ASS',
        'SP_COORD_UPD_SRV_REC',
        'SP_COORD_UPD_TECH'
    ))
   OR (owner = 'HOSPITAL_DBA' AND object_name IN (
        'V_PATIENT_SELF',
        'V_MEDICAL_RECORD_PATIENT',
        'V_PRESCRIPTION_PATIENT',
        'V_SERVICE_RECORD_PATIENT',
        'USP_GET_PATIENT_PROFILE',
        'USP_GET_PATIENT_RECORDS',
        'USP_GET_PATIENT_PRESCRIPTIONS',
        'USP_GET_PATIENT_SERVICES',
        'USP_UPDATE_PATIENT_CONTACT',
        'V_TECHNICIAN_SERVICE_RECORD',
        'GET_TECHNICIAN_SERVICE_RECORDS',
        'UPDATE_TECHNICIAN_SERVICE_RESULT',
        'GET_TECHNICIAN_PERSONAL_INFO',
        'UPDATE_TECHNICIAN_PERSONAL_INFO'
    ))
ORDER BY owner, object_type, object_name;

SELECT object_owner, object_name, policy_name, pf_owner, function
FROM all_policies
WHERE object_owner = 'HOSPITAL'
  AND object_name = 'STAFF'
  AND policy_name IN ('POL_VPD_STAFF_SELF_SELECT', 'POL_VPD_STAFF_SELF_UPDATE')
ORDER BY object_name, policy_name;

-- ==============================================================================
-- OPTIONAL MANUAL REVIEW SECTION - not executed automatically
-- ==============================================================================
-- The combined setup replaced hospital.TRG_VALIDATE_SERVICE_RECORD. Because the
-- original trigger body is unknown, the cleanup script does not drop it by default.
-- Only drop it if you are sure you do not need it:
--
-- DROP TRIGGER hospital.TRG_VALIDATE_SERVICE_RECORD;
--
-- The combined setup changed hospital.service_record.technician_id to allow NULL.
-- Only run a NOT NULL rollback after checking no rows have NULL technician_id:
--
-- SELECT COUNT(*) AS null_technician_rows
-- FROM hospital.service_record
-- WHERE technician_id IS NULL;
--
-- If the count is 0 and your original schema required NOT NULL, you may run:
-- ALTER TABLE hospital.service_record MODIFY technician_id NOT NULL;
--
-- The combined setup may also have inserted a sample unassigned service record:
--   service_type = N'Xét nghiệm máu', technician_id IS NULL, service_result IS NULL
-- Do NOT blindly delete it unless you confirm it is test data.
--
-- Review candidates with:
-- SELECT record_id, service_type, service_date, technician_id, service_result
-- FROM hospital.service_record
-- WHERE service_type = N'Xét nghiệm máu'
--   AND technician_id IS NULL
--   AND service_result IS NULL
-- ORDER BY service_date DESC;
-- ==============================================================================

COMMIT;

PROMPT ==============================================================================
PROMPT DONE: Cleanup Coordinator + Patient + Technician setup
PROMPT ==============================================================================


PROMPT ==============================================================================
PROMPT DONE: Cleanup_All_Roles.sql finished
PROMPT ==============================================================================
