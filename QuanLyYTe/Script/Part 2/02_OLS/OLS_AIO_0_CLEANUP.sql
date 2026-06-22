-- ==============================================================================
-- File: OLS_AIO_0_CLEANUP.sql
-- Run as: HOSPITAL_DBA
-- Connect to: PDB_QLYT
-- Purpose: Surgically tear down the OLS Requirement 2 environment.
-- ==============================================================================

ALTER SESSION SET CURRENT_SCHEMA = hospital;
-- CRITICAL: Ensure LBAC_DBA and DROP USER roles are active for the cleanup
SET ROLE ALL; 
-- CRITICAL: Turn off substitution prompts for ampersands (&)
SET DEFINE OFF; 
SET SERVEROUTPUT ON;

PROMPT ==============================================================================
PROMPT 1. Dropping OLS Policy (Removes Levels, Compartments, Groups, and Labels)
PROMPT ==============================================================================
BEGIN
    SA_SYSDBA.DROP_POLICY(
        policy_name => 'HOSP_OLS_POL',
        drop_column => TRUE
    );
    DBMS_OUTPUT.PUT_LINE('[OK] Policy HOSP_OLS_POL dropped successfully.');
EXCEPTION 
    WHEN OTHERS THEN 
        DBMS_OUTPUT.PUT_LINE('[SKIP] Policy HOSP_OLS_POL not found or already dropped.');
END;
/

PROMPT ==============================================================================
PROMPT 2. Dropping Target Table & Procedure
PROMPT ==============================================================================
BEGIN
    EXECUTE IMMEDIATE 'DROP TABLE hospital.notification CASCADE CONSTRAINTS';
    DBMS_OUTPUT.PUT_LINE('[OK] Table hospital.notification dropped.');
EXCEPTION 
    WHEN OTHERS THEN 
        IF SQLCODE != -942 THEN DBMS_OUTPUT.PUT_LINE('[ERROR] ' || SQLERRM); 
        ELSE DBMS_OUTPUT.PUT_LINE('[SKIP] Table hospital.notification not found.'); END IF;
END;
/

BEGIN
    EXECUTE IMMEDIATE 'DROP SEQUENCE hospital.seq_notification_id';
    DBMS_OUTPUT.PUT_LINE('[OK] Sequence hospital.seq_notification_id dropped.');
EXCEPTION 
    WHEN OTHERS THEN 
        IF SQLCODE != -2289 THEN DBMS_OUTPUT.PUT_LINE('[ERROR] ' || SQLERRM); 
        ELSE DBMS_OUTPUT.PUT_LINE('[SKIP] Sequence hospital.seq_notification_id not found.'); END IF;
END;
/

BEGIN
    EXECUTE IMMEDIATE 'DROP PROCEDURE hospital.USP_GET_NOTIFICATIONS';
    DBMS_OUTPUT.PUT_LINE('[OK] Procedure hospital.USP_GET_NOTIFICATIONS dropped.');
EXCEPTION 
    WHEN OTHERS THEN 
        IF SQLCODE != -4043 THEN DBMS_OUTPUT.PUT_LINE('[ERROR] ' || SQLERRM); 
        ELSE DBMS_OUTPUT.PUT_LINE('[SKIP] Procedure hospital.USP_GET_NOTIFICATIONS not found.'); END IF;
END;
/

BEGIN
    EXECUTE IMMEDIATE 'DROP PROCEDURE hospital.USP_ADD_NOTIFICATION';
    DBMS_OUTPUT.PUT_LINE('[OK] Procedure hospital.USP_ADD_NOTIFICATION dropped.');
EXCEPTION 
    WHEN OTHERS THEN 
        IF SQLCODE != -4043 THEN DBMS_OUTPUT.PUT_LINE('[ERROR] ' || SQLERRM); 
        ELSE DBMS_OUTPUT.PUT_LINE('[SKIP] Procedure hospital.USP_ADD_NOTIFICATION not found.'); END IF;
END;
/

PROMPT ==============================================================================
PROMPT 3. Dropping OLS Test Users (U1 - U8)
PROMPT ==============================================================================
DECLARE
BEGIN
    FOR i IN 1..8 LOOP
        BEGIN
            EXECUTE IMMEDIATE 'DROP USER U' || i || ' CASCADE';
            DBMS_OUTPUT.PUT_LINE('[OK] User U' || i || ' dropped.');
        EXCEPTION 
            WHEN OTHERS THEN 
                IF SQLCODE != -1918 THEN DBMS_OUTPUT.PUT_LINE('[ERROR] U' || i || ': ' || SQLERRM); 
                ELSE DBMS_OUTPUT.PUT_LINE('[SKIP] User U' || i || ' not found.'); END IF;
        END;
    END LOOP;
END;
/

PROMPT ==============================================================================
PROMPT DONE: OLS Environment successfully cleaned up.
PROMPT ==============================================================================