## PHẦN IX. SAO LƯU VÀ PHỤC HỒI (BACKUP & RECOVERY)

### 1. Lý thuyết chuyên sâu về Sao lưu và Phục hồi
Hệ thống Y tế hoạt động 24/7, dữ liệu tăng trưởng liên tục và chứa thông tin sinh tử của người bệnh. Do đó, bảo mật không chỉ dừng ở việc chống người ngoài truy cập trái phép (Confidentiality) hay chống chỉnh sửa sai (Integrity), mà còn phải đảm bảo tính Sẵn sàng (Availability) - tức là dữ liệu không bao giờ được mất dù xảy ra thảm họa phần cứng, phần mềm, hay mã độc tống tiền (Ransomware).

**Các mức độ sao lưu trong kiến trúc Oracle Database:**
- **Cold Backup (Sao lưu nguội):** Yêu cầu phải tắt hoàn toàn Database (trạng thái MOUNT nhưng không OPEN) để copy các file vật lý (Datafiles, Control files, Redo log files). Ưu điểm là an toàn tuyệt đối 100% không sợ sai lệch dữ liệu, nhưng khuyết điểm chí mạng là gây gián đoạn hệ thống. Bệnh viện không thể dừng nhận bệnh nhân để chạy backup.
- **Hot Backup (Sao lưu nóng) với RMAN (Recovery Manager):** Đây là tiêu chuẩn vàng của Oracle. Nó cho phép backup khi CSDL đang hoạt động. RMAN sao lưu ở cấp độ block vật lý, có thể backup Incremental (chỉ sao lưu những block thay đổi so với lần trước), giúp tiết kiệm thời gian và dung lượng lưu trữ cực lớn. Nó còn có khả năng khôi phục đến một thời điểm chính xác (Point-in-Time Recovery) nhờ việc áp dụng các Archive Redo Logs.
- **Logical Backup (Sao lưu logic) với Data Pump (EXPDP/IMPDP):** Khác với RMAN làm việc trên file vật lý, Data Pump xuất trực tiếp dữ liệu (Table, View, Schema, Procedure) ra các file nhị phân (Dump file `.dmp`). Nó cực kỳ linh hoạt nếu ta chỉ muốn mang dữ liệu của schema `hospital` sang một máy chủ khác để test, hoặc chỉ khôi phục lại 1 bảng `medical_record` bị rớt dữ liệu do lỗi con người (Human Error) mà không cần khôi phục cả server.

### 2. Giải thích cơ chế và lý do chọn phương pháp sao lưu
Dự án quyết định lựa chọn sử dụng cơ chế **Oracle Data Pump (Logical Backup)** làm phương pháp trình diễn chính, kết hợp với các script Powershell tự động hóa.

*Lý do lựa chọn Data Pump:*
1. **Tính Granularity (Độ mịn):** Rất dễ dàng để backup riêng lược đồ `hospital`. Trong khi RMAN thường backup toàn bộ PDB hoặc CDB.
2. **Khả năng tái cấu trúc (Remapping):** Khi xảy ra thảm họa, ta có thể import dữ liệu vào một Schema có tên khác, hoặc PDB khác để kiểm tra tính toàn vẹn trước khi chuyển sang production. DataPump hỗ trợ `REMAP_SCHEMA`, `REMAP_TABLESPACE`.
3. **Môi trường đồ án:** Trong phạm vi học thuật và thử nghiệm trên máy cá nhân, Data Pump dễ cấu hình, file dump gọn nhẹ và có thể chạy qua kịch bản `Run-DataPumpImportToRestore.ps1` một cách trực quan, sinh động để chứng minh quy trình.

*Cơ chế triển khai:*
- Thiết lập một Oracle Directory trỏ đến thư mục lưu trữ an toàn trên máy chủ.
- Cấp quyền `EXP_FULL_DATABASE` hoặc quyền read/write trên directory cho tài khoản thực hiện backup (`hospital_dba`).
- Thực thi lệnh `expdp` với các tham số nén dữ liệu. File kết quả `.dmp` có thể được đưa lên Cloud hoặc ổ cứng ngoài định kỳ mỗi đêm lúc 2h sáng (thời điểm bệnh viện ít giao dịch nhất).

### 3. Kết quả đạt được và So sánh
**Kết quả:** Hệ thống có khả năng xuất toàn bộ bảng dữ liệu, views, procedures và toàn bộ thiết lập VPD/OLS sang file dump thành công. Thử nghiệm kịch bản xóa nhầm bảng (DROP TABLE), Data Pump Import (`impdp`) đã phục hồi dữ liệu hoàn chỉnh bao gồm cả các ràng buộc (Constraints/Indexes/Triggers) trong thời gian ngắn mà không làm ảnh hưởng đến các Schema hệ thống khác.

**So sánh các cơ chế:**
- *Data Pump so với RMAN:* RMAN phù hợp với chiến lược Disaster Recovery (DR) toàn diện và chống mất mát dữ liệu đến từng giây cuối cùng (Zero Data Loss) nhờ Redo Logs. Data Pump không thể khôi phục đến "thời điểm 2 giờ 15 phút 10 giây", nó chỉ khôi phục dữ liệu tại chính thời điểm file dump được tạo ra. 
- *Kết luận thực tiễn:* Các bệnh viện lớn trong thực tế luôn kết hợp CẢ HAI. RMAN chạy chìm liên tục ở cấp độ vật lý để đề phòng hỏng ổ cứng, còn Data Pump chạy định kỳ hàng tuần để tạo các bản snapshot gửi cho đội ngũ kiểm thử hoặc lưu trữ dài hạn phục vụ kiểm toán nhà nước.

---

## PHẦN X. ỨNG DỤNG C# WINFORMS

### 1. Kiến trúc kết nối và Bảo mật
Phần mềm ứng dụng (Client) được phát triển trên nền tảng .NET Framework sử dụng giao diện WinForms, đóng vai trò là cầu nối giữa Người dùng Y tế và Oracle Database.

Ứng dụng tuân thủ nghiêm ngặt mô hình **2-Tier (Client-Server)** với định hướng bảo mật tối đa:
- **Xác thực trực tiếp (Direct Authentication):** Ứng dụng không sử dụng một tài khoản "super user" duy nhất để kết nối vào CSDL (Connection Pooling chung). Thay vào đó, khi người dùng (Bác sĩ, Lễ tân) gõ User/Pass trên giao diện đăng nhập WinForms, chuỗi kết nối (`OracleConnection`) được khởi tạo sử dụng chính thông tin đăng nhập đó. 
- *Hiệu quả:* Điều này đảm bảo rằng `SYS_CONTEXT('USERENV', 'SESSION_USER')` mà Oracle nhận được chính xác là tài khoản của người thao tác. Mọi chính sách VPD, OLS và hệ thống Audit FGA sẽ hoạt động chính xác đến từng cá nhân. Nếu ứng dụng dùng 1 tài khoản Proxy chung, VPD sẽ hoàn toàn bị vô hiệu hóa vì Oracle chỉ nhìn thấy 1 user chung đó.

### 2. Sự phản chiếu của cơ chế bảo mật trên giao diện
Mặc dù CSDL thực hiện việc chặn quyền dữ liệu, giao diện WinForms phải được thiết kế để xử lý các Exceptions trả về từ Oracle một cách mượt mà, thân thiện với người dùng (UX).

- Khi Kỹ thuật viên vô tình nhấn vào nút "Thêm Bệnh Án" (hành động không có quyền theo RBAC), Oracle sẽ văng lỗi `ORA-01031: insufficient privileges`. WinForms bắt (catch) `OracleException` này và hiển thị hộp thoại "Bạn không có quyền thực hiện chức năng này", thay vì làm treo chương trình.
- Nhờ VPD, khi Bác sĩ A mở màn hình danh sách, GridControl tự động hiển thị 10 dòng (thuộc về bác sĩ A), thay vì bác sĩ B nhìn thấy 10 dòng. Application không hề có dòng code nào kiểu `WHERE Doctor_id = 'A'`. Giao diện trở nên vô cùng sạch sẽ và loại bỏ rủi ro lập trình viên vô tình bỏ quên điều kiện WHERE.

---

## PHẦN XI. KẾT LUẬN

Qua quá trình nghiên cứu, thiết kế và triển khai dự án "Quản lý dữ liệu y tế", nhóm đã vận dụng thành công các nguyên lý lý thuyết phức tạp của môn học An toàn và Bảo mật Hệ thống Thông tin vào một bài toán thực tiễn có tính ứng dụng cao. 

Dự án đã chứng minh rằng: Một hệ thống phần mềm an toàn không thể chỉ dựa vào lớp xác thực lỏng lẻo ở tầng ứng dụng (Application Level). Tầng ứng dụng rất dễ bị bypass (bằng SQL Injection hoặc API abuse). Hệ thống phòng thủ chiều sâu (Defense-in-Depth) phải được xây dựng từ phần lõi - chính là Hệ quản trị Cơ sở dữ liệu (Database Kernel).

Việc tích hợp đồng bộ RBAC (cấp quyền tĩnh), VPD (kiểm soát truy cập theo ngữ cảnh tự động), OLS (bảo vệ tài liệu mật bằng thuật toán MAC khắt khe), FGA (ghi vết chi tiết chống gian lận nội bộ), và DataPump (đảm bảo tính sẵn sàng) đã tạo nên một pháo đài dữ liệu kiên cố. Dù kẻ tấn công là tin tặc bên ngoài hay người dùng nội bộ (Insider Threat) có ý đồ xấu, thì dữ liệu bệnh nhân vẫn được bảo vệ nguyên vẹn về cả tính bí mật, toàn vẹn và sẵn sàng. Hệ thống đáp ứng trọn vẹn các yêu cầu khắt khe của quy trình nghiệp vụ y tế hiện đại.
