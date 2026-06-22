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
