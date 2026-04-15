-- ──────────────────────────────────────────────────────────────────
-- 1. SP: GET_ALL_USERS - List all users
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE GET_ALL_USERS (
    p_cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_cursor FOR
        SELECT USERNAME, ACCOUNT_STATUS, ORACLE_MAINTAINED, LOCK_DATE, CREATED
        FROM DBA_USERS
        ORDER BY USERNAME;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20001, 'Failed to fetch users: ' || SQLERRM);
END GET_ALL_USERS;
/

-- ──────────────────────────────────────────────────────────────────
-- 2. SP: GET_ALL_ROLES - List all roles
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE GET_ALL_ROLES (
    p_cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_cursor FOR
        SELECT ROLE, PASSWORD_REQUIRED, AUTHENTICATION_TYPE, COMMON, ORACLE_MAINTAINED
        FROM DBA_ROLES
        ORDER BY ROLE;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20002, 'Failed to fetch roles: ' || SQLERRM);
END GET_ALL_ROLES;
/

-- ──────────────────────────────────────────────────────────────────
-- 3. SP: CREATE_NEW_USER - Create a new user
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE CREATE_NEW_USER (
    p_username IN VARCHAR2,
    p_password IN VARCHAR2
)
IS
    v_sql VARCHAR2(1000);
    v_user VARCHAR2(128);
    v_pwd  VARCHAR2(4000);
BEGIN
    -- Validate input
    IF TRIM(p_username) IS NULL OR TRIM(p_password) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20003, 'Username and password must not be empty.');
    END IF;

    v_user := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_username)));
    v_pwd  := REPLACE(p_password, '"', '""');
    
    -- Create user
    v_sql := 'CREATE USER ' || v_user || ' IDENTIFIED BY "' || v_pwd || '"';
    EXECUTE IMMEDIATE v_sql;

    -- Allow the newly created user to logon
    v_sql := 'GRANT CREATE SESSION TO ' || v_user;
    EXECUTE IMMEDIATE v_sql;

    -- Ensure account is unlocked (depends on profile policy)
    v_sql := 'ALTER USER ' || v_user || ' ACCOUNT UNLOCK';
    EXECUTE IMMEDIATE v_sql;
    
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20004, 'Failed to create user: ' || SQLERRM);
END CREATE_NEW_USER;
/

-- ──────────────────────────────────────────────────────────────────
-- 4. SP: ALTER_USER_PASSWORD - Change user password
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE ALTER_USER_PASSWORD (
    p_username IN VARCHAR2,
    p_new_password IN VARCHAR2
)
IS
    v_sql VARCHAR2(1000);
    v_user VARCHAR2(128);
    v_pwd  VARCHAR2(4000);
BEGIN
    IF TRIM(p_username) IS NULL OR TRIM(p_new_password) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20005, 'Username and password must not be empty.');
    END IF;

    v_user := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_username)));
    v_pwd  := REPLACE(p_new_password, '"', '""');
    
    v_sql := 'ALTER USER ' || v_user || ' IDENTIFIED BY "' || v_pwd || '"';
    EXECUTE IMMEDIATE v_sql;
    
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20006, 'Failed to change password: ' || SQLERRM);
END ALTER_USER_PASSWORD;
/

-- ──────────────────────────────────────────────────────────────────
-- 5. SP: LOCK_USER - Lock a user account
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE LOCK_USER (
    p_username IN VARCHAR2
)
IS
    v_sql VARCHAR2(500);
    v_user VARCHAR2(128);
BEGIN
    IF TRIM(p_username) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20007, 'Username must not be empty.');
    END IF;

    v_user := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_username)));
    
    v_sql := 'ALTER USER ' || v_user || ' ACCOUNT LOCK';
    EXECUTE IMMEDIATE v_sql;
    
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20008, 'Failed to lock user: ' || SQLERRM);
END LOCK_USER;
/

-- ──────────────────────────────────────────────────────────────────
-- 6. SP: UNLOCK_USER - Unlock a user account
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE UNLOCK_USER (
    p_username IN VARCHAR2
)
IS
    v_sql VARCHAR2(500);
    v_user VARCHAR2(128);
BEGIN
    IF TRIM(p_username) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20009, 'Username must not be empty.');
    END IF;

    v_user := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_username)));
    
    v_sql := 'ALTER USER ' || v_user || ' ACCOUNT UNLOCK';
    EXECUTE IMMEDIATE v_sql;
    
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20010, 'Failed to unlock user: ' || SQLERRM);
END UNLOCK_USER;
/

-- ──────────────────────────────────────────────────────────────────
-- 7. SP: DROP_USER - Drop a user
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE DROP_USER (
    p_username IN VARCHAR2,
    p_cascade IN VARCHAR2 DEFAULT 'YES'
)
IS
    v_sql VARCHAR2(500);
    v_user VARCHAR2(128);
BEGIN
    IF TRIM(p_username) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20011, 'Username must not be empty.');
    END IF;

    v_user := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_username)));
    
    IF UPPER(p_cascade) = 'YES' THEN
        v_sql := 'DROP USER ' || v_user || ' CASCADE';
    ELSE
        v_sql := 'DROP USER ' || v_user;
    END IF;
    
    EXECUTE IMMEDIATE v_sql;
    
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20012, 'Failed to drop user: ' || SQLERRM);
END DROP_USER;
/

-- ──────────────────────────────────────────────────────────────────
-- 8. SP: CREATE_NEW_ROLE - Create a new role
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE CREATE_NEW_ROLE (
    p_role_name IN VARCHAR2,
    p_password IN VARCHAR2 DEFAULT NULL
)
IS
    v_sql VARCHAR2(1000);
    v_role VARCHAR2(128);
    v_pwd  VARCHAR2(4000);
BEGIN
    IF TRIM(p_role_name) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20013, 'Role name must not be empty.');
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
        RAISE_APPLICATION_ERROR(-20014, 'Failed to create role: ' || SQLERRM);
END CREATE_NEW_ROLE;
/

-- ──────────────────────────────────────────────────────────────────
-- 9. SP: ALTER_ROLE_PASSWORD - Change role password
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE ALTER_ROLE_PASSWORD (
    p_role_name IN VARCHAR2,
    p_password IN VARCHAR2 DEFAULT NULL
)
IS
    v_sql VARCHAR2(1000);
    v_role VARCHAR2(128);
    v_pwd  VARCHAR2(4000);
BEGIN
    IF TRIM(p_role_name) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20015, 'Role name must not be empty.');
    END IF;

    v_role := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_role_name)));
    
    IF TRIM(p_password) IS NULL THEN
        v_sql := 'ALTER ROLE ' || v_role || ' NOT IDENTIFIED';
    ELSE
        v_pwd := REPLACE(p_password, '"', '""');
        v_sql := 'ALTER ROLE ' || v_role || ' IDENTIFIED BY "' || v_pwd || '"';
    END IF;
    
    EXECUTE IMMEDIATE v_sql;
    
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20016, 'Failed to update role: ' || SQLERRM);
END ALTER_ROLE_PASSWORD;
/

-- ──────────────────────────────────────────────────────────────────
-- 10. SP: DROP_ROLE - Drop a role
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE DROP_ROLE (
    p_role_name IN VARCHAR2
)
IS
    v_sql VARCHAR2(500);
    v_role VARCHAR2(128);
BEGIN
    IF TRIM(p_role_name) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20017, 'Role name must not be empty.');
    END IF;

    v_role := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_role_name)));
    
    v_sql := 'DROP ROLE ' || v_role;
    EXECUTE IMMEDIATE v_sql;
    
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20018, 'Failed to drop role: ' || SQLERRM);
END DROP_ROLE;
/
COMMIT;
