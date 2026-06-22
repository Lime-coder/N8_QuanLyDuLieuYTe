-- ==============================================================================
-- 03_seed_patient.sql
-- Chạy dưới quyền: hospital (or sysdba with CURRENT_SCHEMA=hospital)
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

INSERT INTO patient (patient_id, full_name, gender, birthdate, id_card, house_no, street, district, city_province, username_db)
VALUES ('BN000001', N'Bệnh nhân Test 1', N'Nam', TO_DATE('1980-01-01','YYYY-MM-DD'), '999080000001', '10', N'Đường số 1', N'Quận 1', N'TP. Hồ Chí Minh', 'BN000001');

INSERT INTO patient (patient_id, full_name, gender, birthdate, id_card, house_no, street, district, city_province, username_db)
SELECT 'BN' || LPAD(LEVEL+1, 6, '0'),
       N'Bệnh nhân ' || (LEVEL+1),
       CASE WHEN MOD(LEVEL, 2) = 0 THEN N'Nữ' ELSE N'Nam' END,
       TO_DATE('1980-01-01','YYYY-MM-DD') + (LEVEL * 365),
       '001080' || LPAD(LEVEL+1, 6, '0'),
       TO_CHAR(LEVEL * 10),
       N'Đường số ' || LEVEL,
       N'Quận ' || MOD(LEVEL, 10),
       N'TP. Hồ Chí Minh',
       'USR_' || DBMS_RANDOM.STRING('X', 6)
FROM DUAL CONNECT BY LEVEL <= 12;

COMMIT;
