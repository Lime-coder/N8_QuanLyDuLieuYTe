-- Run as: hospital_dba | Container: PDB_QLYT
-- ALTER SESSION SET CONTAINER = PDB_QLYT;

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
        SELECT ROLE FROM (
            SELECT ROLE FROM SESSION_ROLES
            WHERE ROLE LIKE 'RL\_%' ESCAPE '\'
            ORDER BY CASE WHEN ROLE = 'RL_DBA' THEN 1 ELSE 2 END, ROLE
        ) WHERE ROWNUM = 1;
END USP_GET_SESSION_ROLE;
/

-- Returns the user ID based on their username and role
CREATE OR REPLACE PROCEDURE USP_GET_USER_ID (
    p_username IN VARCHAR2,
    p_role     IN VARCHAR2,
    p_cursor   OUT SYS_REFCURSOR
) AUTHID DEFINER AS
BEGIN
    IF UPPER(p_role) = 'RL_PATIENT' THEN
        OPEN p_cursor FOR
            SELECT patient_id AS ID FROM hospital.patient WHERE username_db = UPPER(TRIM(p_username));
    ELSE
        OPEN p_cursor FOR
            SELECT staff_id AS ID FROM hospital.staff WHERE username_db = UPPER(TRIM(p_username));
    END IF;
END USP_GET_USER_ID;
/
