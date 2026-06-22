-- ==============================================================================
-- 04_doctor_procedures.sql
-- Chạy dưới quyền: hospital
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

-- A. MEDICAL_RECORD
CREATE OR REPLACE PROCEDURE USP_GET_MEDICAL_RECORD(p_s NVARCHAR2, p_c OUT SYS_REFCURSOR) AS 
BEGIN 
    OPEN p_c FOR 
    SELECT m.record_id, m.patient_id, p.full_name, 
           TO_CHAR(m.record_date, 'DD/MM/YYYY') as record_date, 
           m.diagnosis, m.treatment_plan, m.conclusion 
    FROM hospital.medical_record m 
    JOIN hospital.patient p ON m.patient_id = p.patient_id
    WHERE m.record_id LIKE '%'||UPPER(p_s)||'%' 
       OR m.patient_id LIKE '%'||UPPER(p_s)||'%'
       OR p.full_name LIKE '%'||p_s||'%'
       OR m.diagnosis LIKE '%'||p_s||'%'
       OR m.treatment_plan LIKE '%'||p_s||'%'
       OR m.conclusion LIKE '%'||p_s||'%'
       OR TO_CHAR(m.record_date, 'DD/MM/YYYY') LIKE '%'||p_s||'%';
END;
/

CREATE OR REPLACE PROCEDURE USP_UPDATE_MEDICAL_RECORD(p_id VARCHAR2, p_dg NVARCHAR2, p_tr NVARCHAR2, p_cl NVARCHAR2) AS
    v_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_count FROM hospital.medical_record WHERE record_id = p_id;
    IF v_count = 0 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Lá»—i: KhÃ´ng tÃ¬m tháº¥y há»“ sÆ¡ bá»‡nh Ã¡n mÃ£ ' || p_id || ' Ä‘á»ƒ cáº­p nháº­t!');
    END IF;

    UPDATE hospital.medical_record SET diagnosis = p_dg, treatment_plan = p_tr, conclusion = p_cl WHERE record_id = p_id;
END;
/

-- B. SERVICE_RECORD
CREATE OR REPLACE PROCEDURE USP_GET_SERVICES(p_s NVARCHAR2, p_c OUT SYS_REFCURSOR) AS
BEGIN 
    OPEN p_c FOR 
    SELECT record_id, service_type, TO_CHAR(service_date, 'DD/MM/YYYY') as service_date, technician_id, service_result 
    FROM hospital.service_record 
    WHERE record_id LIKE '%'||UPPER(p_s)||'%'
    ORDER BY service_record.service_date DESC;
END;
/

CREATE OR REPLACE PROCEDURE USP_ADD_SERVICE(p_id VARCHAR2, p_type NVARCHAR2) AS
    v_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_count FROM hospital.medical_record WHERE record_id = UPPER(TRIM(p_id));
    IF v_count = 0 THEN RAISE_APPLICATION_ERROR(-20001, 'MÃ£ HSBA ' || p_id || ' khÃ´ng tá»“n táº¡i!'); END IF;

    INSERT INTO hospital.service_record VALUES (UPPER(TRIM(p_id)), p_type, TRUNC(SYSDATE), NULL, NULL);
END;
/

CREATE OR REPLACE PROCEDURE USP_DELETE_SERVICE(p_id VARCHAR2, p_type NVARCHAR2, p_date DATE) AS
BEGIN
    DELETE FROM hospital.service_record 
    WHERE record_id = UPPER(TRIM(p_id)) 
      AND service_type = p_type 
      AND TRUNC(service_date) = TRUNC(p_date);
END;
/

-- C. PRESCRIPTION
CREATE OR REPLACE PROCEDURE USP_GET_PRESCRIPTION(p_s NVARCHAR2, p_c OUT SYS_REFCURSOR) AS
BEGIN 
    OPEN p_c FOR 
    SELECT record_id, TO_CHAR(prescription_date, 'DD/MM/YYYY') as prescription_date, medicine_name, dosage 
    FROM hospital.prescription 
    WHERE record_id LIKE '%'||UPPER(TRIM(p_s))||'%'
       OR medicine_name LIKE '%'||p_s||'%'
       OR dosage LIKE '%'||p_s||'%'
       OR TO_CHAR(prescription_date, 'DD/MM/YYYY') LIKE '%'||p_s||'%';
END;
/

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
        SELECT COUNT(*) INTO v_count FROM hospital.medical_record WHERE record_id = v_id;
        IF v_count = 0 THEN RAISE_APPLICATION_ERROR(-20003, 'Lá»—i: MÃ£ HSBA ' || v_id || ' khÃ´ng tá»“n táº¡i!'); END IF;
    END IF;

    IF p_action = 'INSERT' THEN
        SELECT COUNT(*) INTO v_count FROM hospital.prescription 
        WHERE record_id = v_id AND TRUNC(prescription_date) = TRUNC(SYSDATE) AND medicine_name = p_med_name;
        
        IF v_count > 0 THEN
            RAISE_APPLICATION_ERROR(-20005, 'Lá»—i: Thuá»‘c ' || p_med_name || ' Ä‘Ã£ Ä‘Æ°á»£c kÃª trong Ä‘Æ¡n cá»§a ngÃ y hÃ´m nay rá»“i!');
        END IF;

        INSERT INTO hospital.prescription (record_id, prescription_date, medicine_name, dosage)
        VALUES (v_id, TRUNC(SYSDATE), p_med_name, p_dosage);
        
    ELSIF p_action = 'UPDATE' THEN
        UPDATE hospital.prescription 
        SET medicine_name = p_med_name, dosage = p_dosage 
        WHERE record_id = v_id AND TRUNC(prescription_date) = TRUNC(p_date) AND medicine_name = p_old_med_name; 
          
    ELSIF p_action = 'DELETE' THEN
        DELETE FROM hospital.prescription 
        WHERE record_id = v_id AND TRUNC(prescription_date) = TRUNC(p_date) AND medicine_name = p_med_name;
    END IF;
END;
/

-- D. PATIENT
CREATE OR REPLACE PROCEDURE USP_GET_PATIENTS(p_s NVARCHAR2, p_c OUT SYS_REFCURSOR) AS
BEGIN 
    OPEN p_c FOR 
    SELECT p.patient_id, p.full_name, p.gender, 
           TO_CHAR(p.birthdate, 'DD/MM/YYYY') as birthdate, 
           p.medical_history, p.family_medical_history, p.drug_allergies 
    FROM hospital.patient p
    WHERE (
        p.full_name LIKE '%'||p_s||'%' 
        OR p.patient_id LIKE '%'||UPPER(TRIM(p_s))||'%'
        OR p.gender LIKE '%'||p_s||'%'
        OR p.medical_history LIKE '%'||p_s||'%'
        OR EXISTS (
            SELECT 1 FROM hospital.medical_record m 
            WHERE m.patient_id = p.patient_id
            AND (
                m.record_id LIKE '%'||UPPER(TRIM(p_s))||'%'
                OR m.diagnosis LIKE '%'||p_s||'%'
                OR m.treatment_plan LIKE '%'||p_s||'%'
                OR m.conclusion LIKE '%'||p_s||'%'
                OR TO_CHAR(m.record_date, 'DD/MM/YYYY') LIKE '%'||p_s||'%'
            )
        )
    )
    ORDER BY p.patient_id ASC;
END;
/

CREATE OR REPLACE PROCEDURE USP_UPDATE_PATIENT(
    p_id VARCHAR2, 
    p_history NCLOB, 
    p_family_history NCLOB, 
    p_allergy NCLOB
) AS
    v_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_count FROM hospital.patient WHERE patient_id = UPPER(TRIM(p_id));
    IF v_count = 0 THEN
        RAISE_APPLICATION_ERROR(-20004, 'Lá»—i: KhÃ´ng tÃ¬m tháº¥y mÃ£ bá»‡nh nhÃ¢n ' || p_id || ' Ä‘á»ƒ cáº­p nháº­t!');
    END IF;

    UPDATE hospital.patient 
    SET medical_history = p_history, 
        family_medical_history = p_family_history, 
        drug_allergies = p_allergy 
    WHERE patient_id = UPPER(TRIM(p_id));
END;
/

-- E. TC#5
CREATE OR REPLACE PROCEDURE USP_GET_SELF_INFO(p_c OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_c FOR 
    SELECT s.full_name, s.gender, s.birthdate, s.id_card, s.hometown, s.phone, s.staff_role, d.dept_name
    FROM hospital.staff s
    JOIN hospital.department d ON d.dept_id = s.dept_id
    WHERE UPPER(username_db) = SYS_CONTEXT('USERENV', 'SESSION_USER');
END;
/

CREATE OR REPLACE PROCEDURE USP_UPDATE_SELF_INFO(p_hometown NVARCHAR2, p_phone VARCHAR2) AS
BEGIN
    UPDATE hospital.staff SET hometown = p_hometown, phone = p_phone 
    WHERE UPPER(username_db) = SYS_CONTEXT('USERENV', 'SESSION_USER');
    COMMIT;
END;
/
