ALTER SESSION SET CONTAINER = PDB_QLYT;
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
      AND (ST.STAFF_ROLE LIKE N'K%thu%t vi%n' OR ST.STAFF_ROLE = N'Kỹ thuật viên')
      AND ST.IS_ACTIVE = 1;

    V_ROWS_UPDATED := SQL%ROWCOUNT;

    IF V_ROWS_UPDATED = 0 THEN
        RAISE_APPLICATION_ERROR(-20032, 'Không tìm thấy kỹ thuật viên hiện tại hoặc không có quyền cập nhật.');
    END IF;
END;
/
GRANT EXECUTE ON hospital_dba.UPDATE_TECHNICIAN_PERSONAL_INFO TO RL_TECHNICIAN;
EXIT;
