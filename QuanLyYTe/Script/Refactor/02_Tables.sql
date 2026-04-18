-- Run as: hospital | Container: PDB_QLYT
-- Tables are created under the hospital schema
ALTER SESSION SET CONTAINER = PDB_QLYT;
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
    hometown    NVARCHAR2(200),
    phone       VARCHAR2(15)   NOT NULL,
    dept_id     VARCHAR2(10)   NOT NULL,
    staff_role  NVARCHAR2(50)  NOT NULL,
    username_db VARCHAR2(30)   NOT NULL UNIQUE,
    CONSTRAINT fk_staff_dept   FOREIGN KEY (dept_id) REFERENCES department(dept_id),
    CONSTRAINT chk_staff_role  CHECK (staff_role IN (N'Điều phối viên', N'Bác sĩ', N'Kỹ thuật viên')),
    CONSTRAINT chk_staff_gender CHECK (gender IN (N'Nam', N'Nữ'))
);

CREATE TABLE patient (
    patient_id             VARCHAR2(10)   PRIMARY KEY,
    full_name              NVARCHAR2(100) NOT NULL,
    gender                 NVARCHAR2(5)   NOT NULL,
    birthdate              DATE           NOT NULL,
    id_card                VARCHAR2(20)   NOT NULL UNIQUE,
    house_no               NVARCHAR2(50)  NOT NULL,
    street                 NVARCHAR2(100) NOT NULL,
    district               NVARCHAR2(100) NOT NULL,
    city_province          NVARCHAR2(100) NOT NULL,
    medical_history        CLOB,
    family_medical_history CLOB,
    drug_allergies         CLOB,
    username_db            VARCHAR2(30)   NOT NULL UNIQUE,
    CONSTRAINT chk_patient_gender CHECK (gender IN (N'Nam', N'Nữ'))
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