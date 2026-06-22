# Hệ thống Quản Lý Dữ Liệu Y Tế (QuanLyYTe)

🏥 **Healthcare Data Management System** with Enterprise Oracle Security Features.

This project is a desktop application built for the **Information System Security** course. It demonstrates how to integrate a C# WinForms application with advanced Oracle Database security mechanisms to protect sensitive healthcare data (patients, medical records, prescriptions).

## 🚀 Technology Stack
- **Database:** Oracle Database 21c XE
- **Application:** C# .NET 8.0 (WinForms)
- **Data Access:** Oracle Managed Data Access (ODP.NET Core)
- **Architecture:** 3-Tier (Presentation, Services, Repositories)

## 🛡️ Security Features Implemented
This project extensively uses Oracle native security features:

1. **Role-Based Access Control (RBAC):**
   - 5 roles: `DBA`, `DOCTOR`, `COORDINATOR`, `TECHNICIAN`, `PATIENT`.
   - Granular Object & System Privileges (including Column-level Access).
2. **Virtual Private Database (VPD):**
   - Row-level security ensuring users only access data they are authorized to see (e.g., Doctors only see their own department's records).
3. **Oracle Label Security (OLS):**
   - Data classification using Levels, Compartments, and Groups.
   - Applied to internal hospital notifications.
4. **Database Auditing:**
   - Standard Auditing, Fine-Grained Auditing (FGA), and Unified Auditing for tracking successful/failed access and illegal modifications.
5. **Backup & Recovery:**
   - Automated Data Pump backups (`expdp`) using `DBMS_SCHEDULER`.
   - Logical recovery using **Flashback Query** combined with Unified Audit timestamps.

## 📂 Project Structure
- `QuanLyYTe/` - The C# .NET WinForms Application.
- `QuanLyYTe/Script/Part 1/` - Foundation SQL scripts (Schema, Tables, Roles, Mock Data).
- `QuanLyYTe/Script/Part 2/05_AIO/` - Advanced Security SQL scripts (VPD, OLS, Audit, Backup).
- `Report.md` - Comprehensive project report and architectural overview.

## ⚙️ Quick Start (Database Setup)
To set up the database, connect to your Oracle Pluggable Database (e.g., `PDB_QLYT`) and run the scripts in order.

**Phase 1 (Foundation):** Run `AIO_1.sql` through `AIO_4.sql` in `Script/Part 1/AIO - All in one/`
**Phase 2 (Security):** Run `AIO_00` to `AIO_05` in `Script/Part 2/05_AIO/` (Check the README in that folder for the exact execution user/roles required for each script).