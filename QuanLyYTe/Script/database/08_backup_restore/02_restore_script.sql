-- ==============================================================================
-- 02_restore_script.sql (hoặc .bat)
-- ==============================================================================

-- Lệnh nhập dữ liệu (impdp) mẫu
-- Chạy lệnh này trong command prompt/terminal, không chạy trực tiếp trong SQL*Plus

/*
impdp sys/123@localhost:1521/PDB_QLYT AS SYSDBA DIRECTORY=DATA_PUMP_DIR DUMPFILE=hospital_backup_%U.dmp LOGFILE=hospital_restore.log SCHEMAS=hospital,hospital_dba TABLE_EXISTS_ACTION=REPLACE
*/
