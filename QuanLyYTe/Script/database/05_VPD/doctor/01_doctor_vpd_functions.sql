-- ==============================================================================
-- 02_doctor_vpd_functions.sql
-- Chạy dưới quyền: hospital_dba
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

CREATE OR REPLACE FUNCTION FN_VPD_MEDICAL_RECORD_DOCTOR (
    p_schema IN VARCHAR2, 
    p_obj IN VARCHAR2
) RETURN VARCHAR2 AS
    v_staff_id VARCHAR2(10);
    v_user     VARCHAR2(100);
BEGIN
    v_user := SYS_CONTEXT('USERENV', 'SESSION_USER');
    
    IF v_user IN ('HOSPITAL_DBA', 'HOSPITAL') THEN RETURN '1=1'; END IF;
    
    BEGIN
        SELECT staff_id INTO v_staff_id 
        FROM hospital.staff 
        WHERE UPPER(username_db) = v_user;
    EXCEPTION WHEN NO_DATA_FOUND THEN RETURN '1=0';
    END;
    
    RETURN 'doctor_id = ''' || v_staff_id || '''';
END;
/

CREATE OR REPLACE FUNCTION FN_VPD_PATIENT_DOCTOR (
    p_schema IN VARCHAR2, 
    p_obj IN VARCHAR2
) RETURN VARCHAR2 AS
    v_staff_id VARCHAR2(10);
    v_user     VARCHAR2(100);
BEGIN
    v_user := SYS_CONTEXT('USERENV', 'SESSION_USER');
    
    IF v_user IN ('HOSPITAL_DBA', 'HOSPITAL') THEN RETURN '1=1'; END IF;
    
    BEGIN
        SELECT staff_id INTO v_staff_id 
        FROM hospital.staff 
        WHERE UPPER(username_db) = v_user;
    EXCEPTION WHEN NO_DATA_FOUND THEN RETURN '1=0';
    END;
    
    RETURN 'patient_id IN (SELECT m.patient_id FROM hospital.medical_record m WHERE m.doctor_id = ''' || v_staff_id || ''')';
END;
/

CREATE OR REPLACE FUNCTION FN_VPD_RECORD_DETAIL_DOCTOR (
    p_schema IN VARCHAR2, 
    p_obj IN VARCHAR2
) RETURN VARCHAR2 AS
    v_staff_id VARCHAR2(10);
    v_user     VARCHAR2(100);
BEGIN
    v_user := SYS_CONTEXT('USERENV', 'SESSION_USER');
    
    IF v_user IN ('HOSPITAL_DBA', 'HOSPITAL') THEN RETURN '1=1'; END IF;
    
    BEGIN
        SELECT staff_id INTO v_staff_id 
        FROM hospital.staff 
        WHERE UPPER(username_db) = v_user;
    EXCEPTION WHEN NO_DATA_FOUND THEN RETURN '1=0';
    
    END;
    
    RETURN 'record_id IN (SELECT m.record_id FROM hospital.medical_record m WHERE m.doctor_id = ''' || v_staff_id || ''')';
END;
/
