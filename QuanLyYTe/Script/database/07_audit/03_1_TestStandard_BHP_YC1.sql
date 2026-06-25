-- Thay đổi thông tin phù hợp với môi trường của bạn
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
-- THÀNH CÔNG: Điều phối viên lập hồ sơ bệnh án mới 
-- Thực hiện trên UI

-- *** Chạy với tài khoản: NV000121 (Kỹ thuật viên - không có quyền INSERT) ***
CONNECT NV000121/123@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THẤT BẠI: Kỹ thuật viên cố tình tự tạo hồ sơ bệnh án
INSERT INTO hospital_dba.medical_record (record_id, patient_id, record_date, doctor_id, dept_id, diagnosis, treatment_plan, conclusion)
VALUES ('BA_TEST', 'BN000001', SYSDATE, 'NV000021', 'PB01', N'Chưa chẩn đoán', N'Chưa điều trị', N'Chưa kết luận');
/


-- ============================================================
-- NC#2 – TABLE: UPDATE trên bảng STAFF              
-- ============================================================

-- *** Chạy với tài khoản: hospital_dba (hospital_dba – có quyền update staff) ***
-- THÀNH CÔNG: hospital_dba cập nhật trạng thái hoạt động của nhân viên
-- Thực hiện trên UI

-- *** Chạy với tài khoản: NV000121 (Kỹ thuật viên – không có quyền update staff trực tiếp) ***
CONNECT NV000121/123@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THẤT BẠI: Kỹ thuật viên cố sửa trạng thái hoạt động của nhân viên
UPDATE hospital_dba.staff SET is_active = 0 WHERE staff_id = 'NV000021';
/

-- ============================================================
-- NC#3 – VIEW: UPDATE trên V_PATIENT_SELF      
-- ============================================================

-- *** Chạy với tài khoản: NV000121 (Kỹ thuật viên – không được sửa view này) ***
CONNECT NV000121/123@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THẤT BẠI: Kỹ thuật viên cố sửa thông tin của bệnh nhân
UPDATE hospital_dba.V_PATIENT_SELF SET drug_allergies = 'Dị ứng hải sản' WHERE patient_id = 'BN000001';
/


-- ============================================================
-- NC#4 – STORED PROCEDURE: EXECUTE USP_MANAGE_PRESCRIPTION 
-- ============================================================
-- Đăng nhập với user bác sĩ
-- THÀNH CÔNG: Bác sĩ kê đơn thuốc mới cho bệnh nhân
-- Thực hiện UI

-- *** Chạy với tài khoản: NV000121 (Kỹ thuật viên – không có quyền execute SP này) ***
CONNECT NV000121/123@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THẤT BẠI: Kỹ thuật viên lén gọi SP kê đơn thuốc
BEGIN
    hospital_dba.USP_MANAGE_PRESCRIPTION(
        p_action => 'INSERT',
        p_record_id => 'BA_TEST',
        p_med_name => N'Thuốc ngủ',
        p_dosage => N'1 viên'
    );
END;
/

-- ============================================================
-- NC#5 – FUNCTION: EXECUTE F_EXTRACT_MEDICAL_HISTORY 
-- ============================================================

-- *** Chạy với tài khoản: NV000021 (Bác sĩ – có quyền execute function) ***
CONNECT NV000021/123@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THÀNH CÔNG: Bác sĩ gọi function trích xuất bệnh sử của bệnh nhân
SELECT hospital_dba.F_EXTRACT_MEDICAL_HISTORY('BN000001') AS tien_su_benh FROM DUAL;

-- *** Chạy với tài khoản: NV000121 (Nhân viên kỹ thuật – không có quyền execute function) ***
CONNECT NV000121/123@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THẤT BẠI: Nhân viên kỹ thuật cố gọi function trích xuất tiền sử bệnh của bệnh nhân
SELECT hospital_dba.F_EXTRACT_MEDICAL_HISTORY('BN000001') FROM DUAL;
/

