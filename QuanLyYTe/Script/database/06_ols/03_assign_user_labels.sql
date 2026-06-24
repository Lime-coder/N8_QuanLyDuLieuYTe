-- ==============================================================================
-- 03_assign_user_labels.sql
-- Ch?y du?i quy?n: hospital_dba
-- ==============================================================================
ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

SET ROLE ALL;

DECLARE
    v_level VARCHAR2(10);
    v_comp  VARCHAR2(10);
    v_group VARCHAR2(10);
    v_label VARCHAR2(100);
BEGIN
    FOR r IN (SELECT username_db, staff_role, dept_id, facility FROM hospital.staff WHERE username_db IS NOT NULL) LOOP
        -- 1. Determine Level
        IF r.staff_role = UNISTR('Gi\00E1m \0111\1ED1c') THEN 
            v_level := 'BGD';
        ELSIF r.staff_role = UNISTR('Tr\01B0\1EDFng khoa') THEN 
            v_level := 'LDK';
        ELSE 
            v_level := 'NV';
        END IF;

        -- 2. Determine Compartment
        -- PB01 -> Tim Mạch (TM), PB02 -> Thần Kinh (TK), PB03 -> Tiêu Hóa (TH)
        IF r.dept_id = 'PB01' THEN 
            v_comp := 'TM';
        ELSIF r.dept_id = 'PB02' THEN 
            v_comp := 'TK';
        ELSIF r.dept_id = 'PB03' THEN 
            v_comp := 'TH';
        ELSE 
            v_comp := NULL;
        END IF;

        -- 3. Determine Group
        IF r.facility = UNISTR('H\1ED3 Ch\00ED Minh') THEN 
            v_group := 'HCM';
        ELSIF r.facility = UNISTR('H\00E0 N\1ED9i') THEN 
            v_group := 'HN';
        ELSIF r.facility = UNISTR('H\1EA3i Ph\00F2ng') THEN 
            v_group := 'HP';
        ELSE 
            v_group := NULL;
        END IF;

        -- 4. Construct Label String
        v_label := v_level;
        IF v_comp IS NOT NULL THEN 
            v_label := v_label || ':' || v_comp;
        ELSIF v_group IS NOT NULL THEN 
            v_label := v_label || ':';
        END IF;
        
        IF v_group IS NOT NULL THEN 
            v_label := v_label || ':' || v_group; 
        END IF;

        -- 5. Assign
        BEGIN
            SA_USER_ADMIN.SET_USER_LABELS('HOSP_OLS_POL', UPPER(TRIM(r.username_db)), v_label);
        EXCEPTION WHEN OTHERS THEN 
            NULL;
        END;
    END LOOP;
END;
/
