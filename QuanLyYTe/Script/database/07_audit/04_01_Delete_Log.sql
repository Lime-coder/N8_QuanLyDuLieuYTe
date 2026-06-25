-- Chạy bằng SYSDBA

BEGIN
    -- A. Xóa nhật ký (Logs)
    EXECUTE IMMEDIATE 'DELETE FROM SYS.AUD$';      -- Xóa Standard Audit log
    EXECUTE IMMEDIATE 'DELETE FROM SYS.FGA_LOG$';  -- Xóa Fine-Grained Audit log
    COMMIT;
    
    -- Xóa Unified Audit Trail (phải dùng API, không được DELETE trực tiếp)
    BEGIN
        DBMS_AUDIT_MGMT.FLUSH_UNIFIED_AUDIT_TRAIL;  -- Ép đẩy log từ RAM xuống disk
    EXCEPTION WHEN OTHERS THEN NULL; END;
    
    BEGIN
        -- use_last_arch_timestamp => FALSE: xóa toàn bộ, không cần đánh dấu timestamp
        DBMS_AUDIT_MGMT.CLEAN_AUDIT_TRAIL(
            audit_trail_type        => DBMS_AUDIT_MGMT.AUDIT_TRAIL_UNIFIED,
            use_last_arch_timestamp => FALSE
        );
    EXCEPTION WHEN OTHERS THEN NULL; END;
end;