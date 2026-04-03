CREATE OR REPLACE PROCEDURE USP_TEST (
    p_cursor OUT SYS_REFCURSOR
) AS
BEGIN
    OPEN p_cursor FOR
    SELECT staff_id, full_name, staff_role, dept_id, phone
    FROM staff;
END;
/
CREATE OR REPLACE PROCEDURE USP_TEST2 (
    p_staff_id IN VARCHAR2,
    phone_number OUT VARCHAR2,
    p_dept_name OUT NVARCHAR2 
) AS
BEGIN
    SELECT d.dept_name, s.phone
    INTO p_dept_name, phone_number
    FROM STAFF s
    JOIN DEPARTMENT d ON s.dept_id = d.dept_id
    WHERE s.staff_id = p_staff_id;
END;
/