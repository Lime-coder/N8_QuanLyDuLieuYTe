ALTER SESSION SET CONTAINER = PDB_QLYT;
INSERT INTO hospital.COORD_ASSIGNMENT_STAFF (
    username_db,
    staff_id,
    full_name,
    staff_role,
    dept_id,
    specialty
)
SELECT
    s.username_db,
    s.staff_id,
    s.full_name,
    s.staff_role,
    s.dept_id,
    d.dept_name AS specialty
FROM hospital.staff s
LEFT JOIN hospital.department d ON s.dept_id = d.dept_id;
COMMIT;
EXIT;