-- ==============================================================================
-- 00_init_schemas.sql (Bổ sung từ Part 1)
-- Chạy dưới quyền: SYS AS SYSDBA
-- Khởi tạo các user nền tảng (hospital_dba, hospital_dba) và role quản trị cơ bản
-- ==============================================================================
ALTER SESSION SET CONTAINER = PDB_QLYT;



-- 2. Tạo role quản trị (rl_dba) với các quyền tối thiểu để quản lý schema hospital_dba
CREATE ROLE rl_dba;
GRANT CREATE SESSION TO rl_dba;
GRANT CREATE ROLE TO rl_dba;
GRANT CREATE USER TO rl_dba;
GRANT ALTER USER TO rl_dba;
GRANT DROP USER TO rl_dba;
GRANT GRANT ANY ROLE TO rl_dba;
GRANT DROP ANY ROLE TO rl_dba;
GRANT GRANT ANY PRIVILEGE TO rl_dba;
GRANT GRANT ANY OBJECT PRIVILEGE TO rl_dba;

GRANT SELECT ANY DICTIONARY TO rl_dba;
GRANT SELECT ANY TABLE TO rl_dba;
GRANT EXECUTE ANY PROCEDURE TO rl_dba;

GRANT CREATE ANY TABLE TO rl_dba;
GRANT ALTER ANY TABLE TO rl_dba;
GRANT CREATE ANY VIEW TO rl_dba;
GRANT CREATE ANY PROCEDURE TO rl_dba;
GRANT CREATE ANY INDEX TO rl_dba;
GRANT CREATE ANY SEQUENCE TO rl_dba;
GRANT CREATE ANY TRIGGER TO rl_dba;
GRANT DROP ANY TABLE TO rl_dba;
GRANT DROP ANY VIEW TO rl_dba;
GRANT DROP ANY PROCEDURE TO rl_dba;
GRANT DROP ANY INDEX TO rl_dba;

GRANT INSERT ANY TABLE TO rl_dba;
GRANT UPDATE ANY TABLE TO rl_dba;
GRANT DELETE ANY TABLE TO rl_dba;

-- 3. Tạo tài khoản quản trị viện (hospital_dba)
CREATE USER hospital_dba IDENTIFIED BY "&&dba_password";
GRANT rl_dba TO hospital_dba;
ALTER USER hospital_dba DEFAULT ROLE rl_dba;
GRANT UNLIMITED TABLESPACE TO hospital_dba;

-- Các quyền cấp hệ thống bắt buộc cho hospital_dba để thực thi logic phân quyền và OLS/VPD sau này
GRANT SELECT ON SYS.DBA_USERS TO hospital_dba;
GRANT SELECT ON SYS.DBA_ROLES TO hospital_dba;
GRANT SELECT ON SYS.DBA_SYS_PRIVS TO hospital_dba;
GRANT SELECT ON SYS.DBA_ROLE_PRIVS TO hospital_dba;
GRANT SELECT ON SYS.DBA_TAB_PRIVS TO hospital_dba;
GRANT SELECT ON SYS.DBA_COL_PRIVS TO hospital_dba;
GRANT SELECT ON SYS.DBA_TABLES TO hospital_dba;
GRANT SELECT ON SYS.DBA_OBJECTS TO hospital_dba;
GRANT SELECT ON SYS.DBA_TAB_COLUMNS TO hospital_dba;
GRANT EXECUTE ON DBMS_RLS TO hospital_dba;

-- Quyền cho Audit và OLS
GRANT SELECT ON SYS.DBA_AUDIT_TRAIL TO hospital_dba;
GRANT SELECT ON SYS.DBA_FGA_AUDIT_TRAIL TO hospital_dba;
GRANT SELECT ON UNIFIED_AUDIT_TRAIL TO hospital_dba;
GRANT SELECT ON DBA_SA_USERS TO hospital_dba;
GRANT EXECUTE ON SA_USER_ADMIN TO hospital_dba;

-- 4. Tạo các sequence cấp tự động cho ID (do script 01_create_tables thiếu)
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;
CREATE SEQUENCE SEQ_STAFF_ID START WITH 100001 INCREMENT BY 1;
CREATE SEQUENCE SEQ_PATIENT_ID START WITH 100001 INCREMENT BY 1;

