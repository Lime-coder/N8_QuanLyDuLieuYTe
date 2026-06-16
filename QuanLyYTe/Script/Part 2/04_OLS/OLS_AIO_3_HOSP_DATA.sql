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
    DELETE FROM hospital.thongbao;
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

INSERT INTO hospital.thongbao (id_thongbao, noidung, ngaygio, diadiem, ols_label) 
VALUES ('T1', N'Thông báo chung toàn viện', SYSDATE, N'Toàn quốc', CHAR_TO_LABEL('HOSP_OLS_POL', 'NV'));

INSERT INTO hospital.thongbao (id_thongbao, noidung, ngaygio, diadiem, ols_label) 
VALUES ('T2', N'Họp chiến lược Quý 3', SYSDATE, N'Phòng họp VIP', CHAR_TO_LABEL('HOSP_OLS_POL', 'BGD'));

INSERT INTO hospital.thongbao (id_thongbao, noidung, ngaygio, diadiem, ols_label) 
VALUES ('T3', N'Đánh giá ngân sách các khoa', SYSDATE, N'Hội trường A', CHAR_TO_LABEL('HOSP_OLS_POL', 'LDK'));

INSERT INTO hospital.thongbao (id_thongbao, noidung, ngaygio, diadiem, ols_label) 
VALUES ('T4', N'Báo cáo thiết bị nội soi', SYSDATE, N'Phòng Giám đốc', CHAR_TO_LABEL('HOSP_OLS_POL', 'LDK:TH'));

INSERT INTO hospital.thongbao (id_thongbao, noidung, ngaygio, diadiem, ols_label) 
VALUES ('T5', N'Thay đổi ca trực tuần tới', SYSDATE, N'Khoa TH - HCM', CHAR_TO_LABEL('HOSP_OLS_POL', 'NV:TH:HCM'));

INSERT INTO hospital.thongbao (id_thongbao, noidung, ngaygio, diadiem, ols_label) 
VALUES ('T6', N'Tập huấn an toàn vệ sinh', SYSDATE, N'Khoa TH - HN', CHAR_TO_LABEL('HOSP_OLS_POL', 'NV:TH:HN'));

INSERT INTO hospital.thongbao (id_thongbao, noidung, ngaygio, diadiem, ols_label) 
VALUES ('T7', N'Họp liên khoa TH & TK', SYSDATE, N'Chi nhánh Hải Phòng', CHAR_TO_LABEL('HOSP_OLS_POL', 'LDK:TH,TK:HP'));

COMMIT;

PROMPT ==============================================================================
PROMPT 2. Build Stored Procedure for Application
PROMPT ==============================================================================
CREATE OR REPLACE PROCEDURE hospital.USP_GET_NOTIFICATIONS (
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT id_thongbao, noidung, TO_CHAR(ngaygio, 'DD/MM/YYYY HH24:MI') as ngaygio, diadiem
        FROM hospital.thongbao
        ORDER BY id_thongbao;
END;
/

PROMPT ==============================================================================
PROMPT 3. Grant Execute Privileges for OLS and Login Requirements
PROMPT ==============================================================================
DECLARE
BEGIN
    FOR i IN 1..8 LOOP
        -- Quyền cho chức năng xem thông báo OLS
        EXECUTE IMMEDIATE 'GRANT EXECUTE ON hospital.USP_GET_NOTIFICATIONS TO U' || i;
        
        -- Quyền cho cơ chế Đăng nhập WinForm
        EXECUTE IMMEDIATE 'GRANT EXECUTE ON hospital_dba.USP_GET_SESSION_ROLE TO U' || i;
        EXECUTE IMMEDIATE 'GRANT EXECUTE ON hospital_dba.USP_GET_USER_ID TO U' || i;
    END LOOP;
END;
/

PROMPT ==============================================================================
PROMPT 4. Bridge OLS Users (U1-U8) into Application RBAC & VPD
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
PROMPT 5. Grant Application Roles to U1-U8
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