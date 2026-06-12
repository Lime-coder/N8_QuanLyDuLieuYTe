-- ==============================================================================
-- Setup_All_Roles_Fixed.sql
--
-- Run as: hospital_dba
-- Connect to: PDB_QLYT
-- ==============================================================================

-- ==============================================================================
-- Combined_Coordinator_Patient_Technician.sql
-- Purpose: Run Coordinator + Patient + Technician SQL in one go.
-- Excluded doctor files:
--   - GrantPrivileges_Doctor.sql
--   - StoreProcedure_Doctor.sql
--   - VPD_Doctor.sql
--
-- Run as: HOSPITAL_DBA
-- Connect to: PDB_QLYT before running this script.
-- If you are connected to CDB$ROOT as a privileged common user, uncomment this line:
-- ALTER SESSION SET CONTAINER = PDB_QLYT;
--
-- Small safety edits made while combining:
--   1. Added explicit CURRENT_SCHEMA resets between role sections.
--   2. Commented out the technician file's mid-script ALTER SESSION SET CONTAINER.
--      Switching containers halfway through a combined script can fail or change context.
-- ==============================================================================

SET SERVEROUTPUT ON;

PROMPT ==============================================================================
PROMPT START: Combined Coordinator + Patient + Technician setup
PROMPT ==============================================================================



PROMPT ==============================================================================
PROMPT SECTION: Coordinator VPD / views / staff self-policy
PROMPT Source: VPD_Coordinator.sql
PROMPT ==============================================================================
ALTER SESSION SET CURRENT_SCHEMA = hospital;

-- ==============================================================================
-- File: VPD_Coordinator.sql
-- Mục đích:
-- 1. Giữ TC#5: nhân viên query trực tiếp STAFF chỉ thấy chính mình.
-- 2. Tạo bảng phụ tối thiểu cho Điều phối viên chọn Bác sĩ/Y sĩ và Kỹ thuật viên.
-- Run as: HOSPITAL_DBA có quyền DBMS_RLS
-- ==============================================================================

ALTER SESSION SET CURRENT_SCHEMA = hospital;

-- ==============================================================================
-- PHẦN 1: DROP POLICY CŨ
-- ==============================================================================

BEGIN
    DBMS_RLS.DROP_POLICY('HOSPITAL', 'STAFF', 'POL_VPD_STAFF_SELF_SELECT');
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
    DBMS_RLS.DROP_POLICY('HOSPITAL', 'STAFF', 'POL_VPD_STAFF_SELF_UPDATE');
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

-- ==============================================================================
-- PHẦN 2: DROP FUNCTION CŨ
-- ==============================================================================

BEGIN
    EXECUTE IMMEDIATE 'DROP FUNCTION hospital.FN_VPD_STAFF_SELF';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -4043 THEN
            RAISE;
        END IF;
END;
/

-- ==============================================================================
-- PHẦN 3: TẠO FUNCTION VPD CHO STAFF
-- ==============================================================================

CREATE OR REPLACE FUNCTION hospital.FN_VPD_STAFF_SELF (
    p_schema VARCHAR2,
    p_obj    VARCHAR2
)
RETURN VARCHAR2
AS
    v_current_user VARCHAR2(100);
BEGIN
    v_current_user := SYS_CONTEXT('USERENV', 'SESSION_USER');
    
    -- Bypass cho schema owner, DBA app, và khi trigger chạy (CURRENT_USER = 'HOSPITAL')
    IF SYS_CONTEXT('USERENV', 'CURRENT_USER') IN ('HOSPITAL', 'HOSPITAL_DBA') THEN
        RETURN '1=1';
    END IF;

    -- Nhân viên chỉ thấy / sửa dòng của chính mình
    RETURN 'username_db = SYS_CONTEXT(''USERENV'', ''SESSION_USER'')';

EXCEPTION
    WHEN OTHERS THEN
        RETURN '1=0';
END;
/

-- ==============================================================================
-- PHẦN 4: GẮN VPD POLICY CHO STAFF
-- ==============================================================================

BEGIN
    -- Policy 1: SELECT trực tiếp STAFF chỉ thấy chính mình
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL',
        object_name     => 'STAFF',
        policy_name     => 'POL_VPD_STAFF_SELF_SELECT',
        policy_function => 'FN_VPD_STAFF_SELF',
        statement_types => 'SELECT'
    );

    -- Policy 2: UPDATE phone, hometown chỉ trên dòng chính mình
    DBMS_RLS.ADD_POLICY(
        object_schema     => 'HOSPITAL',
        object_name       => 'STAFF',
        policy_name       => 'POL_VPD_STAFF_SELF_UPDATE',
        policy_function   => 'FN_VPD_STAFF_SELF',
        statement_types   => 'UPDATE',
        sec_relevant_cols => 'PHONE,HOMETOWN',
        update_check      => TRUE
    );
END;
/

-- ==============================================================================
-- PHẦN 5: TẠO BẢNG PHỤ TỐI THIỂU CHO ĐIỀU PHỐI VIÊN PHÂN CÔNG
-- ==============================================================================

BEGIN
    EXECUTE IMMEDIATE 'DROP TABLE hospital.COORD_ASSIGNMENT_STAFF PURGE';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -942 THEN
            RAISE;
        END IF;
END;
/

CREATE TABLE hospital.COORD_ASSIGNMENT_STAFF (
    username_db VARCHAR2(50) PRIMARY KEY,
    staff_id    VARCHAR2(10) NOT NULL,
    full_name   NVARCHAR2(100) NOT NULL,
    staff_role  NVARCHAR2(50)  NOT NULL,
    dept_id     VARCHAR2(10),
    specialty   NVARCHAR2(100)
);

-- ==============================================================================
-- PHẦN 6: ĐỔ DỮ LIỆU TỐI THIỂU TỪ STAFF SANG BẢNG PHỤ
-- ==============================================================================

INSERT INTO hospital.COORD_ASSIGNMENT_STAFF (
    username_db,
    staff_id,
    full_name,
    staff_role,
    dept_id,
    specialty
)
SELECT
    s.username_db,
    s.staff_id,
    s.full_name,
    s.staff_role,
    s.dept_id,
    d.dept_name AS specialty
FROM hospital.staff s
LEFT JOIN hospital.department d
    ON d.dept_id = s.dept_id
WHERE s.staff_role IN (
    N'Bác sĩ',
    N'Bác sĩ/Y sĩ',
    N'Kỹ thuật viên'
);

COMMIT;

-- ==============================================================================
-- PHẦN 7: TẠO VIEW CHO ĐIỀU PHỐI VIÊN
-- ==============================================================================

CREATE OR REPLACE VIEW hospital.VW_COORD_DOCTORS AS
SELECT
    username_db,
    staff_id,
    full_name,
    dept_id,
    specialty
FROM hospital.COORD_ASSIGNMENT_STAFF
WHERE staff_role IN (N'Bác sĩ', N'Bác sĩ/Y sĩ');

CREATE OR REPLACE VIEW hospital.VW_COORD_TECHNICIANS AS
SELECT
    username_db,
    staff_id,
    full_name
FROM hospital.COORD_ASSIGNMENT_STAFF
WHERE staff_role = N'Kỹ thuật viên';


-- ==============================================================================
-- PHẦN 8: CHỈNH SỬA SCHEMA CHO PHÉP ĐIỀU PHỐI VIÊN PHÂN CÔNG (ALLOW NULL TECHNICIAN)
-- ==============================================================================

-- 8.1 Cho phép cột technician_id nhận giá trị NULL để lưu các dịch vụ chưa phân công
BEGIN
    EXECUTE IMMEDIATE 'ALTER TABLE hospital.service_record MODIFY technician_id NULL';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE = -1451 THEN
            NULL; -- Bỏ qua nếu cột đã cho phép NULL
        ELSE
            RAISE;
        END IF;
END;
/

-- 8.2 Cập nhật trigger để bỏ qua kiểm tra KTV nếu technician_id là NULL
CREATE OR REPLACE TRIGGER hospital.TRG_VALIDATE_SERVICE_RECORD
BEFORE INSERT OR UPDATE ON hospital.service_record
FOR EACH ROW
DECLARE
    v_tech_active NUMBER(1);
BEGIN
    IF :NEW.technician_id IS NOT NULL THEN
        SELECT is_active INTO v_tech_active FROM hospital.staff WHERE staff_id = :NEW.technician_id;
        IF v_tech_active = 0 THEN
            RAISE_APPLICATION_ERROR(-20012, 'Không thể tạo/cập nhật dịch vụ: Kỹ thuật viên này đã bị khóa (Không hoạt động).');
        END IF;
    END IF;
END;
/

-- 8.3 Thêm một record mẫu chưa phân công lấy từ HSBA có sẵn để tránh lỗi Khóa ngoại
BEGIN
    FOR r IN (SELECT record_id FROM hospital.medical_record WHERE ROWNUM = 1) LOOP
        INSERT INTO hospital.service_record (record_id, service_type, service_date, technician_id, service_result)
        SELECT r.record_id, N'Xét nghiệm máu', SYSDATE - 1, NULL, NULL
        FROM DUAL
        WHERE NOT EXISTS (
            SELECT 1 FROM hospital.service_record WHERE record_id = r.record_id AND service_type = N'Xét nghiệm máu'
        );
    END LOOP;
    COMMIT;
END;
/


PROMPT ==============================================================================
PROMPT SECTION: Coordinator grants
PROMPT Source: GrantPrivileges_Coordinator.sql
PROMPT ==============================================================================
ALTER SESSION SET CURRENT_SCHEMA = hospital;

-- ==============================================================================
-- File: GrantPrivileges_Coordinator.sql
-- Mục đích: Cấp quyền tối thiểu cho vai trò Điều phối viên
-- Run as: HOSPITAL_DBA
-- ==============================================================================

ALTER SESSION SET CURRENT_SCHEMA = hospital;

-- Bảng bệnh nhân: Điều phối viên được xem, thêm, sửa
GRANT SELECT, INSERT, UPDATE ON hospital.patient TO rl_coordinator;

-- Hồ sơ bệnh án:
-- Được xem, thêm hồ sơ mới
GRANT SELECT, INSERT ON hospital.medical_record TO rl_coordinator;

-- Chỉ được phân công bác sĩ/khoa
GRANT UPDATE (doctor_id, dept_id) ON hospital.medical_record TO rl_coordinator;

-- Dịch vụ hỗ trợ chẩn đoán:
-- Được xem danh sách dịch vụ
GRANT SELECT ON hospital.service_record TO rl_coordinator;

-- Chỉ được phân công kỹ thuật viên
GRANT UPDATE (technician_id) ON hospital.service_record TO rl_coordinator;

-- Xem danh mục khoa
GRANT SELECT ON hospital.department TO rl_coordinator;

-- STAFF:
-- Query trực tiếp STAFF sẽ bị VPD lọc chỉ thấy chính mình
GRANT SELECT ON hospital.staff TO rl_coordinator;

-- Chỉ được cập nhật thông tin cá nhân hợp lệ
GRANT UPDATE (phone, hometown) ON hospital.staff TO rl_coordinator;

-- View phục vụ điều phối bác sĩ/kỹ thuật viên
GRANT SELECT ON hospital.VW_COORD_DOCTORS TO rl_coordinator;
GRANT SELECT ON hospital.VW_COORD_TECHNICIANS TO rl_coordinator;


PROMPT ==============================================================================
PROMPT SECTION: Coordinator stored procedures
PROMPT Source: SP_Coordinator.sql
PROMPT ==============================================================================
ALTER SESSION SET CURRENT_SCHEMA = hospital;

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

CREATE OR REPLACE PROCEDURE SP_COORD_UPD_SRV_REC(p_record_id IN VARCHAR2, p_technician_id IN VARCHAR2) AS
BEGIN
    UPDATE hospital.service_record SET technician_id = p_technician_id WHERE record_id = p_record_id;
    COMMIT;
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
GRANT EXECUTE ON hospital.SP_COORD_UPD_SRV_REC TO rl_coordinator;
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


PROMPT ==============================================================================
PROMPT SECTION: Patient RBAC views/procedures/grants
PROMPT Source: Patient_RBAC.sql
PROMPT ==============================================================================
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

-- ============================================================
-- Patient RBAC - Phase 2
-- Run as: hospital_dba | Container: PDB_QLYT
-- Author: Khanh
-- ============================================================

-- ------------------------------------------------------------
-- VIEWS: Row-level security via SYS_CONTEXT SESSION_USER
-- Bệnh nhân chỉ thấy dữ liệu của chính mình
-- ------------------------------------------------------------

CREATE OR REPLACE VIEW hospital_dba.V_PATIENT_SELF AS
    SELECT patient_id, full_name, gender, birthdate, id_card,
           house_no, street, district, city_province,
           medical_history, family_medical_history, drug_allergies,
           username_db, is_active
    FROM hospital.patient
    WHERE username_db = SYS_CONTEXT('USERENV', 'SESSION_USER');

CREATE OR REPLACE VIEW hospital_dba.V_MEDICAL_RECORD_PATIENT AS
    SELECT mr.record_id, mr.patient_id, mr.record_date, mr.diagnosis,
           mr.treatment_plan, mr.doctor_id, mr.dept_id, mr.conclusion
    FROM hospital.medical_record mr
    JOIN hospital.patient p ON mr.patient_id = p.patient_id
    WHERE p.username_db = SYS_CONTEXT('USERENV', 'SESSION_USER');

CREATE OR REPLACE VIEW hospital_dba.V_PRESCRIPTION_PATIENT AS
    SELECT pr.record_id, pr.prescription_date, pr.medicine_name, pr.dosage
    FROM hospital.prescription pr
    JOIN hospital.medical_record mr ON pr.record_id = mr.record_id
    JOIN hospital.patient p ON mr.patient_id = p.patient_id
    WHERE p.username_db = SYS_CONTEXT('USERENV', 'SESSION_USER');

CREATE OR REPLACE VIEW hospital_dba.V_SERVICE_RECORD_PATIENT AS
    SELECT sr.record_id, sr.service_type, sr.service_date,
           sr.technician_id, sr.service_result
    FROM hospital.service_record sr
    JOIN hospital.medical_record mr ON sr.record_id = mr.record_id
    JOIN hospital.patient p ON mr.patient_id = p.patient_id
    WHERE p.username_db = SYS_CONTEXT('USERENV', 'SESSION_USER');

-- ------------------------------------------------------------
-- GRANT VIEW privileges to rl_patient
-- ------------------------------------------------------------

GRANT SELECT ON hospital_dba.V_PATIENT_SELF           TO rl_patient;
GRANT UPDATE (house_no, street, district, city_province, medical_history, family_medical_history, drug_allergies) ON hospital_dba.V_PATIENT_SELF TO rl_patient;
GRANT SELECT ON hospital_dba.V_MEDICAL_RECORD_PATIENT TO rl_patient;
GRANT SELECT ON hospital_dba.V_PRESCRIPTION_PATIENT   TO rl_patient;
GRANT SELECT ON hospital_dba.V_SERVICE_RECORD_PATIENT TO rl_patient;

-- ------------------------------------------------------------
-- STORED PROCEDURES
-- ------------------------------------------------------------

-- USP_GET_PATIENT_PROFILE: Lấy thông tin cá nhân bệnh nhân đang đăng nhập
CREATE OR REPLACE PROCEDURE USP_GET_PATIENT_PROFILE (
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT patient_id, full_name, gender, birthdate, id_card,
               house_no, street, district, city_province,
               medical_history, family_medical_history, drug_allergies
        FROM hospital_dba.V_PATIENT_SELF;
END USP_GET_PATIENT_PROFILE;
/

-- USP_GET_PATIENT_RECORDS: Lấy danh sách hồ sơ bệnh án của bệnh nhân
CREATE OR REPLACE PROCEDURE USP_GET_PATIENT_RECORDS (
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT mr.record_id, mr.record_date, mr.diagnosis,
               mr.treatment_plan, mr.conclusion,
               s.full_name AS doctor_name,
               d.dept_name
        FROM hospital_dba.V_MEDICAL_RECORD_PATIENT mr
        LEFT JOIN hospital.staff s ON mr.doctor_id = s.staff_id
        LEFT JOIN hospital.department d ON mr.dept_id = d.dept_id
        ORDER BY mr.record_date DESC;
END USP_GET_PATIENT_RECORDS;
/

-- USP_GET_PATIENT_PRESCRIPTIONS: Lấy đơn thuốc theo record_id
CREATE OR REPLACE PROCEDURE USP_GET_PATIENT_PRESCRIPTIONS (
    p_record_id IN VARCHAR2,
    p_cursor    OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT record_id, prescription_date, medicine_name, dosage
        FROM hospital_dba.V_PRESCRIPTION_PATIENT
        WHERE record_id = p_record_id
        ORDER BY prescription_date;
END USP_GET_PATIENT_PRESCRIPTIONS;
/

-- USP_GET_PATIENT_SERVICES: Lấy dịch vụ y tế theo record_id
CREATE OR REPLACE PROCEDURE USP_GET_PATIENT_SERVICES (
    p_record_id IN VARCHAR2,
    p_cursor    OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT sr.record_id, sr.service_type, sr.service_date,
               sr.service_result,
               s.full_name AS technician_name
        FROM hospital_dba.V_SERVICE_RECORD_PATIENT sr
        LEFT JOIN hospital.staff s ON sr.technician_id = s.staff_id
        WHERE sr.record_id = p_record_id
        ORDER BY sr.service_date;
END USP_GET_PATIENT_SERVICES;
/

-- USP_UPDATE_PATIENT_CONTACT: Bệnh nhân tự cập nhật thông tin liên lạc và tiền sử bệnh lý (TC#5)
CREATE OR REPLACE PROCEDURE USP_UPDATE_PATIENT_CONTACT (
    p_house_no               IN NVARCHAR2,
    p_street                 IN NVARCHAR2,
    p_district               IN NVARCHAR2,
    p_city_province          IN NVARCHAR2,
    p_medical_history        IN NVARCHAR2,
    p_family_medical_history IN NVARCHAR2,
    p_drug_allergies         IN NVARCHAR2
) AUTHID CURRENT_USER AS
BEGIN
    UPDATE hospital_dba.V_PATIENT_SELF
    SET house_no               = p_house_no,
        street                 = p_street,
        district               = p_district,
        city_province          = p_city_province,
        medical_history        = p_medical_history,
        family_medical_history = p_family_medical_history,
        drug_allergies         = p_drug_allergies;
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END USP_UPDATE_PATIENT_CONTACT;
/

-- ------------------------------------------------------------
-- GRANT EXECUTE on SPs to rl_patient
-- ------------------------------------------------------------

GRANT EXECUTE ON hospital_dba.USP_GET_PATIENT_PROFILE        TO rl_patient;
GRANT EXECUTE ON hospital_dba.USP_GET_PATIENT_RECORDS         TO rl_patient;
GRANT EXECUTE ON hospital_dba.USP_GET_PATIENT_PRESCRIPTIONS   TO rl_patient;
GRANT EXECUTE ON hospital_dba.USP_GET_PATIENT_SERVICES        TO rl_patient;
GRANT EXECUTE ON hospital_dba.USP_UPDATE_PATIENT_CONTACT      TO rl_patient;

-- Grant access to lookup tables (doctor name, dept name)
GRANT SELECT ON hospital.staff      TO rl_patient;
GRANT SELECT ON hospital.department TO rl_patient;

COMMIT;


PROMPT ==============================================================================
PROMPT SECTION: Technician view/procedures/grants
PROMPT Source: Grant_technician.sql
PROMPT ==============================================================================
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

--ALTER SESSION SET CURRENT_SCHEMA = HOSPITAL;
--SET SERVEROUTPUT ON;

-- ALTER SESSION SET CONTAINER = PDB_QLYT; -- commented out in combined script; connect to PDB_QLYT before running
-- ALTER PLUGGABLE DATABASE PDB_QLYT OPEN;
-- ============================================================
-- Remove old objects if exist
-- ============================================================

BEGIN
    EXECUTE IMMEDIATE 'DROP VIEW hospital_dba.V_TECHNICIAN_SERVICE_RECORD';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -942 THEN
            DBMS_OUTPUT.PUT_LINE('Drop view error: ' || SQLERRM);
        END IF;
END;
/ 

BEGIN
    EXECUTE IMMEDIATE 'DROP PROCEDURE hospital_dba.GET_TECHNICIAN_SERVICE_RECORDS';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -4043 THEN
            DBMS_OUTPUT.PUT_LINE('Drop procedure error: ' || SQLERRM);
        END IF;
END;
/ 

BEGIN
    EXECUTE IMMEDIATE 'DROP PROCEDURE hospital_dba.UPDATE_TECHNICIAN_SERVICE_RESULT';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -4043 THEN
            DBMS_OUTPUT.PUT_LINE('Drop procedure error: ' || SQLERRM);
        END IF;
END;
/ 

BEGIN
    EXECUTE IMMEDIATE 'DROP PROCEDURE hospital_dba.GET_TECHNICIAN_PERSONAL_INFO';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -4043 THEN
            DBMS_OUTPUT.PUT_LINE('Drop procedure error: ' || SQLERRM);
        END IF;
END;
/ 

BEGIN
    EXECUTE IMMEDIATE 'DROP PROCEDURE hospital_dba.UPDATE_TECHNICIAN_PERSONAL_INFO';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -4043 THEN
            DBMS_OUTPUT.PUT_LINE('Drop procedure error: ' || SQLERRM);
        END IF;
END;
/
-- ============================================================
-- View: technician only sees assigned service records
-- ============================================================

CREATE OR REPLACE VIEW hospital_dba.V_TECHNICIAN_SERVICE_RECORD AS
SELECT
    SR.RECORD_ID,
    SR.SERVICE_TYPE,
    SR.SERVICE_DATE,
    SR.TECHNICIAN_ID,
    SR.SERVICE_RESULT
FROM HOSPITAL.SERVICE_RECORD SR
JOIN HOSPITAL.STAFF ST
    ON SR.TECHNICIAN_ID = ST.STAFF_ID
WHERE UPPER(ST.USERNAME_DB) = SYS_CONTEXT('USERENV', 'SESSION_USER')
  AND ST.STAFF_ROLE = N'Kỹ thuật viên'
  AND ST.IS_ACTIVE = 1;

-- ============================================================
-- Get assigned service records
-- ============================================================
CREATE OR REPLACE PROCEDURE hospital_dba.GET_TECHNICIAN_SERVICE_RECORDS (
    P_CURSOR OUT SYS_REFCURSOR
)
AUTHID DEFINER
AS
BEGIN
    OPEN P_CURSOR FOR
        SELECT
            SR.RECORD_ID,
            SR.SERVICE_TYPE,
            SR.SERVICE_DATE,
            SR.TECHNICIAN_ID,
            SR.SERVICE_RESULT
        FROM hospital_dba.V_TECHNICIAN_SERVICE_RECORD SR
        ORDER BY SR.SERVICE_DATE DESC;
END;
/ 

-- ============================================================
-- Update only SERVICE_RESULT of assigned service record
-- ============================================================
CREATE OR REPLACE PROCEDURE hospital_dba.UPDATE_TECHNICIAN_SERVICE_RESULT (
    P_RECORD_ID      IN VARCHAR2,
    P_SERVICE_TYPE   IN NVARCHAR2,
    P_SERVICE_DATE   IN DATE,
    P_SERVICE_RESULT IN NVARCHAR2
)
AUTHID DEFINER
AS
    V_ROWS_UPDATED NUMBER;
BEGIN
    UPDATE HOSPITAL.SERVICE_RECORD SR
    SET SR.SERVICE_RESULT = P_SERVICE_RESULT
    WHERE SR.RECORD_ID = P_RECORD_ID
      AND SR.SERVICE_TYPE = P_SERVICE_TYPE
      AND SR.SERVICE_DATE = P_SERVICE_DATE
      AND SR.TECHNICIAN_ID = (
            SELECT ST.STAFF_ID
            FROM HOSPITAL.STAFF ST
            WHERE UPPER(ST.USERNAME_DB) = SYS_CONTEXT('USERENV', 'SESSION_USER')
              AND ST.STAFF_ROLE = N'Kỹ thuật viên'
              AND ST.IS_ACTIVE = 1
      );

    V_ROWS_UPDATED := SQL%ROWCOUNT;

    IF V_ROWS_UPDATED = 0 THEN
        RAISE_APPLICATION_ERROR(
            -20031,
            N'Không được phép cập nhật: dịch vụ không thuộc kỹ thuật viên hiện tại.'
        );
    END IF;
END;
/ 

-- ============================================================
-- Get current technician personal info
-- ============================================================
CREATE OR REPLACE PROCEDURE hospital_dba.GET_TECHNICIAN_PERSONAL_INFO(
    P_CURSOR OUT SYS_REFCURSOR
)
AUTHID DEFINER
AS
BEGIN
    OPEN P_CURSOR FOR
        SELECT
            ST.STAFF_ID,
            ST.FULL_NAME,
            ST.GENDER,
            ST.BIRTHDATE,
            ST.ID_CARD,
            ST.STAFF_ROLE AS ROLE,
            ST.PHONE,
            ST.HOMETOWN
        FROM HOSPITAL.STAFF ST
        WHERE UPPER(ST.USERNAME_DB) = SYS_CONTEXT('USERENV', 'SESSION_USER')
          AND ST.STAFF_ROLE = N'Kỹ thuật viên'
          AND ST.IS_ACTIVE = 1;
END;
/

-- ============================================================
-- Update allowed personal fields only
-- ============================================================
CREATE OR REPLACE PROCEDURE hospital_dba.UPDATE_TECHNICIAN_PERSONAL_INFO(
    P_PHONE IN VARCHAR2,
    P_HOMETOWN IN NVARCHAR2
)
AUTHID DEFINER
AS
    V_ROWS_UPDATED NUMBER;
BEGIN
    UPDATE HOSPITAL.STAFF ST
    SET ST.PHONE = CASE 
                       WHEN P_PHONE IS NULL OR TRIM(P_PHONE) = '' 
                       THEN ST.PHONE 
                       ELSE P_PHONE 
                   END,
        ST.HOMETOWN = CASE 
                          WHEN P_HOMETOWN IS NULL OR TRIM(P_HOMETOWN) = '' 
                          THEN ST.HOMETOWN 
                          ELSE P_HOMETOWN 
                      END
    WHERE UPPER(ST.USERNAME_DB) = SYS_CONTEXT('USERENV', 'SESSION_USER')
      AND ST.STAFF_ROLE = N'Kỹ thuật viên'
      AND ST.IS_ACTIVE = 1;

    V_ROWS_UPDATED := SQL%ROWCOUNT;

    IF V_ROWS_UPDATED = 0 THEN
        RAISE_APPLICATION_ERROR(-20032, N'Không tìm thấy kỹ thuật viên hiện tại hoặc không có quyền cập nhật.');
    END IF;
END;
/


-- ============================================================
-- Grant safe privileges to role
-- ============================================================

GRANT SELECT ON hospital_dba.V_TECHNICIAN_SERVICE_RECORD TO RL_TECHNICIAN;

GRANT EXECUTE ON hospital_dba.GET_TECHNICIAN_SERVICE_RECORDS TO RL_TECHNICIAN;
GRANT EXECUTE ON hospital_dba.UPDATE_TECHNICIAN_SERVICE_RESULT TO RL_TECHNICIAN;

GRANT EXECUTE ON hospital_dba.GET_TECHNICIAN_PERSONAL_INFO TO RL_TECHNICIAN;
GRANT EXECUTE ON hospital_dba.UPDATE_TECHNICIAN_PERSONAL_INFO TO RL_TECHNICIAN;
COMMIT;


PROMPT ==============================================================================

PROMPT ==============================================================================
PROMPT FIX 1: Grant direct privilege needed by definer-rights technician procedure
PROMPT ==============================================================================

-- Roles do NOT count when compiling stored procedures.
-- HOSPITAL_DBA needs a direct grant from HOSPITAL or a DBA/admin user.
GRANT UPDATE ON hospital.service_record TO hospital_dba;

PROMPT ==============================================================================
PROMPT FIX 2: Recompile invalid HOSPITAL_DBA procedure
PROMPT ==============================================================================

ALTER PROCEDURE hospital_dba.UPDATE_TECHNICIAN_SERVICE_RESULT COMPILE;

PROMPT ==============================================================================
PROMPT FIX 3: Drop stale legacy duplicates under HOSPITAL if they exist
PROMPT ==============================================================================

-- These are not the intended owner according to the combined script.
-- Intended owner for patient/technician wrapper objects is HOSPITAL_DBA.
DECLARE
    PROCEDURE drop_obj(p_sql VARCHAR2, p_ignore_code NUMBER) IS
    BEGIN
        EXECUTE IMMEDIATE p_sql;
        DBMS_OUTPUT.PUT_LINE('[DROP] ' || p_sql);
    EXCEPTION
        WHEN OTHERS THEN
            IF SQLCODE = p_ignore_code THEN
                DBMS_OUTPUT.PUT_LINE('[SKIP] Already missing: ' || p_sql);
            ELSE
                DBMS_OUTPUT.PUT_LINE('[WARN] Could not run: ' || p_sql || ' -> ' || SQLERRM);
            END IF;
    END;
BEGIN
    drop_obj('DROP PROCEDURE hospital.USP_GET_PATIENT_PROFILE', -4043);
    drop_obj('DROP PROCEDURE hospital.USP_GET_PATIENT_RECORDS', -4043);
    drop_obj('DROP PROCEDURE hospital.USP_GET_PATIENT_PRESCRIPTIONS', -4043);
    drop_obj('DROP PROCEDURE hospital.USP_GET_PATIENT_SERVICES', -4043);
    drop_obj('DROP PROCEDURE hospital.USP_UPDATE_PATIENT_CONTACT', -4043);
    drop_obj('DROP VIEW hospital.V_TECHNICIAN_SERVICE_RECORD', -942);
    drop_obj('DROP PROCEDURE hospital.GET_TECHNICIAN_SERVICE_RECORDS', -4043);
END;
/

PROMPT ==============================================================================



PROMPT ==============================================================================
PROMPT START: Fixed Doctor setup section inside Setup_All_Roles_Fixed.sql
PROMPT ==============================================================================

-- ==============================================================================
-- Combined_Doctor_Fixed.sql
-- Purpose: Fixed single-file setup for Doctor RBAC + VPD + stored procedures.
-- Replaces the legacy doctor files:
--   - VPD_Doctor.sql
--   - StoreProcedure_Doctor.sql
--   - GrantPrivileges_Doctor.sql
--
-- Run as: HOSPITAL_DBA or another admin user with privileges to create objects
--         in HOSPITAL and manage DBMS_RLS policies.
-- Connect to: PDB_QLYT before running this script.
-- If connected to CDB$ROOT as a privileged common user, uncomment this line:
-- ALTER SESSION SET CONTAINER = PDB_QLYT;
--
-- Important fixes compared with legacy doctor scripts:
--   1. Does NOT delete every POL_VPD_% or FN_VPD_% object.
--      It only drops known doctor policies/functions/procedures.
--   2. Doctor VPD only restricts actual active doctor users.
--      Non-doctor roles are not filtered by doctor logic, so coordinator/patient/
--      technician procedures/views are not accidentally broken.
--   3. Procedure cleanup uses the real procedure names, not only USP_DOCTOR_%.
--   4. Doctor service requests insert technician_id = NULL so coordinator can assign
--      the technician later. The old random technician assignment was removed.
--   5. Stored procedures use explicit schema names and AUTHID DEFINER.
-- ==============================================================================

SET SERVEROUTPUT ON;
SET DEFINE OFF;

PROMPT ==============================================================================
PROMPT START: Fixed Doctor setup
PROMPT ==============================================================================

ALTER SESSION SET CURRENT_SCHEMA = hospital;

-- ==============================================================================
-- 1. Safe cleanup of doctor-only legacy/fixed objects
-- ==============================================================================
PROMPT ==============================================================================
PROMPT SECTION 1: Safe cleanup of doctor-only legacy/fixed objects
PROMPT ==============================================================================

DECLARE
    PROCEDURE try_exec(p_sql IN VARCHAR2) IS
    BEGIN
        EXECUTE IMMEDIATE p_sql;
        DBMS_OUTPUT.PUT_LINE('[OK]   ' || p_sql);
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('[SKIP] ' || p_sql || ' -> ' || SQLERRM);
    END;

    PROCEDURE drop_policy_safe(p_object_name IN VARCHAR2, p_policy_name IN VARCHAR2) IS
    BEGIN
        DBMS_RLS.DROP_POLICY('HOSPITAL', p_object_name, p_policy_name);
        DBMS_OUTPUT.PUT_LINE('[OK]   Dropped policy HOSPITAL.' || p_object_name || '.' || p_policy_name);
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('[SKIP] Drop policy HOSPITAL.' || p_object_name || '.' || p_policy_name || ' -> ' || SQLERRM);
    END;
BEGIN
    DBMS_OUTPUT.PUT_LINE('Dropping old generic doctor VPD policies, if present...');
    drop_policy_safe('MEDICAL_RECORD', 'POL_VPD_MEDICAL_RECORD_READ');
    drop_policy_safe('MEDICAL_RECORD', 'POL_VPD_MEDICAL_RECORD_UPDATE');
    drop_policy_safe('PATIENT',        'POL_VPD_PATIENT_READ');
    drop_policy_safe('PATIENT',        'POL_VPD_PATIENT_UPDATE');
    drop_policy_safe('SERVICE_RECORD', 'POL_VPD_SERVICE_RECORD_READ_DELETE');
    drop_policy_safe('SERVICE_RECORD', 'POL_VPD_SERVICE_RECORD_INSERT');
    drop_policy_safe('PRESCRIPTION',   'POL_VPD_PRESCRIPTION_READ_DELETE');
    drop_policy_safe('PRESCRIPTION',   'POL_VPD_PRESCRIPTION_UPDATE');

    DBMS_OUTPUT.PUT_LINE('Dropping fixed doctor VPD policies, if present...');
    drop_policy_safe('MEDICAL_RECORD', 'POL_DOC_MEDICAL_RECORD_SEL');
    drop_policy_safe('MEDICAL_RECORD', 'POL_DOC_MEDICAL_RECORD_UPD');
    drop_policy_safe('PATIENT',        'POL_DOC_PATIENT_SEL');
    drop_policy_safe('PATIENT',        'POL_DOC_PATIENT_UPD');
    drop_policy_safe('SERVICE_RECORD', 'POL_DOC_SERVICE_RECORD_SEL_DEL');
    drop_policy_safe('SERVICE_RECORD', 'POL_DOC_SERVICE_RECORD_INS');
    drop_policy_safe('PRESCRIPTION',   'POL_DOC_PRESCRIPTION_SEL_DEL');
    drop_policy_safe('PRESCRIPTION',   'POL_DOC_PRESCRIPTION_INS_UPD');

    DBMS_OUTPUT.PUT_LINE('Dropping known doctor stored procedures, if present...');
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

    DBMS_OUTPUT.PUT_LINE('Dropping legacy doctor procedures matching USP_DOCTOR_%, if present...');
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
-- 2. Doctor role grants
-- ==============================================================================
PROMPT ==============================================================================
PROMPT SECTION 2: Doctor role grants
PROMPT ==============================================================================

-- Table grants for direct role access. VPD policies below enforce row-level doctor scope.
GRANT SELECT, UPDATE(medical_history, family_medical_history, drug_allergies)
ON hospital.patient TO rl_doctor;

GRANT SELECT, UPDATE(diagnosis, treatment_plan, conclusion)
ON hospital.medical_record TO rl_doctor;

GRANT SELECT, INSERT, DELETE
ON hospital.service_record TO rl_doctor;

GRANT SELECT, INSERT, UPDATE, DELETE
ON hospital.prescription TO rl_doctor;

-- Direct STAFF access is still limited by the shared STAFF self-VPD from the coordinator setup.
GRANT SELECT, UPDATE(phone, hometown)
ON hospital.staff TO rl_doctor;

-- Shared helper procedures owned by HOSPITAL_DBA.
GRANT EXECUTE ON hospital_dba.USP_GET_SESSION_ROLE TO rl_doctor;
GRANT EXECUTE ON hospital_dba.USP_GET_GRANTED_ROLE TO rl_doctor;

-- ==============================================================================
-- 3. Doctor VPD functions
-- ==============================================================================
PROMPT ==============================================================================
PROMPT SECTION 3: Doctor VPD functions
PROMPT ==============================================================================

CREATE OR REPLACE FUNCTION hospital.FN_VPD_MEDICAL_RECORD_DOCTOR (
    p_schema IN VARCHAR2,
    p_obj    IN VARCHAR2
) RETURN VARCHAR2
AUTHID DEFINER
AS
    v_staff_id   hospital.staff.staff_id%TYPE;
    v_staff_role hospital.staff.staff_role%TYPE;
    v_is_active  hospital.staff.is_active%TYPE;
    v_user       VARCHAR2(100);
BEGIN
    v_user := UPPER(SYS_CONTEXT('USERENV', 'SESSION_USER'));

    IF v_user IN ('HOSPITAL', 'HOSPITAL_DBA') THEN
        RETURN '1=1';
    END IF;

    BEGIN
        SELECT staff_id, staff_role, is_active
        INTO v_staff_id, v_staff_role, v_is_active
        FROM hospital.staff
        WHERE UPPER(username_db) = v_user;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            -- Patient application users are not STAFF rows. Do not let doctor VPD
            -- accidentally block patient/coordinator/technician wrappers.
            RETURN '1=1';
    END;

    IF NVL(v_is_active, 0) <> 1 THEN
        RETURN '1=0';
    END IF;

    IF NOT (
        v_staff_role IN (N'Bác sĩ', N'Bác sĩ/Y sĩ')
        OR LOWER(v_staff_role) LIKE N'%bác%'
        OR LOWER(v_staff_role) LIKE '%doctor%'
    ) THEN
        -- Coordinator/technician/staff self procedures must not be restricted by doctor rules.
        RETURN '1=1';
    END IF;

    RETURN 'doctor_id = ''' || REPLACE(v_staff_id, '''', '''''') || '''';
EXCEPTION
    WHEN OTHERS THEN
        RETURN '1=0';
END;
/

CREATE OR REPLACE FUNCTION hospital.FN_VPD_PATIENT_DOCTOR (
    p_schema IN VARCHAR2,
    p_obj    IN VARCHAR2
) RETURN VARCHAR2
AUTHID DEFINER
AS
    v_staff_id   hospital.staff.staff_id%TYPE;
    v_staff_role hospital.staff.staff_role%TYPE;
    v_is_active  hospital.staff.is_active%TYPE;
    v_user       VARCHAR2(100);
BEGIN
    v_user := UPPER(SYS_CONTEXT('USERENV', 'SESSION_USER'));

    IF v_user IN ('HOSPITAL', 'HOSPITAL_DBA') THEN
        RETURN '1=1';
    END IF;

    BEGIN
        SELECT staff_id, staff_role, is_active
        INTO v_staff_id, v_staff_role, v_is_active
        FROM hospital.staff
        WHERE UPPER(username_db) = v_user;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RETURN '1=1';
    END;

    IF NVL(v_is_active, 0) <> 1 THEN
        RETURN '1=0';
    END IF;

    IF NOT (
        v_staff_role IN (N'Bác sĩ', N'Bác sĩ/Y sĩ')
        OR LOWER(v_staff_role) LIKE N'%bác%'
        OR LOWER(v_staff_role) LIKE '%doctor%'
    ) THEN
        RETURN '1=1';
    END IF;

    RETURN 'patient_id IN (' ||
           'SELECT m.patient_id FROM hospital.medical_record m ' ||
           'WHERE m.doctor_id = ''' || REPLACE(v_staff_id, '''', '''''') || ''')';
EXCEPTION
    WHEN OTHERS THEN
        RETURN '1=0';
END;
/

CREATE OR REPLACE FUNCTION hospital.FN_VPD_RECORD_DETAIL_DOCTOR (
    p_schema IN VARCHAR2,
    p_obj    IN VARCHAR2
) RETURN VARCHAR2
AUTHID DEFINER
AS
    v_staff_id   hospital.staff.staff_id%TYPE;
    v_staff_role hospital.staff.staff_role%TYPE;
    v_is_active  hospital.staff.is_active%TYPE;
    v_user       VARCHAR2(100);
BEGIN
    v_user := UPPER(SYS_CONTEXT('USERENV', 'SESSION_USER'));

    IF v_user IN ('HOSPITAL', 'HOSPITAL_DBA') THEN
        RETURN '1=1';
    END IF;

    BEGIN
        SELECT staff_id, staff_role, is_active
        INTO v_staff_id, v_staff_role, v_is_active
        FROM hospital.staff
        WHERE UPPER(username_db) = v_user;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RETURN '1=1';
    END;

    IF NVL(v_is_active, 0) <> 1 THEN
        RETURN '1=0';
    END IF;

    IF NOT (
        v_staff_role IN (N'Bác sĩ', N'Bác sĩ/Y sĩ')
        OR LOWER(v_staff_role) LIKE N'%bác%'
        OR LOWER(v_staff_role) LIKE '%doctor%'
    ) THEN
        RETURN '1=1';
    END IF;

    RETURN 'record_id IN (' ||
           'SELECT m.record_id FROM hospital.medical_record m ' ||
           'WHERE m.doctor_id = ''' || REPLACE(v_staff_id, '''', '''''') || ''')';
EXCEPTION
    WHEN OTHERS THEN
        RETURN '1=0';
END;
/

-- ==============================================================================
-- 4. Apply fixed doctor VPD policies
-- ==============================================================================
PROMPT ==============================================================================
PROMPT SECTION 4: Apply fixed doctor VPD policies
PROMPT ==============================================================================

BEGIN
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL',
        object_name     => 'MEDICAL_RECORD',
        policy_name     => 'POL_DOC_MEDICAL_RECORD_SEL',
        function_schema => 'HOSPITAL',
        policy_function => 'FN_VPD_MEDICAL_RECORD_DOCTOR',
        statement_types => 'SELECT'
    );

    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL',
        object_name     => 'MEDICAL_RECORD',
        policy_name     => 'POL_DOC_MEDICAL_RECORD_UPD',
        function_schema => 'HOSPITAL',
        policy_function => 'FN_VPD_MEDICAL_RECORD_DOCTOR',
        statement_types => 'UPDATE',
        update_check    => TRUE
    );

    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL',
        object_name     => 'PATIENT',
        policy_name     => 'POL_DOC_PATIENT_SEL',
        function_schema => 'HOSPITAL',
        policy_function => 'FN_VPD_PATIENT_DOCTOR',
        statement_types => 'SELECT'
    );

    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL',
        object_name     => 'PATIENT',
        policy_name     => 'POL_DOC_PATIENT_UPD',
        function_schema => 'HOSPITAL',
        policy_function => 'FN_VPD_PATIENT_DOCTOR',
        statement_types => 'UPDATE',
        update_check    => TRUE
    );

    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL',
        object_name     => 'SERVICE_RECORD',
        policy_name     => 'POL_DOC_SERVICE_RECORD_SEL_DEL',
        function_schema => 'HOSPITAL',
        policy_function => 'FN_VPD_RECORD_DETAIL_DOCTOR',
        statement_types => 'SELECT,DELETE'
    );

    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL',
        object_name     => 'SERVICE_RECORD',
        policy_name     => 'POL_DOC_SERVICE_RECORD_INS',
        function_schema => 'HOSPITAL',
        policy_function => 'FN_VPD_RECORD_DETAIL_DOCTOR',
        statement_types => 'INSERT',
        update_check    => TRUE
    );

    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL',
        object_name     => 'PRESCRIPTION',
        policy_name     => 'POL_DOC_PRESCRIPTION_SEL_DEL',
        function_schema => 'HOSPITAL',
        policy_function => 'FN_VPD_RECORD_DETAIL_DOCTOR',
        statement_types => 'SELECT,DELETE'
    );

    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL',
        object_name     => 'PRESCRIPTION',
        policy_name     => 'POL_DOC_PRESCRIPTION_INS_UPD',
        function_schema => 'HOSPITAL',
        policy_function => 'FN_VPD_RECORD_DETAIL_DOCTOR',
        statement_types => 'INSERT,UPDATE',
        update_check    => TRUE
    );
END;
/

-- ==============================================================================
-- 5. Doctor stored procedures
-- ==============================================================================
PROMPT ==============================================================================
PROMPT SECTION 5: Doctor stored procedures
PROMPT ==============================================================================

CREATE OR REPLACE PROCEDURE hospital.USP_GET_MEDICAL_RECORD(
    p_s IN NVARCHAR2,
    p_c OUT SYS_REFCURSOR
)
AUTHID DEFINER
AS
    v_s NVARCHAR2(4000) := NVL(p_s, N'');
BEGIN
    OPEN p_c FOR
        SELECT
            m.record_id,
            m.patient_id,
            p.full_name,
            TO_CHAR(m.record_date, 'DD/MM/YYYY') AS record_date,
            m.diagnosis,
            m.treatment_plan,
            m.conclusion
        FROM hospital.medical_record m
        JOIN hospital.patient p
            ON p.patient_id = m.patient_id
        WHERE m.record_id LIKE '%' || UPPER(TRIM(v_s)) || '%'
           OR m.patient_id LIKE '%' || UPPER(TRIM(v_s)) || '%'
           OR LOWER(p.full_name) LIKE '%' || LOWER(v_s) || '%'
           OR LOWER(m.diagnosis) LIKE '%' || LOWER(v_s) || '%'
           OR LOWER(m.treatment_plan) LIKE '%' || LOWER(v_s) || '%'
           OR LOWER(m.conclusion) LIKE '%' || LOWER(v_s) || '%'
           OR TO_CHAR(m.record_date, 'DD/MM/YYYY') LIKE '%' || v_s || '%'
        ORDER BY m.record_date DESC, m.record_id;
END;
/

CREATE OR REPLACE PROCEDURE hospital.USP_UPDATE_MEDICAL_RECORD(
    p_id IN VARCHAR2,
    p_dg IN NVARCHAR2,
    p_tr IN NVARCHAR2,
    p_cl IN NVARCHAR2
)
AUTHID DEFINER
AS
BEGIN
    UPDATE hospital.medical_record
    SET diagnosis      = p_dg,
        treatment_plan = p_tr,
        conclusion     = p_cl
    WHERE record_id = UPPER(TRIM(p_id));

    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20002, N'Lỗi: Không tìm thấy hồ sơ bệnh án hoặc bác sĩ không có quyền cập nhật hồ sơ này.');
    END IF;
END;
/

CREATE OR REPLACE PROCEDURE hospital.USP_GET_SERVICES(
    p_s IN NVARCHAR2,
    p_c OUT SYS_REFCURSOR
)
AUTHID DEFINER
AS
    v_s NVARCHAR2(4000) := NVL(p_s, N'');
BEGIN
    OPEN p_c FOR
        SELECT
            record_id,
            service_type,
            TO_CHAR(service_date, 'DD/MM/YYYY') AS service_date,
            technician_id,
            service_result
        FROM hospital.service_record
        WHERE record_id LIKE '%' || UPPER(TRIM(v_s)) || '%'
           OR LOWER(service_type) LIKE '%' || LOWER(v_s) || '%'
           OR TO_CHAR(service_date, 'DD/MM/YYYY') LIKE '%' || v_s || '%'
           OR LOWER(service_result) LIKE '%' || LOWER(v_s) || '%'
        ORDER BY service_date DESC, record_id, service_type;
END;
/

CREATE OR REPLACE PROCEDURE hospital.USP_ADD_SERVICE(
    p_id   IN VARCHAR2,
    p_type IN NVARCHAR2,
    p_res  IN NVARCHAR2 DEFAULT NULL
)
AUTHID DEFINER
AS
    v_count NUMBER;
    v_id    VARCHAR2(20) := UPPER(TRIM(p_id));
BEGIN
    -- This SELECT is protected by doctor VPD. A doctor can only create service
    -- requests for medical records assigned to themselves.
    SELECT COUNT(*)
    INTO v_count
    FROM hospital.medical_record
    WHERE record_id = v_id;

    IF v_count = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, N'Mã HSBA không tồn tại hoặc bác sĩ không có quyền tạo dịch vụ cho HSBA này.');
    END IF;

    -- Coordinator assigns the technician later. Do not randomly assign one here.
    -- p_res is kept only for backward compatibility with the old UI signature.
    INSERT INTO hospital.service_record (
        record_id,
        service_type,
        service_date,
        technician_id,
        service_result
    ) VALUES (
        v_id,
        p_type,
        TRUNC(SYSDATE),
        NULL,
        NULL
    );
EXCEPTION
    WHEN DUP_VAL_ON_INDEX THEN
        RAISE_APPLICATION_ERROR(-20006, N'Lỗi: Dịch vụ này đã tồn tại cho HSBA/ngày hiện tại.');
END;
/

CREATE OR REPLACE PROCEDURE hospital.USP_DELETE_SERVICE(
    p_id   IN VARCHAR2,
    p_type IN NVARCHAR2,
    p_date IN DATE
)
AUTHID DEFINER
AS
BEGIN
    DELETE FROM hospital.service_record
    WHERE record_id = UPPER(TRIM(p_id))
      AND service_type = p_type
      AND TRUNC(service_date) = TRUNC(p_date);

    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20007, N'Lỗi: Không tìm thấy dịch vụ hoặc bác sĩ không có quyền xóa dịch vụ này.');
    END IF;
END;
/

CREATE OR REPLACE PROCEDURE hospital.USP_GET_PRESCRIPTION(
    p_s IN NVARCHAR2,
    p_c OUT SYS_REFCURSOR
)
AUTHID DEFINER
AS
    v_s NVARCHAR2(4000) := NVL(p_s, N'');
BEGIN
    OPEN p_c FOR
        SELECT
            record_id,
            TO_CHAR(prescription_date, 'DD/MM/YYYY') AS prescription_date,
            medicine_name,
            dosage
        FROM hospital.prescription
        WHERE record_id LIKE '%' || UPPER(TRIM(v_s)) || '%'
           OR LOWER(medicine_name) LIKE '%' || LOWER(v_s) || '%'
           OR LOWER(dosage) LIKE '%' || LOWER(v_s) || '%'
           OR TO_CHAR(prescription_date, 'DD/MM/YYYY') LIKE '%' || v_s || '%'
        ORDER BY prescription_date DESC, record_id, medicine_name;
END;
/

CREATE OR REPLACE PROCEDURE hospital.USP_MANAGE_PRESCRIPTION(
    p_action       IN VARCHAR2,
    p_record_id    IN VARCHAR2,
    p_med_name     IN NVARCHAR2,
    p_dosage       IN NVARCHAR2,
    p_date         IN DATE DEFAULT NULL,
    p_old_med_name IN NVARCHAR2 DEFAULT NULL
)
AUTHID DEFINER
AS
    v_count  NUMBER;
    v_id     VARCHAR2(20) := UPPER(TRIM(p_record_id));
    v_action VARCHAR2(20) := UPPER(TRIM(p_action));
BEGIN
    IF v_action NOT IN ('INSERT', 'UPDATE', 'DELETE') THEN
        RAISE_APPLICATION_ERROR(-20008, N'Lỗi: Hành động đơn thuốc không hợp lệ.');
    END IF;

    IF v_action IN ('INSERT', 'UPDATE') THEN
        -- VPD ensures the current doctor can only use their own medical records.
        SELECT COUNT(*)
        INTO v_count
        FROM hospital.medical_record
        WHERE record_id = v_id;

        IF v_count = 0 THEN
            RAISE_APPLICATION_ERROR(-20003, N'Lỗi: Mã HSBA không tồn tại hoặc bác sĩ không có quyền kê đơn cho HSBA này.');
        END IF;
    END IF;

    IF v_action = 'INSERT' THEN
        SELECT COUNT(*)
        INTO v_count
        FROM hospital.prescription
        WHERE record_id = v_id
          AND TRUNC(prescription_date) = TRUNC(SYSDATE)
          AND medicine_name = p_med_name;

        IF v_count > 0 THEN
            RAISE_APPLICATION_ERROR(-20005, N'Lỗi: Thuốc này đã được kê trong đơn của ngày hôm nay.');
        END IF;

        INSERT INTO hospital.prescription (
            record_id,
            prescription_date,
            medicine_name,
            dosage
        ) VALUES (
            v_id,
            TRUNC(SYSDATE),
            p_med_name,
            p_dosage
        );

    ELSIF v_action = 'UPDATE' THEN
        IF p_date IS NULL OR p_old_med_name IS NULL THEN
            RAISE_APPLICATION_ERROR(-20009, N'Lỗi: Cập nhật đơn thuốc cần ngày kê đơn và tên thuốc cũ.');
        END IF;

        UPDATE hospital.prescription
        SET medicine_name = p_med_name,
            dosage        = p_dosage
        WHERE record_id = v_id
          AND TRUNC(prescription_date) = TRUNC(p_date)
          AND medicine_name = p_old_med_name;

        IF SQL%ROWCOUNT = 0 THEN
            RAISE_APPLICATION_ERROR(-20010, N'Lỗi: Không tìm thấy đơn thuốc hoặc bác sĩ không có quyền cập nhật.');
        END IF;

    ELSIF v_action = 'DELETE' THEN
        IF p_date IS NULL THEN
            RAISE_APPLICATION_ERROR(-20011, N'Lỗi: Xóa đơn thuốc cần ngày kê đơn.');
        END IF;

        DELETE FROM hospital.prescription
        WHERE record_id = v_id
          AND TRUNC(prescription_date) = TRUNC(p_date)
          AND medicine_name = p_med_name;

        IF SQL%ROWCOUNT = 0 THEN
            RAISE_APPLICATION_ERROR(-20012, N'Lỗi: Không tìm thấy đơn thuốc hoặc bác sĩ không có quyền xóa.');
        END IF;
    END IF;
END;
/

CREATE OR REPLACE PROCEDURE hospital.USP_GET_PATIENTS(
    p_s IN NVARCHAR2,
    p_c OUT SYS_REFCURSOR
)
AUTHID DEFINER
AS
    v_s NVARCHAR2(4000) := NVL(p_s, N'');
BEGIN
    OPEN p_c FOR
        SELECT
            patient_id,
            full_name,
            gender,
            TO_CHAR(birthdate, 'DD/MM/YYYY') AS birthdate,
            medical_history,
            family_medical_history,
            drug_allergies
        FROM hospital.patient
        WHERE LOWER(full_name) LIKE '%' || LOWER(v_s) || '%'
           OR patient_id LIKE '%' || UPPER(TRIM(v_s)) || '%'
           OR LOWER(gender) LIKE '%' || LOWER(v_s) || '%'
           OR TO_CHAR(birthdate, 'DD/MM/YYYY') LIKE '%' || v_s || '%'
           OR LOWER(medical_history) LIKE '%' || LOWER(v_s) || '%'
           OR LOWER(family_medical_history) LIKE '%' || LOWER(v_s) || '%'
           OR LOWER(drug_allergies) LIKE '%' || LOWER(v_s) || '%'
        ORDER BY full_name, patient_id;
END;
/

CREATE OR REPLACE PROCEDURE hospital.USP_UPDATE_PATIENT(
    p_id             IN VARCHAR2,
    p_history        IN CLOB,
    p_family_history IN CLOB,
    p_allergy        IN CLOB
)
AUTHID DEFINER
AS
BEGIN
    UPDATE hospital.patient
    SET medical_history        = p_history,
        family_medical_history = p_family_history,
        drug_allergies         = p_allergy
    WHERE patient_id = UPPER(TRIM(p_id));

    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20004, N'Lỗi: Không tìm thấy bệnh nhân hoặc bác sĩ không có quyền cập nhật bệnh nhân này.');
    END IF;
END;
/

CREATE OR REPLACE PROCEDURE hospital.USP_GET_SELF_INFO(
    p_c OUT SYS_REFCURSOR
)
AUTHID DEFINER
AS
BEGIN
    OPEN p_c FOR
        SELECT
            s.full_name,
            s.gender,
            s.birthdate,
            s.id_card,
            s.hometown,
            s.phone,
            s.staff_role,
            d.dept_name
        FROM hospital.staff s
        LEFT JOIN hospital.department d
            ON d.dept_id = s.dept_id
        WHERE UPPER(s.username_db) = UPPER(SYS_CONTEXT('USERENV', 'SESSION_USER'))
          AND s.is_active = 1;
END;
/

CREATE OR REPLACE PROCEDURE hospital.USP_UPDATE_SELF_INFO(
    p_hometown IN NVARCHAR2,
    p_phone    IN VARCHAR2
)
AUTHID DEFINER
AS
BEGIN
    UPDATE hospital.staff
    SET hometown = CASE
                       WHEN p_hometown IS NULL OR TRIM(p_hometown) = '' THEN hometown
                       ELSE p_hometown
                   END,
        phone    = CASE
                       WHEN p_phone IS NULL OR TRIM(p_phone) = '' THEN phone
                       ELSE p_phone
                   END
    WHERE UPPER(username_db) = UPPER(SYS_CONTEXT('USERENV', 'SESSION_USER'))
      AND is_active = 1;

    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20013, N'Lỗi: Không tìm thấy thông tin bác sĩ hiện tại hoặc tài khoản đã bị khóa.');
    END IF;
END;
/

-- ==============================================================================
-- 6. Doctor procedure execute grants
-- ==============================================================================
PROMPT ==============================================================================
PROMPT SECTION 6: Doctor procedure execute grants
PROMPT ==============================================================================

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

COMMIT;

-- ==============================================================================
-- 7. Verification
-- ==============================================================================
PROMPT ==============================================================================
PROMPT SECTION 7: Verification - invalid objects and doctor policies
PROMPT ==============================================================================

COLUMN owner FORMAT A20
COLUMN object_name FORMAT A40
COLUMN object_type FORMAT A20
COLUMN status FORMAT A10

SELECT owner, object_name, object_type, status
FROM all_objects
WHERE owner IN ('HOSPITAL', 'HOSPITAL_DBA')
  AND object_type IN ('PROCEDURE', 'FUNCTION', 'VIEW', 'TRIGGER')
  AND status <> 'VALID'
ORDER BY owner, object_type, object_name;

COLUMN name FORMAT A40
COLUMN type FORMAT A20
COLUMN text FORMAT A120

SELECT owner, name, type, line, position, text
FROM all_errors
WHERE owner IN ('HOSPITAL', 'HOSPITAL_DBA')
ORDER BY owner, name, sequence;

COLUMN object_owner FORMAT A20
COLUMN object_name FORMAT A30
COLUMN policy_name FORMAT A35
COLUMN pf_owner FORMAT A20
COLUMN function FORMAT A35

SELECT object_owner, object_name, policy_name, pf_owner, function
FROM all_policies
WHERE object_owner = 'HOSPITAL'
  AND policy_name LIKE 'POL_DOC_%'
ORDER BY object_name, policy_name;

PROMPT ==============================================================================
PROMPT END: Fixed Doctor setup
PROMPT ==============================================================================


PROMPT ==============================================================================
PROMPT DONE: Setup_All_Roles_Fixed.sql finished
PROMPT ==============================================================================
