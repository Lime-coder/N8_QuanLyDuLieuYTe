-- 1. Quyền đối tượng
CREATE OR REPLACE PROCEDURE USP_GRANT_OBJECT_PRIV (
    p_grantee      IN VARCHAR2, -- Người nhận (User hoặc Role)
    p_priv_or_role IN VARCHAR2, -- SELECT, INSERT, UPDATE, DELETE, EXECUTE
    p_object_name  IN VARCHAR2,-- Tên bảng, view, proc, function
    p_column_list  IN VARCHAR2 DEFAULT NULL,
    p_with_grant   IN NUMBER   DEFAULT 0 -- 1 for WITH GRANT OPTION
) 
AUTHID CURRENT_USER AS
    v_sql        VARCHAR2(2000);
    v_option     VARCHAR2(50)  := '';
    v_view_name  VARCHAR2(128);
    v_grantee    VARCHAR2(128) := UPPER(p_grantee);
    v_priv       VARCHAR2(128) := UPPER(p_priv_or_role);
    v_object     VARCHAR2(128) := UPPER(p_object_name);
    v_cols       VARCHAR2(1000):= UPPER(p_column_list);
    v_count      NUMBER;
BEGIN
    -- 1. Kiểm tra tồn tại
    ----- Kiểm tra đối tượng
    IF v_object IS NULL THEN
        RAISE_APPLICATION_ERROR(-20001, 'Lỗi: Phải cung cấp tên đối tượng để cấp quyền đối tượng.');
    END IF;
    
    ----- Kiểm tra tồn tại của người nhận - Grantee
    SELECT COUNT(*) INTO v_count FROM ALL_USERS WHERE USERNAME = v_grantee;
    IF v_count = 0 THEN
        SELECT COUNT(*) INTO v_count FROM DBA_ROLES WHERE ROLE = v_grantee;
        IF v_count = 0 THEN
            RAISE_APPLICATION_ERROR(-20005, 'Lỗi: Người nhận (User/Role) "' || p_grantee || '" không tồn tại.');
        END IF;
    END IF;
    
    ----- Kiểm tra sự tồn tại của đối tượng
    SELECT COUNT(*) INTO v_count FROM ALL_OBJECTS WHERE OBJECT_NAME = v_object;
    IF v_count = 0 THEN
        RAISE_APPLICATION_ERROR(-20006, 'Lỗi: Đối tượng dữ liệu "' || p_object_name || '" không tồn tại.');
    END IF;
    
    -- 2. Xác định dùng WITH GRANT OPTION hay không - 3b
    IF p_with_grant = 1 THEN 
        v_option := ' WITH GRANT OPTION';
    END IF;

    -- 3. Phân quyền (Toàn bảng và mức cột)
    
    -- Trường hợp: Quyền select
        -- SELECT mức cột (Sử dụng Dynamic View)
        IF v_priv = 'SELECT' AND v_cols IS NOT NULL THEN
            -- Đặt tên View để phân quyền mức cột
            v_view_name := 'V_PRIV_' || SUBSTR(v_object, 1, 15) || '_' || SUBSTR(v_grantee, 1, 10);
            
           -- Tạo View giới hạn các cột được chọn
            EXECUTE IMMEDIATE 'CREATE OR REPLACE VIEW ' || v_view_name || 
                             ' AS SELECT ' || v_cols || ' FROM ' || v_object;
             -- Cấp quyền SELECT trên View vừa tạo cho người nhận
            v_sql := 'GRANT SELECT ON ' || v_view_name || ' TO ' || v_grantee || v_option;

        -- TRƯỜNG HỢP: QUYỀN UPDATE
        -- UPDATE mức cột
        ELSIF v_priv = 'UPDATE' AND v_cols IS NOT NULL THEN
            v_sql := 'GRANT UPDATE (' || v_cols || ') ON ' || v_object || ' TO ' || v_grantee || v_option;
        
         -- TRƯỜNG HỢP: QUYỀN EXECUTE (Dành cho Proc/Func)
        ELSIF v_priv = 'EXECUTE' THEN
            v_sql := 'GRANT EXECUTE ON ' || v_object || ' TO ' || v_grantee || v_option;
            
        -- Các quyền (INSERT, DELETE, hoặc SELECT/UPDATE toàn bảng)
        ELSE
            v_sql := 'GRANT ' || v_priv || ' ON ' || v_object || ' TO ' || v_grantee || v_option;
        END IF;

        -- Thực thi lệnh cấp quyền
        EXECUTE IMMEDIATE v_sql;
    EXCEPTION
    WHEN OTHERS THEN
        -- Mã lỗi ORA-01926: cannot GRANT to a role WITH GRANT OPTION
        IF SQLCODE = -1926 THEN
            RAISE_APPLICATION_ERROR(-20004, 'Lỗi: Oracle không cho phép dùng WITH GRANT OPTION khi cấp quyền cho Role.');
        ELSE
            RAISE_APPLICATION_ERROR(-20002, 'Lỗi: ' || SQLERRM);
        END IF;
END;
/

--2. Cấp role cho user
CREATE OR REPLACE PROCEDURE USP_GRANT_ROLE_TO_USER (
    p_user IN VARCHAR2,       -- Tên User nhận Role
    p_role IN VARCHAR2,       -- Tên Role được cấp
    p_with_admin IN NUMBER    -- 1: Có WITH ADMIN OPTION, 0: Không
) AS
    v_sql VARCHAR2(500);
    v_user VARCHAR2(128) := UPPER(p_user);
    v_role VARCHAR2(128) := UPPER(p_role);
    v_count_user NUMBER;
    v_count_role NUMBER;
    
BEGIN
    -- 1. KIỂM TRA NULL
    IF p_user IS NULL OR p_role IS NULL THEN
        RAISE_APPLICATION_ERROR(-20101, 'LỖI: Tên User và tên Role không được để trống.');
    END IF;
    
    -- 2. KIỂM TRA SỰ TỒN TẠI
    -- Kiểm tra User
    SELECT COUNT(*) INTO v_count_user FROM DBA_USERS WHERE USERNAME = v_user;
    IF v_count_user = 0 THEN
        RAISE_APPLICATION_ERROR(-20102, 'LỖI: Người dùng "' || v_user || '" không tồn tại.');
    END IF;

    -- Kiểm tra role
    SELECT COUNT(*) INTO v_count_role FROM DBA_ROLES WHERE ROLE = v_role;
    IF v_count_role = 0 THEN
        RAISE_APPLICATION_ERROR(-20103, 'LỖI: Role "' || p_role || '" không tồn tại.');
    END IF;

    -- 3. XÂY DỰNG SQL ĐỘNG
    v_sql := 'GRANT ' || v_role || ' TO ' || v_user;
    
    IF p_with_admin = 1 THEN
        v_sql := v_sql || ' WITH ADMIN OPTION';
    END IF;

    -- 4. THỰC THI GÁN ROLE
    EXECUTE IMMEDIATE v_sql;
    --5. Kích hoạt Role làm mặc định cho User
    EXECUTE IMMEDIATE 'ALTER USER ' || v_user || ' DEFAULT ROLE ALL';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE = -20100 THEN
            RAISE_APPLICATION_ERROR(-20100, 'Lỗi: HỆ THỐNG: ' || SQLERRM);
        END IF;
END;
/

--3. Quyền hệ thống
CREATE OR REPLACE PROCEDURE USP_GRANT_SYSTEM_PRIVILEGE (
    p_grantee IN VARCHAR2,       -- Người nhận (User hoặc Role)
    p_privilege IN VARCHAR2,     -- Quyền hệ thống (CREATE SESSION, CREATE TABLE...)
    p_with_admin IN NUMBER       -- 1: Có WITH ADMIN OPTION, 0: Không
) 
AUTHID CURRENT_USER -- Giúp thực thi với quyền của người gọi (DBA)
AS
    v_sql VARCHAR2(500);
    v_count NUMBER;
    v_grantee VARCHAR2(128) := UPPER(p_grantee);
    v_priv    VARCHAR2(128) := UPPER(p_privilege);
BEGIN
    -- 1. KIỂM TRA ĐẦU VÀO (NULL CHECK)
    IF p_grantee IS NULL OR p_privilege IS NULL THEN
        RAISE_APPLICATION_ERROR(-20001, 'LỖI: Tên người nhận và Quyền hệ thống không được để trống.');
    END IF;
    
    -- 2. KIỂM TRA SỰ TỒN TẠI CỦA NGƯỜI NHẬN
    -- Kiểm tra trong bảng User
    SELECT COUNT(*) INTO v_count FROM DBA_USERS WHERE USERNAME = v_grantee;
    IF v_count = 0 THEN
        -- Nếu không thấy User, kiểm tra tiếp trong bảng Role
        SELECT COUNT(*) INTO v_count FROM DBA_ROLES WHERE ROLE = v_grantee;
        IF v_count = 0 THEN
            RAISE_APPLICATION_ERROR(-20004, 'Người nhận(User/Role)"' || v_grantee || '" không tồn tại trong hệ thống.');
        END IF;
    END IF;
    
    -- 3. XÂY DỰNG CÂU LỆNH SQL ĐỘNG
    v_sql := 'GRANT ' || v_priv || ' TO ' || v_grantee;
    
    IF p_with_admin = 1 THEN
        v_sql := v_sql || ' WITH ADMIN OPTION';
    END IF;

    -- 3. THỰC THI LỆNH
    EXECUTE IMMEDIATE v_sql;

EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE = -20002 THEN
            RAISE_APPLICATION_ERROR(-20002, 'Lỗi: ' || SQLERRM);
        END IF;
END;
/




