# Yeu cau 4 - Sao luu va phuc hoi du lieu

Module nay hien thuc hai co che Oracle:

1. **Data Pump**: sao luu chu dong/tu dong va import dump vao schema kiem thu
   `HOSPITAL_RESTORE`.
2. **Flashback Query + Unified Audit**: phuc hoi nhanh du lieu
   `HOSPITAL.PRESCRIPTION` ve thoi diem truoc su co.

## 1. Pham vi ban sao luu Data Pump

Nut `Sao luu chu dong` export schema `HOSPITAL` bang `DBMS_DATAPUMP` va tao:

- file dump `.dmp`;
- file log `.log`;
- mot dong lich su trong `HOSPITAL.BACKUP_HISTORY`.

Dump hien tai bao gom cac thanh phan can cho phuc hoi du lieu:

- bang va toan bo du lieu trong bang;
- sequence;
- index;
- primary key, unique key, foreign key va cac constraint khac.

De import on dinh vao schema kiem thu, export chu dong loai tru:

- statistics;
- procedure va function;
- package;
- view;
- trigger;
- type;
- grant.

Vi vay `HOSPITAL_RESTORE` la **ban phuc hoi du lieu logic**, co the query, so sanh
va lay lai du lieu, nhung khong phai schema clone hoan chinh de chay toan bo ung
dung. Ung dung van ket noi va hoat dong tren schema `HOSPITAL`.

Muon bien `HOSPITAL_RESTORE` thanh schema chay app doc lap, can import them PL/SQL,
view, trigger, quyen, VPD/OLS/audit policy va xu ly cac tham chieu schema. Noi dung
do nam ngoai muc tieu import an toan cua demo hien tai.

Data Pump cung la sao luu logic, khong sao luu datafile, control file, redo log,
UNDO hay toan bo cau hinh instance nhu RMAN.

## 2. Thu tu cai dat tu moi truong sach

### 2.1. Tao thu muc backup tren Windows

Mo PowerShell bang quyen Administrator:

```powershell
New-Item -ItemType Directory -Force C:\OracleBackups
icacls "C:\OracleBackups" /grant "NT SERVICE\OracleServiceXE:(OI)(CI)F"
```

Neu Oracle service co ten khac, kiem tra trong `services.msc` va thay
`OracleServiceXE` bang tai khoan service thuc te.

### 2.2. Khoi tao database ung dung

Chay theo dung thu tu va dung user:

1. `Cleanup.sql` - SYSDBA, container `PDB_QLYT`.
2. `AIO_1.sql` - SYSDBA.
3. `AIO_2.sql` - `HOSPITAL_DBA`.
4. `AIO_3.sql` - SYSDBA.
5. Ngat ket noi, ket noi lai `HOSPITAL_DBA`.
6. `AIO_4.sql` - `HOSPITAL_DBA`.

### 2.3. Cai dat module Backup/Recovery

Chay bang SYS AS SYSDBA:

```sql
ALTER SESSION SET CONTAINER = PDB_QLYT;
@04_backup_recovery_grants.sql
```

Sau do ket noi `HOSPITAL_DBA` vao `PDB_QLYT` va chay:

```sql
@03_backup_recovery_setup.sql
```

Script setup tao:

- `HOSPITAL.BACKUP_HISTORY`;
- `HOSPITAL.RECOVERY_HISTORY`;
- `HOSPITAL.PKG_BACKUP_RECOVERY`;
- cac wrapper procedure cho WinForms;
- `HOSPITAL_DBA.USP_IMPORT_DATAPUMP_TO_RESTORE`;
- `HOSPITAL.AUTO_BACKUP_JOB`.

## 3. Cac chuc nang tren giao dien

### 3.1. Sao luu chu dong - Data Pump Export

1. Dang nhap bang `HOSPITAL_DBA`.
2. Mo `Sao luu va Phuc hoi`.
3. Bam `Sao luu chu dong`.
4. Cho den khi lich su backup hien `STATUS = SUCCESS`.

File duoc tao tai:

```text
C:\OracleBackups
```

Ten file co dang:

```text
hospital_schema_<id>_<yyyyMMdd_HHmmss>.dmp
```

### 3.2. Sao luu tu dong - DBMS_SCHEDULER

Chon chu ky va bam `Bat sao luu tu dong`. Oracle Scheduler se goi cung procedure
Data Pump export. Nut `Tat sao luu tu dong` dung job.

### 3.3. Import dump - Data Pump Import

1. Chon dong backup thanh cong trong bang lich su. Cot `DUMP_FILE` se tu dien vao
   o `File dump`.
2. Co the nhap ten file `.dmp` bang tay, nhung khong nhap duong dan.
3. Bam `Import dump` va xac nhan.
4. Procedure se drop va tao lai `HOSPITAL_RESTORE`.
5. Oracle import voi:
   - `REMAP_SCHEMA=HOSPITAL:HOSPITAL_RESTORE`;
   - `REMAP_TABLESPACE=SYSTEM:HOSPITAL_DATA`;
   - loai tru USER va GRANT de tranh xung dot.
6. Giao dien hien job state, ten log, so bang va so dong `PRESCRIPTION`.

Import khong ghi de schema `HOSPITAL`, vi vay khong lam hong ung dung dang chay.
Moi lan import se thay the toan bo schema `HOSPITAL_RESTORE` cu.

### 3.4. Khoi phuc - Flashback Query

Nut `Khoi phuc` khong thuoc Data Pump. No dung timestamp tu Unified Audit va
Flashback Query `AS OF TIMESTAMP` de phuc hoi truc tiep du lieu
`HOSPITAL.PRESCRIPTION` cua ho so demo `BA000001`.

Hai luong doc lap:

```text
Data Pump: Sao luu chu dong/tu dong -> file .dmp -> Import dump -> HOSPITAL_RESTORE
Flashback: Gia lap UPDATE/DELETE -> Audit timestamp -> Khoi phuc -> HOSPITAL
```

## 4. Kiem tra ket qua

```sql
SELECT *
FROM HOSPITAL.BACKUP_HISTORY
ORDER BY BACKUP_TIME DESC;

SELECT COUNT(*) AS TABLE_COUNT
FROM ALL_TABLES
WHERE OWNER = 'HOSPITAL_RESTORE';

SELECT COUNT(*) AS PRESCRIPTION_ROWS
FROM HOSPITAL_RESTORE.PRESCRIPTION;

SELECT *
FROM HOSPITAL_RESTORE.PRESCRIPTION
WHERE RECORD_ID = 'BA000001';
```

Kiem tra file dump va log tai:

```text
C:\OracleBackups
```

## 5. PowerShell import du phong

Giao dien la cach import chinh. Khi can test ngoai ung dung:

```powershell
cd "D:\Science\Study\Technology\Security and protection in information system\QuanLyYTe\QuanLyYTe\Script\database\08_backup_restore"
.\Run-DataPumpImportToRestore.ps1 -DumpFile "hospital_schema_1_20260617_001212.dmp"
```

Script du phong cung import vao `HOSPITAL_RESTORE`, khong import de len
`HOSPITAL`.

## 6. Ket qua da kiem thu

Backend import tren Oracle XE 21c da duoc kiem thu voi ket qua:

```text
Data Pump job: COMPLETED
So bang trong HOSPITAL_RESTORE: 8
PATIENT: 13 dong
PRESCRIPTION: 1 dong
```

File log ket thuc voi `successfully completed`.

## 7. Xu ly loi khi demo

Giao dien co nut `Mo log loi` trong khu vuc quan ly sao luu. Moi exception cua
module Backup/Recovery duoc ghi kem context va stack trace vao:

```text
<thu muc chay ung dung>\Logs\app_error.log
```

MessageBox loi cung hien duong dan day du cua file nay.

Data Pump tao log Oracle rieng tai:

```text
C:\OracleBackups
```

Khi mot nut bi loi:

1. Bam `Mo log loi` va xem entry moi nhat trong `app_error.log`.
2. Neu loi xay ra khi export/import, mo file `.log` moi nhat trong
   `C:\OracleBackups`.
3. Kiem tra nhanh cac loi pho bien:

| Ma loi | Nguyen nhan thuong gap | Cach xu ly |
|---|---|---|
| `ORA-29283`, `ORA-39070` | Folder khong ton tai hoac Oracle service khong co quyen ghi | Tao `C:\OracleBackups`, chay lai `icacls`, sau do chay lai grant script |
| `ORA-39087` | Dang ket noi sai container hoac directory object chua duoc tao | Ket noi `PDB_QLYT`, chay lai `BackupRecoveryGrants.sql` |
| `ORA-31631`, `ORA-39122` | User import thieu Data Pump import role | Chay lai `BackupRecoveryGrants.sql` bang SYSDBA |
| `ORA-01950` | Schema khong co quota tren tablespace | Chay lai grant/setup; import UI da remap ve `HOSPITAL_DATA` |
| `ORA-20020` | Ten dump khong hop le | Chi nhap ten file `.dmp`, khong nhap duong dan |
| `ORA-20003` | UNDO khong con ban ghi tai timestamp Flashback | Chon audit timestamp gan hon hoac dung Data Pump dump |

Sau khi sua grant/setup trong source code, phai chay lai hai script SQL trong
database; build lai ung dung khong tu dong cap nhat stored procedure Oracle.
