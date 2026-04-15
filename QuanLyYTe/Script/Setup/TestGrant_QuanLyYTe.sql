-- ============================================================
-- FILE: TestGrant_QuanLyYTe.sql
-- MỤC ĐÍCH: Tạo user/role mẫu và cấp quyền để test chức năng
--            Thu hồi (Câu 4) và Xem quyền (Câu 5)
-- CHẠY BỞI: hospital_dba (đã có DBA)
-- ============================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;

-- ============================================================
-- BƯỚC 1: Tạo Oracle DB user mẫu (nếu chưa có)
-- ============================================================
BEGIN
    FOR u IN (SELECT username FROM all_users
              WHERE username IN ('NV001','NV002','NV003','NV004')) LOOP
        EXECUTE IMMEDIATE 'DROP USER ' || u.username || ' CASCADE';
    END LOOP;
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

CREATE USER NV001 IDENTIFIED BY Abc123456;  -- Bác sĩ Nguyễn Văn Minh
CREATE USER NV002 IDENTIFIED BY Abc123456;  -- Điều phối viên Trần Thị Mai
CREATE USER NV003 IDENTIFIED BY Abc123456;  -- Kỹ thuật viên Lê Văn Tám
CREATE USER NV004 IDENTIFIED BY Abc123456;  -- Bác sĩ Phạm Minh Anh


-- ============================================================
-- BƯỚC 2: Cấp quyền SYSTEM PRIVILEGE cho user
-- ============================================================

-- NV001 (Bác sĩ): chỉ cần kết nối
GRANT CREATE SESSION TO NV001;

-- NV002 (Điều phối viên): kết nối + tạo view
GRANT CREATE SESSION TO NV002;
GRANT CREATE VIEW    TO NV002;

-- NV003 (Kỹ thuật viên): kết nối
GRANT CREATE SESSION TO NV003;

-- NV004 (Bác sĩ): kết nối + tạo sequence
GRANT CREATE SESSION  TO NV004;
GRANT CREATE SEQUENCE TO NV004;


-- ============================================================
-- BƯỚC 3: Cấp ROLE cho user
-- ============================================================

-- NV001 nhận role Bác sĩ
GRANT RL_DOCTOR      TO NV001;

-- NV002 nhận role Điều phối viên
GRANT RL_COORDINATOR TO NV002;

-- NV003 nhận role Kỹ thuật viên
GRANT RL_TECHNICIAN  TO NV003;

-- NV004 nhận role Bác sĩ
GRANT RL_DOCTOR      TO NV004;


-- ============================================================
-- BƯỚC 4: Cấp OBJECT PRIVILEGE (quyền trên bảng/view) cho ROLE
-- ============================================================

-- RL_DOCTOR: xem + ghi hồ sơ bệnh án, đơn thuốc
GRANT SELECT, INSERT, UPDATE ON hospital_dba.medical_record TO RL_DOCTOR;
GRANT SELECT, INSERT         ON hospital_dba.prescription   TO RL_DOCTOR;
GRANT SELECT                 ON hospital_dba.patient        TO RL_DOCTOR;
GRANT SELECT                 ON hospital_dba.department     TO RL_DOCTOR;
GRANT SELECT                 ON hospital_dba.staff          TO RL_DOCTOR;

-- RL_COORDINATOR: xem nhân viên, phòng ban, hồ sơ
GRANT SELECT, INSERT, UPDATE, DELETE ON hospital_dba.staff          TO RL_COORDINATOR;
GRANT SELECT, INSERT, UPDATE, DELETE ON hospital_dba.department     TO RL_COORDINATOR;
GRANT SELECT                         ON hospital_dba.patient        TO RL_COORDINATOR;
GRANT SELECT                         ON hospital_dba.medical_record TO RL_COORDINATOR;

-- RL_TECHNICIAN: xem + ghi kết quả dịch vụ
GRANT SELECT         ON hospital_dba.medical_record TO RL_TECHNICIAN;
GRANT SELECT, INSERT, UPDATE ON hospital_dba.service_record  TO RL_TECHNICIAN;
GRANT SELECT         ON hospital_dba.staff          TO RL_TECHNICIAN;

-- RL_PATIENT: chỉ được xem thông tin của mình (hạn chế nhất)
GRANT SELECT ON hospital_dba.patient        TO RL_PATIENT;
GRANT SELECT ON hospital_dba.medical_record TO RL_PATIENT;
GRANT SELECT ON hospital_dba.prescription   TO RL_PATIENT;


-- ============================================================
-- BƯỚC 5: Cấp COLUMN-LEVEL PRIVILEGE cho user cụ thể
-- (NV002 chỉ được xem một số cột nhạy cảm của bảng patient)
-- ============================================================

GRANT SELECT (patient_id, full_name, gender, birthdate)
    ON hospital_dba.patient TO NV002;

-- NV001 chỉ được UPDATE cột kết luận của medical_record
GRANT UPDATE (conclusion, treatment_plan)
    ON hospital_dba.medical_record TO NV001;


-- ============================================================
-- BƯỚC 6: Cấp WITH GRANT OPTION (có thể cấp lại cho người khác)
-- ============================================================

GRANT SELECT ON hospital_dba.department TO NV002 WITH GRANT OPTION;

COMMIT;

-- ============================================================
-- KIỂM TRA: Xem quyền của từng đối tượng vừa cấp
-- ============================================================

-- Xem quyền của NV001
SELECT 'SYSTEM' AS loai, privilege, null AS obj FROM USER_SYS_PRIVS WHERE username = 'NV001'
UNION ALL
SELECT 'ROLE',  granted_role, null FROM USER_ROLE_PRIVS WHERE username = 'NV001'
UNION ALL
SELECT 'TABLE', privilege, table_name FROM ALL_TAB_PRIVS WHERE grantee = 'NV001';

-- Xem quyền của RL_DOCTOR
SELECT 'TABLE/VIEW' AS loai, grantee, privilege, table_name AS doi_tuong
FROM ALL_TAB_PRIVS
WHERE grantee = 'RL_DOCTOR'
ORDER BY table_name, privilege;
