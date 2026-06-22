-- ==============================================================================
-- 02_restore_script.sql (or .bat)
-- ==============================================================================

-- Example Data Pump Import (impdp) command
-- Run this in command prompt/terminal, not directly in SQL*Plus

/*
impdp sys/123@localhost:1521/PDB_QLYT AS SYSDBA DIRECTORY=DATA_PUMP_DIR DUMPFILE=hospital_backup_%U.dmp LOGFILE=hospital_restore.log SCHEMAS=hospital,hospital_dba TABLE_EXISTS_ACTION=REPLACE
*/
