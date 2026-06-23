-- ==============================================================================
-- 04_coordinator_procedures.sql
-- ChÃ¡ÂºÂ¡y dÃ†Â°Ã¡Â»â€ºi quyÃ¡Â»Ân: hospital
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;



CREATE TABLE COORD_ASSIGNMENT_STAFF (
    username_db VARCHAR2(50) PRIMARY KEY,
    staff_id    VARCHAR2(10) NOT NULL,
    full_name   NVARCHAR2(100) NOT NULL,
    staff_role  NVARCHAR2(50)  NOT NULL,
    dept_id     VARCHAR2(10),
    specialty   NVARCHAR2(100)
);

-- Đổ dữ liệu vào bảng phụ
INSERT INTO hospital.COORD_ASSIGNMENT_STAFF (
    username_db, staff_id, full_name, staff_role, dept_id, specialty
)
SELECT
    s.username_db, s.staff_id, s.full_name, s.staff_role, s.dept_id, d.dept_name AS specialty
FROM hospital.STAFF s
LEFT JOIN hospital.DEPARTMENT d ON s.dept_id = d.dept_id
WHERE s.staff_role IN (UNISTR('B\00E1c s\0129'), UNISTR('B\00E1c s\0129/Y s\0129'), UNISTR('K\1EF9 thu\1EADt vi\00EAn'));

COMMIT;

CREATE OR REPLACE VIEW hospital.VW_COORD_DOCTORS AS
SELECT
    username_db,
    staff_id,
    full_name,
    dept_id,
    specialty
FROM hospital.COORD_ASSIGNMENT_STAFF
WHERE staff_role IN (UNISTR('B\00E1c s\0129'), UNISTR('B\00E1c s\0129/Y s\0129'));

CREATE OR REPLACE VIEW hospital.VW_COORD_TECHNICIANS AS
SELECT
    username_db,
    staff_id,
    full_name
FROM hospital.COORD_ASSIGNMENT_STAFF
WHERE staff_role = UNISTR('K\1EF9 thu\1EADt vi\00EAn');

CREATE OR REPLACE TRIGGER TRG_VALIDATE_SERVICE_RECORD
BEFORE INSERT OR UPDATE ON hospital.service_record
FOR EACH ROW
DECLARE
    v_tech_active NUMBER(1);
BEGIN
    IF :NEW.technician_id IS NOT NULL THEN
        SELECT is_active INTO v_tech_active FROM hospital.staff WHERE staff_id = :NEW.technician_id;
        IF v_tech_active = 0 THEN
            RAISE_APPLICATION_ERROR(-20012, N'Không thể tạo/cập nhật dịch vụ: Kỹ thuật viên này đã bị khóa (Không hoạt động).');
        END IF;
    END IF;
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_GET_DOCTORS(p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT username_db, staff_id, full_name, specialty, TO_NCHAR(full_name) || N' - ' || TO_NCHAR(staff_id) AS display_name 
    FROM hospital.VW_COORD_DOCTORS 
    ORDER BY full_name;
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_GET_DOC_DEPT(p_dept_id IN VARCHAR2, p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT username_db, staff_id, full_name, specialty, TO_NCHAR(full_name) || N' - ' || TO_NCHAR(staff_id) AS display_name 
    FROM hospital.VW_COORD_DOCTORS 
    WHERE dept_id = p_dept_id 
    ORDER BY full_name;
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_GET_TECHS(p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT username_db, staff_id, full_name, TO_NCHAR(full_name) || N' - ' || TO_NCHAR(staff_id) AS display_name 
    FROM hospital.VW_COORD_TECHNICIANS 
    ORDER BY full_name;
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_GET_DEPTS(p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT dept_id, TO_NCHAR(dept_name) AS dept_name FROM hospital.department ORDER BY dept_id;
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_GET_SELF(p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT s.staff_id, s.full_name, s.staff_role, s.phone, s.hometown, d.dept_name AS specialty 
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

CREATE OR REPLACE PROCEDURE SP_COORD_GET_PATS(p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT patient_id, full_name, gender, birthdate, id_card, house_no, street, district, city_province, medical_history, family_medical_history, drug_allergies, username_db FROM hospital.patient ORDER BY patient_id FETCH FIRST 100 ROWS ONLY;
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
        SELECT COUNT(*) INTO p_count
        FROM (
            SELECT username_db FROM hospital.patient WHERE username_db = UPPER(TRIM(p_username))
            UNION ALL
            SELECT username_db FROM hospital.staff WHERE username_db = UPPER(TRIM(p_username))
        );
    ELSE
        SELECT COUNT(*) INTO p_count
        FROM (
            SELECT username_db
            FROM hospital.patient
            WHERE username_db = UPPER(TRIM(p_username))
              AND patient_id != p_exclude_id
            UNION ALL
            SELECT username_db FROM hospital.staff WHERE username_db = UPPER(TRIM(p_username))
        );
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

CREATE OR REPLACE PROCEDURE SP_COORD_GET_ALL_MED(p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT record_id, patient_id, record_date, TO_NCHAR(diagnosis) AS diagnosis, TO_NCHAR(treatment_plan) AS treatment_plan, doctor_id, dept_id, TO_NCHAR(conclusion) AS conclusion FROM hospital.medical_record ORDER BY record_id FETCH FIRST 100 ROWS ONLY;
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

CREATE OR REPLACE PROCEDURE SP_COORD_GET_SRV_ASS(p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT record_id AS MAHSBA, service_type AS LOAIDV, service_date AS NGAYDV, technician_id AS MAKTV, service_result AS KETQUA FROM hospital.service_record WHERE technician_id IS NULL ORDER BY MAHSBA, NGAYDV FETCH FIRST 100 ROWS ONLY;
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_UPD_TECH(p_record_id IN VARCHAR2, p_service_type IN NVARCHAR2, p_service_date IN DATE, p_technician_id IN VARCHAR2) AS
BEGIN
    UPDATE hospital.service_record SET technician_id = p_technician_id WHERE record_id = p_record_id AND service_type = p_service_type AND service_date = p_service_date;
    COMMIT;
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_GET_PATS_PAGED(
    p_keyword IN NVARCHAR2, 
    p_page_num IN NUMBER, 
    p_page_size IN NUMBER, 
    p_cursor OUT SYS_REFCURSOR
) AS
BEGIN
    OPEN p_cursor FOR
    SELECT patient_id, full_name, gender, birthdate, id_card, house_no, street, district, city_province, medical_history, family_medical_history, drug_allergies, username_db
    FROM (
        SELECT a.*, ROWNUM rnum FROM (
            SELECT patient_id, full_name, gender, birthdate, id_card, house_no, street, district, city_province, medical_history, family_medical_history, drug_allergies, username_db 
            FROM hospital.patient 
            WHERE p_keyword IS NULL 
               OR UPPER(patient_id) LIKE '%' || UPPER(p_keyword) || '%' 
               OR UPPER(full_name) LIKE '%' || UPPER(p_keyword) || '%' 
               OR UPPER(id_card) LIKE '%' || UPPER(p_keyword) || '%' 
            ORDER BY patient_id
        ) a WHERE ROWNUM <= p_page_num * p_page_size
    ) WHERE rnum > (p_page_num - 1) * p_page_size;
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_GET_ALL_MED_PAGED(
    p_keyword IN NVARCHAR2, 
    p_page_num IN NUMBER, 
    p_page_size IN NUMBER, 
    p_cursor OUT SYS_REFCURSOR
) AS
BEGIN
    OPEN p_cursor FOR
    SELECT record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion
    FROM (
        SELECT a.*, ROWNUM rnum FROM (
            SELECT record_id, patient_id, record_date, TO_NCHAR(diagnosis) AS diagnosis, TO_NCHAR(treatment_plan) AS treatment_plan, doctor_id, dept_id, TO_NCHAR(conclusion) AS conclusion 
            FROM hospital.medical_record 
            WHERE p_keyword IS NULL 
               OR UPPER(record_id) LIKE '%' || UPPER(p_keyword) || '%' 
               OR UPPER(patient_id) LIKE '%' || UPPER(p_keyword) || '%' 
            ORDER BY record_id
        ) a WHERE ROWNUM <= p_page_num * p_page_size
    ) WHERE rnum > (p_page_num - 1) * p_page_size;
END;
/

CREATE OR REPLACE PROCEDURE SP_COORD_GET_SRV_ASS_PAGED(
    p_keyword IN NVARCHAR2, 
    p_page_num IN NUMBER, 
    p_page_size IN NUMBER, 
    p_cursor OUT SYS_REFCURSOR
) AS
BEGIN
    OPEN p_cursor FOR
    SELECT MAHSBA, LOAIDV, NGAYDV, MAKTV, KETQUA
    FROM (
        SELECT a.*, ROWNUM rnum FROM (
            SELECT record_id AS MAHSBA, service_type AS LOAIDV, service_date AS NGAYDV, technician_id AS MAKTV, service_result AS KETQUA 
            FROM hospital.service_record 
            WHERE technician_id IS NULL 
              AND (p_keyword IS NULL OR UPPER(record_id) LIKE '%' || UPPER(p_keyword) || '%' OR UPPER(service_type) LIKE '%' || UPPER(p_keyword) || '%')
            ORDER BY MAHSBA, NGAYDV
        ) a WHERE ROWNUM <= p_page_num * p_page_size
    ) WHERE rnum > (p_page_num - 1) * p_page_size;
END;
/
