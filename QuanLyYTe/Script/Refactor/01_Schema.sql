-- Run as: SYSDBA | Container: PDB_QLYT
ALTER SESSION SET CONTAINER = PDB_QLYT;

-- Schema owner: holds all business tables and views
-- Account is locked — no direct login, hospital_dba manages everything
CREATE USER hospital IDENTIFIED BY Hospital#Schema2026 ACCOUNT LOCK;
GRANT UNLIMITED TABLESPACE TO hospital;

-- DBA role: minimal scoped privileges for the admin dashboard user
CREATE ROLE rl_dba;

-- Grant basic role to rl_dba
GRANT CREATE SESSION              TO rl_dba;
GRANT CREATE ROLE                 TO rl_dba;
GRANT CREATE USER                 TO rl_dba;
GRANT ALTER USER                  TO rl_dba;
GRANT DROP USER                   TO rl_dba;
GRANT GRANT ANY ROLE              TO rl_dba;
GRANT DROP ANY ROLE               TO rl_dba;
GRANT GRANT ANY PRIVILEGE         TO rl_dba;
GRANT GRANT ANY OBJECT PRIVILEGE  TO rl_dba;
GRANT SELECT ANY DICTIONARY       TO rl_dba;
GRANT SELECT ANY TABLE            TO rl_dba;
GRANT EXECUTE ANY PROCEDURE       TO rl_dba;

-- Use for creating table and constraint
GRANT CREATE ANY TABLE            TO rl_dba;
GRANT CREATE ANY VIEW             TO rl_dba;
GRANT CREATE ANY PROCEDURE        TO rl_dba;
GRANT CREATE ANY INDEX            TO rl_dba;
GRANT DROP ANY TABLE              TO rl_dba;
GRANT DROP ANY VIEW               TO rl_dba;
GRANT DROP ANY PROCEDURE          TO rl_dba;
GRANT DROP ANY INDEX              TO rl_dba;

-- Use for inserting mock data
GRANT INSERT ANY TABLE            TO rl_dba;

CREATE USER hospital_dba IDENTIFIED BY 123;
GRANT rl_dba TO hospital_dba;
ALTER USER hospital_dba DEFAULT ROLE rl_dba;

-- Direct grants needed because PL/SQL static SQL ignores role privileges at compile time
GRANT SELECT ON SYS.DBA_USERS         TO hospital_dba;
GRANT SELECT ON SYS.DBA_ROLES         TO hospital_dba;
GRANT SELECT ON SYS.DBA_SYS_PRIVS     TO hospital_dba;
GRANT SELECT ON SYS.DBA_ROLE_PRIVS    TO hospital_dba;
GRANT SELECT ON SYS.DBA_TAB_PRIVS     TO hospital_dba;
GRANT SELECT ON SYS.DBA_COL_PRIVS     TO hospital_dba;
GRANT SELECT ON SYS.DBA_TABLES        TO hospital_dba;
GRANT SELECT ON SYS.DBA_ROLES         TO hospital_dba;
GRANT SELECT ON SYS.DBA_OBJECTS       TO hospital_dba;


-- Only run after executed the table
GRANT SELECT ON hospital.department   TO hospital_dba;
GRANT SELECT ON hospital.staff        TO hospital_dba;
GRANT SELECT ON hospital.patient      TO hospital_dba;
GRANT SELECT ON hospital.medical_record  TO hospital_dba;
GRANT SELECT ON hospital.service_record  TO hospital_dba;
GRANT SELECT ON hospital.prescription TO hospital_dba;

/*
DROP USER hospital CASCADE;
DROP USER hospital_dba CASCADE;
DROP ROLE rl_dba;
*/