-- ==============================================================================
-- 02_create_labels.sql
-- Chạy dưới quyền: hospital_dba
-- Mục đích: Khởi tạo các thành phần của nhãn (Levels, Compartments, Groups)
-- và tạo tổ hợp các nhãn OLS.
-- ==============================================================================
ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

-- Kích hoạt Role quản trị OLS (HOSP_OLS_POL_DBA) vừa được cấp trong phiên hiện tại
SET ROLE ALL;

-- 3. Xây dựng các thành phần của Nhãn (Components)
-- Kiến trúc Nhãn: LEVEL : COMPARTMENT : GROUP
BEGIN
    -- CẤP ĐỘ (Levels): Mang tính phân cấp (Linear). Chỉ số càng cao, quyền càng lớn.
    SA_COMPONENTS.CREATE_LEVEL('HOSP_OLS_POL', 3000, 'BGD', 'Ban Giam Doc');
    SA_COMPONENTS.CREATE_LEVEL('HOSP_OLS_POL', 2000, 'LDK', 'Lanh Dao Khoa');
    SA_COMPONENTS.CREATE_LEVEL('HOSP_OLS_POL', 1000, 'NV',  'Nhan Vien');

    -- NGĂN (Compartments): Phân chia theo chiều ngang (Không phân cấp). 
    SA_COMPONENTS.CREATE_COMPARTMENT('HOSP_OLS_POL', 100, 'TH', 'Khoa Tieu Hoa');
    SA_COMPONENTS.CREATE_COMPARTMENT('HOSP_OLS_POL', 200, 'TK', 'Khoa Than Kinh');
    SA_COMPONENTS.CREATE_COMPARTMENT('HOSP_OLS_POL', 300, 'TM', 'Khoa Tim Mach');

    -- NHÓM (Groups): Phân chia theo sơ đồ tổ chức hoặc địa lý.
    SA_COMPONENTS.CREATE_GROUP('HOSP_OLS_POL', 10, 'HCM', 'Ho Chi Minh');
    SA_COMPONENTS.CREATE_GROUP('HOSP_OLS_POL', 20, 'HP',  'Hai Phong');
    SA_COMPONENTS.CREATE_GROUP('HOSP_OLS_POL', 30, 'HN',  'Ha Noi');

    -- TẠO TỔ HỢP NHÃN (Label Combinations)
    -- OLS yêu cầu mọi nhãn phải được định nghĩa trước khi sử dụng. Dưới đây là vòng lặp sinh ra
    -- Tất cả các chuỗi nhãn có thể có từ các Component trên (VD: 'NV:TH:HCM', 'BGD:TH,TK:HN,HP', v.v.)
    DECLARE
        TYPE t_str_arr IS TABLE OF VARCHAR2(100);
        -- Các tổ hợp của Levels
        v_levels t_str_arr := t_str_arr('NV', 'LDK', 'BGD');
        -- Các tổ hợp của Compartment
        v_comps t_str_arr := t_str_arr('', 'TH', 'TK', 'TM', 'TH,TK', 'TH,TM', 'TK,TM', 'TH,TK,TM');
        -- Các tổ hợp của Group
        v_groups t_str_arr := t_str_arr('', 'HCM', 'HN', 'HP', 'HCM,HN', 'HCM,HP', 'HN,HP', 'HCM,HN,HP');
        v_label VARCHAR2(200);
        v_id NUMBER := 10000;
    BEGIN
        FOR l IN 1..v_levels.COUNT LOOP
            FOR c IN 1..v_comps.COUNT LOOP
                FOR g IN 1..v_groups.COUNT LOOP
                    v_label := v_levels(l);
                    IF v_comps(c) IS NOT NULL THEN
                        v_label := v_label || ':' || v_comps(c);
                    ELSIF v_groups(g) IS NOT NULL THEN
                        v_label := v_label || ':';
                    END IF;
                    
                    IF v_groups(g) IS NOT NULL THEN
                        v_label := v_label || ':' || v_groups(g);
                    END IF;
                    
                    BEGIN
                        -- Đăng ký nhãn với hệ thống OLS, gán cho một Label Tag (v_id) duy nhất.
                        SA_LABEL_ADMIN.CREATE_LABEL('HOSP_OLS_POL', v_id, v_label);
                    EXCEPTION WHEN OTHERS THEN NULL;
                    END;
                    v_id := v_id + 1;
                END LOOP;
            END LOOP;
        END LOOP;
    END;
END;
/

-- 4. Áp dụng Policy lên bảng
BEGIN
    -- Gắn Policy 'HOSP_OLS_POL' vào bảng 'NOTIFICATION' trong schema 'HOSPITAL'
    -- Từ lúc này, Oracle sẽ tự động thêm 1 cột tên OLS_LABEL (đã định nghĩa ở script 1) vào bảng này.
    SA_POLICY_ADMIN.APPLY_TABLE_POLICY('HOSP_OLS_POL', 'HOSPITAL', 'NOTIFICATION', 'READ_CONTROL, WRITE_CONTROL, CHECK_CONTROL');
    
    -- Đảm bảo HOSPITAL_DBA có quyền FULL bypass policy
    SA_USER_ADMIN.SET_USER_PRIVS('HOSP_OLS_POL', 'HOSPITAL_DBA', 'FULL');
END;
/
