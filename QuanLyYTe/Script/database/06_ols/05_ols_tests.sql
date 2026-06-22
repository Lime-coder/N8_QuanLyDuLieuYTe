-- ==============================================================================
-- 05_ols_tests.sql
-- Run as: sysdba
-- ==============================================================================
ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

-- Cấp quyền execute
GRANT EXECUTE ON hospital.USP_ADD_NOTIFICATION TO rl_dba;
GRANT EXECUTE ON hospital.USP_ADD_NOTIFICATION TO rl_coordinator;
GRANT EXECUTE ON hospital.USP_GET_NOTIFICATIONS TO rl_dba;
GRANT EXECUTE ON hospital.USP_GET_NOTIFICATIONS TO rl_coordinator;
GRANT EXECUTE ON hospital.USP_GET_NOTIFICATIONS TO rl_doctor;
GRANT EXECUTE ON hospital.USP_GET_NOTIFICATIONS TO rl_technician;
GRANT EXECUTE ON hospital.USP_GET_NOTIFICATIONS TO rl_patient;

GRANT SELECT ON hospital.seq_notification_id TO rl_dba;
GRANT SELECT ON hospital.seq_notification_id TO rl_coordinator;
GRANT INSERT ON hospital.notification TO rl_coordinator;

GRANT EXECUTE ON hospital_dba.USP_GET_USER_OLS_LABEL TO rl_dba;
GRANT EXECUTE ON hospital_dba.USP_SET_USER_OLS_LABEL TO rl_dba;

BEGIN
    DELETE FROM hospital.staff WHERE username_db IN ('U1','U2','U3','U4','U5','U6','U7','U8');
    
    INSERT INTO hospital.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, staff_role, username_db) 
    VALUES ('NV_U1', N'Giám đốc OLS', N'Nam', TO_DATE('1970-01-01','YYYY-MM-DD'), 'OLS_001', N'Chưa rõ', '0000000000', N'Điều phối viên', 'U1');

    INSERT INTO hospital.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, staff_role, dept_id, username_db) 
    VALUES ('NV_U2', N'Lãnh đạo Tim mạch', N'Nữ', TO_DATE('1980-01-01','YYYY-MM-DD'), 'OLS_002', N'Chưa rõ', '0000000000', N'Bác sĩ', 'PB03', 'U2');

    INSERT INTO hospital.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, staff_role, dept_id, username_db) 
    VALUES ('NV_U3', N'Lãnh đạo Thần kinh', N'Nam', TO_DATE('1981-01-01','YYYY-MM-DD'), 'OLS_003', N'Chưa rõ', '0000000000', N'Bác sĩ', 'PB02', 'U3');

    INSERT INTO hospital.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, staff_role, dept_id, username_db) 
    VALUES ('NV_U4', N'Nhân viên Thần kinh', N'Nữ', TO_DATE('1990-01-01','YYYY-MM-DD'), 'OLS_004', N'Chưa rõ', '0000000000', N'Kỹ thuật viên', 'PB02', 'U4');

    INSERT INTO hospital.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, staff_role, dept_id, username_db) 
    VALUES ('NV_U5', N'Nhân viên Tim mạch', N'Nam', TO_DATE('1992-01-01','YYYY-MM-DD'), 'OLS_005', N'Chưa rõ', '0000000000', N'Kỹ thuật viên', 'PB03', 'U5');

    INSERT INTO hospital.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, staff_role, dept_id, username_db) 
    VALUES ('NV_U6', N'Lãnh đạo phòng HCM', N'Nữ', TO_DATE('1985-01-01','YYYY-MM-DD'), 'OLS_006', N'Chưa rõ', '0000000000', N'Bác sĩ', 'PB03', 'U6');

    INSERT INTO hospital.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, staff_role, username_db) 
    VALUES ('NV_U7', N'Lãnh đạo phòng Tổng hợp', N'Nam', TO_DATE('1984-01-01','YYYY-MM-DD'), 'OLS_007', N'Chưa rõ', '0000000000', N'Bác sĩ', 'U7');

    INSERT INTO hospital.staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, staff_role, dept_id, username_db) 
    VALUES ('NV_U8', N'Nhân viên Tiêu hóa', N'Nữ', TO_DATE('1995-01-01','YYYY-MM-DD'), 'OLS_008', N'Chưa rõ', '0000000000', N'Kỹ thuật viên', 'PB01', 'U8');

    COMMIT;
END;
/
