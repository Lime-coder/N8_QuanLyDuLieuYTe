-- ==============================================================================
-- 01_patient_view.sql
-- Chạy dưới quyền: hospital_dba
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

CREATE OR REPLACE VIEW V_PATIENT_SELF AS
    SELECT patient_id, full_name, gender, birthdate, id_card,
           house_no, street, district, city_province,
           medical_history, family_medical_history, drug_allergies,
           username_db, is_active
    FROM hospital_dba.patient
    WHERE username_db = SYS_CONTEXT('USERENV', 'SESSION_USER');

CREATE OR REPLACE VIEW V_MEDICAL_RECORD_PATIENT AS
    SELECT mr.record_id, mr.patient_id, mr.record_date, mr.diagnosis,
           mr.treatment_plan, mr.doctor_id, mr.dept_id, mr.conclusion,
           s.full_name AS doctor_name, d.dept_name
    FROM hospital_dba.medical_record mr
    JOIN hospital_dba.patient p ON mr.patient_id = p.patient_id
    LEFT JOIN hospital_dba.staff s ON mr.doctor_id = s.staff_id
    LEFT JOIN hospital_dba.department d ON mr.dept_id = d.dept_id
    WHERE p.username_db = SYS_CONTEXT('USERENV', 'SESSION_USER');

CREATE OR REPLACE VIEW V_PRESCRIPTION_PATIENT AS
    SELECT pr.record_id, pr.prescription_date, pr.medicine_name, pr.dosage
    FROM hospital_dba.prescription pr
    JOIN hospital_dba.medical_record mr ON pr.record_id = mr.record_id
    JOIN hospital_dba.patient p ON mr.patient_id = p.patient_id
    WHERE p.username_db = SYS_CONTEXT('USERENV', 'SESSION_USER');

CREATE OR REPLACE VIEW V_SERVICE_RECORD_PATIENT AS
    SELECT sr.record_id, sr.service_type, sr.service_date,
           sr.technician_id, sr.service_result,
           s.full_name AS technician_name
    FROM hospital_dba.service_record sr
    JOIN hospital_dba.medical_record mr ON sr.record_id = mr.record_id
    JOIN hospital_dba.patient p ON mr.patient_id = p.patient_id
    LEFT JOIN hospital_dba.staff s ON sr.technician_id = s.staff_id
    WHERE p.username_db = SYS_CONTEXT('USERENV', 'SESSION_USER');



