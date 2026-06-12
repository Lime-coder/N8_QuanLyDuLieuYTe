-- Run as: hospital_dba
ALTER SESSION SET CURRENT_SCHEMA = hospital;

BEGIN
    FOR p IN (SELECT object_name FROM all_objects WHERE owner = 'HOSPITAL' AND object_type = 'PROCEDURE' AND object_name LIKE 'USP_DOCTOR_%') 
    LOOP EXECUTE IMMEDIATE 'DROP PROCEDURE hospital.' || p.object_name; END LOOP;
END;
/

-- A. MEDICAL_RECORD
-- 1. Stored Procedure Doctors get Medical Records
CREATE OR REPLACE PROCEDURE USP_GET_MEDICAL_RECORD(p_s NVARCHAR2, p_c OUT SYS_REFCURSOR) AS 
BEGIN 
    OPEN p_c FOR 
    SELECT m.record_id, m.patient_id, p.full_name, 
           TO_CHAR(m.record_date, 'DD/MM/YYYY') as record_date, 
           m.diagnosis, m.treatment_plan, m.conclusion 
    FROM medical_record m 
    JOIN patient p ON m.patient_id = p.patient_id
    WHERE m.record_id LIKE '%'||UPPER(p_s)||'%' 
       OR m.patient_id LIKE '%'||UPPER(p_s)||'%'
       OR p.full_name LIKE '%'||p_s||'%'
       OR m.diagnosis LIKE '%'||p_s||'%'
       OR m.treatment_plan LIKE '%'||p_s||'%'
       OR m.conclusion LIKE '%'||p_s||'%'
       OR TO_CHAR(m.record_date, 'DD/MM/YYYY') LIKE '%'||p_s||'%';
END;
/

-- 2. Stored Procedure Doctors update Medical Record
CREATE OR REPLACE PROCEDURE USP_UPDATE_MEDICAL_RECORD(p_id VARCHAR2, p_dg NVARCHAR2, p_tr NVARCHAR2, p_cl NVARCHAR2) AS
    v_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_count FROM medical_record WHERE record_id = p_id;
    IF v_count = 0 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Lỗi: Không tìm thấy hồ sơ bệnh án mã ' || p_id || ' để cập nhật!');
    END IF;

    UPDATE medical_record SET diagnosis = p_dg, treatment_plan = p_tr, conclusion = p_cl WHERE record_id = p_id;
END;
/

-- B. SERVICE_RECORD
-- 1. Stored Procedure Doctors get Service Records
CREATE OR REPLACE PROCEDURE USP_GET_SERVICES(p_s NVARCHAR2, p_c OUT SYS_REFCURSOR) AS
BEGIN 
    OPEN p_c FOR 
    SELECT record_id, service_type, TO_CHAR(service_date, 'DD/MM/YYYY') as service_date, technician_id, service_result 
    FROM service_record 
    WHERE record_id LIKE '%'||UPPER(p_s)||'%'
    ORDER BY service_record.service_date DESC;
END;
/

-- 2. Stored Procedure Doctors insert Service Record
CREATE OR REPLACE PROCEDURE USP_ADD_SERVICE(p_id VARCHAR2, p_type NVARCHAR2, p_res NVARCHAR2) AS
    v_count NUMBER; v_ktv_id VARCHAR2(10);
BEGIN
    SELECT COUNT(*) INTO v_count FROM medical_record WHERE record_id = UPPER(TRIM(p_id));
    IF v_count = 0 THEN RAISE_APPLICATION_ERROR(-20001, 'Mã HSBA ' || p_id || ' không tồn tại!'); END IF;

    SELECT staff_id INTO v_ktv_id FROM (SELECT staff_id FROM staff WHERE staff_role = N'Kỹ thuật viên' ORDER BY DBMS_RANDOM.VALUE) WHERE ROWNUM = 1;
    
    INSERT INTO service_record VALUES (UPPER(TRIM(p_id)), p_type, TRUNC(SYSDATE), v_ktv_id, p_res);
END;
/

-- 3. Stored Procedure Doctors delete Service Record
CREATE OR REPLACE PROCEDURE USP_DELETE_SERVICE(p_id VARCHAR2, p_type NVARCHAR2, p_date DATE) AS
BEGIN
    DELETE FROM service_record 
    WHERE record_id = UPPER(TRIM(p_id)) 
      AND service_type = p_type 
      AND TRUNC(service_date) = TRUNC(p_date);
END;
/

-- C. PRESCRIPTION
-- 1. Stored Procedure Doctors get Prescriptions
CREATE OR REPLACE PROCEDURE USP_GET_PRESCRIPTION(p_s NVARCHAR2, p_c OUT SYS_REFCURSOR) AS
BEGIN 
    OPEN p_c FOR 
    SELECT record_id, TO_CHAR(prescription_date, 'DD/MM/YYYY') as prescription_date, medicine_name, dosage 
    FROM prescription 
    WHERE record_id LIKE '%'||UPPER(TRIM(p_s))||'%'
       OR medicine_name LIKE '%'||p_s||'%'
       OR dosage LIKE '%'||p_s||'%'
       OR TO_CHAR(prescription_date, 'DD/MM/YYYY') LIKE '%'||p_s||'%';
END;
/

-- 2. Stored Procerdure Doctors manage Prescriptions
CREATE OR REPLACE PROCEDURE USP_MANAGE_PRESCRIPTION(
    p_action VARCHAR2, 
    p_record_id VARCHAR2, 
    p_med_name NVARCHAR2,  
    p_dosage NVARCHAR2,
    p_date DATE DEFAULT NULL,
    p_old_med_name NVARCHAR2 DEFAULT NULL
) AS
    v_count NUMBER;
    v_id VARCHAR2(10) := UPPER(TRIM(p_record_id));
BEGIN
    IF p_action IN ('INSERT', 'UPDATE') THEN
        SELECT COUNT(*) INTO v_count FROM medical_record WHERE record_id = v_id;
        IF v_count = 0 THEN RAISE_APPLICATION_ERROR(-20003, 'Lỗi: Mã HSBA ' || v_id || ' không tồn tại!'); END IF;
    END IF;

    IF p_action = 'INSERT' THEN
        SELECT COUNT(*) INTO v_count FROM prescription 
        WHERE record_id = v_id AND TRUNC(prescription_date) = TRUNC(SYSDATE) AND medicine_name = p_med_name;
        
        IF v_count > 0 THEN
            RAISE_APPLICATION_ERROR(-20005, 'Lỗi: Thuốc ' || p_med_name || ' đã được kê trong đơn của ngày hôm nay rồi!');
        END IF;

        INSERT INTO prescription (record_id, prescription_date, medicine_name, dosage)
        VALUES (v_id, TRUNC(SYSDATE), p_med_name, p_dosage);
        
    ELSIF p_action = 'UPDATE' THEN
        UPDATE prescription 
        SET medicine_name = p_med_name, dosage = p_dosage 
        WHERE record_id = v_id AND TRUNC(prescription_date) = TRUNC(p_date) AND medicine_name = p_old_med_name; 
          
    ELSIF p_action = 'DELETE' THEN
        DELETE FROM prescription 
        WHERE record_id = v_id AND TRUNC(prescription_date) = TRUNC(p_date) AND medicine_name = p_med_name;
    END IF;
END;
/

-- D. PATIENT
-- 1. Stored Procedure Doctors get Patients
CREATE OR REPLACE PROCEDURE USP_GET_PATIENTS(p_s NVARCHAR2, p_c OUT SYS_REFCURSOR) AS
BEGIN 
    OPEN p_c FOR 
    SELECT patient_id, full_name, gender, TO_CHAR(birthdate, 'DD/MM/YYYY') as birthdate, medical_history, family_medical_history, drug_allergies 
    FROM patient 
    WHERE full_name LIKE '%'||p_s||'%' 
       OR patient_id LIKE '%'||UPPER(TRIM(p_s))||'%'
       OR gender LIKE '%'||p_s||'%'
       OR TO_CHAR(birthdate, 'DD/MM/YYYY') LIKE '%'||p_s||'%'
       OR medical_history LIKE '%'||p_s||'%';
END;
/

-- 2. Stored Procedure Doctors update Patient
CREATE OR REPLACE PROCEDURE USP_UPDATE_PATIENT(
    p_id VARCHAR2, 
    p_history CLOB, 
    p_family_history CLOB, 
    p_allergy CLOB
) AS
    v_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_count FROM patient WHERE patient_id = UPPER(TRIM(p_id));
    IF v_count = 0 THEN
        RAISE_APPLICATION_ERROR(-20004, 'Lỗi: Không tìm thấy mã bệnh nhân ' || p_id || ' để cập nhật!');
    END IF;

    UPDATE patient 
    SET medical_history = p_history, 
        family_medical_history = p_family_history, 
        drug_allergies = p_allergy 
    WHERE patient_id = UPPER(TRIM(p_id));
END;
/

-- E. TC#5
-- 1. Stored Procedure Doctors get their profile
CREATE OR REPLACE PROCEDURE USP_GET_SELF_INFO(p_c OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_c FOR 
    SELECT s.full_name, s.gender, s.birthdate, s.id_card, s.hometown, s.phone, s.staff_role, d.dept_name
    FROM staff s
    JOIN department d ON d.dept_id = s.dept_id
    WHERE UPPER(username_db) = SYS_CONTEXT('USERENV', 'SESSION_USER');
END;
/

-- 2. Stored Procedure Doctors update their profile  
CREATE OR REPLACE PROCEDURE USP_UPDATE_SELF_INFO(p_hometown NVARCHAR2, p_phone VARCHAR2) AS
BEGIN
    UPDATE staff SET hometown = p_hometown, phone = p_phone 
    WHERE UPPER(username_db) = SYS_CONTEXT('USERENV', 'SESSION_USER');
END;
/

GRANT EXECUTE ON hospital.USP_GET_MEDICAL_RECORD TO rl_doctor;
GRANT EXECUTE ON hospital.USP_UPDATE_MEDICAL_RECORD TO rl_doctor;

GRANT EXECUTE ON hospital.USP_GET_SERVICES TO rl_doctor;
GRANT EXECUTE ON hospital.USP_ADD_SERVICE TO rl_doctor;
GRANT EXECUTE ON hospital.USP_DELETE_SERVICE TO rl_doctor;

GRANT EXECUTE ON hospital.USP_GET_PRESCRIPTION TO rl_doctor;
GRANT EXECUTE ON hospital.USP_MANAGE_PRESCRIPTION TO rl_doctor;

GRANT EXECUTE ON hospital.USP_GET_PATIENTS TO rl_doctor;
GRANT EXECUTE ON hospital.USP_UPDATE_PATIENT TO rl_doctor;

GRANT EXECUTE ON hospital.USP_GET_SELF_INFO TO rl_doctor;
GRANT EXECUTE ON hospital.USP_UPDATE_SELF_INFO TO rl_doctor;
