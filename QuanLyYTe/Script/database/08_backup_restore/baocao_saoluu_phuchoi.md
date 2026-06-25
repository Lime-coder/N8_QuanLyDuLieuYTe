# BÁO CÁO BÀI TẬP LỚN: XÂY DỰNG VÀ TRIỂN KHAI CƠ CHẾ SAO LƯU VÀ PHỤC HỒI DỮ LIỆU SAU SỰ CỐ

* **Môn học:** An toàn và bảo mật hệ thống thông tin
* **Yêu cầu thực hiện:** Yêu cầu 4 - Cài đặt chức năng sao lưu (chủ động, tự động) và khôi phục dữ liệu dựa vào nhật ký kiểm toán ở Yêu cầu 3 sau khi có sự cố.
* **Sinh viên thực hiện:** [Tên Sinh Viên / Nhóm]

---

## 1. LÝ THUYẾT VỀ CƠ CHẾ SAO LƯU VÀ PHỤC HỒI DỮ LIỆU TRONG HỆ QTCSDL

Đảm bảo an toàn thông tin và tính sẵn sàng của dữ liệu là nhiệm vụ sống còn của bất kỳ Hệ quản trị cơ sở dữ liệu (HQT CSDL) nào. Qua quá trình học tập và nghiên cứu sâu các tài liệu kỹ thuật của Oracle Database, nhóm/em đã tổng hợp một cách chi tiết và hệ thống hóa lý thuyết về cơ chế sao lưu và phục hồi dữ liệu như sau:

---

### 1.1. Các cơ chế sao lưu dữ liệu (Backup Mechanisms)
Sao lưu dữ liệu là quá trình tạo ra một bản sao lưu trữ của cấu trúc và dữ liệu của cơ sở dữ liệu tại một thời điểm xác định nhằm chuẩn bị cho việc khôi phục khi hệ thống xảy ra sự cố. Dựa vào bản chất kỹ thuật, sao lưu được chia làm hai loại chính:

```
                  ┌─────────────────────────────────────────┐
                  │          CƠ CHẾ SAO LƯU (BACKUP)        │
                  └────────────────────┬────────────────────┘
                                       │
                ┌──────────────────────┴──────────────────────┐
                ▼                                             ▼
  ┌───────────────────────────┐                 ┌───────────────────────────┐
  │     SAO LƯU VẬT LÝ        │                 │       SAO LƯU LOGIC       │
  │     (Physical Backup)     │                 │      (Logical Backup)     │
  └─────────────┬─────────────┘                 └─────────────┬─────────────┘
                │                                             │
      ┌─────────┴─────────┐                         ┌─────────┴─────────┐
      ▼                   ▼                         ▼                   ▼
┌───────────┐       ┌───────────┐             ┌───────────┐       ┌───────────┐
│   COLD    │       │    HOT    │             │ Traditional│      │ DATA PUMP │
│  BACKUP   │       │  BACKUP   │             │   EXPORT  │      │  (expdp)  │
│ (Offline) │       │ (Online)  │             │   (exp)   │      │ (Server)  │
└───────────┘       └───────────┘             └───────────┘       └───────────┘
```

#### 1.1.1. Sao lưu vật lý (Physical Backup)
Sao lưu vật lý liên quan trực tiếp đến việc sao chép các tệp nhị phân lưu trữ thực tế cấu thành nên cơ sở dữ liệu trên hệ điều hành. Các tệp tin này bao gồm:
* **Datafiles:** Tệp tin lưu trữ dữ liệu người dùng và siêu dữ liệu hệ thống.
* **Control Files:** Tệp tin điều khiển chứa siêu dữ liệu về cấu trúc vật lý của database (đường dẫn datafile, thông tin log, trạng thái đồng bộ).
* **Redo Log Files / Archived Redo Logs:** Nhật ký ghi nhận tất cả thay đổi trên block dữ liệu phục vụ tái thiết giao dịch.
* **Parameter Files (SPFILE/PFILE):** Tệp cấu hình các tham số khởi động của Oracle Instance.

Sao lưu vật lý được chia thành 2 trạng thái hoạt động:
* **Sao lưu ngoại tuyến (Cold Backup / Offline Backup):** 
  * Cơ sở dữ liệu phải được tắt hoàn toàn (Shutdown) một cách nhất quán (Normal, Immediate, or Transactional).
  * Quản trị viên sử dụng lệnh của hệ điều hành để sao chép toàn bộ các tệp vật lý sang phân vùng lưu trữ khác.
  * **Ưu điểm:** Đơn giản, an toàn tuyệt đối về mặt nhất quán dữ liệu.
  * **Nhược điểm:** Hệ thống buộc phải dừng hoạt động (Downtime), không phù hợp cho các hệ thống hoạt động liên tục 24/7 như Y tế, Ngân hàng.
* **Sao lưu trực tuyến (Hot Backup / Online Backup):**
  * Cơ sở dữ liệu vẫn mở và hoạt động bình thường, phục vụ người dùng đọc/ghi dữ liệu.
  * Yêu cầu bắt buộc: Cơ sở dữ liệu phải hoạt động ở chế độ **ARCHIVELOG mode** (tự động đóng gói và lưu trữ các Redo log cũ khi chúng bị ghi đè).
  * Công cụ hiện đại chuyên dụng là **Oracle Recovery Manager (RMAN)**. RMAN giao tiếp với nhân CSDL, thực hiện copy các block dữ liệu, đồng thời kiểm tra tính toàn vẹn vật lý và logic của từng block dữ liệu trực tiếp trong quá trình backup, giải quyết triệt để lỗi "fractured blocks" (khối dữ liệu bị chia cắt do sao chép khi đang ghi).

#### 1.1.2. Sao lưu logic (Logical Backup)
Sao lưu logic không sao chép các tệp nhị phân vật lý, mà trích xuất cấu trúc định nghĩa đối tượng (DDL - Data Definition Language) và dữ liệu chứa trong các bảng (DML - Data Manipulation Language) thành một tệp tin nhị phân trung gian có định dạng nhị phân độc lập dạng tệp Dump (`.dmp`).
* **Công cụ xuất nhập dữ liệu truyền thống (`exp` / `imp`):** 
  * Thực hiện ở tầng Client. Chương trình Client gửi câu lệnh `SELECT` để đọc dữ liệu từ Server qua mạng, chuyển đổi dữ liệu thành luồng bytes và lưu xuống đĩa cứng Client. Tốc độ rất chậm và dễ bị tắc nghẽn băng thông mạng.
* **Công cụ Oracle Data Pump (`expdp` / `impdp`):**
  * Hoạt động hoàn toàn ở phía Server (Server-side technology). Mọi công việc trích xuất và ghi tệp dump được đảm nhận bởi các tiến trình nền (Background Processes) trực tiếp trên máy chủ cơ sở dữ liệu.
  * **Kiến trúc luồng xử lý của Data Pump:** 
    * Khi chạy lệnh `expdp`, một tiến trình bóng (**Shadow Process**) được tạo ra để biên dịch lệnh.
    * Shadow Process sẽ khởi tạo một Job Master Table trong cơ sở dữ liệu và gọi tiến trình kiểm soát chính là **Master Control Process (MCP)** mang tên `DM00`.
    * MCP sinh ra các tiến trình công việc **Worker Processes** (`DW01`, `DW02`, v.v.) để trực tiếp đọc dữ liệu từ datafiles, ghi trực tiếp vào tệp Dump nằm trong một Oracle Directory đã cấu hình trên đĩa của Server thông qua công nghệ song song (Parallelism) và các cơ chế Direct Path API (đọc trực tiếp block dữ liệu bỏ qua vùng đệm SGA).
    * Quản lý siêu dữ liệu thông qua gói hệ thống `DBMS_METADATA` và quản lý luồng dữ liệu thông qua gói hệ thống `DBMS_DATAPUMP`.

---

### 1.2. Các cơ chế phục hồi dữ liệu (Recovery Mechanisms)
Phục hồi dữ liệu là quá trình tái thiết lập trạng thái của cơ sở dữ liệu về một thời điểm nhất quán và đúng đắn sau khi xảy ra sự cố phần cứng, phần mềm hoặc do sai sót của con người.

#### 1.2.1. Phục hồi Thực thể (Instance Recovery)
* **Bối cảnh áp dụng:** Máy chủ bị mất điện đột ngột hoặc tiến trình nền Oracle bị tắt bất thường (Crash).
* **Cơ chế hoạt động:** Diễn ra hoàn toàn tự động khi Oracle Database được khởi động lại (Startup). Tiến trình **SMON (System Monitor)** sẽ tự động thực hiện hai pha:
  1. **Pha Roll-forward (Cache Recovery):** Đọc tệp Redo log từ điểm checkpoint gần nhất để ghi lại toàn bộ các thay đổi vào Buffer Cache, bao gồm cả các giao dịch đã commit và chưa commit trước khi crash.
  2. **Pha Roll-back (Transaction Recovery):** Sử dụng dữ liệu cũ trong phân vùng **UNDO** để hoàn tác (Rollback) lại toàn bộ các giao dịch dở dang chưa được commit tại thời điểm xảy ra sự cố crash.

#### 1.2.2. Phục hồi Vật lý (Media Recovery)
* **Bối cảnh áp dụng:** Hỏng ổ cứng vật lý chứa datafile, mất controlfile hoặc lỗi bad block nghiêm trọng.
* **Cơ chế hoạt động:** Yêu cầu quản trị viên can thiệp thủ công:
  1. Khôi phục (Restore) lại các tệp tin datafile cũ từ bản sao lưu vật lý (Physical Backup).
  2. Áp dụng (Apply) các thay đổi từ Archived Redo logs và Active Redo logs để cập nhật dữ liệu tiến lên đến thời điểm nhất quán mong muốn.
  * **Complete Recovery:** Phục hồi toàn bộ dữ liệu đến thời điểm hiện tại trước khi xảy ra thảm họa phần cứng mà không làm mất mát giao dịch nào.
  * **Incomplete Recovery (Point-in-Time Recovery - PITR):** Phục hồi cơ sở dữ liệu về một thời điểm chính xác trong quá khứ (ví dụ 10:00 AM hôm qua) trước khi xảy ra một lỗi phá hoại dữ liệu lớn của ứng dụng. Yêu cầu mở database ở chế độ `RESETLOGS` làm mất mát toàn bộ giao dịch từ thời điểm khôi phục đến hiện tại.

#### 1.2.3. Phục hồi Logic (Logical Recovery)
* **Bối cảnh áp dụng:** Người dùng lỡ xóa hoặc cập nhật nhầm bảng dữ liệu, cần khôi phục lại bảng đó từ file dump sao lưu logic.
* **Cơ chế hoạt động:** Sử dụng công cụ `impdp` để đọc tệp `.dmp` và chèn lại dữ liệu vào database.
* **Hạn chế:** Nếu import ghi đè bảng hiện tại, toàn bộ dữ liệu mới phát sinh hợp lệ của bảng đó kể từ lúc sao lưu đến hiện tại sẽ bị ghi đè mất, gây mất mát dữ liệu nghiêm trọng cho các bản ghi không liên quan đến lỗi.

---

### 1.3. Công nghệ phục hồi nhanh Oracle Flashback (Flashback Technology)
Đây là cuộc cách mạng trong phục hồi dữ liệu của Oracle Database, chuyển dịch từ tư duy "Phục hồi vật lý toàn diện" sang "Quay ngược thời gian cục bộ". Flashback cung cấp khả năng khôi phục nhanh chóng ở nhiều cấp độ khác nhau mà không cần phục hồi vật lý:

```
                      ┌─────────────────────────────────────────┐
                      │        CÔNG NGHỆ ORACLE FLASHBACK       │
                      └────────────────────┬────────────────────┘
                                           │
         ┌─────────────────────────────────┼─────────────────────────────────┐
         ▼                                 ▼                                 ▼
┌──────────────────┐             ┌──────────────────┐             ┌──────────────────┐
│Flashback Database│             │  Flashback Drop  │             │  Flashback Table │
│(Cấp block vật lý)│             │ (Khôi phục Recycle│             │ (Đưa bảng về quá │
│(Dùng Flashback   │             │   Bin của table) │             │  khứ qua UNDO)   │
│     logs)        │             └──────────────────┘             └──────────────────┘
└──────────────────┘                                                         │
                                                                             │
         ┌───────────────────────────────────────────────────────────────────┘
         ▼
┌────────────────────────────────────────────────────────────────────────────┐
│                       ORACLE FLASHBACK QUERY (Row-Level)                   │
├────────────────────────────────────────────────────────────────────────────┤
│ * Flashback Query (AS OF TIMESTAMP): Truy vấn dữ liệu dòng tại mốc thời gian│
│ * Flashback Version Query: Xem lịch sử thay đổi dòng qua nhiều giao dịch  │
│ * Flashback Transaction Query: Lấy mã SQL hoàn tác của một giao dịch cụ thể│
└────────────────────────────────────────────────────────────────────────────┘
```

#### 1.3.1. Các mức độ phục hồi của Flashback
1. **Flashback Database:**
   * **Cơ chế:** Hoạt động ở mức độ block vật lý. Khi được kích hoạt, Oracle ghi nhận hình ảnh cũ của các block dữ liệu vào một phân vùng riêng gọi là **Flashback Logs** nằm trong vùng phục hồi nhanh **FRA (Fast Recovery Area)**.
   * **Mục đích:** Hỗ trợ phục hồi toàn bộ database quay ngược thời gian về quá khứ để hoàn tác lỗi sai lớn trên diện rộng mà không cần khôi phục từ backup RMAN.
2. **Flashback Drop:**
   * **Cơ chế:** Khi thực hiện câu lệnh `DROP TABLE`, Oracle không xóa hẳn bảng khỏi không gian lưu trữ đĩa cứng, mà chỉ đổi tên đối tượng và đưa vào một thùng rác ảo gọi là **Recycle Bin**.
   * **Mục đích:** Cho phép khôi phục lại bảng và toàn bộ chỉ mục liên kết bằng câu lệnh `FLASHBACK TABLE ... TO BEFORE DROP` một cách tức thời.
3. **Flashback Table:**
   * **Cơ chế:** Đưa toàn bộ cấu trúc dữ liệu của một bảng quay lại một thời điểm hoặc SCN trong quá khứ bằng cách đọc dữ liệu cũ lưu trong **UNDO Tablespace** và tự động thực thi các lệnh DML đảo ngược (Insert/Update/Delete ngầm) trên bảng đó. Yêu cầu bảng phải được kích hoạt tính năng chuyển dịch dòng dữ liệu (`ROW MOVEMENT`).
4. **Flashback Query (AS OF TIMESTAMP / SCN):**
   * **Cơ chế:** Cho phép người dùng chạy câu lệnh `SELECT` thông thường nhưng đính kèm mệnh đề `AS OF TIMESTAMP` hoặc `AS OF SCN` để truy vấn cấu trúc dữ liệu chính xác tại một thời điểm hoặc mã số thay đổi hệ thống trong quá khứ.
   * Dữ liệu này được Oracle tái tạo động bằng cách lấy bản ghi hiện tại kết hợp với các bản ghi nhật ký thay đổi trong **UNDO Tablespace**.
5. **Flashback Version Query:**
   * **Cơ chế:** Sử dụng mệnh đề `VERSIONS BETWEEN TIMESTAMP` để truy vấn tất cả các phiên bản thay đổi của một dòng dữ liệu cụ thể trải dài qua nhiều giao dịch khác nhau. Cho phép lấy thông tin mã giao dịch (`versions_xid`), hành vi thực hiện (`versions_operation` gồm I, U, D).
6. **Flashback Transaction Query:**
   * **Cơ chế:** Truy vấn bảng hệ thống `FLASHBACK_TRANSACTION_QUERY` bằng mã giao dịch (`XID`) để xem chi tiết lịch sử giao dịch và đặc biệt là lấy ra cột `UNDO_SQL` chứa câu lệnh SQL hoàn tác chính xác cho giao dịch đó.

#### 1.3.2. Hai khái niệm cốt lõi vận hành cơ chế Flashback
* **SCN (System Change Number):** 
  * Đây là chiếc đồng hồ logic của Oracle Database. SCN là một con số tăng dần tuần tự duy nhất ghi nhận thời điểm chính xác một giao dịch được cam kết (Commit) thành công. Mọi thay đổi vật lý và logic trong CSDL đều được định danh bằng một SCN duy nhất. SCN đảm bảo tính tuần tự của lịch sử dữ liệu.
* **UNDO Tablespace & UNDO Retention:**
  * Bộ máy Flashback Query phụ thuộc hoàn toàn vào vùng nhớ **UNDO Tablespace**. Khi một giao dịch thay đổi dữ liệu (ví dụ sửa giá trị cột từ A thành B), giá trị cũ A sẽ được đưa vào một UNDO Block, còn giá trị mới B sẽ ghi vào Data block.
  * Khi giao dịch đã commit, vùng UNDO chứa giá trị A sẽ chuyển sang trạng thái "Unexpired" (Chưa hết hạn). Oracle sẽ giữ lại khối dữ liệu này trong một khoảng thời gian được định nghĩa bởi tham số hệ thống **`UNDO_RETENTION`** (tính bằng giây). Sau thời gian này, các block UNDO đó mới chuyển sang trạng thái "Expired" và có thể bị ghi đè bởi các giao dịch mới. Do đó, ta chỉ có thể Flashback Query ngược thời gian về quá khứ trong giới hạn thời gian lưu giữ của UNDO.

---

## 2. GIẢI THÍCH CƠ CHẾ VÀ LÝ DO LỰA CHỌN PHƯƠNG PHÁP SAO LƯU, PHỤC HỒI

Dựa trên cấu trúc dự án Quản lý Y tế, nhóm/em đã hiện thực hóa việc cài đặt chức năng sao lưu logic định kỳ bằng tệp [03_backup_recovery_setup.sql](file:///d:/HK2-26/ATBM/PH2-2/QuanLyYTe/Script/database/08_backup_restore/03_backup_recovery_setup.sql) và cấp quyền bằng tệp [04_backup_recovery_grants.sql](file:///d:/HK2-26/ATBM/PH2-2/QuanLyYTe/Script/database/08_backup_restore/04_backup_recovery_grants.sql). Dưới đây là phân tích chi tiết cơ chế hoạt động thực tế và lý do kỹ thuật đằng sau sự lựa chọn giải pháp:

### 2.1. Giải pháp Sao lưu: Oracle Data Pump (expdp) & DBMS_SCHEDULER
Mọi thao tác sao lưu được đóng gói trong Package hệ thống [PKG_BACKUP_RECOVERY](file:///d:/HK2-26/ATBM/PH2-2/QuanLyYTe/Script/database/08_backup_restore/03_backup_recovery_setup.sql#L134).

#### 2.1.1. Chi tiết cơ chế lập lịch tự động hàng ngày:
Trong tệp setup, chúng em đã định nghĩa một tiến trình lập lịch chạy tự động định kỳ bằng gói dịch vụ hệ thống `DBMS_SCHEDULER`. Job này được cấu hình mặc định chạy vào lúc **2:00 AM** hàng ngày khi tải hệ thống ở mức thấp nhất:
```sql
BEGIN
    DBMS_SCHEDULER.CREATE_JOB(
        job_name        => 'HOSPITAL.AUTO_BACKUP_JOB',
        job_type        => 'STORED_PROCEDURE',
        job_action      => 'hospital.PKG_BACKUP_RECOVERY.USP_AUTO_BACKUP',
        start_date      => SYSTIMESTAMP,
        repeat_interval => 'FREQ=DAILY;BYHOUR=2;BYMINUTE=0;BYSECOND=0',
        enabled         => FALSE,
        comments        => 'Automatic Data Pump schema backup for HOSPITAL.'
    );
END;
/
```
Khi kích hoạt, Scheduler Job sẽ tự động gọi thủ tục [USP_AUTO_BACKUP](file:///d:/HK2-26/ATBM/PH2-2/QuanLyYTe/Script/database/08_backup_restore/03_backup_recovery_setup.sql#L328) để gọi tiếp hàm lõi [USP_BACKUP_DATAPUMP](file:///d:/HK2-26/ATBM/PH2-2/QuanLyYTe/Script/database/08_backup_restore/03_backup_recovery_setup.sql#L229). 

Thủ tục sử dụng cơ chế xử lý ngoại lệ chặt chẽ. Nếu thành công hay thất bại, thông tin chi tiết về tên file dump phát sinh, thời gian xuất và lỗi hệ thống (nếu có) đều được ghi nhận tự động vào bảng nhật ký [BACKUP_HISTORY](file:///d:/HK2-26/ATBM/PH2-2/QuanLyYTe/Script/database/08_backup_restore/03_backup_recovery_setup.sql#L23) thông qua kỹ thuật **Autonomous Transaction** (giao dịch tự trị chạy độc lập, tự động commit lỗi mà không bị rollback cùng với tiến trình chính khi gặp lỗi lớn).

#### 2.1.2. Lý do chọn phương pháp Sao lưu này:
1. **Không phụ thuộc lệnh OS máy chủ:**
   Thông thường, để backup dữ liệu bằng Data Pump, người ta phải chạy file batch (`.bat` / `.sh`) gọi tiến trình `expdp` ngoài hệ điều hành. Tuy nhiên, cách này đòi hỏi ứng dụng WinForms phải thực thi CMD lệnh máy chủ hoặc DBA phải cài đặt tác vụ nền Windows Task Scheduler. Điều này dẫn đến nguy cơ mất an toàn hệ thống (Hacker có thể chèn mã độc thực thi Command Injection thông qua ứng dụng).
   * Bằng cách gọi package `DBMS_DATAPUMP` trực tiếp bên trong nhân PL/SQL, ứng dụng C# hoàn toàn chạy qua kết nối ADO.NET chuẩn, an toàn tuyệt đối.
2. **Khả năng tối ưu hiệu năng:**
   Trong code thủ tục [USP_BACKUP_DATAPUMP](file:///d:/HK2-26/ATBM/PH2-2/QuanLyYTe/Script/database/08_backup_restore/03_backup_recovery_setup.sql#L229), chúng em đã cấu hình bộ lọc loại bỏ toàn bộ đối tượng tĩnh bao gồm chỉ mục thống kê, thủ tục, trigger, view:
   `IN (''STATISTICS'', ''PROCEDURE'', ''FUNCTION'', ''PACKAGE'', ''VIEW'', ''GRANT'', ''TYPE'', ''TRIGGER'')`.
   Điều này giúp việc sao lưu chỉ tập trung vào cấu trúc bảng và dữ liệu thô, giúp tiến trình backup diễn ra cực kỳ nhanh (chưa tới vài giây đối với cơ sở dữ liệu y tế) và tiết kiệm tài nguyên bộ nhớ đĩa cứng.

---

### 2.2. Giải pháp Phục hồi: Unified Audit Log & Flashback Query
Khi xảy ra sự cố phá hoại dữ liệu (xóa/sửa nhầm dữ liệu đơn thuốc, bệnh án), hệ thống triển khai khôi phục dữ liệu cục bộ thông qua thủ tục [USP_RESTORE_ALL_RECORDS_BY_AUDIT](file:///d:/HK2-26/ATBM/PH2-2/QuanLyYTe/Script/database/08_backup_restore/03_backup_recovery_setup.sql#L333).

#### 2.2.1. Chi tiết cơ chế khôi phục từ Nhật ký kiểm toán:
Nhật ký kiểm toán đóng vai trò là **"Hộp đen"** ghi nhận thời gian xảy ra lỗi. Khi người dùng muốn phục hồi dữ liệu cho một mã bản ghi (ví dụ mã bệnh án bắt đầu bằng `BA` hoặc mã bệnh nhân bắt đầu bằng `BN`):
1. **Tìm mốc thời gian phá hoại:** Thủ tục truy cập bảng hệ thống `UNIFIED_AUDIT_TRAIL` để tìm mốc thời gian cuối cùng xảy ra sự kiện tác động DML lỗi (`UPDATE` hoặc `DELETE`) trên các bảng dữ liệu của schema `HOSPITAL`:
   ```sql
   SELECT MAX(event_timestamp)
   FROM unified_audit_trail
   WHERE object_schema = 'HOSPITAL'
     AND object_name IN ('PRESCRIPTION', 'MEDICAL_RECORD', 'SERVICE_RECORD', 'PATIENT')
     AND action_name IN ('UPDATE', 'DELETE');
   ```
2. **Xác định mốc Flashback an toàn:**
   Hệ thống thực hiện tính toán lùi mốc thời gian an toàn về trước mốc xảy ra sự cố phá hoại đúng 1 giây (nhằm lấy trạng thái hoàn chỉnh nhất của dữ liệu trước khi lệnh phá hoại thực thi):
   ```sql
   v_flashback_time := v_audit_time - NUMTODSINTERVAL(1, 'SECOND');
   ```
3. **Phục hồi dữ liệu động (AS OF TIMESTAMP):**
   Thực hiện so sánh số lượng dòng tại quá khứ (`v_flashback_time`) và hiện tại để đưa ra quyết định xử lý DML khôi phục:
   * **Bản ghi bị SỬA SAI:** Thực hiện lấy ảnh cũ của bản ghi trong quá khứ và cập nhật đè lên bảng dữ liệu hiện tại.
   * **Bản ghi bị XÓA MẤT:** Thực hiện chèn ngược bản ghi từ quá khứ trở lại bảng hiện tại.
   * **Bản ghi bị THÊM SAI (rác):** Thực hiện xóa bỏ bản ghi khỏi bảng hiện tại.
4. **Đồng bộ hóa khóa ngoại (Foreign Key):**
   Khi khôi phục Hồ sơ bệnh án (`MEDICAL_RECORD`), hệ thống tự động khôi phục dữ liệu đồng thời ở 3 bảng theo đúng trình tự cha-con: Đầu tiên khôi phục bảng cha `MEDICAL_RECORD`, sau đó khôi phục dữ liệu liên đới ở hai bảng con là `PRESCRIPTION` và `SERVICE_RECORD`. Điều này giúp hệ thống không bao giờ bị báo lỗi ràng buộc khóa ngoại `ORA-02291`.

#### 2.2.2. Lý do chọn phương pháp Phục hồi này:
1. **Zero Downtime (Online Recovery):**
   Hệ thống y tế là hệ thống nghiệp vụ khẩn cấp (cần truy cập hồ sơ bệnh án 24/7). Việc khôi phục bằng Flashback Query không yêu cầu ngắt kết nối người dùng, không cần chuyển đổi tablespace về trạng thái offline, hay reboot máy chủ CSDL. Mọi thao tác khôi phục diễn ra trong khi các bác sĩ, y tá khác vẫn đang truy xuất hệ thống bình thường.
2. **Bảo toàn giao dịch hợp lệ chéo:**
   Giả sử lúc 10:00 AM xảy ra sự cố xóa nhầm bệnh án `BA0001`. Đến 10:30 AM quản trị viên phát hiện lỗi. Trong 30 phút đó, có 50 bệnh án mới hợp lệ khác (`BA0002` đến `BA0051`) được tạo ra trên toàn hệ thống.
   * Nếu dùng phương pháp **Import file dump** cũ, ta buộc phải ghi đè toàn bộ bảng `MEDICAL_RECORD` về lúc 2:00 AM sáng. Hậu quả là 50 bệnh án mới phát sinh hoàn toàn biến mất khỏi hệ thống.
   * Sử dụng **Flashback Query & Audit**, hệ thống chỉ can thiệp cục bộ (Targeted Recovery) trên đúng dòng bệnh án `BA0001` bị lỗi, bảo toàn nguyên vẹn 50 bệnh án mới hợp lệ khác.

---

## 3. KẾT QUẢ ĐẠT ĐƯỢC

### 3.1. Phía Cơ sở dữ liệu
* **Cài đặt thành công các cấu trúc quản lý:** Cài đặt thành công bảng lưu vết [BACKUP_HISTORY](file:///d:/HK2-26/ATBM/PH2-2/QuanLyYTe/Script/database/08_backup_restore/03_backup_recovery_setup.sql#L23) và [RECOVERY_HISTORY](file:///d:/HK2-26/ATBM/PH2-2/QuanLyYTe/Script/database/08_backup_restore/03_backup_recovery_setup.sql#L80) cùng các sequence hỗ trợ.
* **Cài đặt Package PKG_BACKUP_RECOVERY:** Triển khai toàn bộ logic sao lưu, khôi phục vào database.
* **Cơ chế Import kiểm thử độc lập (Sandbox Restore):** Xây dựng thành công thủ tục [USP_IMPORT_DATAPUMP_TO_RESTORE](file:///d:/HK2-26/ATBM/PH2-2/QuanLyYTe/Script/database/08_backup_restore/03_backup_recovery_setup.sql#L573). Thủ tục này tự động tạo schema độc lập `HOSPITAL_RESTORE` và nạp dữ liệu từ file dump vào đó để DBA kiểm thử, đối chiếu số liệu mà không gây ảnh hưởng đến dữ liệu chạy thực tế.
* **Tự động hóa lập lịch:** Cấu hình và triển khai thành công Oracle Job tự động `HOSPITAL.AUTO_BACKUP_JOB` trong Oracle Scheduler.

### 3.2. Phía Giao diện ứng dụng C# WinForms
Giao diện quản trị [frmBackupRecovery.cs](file:///d:/HK2-26/ATBM/PH2-2/QuanLyYTe/Forms/BackupRecovery/frmBackupRecovery.cs) được xây dựng hoàn thiện:
1. **Sao lưu chủ động:** Nút "Sao lưu ngay" chạy tiến trình xuất dữ liệu Data Pump ngầm, hiển thị kết quả thành công và cập nhật tức thì GridView danh sách file dump.
2. **Sao lưu tự động:** Cung cấp công tắc chuyển đổi bật/tắt Scheduler Job dễ dàng.
3. **Kiểm thử File Dump:** Chọn bản sao lưu bất kỳ, bấm "Import thử nghiệm", hệ thống tự động import vào schema phụ `HOSPITAL_RESTORE` và báo cáo số dòng dữ liệu khôi phục thành công.
4. **Khôi phục nhanh sau sự cố:** Nhập mã bệnh án cần khôi phục (ví dụ `BA000001`), nhấn nút "Khôi phục dữ liệu". Hệ thống tự truy vấn nhật ký kiểm toán, xác định thời điểm phá hoại và khôi phục bản ghi về trạng thái tốt gần nhất, đồng thời ghi nhận kết quả vào lịch sử khôi phục.

---

## 4. SO SÁNH GIỮA CÁC CƠ CHẾ SAO LƯU VÀ PHỤC HỒI

Để có cái nhìn toàn diện hơn về giải pháp bảo mật dữ liệu, nhóm/em xin đưa ra bảng so sánh chi tiết giữa các cơ chế sao lưu và phục hồi:

### 4.1. Bảng so sánh các cơ chế Sao lưu (Backup)

| Tiêu chí so sánh | Sao lưu Vật lý (RMAN / Cold Backup) | Sao lưu Logic (Data Pump / Export) |
| :--- | :--- | :--- |
| **Bản chất kỹ thuật** | Sao chép trực tiếp các khối nhị phân (Blocks) vật lý của CSDL trên đĩa cứng hệ điều hành. | Trích xuất cấu trúc bảng (DDL) và dữ liệu (DML) thành file dump nhị phân trung gian. |
| **Phạm vi áp dụng** | Toàn bộ Database (gồm system tablespace, redo logs, controlfiles) hoặc cấp Tablespace. | Toàn bộ Database, cấp độ Schema (User), hoặc chỉ định các bảng dữ liệu cụ thể. |
| **Tốc độ thực hiện** | **Rất nhanh** vì chỉ thực hiện đọc/ghi tuần tự các khối dữ liệu vật lý trực tiếp. | **Chậm hơn** vì phải thực thi truy vấn SQL để đọc và biên dịch dữ liệu thô thành file dump. |
| **Khả năng khôi phục phần cứng** | **Tuyệt đối.** Khắc phục hoàn toàn lỗi hỏng ổ đĩa cứng vật lý, mất mát file hệ thống CSDL. | **Không thể.** Không thể khởi dựng lại CSDL từ con số không nếu mất các file cấu hình hệ thống. |
| **Tính linh động (Portability)** | **Thấp.** Bản backup chỉ có thể restore trên máy chủ có cùng hệ điều hành và phiên bản Oracle tương thích. | **Rất cao.** Dễ dàng chuyển dịch file dump sang các phiên bản khác nhau hoặc hệ điều hành khác nhau (Windows sang Linux). |
| **Tác động bảo mật ứng dụng** | Yêu cầu đặc quyền SYSDBA và quyền truy cập dòng lệnh OS máy chủ. | Có thể phân quyền thực thi an toàn qua PL/SQL procedure từ giao diện phần mềm WinForms. |
| **Dung lượng lưu trữ** | Lớn (do sao lưu toàn bộ block dữ liệu vật lý thô kể cả các block trống). | Tối ưu (chỉ lưu trữ dữ liệu thực tế và định nghĩa cấu trúc đối tượng). |

---

### 4.2. Bảng so sánh các cơ chế Phục hồi (Recovery)

| Tiêu chí so sánh | Phục hồi vật lý toàn bộ (Full Recovery) | Phục hồi logic truyền thống (Import đè) | Phục hồi bằng Flashback Query & Audit |
| :--- | :--- | :--- | :--- |
| **Cách thức hoạt động** | Khôi phục lại các file datafile cũ từ backup RMAN, chạy Redo logs để tiến lên thời điểm nhất quán. | Sử dụng công cụ `impdp` của Data Pump với tùy chọn ghi đè (`TABLE_EXISTS_ACTION=REPLACE`). | Sử dụng nhật ký Unified Audit xác định mốc sự cố, dùng `AS OF TIMESTAMP` để khôi phục dòng dữ liệu lỗi. |
| **Thời gian dừng hệ thống (Downtime)** | **Rất lớn.** Database buộc phải chuyển sang trạng thái MOUNT và ngắt mọi kết nối ứng dụng (Offline). | **Trung bình.** Có thể chạy Online nhưng dễ xung đột khóa ngoại, cần hạn chế người dùng thao tác. | **Không cần dừng hệ thống** (Online hoàn toàn, người dùng sử dụng bình thường mà không hề hay biết). |
| **Mức độ mất mát dữ liệu** | Mất toàn bộ giao dịch mới phát sinh kể từ mốc thời gian khôi phục đến thời điểm hiện tại. | Mất toàn bộ dữ liệu mới hợp lệ của các bảng dữ liệu bị ghi đè. | **Không mất mát dữ liệu mới** của các bản ghi không liên quan đến sự cố phá hoại. |
| **Độ chi tiết phục hồi (Granularity)** | Thô (cấp độ toàn bộ Database hoặc cấp độ Tablespace). | Trung bình (cấp độ toàn bộ bảng dữ liệu bị ảnh hưởng). | **Cực kỳ chi tiết (Row-level).** Chỉ tác động phục hồi đúng dòng bản ghi bị lỗi. |
| **Yêu cầu không gian lưu trữ** | Bản sao lưu vật lý đầy đủ + các file Redo logs / Archive logs liên tục. | File dump nhị phân của lần sao lưu logic gần nhất. | Phụ thuộc vào không gian UNDO Tablespace chưa bị ghi đè dữ liệu lịch sử. |

---

## KẾT LUẬN

Qua việc nghiên cứu lý thuyết nền tảng và trực tiếp cấu hình hệ thống, nhóm/em nhận thấy việc kết hợp giữa **Sao lưu logic Data Pump (tự động qua Scheduler)** và **Phục hồi nhanh bằng Flashback Query dựa trên Unified Audit Trail** là giải pháp thông minh và tối ưu nhất cho hệ thống Quản lý Y tế. Cơ chế này giúp giải quyết triệt để bài toán khôi phục dữ liệu sau các sự cố do con người hoặc hacker thay đổi thông tin trái phép, đồng thời giảm thiểu tối đa thời gian gián đoạn hệ thống (Downtime) và bảo toàn vẹn toàn bộ các giao dịch hợp lệ khác.
