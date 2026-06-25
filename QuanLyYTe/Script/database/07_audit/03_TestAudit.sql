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

-- *** Chạy với tài khoản: NV000121 (Kỹ thuật viên - không có quyền INSERT) ***
CONNECT NV000121/123@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THẤT BẠI: Kỹ thuật viên cố tình tự tạo hồ sơ bệnh án
BEGIN
    EXECUTE IMMEDIATE q'[INSERT INTO hospital.medical_record (record_id, diagnosis, treatment_plan, conclusion) VALUES ('BA_TEST002', N'Hack dữ liệu', N'Chưa điều trị', N'Chưa kết luận')]';
EXCEPTION WHEN OTHERS THEN NULL; END;
/


-- ============================================================
-- NC#2 – TABLE: DELETE trên bảng SERVICE_RECORD                 
-- ============================================================

-- *** Chạy với tài khoản: BN000001 (Bệnh nhân – không có quyền xóa dịch vụ) ***
CONNECT BN000001/123@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THẤT BẠI: Bệnh nhân cố xóa dịch vụ
BEGIN
    EXECUTE IMMEDIATE q'[DELETE FROM hospital.service_record WHERE record_id = 'BA001']';
EXCEPTION WHEN OTHERS THEN NULL; END;
/


-- ============================================================
-- NC#3 – VIEW: UPDATE trên V_PATIENT_SELF      
-- ============================================================

-- *** Chạy với tài khoản: NV000121 (Kỹ thuật viên – không được sửa view này) ***
CONNECT NV000021/123@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THẤT BẠI: Kỹ thuật viên cố sửa thông tin của bệnh nhân
BEGIN
    EXECUTE IMMEDIATE q'[UPDATE hospital_dba.V_PATIENT_SELF SET drug_allergies = 'Dị ứng hải sản' WHERE patient_id = 'BN000001']';
EXCEPTION WHEN OTHERS THEN NULL; END;
/


-- ============================================================
-- NC#4 – STORED PROCEDURE: EXECUTE USP_MANAGE_PRESCRIPTION 
-- ============================================================

-- *** Chạy với tài khoản: NV000121 (Kỹ thuật viên – không có quyền execute SP này) ***
CONNECT NV000121/123@&pdb_url
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

-- *** Chạy với tài khoản: BN000001 (Bệnh nhân – không có quyền execute function) ***
CONNECT BN000001/123@&pdb_url
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

-- 3.3c – UNIFIED: Cập nhật BẤT HỢP PHÁP trên HSBA 
CONNECT NV000121/123@&pdb_url
SHOW USER;
SHOW CON_NAME;
BEGIN
    EXECUTE IMMEDIATE q'[UPDATE hospital.medical_record
SET    diagnosis      = N'Viêm gan',
       treatment_plan = N'Uống thuốc đều đặn',
       conclusion     = N'Theo dõi định kỳ'
WHERE  record_id = 'BA001']';
EXCEPTION WHEN OTHERS THEN NULL; END;
/

-- 3.3d – UNIFIED: Thao tác BẤT HỢP PHÁP trên SERVICE_RECORD 
CONNECT BN000001/123@&pdb_url
SHOW USER;
SHOW CON_NAME;
BEGIN
    EXECUTE IMMEDIATE q'[INSERT INTO hospital.service_record (record_id, service_type, service_date, technician_id, service_result)
VALUES ('BA_Test002', N'Xét nghiệm máu', SYSDATE, 'NV000021', N'Bình thường')]';
EXCEPTION WHEN OTHERS THEN NULL; END;
/