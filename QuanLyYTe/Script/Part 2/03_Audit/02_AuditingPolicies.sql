-- ============================================================
-- Yêu cầu 3: Vận dụng cơ chế kiểm toán
-- ============================================================
-- Run as hospital_dba
ALTER SESSION SET CONTAINER = PDB_QLYT;

-- ============================================================
-- 1. Thực hiện kiểm toán dùng Standard audit: 5 ngữ cảnh
-- ============================================================

-- 1.1. Tạo function để demo audit (Hàm trích xuất dữ liệu tiền sử bệnh)
CREATE OR REPLACE FUNCTION hospital_dba.F_EXTRACT_MEDICAL_HISTORY(p_patient_id IN VARCHAR2) 
RETURN NCLOB AS
    v_history NCLOB;
BEGIN
    SELECT medical_history INTO v_history FROM hospital.patient WHERE patient_id = p_patient_id;
    RETURN v_history;
END;
/   
GRANT EXECUTE ON hospital_dba.F_EXTRACT_MEDICAL_HISTORY TO rl_doctor, rl_coordinator;

-- 1.2. 5 ngữ cảnh (có cả SUCCESSFUL và NOT SUCCESSFUL theo yêu cầu đề bài)
-- NC#1: TABLE - Thêm mới HSBA
AUDIT INSERT ON hospital.medical_record BY ACCESS WHENEVER SUCCESSFUL;
AUDIT INSERT ON hospital.medical_record BY ACCESS WHENEVER NOT SUCCESSFUL;
 
-- NC#2: TABLE - Cập nhật thông tin bệnh nhân
AUDIT UPDATE ON hospital.patient BY ACCESS WHENEVER SUCCESSFUL;
AUDIT UPDATE ON hospital.patient BY ACCESS WHENEVER NOT SUCCESSFUL;

-- NC#3: VIEW - Tra cứu danh sách bác sĩ
AUDIT SELECT ON hospital.VW_COORD_DOCTORS BY ACCESS WHENEVER SUCCESSFUL;
AUDIT SELECT ON hospital.VW_COORD_DOCTORS BY ACCESS WHENEVER NOT SUCCESSFUL;

-- NC#4: STORED PROCEDURE - Quản lý (Thêm mới, cập nhật, xóa) đơn thuốc 
AUDIT EXECUTE ON hospital.USP_MANAGE_PRESCRIPTION BY ACCESS WHENEVER SUCCESSFUL;
AUDIT EXECUTE ON hospital.USP_MANAGE_PRESCRIPTION BY ACCESS WHENEVER NOT SUCCESSFUL;

-- NC#5: FUNCTION - Trích xuất tiền sử bệnh án
AUDIT EXECUTE ON hospital_dba.F_EXTRACT_MEDICAL_HISTORY BY ACCESS WHENEVER SUCCESSFUL;
AUDIT EXECUTE ON hospital_dba.F_EXTRACT_MEDICAL_HISTORY BY ACCESS WHENEVER NOT SUCCESSFUL;

-- ============================================================
-- 2. Dùng Fine-grained Audit hoặc Unified Audit để thực hiện kiểm toán
-- ============================================================

--  3.3a, 3.3b (FGA - THÀNH CÔNG)

BEGIN
    -- 3.3a: Hành vi cập nhật trên thuộc tính MÃHSBA, NGÀYĐT, TÊNTHUỐC, LIỀUDÙNG của quan hệ ĐƠNTHUỐC
    DBMS_FGA.ADD_POLICY(
        object_schema   => 'HOSPITAL',
        object_name     => 'PRESCRIPTION', 
        policy_name     => 'FGA_PRESCRIPTION_COLS',
        audit_column    => 'RECORD_ID, PRESCRIPTION_DATE, MEDICINE_NAME, DOSAGE',
        statement_types => 'UPDATE',
        audit_trail     => DBMS_FGA.DB + DBMS_FGA.EXTENDED
    );

    -- 3.3b: Hành vi của người dùng  trên các trường CHẨNĐOÁN, ĐIỀUTRỊ, KẾTLUẬN của quan hệ HSBA
    DBMS_FGA.ADD_POLICY(
        object_schema   => 'HOSPITAL',
        object_name     => 'MEDICAL_RECORD',
        policy_name     => 'FGA_MEDICAL_RECORD_COLS',
        audit_column    => 'DIAGNOSIS, TREATMENT_PLAN, CONCLUSION',
        statement_types => 'UPDATE',
        audit_trail     => DBMS_FGA.DB + DBMS_FGA.EXTENDED
    );
END;
/

-- 3.3c (UNIFIED - BẤT HỢP PHÁP TRÊN HSBA)
-- Hành vi của người dùng cập nhật bất hợp pháp trên các trường CHẨNĐOÁN,ĐIỀUTRỊ, KẾTLUẬN
CREATE AUDIT POLICY AUD_ILLEGAL_MEDICAL_RECORD_POLICY
  ACTIONS 
    UPDATE ON hospital.medical_record;

-- Bắt hành vi Failed (Sai quyền/Bất hợp pháp)
AUDIT POLICY AUD_ILLEGAL_MEDICAL_RECORD_POLICY WHENEVER NOT SUCCESSFUL;

-- 3.3d (UNIFIED - BẤT HỢP PHÁP TRÊN SERVICE_RECORD)
-- Hành vi thêm, xóa, sửa bất hợp pháp trên quan hệ HSBA_DV.
CREATE AUDIT POLICY AUD_ILLEGAL_SERVICE_RECORD_POLICY
  ACTIONS 
    INSERT ON hospital.service_record, 
    UPDATE ON hospital.service_record, 
    DELETE ON hospital.service_record; 

-- Bắt hành vi Failed (Sai quyền/Bất hợp pháp)
AUDIT POLICY AUD_ILLEGAL_SERVICE_RECORD_POLICY WHENEVER NOT SUCCESSFUL;

-- ============================================================
-- 3. STORED PROCEDURES
-- ============================================================

-- 3.1. Tab Kiểm toán hệ thống
CREATE OR REPLACE PROCEDURE hospital_dba.USP_GET_STANDARD_AUDIT_LOGS (p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT CAST(USERNAME AS VARCHAR2(128)) as USERNAME, TO_CHAR(TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') as TIMESTAMP, 
           CAST(OBJ_NAME AS VARCHAR2(128)) as OBJECT, CAST(ACTION_NAME AS VARCHAR2(128)) as ACTION, 
           CASE WHEN RETURNCODE = 0 THEN 'Success' ELSE 'Failed' END AS STATUS, DBMS_LOB.SUBSTR(SQL_TEXT, 4000, 1) as SQL_TEXT
    FROM DBA_AUDIT_TRAIL 
    WHERE OWNER IN ('HOSPITAL', 'HOSPITAL_DBA')
    ORDER BY TIMESTAMP DESC;
END;
/

-- 3.2. Tab Đơn thuốc
CREATE OR REPLACE PROCEDURE hospital_dba.USP_GET_PRESCRIPTION_AUDIT_LOGS (p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT CAST(DB_USER AS VARCHAR2(128)) as USERNAME, TO_CHAR(TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') as TIMESTAMP, 
           CAST(OBJECT_NAME AS VARCHAR2(128)) as OBJECT, 'UPDATE' as ACTION, 'Success' AS STATUS, CAST(SQL_TEXT AS VARCHAR2(4000)) as SQL_TEXT
    FROM DBA_FGA_AUDIT_TRAIL WHERE POLICY_NAME = 'FGA_PRESCRIPTION_COLS'
    ORDER BY TIMESTAMP DESC;
END;
/

-- 3.3. Tab HSBA (Gộp FGA-Thành công và Unified-Thất bại)
CREATE OR REPLACE PROCEDURE hospital_dba.USP_GET_MEDICAL_RECORD_AUDIT_LOGS (p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT CAST(DB_USER AS VARCHAR2(128)) as USERNAME, TO_CHAR(TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') as TIMESTAMP, 
           CAST(OBJECT_NAME AS VARCHAR2(128)) as OBJECT, 'UPDATE' as ACTION, 'Success' AS STATUS, CAST(SQL_TEXT AS VARCHAR2(4000)) as SQL_TEXT
    FROM DBA_FGA_AUDIT_TRAIL WHERE POLICY_NAME = 'FGA_MEDICAL_RECORD_COLS'
    UNION ALL
    SELECT CAST(DBUSERNAME AS VARCHAR2(128)), TO_CHAR(EVENT_TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS'), 
           CAST(OBJECT_NAME AS VARCHAR2(128)), 'UPDATE', 'Failed', DBMS_LOB.SUBSTR(SQL_TEXT, 4000, 1)
    FROM UNIFIED_AUDIT_TRAIL WHERE OBJECT_NAME = 'MEDICAL_RECORD' AND RETURN_CODE != 0
    ORDER BY TIMESTAMP DESC;
END;
/

-- 3.4. Tab Dịch vụ (Lấy từ Unified Thất bại)
CREATE OR REPLACE PROCEDURE hospital_dba.USP_GET_SERVICE_RECORD_AUDIT_LOGS (p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT CAST(DBUSERNAME AS VARCHAR2(128)) as USERNAME, TO_CHAR(EVENT_TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') as TIMESTAMP, 
           CAST(OBJECT_NAME AS VARCHAR2(128)) as OBJECT, CAST(ACTION_NAME AS VARCHAR2(128)) as ACTION, 
           'Failed' AS STATUS, DBMS_LOB.SUBSTR(SQL_TEXT, 4000, 1) as SQL_TEXT
    FROM UNIFIED_AUDIT_TRAIL WHERE OBJECT_NAME = 'SERVICE_RECORD' AND RETURN_CODE != 0
    ORDER BY EVENT_TIMESTAMP DESC;
END;
/
