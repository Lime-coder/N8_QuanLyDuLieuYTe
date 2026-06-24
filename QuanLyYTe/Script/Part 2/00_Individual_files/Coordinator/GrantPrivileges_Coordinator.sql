-- ==============================================================================
-- File: GrantPrivileges_Coordinator.sql
-- Mục đích: Cấp quyền tối thiểu cho vai trò Điều phối viên
-- Run as: HOSPITAL_DBA
-- ==============================================================================

ALTER SESSION SET CURRENT_SCHEMA = hospital;

-- Bảng bệnh nhân: Điều phối viên được xem, thêm, sửa (thừa vì thao tác qua Stored Procedure)
-- GRANT SELECT, INSERT, UPDATE ON hospital.patient TO rl_coordinator;

-- Hồ sơ bệnh án:
-- Được xem, thêm hồ sơ mới (thừa vì thao tác qua Stored Procedure)
-- GRANT SELECT, INSERT ON hospital.medical_record TO rl_coordinator;

-- Chỉ được phân công bác sĩ/khoa (thừa vì thao tác qua Stored Procedure)
-- GRANT UPDATE (doctor_id, dept_id) ON hospital.medical_record TO rl_coordinator;

-- Dịch vụ hỗ trợ chẩn đoán:
-- Được xem danh sách dịch vụ (thừa vì thao tác qua Stored Procedure)
-- GRANT SELECT ON hospital.service_record TO rl_coordinator;

-- Chỉ được phân công kỹ thuật viên (thừa vì thao tác qua Stored Procedure)
-- GRANT UPDATE (technician_id) ON hospital.service_record TO rl_coordinator;

-- Xem danh mục khoa (thừa vì thao tác qua Stored Procedure)
-- GRANT SELECT ON hospital.department TO rl_coordinator;

-- STAFF:
-- Query trực tiếp STAFF sẽ bị VPD lọc chỉ thấy chính mình (thừa vì thao tác qua Stored Procedure)
-- GRANT SELECT ON hospital.staff TO rl_coordinator;

-- Chỉ được cập nhật thông tin cá nhân hợp lệ (thừa vì thao tác qua Stored Procedure)
-- GRANT UPDATE (phone, hometown) ON hospital.staff TO rl_coordinator;

-- View phục vụ điều phối bác sĩ/kỹ thuật viên (thừa vì thao tác qua Stored Procedure)
-- GRANT SELECT ON hospital.VW_COORD_DOCTORS TO rl_coordinator;
-- GRANT SELECT ON hospital.VW_COORD_TECHNICIANS TO rl_coordinator;