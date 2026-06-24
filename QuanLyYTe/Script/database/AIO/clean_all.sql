-- ==============================================================================
-- Thư mục AIO - Script Xóa Toàn Bộ (Clean All)
-- Chú ý: Script này sẽ xóa TOÀN BỘ dữ liệu, người dùng, policy và cấu hình
-- ==============================================================================
SET ECHO OFF
SET VERIFY OFF
SET SERVEROUTPUT ON

PROMPT ====================================================================
PROMPT = QLYT DATABASE CLEANUP SCRIPT (AIO)                               =
PROMPT ====================================================================
PROMPT Canh bao: Hanh dong nay se XOA TOAN BO DU LIEU CSDL QLYT!
PROMPT Nhap mat khau SYS de xac nhan va tien hanh xoa:

ACCEPT sys_password CHAR PROMPT 'Enter SYS password: ' HIDE

SPOOL AIO_clean_log.txt

SET DEFINE ON
CONNECT sys/"&sys_password"@localhost:1521/PDB_QLYT AS SYSDBA
SET DEFINE OFF
SET SERVEROUTPUT ON

PROMPT ====================================================================
PROMPT = 1. XOA OLS POLICY (HOSP_OLS_POL)                                 =
PROMPT ====================================================================
BEGIN 
    EXECUTE IMMEDIATE 'GRANT INHERIT PRIVILEGES ON USER sys TO lbacsys';
    LBACSYS.SA_SYSDBA.DROP_POLICY('HOSP_OLS_POL', TRUE); 
    DBMS_OUTPUT.PUT_LINE('Da xoa OLS Policy HOSP_OLS_POL');
EXCEPTION 
    WHEN OTHERS THEN 
        DBMS_OUTPUT.PUT_LINE('Khong co OLS Policy (Bo qua) hoac loi: ' || SQLERRM); 
END;
/

PROMPT ====================================================================
PROMPT = 2. XOA TAT CA USER BAC SI, BENH NHAN, KTV, DIEU PHOI VIEN        =
PROMPT ====================================================================
BEGIN
    FOR r IN (
        SELECT grantee FROM dba_role_privs 
        WHERE granted_role IN ('RL_PATIENT', 'RL_DOCTOR', 'RL_TECHNICIAN', 'RL_COORDINATOR')
    ) LOOP
        BEGIN
            EXECUTE IMMEDIATE 'DROP USER ' || r.grantee || ' CASCADE';
            DBMS_OUTPUT.PUT_LINE('Da xoa user: ' || r.grantee);
        EXCEPTION WHEN OTHERS THEN NULL;
        END;
    END LOOP;
END;
/

PROMPT ====================================================================
PROMPT = 3. XOA SCHEMA HOSPITAL_DBA VA HOSPITAL                           =
PROMPT ====================================================================
BEGIN 
    EXECUTE IMMEDIATE 'DROP USER hospital_dba CASCADE'; 
    DBMS_OUTPUT.PUT_LINE('Da xoa user hospital_dba');
EXCEPTION 
    WHEN OTHERS THEN DBMS_OUTPUT.PUT_LINE('User hospital_dba chua ton tai.'); 
END;
/

BEGIN 
    EXECUTE IMMEDIATE 'DROP USER hospital CASCADE'; 
    DBMS_OUTPUT.PUT_LINE('Da xoa user hospital');
EXCEPTION 
    WHEN OTHERS THEN DBMS_OUTPUT.PUT_LINE('User hospital chua ton tai.'); 
END;
/

PROMPT ====================================================================
PROMPT = 4. XOA TAT CA ROLE DA TAO                                        =
PROMPT ====================================================================
BEGIN
    FOR r IN (
        SELECT role FROM dba_roles 
        WHERE role IN ('RL_PATIENT', 'RL_DOCTOR', 'RL_TECHNICIAN', 'RL_COORDINATOR', 'RL_DBA')
    ) LOOP
        EXECUTE IMMEDIATE 'DROP ROLE ' || r.role;
        DBMS_OUTPUT.PUT_LINE('Da xoa role: ' || r.role);
    END LOOP;
EXCEPTION 
    WHEN OTHERS THEN 
        DBMS_OUTPUT.PUT_LINE('Loi khi xoa Role: ' || SQLERRM);
END;
/

PROMPT ====================================================================
PROMPT = HOAN TAT QUA TRINH XOA (CLEANUP COMPLETE)!                       =
PROMPT = Kiem tra log tai AIO_clean_log.txt                               =
PROMPT ====================================================================
SPOOL OFF
EXIT;
