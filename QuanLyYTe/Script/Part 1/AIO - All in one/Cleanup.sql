-- Run as: SYSDBA | Container: PDB_QLYT
-- Description: Completely resets the QLYT database environment by dropping all related users, roles, schemas, and objects.
-- ALTER SESSION SET CONTAINER = PDB_QLYT;
SET SERVEROUTPUT ON;

DECLARE
    v_active_sessions NUMBER;
    
    -- Helper procedure to safely kill active sessions and drop a user
    PROCEDURE kill_and_drop_user(p_username IN VARCHAR2) IS
    BEGIN
        -- Kill all active sessions for this user
        FOR s IN (SELECT sid, serial# FROM v$session WHERE username = p_username) LOOP
            BEGIN
                EXECUTE IMMEDIATE 'ALTER SYSTEM KILL SESSION ''' || s.sid || ',' || s.serial# || ''' IMMEDIATE';
            EXCEPTION WHEN OTHERS THEN NULL;
            END;
        END LOOP;
        
        -- Drop the user
        EXECUTE IMMEDIATE 'DROP USER ' || p_username || ' CASCADE';
        DBMS_OUTPUT.PUT_LINE('   - Dropped user: ' || p_username);
    EXCEPTION 
        WHEN OTHERS THEN 
            -- Ignore ORA-01918: user does not exist (expected for mock patients)
            IF SQLCODE != -1918 THEN
                DBMS_OUTPUT.PUT_LINE('   ! Failed to drop user ' || p_username || ': ' || SQLERRM);
            END IF;
    END kill_and_drop_user;
BEGIN
    DBMS_OUTPUT.PUT_LINE('--- STARTING DATABASE CLEANUP PROCESS ---');

    -- 1. Drop all app-linked users (staff and patients) dynamically + fallback for mock users
    -- We use dynamic SQL in case the hospital schema/tables are already dropped
    DBMS_OUTPUT.PUT_LINE('1. Dropping dynamically created users...');
    DECLARE
        TYPE t_user_list IS TABLE OF VARCHAR2(128);
        v_users t_user_list;
    BEGIN
        -- Try to get all users registered in our business tables + the mock patterns
        EXECUTE IMMEDIATE '
            SELECT d.username FROM dba_users d
            WHERE d.username IN (
                SELECT username_db FROM hospital.staff
                UNION
                SELECT username_db FROM hospital.patient
            ) OR d.username LIKE ''NV%'' OR d.username LIKE ''BN%'' OR d.username LIKE ''USR\_%'' ESCAPE ''\''
        ' BULK COLLECT INTO v_users;
            
        FOR i IN 1..v_users.COUNT LOOP
            kill_and_drop_user(v_users(i));
        END LOOP;
    EXCEPTION 
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('   ! Hospital tables not found. Falling back to pattern matching...');
            FOR u IN (
                SELECT username FROM dba_users 
                WHERE username LIKE 'NV%' OR username LIKE 'BN%' OR username LIKE 'USR\_%' ESCAPE '\'
            ) LOOP
                kill_and_drop_user(u.username);
            END LOOP;
    END;

    -- 2. Drop project-specific roles (RL_%)
    DBMS_OUTPUT.PUT_LINE('2. Dropping project roles...');
    FOR r IN (
        SELECT role FROM dba_roles 
        WHERE role IN ('RL_COORDINATOR', 'RL_DOCTOR', 'RL_TECHNICIAN', 'RL_PATIENT', 'RL_DBA')
    ) LOOP
        BEGIN
            EXECUTE IMMEDIATE 'DROP ROLE ' || r.role;
            DBMS_OUTPUT.PUT_LINE('   - Dropped role: ' || r.role);
        EXCEPTION 
            WHEN OTHERS THEN 
                DBMS_OUTPUT.PUT_LINE('   ! Failed to drop role ' || r.role || ': ' || SQLERRM);
        END;
    END LOOP;

    -- 3. Drop hospital_dba user 
    -- This cascades and drops all USP_ procedures and dynamic V_PRIV_ views owned by this user
    DBMS_OUTPUT.PUT_LINE('3. Dropping hospital_dba user and associated procedures/views...');
    FOR u IN (SELECT username FROM dba_users WHERE username = 'HOSPITAL_DBA') LOOP
        kill_and_drop_user(u.username);
    END LOOP;

    -- 4. Drop sequences and hospital schema user 
    -- This cascades and drops all business tables, sequences (SEQ_STAFF_ID, SEQ_PATIENT_ID), and data
    DBMS_OUTPUT.PUT_LINE('4. Dropping hospital schema, sequences and associated tables...');
    FOR u IN (SELECT username FROM dba_users WHERE username = 'HOSPITAL') LOOP
        -- Explicitly drop sequences just in case a partial cleanup is desired in the future
        BEGIN EXECUTE IMMEDIATE 'DROP SEQUENCE hospital.SEQ_STAFF_ID'; EXCEPTION WHEN OTHERS THEN NULL; END;
        BEGIN EXECUTE IMMEDIATE 'DROP SEQUENCE hospital.SEQ_PATIENT_ID'; EXCEPTION WHEN OTHERS THEN NULL; END;
        
        kill_and_drop_user(u.username);
    END LOOP;

    DBMS_OUTPUT.PUT_LINE('--- CLEANUP COMPLETED SUCCESSFULLY ---');
END;
/