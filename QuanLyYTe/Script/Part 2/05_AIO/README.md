# All-In-One (AIO) Scripts for Part 2

This folder contains the consolidated SQL scripts for Part 2, structured by the sequence of execution and the user required to run them.

## Execution Order

Please execute the scripts in the following order. Make sure you are connected to the correct user for each script. Unless specified in the script, they should be run against `PDB_QLYT`.

### 0. Cleanup (Optional, but recommended if starting fresh)
* **File:** `AIO_00_Cleanup_SYSDBA.sql`
* **Run as:** `SYS AS SYSDBA`
* **Purpose:** Drops existing roles, users, and OLS policies to ensure a clean state before running the setups.

### 1. System Administration Prereqs
* **File:** `AIO_01_SYSDBA.sql`
* **Run as:** `SYS AS SYSDBA`
* **Purpose:** Enables system audit trail, sets up prerequisite privileges for `HOSPITAL_DBA`, creates OLS required users (`HOSP_ARCH`, `HOSP_DATA`), and grants directory/scheduler privileges for the backup process.

### 2. OLS Architecture Setup
* **File:** `AIO_02_OLS_Architecture.sql`
* **Run as:** `HOSPITAL_DBA`
* **Purpose:** Creates the Oracle Label Security (OLS) policy, components (levels, compartments, groups), labels, and assigns labels to the specific user roles.

### 3. Hospital DBA Setup & Data Population
* **File:** `AIO_03_HOSPITAL_DBA.sql`
* **Run as:** `HOSPITAL_DBA`
* **Purpose:** The core setup. This script creates the VPD policies, views, and configures the automated backup scheduler. (Note: Data insertion is assumed to be handled separately in Part 1).

### 4. Auditing Policies
* **File:** `AIO_04_SYSDBA_Audit.sql`
* **Run as:** `SYS AS SYSDBA`
* **Purpose:** Configures Standard, Fine-Grained Auditing (FGA), and Unified Auditing policies on the populated tables.

### 5. OLS Data Labeling
* **File:** `AIO_05_OLS_Data.sql`
* **Run as:** `HOSPITAL_DBA`
* **Purpose:** Applies the OLS policies to the `NOTIFICATION` table and assigns row labels based on the data populated by the `HOSPITAL_DBA`.

---

**Note:** The original individual scripts are still kept in their respective folders (`00_Individual_files`, `01_Combined_RQ1`, `02_OLS`, `03_Audit`, `04_BackupRecovery`) for reference or if you need to run a very specific part individually.
