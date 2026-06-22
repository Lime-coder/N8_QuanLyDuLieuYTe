-- ==============================================================================
-- 01_seed_department.sql
-- Run as: hospital (or sysdba with CURRENT_SCHEMA=hospital)
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

INSERT INTO department VALUES ('PB01', N'Nội tổng quát');
INSERT INTO department VALUES ('PB02', N'Ngoại thần kinh');
INSERT INTO department VALUES ('PB03', N'Chẩn đoán hình ảnh');

COMMIT;
