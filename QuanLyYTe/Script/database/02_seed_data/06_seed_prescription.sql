-- ==============================================================================
-- 06_seed_prescription.sql
-- Run as: hospital (or sysdba with CURRENT_SCHEMA=hospital)
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

INSERT INTO prescription (record_id, prescription_date, medicine_name, dosage)
VALUES ('BA001', SYSDATE - 5, 'Omeprazole', N'20mg, 1 viên/ngày');

COMMIT;
