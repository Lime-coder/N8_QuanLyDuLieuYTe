-- ==============================================================================
-- 01_fga_policies.sql
-- Cháº¡y dÆ°á»›i quyá»n: sysdba
-- ==============================================================================
ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

BEGIN
    -- C. XÃ³a cÃ¡c Fine-Grained Audit Policies cÅ©
    BEGIN DBMS_FGA.DROP_POLICY('HOSPITAL', 'PRESCRIPTION', 'FGA_PRESC_UPDATE');    EXCEPTION WHEN OTHERS THEN NULL; END;
    BEGIN DBMS_FGA.DROP_POLICY('HOSPITAL', 'MEDICAL_RECORD', 'FGA_MR_UPDATE');     EXCEPTION WHEN OTHERS THEN NULL; END;
    BEGIN DBMS_FGA.DROP_POLICY('HOSPITAL', 'SERVICE_RECORD', 'FGA_SR_ILLEGAL');    EXCEPTION WHEN OTHERS THEN NULL; END;
    BEGIN DBMS_FGA.DROP_POLICY('HOSPITAL', 'PRESCRIPTION', 'FGA_PRESCRIPTION_COLS');  EXCEPTION WHEN OTHERS THEN NULL; END;
    BEGIN DBMS_FGA.DROP_POLICY('HOSPITAL', 'MEDICAL_RECORD', 'FGA_MEDICAL_RECORD_COLS'); EXCEPTION WHEN OTHERS THEN NULL; END;

    -- D. XÃ³a Unified Audit Policy cÅ©
    BEGIN EXECUTE IMMEDIATE 'NOAUDIT POLICY AUD_ILLEGAL_MR_POLICY'; EXCEPTION WHEN OTHERS THEN NULL; END;
    BEGIN EXECUTE IMMEDIATE 'DROP AUDIT POLICY AUD_ILLEGAL_MR_POLICY';         EXCEPTION WHEN OTHERS THEN NULL; END;
    BEGIN EXECUTE IMMEDIATE 'NOAUDIT POLICY AUD_ILLEGAL_SR_POLICY'; EXCEPTION WHEN OTHERS THEN NULL; END;
    BEGIN EXECUTE IMMEDIATE 'DROP AUDIT POLICY AUD_ILLEGAL_SR_POLICY';         EXCEPTION WHEN OTHERS THEN NULL; END;
END;
/

-- 3.3a: FGA Policy cho báº£ng ÄÆ NTHUá»C
BEGIN
    DBMS_FGA.ADD_POLICY(
        object_schema   => 'HOSPITAL',
        object_name     => 'PRESCRIPTION',
        policy_name     => 'FGA_PRESCRIPTION_COLS',
        audit_column    => 'RECORD_ID, PRESCRIPTION_DATE, MEDICINE_NAME, DOSAGE',
        statement_types => 'UPDATE'
    );
END;
/

-- 3.3b: FGA Policy cho báº£ng Há»’ SÆ  Bá»†NH ÃN
BEGIN
    DBMS_FGA.ADD_POLICY(
        object_schema   => 'HOSPITAL',
        object_name     => 'MEDICAL_RECORD',
        policy_name     => 'FGA_MEDICAL_RECORD_COLS',
        audit_column    => 'DIAGNOSIS, TREATMENT_PLAN, CONCLUSION',
        statement_types => 'UPDATE'
    );
END;
/

-- 3.3c (UNIFIED - Báº¤T Há»¢P PHÃP TRÃŠN HSBA)
CREATE AUDIT POLICY AUD_ILLEGAL_MR_POLICY
  ACTIONS 
    UPDATE ON hospital.medical_record;

-- Báº¯t hÃ nh vi Failed (Sai quyá»n/Báº¥t há»£p phÃ¡p) cho 3.3c
AUDIT POLICY AUD_ILLEGAL_MR_POLICY WHENEVER NOT SUCCESSFUL;

-- 3.3d (UNIFIED - Báº¤T Há»¢P PHÃP TRÃŠN SERVICE_RECORD)
CREATE AUDIT POLICY AUD_ILLEGAL_SR_POLICY
  ACTIONS 
    INSERT ON hospital.service_record, 
    UPDATE ON hospital.service_record, 
    DELETE ON hospital.service_record; 

-- Báº¯t hÃ nh vi Failed (Sai quyá»n/Báº¥t há»£p phÃ¡p) cho 3.3d
AUDIT POLICY AUD_ILLEGAL_SR_POLICY WHENEVER NOT SUCCESSFUL;
