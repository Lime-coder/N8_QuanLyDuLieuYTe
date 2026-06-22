# Thư mục AIO (All-In-One)

Thư mục này chứa các kịch bản giúp khởi tạo toàn bộ hệ thống cơ sở dữ liệu cho dự án Quản lý Y tế một cách tự động, cũng như xóa trắng dữ liệu để làm lại từ đầu.
Do hệ thống yêu cầu chuyển đổi qua lại giữa nhiều session (`SYSDBA`, `HOSPITAL_DBA`, `HOSPITAL`), các kịch bản SQL được thiết kế để yêu cầu người dùng nhập mật khẩu ở đầu chương trình và sẽ tự động `CONNECT` đúng tài khoản cho từng thao tác.

## Cách sử dụng

### 1. Khởi tạo Cơ sở dữ liệu (Cài đặt)
- **Bằng Batch Script:** Chạy file `run_all.bat`. Script sẽ tự động gọi SQL*Plus và lưu toàn bộ kết quả vào `AIO_run_log.txt`.
- **Bằng SQL*Plus:** Đăng nhập dưới quyền sysdba và gõ `@run_all.sql`.

*Chú ý: Bạn cần khởi động lại cơ sở dữ liệu (hoặc ngắt kết nối session) sau khi chạy xong để các thiết lập Audit và OLS có hiệu lực hoàn toàn.*

### 2. Xóa toàn bộ Cơ sở dữ liệu (Clean)
- **Bằng Batch Script:** Chạy file `clean_all.bat`. Cảnh báo: Thao tác này sẽ dọn dẹp sạch sẽ toàn bộ database, các user bệnh nhân/nhân viên, policy (OLS, Audit), role, schema, v.v. Kết quả lưu vào `AIO_clean_log.txt`.
- **Bằng SQL*Plus:** Đăng nhập dưới quyền sysdba và gõ `@clean_all.sql`.
