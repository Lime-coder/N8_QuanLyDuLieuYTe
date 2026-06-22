-- ==============================================================================
-- 01_patient_view.sql
-- Run as: hospital
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

CREATE OR REPLACE VIEW V_PATIENT_SELF AS
    SELECT patient_id, full_name, gender, birthdate, id_card,
           house_no, street, district, city_province,
           medical_history, family_medical_history, drug_allergies,
           username_db, is_active
    FROM hospital.patient
    WHERE username_db = SYS_CONTEXT('USERENV', 'SESSION_USER');

CREATE OR REPLACE VIEW V_MEDICAL_RECORD_PATIENT AS
    SELECT mr.record_id, mr.patient_id, mr.record_date, mr.diagnosis,
           mr.treatment_plan, mr.doctor_id, mr.dept_id, mr.conclusion
    FROM hospital.medical_record mr
    JOIN hospital.patient p ON mr.patient_id = p.patient_id
    WHERE p.username_db = SYS_CONTEXT('USERENV', 'SESSION_USER');

CREATE OR REPLACE VIEW V_PRESCRIPTION_PATIENT AS
    SELECT pr.record_id, pr.prescription_date, pr.medicine_name, pr.dosage
    FROM hospital.prescription pr
    JOIN hospital.medical_record mr ON pr.record_id = mr.record_id
    JOIN hospital.patient p ON mr.patient_id = p.patient_id
    WHERE p.username_db = SYS_CONTEXT('USERENV', 'SESSION_USER');

CREATE OR REPLACE VIEW V_SERVICE_RECORD_PATIENT AS
    SELECT sr.record_id, sr.service_type, sr.service_date,
           sr.technician_id, sr.service_result
    FROM hospital.service_record sr
    JOIN hospital.medical_record mr ON sr.record_id = mr.record_id
    JOIN hospital.patient p ON mr.patient_id = p.patient_id
    WHERE p.username_db = SYS_CONTEXT('USERENV', 'SESSION_USER');
