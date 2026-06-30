-- ==============================================================================
-- 03_technician_grants.sql
-- Mục đích: Cấp quyền (GRANT) cho Role rl_technician.
-- Người thực thi: hospital_dba (hoặc SYS)
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

-- Đã chuyển sang dùng AUTHID DEFINER trên SP, KHÔNG CẦN cấp quyền trực tiếp trên View nữa!
-- Chỉ cần cấp quyền EXECUTE trên SP là đủ, khóa chặt cửa sau 100%.

GRANT EXECUTE ON hospital_dba.USP_GET_TECHNICIAN_SERVICE_RECORDS TO rl_technician;
GRANT EXECUTE ON hospital_dba.USP_UPDATE_TECHNICIAN_SERVICE_RESULT TO rl_technician;
GRANT EXECUTE ON hospital_dba.USP_GET_TECHNICIAN_PERSONAL_INFO TO rl_technician;
GRANT EXECUTE ON hospital_dba.USP_UPDATE_TECHNICIAN_PERSONAL_INFO TO rl_technician;
