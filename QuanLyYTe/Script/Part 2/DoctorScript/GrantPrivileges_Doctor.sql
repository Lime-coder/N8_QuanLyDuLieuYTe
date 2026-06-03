-- Chạy bằng hospital_dba
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

-- 1. Định nghĩa lại USP_GET_SESSION_ROLE (BẮT BUỘC phải có AUTHID CURRENT_USER)
CREATE OR REPLACE PROCEDURE USP_GET_SESSION_ROLE (
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS 
BEGIN
    OPEN p_cursor FOR
        -- Lấy role có tiền tố RL_ đang hoạt động trong session
        SELECT ROLE FROM SESSION_ROLES 
        WHERE ROLE LIKE 'RL_%' AND ROWNUM = 1;
END USP_GET_SESSION_ROLE;
/

-- 2. Định nghĩa lại USP_GET_GRANTED_ROLE
CREATE OR REPLACE PROCEDURE USP_GET_GRANTED_ROLE (
    p_user   IN  VARCHAR2,
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT GRANTED_ROLE FROM DBA_ROLE_PRIVS
        WHERE GRANTEE = UPPER(p_user) AND GRANTED_ROLE LIKE 'RL_%' AND ROWNUM = 1;
END USP_GET_GRANTED_ROLE;
/

-- 3. Cấp quyền thực thi cho các Role (Để Bác sĩ thấy được procedure)
GRANT EXECUTE ON hospital_dba.USP_GET_SESSION_ROLE TO rl_doctor;
GRANT EXECUTE ON hospital_dba.USP_GET_GRANTED_ROLE TO rl_doctor;

COMMIT;