# BÁO CÁO MÔN HỌC: AN TOÀN VÀ BẢO MẬT HỆ THỐNG THÔNG TIN
**ĐỀ TÀI: QUẢN LÝ DỮ LIỆU Y TẾ**

---

## PHẦN I. GIỚI THIỆU HỆ THỐNG VÀ TÓM TẮT DỰ ÁN

### 1. Giới thiệu chung về hệ thống quản lý dữ liệu y tế
Trong thời đại số hóa hiện nay, việc quản lý dữ liệu y tế tại các cơ sở khám chữa bệnh đóng vai trò vô cùng quan trọng. Một hệ thống quản lý dữ liệu y tế hiệu quả không chỉ giúp tối ưu hóa quy trình làm việc của y bác sĩ, nhân viên y tế mà còn đảm bảo chất lượng phục vụ bệnh nhân. Tuy nhiên, đi kèm với sự tiện lợi đó là những thách thức to lớn về an toàn và bảo mật thông tin. Dữ liệu y tế (bao gồm thông tin cá nhân, hồ sơ bệnh án, lịch sử điều trị) được phân loại là dữ liệu nhạy cảm cấp độ cao. Việc rò rỉ hoặc bị truy cập trái phép có thể gây ra những hậu quả nghiêm trọng về quyền riêng tư và pháp lý.

Dự án "Hệ thống Quản lý Dữ liệu Y tế" được xây dựng nhằm giải quyết bài toán cốt lõi: Làm thế nào để lưu trữ, quản lý thông tin khám chữa bệnh một cách có hệ thống, đồng thời áp dụng các cơ chế bảo mật cơ sở dữ liệu chuyên sâu để kiểm soát truy cập chặt chẽ theo từng đối tượng người dùng.

### 2. Tóm tắt các chức năng và kiến trúc bảo mật
Hệ thống được thiết kế với kiến trúc bảo mật đa lớp trên nền tảng Oracle Database. Không chỉ dừng lại ở việc thiết kế cơ sở dữ liệu chuẩn hóa, dự án tập trung ứng dụng các kỹ thuật bảo mật tiên tiến nhất được cung cấp bởi Oracle:
- **Kiểm soát truy cập dựa trên vai trò (RBAC - Role-Based Access Control):** Phân chia người dùng thành các nhóm chức danh cụ thể (Bác sĩ, Kỹ thuật viên, Điều phối viên, Bệnh nhân) và cấp quyền truy cập tối thiểu cần thiết cho từng nhóm.
- **Cơ sở dữ liệu riêng tư ảo (VPD - Virtual Private Database):** Triển khai bảo mật ở mức độ dòng (row-level security), đảm bảo người dùng chỉ nhìn thấy và thao tác trên những dữ liệu thuộc thẩm quyền của mình.
- **Oracle Label Security (OLS):** Phân loại dữ liệu theo các nhãn bảo mật, kiểm soát truy cập dựa trên cấp độ nhạy cảm của thông tin, ngăn chặn việc truy cập trái phép vào các hồ sơ tuyệt mật.
- **Kiểm toán (Auditing):** Theo dõi và ghi vết toàn bộ các hành vi truy cập, thay đổi dữ liệu nhạy cảm, phục vụ cho quá trình điều tra và giám sát tuân thủ.
- **Sao lưu và Phục hồi (Backup & Recovery):** Đảm bảo tính sẵn sàng và toàn vẹn của dữ liệu trước các sự cố hệ thống.

Mục tiêu cuối cùng là xây dựng một hệ thống phần mềm C# WinForms giao tiếp trực tiếp với Oracle Database, phản ánh chân thực cách các cơ chế bảo mật hoạt động trong môi trường thực tế.

---

## PHẦN II. THIẾT KẾ CƠ SỞ DỮ LIỆU

Để đảm bảo khả năng quản lý và bảo mật, cơ sở dữ liệu của hệ thống (lược đồ `hospital`) được thiết kế với sự liên kết chặt chẽ giữa các thực thể.

### 1. Sơ lược về chức năng các bảng
Hệ thống bao gồm 10 bảng dữ liệu (chính và phụ trợ) được thiết kế để phản ánh luồng thông tin trong một cơ sở y tế và lưu trữ các thiết lập bảo mật:

1. **Bảng `department` (Phòng ban/Khoa):** 
   - Chức năng: Lưu trữ thông tin danh mục các khoa/phòng ban. 
   
2. **Bảng `staff` (Nhân viên):** 
   - Chức năng: Quản lý thông tin chi tiết của toàn bộ đội ngũ nhân sự (Bác sĩ, Kỹ thuật viên, Lễ tân).
   - Đặc điểm: Chứa trường `username_db` liên kết trực tiếp tài khoản nhân viên với user trong Oracle để kiểm soát truy cập VPD.

3. **Bảng `patient` (Bệnh nhân):**
   - Chức năng: Lưu trữ hồ sơ hành chính và tiền sử bệnh lý (`NCLOB`) của bệnh nhân. Liên kết với `username_db` để bệnh nhân tự tra cứu.

4. **Bảng `medical_record` (Hồ sơ bệnh án):**
   - Chức năng: Ghi nhận thông tin khám chữa bệnh. Là thực thể trung tâm kết nối Bác sĩ và Bệnh nhân.

5. **Bảng `service_record` (Hồ sơ dịch vụ / Cận lâm sàng):**
   - Chức năng: Lưu trữ các kết quả dịch vụ y tế (xét nghiệm, siêu âm) do kỹ thuật viên thực hiện.

6. **Bảng `prescription` (Đơn thuốc):**
   - Chức năng: Chi tiết đơn thuốc do bác sĩ kê toa tương ứng với một hồ sơ bệnh án.

7. **Bảng `notification` (Thông báo OLS):**
   - Chức năng: Lưu trữ các thông báo nội bộ của bệnh viện.
   - Đặc điểm: Bảng này được gắn nhãn bảo mật (cột ẩn `ols_label`) thông qua Oracle Label Security để lọc nội dung hiển thị tùy theo cấp bậc và phòng ban của nhân viên.

8. **Bảng `coord_assignment_staff` (Phân công nhân sự):**
   - Chức năng: Bảng phụ trợ do Điều phối viên (Coordinator) quản lý để phân luồng hiển thị danh sách Bác sĩ và Kỹ thuật viên.

9. **Bảng `backup_history` (Lịch sử Sao lưu):**
   - Chức năng: Ghi nhận lịch sử các lần Data Pump Export (Backup) thành công/thất bại, thời gian và vị trí file dump.

10. **Bảng `recovery_history` (Lịch sử Phục hồi):**
   - Chức năng: Ghi vết các hành động sử dụng Flashback Query để phục hồi dữ liệu từ Audit Log. Đảm bảo mọi tác động khôi phục dữ liệu quá khứ đều minh bạch.

### 2. Ràng buộc dữ liệu (Trigger, Index, Constraint)
Việc duy trì tính toàn vẹn dữ liệu được thực hiện thông qua các ràng buộc (Constraints) ở mức CSDL.

- **Khóa chính (Primary Key) & Khóa ngoại (Foreign Key):** Đảm bảo dữ liệu không bị mồ côi. Ví dụ, `patient_id` trong `medical_record` bắt buộc phải tồn tại trong bảng `patient`. Mọi khóa ngoại thường đi kèm với tùy chọn `ON DELETE CASCADE` hoặc giới hạn xóa để tránh mất tính nhất quán.
- **Ràng buộc UNIQUE:** Đảm bảo `username_db` và `id_card` của cả bảng `staff` và `patient` là duy nhất trên toàn hệ thống. Không thể có hai người dùng chung một tài khoản DB.
- **Index:** Các chỉ mục được tạo trên các cột thường xuyên được truy vấn để tối ưu hiệu suất đọc, đặc biệt là các cột tham gia vào chính sách bảo mật (VPD) như `username_db`, `doctor_id`, `dept_id`. Quá trình kiểm tra quyền tốn thêm tài nguyên, nên việc có Index giảm thiểu độ trễ đáng kể.

*(Lý thuyết áp dụng:* Việc thiết kế các Constraints và Indexes hợp lý không chỉ cải thiện hiệu suất mà còn là lớp bảo vệ đầu tiên chống lại các lỗi logic từ ứng dụng hoặc từ phía người thao tác cơ sở dữ liệu.)

### 3. Sinh dữ liệu (Data Generation)
Quá trình sinh dữ liệu tuân thủ tính đa dạng để phản ánh đúng thực tế khi kiểm thử các chính sách bảo mật. Dữ liệu mẫu (Seed Data) được sinh ra thông qua các Procedure tự động hoặc kịch bản SQL, tạo ra hàng trăm bệnh nhân, phân chia đều cho các bác sĩ ở nhiều khoa khác nhau. Điều này cho phép chúng ta kiểm thử các chính sách VPD (một bác sĩ chỉ thấy bệnh nhân của mình) hay OLS (phân cấp truy cập hồ sơ) một cách khách quan nhất.

---

## PHẦN III. QUẢN LÝ NGƯỜI DÙNG VÀ VAI TRÒ

### Phân tích yêu cầu trên đề bài
Theo yêu cầu thực tiễn của một bệnh viện đa khoa, không phải ai cũng có quyền xem và chỉnh sửa mọi thông tin. Do đó, việc xác định rõ vai trò và yêu cầu của từng người dùng là bước nền tảng để triển khai Security.

Hệ thống nhận diện 4 nhóm vai trò chính:
1. **Quản trị viên hệ thống (DBA / Hospital Admin):** Có toàn quyền truy cập để quản lý CSDL, thực hiện cấu hình các lớp bảo mật, theo dõi Audit, thiết lập lịch sao lưu phục hồi.
2. **Bác sĩ (Doctor):** Nhiệm vụ khám và điều trị. Yêu cầu chỉ được xem danh sách bệnh nhân đang phụ trách, xem chi tiết và tạo mới hồ sơ khám, kê đơn thuốc. Tuyệt đối không được phép chỉnh sửa kết quả xét nghiệm của kỹ thuật viên hay xem bệnh án của bác sĩ khác khi không có thẩm quyền.
3. **Kỹ thuật viên (Technician):** Thực hiện các chỉ định dịch vụ. Chỉ được phép cập nhật kết quả vào `service_record`. Không có quyền kê đơn thuốc hay sửa chẩn đoán y khoa.
4. **Điều phối viên (Coordinator/TiepTan):** Phụ trách tiếp đón, quản lý lịch trình. Được phép xem danh sách và cập nhật thông tin hành chính của bác sĩ/bệnh nhân, tạo hồ sơ bệnh án sơ bộ, nhưng không được can thiệp vào chuyên môn y tế.
5. **Bệnh nhân (Patient):** Yêu cầu được cấp quyền tự tra cứu hồ sơ cá nhân của chính họ một cách an toàn. Tuyệt đối không được xem dữ liệu của bệnh nhân khác.

Việc phân tích các yêu cầu này giúp định hình rõ ràng ma trận quyền, từ đó áp dụng lý thuyết cấp quyền tĩnh (Grant/Revoke) cũng như các chính sách linh hoạt (VPD, OLS) để giải quyết vấn đề.

---

## PHẦN IV. PHÂN QUYỀN (CẤP / THU HỒI QUYỀN TRUY CẬP)

### Phân tích yêu cầu trên đề
Trong Oracle Database, quyền được chia thành hai loại: System Privileges (Quyền hệ thống) và Object Privileges (Quyền trên đối tượng). Cấp và thu hồi quyền là thao tác bảo mật cơ sở (Discretionary Access Control - DAC).

1. **Quá trình tạo tài khoản:**
   Mỗi nhân viên hay bệnh nhân khi thêm vào hệ thống không chỉ là một record trong bảng `staff`/`patient` mà còn được cấp một user name trong CSDL (ví dụ: `DOC_NGUYENVANA`, `PAT_TRANTHIB`). User này được cấp quyền `CREATE SESSION` để đăng nhập.

2. **Cấp quyền (GRANT):**
   - Hệ thống không cấp quyền trực tiếp cho từng cá nhân (vì rất khó quản lý khi số lượng người lên đến hàng nghìn).
   - Hệ thống sử dụng Role (vai trò). Quyền Object (như `SELECT`, `INSERT`, `UPDATE` trên từng bảng) sẽ được `GRANT` cho Role.
   - Trong quá trình phân tích, chúng ta nhận thấy Điều phối viên cần `SELECT, INSERT, UPDATE` trên bảng `patient` và `medical_record`. Tuy nhiên, lệnh `GRANT UPDATE ON patient TO Role_Coordinator` sẽ cho phép họ sửa toàn bộ bảng. Đây là lúc DAC bộc lộ giới hạn của nó, buộc chúng ta phải sử dụng VPD và OLS ở các phần sau.

3. **Thu hồi quyền (REVOKE):**
   Khi một nhân viên chuyển công tác, chuyển phòng ban, hoặc nghỉ việc, quyền của họ sẽ bị thu hồi. Thao tác này đơn giản là `REVOKE <Role> FROM <User>` hoặc vô hiệu hóa tài khoản CSDL. Điều này giúp ngăn chặn hoàn toàn việc người cũ vẫn duy trì quyền truy cập vào dữ liệu hiện hành.

Tóm lại, thao tác `GRANT`/`REVOKE` giải quyết bài toán: **"Ai được làm thao tác gì trên bảng nào?"**. Tuy nhiên nó chưa giải quyết được bài toán **"Ai được làm thao tác gì trên DÒNG DỮ LIỆU nào?"**. Phần tiếp theo (RBAC và VPD) sẽ làm rõ giải pháp cho giới hạn này.
