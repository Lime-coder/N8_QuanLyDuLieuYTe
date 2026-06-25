-- ==============================================================================
-- 03_assign_user_labels.sql
-- Chạy dưới quyền: hospital_dba
-- Mục đích: Gán nhãn truy cập cho toàn bộ người dùng trong bảng Staff (Nhân viên)
-- để hệ thống tự động kiểm soát quyền đọc/ghi trên bảng có OLS.
-- ==============================================================================
ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

SET ROLE ALL;

DECLARE
    v_level VARCHAR2(10);
    v_comp  VARCHAR2(10);
    v_group VARCHAR2(10);
    v_label VARCHAR2(100);
BEGIN
    -- Duyệt qua tất cả nhân viên có tài khoản database (username_db IS NOT NULL)
    FOR r IN (SELECT username_db, staff_role, dept_id, facility FROM hospital_dba.staff WHERE username_db IS NOT NULL) LOOP
        
        -- 1. Xét Cấp độ (Level) dựa trên chức vụ (staff_role)
        IF r.staff_role = UNISTR('Gi\00E1m \0111\1ED1c') THEN 
            v_level := 'BGD'; -- Ban Giám Đốc (Max level)
        ELSIF r.staff_role = UNISTR('Tr\01B0\1EDFng khoa') THEN 
            v_level := 'LDK'; -- Lãnh Đạo Khoa (Mid level)
        ELSE 
            v_level := 'NV';  -- Nhân Viên (Min level)
        END IF;

        -- 2. Xét Ngăn chứa (Compartment) dựa trên khoa (dept_id)
        IF r.dept_id = 'PB01' THEN 
            v_comp := 'TM';   -- Tim Mạch
        ELSIF r.dept_id = 'PB02' THEN 
            v_comp := 'TK';   -- Thần Kinh
        ELSIF r.dept_id = 'PB03' THEN 
            v_comp := 'TH';   -- Tiêu Hóa
        ELSE 
            v_comp := NULL;   -- Không thuộc khoa nào (VD: Giám đốc, hành chính)
        END IF;

        -- 3. Xét Nhóm (Group) dựa trên cơ sở/chi nhánh làm việc (facility)
        IF r.facility = UNISTR('H\1ED3 Ch\00ED Minh') THEN 
            v_group := 'HCM';
        ELSIF r.facility = UNISTR('H\00E0 N\1ED9i') THEN 
            v_group := 'HN';
        ELSIF r.facility = UNISTR('H\1EA3i Ph\00F2ng') THEN 
            v_group := 'HP';
        ELSE 
            v_group := NULL;
        END IF;

        -- 4. Ghép chuỗi tạo thành Nhãn (Label String) theo cấu trúc LEVEL:COMP:GROUP
        v_label := v_level;
        IF v_comp IS NOT NULL THEN 
            v_label := v_label || ':' || v_comp;
        ELSIF v_group IS NOT NULL THEN 
            v_label := v_label || ':';
        END IF;
        
        IF v_group IS NOT NULL THEN 
            v_label := v_label || ':' || v_group; 
        END IF;

        -- 5. Thực thi Gán nhãn cho User
        BEGIN
            -- SET_USER_LABELS sẽ gán nhãn vừa tạo làm Nhãn đọc tối đa (Max Read Label) 
            -- và Nhãn ghi (Write Label) mặc định cho người dùng.
            SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', UPPER(TRIM(r.username_db)), v_label);
        EXCEPTION WHEN OTHERS THEN 
            NULL;
        END;
    END LOOP;
END;
/


