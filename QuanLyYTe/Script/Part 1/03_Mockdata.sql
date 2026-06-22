-- Run as: hospital_dba | Container: PDB_QLYT
-- ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

-- Departments
INSERT INTO department VALUES ('PB01', N'Nội tổng quát');
INSERT INTO department VALUES ('PB02', N'Ngoại thần kinh');
INSERT INTO department VALUES ('PB03', N'Chẩn đoán hình ảnh');

-- Test Accounts (Explicitly defined for Login/App Testing)
INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, dept_id, facility, staff_role, username_db)
VALUES ('NV000021', N'Nguyễn Văn Minh', N'Nam', TO_DATE('1990-01-01','YYYY-MM-DD'), '999012345678', N'Hà Nội', '0901112223', 'PB01', N'Hồ Chí Minh', N'Bác sĩ', 'NV000021');

INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, dept_id, facility, staff_role, username_db)
VALUES ('NV000001', N'Trần Thị Mai', N'Nữ', TO_DATE('1992-05-15','YYYY-MM-DD'), '999012345679', N'Hà Nội', '0904445556', NULL, N'Hồ Chí Minh', N'Điều phối viên', 'NV000001');

INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, dept_id, facility, staff_role, username_db)
VALUES ('NV000121', N'Lê Văn Tám', N'Nam', TO_DATE('1988-12-10','YYYY-MM-DD'), '999012345680', N'Đà Nẵng', '0907778889', NULL, N'Hồ Chí Minh', N'Kỹ thuật viên', 'NV000121');

INSERT INTO patient (patient_id, full_name, gender, birthdate, id_card, house_no, street, district, city_province, username_db)
VALUES ('BN000001', N'Bệnh nhân Test 1', N'Nam', TO_DATE('1980-01-01','YYYY-MM-DD'), '999080000001', '10', N'Đường số 1', N'Quận 1', N'TP. Hồ Chí Minh', 'BN000001');

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
VALUES ('BA001', 'BN000001', SYSDATE - 5, N'Viêm dạ dày cấp', N'Uống thuốc sau ăn', 'NV000021', 'PB01', N'Tiếp tục theo dõi');

INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion)
VALUES ('BA002', 'BN002', SYSDATE - 2, N'Đau đầu mãn tính', N'Nghỉ ngơi, giảm áp lực', 'NV000021', 'PB01', N'Tái khám sau 1 tuần');

-- Prescriptions
INSERT INTO prescription (record_id, prescription_date, medicine_name, dosage)
VALUES ('BA001', SYSDATE - 5, 'Omeprazole', N'20mg, 1 viên/ngày');

-- Service records
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result)
VALUES ('BA002', N'Chụp MRI não', SYSDATE - 2, 'NV000121', N'Hình ảnh bình thường, không có khối u');

COMMIT;
