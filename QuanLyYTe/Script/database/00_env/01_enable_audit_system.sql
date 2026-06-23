-- ==============================================================================
-- 01_enable_audit_system.sql
-- Chạy dưới quyền: SYS AS SYSDBA
-- ==============================================================================

ALTER SYSTEM SET audit_trail=DB, EXTENDED SCOPE=SPFILE;
ALTER SYSTEM SET streams_pool_size=64M SCOPE=BOTH;
-- Database restart required to take effect.
