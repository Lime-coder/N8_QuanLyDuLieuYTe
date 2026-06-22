--ALTER SESSION SET CURRENT_SCHEMA = HOSPITAL;
--SET SERVEROUTPUT ON;

ALTER SESSION SET CONTAINER = PDB_QLYT;
-- ALTER PLUGGABLE DATABASE PDB_QLYT OPEN;
-- ============================================================
-- Remove old objects if exist
-- ============================================================

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
    EXECUTE IMMEDIATE 'DROP PROCEDURE hospital_dba.GET_TECHNICIAN_SERVICE_RECORDS';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -4043 THEN
            DBMS_OUTPUT.PUT_LINE('Drop procedure error: ' || SQLERRM);
        END IF;
END;
/ 

BEGIN
    EXECUTE IMMEDIATE 'DROP PROCEDURE hospital_dba.UPDATE_TECHNICIAN_SERVICE_RESULT';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -4043 THEN
            DBMS_OUTPUT.PUT_LINE('Drop procedure error: ' || SQLERRM);
        END IF;
END;
/ 

BEGIN
    EXECUTE IMMEDIATE 'DROP PROCEDURE hospital_dba.GET_TECHNICIAN_PERSONAL_INFO';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -4043 THEN
            DBMS_OUTPUT.PUT_LINE('Drop procedure error: ' || SQLERRM);
        END IF;
END;
/ 

BEGIN
    EXECUTE IMMEDIATE 'DROP PROCEDURE hospital_dba.UPDATE_TECHNICIAN_PERSONAL_INFO';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -4043 THEN
            DBMS_OUTPUT.PUT_LINE('Drop procedure error: ' || SQLERRM);
        END IF;
END;
/
-- ============================================================
-- View: technician only sees assigned service records
-- ============================================================

CREATE OR REPLACE VIEW hospital_dba.V_TECHNICIAN_SERVICE_RECORD AS
SELECT
    SR.RECORD_ID,
    SR.SERVICE_TYPE,
    SR.SERVICE_DATE,
    SR.TECHNICIAN_ID,
    SR.SERVICE_RESULT
FROM HOSPITAL.SERVICE_RECORD SR
JOIN HOSPITAL.STAFF ST
    ON SR.TECHNICIAN_ID = ST.STAFF_ID
WHERE UPPER(ST.USERNAME_DB) = SYS_CONTEXT('USERENV', 'SESSION_USER')
  AND ST.STAFF_ROLE = N'Kỹ thuật viên'
  AND ST.IS_ACTIVE = 1;

-- ============================================================
-- Get assigned service records
-- ============================================================
CREATE OR REPLACE PROCEDURE hospital_dba.GET_TECHNICIAN_SERVICE_RECORDS (
    P_CURSOR OUT SYS_REFCURSOR
)
AUTHID DEFINER
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

-- ============================================================
-- Update only SERVICE_RESULT of assigned service record
-- ============================================================
CREATE OR REPLACE PROCEDURE hospital_dba.UPDATE_TECHNICIAN_SERVICE_RESULT (
    P_RECORD_ID      IN VARCHAR2,
    P_SERVICE_TYPE   IN NVARCHAR2,
    P_SERVICE_DATE   IN DATE,
    P_SERVICE_RESULT IN NVARCHAR2
)
AUTHID DEFINER
AS
    V_ROWS_UPDATED NUMBER;
BEGIN
    UPDATE HOSPITAL.SERVICE_RECORD SR
    SET SR.SERVICE_RESULT = P_SERVICE_RESULT
    WHERE SR.RECORD_ID = P_RECORD_ID
      AND SR.SERVICE_TYPE = P_SERVICE_TYPE
      AND SR.SERVICE_DATE = P_SERVICE_DATE
      AND SR.TECHNICIAN_ID = (
            SELECT ST.STAFF_ID
            FROM HOSPITAL.STAFF ST
            WHERE UPPER(ST.USERNAME_DB) = SYS_CONTEXT('USERENV', 'SESSION_USER')
              AND ST.STAFF_ROLE = N'Kỹ thuật viên'
              AND ST.IS_ACTIVE = 1
      );

    V_ROWS_UPDATED := SQL%ROWCOUNT;

    IF V_ROWS_UPDATED = 0 THEN
        RAISE_APPLICATION_ERROR(
            -20031,
            N'Không được phép cập nhật: dịch vụ không thuộc kỹ thuật viên hiện tại.'
        );
    END IF;
END;
/ 

-- ============================================================
-- Get current technician personal info
-- ============================================================
CREATE OR REPLACE PROCEDURE hospital_dba.GET_TECHNICIAN_PERSONAL_INFO(
    P_CURSOR OUT SYS_REFCURSOR
)
AUTHID DEFINER
AS
BEGIN
    OPEN P_CURSOR FOR
        SELECT
            ST.STAFF_ID,
            ST.FULL_NAME,
            ST.GENDER,
            ST.BIRTHDATE,
            ST.ID_CARD,
            ST.STAFF_ROLE AS ROLE,
            ST.PHONE,
            ST.HOMETOWN
        FROM HOSPITAL.STAFF ST
        WHERE UPPER(ST.USERNAME_DB) = SYS_CONTEXT('USERENV', 'SESSION_USER')
          AND ST.STAFF_ROLE = N'Kỹ thuật viên'
          AND ST.IS_ACTIVE = 1;
END;
/

-- ============================================================
-- Update allowed personal fields only
-- ============================================================
CREATE OR REPLACE PROCEDURE hospital_dba.UPDATE_TECHNICIAN_PERSONAL_INFO(
    P_PHONE IN VARCHAR2,
    P_HOMETOWN IN NVARCHAR2
)
AUTHID DEFINER
AS
    V_ROWS_UPDATED NUMBER;
BEGIN
    UPDATE HOSPITAL.STAFF ST
    SET ST.PHONE = CASE 
                       WHEN P_PHONE IS NULL OR TRIM(P_PHONE) = '' 
                       THEN ST.PHONE 
                       ELSE P_PHONE 
                   END,
        ST.HOMETOWN = CASE 
                          WHEN P_HOMETOWN IS NULL OR TRIM(P_HOMETOWN) = '' 
                          THEN ST.HOMETOWN 
                          ELSE P_HOMETOWN 
                      END
    WHERE UPPER(ST.USERNAME_DB) = SYS_CONTEXT('USERENV', 'SESSION_USER')
      AND ST.STAFF_ROLE = N'Kỹ thuật viên'
      AND ST.IS_ACTIVE = 1;

    V_ROWS_UPDATED := SQL%ROWCOUNT;

    IF V_ROWS_UPDATED = 0 THEN
        RAISE_APPLICATION_ERROR(-20032, N'Không tìm thấy kỹ thuật viên hiện tại hoặc không có quyền cập nhật.');
    END IF;
END;
/


-- ============================================================
-- Grant safe privileges to role
-- ============================================================

GRANT SELECT ON hospital_dba.V_TECHNICIAN_SERVICE_RECORD TO RL_TECHNICIAN;

GRANT EXECUTE ON hospital_dba.GET_TECHNICIAN_SERVICE_RECORDS TO RL_TECHNICIAN;
GRANT EXECUTE ON hospital_dba.UPDATE_TECHNICIAN_SERVICE_RESULT TO RL_TECHNICIAN;

GRANT EXECUTE ON hospital_dba.GET_TECHNICIAN_PERSONAL_INFO TO RL_TECHNICIAN;
GRANT EXECUTE ON hospital_dba.UPDATE_TECHNICIAN_PERSONAL_INFO TO RL_TECHNICIAN;
COMMIT;
