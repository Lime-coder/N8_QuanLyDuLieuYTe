ALTER SESSION SET CONTAINER = PDB_QLYT;

UPDATE hospital.staff SET dept_id = 'PB02' WHERE dept_id = 'TH';
UPDATE hospital.staff SET dept_id = 'PB01' WHERE dept_id = 'TK';
UPDATE hospital.staff SET dept_id = 'PB03' WHERE dept_id = 'TM';

DELETE FROM hospital.department WHERE dept_id IN ('TH', 'TK', 'TM');

CREATE OR REPLACE VIEW hospital.VW_COORD_DOCTORS AS
SELECT
    username_db,
    staff_id,
    full_name,
    dept_id,
    specialty
FROM hospital.COORD_ASSIGNMENT_STAFF
WHERE staff_role IN (N'Bác sĩ', N'Bác sĩ/Y sĩ');

COMMIT;
EXIT;