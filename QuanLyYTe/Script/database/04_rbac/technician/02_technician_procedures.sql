-- ==============================================================================
-- 02_technician_procedures.sql
-- Mục đích: Tạo các Stored Procedures để Kỹ thuật viên truy xuất và cập nhật dữ liệu.
-- Người thực thi: hospital_dba
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

-- Xóa thủ tục nếu đã tồn tại
BEGIN
    EXECUTE IMMEDIATE 'DROP PROCEDURE hospital_dba.USP_GET_TECHNICIAN_SERVICE_RECORDS';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -4043 THEN
            DBMS_OUTPUT.PUT_LINE('Drop procedure error: ' || SQLERRM);
        END IF;
END;
/

BEGIN
    EXECUTE IMMEDIATE 'DROP PROCEDURE hospital_dba.USP_UPDATE_TECHNICIAN_SERVICE_RESULT';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -4043 THEN
            DBMS_OUTPUT.PUT_LINE('Drop procedure error: ' || SQLERRM);
        END IF;
END;
/

BEGIN
    EXECUTE IMMEDIATE 'DROP PROCEDURE hospital_dba.USP_GET_TECHNICIAN_PERSONAL_INFO';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -4043 THEN
            DBMS_OUTPUT.PUT_LINE('Drop procedure error: ' || SQLERRM);
        END IF;
END;
/

BEGIN
    EXECUTE IMMEDIATE 'DROP PROCEDURE hospital_dba.USP_UPDATE_TECHNICIAN_PERSONAL_INFO';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -4043 THEN
            DBMS_OUTPUT.PUT_LINE('Drop procedure error: ' || SQLERRM);
        END IF;
END;
/
    
-- Thủ tục lấy danh sách hồ sơ dịch vụ của Kỹ thuật viên đang đăng nhập
CREATE OR REPLACE PROCEDURE USP_GET_TECHNICIAN_SERVICE_RECORDS (
    P_CURSOR OUT SYS_REFCURSOR
)
AUTHID CURRENT_USER
AS
BEGIN
    OPEN P_CURSOR FOR
        SELECT
            SR.RECORD_ID,
            SR.SERVICE_TYPE,
            SR.SERVICE_DATE,
            SR.TECHNICIAN_ID,
            SR.SERVICE_RESULT
        FROM hospital_dba.V_TECHNICIAN_SERVICE_RECORD SR
        ORDER BY SR.SERVICE_DATE DESC;
END;
/

-- Thủ tục cập nhật kết quả dịch vụ do Kỹ thuật viên phụ trách
CREATE OR REPLACE PROCEDURE USP_UPDATE_TECHNICIAN_SERVICE_RESULT (
    P_RECORD_ID      IN VARCHAR2,
    P_SERVICE_TYPE   IN NVARCHAR2,
    P_SERVICE_DATE   IN DATE,
    P_SERVICE_RESULT IN NVARCHAR2
)
AUTHID CURRENT_USER
AS
BEGIN
    UPDATE hospital_dba.V_TECHNICIAN_SERVICE_RECORD SR
    SET SR.SERVICE_RESULT = P_SERVICE_RESULT
    WHERE SR.RECORD_ID = P_RECORD_ID
      AND SR.SERVICE_TYPE = P_SERVICE_TYPE
      AND TRUNC(SR.SERVICE_DATE) = TRUNC(P_SERVICE_DATE);
    COMMIT;
END;
/

-- Thủ tục xem thông tin cá nhân của Kỹ thuật viên đang đăng nhập
CREATE OR REPLACE PROCEDURE USP_GET_TECHNICIAN_PERSONAL_INFO (
    P_CURSOR OUT SYS_REFCURSOR
)
AUTHID CURRENT_USER
AS
BEGIN
    OPEN P_CURSOR FOR
        SELECT ST.STAFF_ID, ST.FULL_NAME, ST.GENDER, ST.BIRTHDATE, ST.ID_CARD, 
               ST.HOMETOWN, ST.PHONE, ST.DEPT_NAME, ST.FACILITY, ST.ROLE
        FROM hospital_dba.V_TECHNICIAN_PERSONAL_INFO ST;
END;
/

-- Thủ tục cập nhật thông tin cá nhân (quê quán, số điện thoại) của Kỹ thuật viên
CREATE OR REPLACE PROCEDURE USP_UPDATE_TECHNICIAN_PERSONAL_INFO (
    P_HOMETOWN IN NVARCHAR2,
    P_PHONE    IN VARCHAR2
)
AUTHID CURRENT_USER
AS
BEGIN
    UPDATE hospital_dba.V_TECHNICIAN_PERSONAL_INFO
    SET HOMETOWN = P_HOMETOWN,
        PHONE = P_PHONE;
    COMMIT;
END;
/


