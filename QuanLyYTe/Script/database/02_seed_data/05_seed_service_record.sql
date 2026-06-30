-- ==============================================================================
-- 05_seed_service_record.sql
-- Chạy dưới quyền: hospital (or sysdba with CURRENT_SCHEMA=hospital)
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result)
VALUES ('BA002', UNISTR('Ch\1EE5p MRI n\00E3o'), SYSDATE - 2, 'NV000121', UNISTR('H\00ECnh \1EA3nh b\00ECnh th\01B0\1EDDng, kh\00F4ng c\00F3 kh\1ED1i u'));

COMMIT;
