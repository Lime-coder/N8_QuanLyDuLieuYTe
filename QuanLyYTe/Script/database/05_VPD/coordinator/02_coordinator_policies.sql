-- ==============================================================================
-- 02_coordinator_policies.sql
-- Chạy dưới quyền: hospital_dba
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

BEGIN
    BEGIN
        DBMS_RLS.DROP_POLICY('HOSPITAL', 'STAFF', 'POL_VPD_STAFF_SELF_SELECT');
    EXCEPTION WHEN OTHERS THEN NULL; END;

    BEGIN
        DBMS_RLS.DROP_POLICY('HOSPITAL', 'STAFF', 'POL_VPD_STAFF_SELF_UPDATE');
    EXCEPTION WHEN OTHERS THEN NULL; END;

    BEGIN
        DBMS_RLS.DROP_POLICY('HOSPITAL', 'MEDICAL_RECORD', 'POL_VPD_MED_REC_COORD_UPD');
    EXCEPTION WHEN OTHERS THEN NULL; END;

    BEGIN
        DBMS_RLS.DROP_POLICY('HOSPITAL', 'SERVICE_RECORD', 'POL_VPD_SRV_REC_COORD_UPD');
    EXCEPTION WHEN OTHERS THEN NULL; END;

    -- Policy 1: SELECT trực tiếp STAFF chỉ thấy chính mình
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL',
        object_name     => 'STAFF',
        policy_name     => 'POL_VPD_STAFF_SELF_SELECT',
        function_schema => 'HOSPITAL_DBA',
        policy_function => 'FN_VPD_STAFF_SELF',
        statement_types => 'SELECT'
    );

    -- Policy 2: UPDATE phone, hometown chỉ trên dòng chính mình
    DBMS_RLS.ADD_POLICY(
        object_schema     => 'HOSPITAL',
        object_name       => 'STAFF',
        policy_name       => 'POL_VPD_STAFF_SELF_UPDATE',
        function_schema   => 'HOSPITAL_DBA',
        policy_function   => 'FN_VPD_STAFF_SELF',
        statement_types   => 'UPDATE',
        sec_relevant_cols => 'PHONE,HOMETOWN'
    );

    -- Policy 3: Điều phối viên không được UPDATE các trường khác ngoài MÃKHOA, MÃBS trên MEDICAL_RECORD
    DBMS_RLS.ADD_POLICY(
        object_schema     => 'HOSPITAL',
        object_name       => 'MEDICAL_RECORD',
        policy_name       => 'POL_VPD_MED_REC_COORD_UPD',
        function_schema   => 'HOSPITAL_DBA',
        policy_function   => 'FN_VPD_COORD_RESTRICT_UPD',
        statement_types   => 'UPDATE',
        sec_relevant_cols => 'PATIENT_ID,RECORD_DATE,DIAGNOSIS,TREATMENT_PLAN,CONCLUSION'
    );

    -- Policy 4: Điều phối viên không được UPDATE các trường khác ngoài MÃKTV trên SERVICE_RECORD
    DBMS_RLS.ADD_POLICY(
        object_schema     => 'HOSPITAL',
        object_name       => 'SERVICE_RECORD',
        policy_name       => 'POL_VPD_SRV_REC_COORD_UPD',
        function_schema   => 'HOSPITAL_DBA',
        policy_function   => 'FN_VPD_COORD_RESTRICT_UPD',
        statement_types   => 'UPDATE',
        sec_relevant_cols => 'SERVICE_TYPE,SERVICE_DATE,SERVICE_RESULT'
    );
END;
/
