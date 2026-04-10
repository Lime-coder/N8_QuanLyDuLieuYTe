CREATE OR REPLACE PROCEDURE usp_GrantPrivilege_Q3 (
    p_grantee      IN VARCHAR2,
    p_priv_or_role IN VARCHAR2,
    p_object_name  IN VARCHAR2 DEFAULT NULL,
    p_column_list  IN VARCHAR2 DEFAULT NULL,
    p_with_grant   IN NUMBER DEFAULT 0, -- 1 cho GRANT OPTION
    p_with_admin   IN NUMBER DEFAULT 0  -- 1 cho ADMIN OPTION
) 
AUTHID CURRENT_USER AS
    v_sql VARCHAR2(2000);
    v_option VARCHAR2(50) := '';
    v_view_name VARCHAR2(128);
BEGIN
    -- Xác định hậu tố Option
    IF p_with_admin = 1 THEN v_option := ' WITH ADMIN OPTION';
    ELSIF p_with_grant = 1 THEN v_option := ' WITH GRANT OPTION';
    END IF;

    -- Trường hợp cấp ROLE cho USER (3a)
    IF p_object_name IS NULL THEN
        v_sql := 'GRANT ' || p_priv_or_role || ' TO ' || p_grantee || v_option;
    ELSE
        -- Trường hợp cấp quyền đối tượng (3c)
        IF UPPER(p_priv_or_role) = 'SELECT' AND p_column_list IS NOT NULL THEN
            v_view_name := 'V_Q3_' || SUBSTR(p_object_name, 1, 15) || '_' || SUBSTR(p_grantee, 1, 10);
            EXECUTE IMMEDIATE 'CREATE OR REPLACE VIEW ' || v_view_name || ' AS SELECT ' || p_column_list || ' FROM ' || p_object_name;
            v_sql := 'GRANT SELECT ON ' || v_view_name || ' TO ' || p_grantee || v_option;
        ELSIF UPPER(p_priv_or_role) = 'UPDATE' AND p_column_list IS NOT NULL THEN
            v_sql := 'GRANT UPDATE (' || p_column_list || ') ON ' || p_object_name || ' TO ' || p_grantee || v_option;
        ELSIF UPPER(p_priv_or_role) = 'EXECUTE' THEN
            v_sql := 'GRANT EXECUTE ON ' || p_object_name || ' TO ' || p_grantee || v_option;
        ELSE
            v_sql := 'GRANT ' || p_priv_or_role || ' ON ' || p_object_name || ' TO ' || p_grantee || v_option;
        END IF;
    END IF;

    EXECUTE IMMEDIATE v_sql;
END;
/