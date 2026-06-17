define sys_pass = "my12345" -- Mật khẩu User SYS
define pdb_url = "localhost:1521/PDB_QLYT"
define pdb_name = "PDB_QLYT"

-- Chuyển hướng tới PDB 
ALTER SESSION SET CONTAINER = PDB_QLYT;

-- ============================================================
-- PHẦN 1: TEST YÊU CẦU 3.2 — STANDARD AUDIT (5 NGỮ CẢNH)
-- ============================================================

-- ┌─────────────────────────────────────────────────────────┐
-- │ NC#1 – TABLE: DELETE trên bảng STAFF            │
-- │ Mục tiêu: Tạo 1 log Thành Công + 1 log Thất Bại │
-- └─────────────────────────────────────────────────────────┘
-- -- *** Chạy với tài khoản: NV000001 (Điều phối viên) ***
CONNECT NV000001/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- Trường hợp THẤT BẠI: Điều phối viên không có quyền xóa nhân viên
DELETE FROM hospital.staff WHERE staff_id = 'NV000021';
-- Kết quả mong đợi: ORA-01031: insufficient privileges → DBA_AUDIT_TRAIL ghi RETURNCODE != 0

-- *** Chạy với tài khoản: SYSDBA hoặc hospital_dba ***
--CONNECT hospital/123@&pdb_url AS SYSDBA
--SHOW USER;
--SHOW CON_NAME;
---- Trường hợp THÀNH CÔNG: DBA xóa nhân viên giả (sẽ rollback lại)
--INSERT INTO hospital.staff (staff_id, full_name, gender, birthdate, id_card, staff_role, username_db)
--VALUES ('NV_TEST', N'Nhân viên test', N'Nam', SYSDATE, '000000000000', N'Bác sĩ', 'NV_TEST');
--COMMIT;
--DELETE FROM hospital.staff WHERE staff_id = 'NV_TEST';
--COMMIT;
-- Kết quả mong đợi: DBA_AUDIT_TRAIL ghi RETURNCODE = 0 (Thành công)


-- ┌─────────────────────────────────────────────────────────┐
-- │ NC#2 – TABLE: UPDATE trên bảng PATIENT          │
-- │ Mục tiêu: Tạo 1 log Thành Công + 1 log Thất Bại │
-- └─────────────────────────────────────────────────────────┘

-- *** Chạy với tài khoản: NV000001 (Điều phối viên – có quyền update patient) ***
CONNECT NV000001/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THÀNH CÔNG: Điều phối viên cập nhật SĐT bệnh nhân
UPDATE hospital.patient SET phone = '0909999999' WHERE patient_id = 'BN000001';
COMMIT;
-- Kết quả mong đợi: DBA_AUDIT_TRAIL ghi RETURNCODE = 0

-- *** Chạy với tài khoản: NV000021 (Bác sĩ – không có quyền update patient trực tiếp) ***
CONNECT NV000021/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THẤT BẠI: Bác sĩ cố sửa thông tin bệnh nhân trực tiếp
UPDATE hospital.patient SET phone = '0900000000' WHERE patient_id = 'BN000001';
-- Kết quả mong đợi: ORA-01031 → DBA_AUDIT_TRAIL ghi RETURNCODE != 0


-- ┌─────────────────────────────────────────────────────────┐
-- │ NC#3 – VIEW: SELECT trên VW_COORD_DOCTORS       │
-- │ Mục tiêu: Tạo 1 log Thành Công + 1 log Thất Bại │
-- └─────────────────────────────────────────────────────────┘

-- *** Chạy với tài khoản: NV000001 (Điều phối viên – có quyền) ***
CONNECT NV000001/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THÀNH CÔNG: Điều phối viên tra cứu danh sách bác sĩ
SELECT * FROM hospital.VW_COORD_DOCTORS;
-- Kết quả mong đợi: DBA_AUDIT_TRAIL ghi RETURNCODE = 0
-- *** Chạy với tài khoản: NV000121 (Kỹ thuật viên – không được xem view này) ***
CONNECT NV000121/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THẤT BẠI: Kỹ thuật viên cố xem danh sách bác sĩ
SELECT * FROM hospital.VW_COORD_DOCTORS;
-- Kết quả mong đợi: ORA-01031 → DBA_AUDIT_TRAIL ghi RETURNCODE != 0


-- ┌─────────────────────────────────────────────────────────┐
-- │ NC#4 – STORED PROCEDURE: EXECUTE USP_UPDATE_MEDICAL_RECORD │
-- │ Mục tiêu: Tạo 1 log Thành Công + 1 log Thất Bại        │
-- └─────────────────────────────────────────────────────────┘

-- *** Chạy với tài khoản: NV000021 (Bác sĩ – có quyền execute) ***
CONNECT NV000021/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THÀNH CÔNG: Bác sĩ cập nhật HSBA của chính mình
EXEC hospital.USP_UPDATE_MEDICAL_RECORD(
    p_record_id      => 'BA001',
    p_diagnosis      => N'Viêm dạ dày cấp - đã cải thiện',
    p_treatment_plan => N'Tiếp tục uống thuốc',
    p_conclusion     => N'Theo dõi thêm 1 tuần'
);
-- Kết quả mong đợi: DBA_AUDIT_TRAIL ghi RETURNCODE = 0

-- *** Chạy với tài khoản: NV000121 (Kỹ thuật viên – không có quyền execute SP này) ***
CONNECT NV000121/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THẤT BẠI: Kỹ thuật viên cố gọi SP của bác sĩ
EXEC hospital.USP_UPDATE_MEDICAL_RECORD(
    p_record_id      => 'BA001',
    p_diagnosis      => N'Hack chẩn đoán',
    p_treatment_plan => N'...',
    p_conclusion     => N'...'
);
-- Kết quả mong đợi: ORA-01031 → DBA_AUDIT_TRAIL ghi RETURNCODE != 0


-- ┌─────────────────────────────────────────────────────────┐
-- │ NC#5 – FUNCTION: EXECUTE F_GET_DOCTOR_STATS             │
-- │ Mục tiêu: Tạo 1 log Thành Công + 1 log Thất Bại        │
-- └─────────────────────────────────────────────────────────┘

-- *** Chạy với tài khoản: NV000021 (Bác sĩ – có quyền execute function) ***
CONNECT NV000021/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THÀNH CÔNG: Bác sĩ gọi function thống kê số bác sĩ trong khoa PB01
SELECT hospital_dba.F_GET_DOCTOR_STATS('PB01') AS so_bac_si FROM DUAL;
-- Kết quả mong đợi: Trả về số bác sĩ khoa PB01, DBA_AUDIT_TRAIL ghi RETURNCODE = 0

-- *** Chạy với tài khoản: BN000001 (Bệnh nhân – không có quyền execute function) ***
CONNECT BN000001/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
-- THẤT BẠI: Bệnh nhân cố gọi function quản lý nội bộ
SELECT hospital_dba.F_GET_DOCTOR_STATS('PB01') FROM DUAL;
-- Kết quả mong đợi: ORA-00904 hoặc ORA-01031 → DBA_AUDIT_TRAIL ghi RETURNCODE != 0

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
UPDATE hospital.medical_record
SET    conclusion = N'Kỹ thuật viên cố sửa kết luận'
WHERE  record_id = 'BA002';

-- 3.3d – UNIFIED: Thao tác BẤT HỢP PHÁP trên SERVICE_RECORD 
CONNECT NV000121/Abc123456@&pdb_url
SHOW USER;
SHOW CON_NAME;
INSERT INTO hospital.service_record (record_id, service_type, service_date, technician_id, service_result)
VALUES ('BA001', N'Xét nghiệm máu giả', SYSDATE, 'NV000121', N'Bác sĩ tự ghi kết quả');