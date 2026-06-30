-- ==============================================================================
-- 07_seed_more_mockdata.sql
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

-- 1. Create 20 new medical records for doctor NV000021
-- using patient_ids BN000101 to BN000120
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA101', 'BN000101', SYSDATE - 10, UNISTR('S\1ED1t si\00EAu vi'), UNISTR('U\1ED1ng nhi\1EC1u n\01B0\1EDBc, h\1EA1 s\1ED1t'), 'NV000021', 'PB01', UNISTR('Theo d\00F5i t\1EA1i nh\00E0'));
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA102', 'BN000102', SYSDATE - 9, UNISTR('Vi\00EAm h\1ECDng'), UNISTR('Kh\00E1ng sinh, kh\00E1ng vi\00EAm'), 'NV000021', 'PB01', UNISTR('Kh\00E1m l\1EA1i sau 5 ng\00E0y'));
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA103', 'BN000103', SYSDATE - 8, UNISTR('R\1ED1i lo\1EA1n ti\00EAu h\00F3a'), UNISTR('Men ti\00EAu h\00F3a'), 'NV000021', 'PB01', UNISTR('Theo d\00F5i th\00EAm'));
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA104', 'BN000104', SYSDATE - 7, UNISTR('Suy nh\01B0\1EE3c'), UNISTR('B\1ED5 sung vitamin'), 'NV000021', 'PB01', UNISTR('\0102n u\1ED1ng \0111i\1EC1u \0111\1ED9'));
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA105', 'BN000105', SYSDATE - 6, UNISTR('Vi\00EAm da d\1ECB \1EE9ng'), UNISTR('B\00F4i thu\1ED1c m\1EE1'), 'NV000021', 'PB01', UNISTR('Tr\00E1nh ti\1EBFp x\00FAc ho\00E1 ch\1EA5t'));
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA106', 'BN000106', SYSDATE - 5, UNISTR('C\1EA3m c\00FAm'), UNISTR('U\1ED1ng thu\1ED1c c\1EA3m'), 'NV000021', 'PB01', UNISTR('Ngh\1EC9 ng\01A1i nhi\1EC1u'));
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA107', 'BN000107', SYSDATE - 4, UNISTR('\0110au d\1EA1 d\00E0y'), UNISTR('Thu\1ED1c gi\1EA3m ti\1EBFt acid'), 'NV000021', 'PB01', UNISTR('Ki\00EAng \0111\1ED3 cay n\00F3ng'));
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA108', 'BN000108', SYSDATE - 3, UNISTR('C\0103ng th\1EB3ng'), UNISTR('Th\01B0 gi\00E3n'), 'NV000021', 'PB01', UNISTR('Theo d\00F5i t\00E2m l\00FD'));
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA109', 'BN000109', SYSDATE - 2, UNISTR('T\0103ng huy\1EBFt \00E1p'), UNISTR('Thu\1ED1c h\1EA1 \00E1p'), 'NV000021', 'PB01', UNISTR('\0110o \00E1p m\1ED7i ng\00E0y'));
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA110', 'BN000110', SYSDATE - 1, UNISTR('\0110au c\01A1'), UNISTR('Gi\1EA3m \0111au, d\00E3n c\01A1'), 'NV000021', 'PB01', UNISTR('H\1EA1n ch\1EBF v\1EADn \0111\1ED9ng m\1EA1nh'));

INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA111', 'BN000111', SYSDATE - 10, UNISTR('G\00E3y t\1EA1m x\01B0\01A1ng'), UNISTR('C\1ED1 \0111\1ECBnh'), 'NV000021', 'PB01', UNISTR('Theo d\00F5i li\1EC1n x\01B0\01A1ng'));
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA112', 'BN000112', SYSDATE - 9, UNISTR('Vi\00EAm m\0169i d\1ECB \1EE9ng'), UNISTR('X\1ECBt m\0169i'), 'NV000021', 'PB01', UNISTR('Tr\00E1nh b\1EE5i b\1EA9n'));
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA113', 'BN000113', SYSDATE - 8, UNISTR('Nh\1EE9c \0111\1EA7u'), UNISTR('Gi\1EA3m \0111au'), 'NV000021', 'PB01', UNISTR('Theo d\00F5i th\00EAm'));
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA114', 'BN000114', SYSDATE - 7, UNISTR('M\1EA5t ng\1EE7'), UNISTR('An th\1EA7n nh\1EB9'), 'NV000021', 'PB01', UNISTR('\0110i\1EC1u ch\1EC9nh sinh ho\1EA1t'));
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA115', 'BN000115', SYSDATE - 6, UNISTR('D\1ECB \1EE9ng h\1EA3i s\1EA3n'), UNISTR('Thu\1ED1c ch\1ED1ng d\1ECB \1EE9ng'), 'NV000021', 'PB01', UNISTR('Tr\00E1nh \0103n h\1EA3i s\1EA3n'));
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA116', 'BN000116', SYSDATE - 5, UNISTR('S\1ECFi th\1EADn nh\1ECF'), UNISTR('U\1ED1ng nhi\1EC1u n\01B0\1EDBc'), 'NV000021', 'PB01', UNISTR('Kh\00E1m l\1EA1i sau 1 th\00E1ng'));
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA117', 'BN000117', SYSDATE - 4, UNISTR('Vi\00EAm ph\1EBF qu\1EA3n'), UNISTR('Kh\00E1ng sinh, long \0111\1EDDm'), 'NV000021', 'PB01', UNISTR('Gi\1EEF \1EA5m c\01A1 th\1EC3'));
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA118', 'BN000118', SYSDATE - 3, UNISTR('Thi\1EBFu m\00E1u'), UNISTR('B\1ED5 sung s\1EAFt'), 'NV000021', 'PB01', UNISTR('\0102n b\1ED5 d\01B0\1EE1ng'));
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA119', 'BN000119', SYSDATE - 2, UNISTR('R\1ED1i lo\1EA1n ti\1EC1n \0111\00ECnh'), UNISTR('Thu\1ED1c c\1EA3i thi\1EC7n tu\1EA7n ho\00E0n'), 'NV000021', 'PB01', UNISTR('H\1EA1n ch\1EBF xoay \0111\1EA7u \0111\1ED9t ng\1ED9t'));
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion) VALUES ('BA120', 'BN000120', SYSDATE - 1, UNISTR('\0110au kh\1EDBp g\1ED1i'), UNISTR('Kh\00E1ng vi\00EAm'), 'NV000021', 'PB01', UNISTR('Gi\1EA3m c\00E2n n\1EB7ng'));

-- 2. Create 10 prescriptions for doctor NV000021
INSERT INTO prescription (record_id, prescription_date, medicine_name, dosage) VALUES ('BA101', SYSDATE - 10, 'Paracetamol', UNISTR('500mg, 2 vi\00EAn/ng\00E0y'));
INSERT INTO prescription (record_id, prescription_date, medicine_name, dosage) VALUES ('BA102', SYSDATE - 9, 'Amoxicillin', UNISTR('500mg, 2 vi\00EAn/ng\00E0y'));
INSERT INTO prescription (record_id, prescription_date, medicine_name, dosage) VALUES ('BA103', SYSDATE - 8, 'Smecta', UNISTR('2 g\00F3i/ng\00E0y'));
INSERT INTO prescription (record_id, prescription_date, medicine_name, dosage) VALUES ('BA104', SYSDATE - 7, 'Vitamin C', UNISTR('1 vi\00EAn/ng\00E0y'));
INSERT INTO prescription (record_id, prescription_date, medicine_name, dosage) VALUES ('BA105', SYSDATE - 6, 'Cetirizine', UNISTR('10mg, 1 vi\00EAn/ng\00E0y'));
INSERT INTO prescription (record_id, prescription_date, medicine_name, dosage) VALUES ('BA106', SYSDATE - 5, 'Decolgen', UNISTR('2 vi\00EAn/ng\00E0y'));
INSERT INTO prescription (record_id, prescription_date, medicine_name, dosage) VALUES ('BA107', SYSDATE - 4, 'Omeprazole', UNISTR('20mg, 1 vi\00EAn/ng\00E0y'));
INSERT INTO prescription (record_id, prescription_date, medicine_name, dosage) VALUES ('BA108', SYSDATE - 3, 'Magnesium B6', UNISTR('2 vi\00EAn/ng\00E0y'));
INSERT INTO prescription (record_id, prescription_date, medicine_name, dosage) VALUES ('BA109', SYSDATE - 2, 'Amlodipine', UNISTR('5mg, 1 vi\00EAn/ng\00E0y'));
INSERT INTO prescription (record_id, prescription_date, medicine_name, dosage) VALUES ('BA110', SYSDATE - 1, 'Ibuprofen', UNISTR('400mg, 2 vi\00EAn/ng\00E0y'));

-- 3. Create 10 service records without technician (for Coordinator to assign)
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA101', UNISTR('X\00E9t nghi\1EC7m m\00E1u'), SYSDATE - 10, NULL, NULL);
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA102', UNISTR('N\1ED9i soi tai m\0169i h\1ECDng'), SYSDATE - 9, NULL, NULL);
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA103', UNISTR('Si\00EAu \00E2m b\1EE5ng'), SYSDATE - 8, NULL, NULL);
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA104', UNISTR('X\00E9t nghi\1EC7m n\01B0\1EDBc ti\1EC3u'), SYSDATE - 7, NULL, NULL);
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA105', UNISTR('Test d\1ECB \1EE9ng'), SYSDATE - 6, NULL, NULL);
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA106', UNISTR('X\00E9t nghi\1EC7m c\00FAm'), SYSDATE - 5, NULL, NULL);
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA107', UNISTR('N\1ED9i soi d\1EA1 d\00E0y'), SYSDATE - 4, NULL, NULL);
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA108', UNISTR('\0110i\1EC7n t\00E2m \0111\1ED3'), SYSDATE - 3, NULL, NULL);
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA109', UNISTR('\0110o \0111i\1EC7n tim'), SYSDATE - 2, NULL, NULL);
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA110', UNISTR('Ch\1EE5p X-Quang'), SYSDATE - 1, NULL, NULL);

-- 4. Create 10 service records with technician NV000121 (for Technician to view/update result)
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA111', UNISTR('Ch\1EE5p X-Quang x\01B0\01A1ng'), SYSDATE - 10, 'NV000121', UNISTR('B\00ECnh th\01B0\1EDDng'));
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA112', UNISTR('N\1ED9i soi tai m\0169i h\1ECDng'), SYSDATE - 9, 'NV000121', UNISTR('Vi\00EAm nh\1EB9'));
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA113', UNISTR('CT Scanner n\00E3o'), SYSDATE - 8, 'NV000121', UNISTR('B\00ECnh th\01B0\1EDDng'));
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA114', UNISTR('X\00E9t nghi\1EC7m m\00E1u'), SYSDATE - 7, 'NV000121', NULL);
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA115', UNISTR('Test d\1ECB \1EE9ng'), SYSDATE - 6, 'NV000121', NULL);
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA116', UNISTR('Si\00EAu \00E2m \1ED5 b\1EE5ng'), SYSDATE - 5, 'NV000121', UNISTR('C\00F3 s\1ECFi nh\1ECF'));
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA117', UNISTR('Ch\1EE5p X-Quang ph\1ED5i'), SYSDATE - 4, 'NV000121', UNISTR('Vi\00EAm ph\1EBF qu\1EA3n'));
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA118', UNISTR('X\00E9t nghi\1EC7m m\00E1u'), SYSDATE - 3, 'NV000121', UNISTR('Thi\1EBFu m\00E1u nh\1EB9'));
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA119', UNISTR('L\01B0u huy\1EBFt n\00E3o \0111\1ED3'), SYSDATE - 2, 'NV000121', UNISTR('Thi\1EC3u n\0103ng tu\1EA7n ho\00E0n'));
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result) VALUES ('BA120', UNISTR('Si\00EAu \00E2m kh\1EDBp g\1ED1i'), SYSDATE - 1, 'NV000121', NULL);

COMMIT;
