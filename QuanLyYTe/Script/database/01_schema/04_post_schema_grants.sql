-- ==============================================================================
-- 01_post_schema_grants.sql (Bổ sung từ Part 1)
-- Ch?y du?i quy?n: SYS AS SYSDBA
-- Cấp quyền thao tác trên các table/sequence của hospital cho hospital_dba
-- Yêu cầu: Chạy SAU khi đã khởi tạo xong các tables ở bước 01
-- ==============================================================================
ALTER SESSION SET CONTAINER = PDB_QLYT;

-- Cho phép hospital_dba có quyền SELECT và có thể GRANT SELECT tiếp cho user khác (phục vụ role/view/VPD)
GRANT SELECT ON hospital.department     TO hospital_dba WITH GRANT OPTION;
GRANT SELECT ON hospital.staff          TO hospital_dba WITH GRANT OPTION;
GRANT SELECT ON hospital.patient        TO hospital_dba WITH GRANT OPTION;
GRANT SELECT ON hospital.medical_record TO hospital_dba WITH GRANT OPTION;
GRANT SELECT ON hospital.service_record TO hospital_dba WITH GRANT OPTION;
GRANT SELECT ON hospital.prescription   TO hospital_dba WITH GRANT OPTION;

-- Cho phép hospital_dba thao tác DML trên staff và patient (để quản lý nhân viên, bệnh nhân qua trigger hoặc package)
GRANT INSERT, UPDATE, DELETE ON hospital.staff TO hospital_dba WITH GRANT OPTION;
GRANT INSERT, UPDATE, DELETE ON hospital.patient TO hospital_dba WITH GRANT OPTION;
GRANT INSERT, UPDATE, DELETE ON hospital.medical_record TO hospital_dba WITH GRANT OPTION;
GRANT INSERT, UPDATE, DELETE ON hospital.service_record TO hospital_dba WITH GRANT OPTION;
GRANT INSERT, UPDATE, DELETE ON hospital.prescription TO hospital_dba WITH GRANT OPTION;

-- Cấp quyền dùng Sequence để generate ID tự động cho tài khoản Linked
GRANT SELECT ON hospital.SEQ_STAFF_ID TO hospital_dba;
GRANT SELECT ON hospital.SEQ_PATIENT_ID TO hospital_dba;
