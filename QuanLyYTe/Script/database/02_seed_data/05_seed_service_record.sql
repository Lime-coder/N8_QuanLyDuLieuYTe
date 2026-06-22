-- ==============================================================================
-- 05_seed_service_record.sql
-- Run as: hospital (or sysdba with CURRENT_SCHEMA=hospital)
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result)
VALUES ('BA002', N'Chụp MRI não', SYSDATE - 2, 'NV000121', N'Hình ảnh bình thường, không có khối u');

COMMIT;
