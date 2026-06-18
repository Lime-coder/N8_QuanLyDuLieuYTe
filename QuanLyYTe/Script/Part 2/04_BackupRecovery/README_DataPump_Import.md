# Data Pump Import To Restore Schema

This folder implements Requirement 4 with:

- Export from the WinForms app using `hospital.USP_MANUAL_BACKUP`.
- Auto export using `DBMS_SCHEDULER`.
- Fast record recovery using Flashback Query and Unified Audit timestamp.
- Manual Data Pump import into `HOSPITAL_RESTORE` for safe restore validation.

## 1. Prepare Windows Folder

Run PowerShell as Administrator:

```powershell
New-Item -ItemType Directory -Force C:\OracleBackups
icacls "C:\OracleBackups" /grant "NT SERVICE\OracleServiceXE:(OI)(CI)F"
```

If your Oracle service name is different, check it in `services.msc` and replace `OracleServiceXE`.

## 2. Run Oracle Grants

Connect to `PDB_QLYT` as `SYS AS SYSDBA`, then run:

```sql
@BackupRecoveryGrants.sql
```

## 3. Install Backup/Recovery Objects

Connect as `HOSPITAL_DBA`, then run:

```sql
@BackupRecoverySetup.sql
```

This creates:

- `hospital.BACKUP_HISTORY`
- `hospital.RECOVERY_HISTORY`
- `hospital.PKG_BACKUP_RECOVERY`
- wrapper procedures used by WinForms
- `HOSPITAL.AUTO_BACKUP_JOB`
- Unified Audit policy for `hospital.PRESCRIPTION`

## 4. Export From The App

Login as DBA, open `Sao luu va Phuc hoi`, then press:

```text
Sao luu chu dong
```

After success, check the backup history grid and copy the value in `DUMP_FILE`, for example:

```text
hospital_schema_1_20260617_001212.dmp
```

The physical file is created under:

```text
C:\OracleBackups
```

## 5. Import Into HOSPITAL_RESTORE

Open PowerShell in this folder:

```powershell
cd "D:\ATBM-Oracle\Project\N8_QuanLyDuLieuYTe\QuanLyYTe\Script\Part 2\BackupRecovery"
```

Run:

```powershell
.\Run-DataPumpImportToRestore.ps1 -DumpFile "hospital_schema_1_20260617_001212.dmp"
```

The script will:

1. Ask for the `hospital_dba` password.
2. Drop old `HOSPITAL_RESTORE` unless `-SkipDrop` is used.
3. Create `HOSPITAL_RESTORE`.
4. Run `impdp` with `REMAP_SCHEMA=HOSPITAL:HOSPITAL_RESTORE`.
5. Verify imported tables and `HOSPITAL_RESTORE.PRESCRIPTION` row count.

## 6. Useful Manual Checks

```sql
SELECT COUNT(*) FROM HOSPITAL_RESTORE.PRESCRIPTION;
SELECT * FROM HOSPITAL_RESTORE.PRESCRIPTION WHERE RECORD_ID = 'BA000001';
SELECT * FROM HOSPITAL.BACKUP_HISTORY ORDER BY BACKUP_TIME DESC;
```

## Notes

- Import is intentionally done into `HOSPITAL_RESTORE`, not `HOSPITAL`, to avoid damaging the running application schema.
- Flashback recovery is still available in the app for quick recovery by audit timestamp.
- Data Pump import is the validation path proving that the exported dump can be restored.
