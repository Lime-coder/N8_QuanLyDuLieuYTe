## PHẦN V. KIỂM SOÁT TRUY CẬP THEO VAI TRÒ (RBAC - ROLE-BASED ACCESS CONTROL)

### 1. Lý thuyết chuyên sâu về RBAC
Kiểm soát truy cập dựa trên vai trò (Role-Based Access Control - RBAC) là một trong những phương pháp kiểm soát truy cập phổ biến và hiệu quả nhất trong các hệ thống thông tin quy mô lớn. Khác với DAC (Discretionary Access Control) phân quyền trực tiếp cho từng cá nhân, RBAC đưa ra khái niệm "Vai trò" (Role) làm trung gian giữa Người dùng (User) và Quyền (Privilege).

**Các khái niệm cốt lõi của RBAC được nhóm nghiên cứu tìm hiểu bao gồm:**
- **Core RBAC:** Yêu cầu cơ bản nhất, trong đó hệ thống phải có khả năng tạo Role, gán User vào Role, và gán Quyền cho Role. Một User có thể đảm nhận nhiều Role, và một Role có thể chứa nhiều User.
- **Hierarchical RBAC:** Cho phép thiết lập sự kế thừa giữa các Role. Ví dụ, Role `TruongKhoa` có thể kế thừa toàn bộ quyền của Role `BacSi`. Trong đồ án này, chúng ta không tập trung sâu vào kế thừa mà tập trung vào phân tách quyền hạn ngang hàng.
- **Constrained RBAC (RBAC ràng buộc):** Thiết lập các nguyên tắc về phân chia trách nhiệm (Segregation of Duties - SoD). SoD tĩnh ngăn chặn việc một người được gán hai Role xung đột nhau (VD: Người kê đơn và Người xuất thuốc không thể là một).

**Lợi ích vượt trội của RBAC khi áp dụng vào Y tế:**
1. **Giảm thiểu sai sót quản trị:** Khi một bác sĩ mới vào làm, quản trị viên chỉ cần gán họ vào Role `RL_DOCTOR` thay vì phải chạy hàng chục lệnh `GRANT` từng bảng.
2. **Quản lý thay đổi dễ dàng:** Khi cần bổ sung quyền xem bảng `department` cho tất cả kỹ thuật viên, DBA chỉ cần `GRANT SELECT ON department TO RL_TECHNICIAN`. Hàng trăm kỹ thuật viên sẽ lập tức có quyền mà không cần thao tác từng người.
3. **Tuân thủ chính sách (Compliance):** RBAC giúp dễ dàng chứng minh với các cơ quan kiểm toán rằng quyền hạn được cấp đúng theo chức danh, đáp ứng tiêu chuẩn HIPAA.

### 2. Phân tích và giải thích cơ chế áp dụng
Trong Oracle, RBAC được triển khai cực kỳ tự nhiên thông qua object `ROLE`. Hệ thống Quản lý Y Tế của chúng ta thiết lập các Role sau: `RL_DOCTOR`, `RL_TECHNICIAN`, `RL_COORDINATOR`, `RL_PATIENT`.

**Quy trình áp dụng cơ chế RBAC:**
- Bước 1: DBA tạo Role (`CREATE ROLE RL_TECHNICIAN;`).
- Bước 2: Cấp các đặc quyền Object cho Role (`GRANT SELECT, UPDATE ON service_record TO RL_TECHNICIAN;`).
- Bước 3: Gán Role cho tài khoản người dùng (`GRANT RL_TECHNICIAN TO TECH_NGUYENVANA;`).

*Giải thích cho từng nhóm:*
- **Bệnh nhân (Patient):** Mọi bệnh nhân đều được gán `RL_PATIENT`. Vai trò này cho phép họ có quyền `SELECT` và `UPDATE` thông tin cá nhân.
- **Kỹ thuật viên (Technician):** Được gán `RL_TECHNICIAN`. Họ có quyền `SELECT` danh sách dịch vụ và `UPDATE` kết quả vào `service_record`.
- **Bác sĩ và Điều phối viên:** Mặc dù RBAC cung cấp quyền, nhưng vì yêu cầu của hai nhóm này quá phức tạp (chỉ được xem dòng dữ liệu của mình), nên RBAC chỉ đóng vai trò cấp quyền thao tác trên *bảng*, còn việc giới hạn trên *dòng* được giao cho VPD.

### 3. Kết quả đạt được và So sánh
**Kết quả:** Hệ thống đã chặn thành công các lỗi truy cập ngang. Kỹ thuật viên không thể chạy lệnh `DELETE FROM medical_record` do `RL_TECHNICIAN` không có quyền này. Bệnh nhân không thể truy cập bảng `staff`. 

**So sánh RBAC với các cơ chế khác:**
- *Ưu điểm:* Cực kỳ dễ hiểu, dễ quản lý cho số lượng người dùng lớn, cấu hình nhanh chóng. Khắc phục hoàn toàn sự rời rạc của DAC.
- *Nhược điểm so với VPD/OLS:* RBAC chỉ giới hạn quyền ở mức "Bảng" (Table-level). Nếu một Bác sĩ có quyền `SELECT ON patient`, họ sẽ nhìn thấy TOÀN BỘ bệnh nhân trong bệnh viện. RBAC không thể nói: "Bác sĩ A chỉ được xem bệnh nhân của Bác sĩ A". Đây chính là lý do RBAC không thể đứng một mình mà phải kết hợp với VPD và OLS để tạo thành hệ thống bảo mật hoàn hảo.

---

## PHẦN VI. BẢO MẬT DỮ LIỆU RIÊNG TƯ ẢO (VPD - VIRTUAL PRIVATE DATABASE)

### 1. Lý thuyết chuyên sâu về VPD
Virtual Private Database (VPD), còn được gọi là Row-Level Security (RLS) hay Fine-Grained Access Control (FGAC), là công nghệ bảo mật độc quyền mạnh mẽ nhất của Oracle. VPD giải quyết triệt để điểm mù của RBAC bằng cách cho phép quản trị viên đính kèm các chính sách bảo mật (Security Policies) trực tiếp vào các đối tượng CSDL (bảng, view).

**Cơ chế hoạt động sâu của VPD:**
Khi một người dùng gửi câu lệnh truy vấn (VD: `SELECT * FROM patient`), Oracle Database Server sẽ đánh chặn câu lệnh này TRƯỚC khi nó thực thi. Engine VPD sẽ gọi một hàm PL/SQL (được gọi là Policy Function) do DBA viết sẵn. Hàm này sẽ trả về một chuỗi điều kiện (Predicate - mệnh đề `WHERE` động). Oracle sẽ lấy predicate này nối thêm vào câu lệnh ban đầu.

*Ví dụ minh họa luồng xử lý:*
- Người dùng gõ: `SELECT * FROM medical_record`
- Policy Function trả về: `"doctor_id = 'DOC_01'"` (vì user hiện tại đang đăng nhập là DOC_01).
- Lệnh thực tế Oracle chạy trong nền: `SELECT * FROM medical_record WHERE doctor_id = 'DOC_01'`

Nhờ cơ chế đánh chặn ở tầng lõi CSDL, VPD bảo vệ dữ liệu một cách trong suốt (Transparent). Ứng dụng C# WinForms không cần phải sửa bất kỳ dòng code SQL nào, không cần tự nối thêm mệnh đề WHERE; CSDL sẽ tự lo liệu. Kẻ tấn công có bypass được ứng dụng để chọc thẳng vào CSDL qua SQL*Plus thì cũng không thể vượt qua VPD.

### 2. Phân tích và giải thích cơ chế triển khai VPD
Trong dự án, VPD được sử dụng chuyên sâu cho 2 vai trò: **Điều phối viên (Coordinator)** và **Bác sĩ (Doctor)**.

**A. VPD cho Điều Phối Viên (Coordinator):**
- *Yêu cầu:* Điều phối viên ở phòng ban nào thì chỉ được xem, sửa thông tin của bác sĩ và bệnh nhân có liên quan đến phòng ban đó. 
- *Cơ chế:* Policy Function sẽ truy xuất phòng ban hiện tại của Điều phối viên thông qua Application Context (hoặc query từ bảng staff dựa vào `SYS_CONTEXT('USERENV', 'SESSION_USER')`). Sau đó, nó trả về predicate: `dept_id = '<mã_khoa_của_coordinator>'`. Bất kỳ câu `SELECT`, `UPDATE` nào lên bảng `medical_record` hay `staff` cũng đều bị nối thêm điều kiện này.

**B. VPD cho Bác sĩ (Doctor):**
- *Yêu cầu:* Bác sĩ chỉ được quyền xem và cập nhật hồ sơ bệnh án của những bệnh nhân mà chính bác sĩ đó phụ trách khám. 
- *Cơ chế:* 
  - Tạo một Package/Function `policy_doctor_medical_record`.
  - Trong Function, lấy username đang đăng nhập: `v_user := SYS_CONTEXT('USERENV', 'SESSION_USER')`.
  - Từ `v_user`, tra cứu `staff_id` tương ứng trong bảng `staff`.
  - Trả về chuỗi điều kiện: `'doctor_id = ''' || v_staff_id || ''''`.
  - Cuối cùng, gán policy này vào bảng `medical_record` thông qua gói `DBMS_RLS.ADD_POLICY`.

### 3. Kết quả đạt được và So sánh
**Kết quả:** Khi Bác sĩ A đăng nhập và mở màn hình Danh sách bệnh án, dù ứng dụng gọi lệnh SELECT ALL, họ chỉ thấy đúng 10 bệnh án do mình phụ trách. Việc cố tình truyền một tham số giả qua giao diện để ép hệ thống hiển thị bệnh án của Bác sĩ B cũng sẽ bị CSDL chặn đứng lại trả về tập kết quả rỗng.

**So sánh sự vượt trội của VPD:**
- *So với RBAC:* VPD kiểm soát mịn đến từng dòng (Row-level) thay vì toàn bộ bảng. Đây là sự khác biệt mang tính cách mạng trong thiết kế CSDL đa người dùng.
- *So với OLS:* VPD rất linh hoạt vì predicate trả về có thể là bất kỳ logic lập trình PL/SQL phức tạp nào (có thể join nhiều bảng để quyết định quyền). OLS thì cứng nhắc hơn vì dựa trên nhãn tĩnh. VPD phù hợp với kiểm soát logic nghiệp vụ (business logic), còn OLS phù hợp với kiểm soát cấp độ mật quốc gia/tổ chức.

---

## PHẦN VII. ORACLE LABEL SECURITY (OLS)

### 1. Lý thuyết chuyên sâu về OLS
Oracle Label Security (OLS) là sự hiện thực hóa của mô hình bảo mật bắt buộc (Mandatory Access Control - MAC) theo tiêu chuẩn Bell-LaPadula của quân đội Hoa Kỳ, áp dụng vào cơ sở dữ liệu thương mại. Trong khi DAC và RBAC cho phép người tạo dữ liệu có quyền quyết định ai được xem dữ liệu của mình, thì MAC tước quyền đó đi. Quyền truy cập được quyết định hoàn toàn bởi hệ thống dựa trên các "Nhãn" (Label) đã được đóng dấu lên dữ liệu và người dùng.

**Cấu trúc của một Nhãn OLS (Label Component):**
Một nhãn OLS bao gồm 3 thành phần (Level:Compartment:Group):
1. **Level (Cấp độ):** Có tính thứ bậc. (VD: PUBLIC < CONFIDENTIAL < SECRET < TOP_SECRET). Người dùng phải có level cao hơn hoặc bằng level của dữ liệu mới được đọc (Read Down). Để ghi dữ liệu, họ phải có level bằng với dữ liệu (Write Equal).
2. **Compartment (Ngăn cách/Phân khu):** Không có tính thứ bậc. Phân chia dữ liệu theo các lĩnh vực độc lập. (VD: KhoaNgoai, KhoaNoi, KeToan). Nếu dữ liệu được dán nhãn KhoaNgoai, người dùng phải có nhãn KhoaNgoai trong hồ sơ của mình mới đọc được.
3. **Group (Nhóm):** Có tính thứ bậc theo hình cây (Tree). 

*Nguyên tắc Read/Write cốt lõi:*
- Người dùng chỉ được **ĐỌC** dữ liệu nếu: Level của họ >= Level dữ liệu; và họ sở hữu tất cả các Compartments của dữ liệu; và họ thuộc về Group của dữ liệu hoặc Group cha.
- Hành động **GHI/SỬA** bị kiểm soát ngặt nghèo hơn rất nhiều để ngăn chặn rò rỉ dữ liệu xuống mức thấp hơn.

### 2. Phân tích và giải thích cơ chế triển khai OLS
Trong dự án Y tế, OLS được áp dụng cho một bảng rất nhạy cảm mà RBAC và VPD không đủ sức bao quát: **Bảng THÔNG BÁO (Notifications) hoặc các HỒ SƠ TUYỆT MẬT.**

- *Yêu cầu:* Giám đốc bệnh viện muốn phát đi thông báo mật chỉ dành riêng cho Trưởng khoa Nội. Kể cả DBA hay các bác sĩ khác trong khoa cũng không được phép đọc.
- *Triển khai:*
  - Bật tính năng OLS trên server (`chopt enable lbac`).
  - Tạo một Policy OLS tên là `HOSPITAL_OLS_POL`.
  - Định nghĩa các Levels: `THONG_THUONG` (10), `NHO_GIOI_HAN` (20), `NHAY_CAM` (30), `TUYET_MAT` (40).
  - Định nghĩa các Compartments dựa trên chuyên khoa: `NOI_KHOA`, `NGOAI_KHOA`.
  - Áp dụng Policy `HOSPITAL_OLS_POL` lên bảng dữ liệu cần bảo vệ. Hệ thống sẽ tự động sinh ra một cột ẩn (hidden column) có kiểu số chứa định danh của nhãn.
  - Phân gán nhãn cho người dùng: Giám đốc cấp nhãn `TUYET_MAT:NOI_KHOA` cho Trưởng khoa nội. Khi trưởng khoa nội truy vấn, Oracle tự động so sánh nhãn của người dùng với nhãn từng dòng dữ liệu và chỉ trả về những dòng thỏa mãn thuật toán MAC.

### 3. Kết quả đạt được và So sánh
**Kết quả:** Nhãn bảo mật (ví dụ định dạng `40:100:`) được hệ thống xử lý hoàn hảo. Những bác sĩ có nhãn `THONG_THUONG` khi truy vấn bảng thông báo sẽ không bao giờ nhìn thấy dòng dữ liệu có nhãn `TUYET_MAT`, dù họ có dùng tài khoản DBA can thiệp lệnh SQL đi chăng nữa. Đây là một lớp khiên thép vững chắc.

**So sánh điểm vượt trội của OLS:**
- OLS vượt trội hoàn toàn so với VPD và RBAC trong việc **bảo vệ chống rò rỉ ngang và rò rỉ dọc**.
- OLS tuân thủ chặt chẽ nguyên tắc **Mandatory**. Quản trị viên (DBA thông thường) không có quyền thay đổi OLS policy, chỉ có người được giao role `LBAC_DBA` mới làm được. Điều này tạo ra khái niệm "Phân quyền quản trị" (Separation of Duty ở mức quản trị), ngăn chặn việc DBA lạm quyền xem hồ sơ bệnh án nhạy cảm của các VIP.

---

## PHẦN VIII. KIỂM TOÁN (AUDITING)

### 1. Lý thuyết chuyên sâu về Auditing trong Oracle
Kiểm toán (Auditing) là quá trình giám sát, ghi lại và đánh giá các hành vi tương tác với hệ thống CSDL. Nếu RBAC, VPD, OLS là các biện pháp "Phòng ngừa" (Preventive), thì Auditing là biện pháp "Phát hiện" (Detective). Không có hệ thống nào an toàn 100%, do đó việc ghi vết mọi thao tác là bắt buộc theo các bộ luật Y tế như HIPAA.

**Các loại Kiểm toán nhóm đã nghiên cứu:**
1. **Standard Auditing (Kiểm toán tiêu chuẩn):** Bắt đầu từ các bản Oracle cũ, ghi lại các sự kiện ở mức câu lệnh (Ai gọi lệnh UPDATE bảng patient) nhưng KHÔNG ghi lại được dữ liệu cũ/mới, và KHÔNG biết họ UPDATE dòng nào.
2. **Fine-Grained Auditing - FGA (Kiểm toán chi tiết/mịn):** Cung cấp khả năng ghi vết ở mức dòng và cột. FGA có thể được cấu hình để chỉ trigger ghi log khi người dùng truy vấn (SELECT) vào một bệnh nhân nổi tiếng (ví dụ có `patient_id = 'VIP_01'`), hoặc khi họ sửa cột `diagnosis` (chẩn đoán).
3. **Unified Auditing (Kiểm toán hợp nhất):** Kiến trúc mới của Oracle từ 12c, gom tất cả log từ Standard, FGA, RMAN, DataPump vào một bảng duy nhất (`UNIFIED_AUDIT_TRAIL`), giúp việc query và quản trị dễ dàng hơn với hiệu năng cao hơn rất nhiều (tối ưu IO).

### 2. Phân tích và giải thích cơ chế
Dự án áp dụng FGA kết hợp Unified Auditing để theo dõi các hành vi thay đổi hồ sơ bệnh án trái phép.
- *Ví dụ kịch bản:* Bệnh viện nghi ngờ có bác sĩ lợi dụng quyền hạn để tra cứu hồ sơ bệnh án của các chính trị gia đang điều trị tại khoa, dù họ không được phân công phụ trách.
- *Thiết lập:* Sử dụng gói `DBMS_FGA.ADD_POLICY`. 
  - Gắn policy lên bảng `medical_record`.
  - Statement type: `SELECT, UPDATE`.
  - Audit Condition: Có thể để NULL (ghi mọi thao tác) hoặc `dept_id = 'KHOA_VIP'`.
  - Audit Column: `diagnosis`, `treatment_plan`.

Khi một bác sĩ thực hiện lệnh UPDATE, FGA policy sẽ bắt sự kiện này trong nền, ghi lại `DB_USER`, `OS_USER`, `TIMESTAMP`, câu `SQL_TEXT` chính xác đã được chạy, và cả giá trị `SCN` (System Change Number).

### 3. Kết quả đạt được và So sánh
**Kết quả:** Mỗi lần có sự cố nghi ngờ chỉnh sửa kết quả khám bệnh, Quản trị viên chỉ cần query view `UNIFIED_AUDIT_TRAIL` (hoặc `DBA_FGA_AUDIT_TRAIL`) là có thể chỉ đích danh nhân viên, thời gian, IP máy tính thực hiện thao tác. Log audit này được bảo vệ ở mức kernel CSDL, DBA thông thường cũng không thể tự ý xóa lệnh mình đã làm.

**So sánh giữa các cơ chế Audit:**
- *Standard vs FGA:* Standard audit tạo ra lượng rác log rất lớn vì bắt mọi thứ. FGA thông minh hơn nhờ `Audit Condition` và `Audit Column`, giúp giảm thiểu dung lượng lưu trữ log và chỉ bắt những sự kiện thực sự cần thiết, đồng thời FGA bắt được chính xác cú pháp SQL đã chạy.
- *Unified Audit vs Traditional:* Tốc độ ghi log của Unified Audit vượt trội hơn nhờ cơ chế queue in-memory, không làm chậm quá trình thao tác DML/Select của người dùng y tế.
