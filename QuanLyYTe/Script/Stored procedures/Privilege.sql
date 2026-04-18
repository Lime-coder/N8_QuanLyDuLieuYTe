-- ============================================================
-- FILE: Privilege.sql
-- SCHEMA: hospital_dba @ PDB_QLYT
-- MÔ TẢ:
--   USP_GET_USERS        : Lấy danh sách user (không tính SYS/SYSTEM)
--   USP_GET_ROLES        : Lấy danh sách role trong database
--   USP_GET_ALL_PRIVS    : Xem toàn bộ quyền của một user hoặc role
--   USP_GET_PRIVS_ON_OBJ : Xem quyền của tất cả user/role trên một đối tượng cụ thể
--   USP_REVOKE_PRIV      : Thu hồi quyền từ user hoặc role
-- ============================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;


-- ============================================================
-- SP 1: LẤY DANH SÁCH USER CỦA DỰ ÁN
-- Chỉ lấy: hospital_dba + các username_db trong bảng staff và patient
-- ============================================================
CREATE OR REPLACE PROCEDURE USP_GET_USERS
(
    p_cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_cursor FOR
    -- DBA của hệ thống
    SELECT 'HOSPITAL_DBA' AS USERNAME FROM DUAL

    UNION

    -- Username của nhân viên (staff)
    SELECT UPPER(s.username_db) AS USERNAME
    FROM staff s
    WHERE s.username_db IS NOT NULL

    UNION

    -- Username của bệnh nhân (patient)
    SELECT UPPER(p.username_db) AS USERNAME
    FROM patient p
    WHERE p.username_db IS NOT NULL

    ORDER BY USERNAME;
END USP_GET_USERS;
/


-- ============================================================
-- SP 2: LẤY DANH SÁCH ROLE CỦA DỰ ÁN
-- Chỉ lấy các role bắt đầu bằng 'RL_' (do đồ án tạo ra)
-- Các role đó: RL_COORDINATOR, RL_DOCTOR, RL_TECHNICIAN, RL_PATIENT
-- ============================================================
CREATE OR REPLACE PROCEDURE USP_GET_ROLES
(
    p_cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_cursor FOR
    SELECT DISTINCT GRANTED_ROLE AS ROLENAME
    FROM USER_ROLE_PRIVS
    WHERE GRANTED_ROLE LIKE 'RL\_%' ESCAPE '\'

    UNION

    SELECT DISTINCT GRANTED_ROLE AS ROLENAME
    FROM ROLE_ROLE_PRIVS
    WHERE GRANTED_ROLE LIKE 'RL\_%' ESCAPE '\'

    ORDER BY ROLENAME;
END USP_GET_ROLES;
/


-- ============================================================
-- SP 3: XEM TOÀN BỘ QUYỀN CỦA MỘT USER HOẶC ROLE
-- p_grantee : Tên user hoặc tên role cần xem quyền
-- ============================================================
CREATE OR REPLACE PROCEDURE USP_GET_ALL_PRIVS
(
    p_grantee IN  VARCHAR2,
    p_cursor  OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_cursor FOR

    -- 1. SYSTEM PRIVILEGES (chỉ áp dụng cho user)
    SELECT
        'SYSTEM'        AS LOAI_QUYEN,
        sp.PRIVILEGE    AS QUYEN,
        NULL            AS CHU_SO_HUU,
        NULL            AS DOI_TUONG,
        NULL            AS COT,
        sp.ADMIN_OPTION AS CO_THE_CAP_LAI
    FROM USER_SYS_PRIVS sp
    WHERE UPPER(sp.USERNAME) = UPPER(p_grantee)

    UNION ALL

    -- 2. ROLE PRIVILEGES (role được cấp cho user hoặc role khác)
    SELECT
        'ROLE'          AS LOAI_QUYEN,
        rp.GRANTED_ROLE AS QUYEN,
        NULL            AS CHU_SO_HUU,
        NULL            AS DOI_TUONG,
        NULL            AS COT,
        rp.ADMIN_OPTION AS CO_THE_CAP_LAI
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
        END                 AS LOAI_QUYEN,
        tp.PRIVILEGE        AS QUYEN,
        tp.TABLE_SCHEMA     AS CHU_SO_HUU,
        tp.TABLE_NAME       AS DOI_TUONG,
        NULL                AS COT,
        tp.GRANTABLE        AS CO_THE_CAP_LAI
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
        'COLUMN'            AS LOAI_QUYEN,
        cp.PRIVILEGE        AS QUYEN,
        cp.TABLE_SCHEMA     AS CHU_SO_HUU,
        cp.TABLE_NAME       AS DOI_TUONG,
        cp.COLUMN_NAME      AS COT,
        cp.GRANTABLE        AS CO_THE_CAP_LAI
    FROM ALL_COL_PRIVS cp
    WHERE UPPER(cp.GRANTEE) = UPPER(p_grantee)

    ORDER BY LOAI_QUYEN, CHU_SO_HUU, DOI_TUONG, COT, QUYEN;

END USP_GET_ALL_PRIVS;
/


-- ============================================================
-- SP 4: XEM QUYỀN TRÊN MỘT ĐỐI TƯỢNG CỤ THỂ
-- Hiển thị tất cả user/role có quyền trên bảng/view/procedure/function đó
-- p_owner       : Schema sở hữu đối tượng (ví dụ: HOSPITAL_DBA)
-- p_object_name : Tên đối tượng (ví dụ: PATIENT)
-- p_cursor      : Con trỏ kết quả
-- ============================================================
CREATE OR REPLACE PROCEDURE USP_GET_PRIVS_ON_OBJ
(
    p_owner       IN  VARCHAR2,
    p_object_name IN  VARCHAR2,
    p_cursor      OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_cursor FOR

    -- Quyền mức bảng/đối tượng
    SELECT
        tp.GRANTEE          AS NGUOI_NHAN,
        tp.TABLE_SCHEMA     AS CHU_SO_HUU,
        tp.TABLE_NAME       AS DOI_TUONG,
        NULL                AS COT,
        tp.PRIVILEGE        AS QUYEN,
        tp.GRANTABLE        AS CO_THE_CAP_LAI,
        tp.HIERARCHY        AS CO_THE_HIERARCHY
    FROM ALL_TAB_PRIVS tp
    WHERE UPPER(tp.TABLE_SCHEMA) = UPPER(p_owner)
      AND UPPER(tp.TABLE_NAME)   = UPPER(p_object_name)
      AND NOT EXISTS (
          SELECT 1
          FROM ALL_COL_PRIVS cp
          WHERE UPPER(cp.GRANTEE)     = UPPER(tp.GRANTEE)
            AND cp.TABLE_SCHEMA       = tp.TABLE_SCHEMA
            AND cp.TABLE_NAME         = tp.TABLE_NAME
            AND cp.PRIVILEGE          = tp.PRIVILEGE
      )

    UNION ALL

    -- Quyền mức cột
    SELECT
        cp.GRANTEE          AS NGUOI_NHAN,
        cp.TABLE_SCHEMA     AS CHU_SO_HUU,
        cp.TABLE_NAME       AS DOI_TUONG,
        cp.COLUMN_NAME      AS COT,
        cp.PRIVILEGE        AS QUYEN,
        cp.GRANTABLE        AS CO_THE_CAP_LAI,
        'NO'                AS CO_THE_HIERARCHY
    FROM ALL_COL_PRIVS cp
    WHERE UPPER(cp.TABLE_SCHEMA) = UPPER(p_owner)
      AND UPPER(cp.TABLE_NAME)   = UPPER(p_object_name)

    ORDER BY NGUOI_NHAN, COT, QUYEN;

END USP_GET_PRIVS_ON_OBJ;
/


-- ============================================================
-- SP 5: THU HỒI QUYỀN TỪ USER HOẶC ROLE
-- p_type      : 'SYSTEM' | 'ROLE' | 'TABLE' | 'VIEW'
--               | 'PROCEDURE' | 'FUNCTION' | 'COLUMN'
-- p_privilege : Tên quyền hoặc tên role được cấp
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
        -- Lưu ý: Oracle không hỗ trợ REVOKE column privilege trực tiếp như GRANT,
        -- cần revoke toàn bộ quyền object rồi grant lại từng cột (workaround phổ biến).
        -- Ở đây ta revoke quyền mức bảng nếu là quyền cột.
        v_sql := 'REVOKE ' || v_privilege ||
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