select *
from hospital.prescription
where record_id = 'BA000001'

select *
from hospital.medical_record
where record_id = 'BA000001'

select *
from hospital.service_record
where record_id = 'BA000001'

select * 
from hospital.staff
where staff_id = 'NV000027'

SELECT * FROM hospital.service_record WHERE technician_id IS NULL;

SELECT owner, table_name
FROM all_tables
WHERE table_name = 'RECOVERY_HISTORY';

SELECT owner, object_name, object_type, status
FROM all_objects
WHERE object_name IN ('RECOVERY_HISTORY', 'SEQ_RECOVERY_ID');

SELECT directory_name, directory_path
FROM dba_directories
WHERE directory_name = 'HOSPITAL_BACKUP_DIR';


SELECT *
FROM HOSPITAL.BACKUP_HISTORY
ORDER BY BACKUP_ID DESC;

INSERT INTO hospital.PRESCRIPTION (RECORD_ID, PRESCRIPTION_DATE, MEDICINE_NAME, DOSAGE) 
VALUES ('BA000001', TO_DATE('01/06/2026', 'DD/MM/YYYY'), N'Paracetamol 500mg', N'1 viên khi đau, tối đa 4 viên/ngày');

COMMIT;