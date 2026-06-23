-- ============================================================
-- Yêu cầu 3: Vận dụng cơ chế kiểm toán
-- ============================================================
-- Run as hospital_dba
ALTER SESSION SET CONTAINER = PDB_QLYT;

-- ============================================================
-- 1. Thực hiện kiểm toán dùng Standard audit: 5 ngữ cảnh
-- ============================================================

-- 1.1. Tạo function để demo audit
CREATE OR REPLACE FUNCTION hospital_dba.F_GET_DOCTOR_STATS(p_dept_id IN VARCHAR2) 
RETURN NUMBER AS
    v_cnt NUMBER;
BEGIN
    SELECT COUNT(*) INTO v_cnt FROM hospital.staff WHERE dept_id = p_dept_id;
    RETURN v_cnt;
END;
/   
GRANT EXECUTE ON hospital_dba.F_GET_DOCTOR_STATS TO rl_doctor, rl_coordinator;

-- 1.2. 5 ngữ cảnh (có cả SUCCESSFUL và NOT SUCCESSFUL theo yêu cầu đề bài)
-- NC#1: TABLE - Xóa nhân viên
AUDIT DELETE ON hospital.staff BY ACCESS WHENEVER SUCCESSFUL;
AUDIT DELETE ON hospital.staff BY ACCESS WHENEVER NOT SUCCESSFUL;

-- NC#2: TABLE - Cập nhật thông tin bệnh nhân
AUDIT UPDATE ON hospital.patient BY ACCESS WHENEVER SUCCESSFUL;
AUDIT UPDATE ON hospital.patient BY ACCESS WHENEVER NOT SUCCESSFUL;

-- NC#3: VIEW - Tra cứu danh sách bác sĩ
AUDIT SELECT ON hospital.VW_COORD_DOCTORS BY ACCESS WHENEVER SUCCESSFUL;
AUDIT SELECT ON hospital.VW_COORD_DOCTORS BY ACCESS WHENEVER NOT SUCCESSFUL;

-- NC#4: STORED PROCEDURE - Cập nhật HSBA
AUDIT EXECUTE ON hospital.USP_UPDATE_MEDICAL_RECORD BY ACCESS WHENEVER SUCCESSFUL;
AUDIT EXECUTE ON hospital.USP_UPDATE_MEDICAL_RECORD BY ACCESS WHENEVER NOT SUCCESSFUL;

-- NC#5: FUNCTION - Thống kê số bác sĩ theo khoa
AUDIT EXECUTE ON hospital_dba.F_GET_DOCTOR_STATS BY ACCESS WHENEVER SUCCESSFUL;
AUDIT EXECUTE ON hospital_dba.F_GET_DOCTOR_STATS BY ACCESS WHENEVER NOT SUCCESSFUL;

-- ============================================================
-- 2. Dùng Fine-grained Audit hoặc Unified Audit để thực hiện kiểm toán
-- ============================================================

--  3.3a, 3.3b (FGA - THÀNH CÔNG)

BEGIN
    -- 3.3a: FGA Policy cho bảng ĐƠNTHUỐC
    DBMS_FGA.ADD_POLICY(
        object_schema   => 'HOSPITAL',
        object_name     => 'PRESCRIPTION',
        policy_name     => 'FGA_PRESCRIPTION_COLS',
        audit_column    => 'RECORD_ID, PRESCRIPTION_DATE, MEDICINE_NAME, DOSAGE',
        statement_types => 'UPDATE'
    );

    -- 3.3b: FGA Policy cho bảng HỒ SƠ BỆNH ÁN
    DBMS_FGA.ADD_POLICY(
        object_schema   => 'HOSPITAL',
        object_name     => 'MEDICAL_RECORD',
        policy_name     => 'FGA_MEDICAL_RECORD_COLS',
        audit_column    => 'DIAGNOSIS, TREATMENT_PLAN, CONCLUSION',
        statement_types => 'UPDATE'
    );
END;
/

-- 3.3c (UNIFIED - BẤT HỢP PHÁP TRÊN HSBA)
CREATE AUDIT POLICY AUD_ILLEGAL_MR_POLICY
  ACTIONS 
    UPDATE ON hospital.medical_record;

-- Bắt hành vi Failed (Sai quyền/Bất hợp pháp) cho 3.3c
AUDIT POLICY AUD_ILLEGAL_MR_POLICY WHENEVER NOT SUCCESSFUL;

-- 3.3d (UNIFIED - BẤT HỢP PHÁP TRÊN SERVICE_RECORD)
CREATE AUDIT POLICY AUD_ILLEGAL_SR_POLICY
  ACTIONS 
    INSERT ON hospital.service_record, 
    UPDATE ON hospital.service_record, 
    DELETE ON hospital.service_record; 

-- Bắt hành vi Failed (Sai quyền/Bất hợp pháp) cho 3.3d
AUDIT POLICY AUD_ILLEGAL_SR_POLICY WHENEVER NOT SUCCESSFUL;

-- ============================================================
-- 3. STORED PROCEDURES
-- ============================================================

-- 3.1. Tab Kiểm toán hệ thống
CREATE OR REPLACE PROCEDURE hospital_dba.USP_GET_REQ32_LOGS (p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT CAST(USERNAME AS VARCHAR2(128)) as USERNAME, TO_CHAR(TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') as TIMESTAMP, 
           CAST(OBJ_NAME AS VARCHAR2(128)) as OBJECT, CAST(ACTION_NAME AS VARCHAR2(128)) as ACTION, 
           CASE WHEN RETURNCODE = 0 THEN 'Success' ELSE 'Failed' END AS STATUS, DBMS_LOB.SUBSTR(SQL_TEXT, 4000, 1) as SQL_TEXT
    FROM DBA_AUDIT_TRAIL 
    WHERE OBJ_NAME IN ('STAFF', 'PATIENT', 'VW_COORD_DOCTORS', 'USP_UPDATE_MEDICAL_RECORD', 'F_GET_DOCTOR_STATS')
    ORDER BY TIMESTAMP DESC;
END;
/

-- 3.2. Tab Đơn thuốc
CREATE OR REPLACE PROCEDURE hospital_dba.USP_GET_REQ33A_LOGS (p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT CAST(DB_USER AS VARCHAR2(128)) as USERNAME, TO_CHAR(TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') as TIMESTAMP, 
           CAST(OBJECT_NAME AS VARCHAR2(128)) as OBJECT, 'UPDATE' as ACTION, 'Success' AS STATUS, CAST(SQL_TEXT AS VARCHAR2(4000)) as SQL_TEXT
    FROM DBA_FGA_AUDIT_TRAIL WHERE POLICY_NAME = 'FGA_PRESCRIPTION_COLS'
    ORDER BY TIMESTAMP DESC;
END;
/

-- 3.3. Tab HSBA (Gộp FGA-Thành công và Unified-Thất bại)
CREATE OR REPLACE PROCEDURE hospital_dba.USP_GET_REQ33BC_LOGS (p_cursor OUT SYS_REFCURSOR) AS
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
CREATE OR REPLACE PROCEDURE hospital_dba.USP_GET_REQ33D_LOGS (p_cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_cursor FOR
    SELECT CAST(DBUSERNAME AS VARCHAR2(128)) as USERNAME, TO_CHAR(EVENT_TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') as TIMESTAMP, 
           CAST(OBJECT_NAME AS VARCHAR2(128)) as OBJECT, CAST(ACTION_NAME AS VARCHAR2(128)) as ACTION, 
           'Failed' AS STATUS, DBMS_LOB.SUBSTR(SQL_TEXT, 4000, 1) as SQL_TEXT
    FROM UNIFIED_AUDIT_TRAIL WHERE OBJECT_NAME = 'SERVICE_RECORD' AND RETURN_CODE != 0
    ORDER BY EVENT_TIMESTAMP DESC;
END;
/
