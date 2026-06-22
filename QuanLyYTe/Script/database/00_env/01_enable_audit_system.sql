-- ==============================================================================
-- 01_enable_audit_system.sql
-- Cháº¡y dÆ°á»›i quyá»n: SYS AS SYSDBA
-- ==============================================================================

ALTER SYSTEM SET audit_trail=DB, EXTENDED SCOPE=SPFILE;
-- Database restart required to take effect.
