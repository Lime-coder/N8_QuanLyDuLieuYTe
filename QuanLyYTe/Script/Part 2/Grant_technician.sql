ALTER SESSION SET CURRENT_SCHEMA = HOSPITAL;
SET SERVEROUTPUT ON;

ALTER SESSION SET CONTAINER = PDB_QLYT;
-- ALTER PLUGGABLE DATABASE PDB_QLYT OPEN;
-- ============================================================
-- Remove old objects if exist
-- ============================================================

BEGIN
    EXECUTE IMMEDIATE 'DROP VIEW HOSPITAL.V_TECHNICIAN_SERVICE_RECORD';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -942 THEN
            DBMS_OUTPUT.PUT_LINE('Drop view error: ' || SQLERRM);
        END IF;
END;
/ 

BEGIN
    EXECUTE IMMEDIATE 'DROP PROCEDURE HOSPITAL.UPDATE_TECHNICIAN_SERVICE_RESULT';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -4043 THEN
            DBMS_OUTPUT.PUT_LINE('Drop procedure error: ' || SQLERRM);
        END IF;
END;
/ 

-- ============================================================
-- Create view for technician
-- This view only returns service records assigned to
-- the current logged-in technician.
-- ============================================================

CREATE OR REPLACE VIEW HOSPITAL.V_TECHNICIAN_SERVICE_RECORD AS
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
  AND ST.STAFF_ROLE = N'Kỹ thuật viên';

-- ============================================================
-- Create update procedure
-- Technician can only update SERVICE_RESULT of their own
-- assigned service record.
-- ============================================================

CREATE OR REPLACE PROCEDURE HOSPITAL.GET_TECHNICIAN_SERVICE_RECORDS (
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
        FROM HOSPITAL.V_TECHNICIAN_SERVICE_RECORD SR
        ORDER BY SR.SERVICE_DATE DESC;
END;
/ 

CREATE OR REPLACE PROCEDURE HOSPITAL.UPDATE_TECHNICIAN_SERVICE_RESULT (
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
-- Grant safe privileges to role
-- ============================================================

GRANT EXECUTE ON HOSPITAL.GET_TECHNICIAN_SERVICE_RECORDS TO RL_TECHNICIAN;
GRANT SELECT ON HOSPITAL.V_TECHNICIAN_SERVICE_RECORD TO RL_TECHNICIAN;
GRANT EXECUTE ON HOSPITAL.UPDATE_TECHNICIAN_SERVICE_RESULT TO RL_TECHNICIAN;

COMMIT;
