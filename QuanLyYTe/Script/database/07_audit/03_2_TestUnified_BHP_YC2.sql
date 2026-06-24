-- Thay đổi thông tin phù hợp với môi trường của bạn
define sys_pass = "my12345" -- Mật khẩu User SYS
define pdb_url = "localhost:1521/PDB_QLYT"
define pdb_name = "PDB_QLYT"

-- Chuyển hướng tới PDB 
ALTER SESSION SET CONTAINER = PDB_QLYT;

-- ============================================================
-- PHẦN 2: TEST YÊU CẦU 3.3 — FGA + UNIFIED AUDIT
-- ============================================================
-- 3.3a – FGA: Bác sĩ cập nhật các cột ĐƠNTHUỐC (RECORD_ID, PRESCRIPTION_DATE, MEDICINE_NAME, DOSAGE)
-- Đăng nhập với user NV000021: test UI

-- 3.3b – FGA: Bác sĩ cập nhật thành công HSBA của mình (DIAGNOSIS, TREATMENT_PLAN, CONCLUSION)  
-- Đăng nhập với user NV000021: test UI

-- 3.3c – UNIFIED: Cập nhật BẤT HỢP PHÁP trên HSBA 
-- Đăng nhập với user kỹ thuật viên cập nhật trên HSBA
CONNECT NV000121/123@&pdb_url
SHOW USER;
SHOW CON_NAME;
UPDATE hospital.medical_record
SET    diagnosis      = N'Viêm gan',
       treatment_plan = N'Uống thuốc đều đặn',
       conclusion     = N'Theo dõi định kỳ'
WHERE  record_id = 'BA001';
/

-- 3.3d – UNIFIED: Thao tác BẤT HỢP PHÁP trên SERVICE_RECORD (HSBA_DV)
-- Đăng nhập với user bệnh nhân thêm mới 1 HSBA_DV
CONNECT BN000001/123@&pdb_url
SHOW USER;
SHOW CON_NAME;
INSERT INTO hospital.service_record (record_id, service_type, service_date, technician_id, service_result)
VALUES ('BA_Test', N'Xét nghiệm máu', SYSDATE, 'NV000021', N'Không bình thường');
/