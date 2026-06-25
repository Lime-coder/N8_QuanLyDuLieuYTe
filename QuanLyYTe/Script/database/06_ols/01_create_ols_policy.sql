-- ==============================================================================
-- 01_create_ols_policy.sql
-- Chạy dưới quyền: SYS AS SYSDBA
-- Mục đích: Tạo bảng thông báo và khởi tạo Chính sách OLS (OLS Policy)
-- ==============================================================================
ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

-- Cho phép LBACSYS kế thừa các quyền của SYS để có thể tạo policy trên schema khác
GRANT INHERIT PRIVILEGES ON USER sys TO lbacsys;

-- 1. Xóa bảng và Sequence cũ
BEGIN EXECUTE IMMEDIATE 'DROP TABLE hospital_dba.notification CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP SEQUENCE hospital_dba.seq_notification_id'; EXCEPTION WHEN OTHERS THEN NULL; END;
/

-- Tạo bảng notification (Thông báo). Lưu ý bảng này chưa có cột lưu nhãn OLS.
-- Cột nhãn sẽ được OLS tự động quản lý hoặc ánh xạ khi ta Apply Policy.
CREATE TABLE hospital_dba.notification (
    notification_id VARCHAR2(10) PRIMARY KEY,
    description     NVARCHAR2(1000) NOT NULL,
    posted_date     DATE NOT NULL,
    location        NVARCHAR2(100)
);

CREATE SEQUENCE hospital_dba.seq_notification_id START WITH 8 INCREMENT BY 1;

-- Cấp quyền cơ bản trên bảng thông báo cho tài khoản admin
GRANT SELECT, INSERT, UPDATE, DELETE ON hospital_dba.notification TO hospital_dba WITH GRANT OPTION;
GRANT SELECT ON hospital_dba.seq_notification_id TO hospital_dba WITH GRANT OPTION;

-- 2. Khởi tạo Chính sách OLS (Policy)
BEGIN SA_SYSDBA.DROP_POLICY('HOSP_OLS_POL', TRUE); EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN
    -- Tạo policy có tên HOSP_OLS_POL
    -- column_name: Cột OLS_LABEL sẽ được tự động thêm vào bảng để lưu trữ nhãn dưới dạng chuỗi số ẩn.
    -- default_options:
    --   - READ_CONTROL: Áp dụng OLS khi SELECT (User phải có nhãn >= nhãn của dòng dữ liệu)
    --   - WRITE_CONTROL: Áp dụng OLS khi INSERT, UPDATE, DELETE
    --   - CHECK_CONTROL: Ngăn user ghi dòng dữ liệu với nhãn mà họ không có quyền đọc (chặn việc giấu dữ liệu)
    SA_SYSDBA.CREATE_POLICY(
        policy_name     => 'HOSP_OLS_POL',
        column_name     => 'OLS_LABEL',
        default_options => 'READ_CONTROL, WRITE_CONTROL, CHECK_CONTROL'
    );
END;
/

-- Cấp Role quản trị của riêng Policy này cho admin. 
-- Role này tự động được Oracle sinh ra khi tạo Policy: <policy_name>_DBA
GRANT HOSP_OLS_POL_DBA TO hospital_dba;

BEGIN
    -- Thiết lập đặc quyền OLS (Privileges) cho admin (HOSPITAL_DBA)
    -- FULL: Có toàn quyền bỏ qua các kiểm tra OLS đối với Policy này.
    -- PROFILE_ACCESS: Cho phép người này thay đổi nhãn/quyền của user khác.
    SA_USER_ADMIN.SET_USER_PRIVS(
        policy_name => 'HOSP_OLS_POL',
        user_name   => 'HOSPITAL_DBA',
        privileges  => 'FULL,PROFILE_ACCESS'
    );
END;
/


