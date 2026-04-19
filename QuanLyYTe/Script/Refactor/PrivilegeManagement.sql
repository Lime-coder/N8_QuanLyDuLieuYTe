-- Run as: hospital_dba | Container: PDB_QLYT
ALTER SESSION SET CONTAINER = PDB_QLYT;

-- USP_GET_USERS: project-scoped user list for the revoke form
-- Returns hospital_dba + all staff/patient username_db values
CREATE OR REPLACE PROCEDURE USP_GET_USERS (
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT USERNAME 
        FROM DBA_USERS 
        WHERE ORACLE_MAINTAINED = 'N' 
        ORDER BY USERNAME;
END USP_GET_USERS;
/

-- USP_GET_ROLES: returns only project-defined roles (RL_ prefix)
CREATE OR REPLACE PROCEDURE USP_GET_ROLES (
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT ROLE AS rolename
        FROM DBA_ROLES
        WHERE ROLE LIKE 'RL\_%' ESCAPE '\'
        ORDER BY ROLE;
END USP_GET_ROLES;
/

-- USP_GET_ALL_PRIVS: all privileges granted to a specific user or role
CREATE OR REPLACE PROCEDURE USP_GET_ALL_PRIVS (
    p_grantee IN  VARCHAR2,
    p_cursor  OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT * FROM (
            -- 1. SYSTEM PRIVILEGES
            SELECT 'SYSTEM' AS LOAI_QUYEN, sp.PRIVILEGE AS QUYEN,
                   NULL AS CHU_SO_HUU, NULL AS DOI_TUONG, NULL AS COT,
                   sp.ADMIN_OPTION AS CO_THE_CAP_LAI
            FROM DBA_SYS_PRIVS sp
            WHERE UPPER(sp.GRANTEE) = UPPER(p_grantee)

            UNION ALL

            -- 2. ROLE PRIVILEGES
            SELECT 'ROLE', rp.GRANTED_ROLE, NULL, NULL, NULL, rp.ADMIN_OPTION
            FROM DBA_ROLE_PRIVS rp
            WHERE UPPER(rp.GRANTEE) = UPPER(p_grantee)

            UNION ALL

            -- 3. OBJECT PRIVILEGES
            SELECT 
                CASE ao.OBJECT_TYPE 
                    WHEN 'TABLE' THEN 'TABLE' WHEN 'VIEW' THEN 'VIEW'
                    WHEN 'PROCEDURE' THEN 'PROCEDURE' WHEN 'FUNCTION' THEN 'FUNCTION'
                    ELSE ao.OBJECT_TYPE 
                END,
                tp.PRIVILEGE, tp.OWNER, tp.TABLE_NAME, NULL, tp.GRANTABLE
            FROM DBA_TAB_PRIVS tp
            JOIN DBA_OBJECTS ao ON ao.OWNER = tp.OWNER AND ao.OBJECT_NAME = tp.TABLE_NAME
            WHERE UPPER(tp.GRANTEE) = UPPER(p_grantee)
              AND NOT EXISTS (
                  SELECT 1 FROM DBA_COL_PRIVS cp
                  WHERE UPPER(cp.GRANTEE) = UPPER(p_grantee)
                    AND cp.OWNER = tp.OWNER
                    AND cp.TABLE_NAME = tp.TABLE_NAME
                    AND cp.PRIVILEGE = tp.PRIVILEGE
              )

            UNION ALL

            -- 4. COLUMN PRIVILEGES (Updated to output "TABLE (COLUMN)")
            SELECT 'COLUMN', cp.PRIVILEGE, cp.OWNER, 
                   cp.TABLE_NAME || ' (' || cp.COLUMN_NAME || ')', NULL, 
                   cp.GRANTABLE
            FROM DBA_COL_PRIVS cp
            WHERE UPPER(cp.GRANTEE) = UPPER(p_grantee)
        )
        ORDER BY LOAI_QUYEN, CHU_SO_HUU, DOI_TUONG, COT, QUYEN;
END USP_GET_ALL_PRIVS;
/   

-- USP_GET_PRIVS_ON_OBJ: all grantees and their privileges on a specific object
CREATE OR REPLACE PROCEDURE USP_GET_PRIVS_ON_OBJ (
    p_owner       IN  VARCHAR2,
    p_object_name IN  VARCHAR2,
    p_cursor      OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        -- 1. TABLE LEVEL PRIVILEGES
        SELECT 'TABLE' AS LOAI_QUYEN, tp.PRIVILEGE AS QUYEN, 
               tp.OWNER AS CHU_SO_HUU, tp.TABLE_NAME AS DOI_TUONG,
               tp.GRANTEE AS NGUOI_NHAN, 
               tp.GRANTABLE AS CO_THE_CAP_LAI -- Added
        FROM DBA_TAB_PRIVS tp
        WHERE UPPER(tp.OWNER) = UPPER(p_owner)
          AND UPPER(tp.TABLE_NAME) = UPPER(p_object_name)
          AND NOT EXISTS (
              SELECT 1 FROM DBA_COL_PRIVS cp
              WHERE UPPER(cp.GRANTEE) = UPPER(tp.GRANTEE)
                AND cp.OWNER = tp.OWNER
                AND cp.TABLE_NAME = tp.TABLE_NAME
                AND cp.PRIVILEGE = tp.PRIVILEGE
          )

        UNION ALL

        -- 2. NATIVE COLUMN LEVEL PRIVILEGES
        SELECT 'COLUMN' AS LOAI_QUYEN, cp.PRIVILEGE AS QUYEN, 
               cp.OWNER AS CHU_SO_HUU, 
               cp.TABLE_NAME || ' (' || cp.COLUMN_NAME || ')' AS DOI_TUONG,
               cp.GRANTEE AS NGUOI_NHAN, 
               cp.GRANTABLE AS CO_THE_CAP_LAI -- Added
        FROM DBA_COL_PRIVS cp
        WHERE UPPER(cp.OWNER) = UPPER(p_owner)
          AND UPPER(cp.TABLE_NAME) = UPPER(p_object_name)

        UNION ALL

        -- 3. DYNAMIC COLUMN-SELECT VIEWS
        SELECT 'COLUMN' AS LOAI_QUYEN, tp.PRIVILEGE AS QUYEN, 
               tp.OWNER AS CHU_SO_HUU, tp.TABLE_NAME AS DOI_TUONG,
               tp.GRANTEE AS NGUOI_NHAN, 
               tp.GRANTABLE AS CO_THE_CAP_LAI -- Added
        FROM DBA_TAB_PRIVS tp
        WHERE UPPER(tp.OWNER) = UPPER(p_owner)
          -- Dynamically fetch views starting with V_PRIV_ + Object Name
          AND UPPER(tp.TABLE_NAME) LIKE 'V\_PRIV\_' || UPPER(p_object_name) || '\_%' ESCAPE '\'

        ORDER BY LOAI_QUYEN, NGUOI_NHAN, QUYEN;
END USP_GET_PRIVS_ON_OBJ;
/

-- USP_GET_OBJECTS: objects of a given type owned by the hospital schema
CREATE OR REPLACE PROCEDURE USP_GET_OBJECTS (
    p_object_type   IN  VARCHAR2,
    p_result_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_result_cursor FOR
        SELECT UPPER(object_name)
        FROM all_objects
        WHERE object_type = UPPER(p_object_type)
          AND owner = 'HOSPITAL'
        ORDER BY object_name;
END USP_GET_OBJECTS;
/

-- USP_GET_COLUMNS: column names for a given table in the hospital schema
CREATE OR REPLACE PROCEDURE USP_GET_COLUMNS (
    p_table_name    IN  VARCHAR2,
    p_result_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_result_cursor FOR
        SELECT UPPER(column_name)
        FROM all_tab_columns
        WHERE table_name = UPPER(p_table_name)
          AND owner = 'HOSPITAL'
        ORDER BY column_id;
END USP_GET_COLUMNS;
/

-- USP_GET_SYSTEM_PRIVILEGES: all Oracle system privilege names for the grant form dropdown
CREATE OR REPLACE PROCEDURE USP_GET_SYSTEM_PRIVILEGES (
    p_result_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_result_cursor FOR
        SELECT NAME FROM SYSTEM_PRIVILEGE_MAP ORDER BY NAME;
END USP_GET_SYSTEM_PRIVILEGES;
/

-- USP_REVOKE_PRIV: revoke a privilege from a user or role
-- p_type: SYSTEM | ROLE | TABLE | VIEW | PROCEDURE | FUNCTION | COLUMN
-- Note: COLUMN type raises an informative error — Oracle does not support column-level REVOKE.
--       Instruct the user to revoke the table-level privilege instead.
CREATE OR REPLACE PROCEDURE USP_REVOKE_PRIV (
    p_type      IN VARCHAR2,
    p_privilege IN VARCHAR2,
    p_owner     IN VARCHAR2,
    p_object    IN VARCHAR2,
    p_column    IN VARCHAR2,
    p_grantee   IN VARCHAR2
) AUTHID CURRENT_USER AS
    v_sql       VARCHAR2(1000);
    v_type      VARCHAR2(30)  := UPPER(TRIM(p_type));
    v_privilege VARCHAR2(200) := UPPER(TRIM(p_privilege));
    v_owner     VARCHAR2(128) := UPPER(TRIM(p_owner));
    v_object    VARCHAR2(128) := UPPER(TRIM(p_object));
    v_grantee   VARCHAR2(128) := UPPER(TRIM(p_grantee));
    v_count     NUMBER;
BEGIN
    IF v_privilege IS NULL OR v_grantee IS NULL THEN
        RAISE_APPLICATION_ERROR(-20001, 'p_privilege and p_grantee cannot be empty.');
    END IF;

    -- Block revoke from Oracle-maintained accounts
    SELECT COUNT(*) INTO v_count FROM (
        SELECT 1 FROM DBA_USERS WHERE USERNAME = v_grantee AND ORACLE_MAINTAINED = 'Y'
        UNION ALL
        SELECT 1 FROM DBA_ROLES WHERE ROLE = v_grantee AND ORACLE_MAINTAINED = 'Y'
    );
    IF v_count > 0 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Cannot revoke from Oracle-maintained account: ' || v_grantee);
    END IF;

    IF v_type = 'SYSTEM' THEN
        v_sql := 'REVOKE ' || v_privilege ||
                 ' FROM ' || DBMS_ASSERT.ENQUOTE_NAME(v_grantee, FALSE);

    ELSIF v_type = 'ROLE' THEN
        v_sql := 'REVOKE ' || DBMS_ASSERT.ENQUOTE_NAME(v_privilege, FALSE) ||
                 ' FROM ' || DBMS_ASSERT.ENQUOTE_NAME(v_grantee, FALSE);

    ELSIF v_type IN ('TABLE', 'VIEW', 'PROCEDURE', 'FUNCTION') THEN
        IF v_owner IS NULL OR v_object IS NULL THEN
            RAISE_APPLICATION_ERROR(-20003, 'p_owner and p_object are required for ' || v_type || ' privileges.');
        END IF;
        v_sql := 'REVOKE ' || v_privilege ||
                 ' ON '    || DBMS_ASSERT.ENQUOTE_NAME(v_owner,  FALSE) || '.' ||
                               DBMS_ASSERT.ENQUOTE_NAME(v_object, FALSE) ||
                 ' FROM '  || DBMS_ASSERT.ENQUOTE_NAME(v_grantee, FALSE);

    ELSIF v_type = 'COLUMN' THEN
        -- Oracle does not support column-level REVOKE natively.
        -- Revoke the privilege at table level using the TABLE type instead.
        RAISE_APPLICATION_ERROR(-20004,
            'Column-level REVOKE is not supported by Oracle. ' ||
            'Use p_type = ''TABLE'' to revoke the privilege on the entire table.');

    ELSE
        RAISE_APPLICATION_ERROR(-20005,
            'Invalid type: ' || p_type ||
            '. Accepted values: SYSTEM, ROLE, TABLE, VIEW, PROCEDURE, FUNCTION, COLUMN.');
    END IF;

    EXECUTE IMMEDIATE v_sql;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20099,
            'USP_REVOKE_PRIV [' || v_type || ' | ' || v_privilege ||
            ' | ' || NVL(v_object, 'N/A') || '] FROM ' || v_grantee || ': ' || SQLERRM);
END USP_REVOKE_PRIV;
/

-- USP_GRANT_OBJECT_PRIVILEGE: grant an object or column-level privilege to a user or role
-- Column SELECT uses a dynamic view; column UPDATE uses native column-level grant
CREATE OR REPLACE PROCEDURE USP_GRANT_OBJECT_PRIVILEGE (
    p_grantee     IN VARCHAR2,
    p_privilege   IN VARCHAR2,  -- SELECT, INSERT, UPDATE, DELETE, EXECUTE
    p_object_name IN VARCHAR2,
    p_column_list IN VARCHAR2 DEFAULT NULL,
    p_with_grant  IN NUMBER   DEFAULT 0
) AUTHID CURRENT_USER AS
    v_sql       VARCHAR2(2000);
    v_option    VARCHAR2(50)  := '';
    v_view_name VARCHAR2(128);
    v_grantee   VARCHAR2(128) := UPPER(p_grantee);
    v_priv      VARCHAR2(128) := UPPER(p_privilege);
    v_object    VARCHAR2(128) := UPPER(p_object_name);
    v_cols      VARCHAR2(1000) := UPPER(p_column_list);
    v_count     NUMBER;
BEGIN
    IF v_object IS NULL THEN
        RAISE_APPLICATION_ERROR(-20001, 'Object name is required.');
    END IF;

    SELECT COUNT(*) INTO v_count FROM ALL_USERS WHERE USERNAME = v_grantee;
    IF v_count = 0 THEN
        SELECT COUNT(*) INTO v_count FROM DBA_ROLES WHERE ROLE = v_grantee;
        IF v_count = 0 THEN
            RAISE_APPLICATION_ERROR(-20002, 'Grantee "' || p_grantee || '" does not exist.');
        END IF;
    END IF;

    SELECT COUNT(*) INTO v_count FROM ALL_OBJECTS
    WHERE OBJECT_NAME = v_object AND OWNER = 'HOSPITAL';
    IF v_count = 0 THEN
        RAISE_APPLICATION_ERROR(-20003, 'Object "' || p_object_name || '" not found in hospital schema.');
    END IF;

    IF p_with_grant = 1 THEN
        v_option := ' WITH GRANT OPTION';
    END IF;

    IF v_priv = 'SELECT' AND v_cols IS NOT NULL THEN
        -- Column-level SELECT is implemented via a restricted view owned by hospital_dba
        v_view_name := 'V_PRIV_' || SUBSTR(v_object, 1, 15) || '_' || SUBSTR(v_grantee, 1, 10);
        EXECUTE IMMEDIATE 'CREATE OR REPLACE VIEW ' || v_view_name ||
                          ' AS SELECT ' || v_cols || ' FROM hospital.' || v_object;
        v_sql := 'GRANT SELECT ON ' || v_view_name || ' TO ' || v_grantee || v_option;

    ELSIF v_priv = 'UPDATE' AND v_cols IS NOT NULL THEN
        v_sql := 'GRANT UPDATE (' || v_cols || ') ON hospital.' || v_object ||
                 ' TO ' || v_grantee || v_option;

    ELSIF v_priv = 'EXECUTE' THEN
        v_sql := 'GRANT EXECUTE ON hospital.' || v_object || ' TO ' || v_grantee || v_option;

    ELSE
        v_sql := 'GRANT ' || v_priv || ' ON hospital.' || v_object ||
                 ' TO ' || v_grantee || v_option;
    END IF;

    EXECUTE IMMEDIATE v_sql;
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE = -1926 THEN
            RAISE_APPLICATION_ERROR(-20004,
                'Oracle does not allow WITH GRANT OPTION when granting to a role.');
        ELSE
            RAISE_APPLICATION_ERROR(-20005, 'USP_GRANT_OBJECT_PRIVILEGE: ' || SQLERRM);
        END IF;
END USP_GRANT_OBJECT_PRIVILEGE;
/

-- USP_GRANT_ROLE_TO_USER: assign a role to a user and activate it as a default role
CREATE OR REPLACE PROCEDURE USP_GRANT_ROLE_TO_USER (
    p_user       IN VARCHAR2,
    p_role       IN VARCHAR2,
    p_with_admin IN NUMBER DEFAULT 0
) AUTHID CURRENT_USER AS
    v_sql   VARCHAR2(500);
    v_user  VARCHAR2(128) := UPPER(p_user);
    v_role  VARCHAR2(128) := UPPER(p_role);
    v_count NUMBER;
BEGIN
    IF p_user IS NULL OR p_role IS NULL THEN
        RAISE_APPLICATION_ERROR(-20101, 'User and role name cannot be empty.');
    END IF;

    SELECT COUNT(*) INTO v_count FROM DBA_USERS WHERE USERNAME = v_user;
    IF v_count = 0 THEN
        RAISE_APPLICATION_ERROR(-20102, 'User "' || v_user || '" does not exist.');
    END IF;

    SELECT COUNT(*) INTO v_count FROM DBA_ROLES WHERE ROLE = v_role;
    IF v_count = 0 THEN
        RAISE_APPLICATION_ERROR(-20103, 'Role "' || v_role || '" does not exist.');
    END IF;

    v_sql := 'GRANT ' || v_role || ' TO ' || v_user;
    IF p_with_admin = 1 THEN
        v_sql := v_sql || ' WITH ADMIN OPTION';
    END IF;

    EXECUTE IMMEDIATE v_sql;
    EXECUTE IMMEDIATE 'ALTER USER ' || v_user || ' DEFAULT ROLE ALL';
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20104, 'USP_GRANT_ROLE_TO_USER: ' || SQLERRM);
END USP_GRANT_ROLE_TO_USER;
/

-- USP_GRANT_SYSTEM_PRIVILEGE: grant a system privilege to a user or role
CREATE OR REPLACE PROCEDURE USP_GRANT_SYSTEM_PRIVILEGE (
    p_grantee    IN VARCHAR2,
    p_privilege  IN VARCHAR2,
    p_with_admin IN NUMBER DEFAULT 0
) AUTHID CURRENT_USER AS
    v_sql     VARCHAR2(500);
    v_count   NUMBER;
    v_grantee VARCHAR2(128) := UPPER(p_grantee);
    v_priv    VARCHAR2(128) := UPPER(p_privilege);
BEGIN
    IF p_grantee IS NULL OR p_privilege IS NULL THEN
        RAISE_APPLICATION_ERROR(-20001, 'Grantee and privilege cannot be empty.');
    END IF;

    SELECT COUNT(*) INTO v_count FROM DBA_USERS WHERE USERNAME = v_grantee;
    IF v_count = 0 THEN
        SELECT COUNT(*) INTO v_count FROM DBA_ROLES WHERE ROLE = v_grantee;
        IF v_count = 0 THEN
            RAISE_APPLICATION_ERROR(-20002, 'Grantee "' || v_grantee || '" does not exist.');
        END IF;
    END IF;

    v_sql := 'GRANT ' || v_priv || ' TO ' || v_grantee;
    IF p_with_admin = 1 THEN
        v_sql := v_sql || ' WITH ADMIN OPTION';
    END IF;

    EXECUTE IMMEDIATE v_sql;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20003, 'USP_GRANT_SYSTEM_PRIVILEGE: ' || SQLERRM);
END USP_GRANT_SYSTEM_PRIVILEGE;
/
