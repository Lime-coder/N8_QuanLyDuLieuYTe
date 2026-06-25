-- ==============================================================================
-- Thư mục AIO - All In One Script
-- Tự động chạy tất cả các kịch bản khởi tạo Cơ sở dữ liệu Quản lý Y tế
-- ==============================================================================
SET ECHO OFF
SET VERIFY OFF

PROMPT ====================================================================
PROMPT = QLYT DATABASE SETUP SCRIPT (AIO)                                 =
PROMPT ====================================================================
PROMPT Vui long nhap mat khau cho cac tai khoan sau:
PROMPT (Tai khoan HOSPITAL_DBA hien dang duoc LOCK nen se su dung SYSDBA de thao tac bang schema cua no)

ACCEPT sys_password CHAR PROMPT 'Enter SYS password: ' HIDE
ACCEPT dba_password CHAR PROMPT 'Enter HOSPITAL_DBA password: ' HIDE

SPOOL AIO_run_log.txt

PROMPT 
PROMPT ====================================================================
PROMPT = 00. KHOI TAO MOI TRUONG (SYS AS SYSDBA TRUOC, SAU DO CHUYEN PDB) =
PROMPT ====================================================================
SET DEFINE ON
CONNECT sys/"&sys_password" AS SYSDBA
-- Chạy enable audit trước khi chuyển qua PDB_QLYT vì nó yêu cầu quyền ở root
@@../00_ENV/01_enable_audit_system.sql
@@../00_ENV/00_set_container.sql
@@../00_ENV/02_init_schemas.sql
SET DEFINE OFF

PROMPT 
PROMPT ====================================================================
PROMPT = 01. KHOI TAO SCHEMA & TABLE (HOSPITAL_DBA)                       =
PROMPT ====================================================================
SET DEFINE ON
CONNECT hospital_dba/"&dba_password"@localhost:1521/PDB_QLYT
SET DEFINE OFF
@@../01_SCHEMA/01_create_tables.sql

PROMPT 
PROMPT ====================================================================
PROMPT = 01. RANG BUOC, CHI MUC & PHAN QUYEN BANG (HOSPITAL_DBA)          =
PROMPT ====================================================================
SET DEFINE ON
CONNECT hospital_dba/"&dba_password"@localhost:1521/PDB_QLYT
SET DEFINE OFF
@@../01_SCHEMA/02_create_constraints.sql
@@../01_SCHEMA/03_create_indexes.sql

PROMPT 
PROMPT ====================================================================
PROMPT = 02. DU LIEU MAU (HOSPITAL_DBA thao tac cho HOSPITAL_DBA)             =
PROMPT ====================================================================
SET DEFINE ON
CONNECT hospital_dba/"&dba_password"@localhost:1521/PDB_QLYT
SET DEFINE OFF
@@../02_SEED_DATA/01_seed_department.sql
@@../02_SEED_DATA/02_seed_staff.sql
@@../02_SEED_DATA/03_seed_patient.sql
@@../02_SEED_DATA/04_seed_medical_record.sql
@@../02_SEED_DATA/05_seed_service_record.sql
@@../02_SEED_DATA/06_seed_prescription.sql
@@../02_SEED_DATA/07_seed_more_mockdata.sql

PROMPT 
PROMPT ====================================================================
PROMPT = 03. TAI KHOAN VA PHAN QUYEN CO BAN                               =
PROMPT ====================================================================
SET DEFINE ON
CONNECT hospital_dba/"&dba_password"@localhost:1521/PDB_QLYT
SET DEFINE OFF
@@../03_ACCOUNT_ROLES/01_create_roles.sql

SET DEFINE ON
CONNECT hospital_dba/"&dba_password"@localhost:1521/PDB_QLYT
SET DEFINE OFF
@@../03_ACCOUNT_ROLES/02_create_users.sql
@@../03_ACCOUNT_ROLES/05_user_role_management.sql
@@../03_ACCOUNT_ROLES/06_privilege_management.sql
@@../03_ACCOUNT_ROLES/07_auth_procedures.sql

SET DEFINE ON
CONNECT hospital_dba/"&dba_password"@localhost:1521/PDB_QLYT
SET DEFINE OFF
@@../03_ACCOUNT_ROLES/03_grant_roles_to_users.sql

PROMPT 
PROMPT ====================================================================
PROMPT = 04. RBAC (HOSPITAL_DBA thao tac cho HOSPITAL_DBA)                    =
PROMPT ====================================================================
SET DEFINE ON
CONNECT hospital_dba/"&dba_password"@localhost:1521/PDB_QLYT
SET DEFINE OFF
@@../04_RBAC/patient/01_patient_view.sql
@@../04_RBAC/patient/02_patient_procedures.sql
@@../04_RBAC/patient/03_patient_grants.sql

@@../04_RBAC/technician/01_technician_view.sql
@@../04_RBAC/technician/02_technician_procedures.sql
@@../04_RBAC/technician/03_technician_grants.sql

PROMPT 
PROMPT ====================================================================
PROMPT = 05. VPD (HOSPITAL_DBA)                                           =
PROMPT ====================================================================
SET DEFINE ON
CONNECT hospital_dba/"&dba_password"@localhost:1521/PDB_QLYT
SET DEFINE OFF
@@../05_VPD/coordinator/01_coordinator_vpd_functions.sql
@@../05_VPD/coordinator/02_coordinator_policies.sql

SET DEFINE ON
CONNECT hospital_dba/"&dba_password"@localhost:1521/PDB_QLYT
SET DEFINE OFF
@@../05_VPD/coordinator/03_coordinator_procedures.sql
@@../05_VPD/coordinator/04_coordinator_grants.sql

SET DEFINE ON
CONNECT hospital_dba/"&dba_password"@localhost:1521/PDB_QLYT
SET DEFINE OFF
@@../05_VPD/doctor/01_doctor_vpd_functions.sql
@@../05_VPD/doctor/02_doctor_policies.sql

SET DEFINE ON
CONNECT hospital_dba/"&dba_password"@localhost:1521/PDB_QLYT
SET DEFINE OFF
@@../05_VPD/doctor/03_doctor_procedures.sql
@@../05_VPD/doctor/04_doctor_grants.sql

PROMPT 
PROMPT ====================================================================
PROMPT = 06. OLS (SYS AS SYSDBA & HOSPITAL_DBA)                           =
PROMPT ====================================================================
SET DEFINE ON
CONNECT sys/"&sys_password"@localhost:1521/PDB_QLYT AS SYSDBA
SET DEFINE OFF
@@../06_OLS/01_create_ols_policy.sql

SET DEFINE ON
CONNECT hospital_dba/"&dba_password"@localhost:1521/PDB_QLYT
SET DEFINE OFF
@@../06_OLS/02_create_labels.sql
@@../06_OLS/03_assign_user_labels.sql
@@../06_OLS/05_ols_tests.sql
@@../06_OLS/04_label_notification_data.sql

PROMPT 
PROMPT ====================================================================
PROMPT = 07. AUDIT (SYS AS SYSDBA & HOSPITAL_DBA)                         =
PROMPT ====================================================================
SET DEFINE ON
CONNECT sys/"&sys_password"@localhost:1521/PDB_QLYT AS SYSDBA
SET DEFINE OFF
@@../07_AUDIT/04_CleanupLogs.sql
@@../07_AUDIT/01_GrantAudit.sql

SET DEFINE ON
CONNECT hospital_dba/"&dba_password"@localhost:1521/PDB_QLYT
SET DEFINE OFF
@@../07_AUDIT/02_AuditingPolicies.sql

PROMPT 
PROMPT ====================================================================
PROMPT = 08. TAO TAI KHOAN DATABASE CHO DU LIEU MAU                       =
PROMPT ====================================================================
SET DEFINE ON
CONNECT hospital_dba/"&dba_password"@localhost:1521/PDB_QLYT
SET DEFINE OFF
@@../03_ACCOUNT_ROLES/04_create_db_accounts_for_seeded_data.sql

PROMPT 
PROMPT ====================================================================
PROMPT = 09. BACKUP VA RECOVERY (SYSDBA & HOSPITAL_DBA)                   =
PROMPT ====================================================================
SET DEFINE ON
CONNECT sys/"&sys_password"@localhost:1521/PDB_QLYT AS SYSDBA
SET DEFINE OFF
@@../08_BACKUP_RESTORE/04_backup_recovery_grants.sql

SET DEFINE ON
CONNECT hospital_dba/"&dba_password"@localhost:1521/PDB_QLYT
SET DEFINE OFF
@@../08_BACKUP_RESTORE/03_backup_recovery_setup.sql

PROMPT ============================================================================
PROMPT = HOAN TAT KHOI TAO CSDL QLYT. Kiem tra log neu co loi!            =
PROMPT = Chu y: Ban can khoi dong lai CSDL de Audit va OLS co hieu luc.   =
PROMPT ====================================================================
SPOOL OFF
EXIT;

