-- ==============================================================================
-- 01_seed_department.sql
-- Cháº¡y dÆ°á»›i quyá»n: hospital (or sysdba with CURRENT_SCHEMA=hospital)
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

INSERT INTO department VALUES ('PB01', UNISTR('N\1ED9i t\1ED5ng qu\00E1t'));
INSERT INTO department VALUES ('PB02', UNISTR('Ngo\1EA1i th\1EA7n kinh'));
INSERT INTO department VALUES ('PB03', UNISTR('Ch\1EA9n \0111o\00E1n h\00ECnh \1EA3nh'));

COMMIT;
