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
    v_current_user := SYS_CONTEXT('USERENV', 'SESSION_USER');
    
    -- Bypass cho schema owner, DBA app, và khi trigger chạy (CURRENT_USER = 'HOSPITAL')
    IF SYS_CONTEXT('USERENV', 'CURRENT_USER') IN ('HOSPITAL', 'HOSPITAL_DBA') THEN
        RETURN '1=1';
    END IF;

    -- Nhân viên chỉ thấy / sửa dòng của chính mình
    RETURN 'username_db = SYS_CONTEXT(''USERENV'', ''SESSION_USER'')';

EXCEPTION
    WHEN OTHERS THEN
        RETURN '1=0';
END;
/

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
-- PHẦN 8: CHỈNH SỬA SCHEMA CHO PHÉP ĐIỀU PHỐI VIÊN PHÂN CÔNG (ALLOW NULL TECHNICIAN)
-- ==============================================================================

-- 8.1 Cho phép cột technician_id nhận giá trị NULL để lưu các dịch vụ chưa phân công
BEGIN
    EXECUTE IMMEDIATE 'ALTER TABLE hospital.service_record MODIFY technician_id NULL';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE = -1451 THEN
            NULL; -- Bỏ qua nếu cột đã cho phép NULL
        ELSE
            RAISE;
        END IF;
END;
/

-- 8.2 Cập nhật trigger để bỏ qua kiểm tra KTV nếu technician_id là NULL
CREATE OR REPLACE TRIGGER hospital.TRG_VALIDATE_SERVICE_RECORD
BEFORE INSERT OR UPDATE ON hospital.service_record
FOR EACH ROW
DECLARE
    v_tech_active NUMBER(1);
BEGIN
    IF :NEW.technician_id IS NOT NULL THEN
        SELECT is_active INTO v_tech_active FROM hospital.staff WHERE staff_id = :NEW.technician_id;
        IF v_tech_active = 0 THEN
            RAISE_APPLICATION_ERROR(-20012, 'Không thể tạo/cập nhật dịch vụ: Kỹ thuật viên này đã bị khóa (Không hoạt động).');
        END IF;
    END IF;
END;
/

-- 8.3 Thêm một record mẫu chưa phân công lấy từ HSBA có sẵn để tránh lỗi Khóa ngoại
BEGIN
    FOR r IN (SELECT record_id FROM hospital.medical_record WHERE ROWNUM = 1) LOOP
        INSERT INTO hospital.service_record (record_id, service_type, service_date, technician_id, service_result)
        SELECT r.record_id, N'Xét nghiệm máu', SYSDATE - 1, NULL, NULL
        FROM DUAL
        WHERE NOT EXISTS (
            SELECT 1 FROM hospital.service_record WHERE record_id = r.record_id AND service_type = N'Xét nghiệm máu'
        );
    END LOOP;
    COMMIT;
END;
/
