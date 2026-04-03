-- Sử dụng database XEPDB1
ALTER SESSION SET CONTAINER = xepdb1;

-- Mockdata (execute with hospital_dba)
-- 1. Departments
INSERT INTO department (dept_id, dept_name) VALUES ('PB01', N'Nội tổng quát');
INSERT INTO department (dept_id, dept_name) VALUES ('PB02', N'Ngoại thần kinh');
INSERT INTO department (dept_id, dept_name) VALUES ('PB03', N'Chẩn đoán hình ảnh');

-- 2. Staff (Lưu ý staff_role phải khớp với CHECK constraint)
INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, phone, dept_id, staff_role, username_db)
VALUES ('NV001', N'Nguyễn Văn Minh', N'Nam', TO_DATE('1990-01-01','YYYY-MM-DD'), '079012345678', '0901112223', 'PB01', N'Bác sĩ', 'NV001');

INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, phone, dept_id, staff_role, username_db)
VALUES ('NV002', N'Trần Thị Mai', N'Nữ', TO_DATE('1992-05-15','YYYY-MM-DD'), '079012345679', '0904445556', 'PB01', N'Điều phối viên', 'NV002');

INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, phone, dept_id, staff_role, username_db)
VALUES ('NV003', N'Lê Văn Tám', N'Nam', TO_DATE('1988-12-10','YYYY-MM-DD'), '079012345680', '0907778889', 'PB03', N'Kỹ thuật viên', 'NV003');

INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, phone, dept_id, staff_role, username_db)
VALUES ('NV004', N'Phạm minh Anh', N'Nữ', TO_DATE('1995-08-20','YYYY-MM-DD'), '079012345681', '0909990001', 'PB02', N'Bác sĩ', 'NV004');

-- 3. Patients (Username_db ngẫu nhiên cho bảo mật)
INSERT INTO patient (patient_id, full_name, gender, birthdate, id_card, house_no, street, district, city_province, username_db)
SELECT 'BN' || LPAD(LEVEL, 3, '0'), 
       N'Bệnh nhân ' || LEVEL, 
       CASE WHEN MOD(LEVEL, 2) = 0 THEN N'Nữ' ELSE N'Nam' END,
       TO_DATE('1980-01-01','YYYY-MM-DD') + (LEVEL * 365),
       '001080' || LPAD(LEVEL, 6, '0'),
       TO_CHAR(LEVEL * 10), 
       N'Đường số ' || LEVEL, 
       N'Quận ' || MOD(LEVEL, 10), 
       N'TP. Hồ Chí Minh',
       'USR_' || DBMS_RANDOM.STRING('X', 6)
FROM DUAL CONNECT BY LEVEL <= 13;

-- 4. Medical Records mẫu
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion)
VALUES ('BA001', 'BN001', SYSDATE - 5, N'Viêm dạ dày cấp', N'Uống thuốc sau ăn', 'NV001', 'PB01', N'Tiếp tục theo dõi');

INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion)
VALUES ('BA002', 'BN002', SYSDATE - 2, N'Đau đầu mãn tính', N'Nghỉ ngơi, giảm áp lực', 'NV004', 'PB02', N'Tái khám sau 1 tuần');

-- 5. Prescriptions
INSERT INTO prescription (record_id, prescription_date, medicine_name, dosage)
VALUES ('BA001', SYSDATE - 5, 'Omeprazole', N'20mg, 1 viên/ngày');

-- 6. Service Records
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result)
VALUES ('BA002', N'Chụp MRI não', SYSDATE - 2, 'NV003', N'Hình ảnh bình thường, không có khối u');

COMMIT;


/*
-- delete data from every table
-- disable constraints
ALTER TABLE service_record DISABLE CONSTRAINT fk_sr_record;
ALTER TABLE service_record DISABLE CONSTRAINT fk_sr_staff;
ALTER TABLE prescription DISABLE CONSTRAINT fk_presc_record;
ALTER TABLE medical_record DISABLE CONSTRAINT fk_mr_patient;
ALTER TABLE medical_record DISABLE CONSTRAINT fk_mr_staff;
ALTER TABLE medical_record DISABLE CONSTRAINT fk_mr_dept;
ALTER TABLE staff DISABLE CONSTRAINT fk_staff_dept;

-- truncate everything
TRUNCATE TABLE prescription;
TRUNCATE TABLE service_record;
TRUNCATE TABLE medical_record;
TRUNCATE TABLE patient;
TRUNCATE TABLE staff;
TRUNCATE TABLE department;

-- enable constraints back
ALTER TABLE staff ENABLE CONSTRAINT fk_staff_dept;
ALTER TABLE medical_record ENABLE CONSTRAINT fk_mr_patient;
ALTER TABLE medical_record ENABLE CONSTRAINT fk_mr_staff;
ALTER TABLE medical_record ENABLE CONSTRAINT fk_mr_dept;
ALTER TABLE service_record ENABLE CONSTRAINT fk_sr_record;
ALTER TABLE service_record ENABLE CONSTRAINT fk_sr_staff;
ALTER TABLE prescription ENABLE CONSTRAINT fk_presc_record;
*/