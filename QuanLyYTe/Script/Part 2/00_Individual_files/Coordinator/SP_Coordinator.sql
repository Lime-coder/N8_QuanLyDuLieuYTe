-- ==============================================================================
-- File: SP_Coordinator.sql
-- Mục đích: Stored Procedures thay thế cho raw SQL trong CoordinatorRepository
-- Run as: HOSPITAL_DBA
-- ==============================================================================

ALTER SESSION SET CURRENT_SCHEMA = hospital;

-- 1. Bác sĩ
CREATE OR REPLACE PROCEDURE SP_COORD_GET_DOCTORS(p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT username_db, staff_id, full_name, specialty, TO_NCHAR(full_name) || N' - ' || TO_NCHAR(specialty) AS display_name 
    FROM hospital.VW_COORD_DOCTORS 
    ORDER BY full_name;
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_GET_DOC_DEPT(p_dept_id IN VARCHAR2, p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT username_db, staff_id, full_name, specialty, TO_NCHAR(full_name) || N' - ' || TO_NCHAR(specialty) AS display_name 
    FROM hospital.VW_COORD_DOCTORS 
    WHERE dept_id = p_dept_id 
    ORDER BY full_name;
END;
/

-- 2. Kỹ thuật viên
CREATE OR REPLACE PROCEDURE SP_COORD_GET_TECHS(p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT username_db, staff_id, full_name, TO_NCHAR(full_name) || N' (' || TO_NCHAR(staff_id) || N')' AS display_name 
    FROM hospital.VW_COORD_TECHNICIANS 
    ORDER BY full_name;
END;
/

-- 3. Khoa
CREATE OR REPLACE PROCEDURE SP_COORD_GET_DEPTS(p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT dept_id, TO_NCHAR(dept_name) AS dept_name FROM hospital.department ORDER BY dept_id;
END;
/

-- 4. Thông tin cá nhân (Điều phối viên)
CREATE OR REPLACE PROCEDURE SP_COORD_GET_SELF(p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT s.staff_id, s.full_name, s.staff_role, s.phone, s.hometown, d.dept_name AS specialty, s.gender, s.birthdate, s.id_card
    FROM hospital.staff s 
    LEFT JOIN hospital.department d ON s.dept_id = d.dept_id 
    WHERE s.username_db = SYS_CONTEXT('USERENV', 'SESSION_USER');
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_UPD_SELF(p_phone IN VARCHAR2, p_hometown IN NVARCHAR2) AS
BEGIN
    UPDATE hospital.staff SET phone = p_phone, hometown = p_hometown WHERE username_db = SYS_CONTEXT('USERENV', 'SESSION_USER');
    COMMIT;
END;
/

-- 5. Bệnh nhân
CREATE OR REPLACE PROCEDURE SP_COORD_GET_PATS(p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT patient_id, full_name, gender, birthdate, id_card, house_no, street, district, city_province, medical_history, family_medical_history, drug_allergies, username_db FROM hospital.patient ORDER BY patient_id;
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_SEARCH_PATS(p_keyword IN NVARCHAR2, p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT patient_id, full_name, gender, birthdate, id_card, house_no, street, district, city_province, medical_history, family_medical_history, drug_allergies, username_db FROM hospital.patient 
    WHERE UPPER(patient_id) LIKE UPPER(p_keyword) 
       OR UPPER(full_name) LIKE UPPER(p_keyword) 
       OR UPPER(id_card) LIKE UPPER(p_keyword) 
    ORDER BY patient_id;
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_CHK_PAT_ID(p_patient_id IN VARCHAR2, p_count OUT NUMBER) AS
BEGIN
    SELECT COUNT(*) INTO p_count FROM hospital.patient WHERE patient_id = p_patient_id;
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_CHK_IDCARD(p_id_card IN VARCHAR2, p_exclude_id IN VARCHAR2, p_count OUT NUMBER) AS
BEGIN
    IF p_exclude_id IS NULL THEN
        SELECT COUNT(*) INTO p_count FROM hospital.patient WHERE id_card = p_id_card;
    ELSE
        SELECT COUNT(*) INTO p_count FROM hospital.patient WHERE id_card = p_id_card AND patient_id != p_exclude_id;
    END IF;
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_CHK_USER(p_username IN VARCHAR2, p_exclude_id IN VARCHAR2, p_count OUT NUMBER) AS
BEGIN
    IF p_exclude_id IS NULL THEN
        SELECT COUNT(*) INTO p_count FROM hospital.patient WHERE username_db = p_username;
    ELSE
        SELECT COUNT(*) INTO p_count FROM hospital.patient WHERE username_db = p_username AND patient_id != p_exclude_id;
    END IF;
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_INS_PAT(
    p_patient_id IN VARCHAR2, p_full_name IN NVARCHAR2, p_gender IN NVARCHAR2, 
    p_birthdate IN DATE, p_id_card IN VARCHAR2, p_house_no IN NVARCHAR2, 
    p_street IN NVARCHAR2, p_district IN NVARCHAR2, p_city_province IN NVARCHAR2, 
    p_medical_history IN NCLOB, p_family_medical_history IN NCLOB, 
    p_drug_allergies IN NCLOB, p_username_db IN VARCHAR2
) AS
BEGIN
    INSERT INTO hospital.patient (patient_id, full_name, gender, birthdate, id_card, house_no, street, district, city_province, medical_history, family_medical_history, drug_allergies, username_db) 
    VALUES (p_patient_id, p_full_name, p_gender, p_birthdate, p_id_card, p_house_no, p_street, p_district, p_city_province, p_medical_history, p_family_medical_history, p_drug_allergies, p_username_db);
    COMMIT;
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_UPD_PAT(
    p_patient_id IN VARCHAR2, p_full_name IN NVARCHAR2, p_gender IN NVARCHAR2, 
    p_birthdate IN DATE, p_id_card IN VARCHAR2, p_house_no IN NVARCHAR2, 
    p_street IN NVARCHAR2, p_district IN NVARCHAR2, p_city_province IN NVARCHAR2, 
    p_medical_history IN NCLOB, p_family_medical_history IN NCLOB, 
    p_drug_allergies IN NCLOB, p_username_db IN VARCHAR2
) AS
BEGIN
    UPDATE hospital.patient SET full_name = p_full_name, gender = p_gender, birthdate = p_birthdate, id_card = p_id_card, house_no = p_house_no, street = p_street, district = p_district, city_province = p_city_province, medical_history = p_medical_history, family_medical_history = p_family_medical_history, drug_allergies = p_drug_allergies, username_db = p_username_db WHERE patient_id = p_patient_id;
    COMMIT;
END;
/

-- 6. Hồ sơ bệnh án
CREATE OR REPLACE PROCEDURE SP_COORD_GET_ALL_MED(p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT record_id, patient_id, record_date, TO_NCHAR(diagnosis) AS diagnosis, TO_NCHAR(treatment_plan) AS treatment_plan, doctor_id, dept_id, TO_NCHAR(conclusion) AS conclusion FROM hospital.medical_record ORDER BY record_id;
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_INS_MED(p_record_id IN VARCHAR2, p_patient_id IN VARCHAR2, p_record_date IN DATE, p_doctor_id IN VARCHAR2, p_dept_id IN VARCHAR2) AS
BEGIN
    INSERT INTO hospital.medical_record (record_id, patient_id, record_date, doctor_id, dept_id, diagnosis, treatment_plan, conclusion) 
    VALUES (p_record_id, p_patient_id, p_record_date, p_doctor_id, p_dept_id, N'Chưa chẩn đoán', N'Chưa điều trị', N'Chưa kết luận');
    COMMIT;
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_UPD_MED_REC(p_record_id IN VARCHAR2, p_doctor_id IN VARCHAR2, p_dept_id IN VARCHAR2) AS
BEGIN
    UPDATE hospital.medical_record SET doctor_id = p_doctor_id, dept_id = p_dept_id WHERE record_id = p_record_id;
    COMMIT;
END;
/

-- 7. Dịch vụ hỗ trợ (Service Assignment)
CREATE OR REPLACE PROCEDURE SP_COORD_GET_SRV_ASS(p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT record_id AS MAHSBA, service_type AS LOAIDV, service_date AS NGAYDV, technician_id AS MAKTV, service_result AS KETQUA FROM hospital.service_record WHERE technician_id IS NULL ORDER BY MAHSBA, NGAYDV;
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_UPD_TECH(p_record_id IN VARCHAR2, p_service_type IN NVARCHAR2, p_service_date IN DATE, p_technician_id IN VARCHAR2) AS
BEGIN
    UPDATE hospital.service_record SET technician_id = p_technician_id WHERE record_id = p_record_id AND service_type = p_service_type AND service_date = p_service_date;
    COMMIT;
END;
/

-- ==========================================
-- Grant Execute
-- ==========================================
GRANT EXECUTE ON hospital.SP_COORD_GET_DOCTORS TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_DOC_DEPT TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_TECHS TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_DEPTS TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_UPD_MED_REC TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_SELF TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_UPD_SELF TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_PATS TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_SEARCH_PATS TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_CHK_PAT_ID TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_CHK_IDCARD TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_CHK_USER TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_INS_PAT TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_UPD_PAT TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_ALL_MED TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_INS_MED TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_GET_SRV_ASS TO rl_coordinator;
GRANT EXECUTE ON hospital.SP_COORD_UPD_TECH TO rl_coordinator;
