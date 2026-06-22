-- ==============================================================================
-- 02_create_labels.sql
-- Cháº¡y dÆ°á»›i quyá»n: sysdba
-- ==============================================================================
ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

-- CRITICAL: Activate the newly generated HOSP_OLS_POL_DBA role dynamically
SET ROLE ALL;

-- 3. Build Components & Labels
BEGIN
    SA_COMPONENTS.CREATE_LEVEL('HOSP_OLS_POL', 3000, 'BGD', 'Ban Giam Doc');
    SA_COMPONENTS.CREATE_LEVEL('HOSP_OLS_POL', 2000, 'LDK', 'Lanh Dao Khoa');
    SA_COMPONENTS.CREATE_LEVEL('HOSP_OLS_POL', 1000, 'NV',  'Nhan Vien');

    SA_COMPONENTS.CREATE_COMPARTMENT('HOSP_OLS_POL', 100, 'TH', 'Khoa Tieu Hoa');
    SA_COMPONENTS.CREATE_COMPARTMENT('HOSP_OLS_POL', 200, 'TK', 'Khoa Than Kinh');
    SA_COMPONENTS.CREATE_COMPARTMENT('HOSP_OLS_POL', 300, 'TM', 'Khoa Tim Mach');

    SA_COMPONENTS.CREATE_GROUP('HOSP_OLS_POL', 10, 'HCM', 'Ho Chi Minh');
    SA_COMPONENTS.CREATE_GROUP('HOSP_OLS_POL', 20, 'HP',  'Hai Phong');
    SA_COMPONENTS.CREATE_GROUP('HOSP_OLS_POL', 30, 'HN',  'Ha Noi');

    DECLARE
        TYPE t_str_arr IS TABLE OF VARCHAR2(100);
        v_levels t_str_arr := t_str_arr('NV', 'LDK', 'BGD');
        v_comps t_str_arr := t_str_arr('', 'TH', 'TK', 'TM', 'TH,TK', 'TH,TM', 'TK,TM', 'TH,TK,TM');
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

-- 4. Apply Policy & Grant Bypass privileges
BEGIN
    SA_POLICY_ADMIN.APPLY_TABLE_POLICY('HOSP_OLS_POL', 'HOSPITAL', 'NOTIFICATION', 'READ_CONTROL, WRITE_CONTROL, CHECK_CONTROL');
    SA_USER_ADMIN.SET_USER_PRIVS('HOSP_OLS_POL', 'HOSPITAL_DBA', 'FULL');
END;
/
