-- ==============================================================================
-- 01_technician_view.sql
-- Mục đích: Tạo View cho phép Kỹ thuật viên xem các hồ sơ dịch vụ do họ phụ trách.
-- Người thực thi: hospital_dba
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

-- Xóa View nếu đã tồn tại
BEGIN
    EXECUTE IMMEDIATE 'DROP VIEW hospital_dba.V_TECHNICIAN_SERVICE_RECORD';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -942 THEN
            DBMS_OUTPUT.PUT_LINE('Drop view error: ' || SQLERRM);
        END IF;
END;
/

BEGIN
    EXECUTE IMMEDIATE 'DROP VIEW hospital_dba.V_TECHNICIAN_PERSONAL_INFO';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -942 THEN
            DBMS_OUTPUT.PUT_LINE('Drop view error: ' || SQLERRM);
        END IF;
END;
/

-- Tạo View lấy thông tin dịch vụ của Kỹ thuật viên (chỉ lấy dịch vụ do user hiện tại phụ trách)
CREATE OR REPLACE VIEW V_TECHNICIAN_SERVICE_RECORD AS
SELECT
    SR.RECORD_ID,
    SR.SERVICE_TYPE,
    SR.SERVICE_DATE,
    SR.TECHNICIAN_ID,
    SR.SERVICE_RESULT
FROM hospital_dba.SERVICE_RECORD SR
WHERE SR.TECHNICIAN_ID = (
    SELECT ST.STAFF_ID 
    FROM hospital_dba.STAFF ST 
    WHERE UPPER(ST.USERNAME_DB) = SYS_CONTEXT('USERENV', 'SESSION_USER')
);

-- Tạo View để nhân viên xem thông tin cá nhân
CREATE OR REPLACE VIEW V_TECHNICIAN_PERSONAL_INFO AS
SELECT ST.STAFF_ID, 
       ST.FULL_NAME, 
       ST.GENDER, 
       ST.BIRTHDATE, 
       ST.ID_CARD, 
       ST.HOMETOWN, 
       ST.PHONE, 
       D.DEPT_NAME, 
       ST.FACILITY, 
       ST.STAFF_ROLE AS ROLE
FROM hospital_dba.STAFF ST
LEFT JOIN hospital_dba.DEPARTMENT D ON ST.DEPT_ID = D.DEPT_ID
WHERE UPPER(ST.USERNAME_DB) = SYS_CONTEXT('USERENV', 'SESSION_USER');


