-- ==============================================================================
-- 03_doctor_policies.sql
-- Cháº¡y dÆ°á»›i quyá»n: hospital_dba
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

BEGIN
    -- MEDICAL_RECORD
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL', 
        object_name     => 'medical_record', 
        policy_name     => 'POL_VPD_MEDICAL_RECORD_READ', 
        function_schema => 'HOSPITAL_DBA', 
        policy_function => 'FN_VPD_MEDICAL_RECORD_DOCTOR', 
        statement_types => 'SELECT'
    );
    
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL', 
        object_name     => 'medical_record', 
        policy_name     => 'POL_VPD_MEDICAL_RECORD_UPDATE', 
        function_schema => 'HOSPITAL_DBA', 
        policy_function => 'FN_VPD_MEDICAL_RECORD_DOCTOR', 
        statement_types => 'UPDATE', 
        update_check    => TRUE
    );

    -- PATIENT
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL', 
        object_name     => 'patient', 
        policy_name     => 'POL_VPD_PATIENT_READ', 
        function_schema => 'HOSPITAL_DBA', 
        policy_function => 'FN_VPD_PATIENT_DOCTOR', 
        statement_types => 'SELECT'
    );
    
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL', 
        object_name     => 'patient', 
        policy_name     => 'POL_VPD_PATIENT_UPDATE', 
        function_schema => 'HOSPITAL_DBA', 
        policy_function => 'FN_VPD_PATIENT_DOCTOR', 
        statement_types => 'UPDATE', 
        sec_relevant_cols=> 'medical_history,family_medical_history,drug_allergies',
        update_check    => TRUE
    );

    --SERVICE_RECORD
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL', 
        object_name     => 'service_record', 
        policy_name     => 'POL_VPD_SERVICE_RECORD_READ_DELETE', 
        function_schema => 'HOSPITAL_DBA', 
        policy_function => 'FN_VPD_RECORD_DETAIL_DOCTOR', 
        statement_types => 'SELECT,DELETE'
    );
    
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL', 
        object_name     => 'service_record', 
        policy_name     => 'POL_VPD_SERVICE_RECORD_INSERT', 
        function_schema => 'HOSPITAL_DBA', 
        policy_function => 'FN_VPD_RECORD_DETAIL_DOCTOR', 
        statement_types => 'INSERT', 
        update_check    => TRUE
    );

    -- PRESCRIPTION
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL', 
        object_name     => 'prescription', 
        policy_name     => 'POL_VPD_PRESCRIPTION_READ_DELETE', 
        function_schema => 'HOSPITAL_DBA', 
        policy_function => 'FN_VPD_RECORD_DETAIL_DOCTOR', 
        statement_types => 'SELECT,DELETE'
    );
    
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL', 
        object_name     => 'prescription', 
        policy_name     => 'POL_VPD_PRESCRIPTION_UPDATE', 
        function_schema => 'HOSPITAL_DBA', 
        policy_function => 'FN_VPD_RECORD_DETAIL_DOCTOR', 
        statement_types => 'INSERT,UPDATE', 
        update_check    => TRUE
    );
END;
/
