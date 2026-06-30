define sys_pass = "my12345" -- Mật khẩu User SYS
define pdb_url = "localhost:1521/PDB_QLYT"
define pdb_name = "PDB_QLYT"

-- Chuyển hướng tới PDB 
ALTER SESSION SET CONTAINER = PDB_QLYT;

-- ============================================================
-- PHẦN 1: TEST YÊU CẦU 3.2 — STANDARD AUDIT (5 NGỮ CẢNH)
-- ============================================================

-- ============================================================
-- NC#1 – TABLE: INSERT trên bảng MEDICAL_RECORD         
-- ============================================================

-- *** Chạy với tài khoản: NV000001 (Điều phối viên - có quyền thêm HSBA) ***
CONNECT NV000001/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THÀNH CÔNG: Điều phối viên lập hồ sơ bệnh án mới
INSERT INTO hospital.medical_record (record_id, patient_id, record_date, doctor_id, dept_id, diagnosis, treatment_plan, conclusion)
VALUES ('BA_TEST002', 'BN000001', SYSDATE, 'NV000021', 'PB01', N'Chưa chẩn đoán', N'Chưa điều trị', N'Chưa kết luận');
COMMIT;

-- *** Chạy với tài khoản: NV000121 (Kỹ thuật viên - không có quyền INSERT) ***
CONNECT NV000121/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THẤT BẠI: Kỹ thuật viên cố tình tự tạo hồ sơ bệnh án
BEGIN
    EXECUTE IMMEDIATE q'[INSERT INTO hospital.medical_record (record_id, diagnosis, treatment_plan, conclusion) VALUES ('BA_TEST002', N'Hack dữ liệu', N'Chưa điều trị', N'Chưa kết luận')]';
EXCEPTION WHEN OTHERS THEN NULL; 
END;
/


-- ============================================================
-- NC#2 – TABLE: UPDATE trên bảng PATIENT                 
-- ============================================================

-- *** Chạy với tài khoản: NV000001 (Điều phối viên – có quyền update patient) ***
CONNECT NV000001/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THÀNH CÔNG: Điều phối viên cập nhật dị ứng thuốc
UPDATE hospital.patient SET drug_allergies = N'Không có dị ứng' WHERE patient_id = 'BN000001';
COMMIT;

-- *** Chạy với tài khoản: NV000121 (Kỹ thuật viên – không có quyền update patient trực tiếp) ***
CONNECT NV000121/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THẤT BẠI: Kỹ thuật viên cố sửa thông tin bệnh nhân trực tiếp
BEGIN
    EXECUTE IMMEDIATE q'[UPDATE hospital.patient SET drug_allergies = 'Dị ứng hải sản' WHERE patient_id = 'BN000001']';
EXCEPTION WHEN OTHERS THEN NULL; END;
/


-- ============================================================
-- NC#3 – VIEW: SELECT trên VW_COORD_DOCTORS       
-- ============================================================

-- *** Chạy với tài khoản: NV000001 (Điều phối viên – có quyền) ***
CONNECT NV000001/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THÀNH CÔNG: Điều phối viên tra cứu danh sách bác sĩ
SELECT * FROM hospital.VW_COORD_DOCTORS;

-- *** Chạy với tài khoản: BN000001 (Bệnh nhân – không được xem view này) ***
CONNECT BN000001/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THẤT BẠI: Bệnh nhân cố xem danh sách bác sĩ
BEGIN
    EXECUTE IMMEDIATE q'[SELECT * FROM hospital.VW_COORD_DOCTORS]';
EXCEPTION WHEN OTHERS THEN NULL; END;
/


-- ============================================================
-- NC#4 – STORED PROCEDURE: EXECUTE USP_MANAGE_PRESCRIPTION 
-- ============================================================

CONNECT NV000021/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THÀNH CÔNG: Bác sĩ kê đơn thuốc mới cho bệnh nhân
BEGIN
    hospital.USP_MANAGE_PRESCRIPTION(
        p_action => 'INSERT',
        p_record_id => 'BA_TEST002',
        p_med_name => N'Paracetamol 500mg',
        p_dosage => N'Ngày 2 viên sáng tối'
    );
END;
/

-- *** Chạy với tài khoản: NV000121 (Kỹ thuật viên – không có quyền execute SP này) ***
CONNECT NV000121/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THẤT BẠI: Kỹ thuật viên lén gọi SP kê đơn thuốc
BEGIN
    hospital.USP_MANAGE_PRESCRIPTION(
        p_action => 'INSERT',
        p_record_id => 'BA_TEST002',
        p_med_name => N'Thuốc ngủ',
        p_dosage => N'1 viên'
    );
END;
/


-- ============================================================
-- NC#5 – FUNCTION: EXECUTE F_EXTRACT_MEDICAL_HISTORY 
-- ============================================================

-- *** Chạy với tài khoản: NV000021 (Bác sĩ – có quyền execute function) ***
CONNECT NV000021/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THÀNH CÔNG: Bác sĩ gọi function trích xuất bệnh sử của bệnh nhân
SELECT hospital_dba.F_EXTRACT_MEDICAL_HISTORY('BN000001') AS tien_su_benh FROM DUAL;

-- *** Chạy với tài khoản: BN000001 (Bệnh nhân – không có quyền execute function) ***
CONNECT BN000001/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THẤT BẠI: Bệnh nhân cố gọi function trích xuất bệnh sử của người khác
BEGIN
    EXECUTE IMMEDIATE q'[SELECT hospital_dba.F_EXTRACT_MEDICAL_HISTORY('BN000002') FROM DUAL]';
EXCEPTION WHEN OTHERS THEN NULL; END;
/

-- ============================================================
-- PHẦN 2: TEST YÊU CẦU 3.3 — FGA + UNIFIED AUDIT
-- ============================================================
-- 3.3a – FGA: Bác sĩ cập nhật các cột ĐƠNTHUỐC (RECORD_ID, PRESCRIPTION_DATE, MEDICINE_NAME, DOSAGE)
-- Đăng nhập với user NV000021
CONNECT NV000021/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;

UPDATE hospital.prescription
SET    medicine_name = 'Omeprazole-updatev2',
       dosage        = N'20mg, 2 viên/ngày'
WHERE  record_id = 'BA999999';
COMMIT;

-- 3.3b – FGA: Bác sĩ cập nhật thành công HSBA của mình (DIAGNOSIS, TREATMENT_PLAN, CONCLUSION)  
-- Đăng nhập với user NV000021
CONNECT NV000021/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
UPDATE hospital.medical_record
SET    diagnosis      = N'Viêm dạ dày cấp - đã hồi phục tốt',
       treatment_plan = N'Ngưng thuốc, ăn uống bình thường',
       conclusion     = N'Khỏi bệnh'
WHERE  record_id = 'BA999999';
COMMIT;

-- 3.3c – UNIFIED: Cập nhật BẤT HỢP PHÁP trên HSBA 
CONNECT NV000121/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
BEGIN
    EXECUTE IMMEDIATE q'[UPDATE hospital.medical_record
SET    diagnosis      = N'Viêm gan',
       treatment_plan = N'Uống thuốc đều đặn',
       conclusion     = N'Theo dõi định kỳ'
WHERE  record_id = 'BA999999']';
EXCEPTION WHEN OTHERS THEN NULL; END;
/

-- 3.3d – UNIFIED: Thao tác BẤT HỢP PHÁP trên SERVICE_RECORD 
CONNECT BN000001/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
BEGIN
    EXECUTE IMMEDIATE q'[INSERT INTO hospital.service_record (record_id, service_type, service_date, technician_id, service_result)
VALUES ('BA_Test002', N'Xét nghiệm máu', SYSDATE, 'NV000021', N'Bình thường')]';
EXCEPTION WHEN OTHERS THEN NULL; END;
/