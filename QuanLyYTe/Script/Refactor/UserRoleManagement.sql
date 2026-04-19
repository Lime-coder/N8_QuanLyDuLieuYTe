-- Run as: hospital_dba | Container: PDB_QLYT
-- ALTER SESSION SET CONTAINER = PDB_QLYT;

-- Returns all database users including Oracle-maintained ones
-- The UI layer is responsible for restricting modification of ORACLE_MAINTAINED = 'Y' accounts
CREATE OR REPLACE PROCEDURE USP_GET_ALL_USERS (
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT USERNAME, ACCOUNT_STATUS, ORACLE_MAINTAINED, LOCK_DATE, CREATED
        FROM DBA_USERS
        ORDER BY USERNAME;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20001, 'USP_GET_ALL_USERS: ' || SQLERRM);
END USP_GET_ALL_USERS;
/

-- Returns all database roles
CREATE OR REPLACE PROCEDURE USP_GET_ALL_ROLES (
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT ROLE, PASSWORD_REQUIRED, AUTHENTICATION_TYPE, COMMON, ORACLE_MAINTAINED
        FROM DBA_ROLES
        ORDER BY ROLE;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20002, 'USP_GET_ALL_ROLES: ' || SQLERRM);
END USP_GET_ALL_ROLES;
/

-- Creates a new user and grants CREATE SESSION by default
CREATE OR REPLACE PROCEDURE USP_CREATE_USER (
    p_username IN VARCHAR2,
    p_password IN VARCHAR2
) AUTHID CURRENT_USER AS
    v_user VARCHAR2(128);
    v_pwd  VARCHAR2(4000);
BEGIN
    IF TRIM(p_username) IS NULL OR TRIM(p_password) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20003, 'Username and password must not be empty.');
    END IF;
    v_user := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_username)));
    v_pwd  := REPLACE(p_password, '"', '""');
    EXECUTE IMMEDIATE 'CREATE USER ' || v_user || ' IDENTIFIED BY "' || v_pwd || '"';
    EXECUTE IMMEDIATE 'GRANT CREATE SESSION TO ' || v_user;
    EXECUTE IMMEDIATE 'ALTER USER ' || v_user || ' ACCOUNT UNLOCK';
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20004, 'USP_CREATE_USER: ' || SQLERRM);
END USP_CREATE_USER;
/

-- Changes a user's password; blocked for Oracle-maintained accounts
CREATE OR REPLACE PROCEDURE USP_UPDATE_USER_PASSWORD (
    p_username     IN VARCHAR2,
    p_new_password IN VARCHAR2
) AUTHID CURRENT_USER AS
    v_user       VARCHAR2(128);
    v_pwd        VARCHAR2(4000);
    v_maintained VARCHAR2(1);
BEGIN
    IF TRIM(p_username) IS NULL OR TRIM(p_new_password) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20005, 'Username and password must not be empty.');
    END IF;
    v_user := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_username)));
    SELECT ORACLE_MAINTAINED INTO v_maintained FROM DBA_USERS WHERE USERNAME = v_user;
    IF v_maintained = 'Y' THEN
        RAISE_APPLICATION_ERROR(-20006, 'Cannot modify Oracle-maintained user: ' || v_user);
    END IF;
    v_pwd := REPLACE(p_new_password, '"', '""');
    EXECUTE IMMEDIATE 'ALTER USER ' || v_user || ' IDENTIFIED BY "' || v_pwd || '"';
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20006, 'User not found: ' || p_username);
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20007, 'USP_UPDATE_USER_PASSWORD: ' || SQLERRM);
END USP_UPDATE_USER_PASSWORD;
/

-- Locks a user account; blocked for Oracle-maintained accounts
CREATE OR REPLACE PROCEDURE USP_LOCK_USER (
    p_username IN VARCHAR2
) AUTHID CURRENT_USER AS
    v_user       VARCHAR2(128);
    v_maintained VARCHAR2(1);
BEGIN
    IF TRIM(p_username) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20008, 'Username must not be empty.');
    END IF;
    v_user := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_username)));
    SELECT ORACLE_MAINTAINED INTO v_maintained FROM DBA_USERS WHERE USERNAME = v_user;
    IF v_maintained = 'Y' THEN
        RAISE_APPLICATION_ERROR(-20009, 'Cannot lock Oracle-maintained user: ' || v_user);
    END IF;
    EXECUTE IMMEDIATE 'ALTER USER ' || v_user || ' ACCOUNT LOCK';
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20009, 'User not found: ' || p_username);
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20010, 'USP_LOCK_USER: ' || SQLERRM);
END USP_LOCK_USER;
/

-- Unlocks a user account; blocked for Oracle-maintained accounts
CREATE OR REPLACE PROCEDURE USP_UNLOCK_USER (
    p_username IN VARCHAR2
) AUTHID CURRENT_USER AS
    v_user       VARCHAR2(128);
    v_maintained VARCHAR2(1);
BEGIN
    IF TRIM(p_username) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20011, 'Username must not be empty.');
    END IF;
    v_user := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_username)));
    SELECT ORACLE_MAINTAINED INTO v_maintained FROM DBA_USERS WHERE USERNAME = v_user;
    IF v_maintained = 'Y' THEN
        RAISE_APPLICATION_ERROR(-20012, 'Cannot unlock Oracle-maintained user: ' || v_user);
    END IF;
    EXECUTE IMMEDIATE 'ALTER USER ' || v_user || ' ACCOUNT UNLOCK';
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20012, 'User not found: ' || p_username);
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20013, 'USP_UNLOCK_USER: ' || SQLERRM);
END USP_UNLOCK_USER;
/

-- Drops a user (CASCADE by default); blocked for Oracle-maintained accounts
CREATE OR REPLACE PROCEDURE USP_DROP_USER (
    p_username IN VARCHAR2,
    p_cascade  IN VARCHAR2 DEFAULT 'YES'
) AUTHID CURRENT_USER AS
    v_user       VARCHAR2(128);
    v_maintained VARCHAR2(1);
BEGIN
    IF TRIM(p_username) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20014, 'Username must not be empty.');
    END IF;
    v_user := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_username)));
    SELECT ORACLE_MAINTAINED INTO v_maintained FROM DBA_USERS WHERE USERNAME = v_user;
    IF v_maintained = 'Y' THEN
        RAISE_APPLICATION_ERROR(-20015, 'Cannot drop Oracle-maintained user: ' || v_user);
    END IF;
    IF UPPER(p_cascade) = 'YES' THEN
        EXECUTE IMMEDIATE 'DROP USER ' || v_user || ' CASCADE';
    ELSE
        EXECUTE IMMEDIATE 'DROP USER ' || v_user;
    END IF;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20015, 'User not found: ' || p_username);
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20016, 'USP_DROP_USER: ' || SQLERRM);
END USP_DROP_USER;
/

-- Creates a new role, optionally password-protected
CREATE OR REPLACE PROCEDURE USP_CREATE_ROLE (
    p_role_name IN VARCHAR2,
    p_password  IN VARCHAR2 DEFAULT NULL
) AUTHID CURRENT_USER AS
    v_sql  VARCHAR2(1000);
    v_role VARCHAR2(128);
    v_pwd  VARCHAR2(4000);
BEGIN
    IF TRIM(p_role_name) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20017, 'Role name must not be empty.');
    END IF;
    v_role := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_role_name)));
    IF TRIM(p_password) IS NULL THEN
        v_sql := 'CREATE ROLE ' || v_role;
    ELSE
        v_pwd := REPLACE(p_password, '"', '""');
        v_sql := 'CREATE ROLE ' || v_role || ' IDENTIFIED BY "' || v_pwd || '"';
    END IF;
    EXECUTE IMMEDIATE v_sql;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20018, 'USP_CREATE_ROLE: ' || SQLERRM);
END USP_CREATE_ROLE;
/

-- Updates a role's password or removes it; blocked for Oracle-maintained roles
CREATE OR REPLACE PROCEDURE USP_UPDATE_ROLE_PASSWORD (
    p_role_name IN VARCHAR2,
    p_password  IN VARCHAR2 DEFAULT NULL
) AUTHID CURRENT_USER AS
    v_sql        VARCHAR2(1000);
    v_role       VARCHAR2(128);
    v_pwd        VARCHAR2(4000);
    v_maintained VARCHAR2(1);
BEGIN
    IF TRIM(p_role_name) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20019, 'Role name must not be empty.');
    END IF;
    v_role := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_role_name)));
    SELECT ORACLE_MAINTAINED INTO v_maintained FROM DBA_ROLES WHERE ROLE = v_role;
    IF v_maintained = 'Y' THEN
        RAISE_APPLICATION_ERROR(-20020, 'Cannot modify Oracle-maintained role: ' || v_role);
    END IF;
    IF TRIM(p_password) IS NULL THEN
        v_sql := 'ALTER ROLE ' || v_role || ' NOT IDENTIFIED';
    ELSE
        v_pwd := REPLACE(p_password, '"', '""');
        v_sql := 'ALTER ROLE ' || v_role || ' IDENTIFIED BY "' || v_pwd || '"';
    END IF;
    EXECUTE IMMEDIATE v_sql;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20020, 'Role not found: ' || p_role_name);
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20021, 'USP_UPDATE_ROLE_PASSWORD: ' || SQLERRM);
END USP_UPDATE_ROLE_PASSWORD;
/

-- Drops a role; blocked for Oracle-maintained roles
CREATE OR REPLACE PROCEDURE USP_DROP_ROLE (
    p_role_name IN VARCHAR2
) AUTHID CURRENT_USER AS
    v_role       VARCHAR2(128);
    v_maintained VARCHAR2(1);
BEGIN
    IF TRIM(p_role_name) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20022, 'Role name must not be empty.');
    END IF;
    v_role := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_role_name)));
    SELECT ORACLE_MAINTAINED INTO v_maintained FROM DBA_ROLES WHERE ROLE = v_role;
    IF v_maintained = 'Y' THEN
        RAISE_APPLICATION_ERROR(-20023, 'Cannot drop Oracle-maintained role: ' || v_role);
    END IF;
    EXECUTE IMMEDIATE 'DROP ROLE ' || v_role;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20023, 'Role not found: ' || p_role_name);
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20024, 'USP_DROP_ROLE: ' || SQLERRM);
END USP_DROP_ROLE;
/

-- Get grantees
CREATE OR REPLACE PROCEDURE USP_GET_GRANTEES (
    p_grantee_type IN VARCHAR2, -- 'USER' or 'ROLE'
    p_result_cursor OUT SYS_REFCURSOR
) AS
    v_type VARCHAR2(10) := UPPER(p_grantee_type);
BEGIN
    IF v_type = 'USER' THEN
        OPEN p_result_cursor FOR 
        SELECT UPPER(username) FROM dba_users 
        WHERE oracle_maintained = 'N' AND username != 'HOSPITAL_DBA'
        ORDER BY username;
    ELSE
        OPEN p_result_cursor FOR 
        SELECT UPPER(role) FROM dba_roles 
        WHERE oracle_maintained = 'N' 
        ORDER BY role;
    END IF;
END;
/

