-- ==============================================================================
-- 02_coordinator_vpd_functions.sql
-- Chạy dưới quyền: hospital_dba
-- ==============================================================================

 CREATE OR REPLACE FUNCTION FN_VPD_STAFF_SELF (
     p_schema VARCHAR2,
     p_obj    VARCHAR2
 )
 RETURN VARCHAR2
 AS
+    v_user VARCHAR2(100);
+    v_role NVARCHAR2(50);
 BEGIN
-    IF SYS_CONTEXT('USERENV', 'SESSION_USER') IN ('HOSPITAL', 'HOSPITAL_DBA') THEN
+    v_user := SYS_CONTEXT('USERENV', 'SESSION_USER');
+
+    -- DBA/schema owner → bypass
+    IF v_user IN ('HOSPITAL', 'HOSPITAL_DBA') THEN
         RETURN '1=1';
     END IF;
 
-    RETURN 'UPPER(username_db) = SYS_CONTEXT(''USERENV'', ''SESSION_USER'')';
+    -- Kiểm tra vai trò: Coordinator cần xem tất cả staff (TC#2)
+    BEGIN
+        SELECT staff_role INTO v_role
+        FROM hospital.staff
+        WHERE UPPER(username_db) = v_user;
+    EXCEPTION WHEN NO_DATA_FOUND THEN
+        RETURN '1=0'; -- Không phải nhân viên → không thấy gì
+    END;
+
+    IF v_role = UNISTR('\0110i\1EC1u ph\1ED1i vi\00EAn') THEN
+        RETURN '1=1'; -- Coordinator thấy tất cả (TC#2)
+    END IF;
+
+    -- Bác sĩ, KTV → chỉ thấy chính mình (TC#5)
+    RETURN 'UPPER(username_db) = ''' || v_user || '''';
 
 EXCEPTION
     WHEN OTHERS THEN
         RETURN '1=0';
 END;

