
--------------------------------------------------------------------------------
--1. Lấy danh sách User hoặc Role
--------------------------------------------------------------------------------
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

--------------------------------------------------------------------------------
-- 2. Lấy danh sách đối tượng (Procedure, Function, Table, View)
--------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE USP_GET_OBJECTS (
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
-- 3. PROCEDURE: Lấy danh sách cột của một bảng cụ thể (Dùng cho phân quyền mức cột)
--------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE USP_GET_COLUMNS (
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
-- 4. Danh sách quyền hệ thống 
--------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE USP_GET_ALL_SYSTEM_PRIVILEGES (
    p_result_cursor OUT SYS_REFCURSOR
) AS
BEGIN
    OPEN p_result_cursor FOR 
    SELECT NAME FROM SYSTEM_PRIVILEGE_MAP 
    ORDER BY NAME;
END;
/