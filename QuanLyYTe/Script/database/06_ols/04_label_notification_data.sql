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
VALUES ('T1', N'Thông báo chung toàn viện', SYSDATE, N'Toàn quốc', CHAR_TO_LABEL('HOSP_OLS_POL', 'NV'));

INSERT INTO hospital.notification (notification_id, description, posted_date, location, ols_label) 
VALUES ('T2', N'Họp chiến lược Quý 3', SYSDATE, N'Phòng họp VIP', CHAR_TO_LABEL('HOSP_OLS_POL', 'BGD'));

INSERT INTO hospital.notification (notification_id, description, posted_date, location, ols_label) 
VALUES ('T3', N'Đánh giá ngân sách các khoa', SYSDATE, N'Hội trường A', CHAR_TO_LABEL('HOSP_OLS_POL', 'LDK'));

INSERT INTO hospital.notification (notification_id, description, posted_date, location, ols_label) 
VALUES ('T4', N'Báo cáo thiết bị nội soi', SYSDATE, N'Phòng Giám đốc', CHAR_TO_LABEL('HOSP_OLS_POL', 'LDK:TH'));

INSERT INTO hospital.notification (notification_id, description, posted_date, location, ols_label) 
VALUES ('T5', N'Thay đổi ca trực tuần tới', SYSDATE, N'Khoa TH - HCM', CHAR_TO_LABEL('HOSP_OLS_POL', 'NV:TH:HCM'));

INSERT INTO hospital.notification (notification_id, description, posted_date, location, ols_label) 
VALUES ('T6', N'Tập huấn an toàn vệ sinh', SYSDATE, N'Khoa TH - HN', CHAR_TO_LABEL('HOSP_OLS_POL', 'NV:TH:HN'));

INSERT INTO hospital.notification (notification_id, description, posted_date, location, ols_label) 
VALUES ('T7', N'Họp liên khoa TH & TK', SYSDATE, N'Chi nhánh Hải Phòng', CHAR_TO_LABEL('HOSP_OLS_POL', 'LDK:TH,TK:HP'));

COMMIT;

-- Procedures
CREATE OR REPLACE PROCEDURE hospital.USP_GET_NOTIFICATIONS (
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT notification_id, description, TO_CHAR(posted_date, 'DD/MM/YYYY HH24:MI') as posted_date, location
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
