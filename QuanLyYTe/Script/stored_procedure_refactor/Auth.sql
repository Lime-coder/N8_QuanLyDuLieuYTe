-- Run as: hospital_dba | Container: PDB_QLYT
ALTER SESSION SET CONTAINER = PDB_QLYT_Nhap;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba_nhap; 

-- Returns the granted role for a user (assumes one active role per user)
CREATE OR REPLACE PROCEDURE USP_GET_GRANTED_ROLE (
    p_user   IN  VARCHAR2,
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT GRANTED_ROLE FROM (
            SELECT GRANTED_ROLE FROM SYS.DBA_ROLE_PRIVS
            WHERE GRANTEE = UPPER(p_user)
            ORDER BY GRANTED_ROLE
        ) WHERE ROWNUM = 1;
END USP_GET_GRANTED_ROLE;
/

-- Returns the active role for the current database session
CREATE OR REPLACE PROCEDURE USP_GET_SESSION_ROLE (
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT ROLE FROM SESSION_ROLES WHERE ROWNUM = 1;
END USP_GET_SESSION_ROLE;
/
