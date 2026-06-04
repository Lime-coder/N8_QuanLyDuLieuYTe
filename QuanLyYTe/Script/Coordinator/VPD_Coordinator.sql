-- ==============================================================================
-- File: VPD_Coordinator.sql
-- Má»¥c Ä‘Ã­ch:
-- 1. Giá»¯ TC#5: nhÃ¢n viÃªn query trá»±c tiáº¿p STAFF chá»‰ tháº¥y chÃ­nh mÃ¬nh.
-- 2. Táº¡o báº£ng phá»¥ tá»‘i thiá»ƒu cho Äiá»u phá»‘i viÃªn chá»n BÃ¡c sÄ©/Y sÄ© vÃ  Ká»¹ thuáº­t viÃªn.
-- 3. KhÃ´ng dÃ¹ng CLIENT_IDENTIFIER.
-- Run as: HOSPITAL_DBA cÃ³ quyá»n DBMS_RLS
-- ==============================================================================

ALTER SESSION SET CURRENT_SCHEMA = hospital;

-- ==============================================================================
-- PHáº¦N 1: DROP POLICY CÅ¨
-- ==============================================================================

BEGIN
    DBMS_RLS.DROP_POLICY('HOSPITAL', 'STAFF', 'POL_VPD_STAFF_SELF_SELECT');
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
    DBMS_RLS.DROP_POLICY('HOSPITAL', 'STAFF', 'POL_VPD_STAFF_SELF_UPDATE');
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
    DBMS_RLS.DROP_POLICY('HOSPITAL', 'STAFF', 'POLICY_STAFF_SELF');
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

-- ==============================================================================
-- PHáº¦N 2: DROP FUNCTION CÅ¨
-- ==============================================================================

BEGIN
    EXECUTE IMMEDIATE 'DROP FUNCTION hospital.FN_VPD_STAFF_SELF';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -4043 THEN
            RAISE;
        END IF;
END;
/

-- ==============================================================================
-- PHáº¦N 3: Táº O FUNCTION VPD CHO STAFF
-- ==============================================================================

CREATE OR REPLACE FUNCTION hospital.FN_VPD_STAFF_SELF (
    p_schema VARCHAR2,
    p_obj    VARCHAR2
)
RETURN VARCHAR2
AS
    v_current_user VARCHAR2(100);
BEGIN
    v_current_user := SYS_CONTEXT('USERENV', 'CURRENT_USER');
    
    -- Bypass cho schema owner, DBA app, và khi trigger chạy (CURRENT_USER = 'HOSPITAL')
    IF v_current_user IN ('HOSPITAL', 'HOSPITAL_DBA') THEN
        RETURN '1=1';
    END IF;

    -- Nhân viên chỉ thấy / sửa dòng của chính mình
    RETURN 'username_db = SYS_CONTEXT(''USERENV'', ''SESSION_USER'')';

EXCEPTION
    WHEN OTHERS THEN
        RETURN '1=0';
END;
/

SHOW ERRORS FUNCTION hospital.FN_VPD_STAFF_SELF;

-- ==============================================================================
-- PHáº¦N 4: Gáº®N VPD POLICY CHO STAFF
-- ==============================================================================

BEGIN
    -- Policy 1: SELECT trá»±c tiáº¿p STAFF chá»‰ tháº¥y chÃ­nh mÃ¬nh
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL',
        object_name     => 'STAFF',
        policy_name     => 'POL_VPD_STAFF_SELF_SELECT',
        policy_function => 'FN_VPD_STAFF_SELF',
        statement_types => 'SELECT'
    );

    -- Policy 2: UPDATE phone, hometown chá»‰ trÃªn dÃ²ng chÃ­nh mÃ¬nh
    DBMS_RLS.ADD_POLICY(
        object_schema     => 'HOSPITAL',
        object_name       => 'STAFF',
        policy_name       => 'POL_VPD_STAFF_SELF_UPDATE',
        policy_function   => 'FN_VPD_STAFF_SELF',
        statement_types   => 'UPDATE',
        sec_relevant_cols => 'PHONE,HOMETOWN',
        update_check      => TRUE
    );
END;
/

-- ==============================================================================
-- PHáº¦N 5: Táº O Báº¢NG PHá»¤ Tá»I THIá»‚U CHO ÄIá»€U PHá»I VIÃŠN PHÃ‚N CÃ”NG
-- ==============================================================================

BEGIN
    EXECUTE IMMEDIATE 'DROP TABLE hospital.COORD_ASSIGNMENT_STAFF PURGE';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -942 THEN
            RAISE;
        END IF;
END;
/

CREATE TABLE hospital.COORD_ASSIGNMENT_STAFF (
    username_db VARCHAR2(50) PRIMARY KEY,
    staff_id    VARCHAR2(10) NOT NULL,
    full_name   NVARCHAR2(100) NOT NULL,
    staff_role  NVARCHAR2(50)  NOT NULL,
    dept_id     VARCHAR2(10),
    specialty   NVARCHAR2(100)
);

-- ==============================================================================
-- PHáº¦N 6: Äá»” Dá»® LIá»†U Tá»I THIá»‚U Tá»ª STAFF SANG Báº¢NG PHá»¤
-- ==============================================================================

INSERT INTO hospital.COORD_ASSIGNMENT_STAFF (
    username_db,
    staff_id,
    full_name,
    staff_role,
    dept_id,
    specialty
)
SELECT
    s.username_db,
    s.staff_id,
    s.full_name,
    s.staff_role,
    s.dept_id,
    d.dept_name AS specialty
FROM hospital.staff s
LEFT JOIN hospital.department d
    ON d.dept_id = s.dept_id
WHERE s.staff_role IN (
    N'BÃ¡c sÄ©',
    N'BÃ¡c sÄ©/Y sÄ©',
    N'Ká»¹ thuáº­t viÃªn'
);

COMMIT;

-- ==============================================================================
-- PHáº¦N 7: Táº O VIEW CHO ÄIá»€U PHá»I VIÃŠN
-- ==============================================================================

CREATE OR REPLACE VIEW hospital.VW_COORD_DOCTORS AS
SELECT
    username_db,
    staff_id,
    full_name,
    dept_id,
    specialty
FROM hospital.COORD_ASSIGNMENT_STAFF
WHERE staff_role IN (N'BÃ¡c sÄ©', N'BÃ¡c sÄ©/Y sÄ©');

CREATE OR REPLACE VIEW hospital.VW_COORD_TECHNICIANS AS
SELECT
    username_db,
    staff_id,
    full_name
FROM hospital.COORD_ASSIGNMENT_STAFF
WHERE staff_role = N'Ká»¹ thuáº­t viÃªn';

-- ==============================================================================
-- PHáº¦N 8: TEST NHANH
-- ==============================================================================

SELECT owner, object_name, object_type, status
FROM dba_objects
WHERE owner = 'HOSPITAL'
  AND object_name IN (
      'FN_VPD_STAFF_SELF',
      'COORD_ASSIGNMENT_STAFF',
      'VW_COORD_DOCTORS',
      'VW_COORD_TECHNICIANS'
  );
