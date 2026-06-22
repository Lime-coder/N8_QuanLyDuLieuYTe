-- ==============================================================================
-- 01_backup_script.sql (hoặc .bat)
-- ==============================================================================

-- Lệnh xuất dữ liệu (expdp) mẫu
-- Chạy lệnh này trong command prompt/terminal, không chạy trực tiếp trong SQL*Plus

/*
expdp sys/123@localhost:1521/PDB_QLYT AS SYSDBA DIRECTORY=DATA_PUMP_DIR DUMPFILE=hospital_backup_%U.dmp LOGFILE=hospital_backup.log SCHEMAS=hospital,hospital_dba
*/
