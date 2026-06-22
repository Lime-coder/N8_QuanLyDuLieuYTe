ALTER SESSION SET CONTAINER = PDB_QLYT;
DECLARE
  TYPE t_names IS TABLE OF NVARCHAR2(100);
  v_names t_names := t_names(
    N'Lê Minh Khang', N'Nguyễn Văn An', N'Trần Thị Bích Ngọc', N'Phạm Thị Lan', N'Võ Thị Mỹ Linh', N'Đặng Quốc Bảo', N'Bùi Thanh Sơn', N'Đỗ Ngọc Ánh', N'Hồ Hữu Phúc', N'Ngô Gia Huy',
    N'Dương Thanh Tùng', N'Lý Minh Tuấn', N'Nguyễn Thu Hà', N'Trần Quang Đức', N'Lê Thị Phương', N'Phạm Minh Châu', N'Võ Thành Công', N'Đặng Phương Thảo', N'Bùi Gia Khánh', N'Đỗ Thị Diễm',
    N'Hồ Quang Vinh', N'Ngô Thị Thùy', N'Dương Minh Ngọc', N'Lý Đình Phong', N'Nguyễn Hữu Nam', N'Trần Gia Cường', N'Lê Quốc Anh', N'Phạm Thị Mai', N'Võ Minh Quân', N'Đặng Thu Hằng',
    N'Bùi Quốc Thái', N'Đỗ Minh Dũng', N'Hồ Thị Bích', N'Ngô Tuấn Kiệt', N'Dương Gia Bảo', N'Lý Thanh Oanh', N'Nguyễn Minh Hoàng', N'Trần Hữu Hạnh', N'Lê Gia Đạt', N'Phạm Quang Sơn',
    N'Võ Đình Hiếu', N'Đặng Văn Thành', N'Bùi Thu Thủy', N'Đỗ Gia Tùng', N'Hồ Quốc Tuấn', N'Ngô Thị Hồng', N'Dương Minh Hiếu', N'Lý Thu Huyền', N'Nguyễn Quốc Vượng', N'Trần Thị Yến',
    N'Lê Văn Sang', N'Phạm Gia Khang', N'Võ Thu Phương', N'Đặng Quốc Thịnh', N'Bùi Văn Lâm', N'Đỗ Minh Thuận', N'Hồ Thị Ngọc', N'Ngô Quốc Dũng', N'Dương Thu Nga', N'Lý Văn Long',
    N'Nguyễn Gia Phát', N'Trần Đình Toàn', N'Lê Thu Trang', N'Phạm Quốc Hùng', N'Võ Gia Hân', N'Đặng Văn Đại', N'Bùi Thu Cúc', N'Đỗ Quốc Hưng', N'Hồ Đình Nam', N'Ngô Văn Tiến',
    N'Dương Quốc Trung', N'Lý Thu Hà', N'Nguyễn Văn Phúc', N'Trần Gia Long', N'Lê Đình Phú', N'Phạm Thu Thảo', N'Võ Quốc Bình', N'Đặng Gia Hào', N'Bùi Văn Sáng', N'Đỗ Thu Thủy',
    N'Hồ Quốc Thái', N'Ngô Gia Minh', N'Dương Văn Hải', N'Lý Quốc Việt', N'Nguyễn Đình Hậu', N'Trần Thu Ngọc', N'Lê Quốc Bảo', N'Phạm Văn Tâm', N'Võ Gia Phát', N'Đặng Thu Thảo',
    N'Bùi Quốc Cường', N'Đỗ Văn Hậu', N'Hồ Thu Hương', N'Ngô Quốc Vinh', N'Dương Đình Lộc', N'Lý Gia Khánh', N'Nguyễn Thu Thủy', N'Trần Quốc Đạt', N'Lê Văn Hùng', N'Phạm Gia Tuấn',
    N'Võ Thu Trà', N'Đặng Quốc Khang', N'Bùi Gia Lộc', N'Đỗ Thu Hà', N'Hồ Văn Bằng', N'Ngô Quốc Tài', N'Dương Thu Yến', N'Lý Văn Đạt', N'Nguyễn Quốc Thắng', N'Trần Gia Khang',
    N'Lê Thu Vân', N'Phạm Quốc Vĩ', N'Võ Văn Hưng', N'Đặng Gia Hưng', N'Bùi Thu Thảo', N'Đỗ Quốc Vượng', N'Hồ Gia Huy', N'Ngô Thu Ngọc', N'Dương Quốc Hưng', N'Lý Gia Hân',
    N'Nguyễn Văn Thắng', N'Trần Thu Hương', N'Lê Quốc Việt', N'Phạm Gia Đạt', N'Võ Thu Thủy', N'Đặng Quốc Tuấn', N'Bùi Văn Tú', N'Đỗ Gia Khang', N'Hồ Thu Nga', N'Ngô Quốc Bình',
    N'Dương Gia Bảo', N'Lý Thu Thảo', N'Nguyễn Quốc Cường', N'Trần Văn Long', N'Lê Gia Phát', N'Phạm Thu Yến', N'Võ Quốc Khang', N'Đặng Văn Phú', N'Bùi Thu Trang', N'Đỗ Quốc Hùng',
    N'Hồ Gia Lộc', N'Ngô Thu Trà', N'Dương Quốc Thái', N'Lý Văn Đạt', N'Nguyễn Thu Hà', N'Trần Quốc Tuấn', N'Lê Văn Hùng', N'Phạm Gia Khánh', N'Võ Thu Thảo', N'Đặng Quốc Vinh',
    N'Bùi Gia Tuấn', N'Đỗ Thu Hương', N'Hồ Quốc Đạt', N'Ngô Văn Sáng', N'Dương Thu Thủy', N'Lý Quốc Cường', N'Nguyễn Gia Hưng', N'Trần Thu Ngọc', N'Lê Quốc Khang', N'Phạm Văn Lộc',
    N'Võ Gia Hân', N'Đặng Thu Hà', N'Bùi Quốc Bảo', N'Đỗ Văn Tài', N'Hồ Thu Yến', N'Ngô Quốc Việt', N'Dương Gia Lộc', N'Lý Thu Trang', N'Nguyễn Văn Phú', N'Trần Quốc Bình',
    N'Lê Thu Thủy', N'Phạm Gia Đạt', N'Võ Quốc Tuấn', N'Đặng Văn Hưng', N'Bùi Thu Hương', N'Đỗ Quốc Khang', N'Hồ Gia Hưng', N'Ngô Thu Vân', N'Dương Quốc Vượng', N'Lý Văn Tú'
  );
  v_idx NUMBER := 1;
BEGIN
  FOR rec IN (SELECT staff_id FROM hospital.staff ORDER BY staff_id) LOOP
    IF v_idx <= v_names.COUNT THEN
      UPDATE hospital.staff SET full_name = v_names(v_idx) WHERE staff_id = rec.staff_id;
      v_idx := v_idx + 1;
    ELSE
      UPDATE hospital.staff SET full_name = N'Nhân viên ' || rec.staff_id WHERE staff_id = rec.staff_id;
    END IF;
  END LOOP;
  
  UPDATE hospital.COORD_ASSIGNMENT_STAFF c 
  SET (full_name, specialty) = (SELECT s.full_name, d.dept_name FROM hospital.staff s LEFT JOIN hospital.department d ON s.dept_id = d.dept_id WHERE s.username_db = c.username_db);
  COMMIT;
END;
/
EXIT;