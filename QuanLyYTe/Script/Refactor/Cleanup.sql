-- Run as: SYSDBA | Container: PDB_QLYT
-- Description: Completely resets the QLYT database environment by dropping all related users, roles, schemas, and objects.
ALTER SESSION SET CONTAINER = PDB_QLYT;
SET SERVEROUTPUT ON;

DECLARE
    v_active_sessions NUMBER;
BEGIN
    DBMS_OUTPUT.PUT_LINE('--- STARTING DATABASE CLEANUP PROCESS ---');

    -- 1. Drop mock staff and dynamically created patient users (NV% and USR_%)
    -- Catching OTHERS to allow the script to continue if a user doesn't exist
    DBMS_OUTPUT.PUT_LINE('1. Dropping dynamically created test users...');
    FOR u IN (
        SELECT username FROM dba_users 
        WHERE username LIKE 'NV%' OR username LIKE 'USR\_%' ESCAPE '\'
    ) LOOP
        BEGIN
            EXECUTE IMMEDIATE 'DROP USER ' || u.username || ' CASCADE';
            DBMS_OUTPUT.PUT_LINE('   - Dropped user: ' || u.username);
        EXCEPTION 
            WHEN OTHERS THEN 
                DBMS_OUTPUT.PUT_LINE('   ! Failed to drop user ' || u.username || ': ' || SQLERRM);
        END;
    END LOOP;

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
        BEGIN
            EXECUTE IMMEDIATE 'DROP USER ' || u.username || ' CASCADE';
            DBMS_OUTPUT.PUT_LINE('   - Dropped user: ' || u.username);
        EXCEPTION 
            WHEN OTHERS THEN 
                DBMS_OUTPUT.PUT_LINE('   ! Failed to drop user ' || u.username || ': ' || SQLERRM);
        END;
    END LOOP;

    -- 4. Drop hospital schema user 
    -- This cascades and drops all business tables (department, staff, patient, etc.), constraints, and data
    DBMS_OUTPUT.PUT_LINE('4. Dropping hospital schema and associated tables...');
    FOR u IN (SELECT username FROM dba_users WHERE username = 'HOSPITAL') LOOP
        BEGIN
            EXECUTE IMMEDIATE 'DROP USER ' || u.username || ' CASCADE';
            DBMS_OUTPUT.PUT_LINE('   - Dropped user: ' || u.username);
        EXCEPTION 
            WHEN OTHERS THEN 
                DBMS_OUTPUT.PUT_LINE('   ! Failed to drop user ' || u.username || ': ' || SQLERRM);
        END;
    END LOOP;

    DBMS_OUTPUT.PUT_LINE('--- CLEANUP COMPLETED SUCCESSFULLY ---');
END;
/