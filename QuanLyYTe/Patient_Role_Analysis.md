# 5.3. Vai trò bệnh nhân

## 5.3.1. Phân tích yêu cầu bảo mật
Theo yêu cầu hệ thống, bệnh nhân được quyền xem thông tin hồ sơ bệnh án của chính mình (bao gồm hồ sơ khám bệnh, đơn thuốc, dịch vụ khám chữa bệnh). Ngoài ra, bệnh nhân cũng có quyền xem và chỉnh sửa thông tin cá nhân của mình, đặc biệt là các trường liên quan đến địa chỉ liên lạc và tiền sử bệnh tật.
Từ yêu cầu đó, nhóm xác định rằng nếu cấp quyền trực tiếp trên các bảng gốc (như PATIENT, MEDICAL_RECORD, PRESCRIPTION, SERVICE_RECORD) cho bệnh nhân thì bệnh nhân có nguy cơ xem được hồ sơ và thông tin cá nhân của những bệnh nhân khác. Do đó, nhóm không cho giao diện thao tác trực tiếp trên các bảng gốc. Thay vào đó, nhóm xây dựng các view để lọc dữ liệu chỉ hiển thị những thông tin liên quan đến user (bệnh nhân) đang đăng nhập, và tạo các stored procedure để giao diện ứng dụng gọi khi cần truy xuất hoặc cập nhật.
Cơ chế được triển khai bao gồm ba thành phần chính: thành phần thứ nhất là các view dùng để giới hạn dòng dữ liệu (chỉ lấy dữ liệu của chính bệnh nhân đó), thành phần thứ hai là các stored procedure để kiểm soát việc đọc/ghi từ giao diện, và thành phần thứ ba là các câu lệnh cấp quyền (grant) trên role Bệnh nhân (`rl_patient`). Ba thành phần này phối hợp chặt chẽ để đảm bảo bệnh nhân chỉ thao tác được trong giới hạn dữ liệu của chính mình.

## 5.3.2. Cài đặt cơ chế

### Tạo view giới hạn dữ liệu
Nhóm tạo các view `V_PATIENT_SELF`, `V_MEDICAL_RECORD_PATIENT`, `V_PRESCRIPTION_PATIENT`, `V_SERVICE_RECORD_PATIENT` để hiển thị thông tin cá nhân, hồ sơ bệnh án, đơn thuốc, và dịch vụ khám chữa bệnh tương ứng của bệnh nhân hiện tại. Các view này liên kết (join) với bảng gốc `PATIENT` (để xác định thông tin đăng nhập) và sử dụng điều kiện lọc quan trọng:
```sql
WHERE p.username_db = SYS_CONTEXT('USERENV', 'SESSION_USER')
```
Hàm `SYS_CONTEXT('USERENV', 'SESSION_USER')` trả về username Oracle của phiên đăng nhập hiện tại. Bằng cách so khớp giá trị này với cột `username_db` trong bảng `PATIENT`, hệ thống xác định được bệnh nhân tương ứng với tài khoản đăng nhập và chỉ trả về các dữ liệu (hồ sơ, dịch vụ, đơn thuốc...) thuộc về đúng bệnh nhân đó. View đóng vai trò như một lớp màng lọc bảo mật, che giấu bảng gốc và giới hạn dữ liệu theo từng user cụ thể.

### Tạo stored procedure
Sau khi tạo view, các procedure được xây dựng để cung cấp dữ liệu cho giao diện: `USP_GET_PATIENT_PROFILE`, `USP_GET_PATIENT_RECORDS`, `USP_GET_PATIENT_PRESCRIPTIONS`, `USP_GET_PATIENT_SERVICES`. Các procedure này truy vấn qua các view đã tạo ở trên và trả về dữ liệu bằng `SYS_REFCURSOR`, giúp giao diện dễ dàng hiển thị lên bảng. Nhờ lấy từ view, kết quả trả về đã được lọc theo user đang đăng nhập.
Để cho phép cập nhật, nhóm viết procedure `USP_UPDATE_PATIENT_CONTACT`. Procedure này chỉ cho phép cập nhật một số thông tin trên view `V_PATIENT_SELF` bao gồm địa chỉ (`house_no`, `street`, `district`, `city_province`) và tiền sử bệnh (`medical_history`, `family_medical_history`, `drug_allergies`). Các thông tin định danh như mã bệnh nhân, họ tên, CCCD, ngày sinh không được phép thay đổi nhằm tránh người dùng tự ý can thiệp vào định danh cá nhân.

### Cấp quyền cho role
Cuối cùng, role Bệnh nhân (`rl_patient`) được cấp quyền `SELECT` trên tất cả các view `V_PATIENT_SELF`, `V_MEDICAL_RECORD_PATIENT`, `V_PRESCRIPTION_PATIENT`, `V_SERVICE_RECORD_PATIENT`. Đối với view `V_PATIENT_SELF`, role này được cấp thêm quyền `UPDATE` nhưng chỉ giới hạn trên các cột địa chỉ và tiền sử bệnh như đã nêu. Ngoài ra, quyền `EXECUTE` được cấp trên tất cả các procedure phục vụ chức năng của bệnh nhân để giao diện có thể gọi được.

### Cơ chế hoạt động
Cơ chế hoạt động tổng thể là: user bệnh nhân đăng nhập, user này có role bệnh nhân, giao diện gọi procedure, procedure truy xuất hoặc cập nhật dữ liệu qua view, view lọc dữ liệu theo `SESSION_USER`. Từ đó bệnh nhân chỉ nhìn thấy và cập nhật được các dữ liệu thuộc phạm vi sở hữu của chính mình.

## 5.3.3. Kết quả đạt được
Sau khi cài đặt cơ chế RBAC cho vai trò Bệnh nhân, hệ thống đã đảm bảo bệnh nhân chỉ xem được thông tin cá nhân, hồ sơ bệnh án, dịch vụ và đơn thuốc của chính bản thân mình thông qua các view. Bệnh nhân không thể truy xuất hay xem hồ sơ của bệnh nhân khác vì không được cấp quyền trực tiếp trên các bảng gốc.
Về quyền cập nhật, bệnh nhân chỉ được phép thay đổi thông tin liên lạc và tiền sử bệnh tật của mình. Các trường định danh cốt lõi như mã số, họ tên, ngày sinh, CCCD... vẫn được bảo vệ nguyên vẹn, đáp ứng đúng yêu cầu nghiệp vụ và an toàn thông tin.

## 5.3.4. Đánh giá và lựa chọn giải pháp
Đối với số lượng tài khoản Bệnh nhân rất lớn, sử dụng RBAC giúp quản lý phân quyền dễ dàng. Dù RBAC không hỗ trợ bảo mật mức dòng (row-level security), nhóm đã khắc phục bằng cách lồng ghép hàm `SYS_CONTEXT` vào các View, đảm bảo mỗi người chỉ xem được hồ sơ của mình.

So với các công nghệ bảo mật khác của Oracle, giải pháp này tối ưu và tinh gọn hơn. Việc áp dụng VPD (Virtual Private Database) là không cần thiết vì đòi hỏi phải viết các hàm chính sách phức tạp, làm hệ thống cồng kềnh. Trong khi đó, OLS (Oracle Label Security) được thiết kế cho kịch bản bảo mật dựa trên phân cấp nhãn (Tối mật, Bí mật...), không phù hợp với nhu cầu phân mảnh dữ liệu theo sở hữu cá nhân.

Tóm lại, sự kết hợp giữa RBAC, View động và Stored Procedure là phương án thực tế, tiết kiệm tài nguyên và đáp ứng trọn vẹn yêu cầu bảo mật quyền riêng tư cho bệnh nhân.
