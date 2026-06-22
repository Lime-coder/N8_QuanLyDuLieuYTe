-- ==============================================================================
-- 01_backup_script.sql (or .bat)
-- ==============================================================================

-- Example Data Pump Export (expdp) command
-- Run this in command prompt/terminal, not directly in SQL*Plus

/*
expdp sys/123@localhost:1521/PDB_QLYT AS SYSDBA DIRECTORY=DATA_PUMP_DIR DUMPFILE=hospital_backup_%U.dmp LOGFILE=hospital_backup.log SCHEMAS=hospital,hospital_dba
*/
