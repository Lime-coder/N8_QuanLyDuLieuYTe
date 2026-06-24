-- ==============================================================================
-- 04_label_notification_data.sql
-- Chạy dưới quyền: hospital_dba
-- ==============================================================================
ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

SET DEFINE OFF; 
SET SERVEROUTPUT ON;

BEGIN
    DELETE FROM hospital.notification;
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

INSERT INTO hospital.notification (notification_id, description, posted_date, location, ols_label) 
VALUES ('T1', UNISTR('Th\00F4ng b\00E1o chung to\00E0n vi\1EC7n'), SYSDATE, UNISTR('To\00E0n qu\1ED1c'), CHAR_TO_LABEL('HOSP_OLS_POL', 'NV'));

INSERT INTO hospital.notification (notification_id, description, posted_date, location, ols_label) 
VALUES ('T2', UNISTR('H\1ECDp chi\1EBFn l\01B0\1EE3c Qu\00FD 3'), SYSDATE, UNISTR('Ph\00F2ng h\1ECDp VIP'), CHAR_TO_LABEL('HOSP_OLS_POL', 'BGD'));

INSERT INTO hospital.notification (notification_id, description, posted_date, location, ols_label) 
VALUES ('T3', UNISTR('\0110\00E1nh gi\00E1 ng\00E2n s\00E1ch c\00E1c khoa'), SYSDATE, UNISTR('H\1ED9i tr\01B0\1EDDng A'), CHAR_TO_LABEL('HOSP_OLS_POL', 'LDK'));

INSERT INTO hospital.notification (notification_id, description, posted_date, location, ols_label) 
VALUES ('T4', UNISTR('B\00E1o c\00E1o thi\1EBFt b\1ECB n\1ED9i soi'), SYSDATE, UNISTR('Ph\00F2ng Gi\00E1m \0111\1ED1c'), CHAR_TO_LABEL('HOSP_OLS_POL', 'LDK:TH'));

INSERT INTO hospital.notification (notification_id, description, posted_date, location, ols_label) 
VALUES ('T5', UNISTR('Thay \0111\1ED5i ca tr\1EF1c tu\1EA7n t\1EDBi'), SYSDATE, UNISTR('Khoa TH - HCM'), CHAR_TO_LABEL('HOSP_OLS_POL', 'NV:TH:HCM'));

INSERT INTO hospital.notification (notification_id, description, posted_date, location, ols_label) 
VALUES ('T6', UNISTR('T\1EADp hu\1EA5n an to\00E0n v\1EC7 sinh'), SYSDATE, UNISTR('Khoa TH - HN'), CHAR_TO_LABEL('HOSP_OLS_POL', 'NV:TH:HN'));

INSERT INTO hospital.notification (notification_id, description, posted_date, location, ols_label) 
VALUES ('T7', UNISTR('H\1ECDp li\00EAn khoa TH & TK'), SYSDATE, UNISTR('Chi nh\00E1nh H\1EA3i Ph\00F2ng'), CHAR_TO_LABEL('HOSP_OLS_POL', 'LDK:TH,TK:HP'));

COMMIT;

-- Procedures
CREATE OR REPLACE PROCEDURE hospital.USP_GET_NOTIFICATIONS (
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT notification_id, description, TO_CHAR(posted_date, 'DD/MM/YYYY HH24:MI') as posted_date, location, LABEL_TO_CHAR(ols_label) as OLS_LABEL
        FROM hospital.notification
        ORDER BY TO_NUMBER(SUBSTR(notification_id, 2));
END;
/

CREATE OR REPLACE PROCEDURE hospital.USP_ADD_NOTIFICATION (
    p_description IN NVARCHAR2,
    p_location    IN NVARCHAR2,
    p_label_str   IN VARCHAR2
) AUTHID CURRENT_USER AS
    v_id VARCHAR2(10);
BEGIN
    v_id := 'T' || hospital.seq_notification_id.NEXTVAL;
    
    INSERT INTO hospital.notification (notification_id, description, posted_date, location, ols_label)
    VALUES (v_id, p_description, SYSDATE, p_location, CHAR_TO_LABEL('HOSP_OLS_POL', UPPER(p_label_str)));
    
    COMMIT;
END;
/

ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

CREATE OR REPLACE PROCEDURE hospital_dba.USP_GET_USER_OLS_LABEL (
    p_username IN VARCHAR2,
    p_cursor   OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT USER_NAME, MAX_READ_LABEL 
        FROM DBA_SA_USERS 
        WHERE POLICY_NAME = 'HOSP_OLS_POL' 
          AND USER_NAME = UPPER(TRIM(p_username));
END;
/

CREATE OR REPLACE PROCEDURE hospital_dba.USP_SET_USER_OLS_LABEL (
    p_username  IN VARCHAR2,
    p_label_str IN VARCHAR2
) AUTHID CURRENT_USER AS
BEGIN
    BEGIN
        SA_USER_ADMIN.DROP_USER_ACCESS(
            policy_name => 'HOSP_OLS_POL', 
            user_name   => UPPER(TRIM(p_username))
        );
    EXCEPTION WHEN OTHERS THEN NULL; 
    END;
    
    IF p_label_str IS NOT NULL AND LENGTH(TRIM(p_label_str)) > 0 THEN
        SA_USER_ADMIN.SET_USER_LABELS(
            policy_name    => 'HOSP_OLS_POL',
            user_name      => UPPER(TRIM(p_username)),
            max_read_label => UPPER(p_label_str)
        );
    END IF;
    
    COMMIT;
END;
/

GRANT EXECUTE ON hospital_dba.USP_GET_USER_OLS_LABEL TO hospital_dba;
GRANT EXECUTE ON hospital_dba.USP_SET_USER_OLS_LABEL TO hospital_dba;
GRANT SELECT ON hospital.notification TO rl_dba, rl_coordinator, rl_doctor, rl_technician, rl_patient;
GRANT EXECUTE ON hospital.USP_GET_NOTIFICATIONS TO rl_dba, rl_coordinator, rl_doctor, rl_technician, rl_patient;
GRANT EXECUTE ON hospital.USP_ADD_NOTIFICATION TO rl_dba, rl_coordinator;
GRANT INSERT ON hospital.notification TO rl_dba, rl_coordinator;
GRANT SELECT ON hospital.seq_notification_id TO rl_dba, rl_coordinator;
