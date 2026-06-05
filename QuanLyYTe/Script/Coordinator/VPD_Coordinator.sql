-- ==============================================================================
-- File: VPD_Coordinator.sql
-- Mục đích:
-- 1. Giữ TC#5: nhân viên query trực tiếp STAFF chỉ thấy chính mình.
-- 2. Tạo bảng phụ tối thiểu cho Điều phối viên chọn Bác sĩ/Y sĩ và Kỹ thuật viên.
-- Run as: HOSPITAL_DBA có quyền DBMS_RLS
-- ==============================================================================

ALTER SESSION SET CURRENT_SCHEMA = hospital;

-- ==============================================================================
-- PHẦN 1: DROP POLICY CŨ
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

-- ==============================================================================
-- PHẦN 2: DROP FUNCTION CŨ
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
-- PHẦN 3: TẠO FUNCTION VPD CHO STAFF
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
-- PHẦN 4: GẮN VPD POLICY CHO STAFF
-- ==============================================================================

BEGIN
    -- Policy 1: SELECT trực tiếp STAFF chỉ thấy chính mình
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'HOSPITAL',
        object_name     => 'STAFF',
        policy_name     => 'POL_VPD_STAFF_SELF_SELECT',
        policy_function => 'FN_VPD_STAFF_SELF',
        statement_types => 'SELECT'
    );

    -- Policy 2: UPDATE phone, hometown chỉ trên dòng chính mình
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
-- PHẦN 5: TẠO BẢNG PHỤ TỐI THIỂU CHO ĐIỀU PHỐI VIÊN PHÂN CÔNG
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
-- PHẦN 6: ĐỔ DỮ LIỆU TỐI THIỂU TỪ STAFF SANG BẢNG PHỤ
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
    N'Bác sĩ',
    N'Bác sĩ/Y sĩ',
    N'Kỹ thuật viên'
);

COMMIT;

-- ==============================================================================
-- PHẦN 7: TẠO VIEW CHO ĐIỀU PHỐI VIÊN
-- ==============================================================================

CREATE OR REPLACE VIEW hospital.VW_COORD_DOCTORS AS
SELECT
    username_db,
    staff_id,
    full_name,
    dept_id,
    specialty
FROM hospital.COORD_ASSIGNMENT_STAFF
WHERE staff_role IN (N'Bác sĩ', N'Bác sĩ/Y sĩ');

CREATE OR REPLACE VIEW hospital.VW_COORD_TECHNICIANS AS
SELECT
    username_db,
    staff_id,
    full_name
FROM hospital.COORD_ASSIGNMENT_STAFF
WHERE staff_role = N'Kỹ thuật viên';

-- ==============================================================================
-- PHẦN 8: TEST NHANH
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
