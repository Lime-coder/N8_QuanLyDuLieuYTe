-- ==============================================================================
-- 04_create_db_accounts_for_seeded_data.sql
-- Cháº¡y dÆ°á»›i quyá»n: SYS AS SYSDBA
-- Má»¥c Ä‘Ã­ch: Táº¡o Oracle Database Login Account cho toÃ n bá»™ dá»¯ liá»‡u máº«u (Seed Data)
--           Bao gá»“m cáº£ U1 -> U8 vÃ  toÃ n bá»™ NV000xxx, BN000xxx
-- ==============================================================================
ALTER SESSION SET CONTAINER = PDB_QLYT;

PROMPT ====================================================================
PROMPT Tao tai khoan Database thuc te cho Toan bo Nhan vien
PROMPT ====================================================================
DECLARE
    v_sql VARCHAR2(1000);
BEGIN
    FOR r IN (SELECT username_db, staff_role FROM hospital.staff WHERE username_db IN ('NV000001', 'NV000021', 'NV000121', 'U1', 'U2', 'U3', 'U4', 'U5', 'U6', 'U7', 'U8')) LOOP
        BEGIN
            v_sql := 'CREATE USER ' || r.username_db || ' IDENTIFIED BY "123"';
            EXECUTE IMMEDIATE v_sql;
        EXCEPTION WHEN OTHERS THEN NULL; -- Bá» qua náº¿u user Ä‘Ã£ tá»“n táº¡i
        END;
        
        BEGIN
            
            v_sql := 'GRANT CREATE SESSION TO ' || r.username_db;
            EXECUTE IMMEDIATE v_sql;
            
            v_sql := 'GRANT EXECUTE ON hospital_dba.USP_GET_SESSION_ROLE TO ' || r.username_db;
            EXECUTE IMMEDIATE v_sql;
            
            v_sql := 'GRANT EXECUTE ON hospital_dba.USP_GET_USER_ID TO ' || r.username_db;
            EXECUTE IMMEDIATE v_sql;
            
            IF r.staff_role = UNISTR('B\00E1c s\0129') THEN
                v_sql := 'GRANT rl_doctor TO ' || r.username_db;
            ELSIF r.staff_role = UNISTR('\0110i\1EC1u ph\1ED1i vi\00EAn') THEN
                v_sql := 'GRANT rl_coordinator TO ' || r.username_db;
            ELSIF r.staff_role = UNISTR('K\1EF9 thu\1EADt vi\00EAn') THEN
                v_sql := 'GRANT rl_technician TO ' || r.username_db;
            ELSE
                v_sql := 'GRANT rl_technician TO ' || r.username_db; -- Default fallback
            END IF;
            EXECUTE IMMEDIATE v_sql;
            
            v_sql := 'ALTER USER ' || r.username_db || ' DEFAULT ROLE ALL';
            EXECUTE IMMEDIATE v_sql;
        EXCEPTION WHEN OTHERS THEN 
            NULL; -- Bá» qua náº¿u user Ä‘Ã£ tá»“n táº¡i
        END;
    END LOOP;
END;
/

PROMPT ====================================================================
PROMPT Tao tai khoan Database thuc te cho Toan bo Benh nhan
PROMPT ====================================================================
DECLARE
    v_sql VARCHAR2(1000);
BEGIN
    FOR r IN (SELECT username_db FROM hospital.patient WHERE username_db = 'BN000001') LOOP
        BEGIN
            v_sql := 'CREATE USER ' || r.username_db || ' IDENTIFIED BY "123"';
            EXECUTE IMMEDIATE v_sql;
        EXCEPTION WHEN OTHERS THEN NULL;
        END;
        
        BEGIN
            
            v_sql := 'GRANT CREATE SESSION TO ' || r.username_db;
            EXECUTE IMMEDIATE v_sql;
            
            v_sql := 'GRANT EXECUTE ON hospital_dba.USP_GET_SESSION_ROLE TO ' || r.username_db;
            EXECUTE IMMEDIATE v_sql;
            
            v_sql := 'GRANT EXECUTE ON hospital_dba.USP_GET_USER_ID TO ' || r.username_db;
            EXECUTE IMMEDIATE v_sql;
            
            v_sql := 'GRANT rl_patient TO ' || r.username_db;
            EXECUTE IMMEDIATE v_sql;
            
            v_sql := 'ALTER USER ' || r.username_db || ' DEFAULT ROLE ALL';
            EXECUTE IMMEDIATE v_sql;
        EXCEPTION WHEN OTHERS THEN 
            NULL; -- Bá» qua náº¿u user Ä‘Ã£ tá»“n táº¡i
        END;
    END LOOP;
END;
/

PROMPT ====================================================================
PROMPT Cap quyen cho cac tai khoan U1-U8 de su dung chung voi OLS_TEST
PROMPT ====================================================================
DECLARE
BEGIN
    FOR i IN 1..8 LOOP
        BEGIN
            EXECUTE IMMEDIATE 'GRANT EXECUTE ON hospital.USP_GET_NOTIFICATIONS TO U' || i;
        EXCEPTION WHEN OTHERS THEN NULL;
        END;
    END LOOP;
END;
/
