-- Run as: hospital_dba | Container: PDB_QLYT
ALTER SESSION SET CURRENT_SCHEMA = hospital;

--------------------------------------------------------------------------------
-- 1. DELETE FUNCTIONS
--------------------------------------------------------------------------------
BEGIN
    FOR p IN (
        SELECT policy_name, object_name 
        FROM all_policies 
        WHERE object_owner = 'HOSPITAL' AND policy_name LIKE 'POL_VPD_%'
    ) 
    LOOP
        DBMS_RLS.DROP_POLICY('HOSPITAL', p.object_name, p.policy_name);
    END LOOP;
    FOR f IN (
        SELECT object_name 
        FROM all_objects 
        WHERE owner = 'HOSPITAL' AND object_type = 'FUNCTION' AND object_name LIKE 'FN_VPD_%'
    ) 
    LOOP
        EXECUTE IMMEDIATE 'DROP FUNCTION hospital.' || f.object_name;
    END LOOP;
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

--------------------------------------------------------------------------------
-- 2. GRANT PRIVILEGES (TC#3)
--------------------------------------------------------------------------------
GRANT SELECT, UPDATE(medical_history, family_medical_history, drug_allergies) ON patient TO rl_doctor;
GRANT SELECT, UPDATE(diagnosis, treatment_plan, conclusion) ON medical_record TO rl_doctor;
GRANT SELECT, INSERT, DELETE ON service_record TO rl_doctor;
GRANT SELECT, INSERT, UPDATE, DELETE ON prescription TO rl_doctor;
GRANT SELECT ON staff TO rl_doctor;

--------------------------------------------------------------------------------
-- 3. CREATE POLICY FUNCTIONS
--------------------------------------------------------------------------------

-- A. Function for MEDICAL_RECORD (TC#3a)
CREATE OR REPLACE FUNCTION FN_VPD_MEDICAL_RECORD_DOCTOR (
    p_schema IN VARCHAR2, 
    p_obj IN VARCHAR2
) RETURN VARCHAR2 AS
    v_staff_id VARCHAR2(10);
    v_user     VARCHAR2(100);
BEGIN
    v_user := SYS_CONTEXT('USERENV', 'SESSION_USER');
    
    IF v_user = 'HOSPITAL_DBA' THEN RETURN '1=1'; END IF;
    
    BEGIN
        SELECT staff_id INTO v_staff_id 
        FROM hospital.staff 
        WHERE UPPER(username_db) = v_user;
    EXCEPTION WHEN NO_DATA_FOUND THEN RETURN '1=0';
    END;
    
    RETURN 'doctor_id = ''' || v_staff_id || '''';
END;
/

-- B. Function for PATIENT (TC#3d)
CREATE OR REPLACE FUNCTION FN_VPD_PATIENT_DOCTOR (
    p_schema IN VARCHAR2, 
    p_obj IN VARCHAR2
) RETURN VARCHAR2 AS
    v_staff_id VARCHAR2(10);
    v_user     VARCHAR2(100);
BEGIN
    v_user := SYS_CONTEXT('USERENV', 'SESSION_USER');
    
    IF v_user = 'HOSPITAL_DBA' THEN RETURN '1=1'; END IF;
    
    BEGIN
        SELECT staff_id INTO v_staff_id 
        FROM hospital.staff 
        WHERE UPPER(username_db) = v_user;
    EXCEPTION WHEN NO_DATA_FOUND THEN RETURN '1=0';
    END;
    
    RETURN 'patient_id IN (SELECT m.patient_id FROM hospital.medical_record m WHERE m.doctor_id = ''' || v_staff_id || ''')';
END;
/

-- C. Function for SERVICE_RECORD and PRESCRIPTION (TC#3b, TC#3e)
CREATE OR REPLACE FUNCTION FN_VPD_RECORD_DETAIL_DOCTOR (
    p_schema IN VARCHAR2, 
    p_obj IN VARCHAR2
) RETURN VARCHAR2 AS
    v_staff_id VARCHAR2(10);
    v_user     VARCHAR2(100);
BEGIN
    v_user := SYS_CONTEXT('USERENV', 'SESSION_USER');
    
    IF v_user = 'HOSPITAL_DBA' THEN RETURN '1=1'; END IF;
    
    BEGIN
        SELECT staff_id INTO v_staff_id 
        FROM hospital.staff 
        WHERE UPPER(username_db) = v_user;
    EXCEPTION WHEN NO_DATA_FOUND THEN RETURN '1=0';
    
    END;
    
    RETURN 'record_id IN (SELECT m.record_id FROM hospital.medical_record m WHERE m.doctor_id = ''' || v_staff_id || ''')';
END;
/

--------------------------------------------------------------------------------
-- 4. APPLY POLICY FUNCTIONS
--------------------------------------------------------------------------------
BEGIN
    -- MEDICAL_RECORD
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL', 
        object_name     => 'medical_record', 
        policy_name     => 'POL_VPD_MEDICAL_RECORD_READ', 
        function_schema => 'HOSPITAL', 
        policy_function => 'FN_VPD_MEDICAL_RECORD_DOCTOR', 
        statement_types => 'SELECT'
    );
    
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL', 
        object_name     => 'medical_record', 
        policy_name     => 'POL_VPD_MEDICAL_RECORD_UPDATE', 
        function_schema => 'HOSPITAL', 
        policy_function => 'FN_VPD_MEDICAL_RECORD_DOCTOR', 
        statement_types => 'UPDATE', 
        update_check    => TRUE
    );

    -- PATIENT
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL', 
        object_name     => 'patient', 
        policy_name     => 'POL_VPD_PATIENT_READ', 
        function_schema => 'HOSPITAL', 
        policy_function => 'FN_VPD_PATIENT_DOCTOR', 
        statement_types => 'SELECT'
    );
    
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL', 
        object_name     => 'patient', 
        policy_name     => 'POL_VPD_PATIENT_UPDATE', 
        function_schema => 'HOSPITAL', 
        policy_function => 'FN_VPD_PATIENT_DOCTOR', 
        statement_types => 'UPDATE', 
        update_check    => TRUE
    );

    --SERVICE_RECORD
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL', 
        object_name     => 'service_record', 
        policy_name     => 'POL_VPD_SERVICE_RECORD_READ_DELETE', 
        function_schema => 'HOSPITAL', 
        policy_function => 'FN_VPD_RECORD_DETAIL_DOCTOR', 
        statement_types => 'SELECT,DELETE'
    );
    
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL', 
        object_name     => 'service_record', 
        policy_name     => 'POL_VPD_SERVICE_RECORD_INSERT', 
        function_schema => 'HOSPITAL', 
        policy_function => 'FN_VPD_RECORD_DETAIL_DOCTOR', 
        statement_types => 'INSERT', 
        update_check    => TRUE
    );

    -- PRESCRIPTION
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL', 
        object_name     => 'prescription', 
        policy_name     => 'POL_VPD_PRESCRIPTION_READ_DELETE', 
        function_schema => 'HOSPITAL', 
        policy_function => 'FN_VPD_RECORD_DETAIL_DOCTOR', 
        statement_types => 'SELECT,DELETE'
    );
    
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL', 
        object_name     => 'prescription', 
        policy_name     => 'POL_VPD_PRESCRIPTION_UPDATE', 
        function_schema => 'HOSPITAL', 
        policy_function => 'FN_VPD_RECORD_DETAIL_DOCTOR', 
        statement_types => 'INSERT,UPDATE', 
        update_check    => TRUE
    );
END;
/