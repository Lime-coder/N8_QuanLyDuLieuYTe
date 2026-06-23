-- ==============================================================================
-- 03_coordinator_policies.sql
-- Cháº¡y dÆ°á»›i quyá»n: hospital_dba
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

BEGIN
    BEGIN
        DBMS_RLS.DROP_POLICY('HOSPITAL', 'STAFF', 'POL_VPD_STAFF_SELF_SELECT');
    EXCEPTION WHEN OTHERS THEN NULL; END;

    BEGIN
        DBMS_RLS.DROP_POLICY('HOSPITAL', 'STAFF', 'POL_VPD_STAFF_SELF_UPDATE');
    EXCEPTION WHEN OTHERS THEN NULL; END;

    -- Policy 1: SELECT trá»±c tiáº¿p STAFF chá»‰ tháº¥y chÃ­nh mÃ¬nh
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL',
        object_name     => 'STAFF',
        policy_name     => 'POL_VPD_STAFF_SELF_SELECT',
        function_schema => 'HOSPITAL',
        policy_function => 'FN_VPD_STAFF_SELF',
        statement_types => 'SELECT'
    );

    -- Policy 2: UPDATE phone, hometown chá»‰ trÃªn dÃ²ng chÃ­nh mÃ¬nh
    DBMS_RLS.ADD_POLICY(
        object_schema     => 'HOSPITAL',
        object_name       => 'STAFF',
        policy_name       => 'POL_VPD_STAFF_SELF_UPDATE',
        function_schema   => 'HOSPITAL',
        policy_function   => 'FN_VPD_STAFF_SELF',
        statement_types   => 'UPDATE',
        sec_relevant_cols => 'PHONE,HOMETOWN',
        update_check      => TRUE
    );
END;
/
