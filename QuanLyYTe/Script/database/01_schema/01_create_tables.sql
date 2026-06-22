-- ==============================================================================
-- 01_create_tables.sql
-- Cháº¡y dÆ°á»›i quyá»n: hospital_dba (or sysdba, then grant to hospital)
-- It's best to run this as hospital user, or hospital_dba creates tables in hospital schema.
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

SET SERVEROUTPUT ON;

-- Table: department
CREATE TABLE department (
    dept_id   VARCHAR2(10)   PRIMARY KEY,
    dept_name NVARCHAR2(100) NOT NULL
);

-- Table: staff
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
    is_active   NUMBER(1)      DEFAULT 1 NOT NULL
);

-- Table: patient
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
    is_active              NUMBER(1)      DEFAULT 1 NOT NULL
);

-- Table: medical_record
CREATE TABLE medical_record (
    record_id      VARCHAR2(10)   PRIMARY KEY,
    patient_id     VARCHAR2(10)   NOT NULL,
    record_date    DATE           NOT NULL,
    diagnosis      NVARCHAR2(500) NOT NULL,
    treatment_plan NVARCHAR2(500),
    doctor_id      VARCHAR2(10)   NOT NULL,
    dept_id        VARCHAR2(10)   NOT NULL,
    conclusion     NVARCHAR2(500)
);

-- Table: service_record
CREATE TABLE service_record (
    record_id      VARCHAR2(10)   NOT NULL,
    service_type   NVARCHAR2(100) NOT NULL,
    service_date   DATE           NOT NULL,
    technician_id  VARCHAR2(10),
    service_result NVARCHAR2(500),
    PRIMARY KEY (record_id, service_type, service_date)
);

-- Table: prescription
CREATE TABLE prescription (
    record_id         VARCHAR2(10)   NOT NULL,
    prescription_date DATE           NOT NULL,
    medicine_name     NVARCHAR2(200) NOT NULL,
    dosage            NVARCHAR2(200) NOT NULL,
    PRIMARY KEY (record_id, prescription_date, medicine_name)
);
