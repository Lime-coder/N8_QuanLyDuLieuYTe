-- Chạy với quyền SYSDBA
ALTER SESSION SET CONTAINER = PDB_QLYT;

-- ============================================================
-- 1. XÓA CẤU TRÚC VÀ DỮ LIỆU NHẬT KÝ CŨ
-- ============================================================
BEGIN
    EXECUTE IMMEDIATE 'DELETE FROM SYS.AUD$';      -- Xóa Standard Audit log
    EXECUTE IMMEDIATE 'DELETE FROM SYS.FGA_LOG$';  -- Xóa Fine-Grained Audit log
    COMMIT;

    -- B. Hủy các thiết lập quy tắc cũ
    BEGIN EXECUTE IMMEDIATE 'NOAUDIT ALL ON hospital.staff'; EXCEPTION WHEN OTHERS THEN NULL; END;
    BEGIN EXECUTE IMMEDIATE 'NOAUDIT ALL ON hospital.department'; EXCEPTION WHEN OTHERS THEN NULL; END;
    BEGIN EXECUTE IMMEDIATE 'NOAUDIT ALL ON hospital.medical_record'; EXCEPTION WHEN OTHERS THEN NULL; END;
    BEGIN EXECUTE IMMEDIATE 'NOAUDIT ALL ON hospital.USP_MANAGE_PRESCRIPTION'; EXCEPTION WHEN OTHERS THEN NULL; END;
    BEGIN EXECUTE IMMEDIATE 'NOAUDIT ALL ON hospital.SP_COORD_GET_DOCTORS'; EXCEPTION WHEN OTHERS THEN NULL; END;

    -- C. Xóa các Fine-Grained Audit Policies cũ
    BEGIN DBMS_FGA.DROP_POLICY('HOSPITAL', 'PRESCRIPTION', 'FGA_PRESC_UPDATE'); EXCEPTION WHEN OTHERS THEN NULL; END;
    BEGIN DBMS_FGA.DROP_POLICY('HOSPITAL', 'MEDICAL_RECORD', 'FGA_MR_UPDATE'); EXCEPTION WHEN OTHERS THEN NULL; END;
    BEGIN DBMS_FGA.DROP_POLICY('HOSPITAL', 'SERVICE_RECORD', 'FGA_SR_ILLEGAL'); EXCEPTION WHEN OTHERS THEN NULL; END;

    -- D. Xóa các Stored Procedures cũ của Nhật ký
    BEGIN EXECUTE IMMEDIATE 'DROP PROCEDURE hospital_dba.USP_GET_STANDARD_AUDIT'; EXCEPTION WHEN OTHERS THEN NULL; END;
    BEGIN EXECUTE IMMEDIATE 'DROP PROCEDURE hospital_dba.USP_GET_FGA_AUDIT'; EXCEPTION WHEN OTHERS THEN NULL; END;
END;
/

-- ============================================================
-- 2. CẤP QUYỀN VÀ THIẾT LẬP HỆ THỐNG
-- ============================================================

GRANT SELECT ON SYS.DBA_AUDIT_TRAIL TO hospital_dba;
GRANT SELECT ON SYS.DBA_FGA_AUDIT_TRAIL TO hospital_dba;
GRANT SELECT ANY DICTIONARY TO hospital_dba;

-- ============================================================
-- 3. THIẾT LẬP LẠI QUY TẮC KIỂM TOÁN (5 NGỮ CẢNH)
-- ============================================================
AUDIT DELETE ON hospital.staff BY ACCESS;
AUDIT UPDATE ON hospital.department BY ACCESS;
AUDIT SELECT ON hospital.medical_record BY ACCESS;
AUDIT EXECUTE ON hospital.USP_MANAGE_PRESCRIPTION BY ACCESS;
AUDIT EXECUTE ON hospital.SP_COORD_GET_DOCTORS BY ACCESS; 

-- ============================================================
-- 4. THIẾT LẬP LẠI FINE-GRAINED AUDIT (FGA)
-- ============================================================
BEGIN
  -- a. Cập nhật ĐƠN THUỐC
  DBMS_FGA.ADD_POLICY('HOSPITAL', 'PRESCRIPTION', 'FGA_PRESC_UPDATE', 
    audit_column => 'RECORD_ID, PRESCRIPTION_DATE, MEDICINE_NAME, DOSAGE', statement_types => 'UPDATE');

  -- b & c. Cập nhật HSBA
  DBMS_FGA.ADD_POLICY('HOSPITAL', 'MEDICAL_RECORD', 'FGA_MR_UPDATE', 
    audit_column => 'DIAGNOSIS, TREATMENT_PLAN, CONCLUSION', statement_types => 'UPDATE');

  -- d. Thêm, xóa, sửa trên SERVICE_RECORD
  DBMS_FGA.ADD_POLICY('HOSPITAL', 'SERVICE_RECORD', 'FGA_SR_ILLEGAL', 
    statement_types => 'INSERT, UPDATE, DELETE');
END;
/

-- ============================================================
-- 5. TẠO STORED PROCEDURES CHO GIAO DIỆN
-- ============================================================

-- 1. Lấy nhật ký hệ thống
CREATE OR REPLACE PROCEDURE hospital_dba.USP_GET_STANDARD_AUDIT (
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS 
BEGIN
    OPEN p_cursor FOR
    SELECT USERNAME, 
           TO_CHAR(TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') as TIMESTAMP, 
           OBJ_NAME as OBJECT, 
           ACTION_NAME as ACTION, 
           CASE WHEN RETURNCODE = 0 THEN 'Success' ELSE 'Failed' END AS STATUS, 
           SQL_TEXT
    FROM SYS.DBA_AUDIT_TRAIL 
    WHERE OWNER IN ('HOSPITAL', 'HOSPITAL_DBA')
    ORDER BY TIMESTAMP DESC;
END;
/

-- 2. Lấy nhật ký FGA
CREATE OR REPLACE PROCEDURE hospital_dba.USP_GET_FGA_AUDIT (
    p_policy_name IN VARCHAR2,
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
    SELECT DB_USER as USERNAME, 
           TO_CHAR(TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') as TIMESTAMP, 
           OBJECT_NAME, POLICY_NAME, STATEMENT_TYPE, SQL_TEXT,
           'Success' AS STATUS
    FROM SYS.DBA_FGA_AUDIT_TRAIL
    WHERE OBJECT_SCHEMA = 'HOSPITAL'
      AND (p_policy_name IS NULL OR POLICY_NAME = p_policy_name)
    ORDER BY TIMESTAMP DESC;
END;
/

GRANT EXECUTE ON hospital_dba.USP_GET_STANDARD_AUDIT TO hospital_dba;
GRANT EXECUTE ON hospital_dba.USP_GET_FGA_AUDIT TO hospital_dba;