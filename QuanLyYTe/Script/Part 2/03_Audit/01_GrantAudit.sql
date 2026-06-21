-- Run as-- Run as: SYSDBA
-- 1. Kích hoạt Standard Audit (Yêu cầu 3.1) - Cần thực hiện ở mức CDB và restart (nếu chưa bật)
ALTER SESSION SET CONTAINER = CDB$ROOT;
ALTER SYSTEM SET audit_trail = DB, EXTENDED SCOPE = SPFILE;
-- Lưu ý: Lệnh trên cần khởi động lại Database để có hiệu lực hoàn toàn.

-- 2. Chuyển vào PDB để cấp quyền cho User cục bộ
ALTER SESSION SET CONTAINER = PDB_QLYT;

-- Cấp các Role đặc quyền về Audit
GRANT AUDIT_ADMIN TO HOSPITAL_DBA;   -- Quyền quản trị chính sách
GRANT AUDIT_VIEWER TO HOSPITAL_DBA;  -- Quyền xem nhật ký (Bao gồm UNIFIED_AUDIT_TRAIL)

-- SỬA LỖI ORA-00942: Cấp quyền trên view thuộc AUDSYS
GRANT SELECT ON AUDSYS.UNIFIED_AUDIT_TRAIL TO HOSPITAL_DBA;
GRANT SELECT ON SYS.DBA_AUDIT_TRAIL TO HOSPITAL_DBA;
GRANT SELECT ON SYS.DBA_FGA_AUDIT_TRAIL TO HOSPITAL_DBA;

-- Cấp quyền thực thi các gói package bảo mật
GRANT EXECUTE ON DBMS_FGA TO HOSPITAL_DBA;
GRANT EXECUTE ON DBMS_AUDIT_MGMT TO HOSPITAL_DBA;

-- Quyền thực hiện lệnh AUDIT (Standard Audit)
GRANT AUDIT SYSTEM TO HOSPITAL_DBA;