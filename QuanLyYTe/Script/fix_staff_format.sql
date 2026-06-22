ALTER SESSION SET CONTAINER = PDB_QLYT;

-- Sửa lỗi hiển thị tiếng Việt cho cột facility
UPDATE hospital.staff SET facility = N'Hà Nội' WHERE facility LIKE 'H% N_i' OR facility LIKE 'Ha N?i';
UPDATE hospital.staff SET facility = N'Hải Phòng' WHERE facility LIKE 'H_i Ph_ng' OR facility LIKE 'H_i Phong' OR facility LIKE 'H?i Phong';
UPDATE hospital.staff SET facility = N'Hồ Chí Minh' WHERE facility LIKE 'H_ Ch_ Minh' OR facility LIKE 'H_ Chi Minh' OR facility LIKE 'H? Chi Minh';

-- Sửa lỗi hiển thị tiếng Việt cho cột gender nếu có
UPDATE hospital.staff SET gender = N'Nữ' WHERE gender = 'N?' OR gender LIKE 'N_';

-- Sửa một số quê quán (hometown) nếu bị lỗi (bạn có thể bổ sung thêm nếu cần)
UPDATE hospital.staff SET hometown = N'Hà Nội' WHERE hometown LIKE 'H% N_i';
UPDATE hospital.staff SET hometown = N'Đà Nẵng' WHERE hometown LIKE 'D_ N_ng' OR hometown LIKE 'D_ N?ng';
UPDATE hospital.staff SET hometown = N'Hải Phòng' WHERE hometown LIKE 'H_i Ph_ng';
UPDATE hospital.staff SET hometown = N'Hồ Chí Minh' WHERE hometown LIKE 'H_ Ch_ Minh';
UPDATE hospital.staff SET hometown = N'Cần Thơ' WHERE hometown LIKE 'C_n Th_';

COMMIT;
EXIT;
