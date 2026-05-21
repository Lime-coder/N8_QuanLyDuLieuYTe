-- Run as: hospital_dba | Container: PDB_QLYT
ALTER SESSION SET CURRENT_SCHEMA = hospital;

-- 1. XÓA DỮ LIỆU CŨ ĐỂ CHẠY LẠI TỪ ĐẦU
ALTER TABLE service_record DISABLE CONSTRAINT fk_sr_record;
ALTER TABLE service_record DISABLE CONSTRAINT fk_sr_staff;
ALTER TABLE prescription   DISABLE CONSTRAINT fk_presc_record;
ALTER TABLE medical_record DISABLE CONSTRAINT fk_mr_patient;
ALTER TABLE medical_record DISABLE CONSTRAINT fk_mr_staff;
ALTER TABLE medical_record DISABLE CONSTRAINT fk_mr_dept;
ALTER TABLE staff          DISABLE CONSTRAINT fk_staff_dept;

TRUNCATE TABLE prescription;
TRUNCATE TABLE service_record;
TRUNCATE TABLE medical_record;
TRUNCATE TABLE patient;
TRUNCATE TABLE staff;
TRUNCATE TABLE department;

ALTER TABLE staff          ENABLE CONSTRAINT fk_staff_dept;
ALTER TABLE medical_record ENABLE CONSTRAINT fk_mr_patient;
ALTER TABLE medical_record ENABLE CONSTRAINT fk_mr_staff;
ALTER TABLE medical_record ENABLE CONSTRAINT fk_mr_dept;
ALTER TABLE service_record ENABLE CONSTRAINT fk_sr_record;
ALTER TABLE service_record ENABLE CONSTRAINT fk_sr_staff;
ALTER TABLE prescription   ENABLE CONSTRAINT fk_presc_record;

-- 2. THÊM DỮ LIỆU PHÒNG BAN
INSERT INTO department VALUES ('PB01', N'Nội tổng quát');
INSERT INTO department VALUES ('PB02', N'Ngoại thần kinh');
INSERT INTO department VALUES ('PB03', N'Chẩn đoán hình ảnh');
INSERT INTO department VALUES ('PB04', N'Tiêu hóa');
INSERT INTO department VALUES ('PB05', N'Tim mạch');
COMMIT;

-- 3. AUTO-GENERATE 170 NHÂN VIÊN 
-- Tạo 20 Điều phối viên
INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, dept_id, staff_role, username_db)
SELECT 
    'DP' || LPAD(LEVEL, 3, '0'), N'Điều phối viên ' || LEVEL, CASE WHEN MOD(LEVEL, 2) = 0 THEN N'Nữ' ELSE N'Nam' END,
    TO_DATE('1990-01-01','YYYY-MM-DD') + LEVEL, '079100' || LPAD(LEVEL, 6, '0'), N'TP. HCM', '0901' || LPAD(LEVEL, 6, '0'),
    'PB01', N'Điều phối viên', 'DP' || LPAD(LEVEL, 3, '0')
FROM DUAL CONNECT BY LEVEL <= 20;

-- Tạo 100 Bác sĩ
INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, dept_id, staff_role, username_db)
SELECT 
    'BS' || LPAD(LEVEL, 3, '0'), N'Bác sĩ ' || LEVEL, CASE WHEN MOD(LEVEL, 2) = 0 THEN N'Nữ' ELSE N'Nam' END,
    TO_DATE('1985-01-01','YYYY-MM-DD') + LEVEL, '079200' || LPAD(LEVEL, 6, '0'), N'Hà Nội', '0902' || LPAD(LEVEL, 6, '0'),
    'PB' || LPAD(MOD(LEVEL, 5) + 1, 2, '0'), N'Bác sĩ', 'BS' || LPAD(LEVEL, 3, '0')
FROM DUAL CONNECT BY LEVEL <= 100;

-- Tạo 50 Kỹ thuật viên
INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, dept_id, staff_role, username_db)
SELECT 
    'KT' || LPAD(LEVEL, 3, '0'), N'Kỹ thuật viên ' || LEVEL, CASE WHEN MOD(LEVEL, 2) = 0 THEN N'Nữ' ELSE N'Nam' END,
    TO_DATE('1995-01-01','YYYY-MM-DD') + LEVEL, '079300' || LPAD(LEVEL, 6, '0'), N'Hải Phòng', '0903' || LPAD(LEVEL, 6, '0'),
    'PB03', N'Kỹ thuật viên', 'KT' || LPAD(LEVEL, 3, '0')
FROM DUAL CONNECT BY LEVEL <= 50;

COMMIT;

-- 4. AUTO-GENERATE 100.000 BỆNH NHÂN (Sử dụng Hint /*+ APPEND */ để tăng tốc độ Insert)
INSERT /*+ APPEND */ INTO patient (patient_id, full_name, gender, birthdate, id_card, house_no, street, district, city_province, username_db)
SELECT 
    'BN' || LPAD(LEVEL, 6, '0'),
    N'Bệnh nhân ' || LEVEL,
    CASE WHEN MOD(LEVEL, 2) = 0 THEN N'Nữ' ELSE N'Nam' END,
    TO_DATE('1950-01-01','YYYY-MM-DD') + MOD(LEVEL, 20000),
    '001080' || LPAD(LEVEL, 6, '0'),
    TO_CHAR(MOD(LEVEL, 1000) + 1),
    N'Đường số ' || MOD(LEVEL, 100),
    N'Quận ' || (MOD(LEVEL, 12) + 1),
    N'TP. Hồ Chí Minh',
    'BN' || LPAD(LEVEL, 6, '0')
FROM DUAL CONNECT BY LEVEL <= 100000;
COMMIT;

-- 5. AUTO-GENERATE HỒ SƠ BỆNH ÁN VÀ DỊCH VỤ (Sinh ngẫu nhiên 10.000 hồ sơ làm mẫu)
INSERT /*+ APPEND */ INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion)
SELECT 
    'BA' || LPAD(LEVEL, 6, '0'),
    'BN' || LPAD(MOD(LEVEL, 100000) + 1, 6, '0'), -- Lấy ngẫu nhiên bệnh nhân
    SYSDATE - MOD(LEVEL, 365),
    N'Chẩn đoán bệnh lý ' || LEVEL,
    N'Kế hoạch điều trị ' || LEVEL,
    'BS' || LPAD(MOD(LEVEL, 100) + 1, 3, '0'), -- Lấy ngẫu nhiên 1 trong 100 bác sĩ
    'PB' || LPAD(MOD(LEVEL, 5) + 1, 2, '0'),
    N'Tiếp tục theo dõi'
FROM DUAL CONNECT BY LEVEL <= 10000;
COMMIT;