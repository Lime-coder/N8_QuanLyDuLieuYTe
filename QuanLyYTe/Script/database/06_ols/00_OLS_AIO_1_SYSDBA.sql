-- ==============================================================================
-- File: 00_OLS_AIO_1_SYSDBA.sql (Ban đầu là OLS_AIO_1_SYSDBA.sql)
-- Chạy dưới quyền: SYS AS SYSDBA 
-- Mục đích: Kích hoạt và cấu hình Oracle Label Security (OLS) ở mức hệ thống
-- ==============================================================================
SET SERVEROUTPUT ON;

PROMPT ==============================================================================
PROMPT 1. Switch to CDB$ROOT to unlock global LBACSYS account
PROMPT ==============================================================================
ALTER SESSION SET CONTAINER = CDB$ROOT;
ALTER USER lbacsys IDENTIFIED BY Lbacsys#2026 ACCOUNT UNLOCK CONTAINER=ALL;

PROMPT ==============================================================================
PROMPT 2. Switch to PDB_QLYT to configure OLS for the project
PROMPT ==============================================================================
ALTER SESSION SET CONTAINER = PDB_QLYT;

-- Cấp các quyền quản trị OLS cho tài khoản quản trị hệ thống (hospital_dba)
GRANT LBAC_DBA TO hospital_dba;
-- Cho phép hospital_dba thực thi các package quản trị của OLS:
GRANT EXECUTE ON sa_sysdba TO hospital_dba;       -- Quản trị cấu hình OLS hệ thống
GRANT EXECUTE ON sa_components TO hospital_dba;   -- Quản trị các thành phần (Level, Compartment, Group)
GRANT EXECUTE ON sa_label_admin TO hospital_dba;  -- Quản trị các nhãn (Labels)
GRANT EXECUTE ON sa_policy_admin TO hospital_dba; -- Quản trị các chính sách (Policies)
GRANT EXECUTE ON sa_user_admin TO hospital_dba;   -- Gán nhãn cho người dùng (Users)

-- Cho phép hospital_dba xem thông tin đặc quyền OLS của user qua Data Dictionary view
GRANT SELECT ON DBA_SA_USERS TO hospital_dba;

PROMPT ==============================================================================
PROMPT 3. Enable OLS Kernel and Restart PDB
PROMPT ==============================================================================
-- Kích hoạt cấu hình OLS
EXEC LBACSYS.CONFIGURE_OLS;
-- Bật trình thực thi (Enforcement) của OLS trên database
EXEC LBACSYS.OLS_ENFORCEMENT.ENABLE_OLS;

-- Khởi động lại PDB để các thay đổi về kernel OLS có hiệu lực
ALTER PLUGGABLE DATABASE CLOSE IMMEDIATE;
ALTER PLUGGABLE DATABASE OPEN;

PROMPT ==============================================================================
PROMPT DONE: SYSDBA setup complete. 
PROMPT Disconnect and connect as HOSPITAL_DBA for the next script.
PROMPT ==============================================================================