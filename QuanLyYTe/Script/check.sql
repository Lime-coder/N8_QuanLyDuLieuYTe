SET PAGESIZE 100
SET LINESIZE 200
COLUMN full_name FORMAT A30
SELECT staff_id, full_name, staff_role FROM hospital.staff WHERE staff_id IN ('NV000151', 'NV000128');
SELECT staff_id, full_name, specialty FROM hospital.COORD_ASSIGNMENT_STAFF WHERE staff_id IN ('NV000151', 'NV000128');
EXIT;
