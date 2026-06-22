-- ==============================================================================
-- 00_init_schemas.sql (Bá»• sung tá»« Part 1)
-- Cháº¡y dÆ°á»›i quyá»n: SYS AS SYSDBA
-- Khá»Ÿi táº¡o cÃ¡c user ná»n táº£ng (hospital, hospital_dba) vÃ  role quáº£n trá»‹ cÆ¡ báº£n
-- ==============================================================================
ALTER SESSION SET CONTAINER = PDB_QLYT;

-- 1. Táº¡o Schema chÃ­nh cá»§a há»‡ thá»‘ng (bá»‹ khÃ³a, khÃ´ng cho phÃ©p login trá»±c tiáº¿p)
CREATE USER hospital IDENTIFIED BY Hospital#Schema2026 ACCOUNT LOCK;
GRANT UNLIMITED TABLESPACE TO hospital;

-- 2. Táº¡o role quáº£n trá»‹ (rl_dba) vá»›i cÃ¡c quyá»n tá»‘i thiá»ƒu Ä‘á»ƒ quáº£n lÃ½ schema hospital
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

-- 3. Táº¡o tÃ i khoáº£n quáº£n trá»‹ viá»‡n (hospital_dba)
CREATE USER hospital_dba IDENTIFIED BY "&&dba_password";
GRANT rl_dba TO hospital_dba;
ALTER USER hospital_dba DEFAULT ROLE rl_dba;

-- CÃ¡c quyá»n cáº¥p há»‡ thá»‘ng báº¯t buá»™c cho hospital_dba Ä‘á»ƒ thá»±c thi logic phÃ¢n quyá»n vÃ  OLS/VPD sau nÃ y
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

-- QuyÃªÌ€n cho Audit vÃ  OLS
GRANT SELECT ON SYS.DBA_AUDIT_TRAIL TO hospital_dba;
GRANT SELECT ON SYS.DBA_FGA_AUDIT_TRAIL TO hospital_dba;
GRANT SELECT ON UNIFIED_AUDIT_TRAIL TO hospital_dba;
GRANT SELECT ON DBA_SA_USERS TO hospital_dba;
GRANT EXECUTE ON SA_USER_ADMIN TO hospital_dba;

-- 4. Táº¡o cÃ¡c sequence cáº¥p tá»± Ä‘á»™ng cho ID (do script 01_create_tables thiáº¿u)
ALTER SESSION SET CURRENT_SCHEMA = hospital;
CREATE SEQUENCE SEQ_STAFF_ID START WITH 100001 INCREMENT BY 1;
CREATE SEQUENCE SEQ_PATIENT_ID START WITH 100001 INCREMENT BY 1;
