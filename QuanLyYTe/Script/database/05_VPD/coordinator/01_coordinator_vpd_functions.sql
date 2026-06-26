-- ==============================================================================
-- 01_coordinator_vpd_functions.sql
-- Chạy dưới quyền: hospital_dba
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

CREATE OR REPLACE FUNCTION FN_VPD_STAFF_SELF (
    p_schema VARCHAR2,
    p_obj    VARCHAR2
)
RETURN VARCHAR2
AS
    v_user         VARCHAR2(100);
    v_current_user VARCHAR2(100);
    v_role         NVARCHAR2(50);
BEGIN
    v_user         := SYS_CONTEXT('USERENV', 'SESSION_USER');
    v_current_user := SYS_CONTEXT('USERENV', 'CURRENT_USER');

    -- DBA/schema owner → bypass
    IF v_user IN ('HOSPITAL', 'HOSPITAL_DBA') THEN
        RETURN '1=1';
    END IF;

    -- Nếu chạy qua Stored Procedure AUTHID DEFINER của HOSPITAL_DBA → bypass
    IF v_current_user IN ('HOSPITAL', 'HOSPITAL_DBA') THEN
        RETURN '1=1';
    END IF;

    -- Kiểm tra vai trò: Coordinator cần xem tất cả staff (TC#2)
    -- Dùng EXECUTE IMMEDIATE để tránh VPD policy áp dụng lại khi query hospital.staff
    BEGIN
        EXECUTE IMMEDIATE
            'SELECT staff_role FROM hospital.staff WHERE UPPER(username_db) = :1'
            INTO v_role USING v_user;
    EXCEPTION WHEN NO_DATA_FOUND THEN
        RETURN '1=0'; -- Không phải nhân viên → không thấy gì
    END;

    IF v_role = UNISTR('\0110i\1EC1u ph\1ED1i vi\00EAn') THEN
        RETURN '1=1'; -- Coordinator thấy tất cả (TC#2)
    END IF;

    -- Bác sĩ, KTV → chỉ thấy chính mình (TC#5)
    RETURN 'UPPER(username_db) = ''' || v_user || '''';

EXCEPTION
    WHEN OTHERS THEN
        RETURN '1=0';
END;
/

CREATE OR REPLACE FUNCTION FN_VPD_COORD_RESTRICT_UPD (
    p_schema VARCHAR2,
    p_obj    VARCHAR2
)
RETURN VARCHAR2
AS
    v_session_user VARCHAR2(100);
    v_current_user VARCHAR2(100);
    v_role         NVARCHAR2(50);
BEGIN
    v_session_user := SYS_CONTEXT('USERENV', 'SESSION_USER');
    v_current_user := SYS_CONTEXT('USERENV', 'CURRENT_USER');

    -- Nếu chạy qua Stored Procedure của HOSPITAL (hoặc DBA), bypass hoàn toàn để tránh bị lỗi khi SP dùng các trường này trong mệnh đề WHERE
    IF v_current_user IN ('HOSPITAL', 'HOSPITAL_DBA') THEN
        RETURN '1=1';
    END IF;

    BEGIN
        EXECUTE IMMEDIATE
            'SELECT staff_role FROM hospital.staff WHERE UPPER(username_db) = :1'
            INTO v_role USING v_session_user;
    EXCEPTION WHEN NO_DATA_FOUND THEN
        RETURN '1=1';
    END;

    -- Nếu Điều phối viên cố tình chạy UPDATE trực tiếp, chặn các cột đã khai báo ở sec_relevant_cols (trả về 1=0 sẽ update 0 dòng)
    IF v_role = UNISTR('\0110i\1EC1u ph\1ED1i vi\00EAn') THEN
        RETURN '1=0';
    END IF;

    RETURN '1=1';
EXCEPTION
    WHEN OTHERS THEN
        RETURN '1=0';
END;
/
