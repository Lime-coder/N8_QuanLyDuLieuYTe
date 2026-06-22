-- ==============================================================================
-- 01_post_schema_grants.sql (Bá»• sung tá»« Part 1)
-- Cháº¡y dÆ°á»›i quyá»n: SYS AS SYSDBA
-- Cáº¥p quyá»n thao tÃ¡c trÃªn cÃ¡c table/sequence cá»§a hospital cho hospital_dba
-- YÃªu cáº§u: Cháº¡y SAU khi Ä‘Ã£ khá»Ÿi táº¡o xong cÃ¡c tables á»Ÿ bÆ°á»›c 01
-- ==============================================================================
ALTER SESSION SET CONTAINER = PDB_QLYT;

-- Cho phÃ©p hospital_dba cÃ³ quyá»n SELECT vÃ  cÃ³ thá»ƒ GRANT SELECT tiáº¿p cho user khÃ¡c (phá»¥c vá»¥ role/view/VPD)
GRANT SELECT ON hospital.department     TO hospital_dba WITH GRANT OPTION;
GRANT SELECT ON hospital.staff          TO hospital_dba WITH GRANT OPTION;
GRANT SELECT ON hospital.patient        TO hospital_dba WITH GRANT OPTION;
GRANT SELECT ON hospital.medical_record TO hospital_dba WITH GRANT OPTION;
GRANT SELECT ON hospital.service_record TO hospital_dba WITH GRANT OPTION;
GRANT SELECT ON hospital.prescription   TO hospital_dba WITH GRANT OPTION;

-- Cho phÃ©p hospital_dba thao tÃ¡c DML trÃªn staff vÃ  patient (Ä‘á»ƒ quáº£n lÃ½ nhÃ¢n viÃªn, bá»‡nh nhÃ¢n qua trigger hoáº·c package)
GRANT INSERT, UPDATE, DELETE ON hospital.staff TO hospital_dba;
GRANT INSERT, UPDATE, DELETE ON hospital.patient TO hospital_dba;
GRANT INSERT, UPDATE, DELETE ON hospital.medical_record TO hospital_dba;
GRANT INSERT, UPDATE, DELETE ON hospital.service_record TO hospital_dba;
GRANT INSERT, UPDATE, DELETE ON hospital.prescription TO hospital_dba;

-- Cáº¥p quyá»n dÃ¹ng Sequence Ä‘á»ƒ generate ID tá»± Ä‘á»™ng cho tÃ i khoáº£n Linked
GRANT SELECT ON hospital.SEQ_STAFF_ID TO hospital_dba;
GRANT SELECT ON hospital.SEQ_PATIENT_ID TO hospital_dba;
