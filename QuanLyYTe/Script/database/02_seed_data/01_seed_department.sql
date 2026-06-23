-- ==============================================================================
-- 01_seed_department.sql
-- Chạy dưới quyền: hospital (or sysdba with CURRENT_SCHEMA=hospital)
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

INSERT INTO department VALUES ('PB01', UNISTR('Khoa tim m\1EA1ch'));
INSERT INTO department VALUES ('PB02', UNISTR('Khoa th\1EA7n kinh'));
INSERT INTO department VALUES ('PB03', UNISTR('Khoa ti\00EAu h\00F3a'));

COMMIT;
