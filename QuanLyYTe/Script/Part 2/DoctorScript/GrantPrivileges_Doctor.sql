-- Chạy bằng hospital_dba
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

-- Cấp quyền thực thi cho các Role (Để Bác sĩ thấy được procedure)
GRANT EXECUTE ON hospital_dba.USP_GET_SESSION_ROLE TO rl_doctor;
GRANT EXECUTE ON hospital_dba.USP_GET_GRANTED_ROLE TO rl_doctor;

COMMIT;
