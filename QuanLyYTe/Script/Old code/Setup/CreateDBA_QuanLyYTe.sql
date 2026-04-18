-- Sử dụng database XEPDB1
ALTER SESSION SET CONTAINER = PDB_QLYT;

--  Tạo User Admin cho đồ án và cấp các quyền cần thiết để DBA có thể sử dụng database
CREATE USER hospital_dba IDENTIFIED BY 123;

GRANT connect, resource, dba TO hospital_dba;

GRANT
    UNLIMITED TABLESPACE
TO hospital_dba;

-- Cấp quyền để HOSPITAL_DBA có thể quản lý User cho Phân hệ 1
GRANT
    CREATE USER,
    ALTER USER,
    DROP USER
TO hospital_dba;

GRANT
    GRANT ANY ROLE
TO hospital_dba;

-- Delete user
-- Must log in as SYSDBA to execute
-- DROP USER hospital_dba CASCADE;