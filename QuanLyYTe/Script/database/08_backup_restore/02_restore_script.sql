-- ==============================================================================
-- 02_restore_script.sql (hoáº·c .bat)
-- ==============================================================================

-- Lá»‡nh nháº­p dá»¯ liá»‡u (impdp) máº«u
-- Cháº¡y lá»‡nh nÃ y trong command prompt/terminal, khÃ´ng cháº¡y trá»±c tiáº¿p trong SQL*Plus

/*
impdp sys/123@localhost:1521/PDB_QLYT AS SYSDBA DIRECTORY=DATA_PUMP_DIR DUMPFILE=hospital_backup_%U.dmp LOGFILE=hospital_restore.log SCHEMAS=hospital,hospital_dba TABLE_EXISTS_ACTION=REPLACE
*/
