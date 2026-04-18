-- Run as: SYSDBA | Container: PDB_QLYT
ALTER SESSION SET CONTAINER = PDB_QLYT;

-- Schema owner: holds all business tables and views
-- Account is locked — no direct login, hospital_dba manages everything
CREATE USER hospital IDENTIFIED BY Hospital#Schema2024 ACCOUNT LOCK;
GRANT UNLIMITED TABLESPACE TO hospital;

-- DBA role: minimal scoped privileges for the admin dashboard user
CREATE ROLE rl_dba;

GRANT CREATE SESSION              TO rl_dba;
GRANT CREATE ROLE                 TO rl_dba;
GRANT CREATE USER                 TO rl_dba;
GRANT ALTER USER                  TO rl_dba;
GRANT DROP USER                   TO rl_dba;
GRANT GRANT ANY ROLE              TO rl_dba;
GRANT GRANT ANY PRIVILEGE         TO rl_dba;
GRANT GRANT ANY OBJECT PRIVILEGE  TO rl_dba;
GRANT SELECT ANY DICTIONARY       TO rl_dba;
GRANT SELECT ANY TABLE            TO rl_dba;
GRANT EXECUTE ANY PROCEDURE       TO rl_dba;
GRANT CREATE ANY TABLE            TO rl_dba;
GRANT CREATE ANY VIEW             TO rl_dba;
GRANT CREATE ANY PROCEDURE        TO rl_dba;
--GRANT UNLIMITED TABLESPACE        TO rl_dba; ---- Error: Can't grant tablespace to a role

-- Application DBA user
CREATE USER hospital_dba IDENTIFIED BY HospitalDBA#2024;
GRANT rl_dba TO hospital_dba;
ALTER USER hospital_dba DEFAULT ROLE rl_dba;

GRANT UNLIMITED TABLESPACE        TO hospital_dba;

GRANT CREATE ANY TABLE            TO hospital_dba;
GRANT DROP ANY TABLE              TO hospital_dba;

GRANT CREATE ANY INDEX TO hospital_dba;
GRANT ALTER ANY TABLE TO hospital_dba;
GRANT INSERT ANY TABLE, UPDATE ANY TABLE, DELETE ANY TABLE TO hospital_dba;
GRANT SELECT ANY TABLE TO hospital_dba;
GRANT SELECT ON DBA_ROLE_PRIVS TO hospital_dba;
GRANT SELECT ON SESSION_ROLES TO hospital_dba;

GRANT SELECT ON DBA_USERS TO hospital_dba;
GRANT SELECT ON DBA_ROLES TO hospital_dba;
GRANT SELECT ON DBA_SYS_PRIVS TO hospital_dba;
GRANT SELECT ON DBA_ROLE_PRIVS TO hospital_dba;
GRANT SELECT ON SYSTEM_PRIVILEGE_MAP TO hospital_dba;

GRANT CREATE ANY PROCEDURE TO hospital_dba;
GRANT CREATE ANY VIEW TO hospital_dba;
