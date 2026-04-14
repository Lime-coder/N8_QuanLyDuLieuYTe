-- Sử dụng database XEPDB1
ALTER SESSION SET CONTAINER = PDB_QLYT;

-- Mockdata (execute with hospital_dba)
-- 1. Departments
INSERT INTO department (dept_id, dept_name) VALUES ('PB01', N'Nội tổng quát');
INSERT INTO department (dept_id, dept_name) VALUES ('PB02', N'Ngoại thần kinh');
INSERT INTO department (dept_id, dept_name) VALUES ('PB03', N'Chẩn đoán hình ảnh');

-- 2. Staff (Lưu ý staff_role phải khớp với CHECK constraint)
INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, phone, dept_id, staff_role, username_db)
VALUES ('NV001', N'Nguyễn Văn Minh', N'Nam', TO_DATE('1990-01-01','YYYY-MM-DD'), '079012345678', '0901112223', 'PB01', N'Bác sĩ', 'NV001');

INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, phone, dept_id, staff_role, username_db)
VALUES ('NV002', N'Trần Thị Mai', N'Nữ', TO_DATE('1992-05-15','YYYY-MM-DD'), '079012345679', '0904445556', 'PB01', N'Điều phối viên', 'NV002');

INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, phone, dept_id, staff_role, username_db)
VALUES ('NV003', N'Lê Văn Tám', N'Nam', TO_DATE('1988-12-10','YYYY-MM-DD'), '079012345680', '0907778889', 'PB03', N'Kỹ thuật viên', 'NV003');

INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, phone, dept_id, staff_role, username_db)
VALUES ('NV004', N'Phạm minh Anh', N'Nữ', TO_DATE('1995-08-20','YYYY-MM-DD'), '079012345681', '0909990001', 'PB02', N'Bác sĩ', 'NV004');

-- 3. Patients (Username_db ngẫu nhiên cho bảo mật)
INSERT INTO patient (patient_id, full_name, gender, birthdate, id_card, house_no, street, district, city_province, username_db)
SELECT 'BN' || LPAD(LEVEL, 3, '0'), 
       N'Bệnh nhân ' || LEVEL, 
       CASE WHEN MOD(LEVEL, 2) = 0 THEN N'Nữ' ELSE N'Nam' END,
       TO_DATE('1980-01-01','YYYY-MM-DD') + (LEVEL * 365),
       '001080' || LPAD(LEVEL, 6, '0'),
       TO_CHAR(LEVEL * 10), 
       N'Đường số ' || LEVEL, 
       N'Quận ' || MOD(LEVEL, 10), 
       N'TP. Hồ Chí Minh',
       'USR_' || DBMS_RANDOM.STRING('X', 6)
FROM DUAL CONNECT BY LEVEL <= 13;

-- 4. Medical Records mẫu
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion)
VALUES ('BA001', 'BN001', SYSDATE - 5, N'Viêm dạ dày cấp', N'Uống thuốc sau ăn', 'NV001', 'PB01', N'Tiếp tục theo dõi');

INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion)
VALUES ('BA002', 'BN002', SYSDATE - 2, N'Đau đầu mãn tính', N'Nghỉ ngơi, giảm áp lực', 'NV004', 'PB02', N'Tái khám sau 1 tuần');

-- 5. Prescriptions
INSERT INTO prescription (record_id, prescription_date, medicine_name, dosage)
VALUES ('BA001', SYSDATE - 5, 'Omeprazole', N'20mg, 1 viên/ngày');

-- 6. Service Records
INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result)
VALUES ('BA002', N'Chụp MRI não', SYSDATE - 2, 'NV003', N'Hình ảnh bình thường, không có khối u');

COMMIT;


/*
-- delete data from every table
-- disable constraints
ALTER TABLE service_record DISABLE CONSTRAINT fk_sr_record;
ALTER TABLE service_record DISABLE CONSTRAINT fk_sr_staff;
ALTER TABLE prescription DISABLE CONSTRAINT fk_presc_record;
ALTER TABLE medical_record DISABLE CONSTRAINT fk_mr_patient;
ALTER TABLE medical_record DISABLE CONSTRAINT fk_mr_staff;
ALTER TABLE medical_record DISABLE CONSTRAINT fk_mr_dept;
ALTER TABLE staff DISABLE CONSTRAINT fk_staff_dept;

-- truncate everything
TRUNCATE TABLE prescription;
TRUNCATE TABLE service_record;
TRUNCATE TABLE medical_record;
TRUNCATE TABLE patient;
TRUNCATE TABLE staff;
TRUNCATE TABLE department;

-- enable constraints back
ALTER TABLE staff ENABLE CONSTRAINT fk_staff_dept;
ALTER TABLE medical_record ENABLE CONSTRAINT fk_mr_patient;
ALTER TABLE medical_record ENABLE CONSTRAINT fk_mr_staff;
ALTER TABLE medical_record ENABLE CONSTRAINT fk_mr_dept;
ALTER TABLE service_record ENABLE CONSTRAINT fk_sr_record;
ALTER TABLE service_record ENABLE CONSTRAINT fk_sr_staff;
ALTER TABLE prescription ENABLE CONSTRAINT fk_presc_record;
*/

-- ════════════════════════════════════════════════════════════════
-- SECURITY ADMIN STORED PROCEDURES
-- Quản lý Users & Roles trong Oracle
-- ════════════════════════════════════════════════════════════════

-- Notes:
-- - Các thao tác CREATE/DROP/ALTER user/role bắt buộc dùng dynamic SQL.
-- - Tuyệt đối không ghép chuỗi identifier "trần": dùng DBMS_ASSERT để giảm rủi ro injection.
-- - Password được đặt trong dấu "..." để giữ nguyên ký tự; có escape dấu " bằng "".

-- ──────────────────────────────────────────────────────────────────
-- 1. SP: GET_ALL_USERS - Lấy danh sách tất cả users
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE GET_ALL_USERS (
    p_cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_cursor FOR
        SELECT USERNAME, ACCOUNT_STATUS, LOCK_DATE, CREATED
        FROM DBA_USERS
        ORDER BY USERNAME;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20001, 'Lỗi lấy danh sách users: ' || SQLERRM);
END GET_ALL_USERS;
/

-- ──────────────────────────────────────────────────────────────────
-- 2. SP: GET_ALL_ROLES - Lấy danh sách tất cả roles
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE GET_ALL_ROLES (
    p_cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_cursor FOR
        SELECT ROLE, PASSWORD_REQUIRED, AUTHENTICATION_TYPE, COMMON, ORACLE_MAINTAINED
        FROM DBA_ROLES
        ORDER BY ROLE;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20002, 'Lỗi lấy danh sách roles: ' || SQLERRM);
END GET_ALL_ROLES;
/

-- ──────────────────────────────────────────────────────────────────
-- 3. SP: CREATE_NEW_USER - Tạo user mới
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE CREATE_NEW_USER (
    p_username IN VARCHAR2,
    p_password IN VARCHAR2
)
IS
    v_sql VARCHAR2(1000);
    v_user VARCHAR2(128);
    v_pwd  VARCHAR2(4000);
BEGIN
    -- Validate input
    IF TRIM(p_username) IS NULL OR TRIM(p_password) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20003, 'Username và password không được để trống.');
    END IF;

    v_user := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_username)));
    v_pwd  := REPLACE(p_password, '"', '""');
    
    -- Create user
    v_sql := 'CREATE USER ' || v_user || ' IDENTIFIED BY "' || v_pwd || '"';
    EXECUTE IMMEDIATE v_sql;
    
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20004, 'Lỗi tạo user: ' || SQLERRM);
END CREATE_NEW_USER;
/

-- ──────────────────────────────────────────────────────────────────
-- 4. SP: ALTER_USER_PASSWORD - Đổi mật khẩu user
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE ALTER_USER_PASSWORD (
    p_username IN VARCHAR2,
    p_new_password IN VARCHAR2
)
IS
    v_sql VARCHAR2(1000);
    v_user VARCHAR2(128);
    v_pwd  VARCHAR2(4000);
BEGIN
    IF TRIM(p_username) IS NULL OR TRIM(p_new_password) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20005, 'Username và password không được để trống.');
    END IF;

    v_user := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_username)));
    v_pwd  := REPLACE(p_new_password, '"', '""');
    
    v_sql := 'ALTER USER ' || v_user || ' IDENTIFIED BY "' || v_pwd || '"';
    EXECUTE IMMEDIATE v_sql;
    
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20006, 'Lỗi đổi mật khẩu: ' || SQLERRM);
END ALTER_USER_PASSWORD;
/

-- ──────────────────────────────────────────────────────────────────
-- 5. SP: LOCK_USER - Khóa tài khoản user
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE LOCK_USER (
    p_username IN VARCHAR2
)
IS
    v_sql VARCHAR2(500);
    v_user VARCHAR2(128);
BEGIN
    IF TRIM(p_username) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20007, 'Username không được để trống.');
    END IF;

    v_user := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_username)));
    
    v_sql := 'ALTER USER ' || v_user || ' ACCOUNT LOCK';
    EXECUTE IMMEDIATE v_sql;
    
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20008, 'Lỗi khóa user: ' || SQLERRM);
END LOCK_USER;
/

-- ──────────────────────────────────────────────────────────────────
-- 6. SP: UNLOCK_USER - Mở khóa tài khoản user
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE UNLOCK_USER (
    p_username IN VARCHAR2
)
IS
    v_sql VARCHAR2(500);
    v_user VARCHAR2(128);
BEGIN
    IF TRIM(p_username) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20009, 'Username không được để trống.');
    END IF;

    v_user := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_username)));
    
    v_sql := 'ALTER USER ' || v_user || ' ACCOUNT UNLOCK';
    EXECUTE IMMEDIATE v_sql;
    
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20010, 'Lỗi mở khóa user: ' || SQLERRM);
END UNLOCK_USER;
/

-- ──────────────────────────────────────────────────────────────────
-- 7. SP: DROP_USER - Xóa user
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE DROP_USER (
    p_username IN VARCHAR2,
    p_cascade IN VARCHAR2 DEFAULT 'YES'
)
IS
    v_sql VARCHAR2(500);
    v_user VARCHAR2(128);
BEGIN
    IF TRIM(p_username) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20011, 'Username không được để trống.');
    END IF;

    v_user := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_username)));
    
    IF UPPER(p_cascade) = 'YES' THEN
        v_sql := 'DROP USER ' || v_user || ' CASCADE';
    ELSE
        v_sql := 'DROP USER ' || v_user;
    END IF;
    
    EXECUTE IMMEDIATE v_sql;
    
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20012, 'Lỗi xóa user: ' || SQLERRM);
END DROP_USER;
/

-- ──────────────────────────────────────────────────────────────────
-- 8. SP: CREATE_NEW_ROLE - Tạo role mới
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE CREATE_NEW_ROLE (
    p_role_name IN VARCHAR2,
    p_password IN VARCHAR2 DEFAULT NULL
)
IS
    v_sql VARCHAR2(1000);
    v_role VARCHAR2(128);
    v_pwd  VARCHAR2(4000);
BEGIN
    IF TRIM(p_role_name) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20013, 'Tên role không được để trống.');
    END IF;

    v_role := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_role_name)));
    
    IF TRIM(p_password) IS NULL THEN
        v_sql := 'CREATE ROLE ' || v_role;
    ELSE
        v_pwd := REPLACE(p_password, '"', '""');
        v_sql := 'CREATE ROLE ' || v_role || ' IDENTIFIED BY "' || v_pwd || '"';
    END IF;
    
    EXECUTE IMMEDIATE v_sql;
    
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20014, 'Lỗi tạo role: ' || SQLERRM);
END CREATE_NEW_ROLE;
/

-- ──────────────────────────────────────────────────────────────────
-- 9. SP: ALTER_ROLE_PASSWORD - Đổi mật khẩu role
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE ALTER_ROLE_PASSWORD (
    p_role_name IN VARCHAR2,
    p_password IN VARCHAR2 DEFAULT NULL
)
IS
    v_sql VARCHAR2(1000);
    v_role VARCHAR2(128);
    v_pwd  VARCHAR2(4000);
BEGIN
    IF TRIM(p_role_name) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20015, 'Tên role không được để trống.');
    END IF;

    v_role := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_role_name)));
    
    IF TRIM(p_password) IS NULL THEN
        v_sql := 'ALTER ROLE ' || v_role || ' NOT IDENTIFIED';
    ELSE
        v_pwd := REPLACE(p_password, '"', '""');
        v_sql := 'ALTER ROLE ' || v_role || ' IDENTIFIED BY "' || v_pwd || '"';
    END IF;
    
    EXECUTE IMMEDIATE v_sql;
    
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20016, 'Lỗi cập nhật role: ' || SQLERRM);
END ALTER_ROLE_PASSWORD;
/

-- ──────────────────────────────────────────────────────────────────
-- 10. SP: DROP_ROLE - Xóa role
-- ──────────────────────────────────────────────────────────────────
CREATE OR REPLACE PROCEDURE DROP_ROLE (
    p_role_name IN VARCHAR2
)
IS
    v_sql VARCHAR2(500);
    v_role VARCHAR2(128);
BEGIN
    IF TRIM(p_role_name) IS NULL THEN
        RAISE_APPLICATION_ERROR(-20017, 'Tên role không được để trống.');
    END IF;

    v_role := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_role_name)));
    
    v_sql := 'DROP ROLE ' || v_role;
    EXECUTE IMMEDIATE v_sql;
    
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20018, 'Lỗi xóa role: ' || SQLERRM);
END DROP_ROLE;
/

-- ════════════════════════════════════════════════════════════════
-- COMMIT
-- ════════════════════════════════════════════════════════════════
COMMIT;
