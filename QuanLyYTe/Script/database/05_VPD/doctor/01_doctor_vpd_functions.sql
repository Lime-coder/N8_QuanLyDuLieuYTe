-- ==============================================================================
-- 02_doctor_vpd_functions.sql
-- Chạy dưới quyền: hospital_dba
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

CREATE OR REPLACE FUNCTION FN_VPD_MEDICAL_RECORD_DOCTOR (
    p_schema IN VARCHAR2, 
    p_obj IN VARCHAR2
) RETURN VARCHAR2 AS
    v_staff_id VARCHAR2(10);
    v_user     VARCHAR2(100);
    v_role     NVARCHAR2(50);
BEGIN
    v_user := SYS_CONTEXT('USERENV', 'SESSION_USER');
    
    IF v_user IN ('HOSPITAL_DBA', 'HOSPITAL') THEN RETURN '1=1'; END IF;
    
    BEGIN
        SELECT staff_id, staff_role INTO v_staff_id, v_role 
        FROM hospital.staff 
        WHERE UPPER(username_db) = v_user;
    EXCEPTION WHEN NO_DATA_FOUND THEN 
        RETURN 'patient_id IN (SELECT patient_id FROM hospital.patient WHERE UPPER(username_db) = ''' || v_user || ''')';
    END;
    
    IF v_role NOT IN (UNISTR('B\00E1c s\0129'), UNISTR('B\00E1c s\0129/Y s\0129')) THEN
        RETURN '1=1';
    END IF;
    
    RETURN 'doctor_id = ''' || v_staff_id || '''';
END;
/

CREATE OR REPLACE FUNCTION FN_VPD_PATIENT_DOCTOR (
    p_schema IN VARCHAR2, 
    p_obj IN VARCHAR2
) RETURN VARCHAR2 AS
    v_staff_id VARCHAR2(10);
    v_user     VARCHAR2(100);
    v_role     NVARCHAR2(50);
BEGIN
    v_user := SYS_CONTEXT('USERENV', 'SESSION_USER');
    
    IF v_user IN ('HOSPITAL_DBA', 'HOSPITAL') THEN RETURN '1=1'; END IF;
    
    BEGIN
        SELECT staff_id, staff_role INTO v_staff_id, v_role 
        FROM hospital.staff 
        WHERE UPPER(username_db) = v_user;
    EXCEPTION WHEN NO_DATA_FOUND THEN 
        RETURN 'UPPER(username_db) = ''' || v_user || '''';
    END;
    
    IF v_role NOT IN (UNISTR('B\00E1c s\0129'), UNISTR('B\00E1c s\0129/Y s\0129')) THEN
        RETURN '1=1';
    END IF;
    
    RETURN 'patient_id IN (SELECT m.patient_id FROM hospital.medical_record m WHERE m.doctor_id = ''' || v_staff_id || ''')';
END;
/

CREATE OR REPLACE FUNCTION FN_VPD_RECORD_DETAIL_DOCTOR (
    p_schema IN VARCHAR2, 
    p_obj IN VARCHAR2
) RETURN VARCHAR2 AS
    v_staff_id VARCHAR2(10);
    v_user     VARCHAR2(100);
    v_role     NVARCHAR2(50);
BEGIN
    v_user := SYS_CONTEXT('USERENV', 'SESSION_USER');
    
    IF v_user IN ('HOSPITAL_DBA', 'HOSPITAL') THEN RETURN '1=1'; END IF;
    
    BEGIN
        SELECT staff_id, staff_role INTO v_staff_id, v_role 
        FROM hospital.staff 
        WHERE UPPER(username_db) = v_user;
    EXCEPTION WHEN NO_DATA_FOUND THEN 
        RETURN 'record_id IN (SELECT mr.record_id FROM hospital.medical_record mr JOIN hospital.patient p ON mr.patient_id = p.patient_id WHERE UPPER(p.username_db) = ''' || v_user || ''')';
    END;
    
    IF v_role NOT IN (UNISTR('B\00E1c s\0129'), UNISTR('B\00E1c s\0129/Y s\0129')) THEN
        RETURN '1=1';
    END IF;
    
    RETURN 'record_id IN (SELECT m.record_id FROM hospital.medical_record m WHERE m.doctor_id = ''' || v_staff_id || ''')';
END;
/

