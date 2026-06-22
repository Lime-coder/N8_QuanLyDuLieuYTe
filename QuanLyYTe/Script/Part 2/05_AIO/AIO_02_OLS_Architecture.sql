-- ==============================================================================
-- AIO_02_OLS_Architecture.sql
-- Run as: HOSPITAL_DBA
-- Container: PDB_QLYT (unless specified otherwise)
-- ==============================================================================

-- ==============================================================================
-- Source: 02_OLS\OLS_AIO_2_HOSP_ARCH.sql
-- ==============================================================================

-- ==============================================================================
-- File: OLS_AIO_2_HOSP_ARCH.sql
-- Run as: HOSPITAL_DBA
-- Connect to: PDB_QLYT
-- ==============================================================================
ALTER SESSION SET CURRENT_SCHEMA = hospital;
SET SERVEROUTPUT ON;

-- 1. Base Table & Sequence (English Naming)
BEGIN EXECUTE IMMEDIATE 'DROP TABLE hospital.notification CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP SEQUENCE hospital.seq_notification_id'; EXCEPTION WHEN OTHERS THEN NULL; END;
/

CREATE TABLE hospital.notification (
    notification_id VARCHAR2(10) PRIMARY KEY,
    description     NVARCHAR2(1000) NOT NULL,
    posted_date     DATE NOT NULL,
    location        NVARCHAR2(100)
);

CREATE SEQUENCE hospital.seq_notification_id START WITH 8 INCREMENT BY 1;

-- 2. Initialize Policy
BEGIN SA_SYSDBA.DROP_POLICY('HOSP_OLS_POL', TRUE); EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN
    SA_SYSDBA.CREATE_POLICY(
        policy_name     => 'HOSP_OLS_POL',
        column_name     => 'OLS_LABEL',
        default_options => 'READ_CONTROL, WRITE_CONTROL, CHECK_CONTROL'
    );
END;
/

-- CRITICAL: Activate the newly generated HOSP_OLS_POL_DBA role dynamically
SET ROLE ALL;

-- 3. Build Components & Labels
BEGIN
    SA_COMPONENTS.CREATE_LEVEL('HOSP_OLS_POL', 3000, 'BGD', 'Ban Giam Doc');
    SA_COMPONENTS.CREATE_LEVEL('HOSP_OLS_POL', 2000, 'LDK', 'Lanh Dao Khoa');
    SA_COMPONENTS.CREATE_LEVEL('HOSP_OLS_POL', 1000, 'NV',  'Nhan Vien');

    SA_COMPONENTS.CREATE_COMPARTMENT('HOSP_OLS_POL', 100, 'TH', 'Khoa Tieu Hoa');
    SA_COMPONENTS.CREATE_COMPARTMENT('HOSP_OLS_POL', 200, 'TK', 'Khoa Than Kinh');
    SA_COMPONENTS.CREATE_COMPARTMENT('HOSP_OLS_POL', 300, 'TM', 'Khoa Tim Mach');

    SA_COMPONENTS.CREATE_GROUP('HOSP_OLS_POL', 10, 'HCM', 'Ho Chi Minh');
    SA_COMPONENTS.CREATE_GROUP('HOSP_OLS_POL', 20, 'HP',  'Hai Phong');
    SA_COMPONENTS.CREATE_GROUP('HOSP_OLS_POL', 30, 'HN',  'Ha Noi');

    -- Sinh toàn bộ 192 tổ hợp nhãn (3 Cấp bậc x 8 Tổ hợp khoa x 8 Tổ hợp cơ sở)
    -- Điều này là BẮT BUỘC vì Oracle OLS chỉ cho phép Read Access chính xác 
    -- trên các nhãn đã được khởi tạo tường minh (Valid Data Labels).
    DECLARE
        TYPE t_str_arr IS TABLE OF VARCHAR2(100);
        v_levels t_str_arr := t_str_arr('NV', 'LDK', 'BGD');
        v_comps t_str_arr := t_str_arr('', 'TH', 'TK', 'TM', 'TH,TK', 'TH,TM', 'TK,TM', 'TH,TK,TM');
        v_groups t_str_arr := t_str_arr('', 'HCM', 'HN', 'HP', 'HCM,HN', 'HCM,HP', 'HN,HP', 'HCM,HN,HP');
        v_label VARCHAR2(200);
        v_id NUMBER := 10000;
    BEGIN
        FOR l IN 1..v_levels.COUNT LOOP
            FOR c IN 1..v_comps.COUNT LOOP
                FOR g IN 1..v_groups.COUNT LOOP
                    v_label := v_levels(l);
                    IF v_comps(c) IS NOT NULL THEN
                        v_label := v_label || ':' || v_comps(c);
                    ELSIF v_groups(g) IS NOT NULL THEN
                        v_label := v_label || ':';
                    END IF;
                    
                    IF v_groups(g) IS NOT NULL THEN
                        v_label := v_label || ':' || v_groups(g);
                    END IF;
                    
                    BEGIN
                        SA_LABEL_ADMIN.CREATE_LABEL('HOSP_OLS_POL', v_id, v_label);
                    EXCEPTION WHEN OTHERS THEN NULL;
                    END;
                    v_id := v_id + 1;
                END LOOP;
            END LOOP;
        END LOOP;
    END;
END;
/

-- 4. Apply Policy & Grant Bypass privileges (Updated table name)
BEGIN
    SA_POLICY_ADMIN.APPLY_TABLE_POLICY('HOSP_OLS_POL', 'HOSPITAL', 'NOTIFICATION', 'READ_CONTROL, WRITE_CONTROL, CHECK_CONTROL');
    SA_USER_ADMIN.SET_USER_PRIVS('HOSP_OLS_POL', 'HOSPITAL_DBA', 'FULL');
END;
/

-- 5. User Creation & Authorization (Unchanged)
DECLARE
    PROCEDURE create_u(p_user IN VARCHAR2) IS
    BEGIN
        EXECUTE IMMEDIATE 'DROP USER ' || p_user || ' CASCADE';
    EXCEPTION WHEN OTHERS THEN NULL;
    END;
BEGIN
    FOR i IN 1..8 LOOP
        create_u('U' || i);
        EXECUTE IMMEDIATE 'CREATE USER U' || i || ' IDENTIFIED BY 123';
        EXECUTE IMMEDIATE 'GRANT CREATE SESSION TO U' || i;
        EXECUTE IMMEDIATE 'GRANT SELECT ON hospital.notification TO U' || i;
    END LOOP;
    EXECUTE IMMEDIATE 'GRANT SELECT ON hospital.notification TO rl_doctor, rl_coordinator, rl_technician, rl_patient';
END;
/

BEGIN
    SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', 'U1', 'BGD:TH,TK,TM:HCM,HP,HN');
    SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', 'U2', 'LDK:TM:HCM');
    SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', 'U3', 'LDK:TK:HN');
    SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', 'U4', 'NV:TK:HCM');
    SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', 'U5', 'NV:TM:HCM');
    SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', 'U6', 'LDK:TM:HCM');
    SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', 'U7', 'LDK:TH,TK,TM:HCM,HP,HN');
    SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', 'U8', 'NV:TH:HN');

    -- Assign OLS labels to specific testing staff accounts
    SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', 'NV000001', 'BGD:TH,TK,TM:HCM,HP,HN');
    SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', 'NV000021', 'NV:TK:HCM');
    SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', 'NV000121', 'NV:TK:HCM');
END;
/
PROMPT ==============================================================================
PROMPT DONE: Architecture complete. 
PROMPT CRITICAL: Disconnect and Reconnect your HOSPITAL_DBA session now!
PROMPT ==============================================================================

