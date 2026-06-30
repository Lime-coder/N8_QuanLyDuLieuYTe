-- ==============================================================================
-- 02_patient_procedures.sql
-- Chạy dưới quyền: hospital
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

CREATE OR REPLACE PROCEDURE USP_GET_PATIENT_PROFILE (
    p_cursor OUT SYS_REFCURSOR
) AUTHID DEFINER AS
BEGIN
    OPEN p_cursor FOR
        SELECT patient_id, full_name, gender, birthdate, id_card,
               house_no, street, district, city_province,
               medical_history, family_medical_history, drug_allergies
        FROM hospital_dba.V_PATIENT_SELF;
END USP_GET_PATIENT_PROFILE;
/

CREATE OR REPLACE PROCEDURE USP_GET_PATIENT_RECORDS (
    p_cursor OUT SYS_REFCURSOR
) AUTHID DEFINER AS
BEGIN
    OPEN p_cursor FOR
        SELECT mr.record_id, mr.record_date, mr.diagnosis,
               mr.treatment_plan, mr.conclusion,
               mr.doctor_name,
               mr.dept_name
        FROM hospital_dba.V_MEDICAL_RECORD_PATIENT mr
        ORDER BY mr.record_date DESC;
END USP_GET_PATIENT_RECORDS;
/

CREATE OR REPLACE PROCEDURE USP_GET_PATIENT_PRESCRIPTIONS (
    p_record_id IN VARCHAR2,
    p_cursor    OUT SYS_REFCURSOR
) AUTHID DEFINER AS
BEGIN
    OPEN p_cursor FOR
        SELECT record_id, prescription_date, medicine_name, dosage
        FROM hospital_dba.V_PRESCRIPTION_PATIENT
        WHERE record_id = p_record_id
        ORDER BY prescription_date;
END USP_GET_PATIENT_PRESCRIPTIONS;
/

CREATE OR REPLACE PROCEDURE USP_GET_PATIENT_SERVICES (
    p_record_id IN VARCHAR2,
    p_cursor    OUT SYS_REFCURSOR
) AUTHID DEFINER AS
BEGIN
    OPEN p_cursor FOR
        SELECT sr.record_id, sr.service_type, sr.service_date,
               sr.service_result,
               sr.technician_name
        FROM hospital_dba.V_SERVICE_RECORD_PATIENT sr
        WHERE sr.record_id = p_record_id
        ORDER BY sr.service_date;
END USP_GET_PATIENT_SERVICES;
/

CREATE OR REPLACE PROCEDURE USP_UPDATE_PATIENT_CONTACT (
    p_house_no               IN NVARCHAR2,
    p_street                 IN NVARCHAR2,
    p_district               IN NVARCHAR2,
    p_city_province          IN NVARCHAR2,
    p_medical_history        IN NVARCHAR2,
    p_family_medical_history IN NVARCHAR2,
    p_drug_allergies         IN NVARCHAR2
) AUTHID DEFINER AS
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


