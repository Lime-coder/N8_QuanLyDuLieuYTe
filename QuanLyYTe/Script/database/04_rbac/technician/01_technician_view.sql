-- ==============================================================================
-- 01_technician_view.sql
-- Cháº¡y dÆ°á»›i quyá»n: hospital
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital_dba;

CREATE OR REPLACE VIEW V_TECHNICIAN_SERVICE_RECORD AS
SELECT
    SR.RECORD_ID,
    SR.SERVICE_TYPE,
    SR.SERVICE_DATE,
    SR.TECHNICIAN_ID,
    SR.SERVICE_RESULT
FROM HOSPITAL.SERVICE_RECORD SR
JOIN HOSPITAL.STAFF ST
    ON SR.TECHNICIAN_ID = ST.STAFF_ID
WHERE UPPER(ST.USERNAME_DB) = SYS_CONTEXT('USERENV', 'SESSION_USER')
  AND ST.STAFF_ROLE = UNISTR('K\1EF9 thu\1EADt vi\00EAn')
  AND ST.IS_ACTIVE = 1;

