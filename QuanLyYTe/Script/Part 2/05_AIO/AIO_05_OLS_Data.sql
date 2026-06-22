-- ==============================================================================
-- AIO_05_OLS_Data.sql
-- Run as: HOSPITAL_DBA
-- Container: PDB_QLYT (unless specified otherwise)
-- ==============================================================================

-- ==============================================================================
-- Source: 02_OLS\OLS_AIO_3_HOSP_DATA.sql
-- ==============================================================================

-- ==============================================================================
-- File: OLS_AIO_3_HOSP_DATA.sql
-- Run as: HOSPITAL_DBA (MUST BE A FRESH CONNECTION)
-- Connect to: PDB_QLYT
-- ==============================================================================

ALTER SESSION SET CURRENT_SCHEMA = hospital;
SET DEFINE OFF; 
SET SERVEROUTPUT ON;

PROMPT ==============================================================================
PROMPT 1. Insert Target Data (T1 -> T7)
PROMPT ==============================================================================
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

PROMPT ==============================================================================
PROMPT 2. Build Stored Procedures for Notification App
PROMPT ==============================================================================

-- GET Procedure (Updated with English columns and sorting)
CREATE OR REPLACE PROCEDURE hospital.USP_GET_NOTIFICATIONS (
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT notification_id, description, TO_CHAR(posted_date, 'DD/MM/YYYY HH24:MI') as posted_date, location
        FROM hospital.notification
        ORDER BY TO_NUMBER(SUBSTR(notification_id, 2)); -- T1, T2.. T10 sort correctly
END;
/

-- ADD Procedure (For Admin/Coordinator to create new notifications)
CREATE OR REPLACE PROCEDURE hospital.USP_ADD_NOTIFICATION (
    p_description IN NVARCHAR2,
    p_location    IN NVARCHAR2,
    p_label_str   IN VARCHAR2
) AUTHID CURRENT_USER AS
    v_id VARCHAR2(10);
BEGIN
    -- Format ID as T8, T9, T10...
    v_id := 'T' || hospital.seq_notification_id.NEXTVAL;
    
    INSERT INTO hospital.notification (notification_id, description, posted_date, location, ols_label)
    VALUES (v_id, p_description, SYSDATE, p_location, CHAR_TO_LABEL('HOSP_OLS_POL', UPPER(p_label_str)));
    
    COMMIT;
END;
/

PROMPT ==============================================================================
PROMPT 3. Build OLS Label Management Procedures for DBA
PROMPT ==============================================================================
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
    -- 1. Thu hồi quyền OLS cũ của user (nếu có) để làm sạch
    BEGIN
        SA_USER_ADMIN.DROP_USER_ACCESS(
            policy_name => 'HOSP_OLS_POL', 
            user_name   => UPPER(TRIM(p_username))
        );
    EXCEPTION WHEN OTHERS THEN NULL; 
    END;
    
    -- 2. Cấp nhãn mới (nếu chuỗi truyền vào không rỗng)
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

ALTER SESSION SET CURRENT_SCHEMA = hospital;

PROMPT ==============================================================================
PROMPT 4. Grant Execute Privileges for Application
PROMPT ==============================================================================
DECLARE
BEGIN
    FOR i IN 1..8 LOOP
        EXECUTE IMMEDIATE 'GRANT EXECUTE ON hospital.USP_GET_NOTIFICATIONS TO U' || i;
        EXECUTE IMMEDIATE 'GRANT EXECUTE ON hospital_dba.USP_GET_SESSION_ROLE TO U' || i;
        EXECUTE IMMEDIATE 'GRANT EXECUTE ON hospital_dba.USP_GET_USER_ID TO U' || i;
    END LOOP;
    EXECUTE IMMEDIATE 'GRANT EXECUTE ON hospital.USP_GET_NOTIFICATIONS TO rl_doctor, rl_coordinator, rl_technician, rl_patient';
END;
/

-- Quản lý thông báo
GRANT EXECUTE ON hospital.USP_ADD_NOTIFICATION TO rl_dba;
GRANT EXECUTE ON hospital.USP_ADD_NOTIFICATION TO rl_coordinator;

-- Cấp quyền sequence và bảng cho các role sử dụng USP_ADD_NOTIFICATION
GRANT SELECT ON hospital.seq_notification_id TO rl_dba;
GRANT SELECT ON hospital.seq_notification_id TO rl_coordinator;
GRANT INSERT ON hospital.notification TO rl_coordinator;

-- DBA quản lý nhãn OLS
GRANT EXECUTE ON hospital_dba.USP_GET_USER_OLS_LABEL TO rl_dba;
GRANT EXECUTE ON hospital_dba.USP_SET_USER_OLS_LABEL TO rl_dba;


PROMPT ==============================================================================
PROMPT 5. Bridge OLS Users (U1-U8) into Application RBAC & VPD
PROMPT ==============================================================================
BEGIN
    -- Insert specific OLS departments if they don't exist
    BEGIN
        INSERT INTO hospital.department (dept_id, dept_name) VALUES ('TH', N'Khoa Tiêu hóa');
        INSERT INTO hospital.department (dept_id, dept_name) VALUES ('TK', N'Khoa Thần kinh');
        INSERT INTO hospital.department (dept_id, dept_name) VALUES ('TM', N'Khoa Tim mạch');
    EXCEPTION WHEN OTHERS THEN NULL; END;

    -- Clear existing dummy records to prevent Unique Constraint errors if rerunning
    DELETE FROM hospital.staff WHERE username_db IN ('U1','U2','U3','U4','U5','U6','U7','U8');

    -- Insert U1-U8 as official staff members
    INSERT INTO hospital.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, staff_role, username_db) 
    VALUES ('NV_U1', N'Giám đốc OLS', N'Nam', TO_DATE('1970-01-01','YYYY-MM-DD'), 'OLS_001', N'Chưa rõ', '0000000000', N'Điều phối viên', 'U1');

    INSERT INTO hospital.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, staff_role, dept_id, username_db) 
    VALUES ('NV_U2', N'Lãnh đạo Tim mạch', N'Nữ', TO_DATE('1980-01-01','YYYY-MM-DD'), 'OLS_002', N'Chưa rõ', '0000000000', N'Bác sĩ', 'TM', 'U2');

    INSERT INTO hospital.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, staff_role, dept_id, username_db) 
    VALUES ('NV_U3', N'Lãnh đạo Thần kinh', N'Nam', TO_DATE('1981-01-01','YYYY-MM-DD'), 'OLS_003', N'Chưa rõ', '0000000000', N'Bác sĩ', 'TK', 'U3');

    INSERT INTO hospital.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, staff_role, dept_id, username_db) 
    VALUES ('NV_U4', N'Nhân viên Thần kinh', N'Nữ', TO_DATE('1990-01-01','YYYY-MM-DD'), 'OLS_004', N'Chưa rõ', '0000000000', N'Kỹ thuật viên', 'TK', 'U4');

    INSERT INTO hospital.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, staff_role, dept_id, username_db) 
    VALUES ('NV_U5', N'Nhân viên Tim mạch', N'Nam', TO_DATE('1992-01-01','YYYY-MM-DD'), 'OLS_005', N'Chưa rõ', '0000000000', N'Kỹ thuật viên', 'TM', 'U5');

    INSERT INTO hospital.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, staff_role, dept_id, username_db) 
    VALUES ('NV_U6', N'Lãnh đạo phòng HCM', N'Nữ', TO_DATE('1985-01-01','YYYY-MM-DD'), 'OLS_006', N'Chưa rõ', '0000000000', N'Bác sĩ', 'TM', 'U6');

    INSERT INTO hospital.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, staff_role, username_db) 
    VALUES ('NV_U7', N'Lãnh đạo phòng Tổng hợp', N'Nam', TO_DATE('1984-01-01','YYYY-MM-DD'), 'OLS_007', N'Chưa rõ', '0000000000', N'Bác sĩ', 'U7');

    INSERT INTO hospital.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, staff_role, dept_id, username_db) 
    VALUES ('NV_U8', N'Nhân viên Tiêu hóa', N'Nữ', TO_DATE('1995-01-01','YYYY-MM-DD'), 'OLS_008', N'Chưa rõ', '0000000000', N'Kỹ thuật viên', 'TH', 'U8');

    COMMIT;
END;
/

PROMPT ==============================================================================
PROMPT 6. Grant Application Roles to U1-U8
PROMPT ==============================================================================
BEGIN
    EXECUTE IMMEDIATE 'GRANT rl_coordinator TO U1';
    
    EXECUTE IMMEDIATE 'GRANT rl_doctor TO U2';
    EXECUTE IMMEDIATE 'GRANT rl_doctor TO U3';
    EXECUTE IMMEDIATE 'GRANT rl_doctor TO U6';
    EXECUTE IMMEDIATE 'GRANT rl_doctor TO U7';
    
    EXECUTE IMMEDIATE 'GRANT rl_technician TO U4';
    EXECUTE IMMEDIATE 'GRANT rl_technician TO U5';
    EXECUTE IMMEDIATE 'GRANT rl_technician TO U8';
END;
/

PROMPT ==============================================================================
PROMPT DONE: OLS Setup is completely finished and integrated with the UI!
PROMPT ==============================================================================

