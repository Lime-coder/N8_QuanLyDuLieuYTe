-- ============================================================
-- FILE: Privilege.sql
-- SCHEMA: hospital_dba @ PDB_QLYT
-- LƯU Ý: Dùng USER_*/ALL_* thay DBA_* vì hospital_dba không có
--         SELECT_CATALOG_ROLE.
--         Nếu muốn thấy đầy đủ, grant quyền sau (chạy bởi SYS):
--           GRANT SELECT_CATALOG_ROLE TO hospital_dba;
-- ============================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;

-- ============================================================
-- SP 1: XEM TOÀN BỘ QUYỀN CỦA MỘT USER HOẶC ROLE
-- ============================================================
CREATE OR REPLACE PROCEDURE USP_GET_ALL_PRIVS
(
    p_grantee IN  VARCHAR2,
    p_cursor  OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_cursor FOR

    -- 1. SYSTEM PRIVILEGES
    SELECT
        'SYSTEM'        AS TYPE,
        sp.PRIVILEGE    AS PRIVILEGE,
        NULL            AS OWNER,
        NULL            AS OBJECT_NAME,
        NULL            AS COLUMN_NAME,
        sp.ADMIN_OPTION AS GRANTABLE
    FROM USER_SYS_PRIVS sp
    WHERE UPPER(sp.USERNAME) = UPPER(p_grantee)

    UNION ALL

    -- 2. ROLE PRIVILEGES
    SELECT
        'ROLE'          AS TYPE,
        rp.GRANTED_ROLE AS PRIVILEGE,
        NULL            AS OWNER,
        NULL            AS OBJECT_NAME,
        NULL            AS COLUMN_NAME,
        rp.ADMIN_OPTION AS GRANTABLE
    FROM USER_ROLE_PRIVS rp
    WHERE UPPER(rp.USERNAME) = UPPER(p_grantee)

    UNION ALL

    -- 3. OBJECT PRIVILEGES trên TABLE / VIEW / PROCEDURE / FUNCTION
    SELECT
        CASE ao.OBJECT_TYPE
            WHEN 'TABLE'     THEN 'TABLE'
            WHEN 'VIEW'      THEN 'VIEW'
            WHEN 'PROCEDURE' THEN 'PROCEDURE'
            WHEN 'FUNCTION'  THEN 'FUNCTION'
            ELSE ao.OBJECT_TYPE
        END                 AS TYPE,
        tp.PRIVILEGE        AS PRIVILEGE,
        tp.TABLE_SCHEMA     AS OWNER,
        tp.TABLE_NAME       AS OBJECT_NAME,
        NULL                AS COLUMN_NAME,
        tp.GRANTABLE        AS GRANTABLE
    FROM ALL_TAB_PRIVS tp
    JOIN ALL_OBJECTS ao
        ON  ao.OWNER       = tp.TABLE_SCHEMA
        AND ao.OBJECT_NAME = tp.TABLE_NAME
    WHERE UPPER(tp.GRANTEE) = UPPER(p_grantee)
      AND NOT EXISTS (
          SELECT 1
          FROM ALL_COL_PRIVS cp
          WHERE UPPER(cp.GRANTEE) = UPPER(p_grantee)
            AND cp.TABLE_SCHEMA   = tp.TABLE_SCHEMA
            AND cp.TABLE_NAME     = tp.TABLE_NAME
            AND cp.PRIVILEGE      = tp.PRIVILEGE
      )

    UNION ALL

    -- 4. COLUMN-LEVEL PRIVILEGES (SELECT / UPDATE trên từng cột)
    SELECT
        'COLUMN'            AS TYPE,
        cp.PRIVILEGE        AS PRIVILEGE,
        cp.TABLE_SCHEMA     AS OWNER,
        cp.TABLE_NAME       AS OBJECT_NAME,
        cp.COLUMN_NAME      AS COLUMN_NAME,
        cp.GRANTABLE        AS GRANTABLE
    FROM ALL_COL_PRIVS cp
    WHERE UPPER(cp.GRANTEE) = UPPER(p_grantee)

    ORDER BY TYPE, OWNER, OBJECT_NAME, COLUMN_NAME, PRIVILEGE;

END USP_GET_ALL_PRIVS;
/


-- ============================================================
-- SP 2: THU HỒI QUYỀN TỪ USER HOẶC ROLE
-- p_type      : 'SYSTEM' | 'ROLE' | 'TABLE' | 'VIEW'
--               | 'PROCEDURE' | 'FUNCTION' | 'COLUMN'
-- p_privilege : Tên quyền hoặc tên role
-- p_owner     : Schema sở hữu object (NULL nếu SYSTEM/ROLE)
-- p_object    : Tên object            (NULL nếu SYSTEM/ROLE)
-- p_column    : Tên cột               (chỉ dùng khi TYPE='COLUMN')
-- p_grantee   : User hoặc role cần thu hồi quyền
-- ============================================================
CREATE OR REPLACE PROCEDURE USP_REVOKE_PRIV
(
    p_type      IN VARCHAR2,
    p_privilege IN VARCHAR2,
    p_owner     IN VARCHAR2,
    p_object    IN VARCHAR2,
    p_column    IN VARCHAR2,
    p_grantee   IN VARCHAR2
)
AS
    v_sql       VARCHAR2(1000);
    v_type      VARCHAR2(30)  := UPPER(TRIM(p_type));
    v_privilege VARCHAR2(200) := UPPER(TRIM(p_privilege));
    v_owner     VARCHAR2(128) := UPPER(TRIM(p_owner));
    v_object    VARCHAR2(128) := UPPER(TRIM(p_object));
    v_column    VARCHAR2(128) := UPPER(TRIM(p_column));
    v_grantee   VARCHAR2(128) := UPPER(TRIM(p_grantee));
BEGIN
    IF v_privilege IS NULL OR v_grantee IS NULL THEN
        RAISE_APPLICATION_ERROR(-20001,
            'p_privilege va p_grantee khong duoc de trong.');
    END IF;

    IF v_type = 'SYSTEM' THEN
        v_sql := 'REVOKE ' || v_privilege ||
                 ' FROM '  || DBMS_ASSERT.ENQUOTE_NAME(v_grantee, FALSE);

    ELSIF v_type = 'ROLE' THEN
        v_sql := 'REVOKE ' || DBMS_ASSERT.ENQUOTE_NAME(v_privilege, FALSE) ||
                 ' FROM '  || DBMS_ASSERT.ENQUOTE_NAME(v_grantee, FALSE);

    ELSIF v_type IN ('TABLE', 'VIEW', 'PROCEDURE', 'FUNCTION') THEN
        IF v_owner IS NULL OR v_object IS NULL THEN
            RAISE_APPLICATION_ERROR(-20002,
                'p_owner va p_object la bat buoc voi loai quyen ' || v_type || '.');
        END IF;
        v_sql := 'REVOKE ' || v_privilege ||
                 ' ON '    || DBMS_ASSERT.ENQUOTE_NAME(v_owner,  FALSE) || '.' ||
                              DBMS_ASSERT.ENQUOTE_NAME(v_object, FALSE) ||
                 ' FROM '  || DBMS_ASSERT.ENQUOTE_NAME(v_grantee, FALSE);

    ELSIF v_type = 'COLUMN' THEN
        IF v_owner IS NULL OR v_object IS NULL OR v_column IS NULL THEN
            RAISE_APPLICATION_ERROR(-20003,
                'p_owner, p_object va p_column la bat buoc voi loai quyen COLUMN.');
        END IF;
        v_sql := 'REVOKE ' || v_privilege ||
                 ' (' || DBMS_ASSERT.ENQUOTE_NAME(v_column, FALSE) || ')' ||
                 ' ON '   || DBMS_ASSERT.ENQUOTE_NAME(v_owner,  FALSE) || '.' ||
                             DBMS_ASSERT.ENQUOTE_NAME(v_object, FALSE) ||
                 ' FROM ' || DBMS_ASSERT.ENQUOTE_NAME(v_grantee, FALSE);

    ELSE
        RAISE_APPLICATION_ERROR(-20004,
            'Loai quyen khong hop le: ' || p_type ||
            '. Chap nhan: SYSTEM, ROLE, TABLE, VIEW, PROCEDURE, FUNCTION, COLUMN.');
    END IF;

    EXECUTE IMMEDIATE v_sql;

EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20099,
            'Loi thu hoi quyen [' || v_type || ' | ' || v_privilege ||
            ' | ' || NVL(v_object, 'N/A') || ' | ' || NVL(v_column, 'N/A') ||
            '] FROM ' || v_grantee || ': ' || SQLERRM);
END USP_REVOKE_PRIV;
/

CREATE OR REPLACE PROCEDURE USP_GET_USERS
(
    p_cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_cursor FOR
    SELECT USERNAME
    FROM ALL_USERS
    WHERE USERNAME NOT IN ('SYS','SYSTEM') -- lọc cho gọn
    ORDER BY USERNAME;
END;
/