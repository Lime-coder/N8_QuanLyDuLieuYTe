-- ==============================================================================
-- 04_seed_medical_record.sql
-- Chạy dưới quyền: hospital_dba (or sysdba with CURRENT_SCHEMA=hospital_dba)
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion)
VALUES ('BA001', 'BN000001', SYSDATE - 5, UNISTR('Vi\00EAm d\1EA1 d\00E0y c\1EA5p'), UNISTR('U\1ED1ng thu\1ED1c sau \0103n'), 'NV000021', 'PB01', UNISTR('Ti\1EBFp t\1EE5c theo d\00F5i'));

-- Adding a second test patient so we can reference BN002 correctly since patient 2 in the loop will be BN000002
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion)
VALUES ('BA002', 'BN000002', SYSDATE - 2, UNISTR('\0110au \0111\1EA7u m\00E3n t\00EDnh'), UNISTR('Ngh\1EC9 ng\01A1i, gi\1EA3m \00E1p l\1EF1c'), 'NV000021', 'PB01', UNISTR('T\00E1i kh\00E1m sau 1 tu\1EA7n'));

COMMIT;

