-- Run as: hospital_dba | Container: PDB_QLYT
ALTER SESSION SET CONTAINER = PDB_QLYT;

-- Returns the granted role for a user (assumes one active role per user)
CREATE OR REPLACE PROCEDURE USP_GET_GRANTED_ROLE (
    p_user   IN  VARCHAR2,
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT GRANTED_ROLE FROM (
            SELECT GRANTED_ROLE FROM DBA_ROLE_PRIVS
            WHERE GRANTEE = UPPER(p_user)
            ORDER BY 
                CASE WHEN GRANTED_ROLE = 'RL_DBA' THEN 1 ELSE 2 END,
                GRANTED_ROLE
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
