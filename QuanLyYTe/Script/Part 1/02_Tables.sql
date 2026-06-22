-- Run as: hospital | Container: PDB_QLYT
-- Tables are created under the hospital schema
-- ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

SET SERVEROUTPUT ON;
BEGIN
    FOR t IN (
        SELECT table_name FROM dba_tables
        WHERE owner = 'HOSPITAL'
          AND table_name IN ('PRESCRIPTION','SERVICE_RECORD','MEDICAL_RECORD','PATIENT','STAFF','DEPARTMENT')
    ) LOOP
        EXECUTE IMMEDIATE 'DROP TABLE hospital.' || t.table_name || ' CASCADE CONSTRAINTS';
    END LOOP;

    FOR r IN (
        SELECT role FROM dba_roles
        WHERE role IN ('RL_COORDINATOR','RL_DOCTOR','RL_TECHNICIAN','RL_PATIENT')
    ) LOOP
        EXECUTE IMMEDIATE 'DROP ROLE ' || r.role;
    END LOOP;
EXCEPTION 
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE(SQLERRM);
END;
/

CREATE ROLE rl_coordinator;
CREATE ROLE rl_doctor;
CREATE ROLE rl_technician;
CREATE ROLE rl_patient;

GRANT CREATE SESSION TO rl_coordinator, rl_doctor, rl_technician, rl_patient;

CREATE TABLE department  (
    dept_id   VARCHAR2(10)   PRIMARY KEY,
    dept_name NVARCHAR2(100) NOT NULL
);

CREATE TABLE staff (
    staff_id    VARCHAR2(10)   PRIMARY KEY,
    full_name   NVARCHAR2(100) NOT NULL,
    gender      NVARCHAR2(5)   NOT NULL,
    birthdate   DATE           NOT NULL,
    id_card     VARCHAR2(20)   NOT NULL UNIQUE,
    hometown    NVARCHAR2(200) NOT NULL,
    phone       VARCHAR2(15)   NOT NULL,
    dept_id     VARCHAR2(10),
    staff_role  NVARCHAR2(50)  NOT NULL,
    username_db VARCHAR2(30)   NOT NULL UNIQUE,
    facility    NVARCHAR2(100) NOT NULL,
    is_active   NUMBER(1)      DEFAULT 1 NOT NULL,
    CONSTRAINT fk_staff_dept   FOREIGN KEY (dept_id) REFERENCES department(dept_id),
    CONSTRAINT chk_staff_role  CHECK (staff_role IN (
        UNISTR('\0110i\1EC1u ph\1ED1i vi\00EAn'),
        UNISTR('B\00E1c s\0129'),
        UNISTR('K\1EF9 thu\1EADt vi\00EAn'),
        UNISTR('T\00E0i v\1EE5'),
        UNISTR('B\00E1c s\0129/Y s\0129')
    )),
    CONSTRAINT chk_staff_gender CHECK (gender IN ('Nam', UNISTR('N\01B0')))
);

CREATE TABLE patient (
    patient_id             VARCHAR2(10)   PRIMARY KEY,
    full_name              NVARCHAR2(100) NOT NULL,
    gender                 NVARCHAR2(5)   NOT NULL,
    birthdate              DATE           NOT NULL,
    id_card                VARCHAR2(20)   NOT NULL UNIQUE,
    house_no               NVARCHAR2(50),
    street                 NVARCHAR2(100),
    district               NVARCHAR2(100),
    city_province          NVARCHAR2(100),
    medical_history        NCLOB,
    family_medical_history NCLOB,
    drug_allergies         NCLOB,
    username_db            VARCHAR2(30)   NOT NULL UNIQUE,
    is_active              NUMBER(1)      DEFAULT 1 NOT NULL,
    CONSTRAINT chk_patient_gender CHECK (gender IN ('Nam', UNISTR('N\01B0')))
);

CREATE TABLE medical_record (
    record_id      VARCHAR2(10)   PRIMARY KEY,
    patient_id     VARCHAR2(10)   NOT NULL,
    record_date    DATE           NOT NULL,
    diagnosis      NVARCHAR2(500) NOT NULL,
    treatment_plan NVARCHAR2(500),
    doctor_id      VARCHAR2(10)   NOT NULL,
    dept_id        VARCHAR2(10)   NOT NULL,
    conclusion     NVARCHAR2(500),
    CONSTRAINT fk_mr_patient FOREIGN KEY (patient_id) REFERENCES patient(patient_id),
    CONSTRAINT fk_mr_staff   FOREIGN KEY (doctor_id)  REFERENCES staff(staff_id),
    CONSTRAINT fk_mr_dept    FOREIGN KEY (dept_id)    REFERENCES department(dept_id)
);

CREATE TABLE service_record (
    record_id      VARCHAR2(10)   NOT NULL,
    service_type   NVARCHAR2(100) NOT NULL,
    service_date   DATE           NOT NULL,
    technician_id  VARCHAR2(10)   NOT NULL,
    service_result NVARCHAR2(500),
    PRIMARY KEY (record_id, service_type, service_date),
    CONSTRAINT fk_sr_record FOREIGN KEY (record_id)     REFERENCES medical_record(record_id),
    CONSTRAINT fk_sr_staff  FOREIGN KEY (technician_id) REFERENCES staff(staff_id)
);

CREATE TABLE prescription (
    record_id         VARCHAR2(10)   NOT NULL,
    prescription_date DATE           NOT NULL,
    medicine_name     NVARCHAR2(200) NOT NULL,
    dosage            NVARCHAR2(200) NOT NULL,
    PRIMARY KEY (record_id, prescription_date, medicine_name),
    CONSTRAINT fk_presc_record FOREIGN KEY (record_id) REFERENCES medical_record(record_id)
);

CREATE SEQUENCE SEQ_STAFF_ID START WITH 171 INCREMENT BY 1;
CREATE SEQUENCE SEQ_PATIENT_ID START WITH 100001 INCREMENT BY 1;

-- Validation Triggers (Safety Guards)
-- Prevent creating medical records with inactive doctors or patients
CREATE OR REPLACE TRIGGER TRG_VALIDATE_MEDICAL_RECORD
BEFORE INSERT ON hospital.medical_record
FOR EACH ROW
DECLARE
    v_patient_active NUMBER(1);
    v_doctor_active NUMBER(1);
BEGIN
    SELECT is_active INTO v_patient_active FROM hospital.patient WHERE patient_id = :NEW.patient_id;
    IF v_patient_active = 0 THEN
        RAISE_APPLICATION_ERROR(-20010, UNISTR('Kh\00F4ng th\1EC3 t\1EA1o h\1ED3 s\01A1: B\1EC7nh nh\00E2n n\00E0y \0111\00E3 b\1ECB kh\00F3a (Kh\00F4ng ho\1EA1t \0111\1ED9ng).'));
    END IF;

    SELECT is_active INTO v_doctor_active FROM hospital.staff WHERE staff_id = :NEW.doctor_id;
    IF v_doctor_active = 0 THEN
        RAISE_APPLICATION_ERROR(-20011, UNISTR('Kh\00F4ng th\1EC3 t\1EA1o h\1ED3 s\01A1: B\00E1c s\0129 n\00E0y \0111\00E3 b\1ECB kh\00F3a (Kh\00F4ng ho\1EA1t \0111\1ED9ng).'));
    END IF;
END;
/

-- Prevent creating service records with inactive technicians
CREATE OR REPLACE TRIGGER TRG_VALIDATE_SERVICE_RECORD
BEFORE INSERT ON hospital.service_record
FOR EACH ROW
DECLARE
    v_tech_active NUMBER(1);
BEGIN
    SELECT is_active INTO v_tech_active FROM hospital.staff WHERE staff_id = :NEW.technician_id;
    IF v_tech_active = 0 THEN
        RAISE_APPLICATION_ERROR(-20012, 'Không thể tạo dịch vụ: Kỹ thuật viên này đã bị khóa (Không hoạt động).');
    END IF;
END;
/