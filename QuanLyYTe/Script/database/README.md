# QuanLyYTe Database Scripts

Thư mục này chứa toàn bộ các kịch bản SQL để khởi tạo và cấu hình cơ sở dữ liệu cho dự án Quản lý Y tế (QuanLyYTe), đã được phân tách rõ ràng theo từng phân hệ (Schema, Dữ liệu mẫu, RBAC, VPD, OLS, Audit).

## Cấu trúc thư mục

- `00_env/`: Thiết lập môi trường (Container, Audit system).
- `01_schema/`: Tạo bảng, khóa ngoại, chỉ mục.
- `02_seed_data/`: Dữ liệu mẫu (Phòng ban, Nhân viên, Bệnh nhân, Hồ sơ...).
- `03_accounts_roles/`: Khởi tạo Roles và phân quyền cơ bản.
- `04_rbac/`: Các View, Procedure, và Grants theo từng Role (Patient, Technician).
- `05_vpd/`: Cấu hình Virtual Private Database (Doctor, Coordinator).
- `06_ols/`: Cấu hình Oracle Label Security (Quản lý Thông báo).
- `07_audit/`: Cấu hình Giám sát (FGA, Unified Audit, Standard Audit).
- `08_backup_restore/`: Các kịch bản Backup/Restore (Expdp/Impdp).
- `AIO/`: Thư mục All-In-One dành cho ai muốn chạy các kịch bản gom sẵn.

## Hướng dẫn thực thi

Do hệ thống yêu cầu chuyển đổi qua lại giữa nhiều session (`SYSDBA`, `HOSPITAL_DBA`, `HOSPITAL`) và khởi động lại dịch vụ (khi cấu hình OLS, Audit), việc chạy một file `run_all.sql` duy nhất không được khuyến khích vì có thể gây lỗi bảo mật. Vui lòng thực thi các kịch bản theo thứ tự thư mục (từ 00 đến 07) và chú ý tài khoản chạy ở đầu mỗi file.
