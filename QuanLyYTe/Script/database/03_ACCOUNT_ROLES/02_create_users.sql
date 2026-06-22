-- Cháº¡y dÆ°á»›i quyá»n: hospital_dba | Container: PDB_QLYT
-- ALTER SESSION SET CONTAINER = PDB_QLYT;

-- Returns the granted role for a user (assumes one active role per user)
CREATE OR REPLACE PROCEDURE USP_GET_GRANTED_ROLE (
    p_user   IN  VARCHAR2,
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT GRANTED_ROLE FROM (
            SELECT GRANTED_ROLE FROM DBA_ROLE_PRIVS
            WHERE GRANTEE = UPPER(p_user)
            ORDER BY 
                CASE WHEN GRANTED_ROLE = 'RL_DBA' THEN 1 ELSE 2 END,
                GRANTED_ROLE
        ) WHERE ROWNUM = 1;
END USP_GET_GRANTED_ROLE;
/

-- Returns the active role for the current database session
CREATE OR REPLACE PROCEDURE USP_GET_SESSION_ROLE (
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT ROLE FROM (
            SELECT ROLE FROM SESSION_ROLES
            WHERE ROLE LIKE 'RL\_%' ESCAPE '\'
            ORDER BY CASE WHEN ROLE = 'RL_DBA' THEN 1 ELSE 2 END, ROLE
        ) WHERE ROWNUM = 1;
END USP_GET_SESSION_ROLE;
/

-- Returns the user ID based on their username and role
CREATE OR REPLACE PROCEDURE USP_GET_USER_ID (
    p_username IN VARCHAR2,
    p_role     IN VARCHAR2,
    p_cursor   OUT SYS_REFCURSOR
) AUTHID DEFINER AS
BEGIN
    IF UPPER(p_role) = 'RL_PATIENT' THEN
        OPEN p_cursor FOR
            SELECT patient_id AS ID FROM hospital.patient WHERE username_db = UPPER(TRIM(p_username));
    ELSE
        OPEN p_cursor FOR
            SELECT staff_id AS ID FROM hospital.staff WHERE username_db = UPPER(TRIM(p_username));
    END IF;
END USP_GET_USER_ID;
/
-- Cháº¡y dÆ°á»›i quyá»n: hospital_dba | Container: PDB_QLYT
-- This script contains the procedures for linking DB users to App tables.

ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

-- CREATE LINKED USER
CREATE OR REPLACE PROCEDURE USP_CREATE_USER_LINKED (
    p_username IN VARCHAR2,
    p_password IN VARCHAR2,
    p_full_name IN NVARCHAR2,
    p_gender IN NVARCHAR2,
    p_birthdate IN DATE,
    p_id_card IN VARCHAR2,
    p_role IN VARCHAR2, 
    p_phone IN VARCHAR2 DEFAULT NULL,
    p_hometown IN NVARCHAR2 DEFAULT NULL,
    p_dept_id IN VARCHAR2 DEFAULT NULL,
    p_facility IN NVARCHAR2 DEFAULT NULL,
    p_house_no IN NVARCHAR2 DEFAULT NULL,
    p_street IN NVARCHAR2 DEFAULT NULL,
    p_district IN NVARCHAR2 DEFAULT NULL,
    p_city_province IN NVARCHAR2 DEFAULT NULL,
    p_medical_history IN NCLOB DEFAULT NULL,
    p_family_medical_history IN NCLOB DEFAULT NULL,
    p_drug_allergies IN NCLOB DEFAULT NULL
) AUTHID CURRENT_USER AS
    v_user VARCHAR2(128);
    v_id   VARCHAR2(10);
    v_role VARCHAR2(128);
    v_pwd  VARCHAR2(4000);
BEGIN
    v_user := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_username)));
    v_role := UPPER(TRIM(p_role));
    v_pwd  := REPLACE(p_password, '"', '""');

    -- 1. Create DB User
    EXECUTE IMMEDIATE 'CREATE USER ' || v_user || ' IDENTIFIED BY "' || v_pwd || '"';
    EXECUTE IMMEDIATE 'GRANT CREATE SESSION TO ' || v_user;
    EXECUTE IMMEDIATE 'GRANT EXECUTE ON hospital_dba.USP_GET_SESSION_ROLE TO ' || v_user;
    EXECUTE IMMEDIATE 'GRANT EXECUTE ON hospital_dba.USP_GET_USER_ID TO ' || v_user;
    EXECUTE IMMEDIATE 'GRANT ' || v_role || ' TO ' || v_user;
    EXECUTE IMMEDIATE 'ALTER USER ' || v_user || ' DEFAULT ROLE ALL';

    -- 2. Insert into App Tables (is_active is 1 by default)
    IF v_role = 'RL_PATIENT' THEN
        v_id := 'BN' || LPAD(hospital.SEQ_PATIENT_ID.NEXTVAL, 6, '0');
        INSERT INTO hospital.patient (patient_id, full_name, gender, birthdate, id_card, house_no, street, district, city_province, medical_history, family_medical_history, drug_allergies, username_db, is_active)
        VALUES (v_id, p_full_name, p_gender, p_birthdate, p_id_card, p_house_no, p_street, p_district, p_city_province, p_medical_history, p_family_medical_history, p_drug_allergies, v_user, 1);
    ELSE
        v_id := 'NV' || LPAD(hospital.SEQ_STAFF_ID.NEXTVAL, 6, '0');
        INSERT INTO hospital.staff (staff_id, full_name, gender, birthdate, id_card, phone, hometown, dept_id, facility, staff_role, username_db, is_active)
        VALUES (v_id, p_full_name, p_gender, p_birthdate, p_id_card, p_phone, p_hometown, 
                CASE WHEN v_role = 'RL_DOCTOR' THEN p_dept_id ELSE NULL END, 
                p_facility,
                CASE v_role 
                    WHEN 'RL_DOCTOR' THEN UNISTR('B\00E1c s\0129')
                    WHEN 'RL_COORDINATOR' THEN UNISTR('\0110i\1EC1u ph\1ED1i vi\00EAn')
                    WHEN 'RL_TECHNICIAN' THEN UNISTR('K\1EF9 thu\1EADt vi\00EAn')
                END, 
                v_user, 1);
    END IF;
    
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/

-- UPDATE LINKED USER
CREATE OR REPLACE PROCEDURE USP_UPDATE_USER_LINKED (
    p_username IN VARCHAR2,
    p_full_name IN NVARCHAR2,
    p_gender IN NVARCHAR2,
    p_birthdate IN DATE,
    p_id_card IN VARCHAR2,
    p_role IN VARCHAR2,
    p_phone IN VARCHAR2 DEFAULT NULL,
    p_hometown IN NVARCHAR2 DEFAULT NULL,
    p_dept_id IN VARCHAR2 DEFAULT NULL,
    p_facility IN NVARCHAR2 DEFAULT NULL,
    p_house_no IN NVARCHAR2 DEFAULT NULL,
    p_street IN NVARCHAR2 DEFAULT NULL,
    p_district IN NVARCHAR2 DEFAULT NULL,
    p_city_province IN NVARCHAR2 DEFAULT NULL,
    p_medical_history IN NCLOB DEFAULT NULL,
    p_family_medical_history IN NCLOB DEFAULT NULL,
    p_drug_allergies IN NCLOB DEFAULT NULL
) AUTHID CURRENT_USER AS
    v_user VARCHAR2(128);
    v_old_role VARCHAR2(128);
    v_new_role VARCHAR2(128);
BEGIN
    v_user := UPPER(TRIM(p_username));
    v_new_role := UPPER(TRIM(p_role));

    -- Guard: Detect cross-type role change (Staff<->Patient) which is not supported
    BEGIN
        SELECT granted_role INTO v_old_role
        FROM DBA_ROLE_PRIVS
        WHERE grantee = v_user
          AND granted_role IN ('RL_PATIENT', 'RL_DOCTOR', 'RL_COORDINATOR', 'RL_TECHNICIAN')
          AND ROWNUM = 1;

        IF (v_old_role = 'RL_PATIENT' AND v_new_role != 'RL_PATIENT')
           OR (v_old_role != 'RL_PATIENT' AND v_new_role = 'RL_PATIENT') THEN
            RAISE_APPLICATION_ERROR(-20050, UNISTR('Kh\00F4ng th\1EC3 chuy\1EC3n \0111\1ED5i gi\1EEFa Nh\00E2n vi\00EAn v\00E0 B\1EC7nh nh\00E2n. Vui l\00F2ng t\1EA1o t\00E0i kho\1EA3n m\1EDBi.'));
        END IF;
    EXCEPTION WHEN NO_DATA_FOUND THEN NULL;
    END;

    -- 1. Update Role if changed (Staff only)
    IF v_new_role != 'RL_PATIENT' THEN
        BEGIN
            SELECT granted_role INTO v_old_role
            FROM DBA_ROLE_PRIVS
            WHERE grantee = v_user
              AND granted_role IN ('RL_DOCTOR', 'RL_COORDINATOR', 'RL_TECHNICIAN')
              AND ROWNUM = 1;
              
            IF v_old_role != v_new_role THEN
                EXECUTE IMMEDIATE 'REVOKE ' || v_old_role || ' FROM ' || v_user;
                EXECUTE IMMEDIATE 'GRANT ' || v_new_role || ' TO ' || v_user;
            END IF;
        EXCEPTION WHEN NO_DATA_FOUND THEN
            EXECUTE IMMEDIATE 'GRANT ' || v_new_role || ' TO ' || v_user;
        END;
        
        UPDATE hospital.staff
        SET full_name = p_full_name,
            gender = p_gender,
            birthdate = p_birthdate,
            id_card = p_id_card,
            phone = p_phone,
            hometown = p_hometown,
            dept_id = p_dept_id,
            facility = p_facility,
            staff_role = CASE v_new_role 
                            WHEN 'RL_DOCTOR' THEN UNISTR('B\00E1c s\0129')
                            WHEN 'RL_COORDINATOR' THEN UNISTR('\0110i\1EC1u ph\1ED1i vi\00EAn')
                            WHEN 'RL_TECHNICIAN' THEN UNISTR('K\1EF9 thu\1EADt vi\00EAn')
                         END
        WHERE username_db = v_user;
    ELSE
        UPDATE hospital.patient
        SET full_name = p_full_name,
            gender = p_gender,
            birthdate = p_birthdate,
            id_card = p_id_card,
            house_no = p_house_no,
            street = p_street,
            district = p_district,
            city_province = p_city_province,
            medical_history = p_medical_history,
            family_medical_history = p_family_medical_history,
            drug_allergies = p_drug_allergies
        WHERE username_db = v_user;
    END IF;
    
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/

-- DROP LINKED USER (Soft Delete: Lock and Set Inactive)
CREATE OR REPLACE PROCEDURE USP_DROP_USER_LINKED (
    p_username IN VARCHAR2
) AUTHID CURRENT_USER AS
    v_user VARCHAR2(128);
BEGIN
    v_user := DBMS_ASSERT.SIMPLE_SQL_NAME(UPPER(TRIM(p_username)));
    
    UPDATE hospital.staff SET is_active = 0 WHERE username_db = v_user;
    UPDATE hospital.patient SET is_active = 0 WHERE username_db = v_user;
    
    EXECUTE IMMEDIATE 'ALTER USER ' || v_user || ' ACCOUNT LOCK';
    
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/

-- GET LINKED USER INFO
CREATE OR REPLACE PROCEDURE USP_GET_USER_INFO (
    p_username IN VARCHAR2,
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
    v_user VARCHAR2(128);
    v_role VARCHAR2(128);
BEGIN
    v_user := UPPER(TRIM(p_username));
    
    SELECT granted_role INTO v_role
    FROM DBA_ROLE_PRIVS
    WHERE grantee = v_user
      AND granted_role IN ('RL_PATIENT', 'RL_DOCTOR', 'RL_COORDINATOR', 'RL_TECHNICIAN')
      AND ROWNUM = 1;
      
    IF v_role = 'RL_PATIENT' THEN
        OPEN p_cursor FOR
            SELECT 'RL_PATIENT' as ROLE, patient_id as ID, full_name, gender, birthdate, id_card, 
                   NULL as phone, NULL as hometown, NULL as DEPT_ID, NULL as FACILITY,
                   house_no, street, district, city_province, medical_history, family_medical_history, drug_allergies,
                   is_active
            FROM hospital.patient
            WHERE username_db = v_user;
    ELSE
        OPEN p_cursor FOR
            SELECT v_role as ROLE, staff_id as ID, full_name, gender, birthdate, id_card, 
                   phone, hometown, dept_id, facility as FACILITY,
                   NULL as house_no, NULL as street, NULL as district, NULL as city_province, NULL as medical_history, NULL as family_medical_history, NULL as drug_allergies,
                   is_active
            FROM hospital.staff
            WHERE username_db = v_user;
    END IF;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        OPEN p_cursor FOR SELECT NULL FROM DUAL WHERE 1=0;
END;
/

-- GET ACTIVE DOCTORS (For new medical records)
CREATE OR REPLACE PROCEDURE USP_GET_ACTIVE_DOCTORS (
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT s.staff_id, s.full_name, s.dept_id, d.dept_name
        FROM hospital.staff s
        LEFT JOIN hospital.department d ON s.dept_id = d.dept_id
        WHERE s.staff_role = UNISTR('B\00E1c s\0129') AND s.is_active = 1
        ORDER BY s.full_name;
END;
/

-- GET ACTIVE PATIENTS
CREATE OR REPLACE PROCEDURE USP_GET_ACTIVE_PATIENTS (
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT patient_id, full_name, gender, birthdate, id_card
        FROM hospital.patient
        WHERE is_active = 1
        ORDER BY full_name;
END;
/

-- GET ACTIVE STAFF
CREATE OR REPLACE PROCEDURE USP_GET_ACTIVE_STAFF (
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT staff_id, full_name, staff_role, dept_id
        FROM hospital.staff
        WHERE is_active = 1
        ORDER BY staff_role, full_name;
END;
/

-- GET ALL DEPARTMENTS
CREATE OR REPLACE PROCEDURE USP_GET_ALL_DEPARTMENTS (
    p_cursor OUT SYS_REFCURSOR
) AUTHID CURRENT_USER AS
BEGIN
    OPEN p_cursor FOR
        SELECT dept_id, dept_name FROM hospital.department ORDER BY dept_id;
END;
/
