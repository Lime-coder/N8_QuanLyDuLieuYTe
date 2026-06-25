-- ==============================================================================
-- 05_ols_tests.sql
-- Chạy dưới quyền: hospital_dba (sau khi các nhãn của HOSP_OLS_POL đã được tạo)
-- Mục đích: Tạo danh sách các user đặc biệt (U1 đến U8) nhằm phục vụ cho 
-- việc demo, kiểm thử (test) cơ chế OLS trong lúc vấn đáp, báo cáo.
-- ==============================================================================
ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

SET SERVEROUTPUT ON;

-- 1. Chuẩn bị dữ liệu nhân sự (bảng Staff) cho các tài khoản test U1 - U8
BEGIN
    DELETE FROM hospital_dba.staff WHERE username_db IN ('U1','U2','U3','U4','U5','U6','U7','U8');

    -- U1: Giám đốc OLS (Không thuộc khoa nào, quản lý toàn bộ)
    INSERT INTO hospital_dba.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, dept_id, staff_role, username_db, facility)
    VALUES ('NV_U1', UNISTR('Gi\00E1m \0111\1ED1c OLS'), 'Nam', TO_DATE('1970-01-01','YYYY-MM-DD'), 'OLS_001', UNISTR('Ch\01B0a r\00F5'), '0000000000', NULL, UNISTR('\0110i\1EC1u ph\1ED1i vi\00EAn'), 'U1', UNISTR('H\1ED3 Ch\00ED Minh'));

    -- U2: Lãnh đạo khoa Tim Mạch ở HCM
    INSERT INTO hospital_dba.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, dept_id, staff_role, username_db, facility)
    VALUES ('NV_U2', UNISTR('L\00E3nh \0111\1EA1o Tim m\1EA1ch'), UNISTR('N\1EEF'), TO_DATE('1980-01-01','YYYY-MM-DD'), 'OLS_002', UNISTR('Ch\01B0a r\00F5'), '0000000000', 'PB01', UNISTR('B\00E1c s\0129'), 'U2', UNISTR('H\1ED3 Ch\00ED Minh'));

    -- U3: Lãnh đạo khoa Thần Kinh ở HN
    INSERT INTO hospital_dba.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, dept_id, staff_role, username_db, facility)
    VALUES ('NV_U3', UNISTR('L\00E3nh \0111\1EA1o Th\1EA7n kinh'), 'Nam', TO_DATE('1981-01-01','YYYY-MM-DD'), 'OLS_003', UNISTR('Ch\01B0a r\00F5'), '0000000000', 'PB02', UNISTR('B\00E1c s\0129'), 'U3', UNISTR('H\00E0 N\1ED9i'));

    -- U4: Nhân viên khoa Thần Kinh ở HCM
    INSERT INTO hospital_dba.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, dept_id, staff_role, username_db, facility)
    VALUES ('NV_U4', UNISTR('Nh\00E2n vi\00EAn Th\1EA7n kinh'), UNISTR('N\1EEF'), TO_DATE('1990-01-01','YYYY-MM-DD'), 'OLS_004', UNISTR('Ch\01B0a r\00F5'), '0000000000', 'PB02', UNISTR('K\1EF9 thu\1EADt vi\00EAn'), 'U4', UNISTR('H\1ED3 Ch\00ED Minh'));

    -- U5: Nhân viên khoa Tim Mạch ở HCM
    INSERT INTO hospital_dba.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, dept_id, staff_role, username_db, facility)
    VALUES ('NV_U5', UNISTR('Nh\00E2n vi\00EAn Tim m\1EA1ch'), 'Nam', TO_DATE('1992-01-01','YYYY-MM-DD'), 'OLS_005', UNISTR('Ch\01B0a r\00F5'), '0000000000', 'PB01', UNISTR('K\1EF9 thu\1EADt vi\00EAn'), 'U5', UNISTR('H\1ED3 Ch\00ED Minh'));

    -- U6: Lãnh đạo khoa Tim Mạch ở HCM
    INSERT INTO hospital_dba.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, dept_id, staff_role, username_db, facility)
    VALUES ('NV_U6', UNISTR('L\00E3nh \0111\1EA1o ph\00F2ng HCM'), UNISTR('N\1EEF'), TO_DATE('1985-01-01','YYYY-MM-DD'), 'OLS_006', UNISTR('Ch\01B0a r\00F5'), '0000000000', 'PB01', UNISTR('B\00E1c s\0129'), 'U6', UNISTR('H\1ED3 Ch\00ED Minh'));

    -- U7: Lãnh đạo tổng hợp (Quản lý đa khoa) ở Hải Phòng
    INSERT INTO hospital_dba.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, dept_id, staff_role, username_db, facility)
    VALUES ('NV_U7', UNISTR('L\00E3nh \0111\1EA1o ph\00F2ng T\1ED5ng h\1EE3p'), 'Nam', TO_DATE('1984-01-01','YYYY-MM-DD'), 'OLS_007', UNISTR('Ch\01B0a r\00F5'), '0000000000', NULL, UNISTR('B\00E1c s\0129'), 'U7', UNISTR('H\1EA3i Ph\00F2ng'));

    -- U8: Nhân viên khoa Tiêu Hóa ở Hà Nội
    INSERT INTO hospital_dba.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, dept_id, staff_role, username_db, facility)
    VALUES ('NV_U8', UNISTR('Nh\00E2n vi\00EAn Ti\00EAu h\00F3a'), UNISTR('N\1EEF'), TO_DATE('1995-01-01','YYYY-MM-DD'), 'OLS_008', UNISTR('Ch\01B0a r\00F5'), '0000000000', 'PB03', UNISTR('K\1EF9 thu\1EADt vi\00EAn'), 'U8', UNISTR('H\00E0 N\1ED9i'));

    COMMIT;
END;
/

-- 2. Khởi tạo Database Users và gán quyền RBAC (Role)
DECLARE
    -- Hàm hỗ trợ bỏ qua lỗi nếu user đã tồn tại (exception -01920)
    PROCEDURE exec_ignore(p_sql IN VARCHAR2) IS
    BEGIN
        EXECUTE IMMEDIATE p_sql;
    EXCEPTION WHEN OTHERS THEN
        IF SQLCODE NOT IN (-01920, -01917, -01918, -01919) THEN
            DBMS_OUTPUT.PUT_LINE('[WARN] ' || p_sql || ' -> ' || SQLERRM);
        END IF;
    END;
BEGIN
    FOR i IN 1..8 LOOP
        exec_ignore('CREATE USER U' || i || ' IDENTIFIED BY "123"');
        exec_ignore('GRANT CREATE SESSION TO U' || i);
        exec_ignore('GRANT EXECUTE ON hospital_dba.USP_GET_SESSION_ROLE TO U' || i);
        exec_ignore('GRANT EXECUTE ON hospital_dba.USP_GET_USER_ID TO U' || i);
    END LOOP;

    -- Gán Role (RBAC) để xác định quyền thao tác với các Object / View trong hệ thống
    exec_ignore('GRANT rl_coordinator TO U1');
    exec_ignore('GRANT rl_doctor TO U2');
    exec_ignore('GRANT rl_doctor TO U3');
    exec_ignore('GRANT rl_technician TO U4');
    exec_ignore('GRANT rl_technician TO U5');
    exec_ignore('GRANT rl_doctor TO U6');
    exec_ignore('GRANT rl_doctor TO U7');
    exec_ignore('GRANT rl_technician TO U8');

    -- Đặt các Role làm Default để người dùng có quyền ngay khi đăng nhập
    FOR i IN 1..8 LOOP
        exec_ignore('ALTER USER U' || i || ' DEFAULT ROLE ALL');
    END LOOP;
END;
/

-- 3. Gán Nhãn OLS cho các user test
-- Đây là bước gán nhãn tĩnh (hard-code) giúp việc chấm thi / demo nhanh hơn so với việc gọi script 03
BEGIN
    -- U1 có cấp cao nhất (Ban Giám Đốc), xem được mọi khoa, mọi chi nhánh
    SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', 'U1', 'BGD:TH,TK,TM:HCM,HP,HN');
    -- U2 xem được khoa Tim Mạch ở HCM, cấp Lãnh đạo khoa
    SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', 'U2', 'LDK:TM:HCM');
    -- U3 xem được khoa Thần Kinh ở HN, cấp Lãnh đạo khoa
    SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', 'U3', 'LDK:TK:HN');
    -- U4 xem được khoa Thần Kinh ở HCM, cấp Nhân viên
    SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', 'U4', 'NV:TK:HCM');
    -- U5 xem được khoa Tim Mạch ở HCM, cấp Nhân viên
    SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', 'U5', 'NV:TM:HCM');
    -- U6 xem được khoa Tim Mạch ở HCM, cấp Lãnh đạo khoa
    SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', 'U6', 'LDK:TM:HCM');
    -- U7 là lãnh đạo tổng hợp, xem mọi khoa, mọi chi nhánh
    SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', 'U7', 'LDK:TH,TK,TM:HCM,HP,HN');
    -- U8 xem được khoa Tiêu hóa ở HN, cấp Nhân viên
    SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', 'U8', 'NV:TH:HN');
END;
/


