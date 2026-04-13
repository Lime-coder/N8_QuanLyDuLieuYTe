-- Chuyển vào CDB$ROOT
ALTER SESSION SET CONTAINER = CDB$ROOT;

---- Tạo PDB (Nếu đã tạo rồi thì bỏ qua bước này)
-- CREATE PLUGGABLE DATABASE PDB_QLYT ADMIN USER pdb_admin IDENTIFIED BY 123 FILE_NAME_CONVERT = ('pdbseed', 'pdb_qlyt');

---- Mở và Lưu trạng thái
--ALTER PLUGGABLE DATABASE PDB_QLYT OPEN;
--ALTER PLUGGABLE DATABASE PDB_QLYT SAVE STATE;

-- Chuyển vào PDB để cấp quyền
ALTER SESSION SET CONTAINER = PDB_QLYT;

---- 1. Tạo User (Nếu đã có thì bỏ qua)
-- CREATE USER hospital_dba IDENTIFIED BY 123;

-- 2. Cấp quyền DBA cơ bản
GRANT CONNECT, RESOURCE, DBA TO hospital_dba;

-- 3. CẤP QUYỀN TRỰC TIẾP (Bắt buộc để Stored Procedure hoạt động)
-- Quyền đọc các bảng hệ thống (để hiện danh sách User/Role lên UI)
GRANT SELECT ANY DICTIONARY TO hospital_dba;

-- Quyền thực hiện lệnh GRANT cho người khác bên trong SP
GRANT GRANT ANY PRIVILEGE TO hospital_dba;
GRANT GRANT ANY OBJECT PRIVILEGE TO hospital_dba;
GRANT GRANT ANY ROLE TO hospital_dba;

-- Quyền tạo View (Dùng cho chức năng giới hạn cột SELECT)
GRANT CREATE ANY VIEW TO hospital_dba;

-- Cấp quyền để HOSPITAL_DBA có thể quản lý User cho Phân hệ 1
GRANT
    CREATE USER,
    ALTER USER,
    DROP USER
TO hospital_dba;

GRANT
    GRANT ANY ROLE
TO hospital_dba;
