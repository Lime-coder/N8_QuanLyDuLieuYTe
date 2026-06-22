-- ==============================================================================
-- 01_create_ols_policy.sql
-- Run as: sysdba
-- ==============================================================================
ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

-- 1. Base Table & Sequence
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
