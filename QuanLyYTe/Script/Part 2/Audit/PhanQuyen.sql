-- Run as: SYSDBA
-- Chuyển về vùng root
ALTER SESSION SET CONTAINER = CDB$ROOT;

-- Kích hoạt kiểm toán hệ thống (Yêu cầu 3.1)
ALTER SYSTEM SET audit_trail = DB, EXTENDED SCOPE = SPFILE;