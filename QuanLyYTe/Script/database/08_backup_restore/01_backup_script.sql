-- ==============================================================================
-- 01_backup_script.sql (hoáº·c .bat)
-- ==============================================================================

-- Lá»‡nh xuáº¥t dá»¯ liá»‡u (expdp) máº«u
-- Cháº¡y lá»‡nh nÃ y trong command prompt/terminal, khÃ´ng cháº¡y trá»±c tiáº¿p trong SQL*Plus

/*
expdp sys/123@localhost:1521/PDB_QLYT AS SYSDBA DIRECTORY=DATA_PUMP_DIR DUMPFILE=hospital_backup_%U.dmp LOGFILE=hospital_backup.log SCHEMAS=hospital,hospital_dba
*/
