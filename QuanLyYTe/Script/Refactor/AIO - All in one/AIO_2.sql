-- Run as: hospital_dba | Container: PDB_QLYT
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
    hometown    NVARCHAR2(200),
    phone       VARCHAR2(15),
    dept_id     VARCHAR2(10),
    staff_role  NVARCHAR2(50)  NOT NULL,
    username_db VARCHAR2(30)   NOT NULL UNIQUE,
    is_active   NUMBER(1)      DEFAULT 1 NOT NULL,
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
    house_no               NVARCHAR2(50),
    street                 NVARCHAR2(100),
    district               NVARCHAR2(100),
    city_province          NVARCHAR2(100),
    medical_history        NCLOB,
    family_medical_history NCLOB,
    drug_allergies         NCLOB,
    username_db            VARCHAR2(30)   NOT NULL UNIQUE,
    is_active              NUMBER(1)      DEFAULT 1 NOT NULL,
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

CREATE SEQUENCE SEQ_STAFF_ID START WITH 5 INCREMENT BY 1;
CREATE SEQUENCE SEQ_PATIENT_ID START WITH 14 INCREMENT BY 1;

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
        RAISE_APPLICATION_ERROR(-20010, 'Không thể tạo hồ sơ: Bệnh nhân này đã bị khóa (Không hoạt động).');
    END IF;

    SELECT is_active INTO v_doctor_active FROM hospital.staff WHERE staff_id = :NEW.doctor_id;
    IF v_doctor_active = 0 THEN
        RAISE_APPLICATION_ERROR(-20011, 'Không thể tạo hồ sơ: Bác sĩ này đã bị khóa (Không hoạt động).');
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

-- Departments
INSERT INTO department VALUES ('PB01', N'Nội tổng quát');
INSERT INTO department VALUES ('PB02', N'Ngoại thần kinh');
INSERT INTO department VALUES ('PB03', N'Chẩn đoán hình ảnh');

-- Test Accounts (Explicitly defined for Login/App Testing)
INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, phone, dept_id, staff_role, username_db)
VALUES ('NV001', N'Nguyễn Văn Minh', N'Nam', TO_DATE('1990-01-01','YYYY-MM-DD'), '999012345678', '0901112223', 'PB01', N'Bác sĩ', 'NV001');

INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, phone, dept_id, staff_role, username_db)
VALUES ('NV002', N'Trần Thị Mai', N'Nữ', TO_DATE('1992-05-15','YYYY-MM-DD'), '999012345679', '0904445556', NULL, N'Điều phối viên', 'NV002');

INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, phone, dept_id, staff_role, username_db)
VALUES ('NV003', N'Lê Văn Tám', N'Nam', TO_DATE('1988-12-10','YYYY-MM-DD'), '999012345680', '0907778889', NULL, N'Kỹ thuật viên', 'NV003');

INSERT INTO patient (patient_id, full_name, gender, birthdate, id_card, house_no, street, district, city_province, username_db)
VALUES ('BN001', N'Bệnh nhân Test 1', N'Nam', TO_DATE('1980-01-01','YYYY-MM-DD'), '999080000001', '10', N'Đường số 1', N'Quận 1', N'TP. Hồ Chí Minh', 'BN001');

-- Patients (random username_db for security)
INSERT INTO patient (patient_id, full_name, gender, birthdate, id_card, house_no, street, district, city_province, username_db)
SELECT 'BN' || LPAD(LEVEL+1, 3, '0'),
       N'Bệnh nhân ' || (LEVEL+1),
       CASE WHEN MOD(LEVEL, 2) = 0 THEN N'Nữ' ELSE N'Nam' END,
       TO_DATE('1980-01-01','YYYY-MM-DD') + (LEVEL * 365),
       '001080' || LPAD(LEVEL+1, 6, '0'),
       TO_CHAR(LEVEL * 10),
       N'Đường số ' || LEVEL,
       N'Quận ' || MOD(LEVEL, 10),
       N'TP. Hồ Chí Minh',
       'USR_' || DBMS_RANDOM.STRING('X', 6)
FROM DUAL CONNECT BY LEVEL <= 12;

-- Medical records
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion)
VALUES ('BA001', 'BN001', SYSDATE - 5, N'Viêm dạ dày cấp', N'Uống thuốc sau ăn', 'NV001', 'PB01', N'Tiếp tục theo dõi');

INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion)
VALUES ('BA002', 'BN002', SYSDATE - 2, N'Đau đầu mãn tính', N'Nghỉ ngơi, giảm áp lực', 'NV001', 'PB01', N'Tái khám sau 1 tuần');

-- Prescriptions
INSERT INTO prescription (record_id, prescription_date, medicine_name, dosage)
VALUES ('BA001', SYSDATE - 5, 'Omeprazole', N'20mg, 1 viên/ngày');

-- Service records
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result)
VALUES ('BA002', N'Chụp MRI não', SYSDATE - 2, 'NV003', N'Hình ảnh bình thường, không có khối u');

COMMIT;