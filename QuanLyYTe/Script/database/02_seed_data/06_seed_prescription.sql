-- ==============================================================================
-- 06_seed_prescription.sql
-- Chạy dưới quyền: hospital_dba (or sysdba with CURRENT_SCHEMA=hospital_dba)
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

INSERT INTO prescription (record_id, prescription_date, medicine_name, dosage)
VALUES ('BA001', SYSDATE - 5, 'Omeprazole', UNISTR('20mg, 1 vi\00EAn/ng\00E0y'));

COMMIT;

