--------------------------------------------------------------------------------
-- 1. PROCEDURE: Get list of Grantees (Users or Roles)
-- Requirement: Data formatting inside SP, English naming/comments
--------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE usp_GetGrantees (
    p_grantee_type IN VARCHAR2, -- 'USER' or 'ROLE'
    p_result_cursor OUT SYS_REFCURSOR
) AS
    v_type VARCHAR2(10) := UPPER(p_grantee_type);
BEGIN
    IF v_type = 'USER' THEN
        OPEN p_result_cursor FOR 
        SELECT UPPER(username) FROM dba_users 
        WHERE oracle_maintained = 'N' 
        ORDER BY username;
    ELSE
        OPEN p_result_cursor FOR 
        SELECT UPPER(role) FROM dba_roles 
        WHERE oracle_maintained = 'N' 
        ORDER BY role;
    END IF;
END;
/

--------------------------------------------------------------------------------
-- 2. PROCEDURE: Get list of Database Objects (Tables, Views, etc.)
--------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE usp_GetObjects (
    p_object_type IN VARCHAR2, -- 'TABLE', 'VIEW', 'PROCEDURE', 'FUNCTION'
    p_result_cursor OUT SYS_REFCURSOR
) AS
    v_type VARCHAR2(20) := UPPER(p_object_type);
BEGIN
    OPEN p_result_cursor FOR 
    SELECT UPPER(object_name) 
    FROM all_objects 
    WHERE object_type = v_type 
      AND owner = 'HOSPITAL_DBA' -- Ensure the owner is consistent
    ORDER BY object_name;
END;
/

--------------------------------------------------------------------------------
-- 3. PROCEDURE: Get list of Columns for a specific table/view
--------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE usp_GetColumns (
    p_table_name IN VARCHAR2,
    p_result_cursor OUT SYS_REFCURSOR
) AS
    v_table VARCHAR2(128) := UPPER(p_table_name);
BEGIN
    OPEN p_result_cursor FOR 
    SELECT UPPER(column_name) 
    FROM all_tab_columns 
    WHERE table_name = v_table 
      AND owner = 'HOSPITAL_DBA'
    ORDER BY column_id;
END;
/

--------------------------------------------------------------------------------
-- 4. PROCEDURE: Main Privilege Granting Logic (Rule 3)
-- Requirements: All inputs handled with UPPER, Logic for Column-level select
--------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE usp_HandleGrantPrivilege (
    p_grantee      IN VARCHAR2,
    p_priv_or_role IN VARCHAR2,
    p_object_name  IN VARCHAR2 DEFAULT NULL,
    p_column_list  IN VARCHAR2 DEFAULT NULL,
    p_with_grant   IN NUMBER   DEFAULT 0, -- 1 for WITH GRANT OPTION
    p_with_admin   IN NUMBER   DEFAULT 0  -- 1 for WITH ADMIN OPTION
) 
AUTHID CURRENT_USER AS
    v_sql        VARCHAR2(2000);
    v_option     VARCHAR2(50)  := '';
    v_view_name  VARCHAR2(128);
    v_grantee    VARCHAR2(128) := UPPER(p_grantee);
    v_priv       VARCHAR2(128) := UPPER(p_priv_or_role);
    v_object     VARCHAR2(128) := UPPER(p_object_name);
    v_cols       VARCHAR2(1000):= UPPER(p_column_list);
BEGIN
    -- 1. Determine the Granting Option suffix (Rule 3b)
    IF p_with_admin = 1 THEN 
        v_option := ' WITH ADMIN OPTION';
    ELSIF p_with_grant = 1 THEN 
        v_option := ' WITH GRANT OPTION';
    END IF;

    -- 2. Handle Case: Granting ROLE to USER (Rule 3a)
    IF v_object IS NULL THEN
        v_sql := 'GRANT ' || v_priv || ' TO ' || v_grantee || v_option;
        EXECUTE IMMEDIATE v_sql;
        
        -- Automatically enable role as default for the user (Standard Practice)
        -- We perform this only if the grantee is a user (not another role)
        BEGIN
            EXECUTE IMMEDIATE 'ALTER USER ' || v_grantee || ' DEFAULT ROLE ALL';
        EXCEPTION
            WHEN OTHERS THEN NULL; -- Ignore if grantee is a role
        END;
        
    ELSE
        -- 3. Handle Case: Granting Object Privileges (Rule 3c)
        
        -- SELECT privilege with specific columns (Using Dynamic View)
        IF v_priv = 'SELECT' AND v_cols IS NOT NULL THEN
            v_view_name := 'V_PRIV_' || SUBSTR(v_object, 1, 15) || '_' || SUBSTR(v_grantee, 1, 10);
            
            -- Create the restricted view
            EXECUTE IMMEDIATE 'CREATE OR REPLACE VIEW ' || v_view_name || 
                             ' AS SELECT ' || v_cols || ' FROM ' || v_object;
            
            -- Grant select on the new view
            v_sql := 'GRANT SELECT ON ' || v_view_name || ' TO ' || v_grantee || v_option;
            
        -- UPDATE privilege with specific columns
        ELSIF v_priv = 'UPDATE' AND v_cols IS NOT NULL THEN
            v_sql := 'GRANT UPDATE (' || v_cols || ') ON ' || v_object || ' TO ' || v_grantee || v_option;
            
        -- EXECUTE privilege for Procedures/Functions
        ELSIF v_priv = 'EXECUTE' THEN
            v_sql := 'GRANT EXECUTE ON ' || v_object || ' TO ' || v_grantee || v_option;
            
        -- Standard privileges (INSERT, DELETE, or full-table SELECT/UPDATE)
        ELSE
            v_sql := 'GRANT ' || v_priv || ' ON ' || v_object || ' TO ' || v_grantee || v_option;
        END IF;

        -- Execute the privilege command
        EXECUTE IMMEDIATE v_sql;
    END IF;
END;
/
