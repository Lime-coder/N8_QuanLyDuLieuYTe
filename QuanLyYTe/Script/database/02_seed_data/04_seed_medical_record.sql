-- ==============================================================================
-- 04_seed_medical_record.sql
-- Chạy dưới quyền: hospital (or sysdba with CURRENT_SCHEMA=hospital)
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion)
VALUES ('BA001', 'BN000001', SYSDATE - 5, N'Viêm dạ dày cấp', N'Uống thuốc sau ăn', 'NV000021', 'PB01', N'Tiếp tục theo dõi');

-- Adding a second test patient so we can reference BN002 correctly since patient 2 in the loop will be BN000002
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion)
VALUES ('BA002', 'BN000002', SYSDATE - 2, N'Đau đầu mãn tính', N'Nghỉ ngơi, giảm áp lực', 'NV000021', 'PB01', N'Tái khám sau 1 tuần');

COMMIT;
