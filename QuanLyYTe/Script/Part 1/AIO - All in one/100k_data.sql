-- Run as: hospital_dba | Container: PDB_QLYT
ALTER SESSION SET CURRENT_SCHEMA = hospital;

-- 1. XÓA DỮ LIỆU CŨ ĐỂ CHẠY LẠI TỪ ĐẦU
ALTER TABLE service_record DISABLE CONSTRAINT fk_sr_record;
ALTER TABLE service_record DISABLE CONSTRAINT fk_sr_staff;
ALTER TABLE prescription   DISABLE CONSTRAINT fk_presc_record;
ALTER TABLE medical_record DISABLE CONSTRAINT fk_mr_patient;
ALTER TABLE medical_record DISABLE CONSTRAINT fk_mr_staff;
ALTER TABLE medical_record DISABLE CONSTRAINT fk_mr_dept;
ALTER TABLE staff          DISABLE CONSTRAINT fk_staff_dept;

TRUNCATE TABLE prescription;
TRUNCATE TABLE service_record;
TRUNCATE TABLE medical_record;
TRUNCATE TABLE patient;
TRUNCATE TABLE staff;
TRUNCATE TABLE department;

ALTER TABLE staff          ENABLE CONSTRAINT fk_staff_dept;
ALTER TABLE medical_record ENABLE CONSTRAINT fk_mr_patient;
ALTER TABLE medical_record ENABLE CONSTRAINT fk_mr_staff;
ALTER TABLE medical_record ENABLE CONSTRAINT fk_mr_dept;
ALTER TABLE service_record ENABLE CONSTRAINT fk_sr_record;
ALTER TABLE service_record ENABLE CONSTRAINT fk_sr_staff;
ALTER TABLE prescription   ENABLE CONSTRAINT fk_presc_record;

-- 2. THÊM DỮ LIỆU PHÒNG BAN
INSERT INTO department VALUES ('PB01', N'Thần kinh');
INSERT INTO department VALUES ('PB02', N'Tiêu hóa');
INSERT INTO department VALUES ('PB03', N'Tim mạch');
COMMIT;

-- ============================================================
-- 3. AUTO-GENERATE 170 NHÂN VIÊN CÓ NGHĨA HƠN
-- 20 Điều phối viên, 100 Bác sĩ, 50 Kỹ thuật viên
-- ============================================================
-- 3.1. Tạo 20 Điều phối viên
INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, dept_id, staff_role, username_db, facility)
SELECT
    'NV' || LPAD(LEVEL, 6, '0') AS staff_id,

    CASE MOD(LEVEL, 10)
        WHEN 0 THEN N'Nguyễn Thị Thu Hà'
        WHEN 1 THEN N'Trần Văn Minh'
        WHEN 2 THEN N'Lê Thị Hồng Nhung'
        WHEN 3 THEN N'Phạm Quốc Huy'
        WHEN 4 THEN N'Hoàng Thị Mai Anh'
        WHEN 5 THEN N'Võ Minh Quân'
        WHEN 6 THEN N'Đặng Thị Thanh Trúc'
        WHEN 7 THEN N'Bùi Anh Tuấn'
        WHEN 8 THEN N'Đỗ Thị Ngọc Lan'
        ELSE N'Phan Thành Đạt'
    END AS full_name,

    CASE WHEN MOD(LEVEL, 2) = 0 THEN N'Nữ' ELSE N'Nam' END AS gender,

    DATE '1990-01-01' + LEVEL AS birthdate,

    '079100' || LPAD(LEVEL, 6, '0') AS id_card,

    CASE MOD(LEVEL, 4)
        WHEN 0 THEN N'TP. Hồ Chí Minh'
        WHEN 1 THEN N'Hà Nội'
        WHEN 2 THEN N'Hải Phòng'
        ELSE N'Đà Nẵng'
    END AS hometown,

    '0901' || LPAD(LEVEL, 6, '0') AS phone,

    NULL AS dept_id,

    N'Điều phối viên' AS staff_role,

    'DPV_' || LPAD(LEVEL, 6, '0') AS username_db,
    CASE MOD(LEVEL, 3)
        WHEN 0 THEN N'Hà Nội'
        WHEN 1 THEN N'Hồ Chí Minh'
        ELSE N'Hải Phòng'
    END AS facility
FROM DUAL
CONNECT BY LEVEL <= 20;


-- 3.2. Tạo 100 Bác sĩ
INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, dept_id, staff_role, username_db, facility)
SELECT
    'NV' || LPAD(LEVEL + 20, 6, '0') AS staff_id,

    CASE MOD(LEVEL, 12)
        WHEN 0 THEN N'Nguyễn Văn An'
        WHEN 1 THEN N'Trần Thị Bích Ngọc'
        WHEN 2 THEN N'Lê Minh Khang'
        WHEN 3 THEN N'Phạm Thị Lan'
        WHEN 4 THEN N'Hoàng Văn Dũng'
        WHEN 5 THEN N'Võ Thị Mỹ Linh'
        WHEN 6 THEN N'Đặng Quốc Bảo'
        WHEN 7 THEN N'Bùi Thanh Sơn'
        WHEN 8 THEN N'Đỗ Ngọc Ánh'
        WHEN 9 THEN N'Phan Minh Đức'
        WHEN 10 THEN N'Đinh Thu Phương'
        ELSE N'Cao Gia Huy'
    END AS full_name,

    CASE WHEN MOD(LEVEL, 2) = 0 THEN N'Nữ' ELSE N'Nam' END AS gender,

    DATE '1980-01-01' + MOD(LEVEL * 97, 7000) AS birthdate,

    '079200' || LPAD(LEVEL, 6, '0') AS id_card,

    CASE MOD(LEVEL, 5)
        WHEN 0 THEN N'TP. Hồ Chí Minh'
        WHEN 1 THEN N'Hà Nội'
        WHEN 2 THEN N'Hải Phòng'
        WHEN 3 THEN N'Cần Thơ'
        ELSE N'Đà Nẵng'
    END AS hometown,

    '0902' || LPAD(LEVEL, 6, '0') AS phone,

    CASE
        WHEN LEVEL <= 34 THEN 'PB01'       -- Thần kinh
        WHEN LEVEL <= 67 THEN 'PB02'       -- Tiêu hóa
        ELSE 'PB03'                        -- Tim mạch
    END AS dept_id,

    N'Bác sĩ' AS staff_role,

    'BS_' || LPAD(LEVEL + 20, 6, '0') AS username_db,
    CASE MOD(LEVEL, 3)
        WHEN 0 THEN N'Hà Nội'
        WHEN 1 THEN N'Hồ Chí Minh'
        ELSE N'Hải Phòng'
    END AS facility
FROM DUAL
CONNECT BY LEVEL <= 100;

COMMIT;
-- 3.3. Tạo 50 Kỹ thuật viên
INSERT INTO staff (staff_id, full_name, gender, birthdate, id_card, hometown, phone, dept_id, staff_role, username_db, facility)
SELECT
    'NV' || LPAD(LEVEL + 120, 6, '0') AS staff_id,

    CASE MOD(LEVEL, 10)
        WHEN 0 THEN N'Nguyễn Hải Nam'
        WHEN 1 THEN N'Trần Phương Thảo'
        WHEN 2 THEN N'Lê Đức Anh'
        WHEN 3 THEN N'Phạm Minh Châu'
        WHEN 4 THEN N'Hoàng Nhật Linh'
        WHEN 5 THEN N'Võ Thanh Tùng'
        WHEN 6 THEN N'Đặng Hà My'
        WHEN 7 THEN N'Bùi Quang Hưng'
        WHEN 8 THEN N'Đỗ Thảo Vy'
        ELSE N'Phan Khánh Duy'
    END AS full_name,

    CASE WHEN MOD(LEVEL, 2) = 0 THEN N'Nữ' ELSE N'Nam' END AS gender,

    DATE '1992-01-01' + MOD(LEVEL * 113, 5000) AS birthdate,

    '079300' || LPAD(LEVEL, 6, '0') AS id_card,

    CASE MOD(LEVEL, 4)
        WHEN 0 THEN N'Hải Phòng'
        WHEN 1 THEN N'TP. Hồ Chí Minh'
        WHEN 2 THEN N'Hà Nội'
        ELSE N'Bình Dương'
    END AS hometown,

    '0903' || LPAD(LEVEL, 6, '0') AS phone,

    NULL AS dept_id,

    N'Kỹ thuật viên' AS staff_role,

    'KTV_' || LPAD(LEVEL + 120, 6, '0') AS username_db,
    CASE MOD(LEVEL, 3)
        WHEN 0 THEN N'Hà Nội'
        WHEN 1 THEN N'Hồ Chí Minh'
        ELSE N'Hải Phòng'
    END AS facility
FROM DUAL
CONNECT BY LEVEL <= 50;

COMMIT;
-- ============================================================
-- 4. AUTO-GENERATE 100.000 BỆNH NHÂN
-- ============================================================

INSERT /*+ APPEND */ INTO patient (patient_id, full_name, gender, birthdate, id_card, house_no, street, district, city_province, medical_history, family_medical_history, drug_allergies, username_db
)
SELECT
    'BN' || LPAD(LEVEL, 6, '0') AS patient_id,

    CASE
        -- LEVEL chẵn: Nữ
        WHEN MOD(LEVEL, 2) = 0 THEN
            CASE MOD(LEVEL, 15)
                WHEN 0 THEN N'Nguyễn Thị Lan'
                WHEN 1 THEN N'Trần Thị Mai'
                WHEN 2 THEN N'Lê Ngọc Anh'
                WHEN 3 THEN N'Phạm Thùy Linh'
                WHEN 4 THEN N'Hoàng Mỹ Duyên'
                WHEN 5 THEN N'Võ Thanh Trúc'
                WHEN 6 THEN N'Đặng Hà My'
                WHEN 7 THEN N'Bùi Minh Châu'
                WHEN 8 THEN N'Đỗ Thảo Vy'
                WHEN 9 THEN N'Phan Ngọc Hân'
                WHEN 10 THEN N'Đinh Thu Phương'
                WHEN 11 THEN N'Cao Bảo Ngọc'
                WHEN 12 THEN N'Nguyễn Khánh Linh'
                WHEN 13 THEN N'Trần Gia Hân'
                ELSE N'Lê Minh Thư'
            END

        -- LEVEL lẻ: Nam
        ELSE
            CASE MOD(LEVEL, 15)
                WHEN 0 THEN N'Nguyễn Văn An'
                WHEN 1 THEN N'Trần Minh Quân'
                WHEN 2 THEN N'Lê Quốc Huy'
                WHEN 3 THEN N'Phạm Hoàng Nam'
                WHEN 4 THEN N'Hoàng Đức Dũng'
                WHEN 5 THEN N'Võ Thanh Tùng'
                WHEN 6 THEN N'Đặng Gia Bảo'
                WHEN 7 THEN N'Bùi Anh Tuấn'
                WHEN 8 THEN N'Đỗ Minh Khang'
                WHEN 9 THEN N'Phan Thành Đạt'
                WHEN 10 THEN N'Đinh Nhật Minh'
                WHEN 11 THEN N'Cao Gia Huy'
                WHEN 12 THEN N'Nguyễn Hải Nam'
                WHEN 13 THEN N'Trần Quốc Bảo'
                ELSE N'Lê Đức Anh'
            END
    END AS full_name,

    CASE
        WHEN MOD(LEVEL, 2) = 0 THEN N'Nữ'
        ELSE N'Nam'
    END AS gender,

    DATE '1950-01-01' + MOD(LEVEL * 37, 20000) AS birthdate,

    '001080' || LPAD(LEVEL, 6, '0') AS id_card,

    TO_CHAR(MOD(LEVEL, 300) + 1) AS house_no,

    CASE MOD(LEVEL, 10)
        WHEN 0 THEN N'Nguyễn Trãi'
        WHEN 1 THEN N'Lê Lợi'
        WHEN 2 THEN N'Trần Hưng Đạo'
        WHEN 3 THEN N'Cách Mạng Tháng Tám'
        WHEN 4 THEN N'Điện Biên Phủ'
        WHEN 5 THEN N'Võ Văn Kiệt'
        WHEN 6 THEN N'Phạm Văn Đồng'
        WHEN 7 THEN N'Nguyễn Văn Cừ'
        WHEN 8 THEN N'Hoàng Văn Thụ'
        ELSE N'Lý Thường Kiệt'
    END AS street,

    N'Quận ' || TO_CHAR(MOD(LEVEL, 12) + 1) AS district,

    CASE MOD(LEVEL, 5)
        WHEN 0 THEN N'TP. Hồ Chí Minh'
        WHEN 1 THEN N'Hà Nội'
        WHEN 2 THEN N'Hải Phòng'
        WHEN 3 THEN N'Cần Thơ'
        ELSE N'Đà Nẵng'
    END AS city_province,
    
    CASE MOD(LEVEL, 10)
        WHEN 0 THEN N'Tăng huyết áp nhiều năm, đang dùng thuốc hạ áp định kỳ.'
        WHEN 1 THEN N'Viêm dạ dày mạn tính, hay đau vùng thượng vị sau ăn.'
        WHEN 2 THEN N'Đái tháo đường type 2, đang theo dõi đường huyết định kỳ.'
        WHEN 3 THEN N'Rối loạn lipid máu, được khuyên điều chỉnh chế độ ăn.'
        WHEN 4 THEN N'Viêm xoang dị ứng, thường tái phát khi thay đổi thời tiết.'
        WHEN 5 THEN N'Từng phẫu thuật ruột thừa, hiện không ghi nhận biến chứng.'
        WHEN 6 THEN N'Đau nửa đầu tái phát, tăng khi căng thẳng hoặc thiếu ngủ.'
        WHEN 7 THEN N'Có tiền sử hen phế quản nhẹ, ít khi lên cơn cấp.'
        WHEN 8 THEN N'Từng nhập viện do viêm phổi, đã điều trị ổn định.'
        ELSE N'Chưa ghi nhận bệnh nền đáng chú ý.'
    END AS medical_history,

    CASE MOD(LEVEL, 8)
        WHEN 0 THEN N'Bố bị tăng huyết áp, đang điều trị ngoại trú.'
        WHEN 1 THEN N'Mẹ bị đái tháo đường type 2.'
        WHEN 2 THEN N'Gia đình có tiền sử bệnh tim mạch.'
        WHEN 3 THEN N'Anh/chị/em ruột từng bị viêm dạ dày mạn.'
        WHEN 4 THEN N'Ông/bà có tiền sử tai biến mạch máu não.'
        WHEN 5 THEN N'Gia đình có người mắc rối loạn lipid máu.'
        WHEN 6 THEN N'Không ghi nhận bệnh di truyền hoặc bệnh mạn tính trong gia đình.'
        ELSE N'Chưa khai thác đầy đủ tiền sử bệnh gia đình.'
    END AS family_medical_history,

    CASE MOD(LEVEL, 8)
        WHEN 0 THEN N'Dị ứng Penicillin, từng nổi mẩn đỏ sau khi sử dụng.'
        WHEN 1 THEN N'Dị ứng hải sản, biểu hiện ngứa và nổi mề đay.'
        WHEN 2 THEN N'Dị ứng thuốc giảm đau nhóm NSAID.'
        WHEN 3 THEN N'Dị ứng phấn hoa, thường hắt hơi và nghẹt mũi.'
        WHEN 4 THEN N'Dị ứng bụi nhà, thường gây viêm mũi dị ứng.'
        WHEN 5 THEN N'Từng dị ứng nhẹ với thuốc kháng sinh không rõ loại.'
        WHEN 6 THEN N'Không ghi nhận dị ứng thuốc.'
        ELSE N'Chưa ghi nhận dị ứng.'
    END AS drug_allergies,

    'BN' || LPAD(LEVEL, 6, '0') AS username_db

FROM DUAL
CONNECT BY LEVEL <= 100000;

COMMIT;

-- ============================================================
-- 5. AUTO-GENERATE 10.000 HỒ SƠ BỆNH ÁN
-- ============================================================

INSERT /*+ APPEND */ INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion)
SELECT
    'BA' || LPAD(rn, 6, '0') AS record_id,

    'BN' || LPAD(MOD(rn * 17, 100000) + 1, 6, '0') AS patient_id,

    TRUNC(SYSDATE) - MOD(rn, 365) AS record_date,

    CASE disease_code
        WHEN 0 THEN N'Viêm dạ dày cấp'
        WHEN 1 THEN N'Tăng huyết áp độ 2'
        WHEN 2 THEN N'Đau nửa đầu mạn tính'
        WHEN 3 THEN N'Thoát vị đĩa đệm cột sống thắt lưng'
        WHEN 4 THEN N'Rối loạn nhịp tim'
        WHEN 5 THEN N'Rối loạn tiêu hóa'
        WHEN 6 THEN N'Thiếu máu cơ tim ổn định'
        WHEN 7 THEN N'Sỏi túi mật'
        WHEN 8 THEN N'Đau đầu sau chấn thương nhẹ'
        ELSE N'Trào ngược dạ dày - thực quản'
    END AS diagnosis,

    CASE disease_code
        WHEN 0 THEN N'Dùng thuốc ức chế tiết acid, ăn uống mềm, tránh thức ăn cay nóng'
        WHEN 1 THEN N'Dùng thuốc hạ áp, theo dõi huyết áp mỗi ngày, giảm muối'
        WHEN 2 THEN N'Dùng thuốc giảm đau theo toa, nghỉ ngơi, hạn chế căng thẳng'
        WHEN 3 THEN N'Dùng thuốc giảm đau, vật lý trị liệu, hạn chế mang vác nặng'
        WHEN 4 THEN N'Theo dõi điện tim, dùng thuốc ổn định nhịp theo chỉ định'
        WHEN 5 THEN N'Bù nước, men tiêu hóa, theo dõi đau bụng và số lần đi ngoài'
        WHEN 6 THEN N'Theo dõi triệu chứng đau ngực, kiểm tra điện tim và men tim khi cần'
        WHEN 7 THEN N'Theo dõi đau hạ sườn phải, siêu âm gan mật định kỳ'
        WHEN 8 THEN N'Nghỉ ngơi, theo dõi dấu hiệu thần kinh, chụp kiểm tra nếu cần'
        ELSE N'Dùng thuốc giảm tiết acid, tránh ăn khuya, tái khám nếu đau tăng'
    END AS treatment_plan,

    CASE disease_code
        WHEN 0 THEN 'NV' || LPAD(MOD(rn, 33) + 55, 6, '0')   -- Tiêu hóa: NV000055 - NV000087
        WHEN 1 THEN 'NV' || LPAD(MOD(rn, 33) + 88, 6, '0')   -- Tim mạch: NV000088 - NV000120
        WHEN 2 THEN 'NV' || LPAD(MOD(rn, 34) + 21, 6, '0')   -- Thần kinh: NV000021 - NV000054
        WHEN 3 THEN 'NV' || LPAD(MOD(rn, 34) + 21, 6, '0')   -- Thần kinh
        WHEN 4 THEN 'NV' || LPAD(MOD(rn, 33) + 88, 6, '0')   -- Tim mạch
        WHEN 5 THEN 'NV' || LPAD(MOD(rn, 33) + 55, 6, '0')   -- Tiêu hóa
        WHEN 6 THEN 'NV' || LPAD(MOD(rn, 33) + 88, 6, '0')   -- Tim mạch
        WHEN 7 THEN 'NV' || LPAD(MOD(rn, 33) + 55, 6, '0')   -- Tiêu hóa
        WHEN 8 THEN 'NV' || LPAD(MOD(rn, 34) + 21, 6, '0')   -- Thần kinh
        ELSE 'NV' || LPAD(MOD(rn, 33) + 55, 6, '0')          -- Tiêu hóa
    END AS doctor_id,

    CASE disease_code
        WHEN 0 THEN 'PB02' -- Tiêu hóa
        WHEN 1 THEN 'PB03' -- Tim mạch
        WHEN 2 THEN 'PB01' -- Thần kinh
        WHEN 3 THEN 'PB01' -- Thần kinh
        WHEN 4 THEN 'PB03' -- Tim mạch
        WHEN 5 THEN 'PB02' -- Tiêu hóa
        WHEN 6 THEN 'PB03' -- Tim mạch
        WHEN 7 THEN 'PB02' -- Tiêu hóa
        WHEN 8 THEN 'PB01' -- Thần kinh
        ELSE 'PB02'        -- Tiêu hóa
    END AS dept_id,

    CASE MOD(rn, 5)
        WHEN 0 THEN N'Ổn định, tái khám sau 2 tuần'
        WHEN 1 THEN N'Cần theo dõi thêm'
        WHEN 2 THEN N'Đáp ứng điều trị tốt'
        WHEN 3 THEN N'Hẹn tái khám sau 1 tuần'
        ELSE N'Tiếp tục điều trị ngoại trú'
    END AS conclusion

FROM (
    SELECT
        LEVEL AS rn,
        MOD(LEVEL, 10) AS disease_code
    FROM DUAL
    CONNECT BY LEVEL <= 10000
);

COMMIT;
-- ============================================================
-- 6. AUTO-GENERATE 20.000 DỊCH VỤ CẬN LÂM SÀNG
-- ============================================================

INSERT /*+ APPEND */ INTO service_record (record_id, service_type, service_date, technician_id, service_result)
SELECT
    'BA' || LPAD(rn, 6, '0') AS record_id,

CASE disease_code
    WHEN 0 THEN
        CASE svc_no
            WHEN 1 THEN N'Nội soi dạ dày'
            ELSE N'Xét nghiệm H. pylori'
        END
    WHEN 1 THEN
        CASE svc_no
            WHEN 1 THEN N'Điện tim'
            ELSE N'Xét nghiệm mỡ máu'
        END
    WHEN 2 THEN
        CASE svc_no
            WHEN 1 THEN N'Chụp MRI não'
            ELSE N'Đo điện não đồ'
        END
    WHEN 3 THEN
        CASE svc_no
            WHEN 1 THEN N'Chụp MRI cột sống thắt lưng'
            ELSE N'Chụp X-quang cột sống'
        END
    WHEN 4 THEN
        CASE svc_no
            WHEN 1 THEN N'Điện tim'
            ELSE N'Siêu âm tim'
        END
    WHEN 5 THEN
        CASE svc_no
            WHEN 1 THEN N'Xét nghiệm phân'
            ELSE N'Siêu âm ổ bụng'
        END
    WHEN 6 THEN
        CASE svc_no
            WHEN 1 THEN N'Điện tim gắng sức'
            ELSE N'Siêu âm tim'
        END
    WHEN 7 THEN
        CASE svc_no
            WHEN 1 THEN N'Siêu âm gan mật'
            ELSE N'Xét nghiệm chức năng gan'
        END
    WHEN 8 THEN
        CASE svc_no
            WHEN 1 THEN N'Chụp CT sọ não'
            ELSE N'Đánh giá phản xạ thần kinh'
        END
    ELSE
        CASE svc_no
            WHEN 1 THEN N'Nội soi dạ dày'
            ELSE N'Đo pH thực quản'
        END
END AS service_type,
    TRUNC(SYSDATE) - MOD(rn, 365) AS service_date,

    CASE
    -- PB02 Tiêu hóa: disease 0,5,7,9
    WHEN disease_code IN (0, 5, 7, 9) THEN
        'NV' || LPAD(122 + MOD(rn + svc_no, 16) * 3, 6, '0')

    -- PB03 Tim mạch: disease 1,4,6
    WHEN disease_code IN (1, 4, 6) THEN
        'NV' || LPAD(123 + MOD(rn + svc_no, 16) * 3, 6, '0')

    -- PB01 Thần kinh: disease 2,3,8
    ELSE
        'NV' || LPAD(121 + MOD(rn + svc_no, 17) * 3, 6, '0')
    END AS technician_id,

    CASE
        WHEN MOD(rn + svc_no, 4) = 0 THEN NULL
        ELSE
            CASE disease_code
                WHEN 0 THEN
                    CASE svc_no
                        WHEN 1 THEN N'Niêm mạc dạ dày sung huyết nhẹ'
                        ELSE N'H. pylori âm tính'
                    END
                WHEN 1 THEN
                    CASE svc_no
                        WHEN 1 THEN N'Nhịp xoang, chưa ghi nhận bất thường cấp tính'
                        ELSE N'Cholesterol tăng nhẹ'
                    END
                WHEN 2 THEN
                    CASE svc_no
                        WHEN 1 THEN N'Chưa ghi nhận tổn thương khu trú'
                        ELSE N'Sóng điện não trong giới hạn bình thường'
                    END
                WHEN 3 THEN
                    CASE svc_no
                        WHEN 1 THEN N'Lồi đĩa đệm nhẹ vùng L4-L5'
                        ELSE N'Không ghi nhận tổn thương xương cấp tính'
                    END
                WHEN 4 THEN
                    CASE svc_no
                        WHEN 1 THEN N'Tăng đậm phế quản hai bên'
                        ELSE N'Bạch cầu tăng nhẹ'
                    END
                WHEN 5 THEN
                    CASE svc_no
                        WHEN 1 THEN N'Không phát hiện ký sinh trùng'
                        ELSE N'Tăng nhu động ruột, chưa thấy dịch ổ bụng'
                    END
                WHEN 6 THEN
                    CASE svc_no
                        WHEN 1 THEN N'Nhịp nhanh xoang'
                        ELSE N'Chức năng co bóp thất trái trong giới hạn'
                    END
                WHEN 7 THEN
                    CASE svc_no
                        WHEN 1 THEN N'Sỏi túi mật kích thước nhỏ'
                        ELSE N'Men gan trong giới hạn bình thường'
                    END
                WHEN 8 THEN
                    CASE svc_no
                        WHEN 1 THEN N'Không thấy xuất huyết nội sọ'
                        ELSE N'Phản xạ đồng đều hai bên'
                    END
                ELSE
                    CASE svc_no
                        WHEN 1 THEN N'Các chỉ số huyết học trong giới hạn'
                        ELSE N'Không ghi nhận bất thường'
                    END
            END
    END AS service_result

FROM (
    SELECT
        mr.rn,
        mr.disease_code,
        svc.svc_no
    FROM (
        SELECT
            LEVEL AS rn,
            MOD(LEVEL, 10) AS disease_code
        FROM DUAL
        CONNECT BY LEVEL <= 10000
    ) mr
    CROSS JOIN (
        SELECT 1 AS svc_no FROM DUAL
        UNION ALL
        SELECT 2 AS svc_no FROM DUAL
    ) svc
);

COMMIT;


-- ============================================================
-- 7. AUTO-GENERATE 20.000 ĐƠN THUỐC
-- ============================================================

INSERT /*+ APPEND */ INTO prescription (record_id, prescription_date, medicine_name, dosage)
SELECT
    'BA' || LPAD(rn, 6, '0') AS record_id,

    TRUNC(SYSDATE) - MOD(rn, 365) AS prescription_date,

CASE disease_code
    WHEN 0 THEN
        CASE med_no
            WHEN 1 THEN 'Omeprazole 20mg'
            ELSE 'Sucralfate 1g'
        END
    WHEN 1 THEN
        CASE med_no
            WHEN 1 THEN 'Amlodipine 5mg'
            ELSE 'Losartan 50mg'
        END
    WHEN 2 THEN
        CASE med_no
            WHEN 1 THEN 'Paracetamol 500mg'
            ELSE 'Magnesium B6'
        END
    WHEN 3 THEN
        CASE med_no
            WHEN 1 THEN 'Meloxicam 7.5mg'
            ELSE 'Vitamin B1-B6-B12'
        END
    WHEN 4 THEN
        CASE med_no
            WHEN 1 THEN 'Bisoprolol 2.5mg'
            ELSE 'Magnesium B6'
        END
    WHEN 5 THEN
        CASE med_no
            WHEN 1 THEN 'Smecta'
            ELSE 'Oresol'
        END
    WHEN 6 THEN
        CASE med_no
            WHEN 1 THEN 'Nitroglycerin 2.5mg'
            ELSE 'Aspirin 81mg'
        END
    WHEN 7 THEN
        CASE med_no
            WHEN 1 THEN 'Ursodeoxycholic acid 250mg'
            ELSE 'Hyoscine butylbromide 10mg'
        END
    WHEN 8 THEN
        CASE med_no
            WHEN 1 THEN 'Paracetamol 500mg'
            ELSE 'Piracetam 800mg'
        END
    ELSE
        CASE med_no
            WHEN 1 THEN 'Esomeprazole 20mg'
            ELSE 'Domperidone 10mg'
        END
END AS medicine_name,
    CASE disease_code
        WHEN 0 THEN
            CASE med_no
                WHEN 1 THEN N'1 viên/lần, ngày 1 lần trước ăn sáng'
                ELSE N'1 gói/lần, ngày 2 lần sau ăn'
            END
        WHEN 1 THEN
            CASE med_no
                WHEN 1 THEN N'1 viên/lần, ngày 1 lần buổi sáng'
                ELSE N'1 viên/lần, ngày 1 lần buổi tối'
            END
        WHEN 2 THEN
            CASE med_no
                WHEN 1 THEN N'1 viên khi đau, tối đa 4 viên/ngày'
                ELSE N'1 viên/lần, ngày 2 lần sau ăn'
            END
        WHEN 3 THEN
            CASE med_no
                WHEN 1 THEN N'1 viên/lần, ngày 1 lần sau ăn'
                ELSE N'1 viên/lần, ngày 2 lần'
            END
        WHEN 4 THEN
            CASE med_no
                WHEN 1 THEN N'1 gói/lần, ngày 3 lần'
                ELSE N'1 viên/lần, ngày 1 lần buổi tối'
            END
        WHEN 5 THEN
            CASE med_no
                WHEN 1 THEN N'1 gói/lần, ngày 3 lần'
                ELSE N'Pha 1 gói với 200ml nước, uống từng ngụm'
            END
        WHEN 6 THEN
            CASE med_no
                WHEN 1 THEN N'1 viên/lần, ngày 1 lần buổi sáng'
                ELSE N'1 viên/lần, ngày 2 lần'
            END
        WHEN 7 THEN
            CASE med_no
                WHEN 1 THEN N'1 viên/lần, ngày 2 lần sau ăn'
                ELSE N'1 viên khi đau quặn bụng'
            END
        WHEN 8 THEN
            CASE med_no
                WHEN 1 THEN N'1 viên khi đau, tối đa 4 viên/ngày'
                ELSE N'1 viên/lần, ngày 2 lần'
            END
        ELSE
            CASE med_no
                WHEN 1 THEN N'1 viên/lần, ngày 1 lần sau ăn'
                ELSE N'1 viên/lần, ngày 1 lần'
            END
    END AS dosage

FROM (
    SELECT
        mr.rn,
        mr.disease_code,
        med.med_no
    FROM (
        SELECT
            LEVEL AS rn,
            MOD(LEVEL, 10) AS disease_code
        FROM DUAL
        CONNECT BY LEVEL <= 10000
    ) mr
    CROSS JOIN (
        SELECT 1 AS med_no FROM DUAL
        UNION ALL
        SELECT 2 AS med_no FROM DUAL
    ) med
);

COMMIT;

-- ============================================================
-- 8. EXPLICIT RECORDS FOR 4 SPECIFIC TESTING ACCOUNTS
-- ============================================================
-- Ensure BN000001, NV000021, and NV000121 have records so they don't just exist as users.
INSERT INTO medical_record (record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion)
VALUES ('BA999999', 'BN000001', SYSDATE, N'Khám sức khỏe tổng quát', N'Nghỉ ngơi', 'NV000021', 'PB01', N'Khỏe mạnh');

INSERT INTO service_record (record_id, service_type, service_date, technician_id, service_result)
VALUES ('BA999999', N'Siêu âm', SYSDATE, 'NV000121', N'Bình thường');

COMMIT;
-- Ensure at least 10 medical history records for BN000001
BEGIN
    FOR i IN 1..10 LOOP
        INSERT INTO hospital.medical_record (
            record_id, patient_id, record_date, diagnosis, treatment_plan, doctor_id, dept_id, conclusion
        ) VALUES (
            'T_BA' || LPAD(i, 3, '0'), 'BN000001', SYSDATE - (30 - i), N'Bệnh lý kiểm tra số ' || i, N'Kế hoạch điều trị ' || i, 'NV000021', 'PB01', N'Kết luận ' || i
        );

        INSERT INTO hospital.service_record (
            record_id, service_type, service_date, technician_id, service_result
        ) VALUES (
            'T_BA' || LPAD(i, 3, '0'), N'Dịch vụ khám ' || i, SYSDATE - (30 - i), 'NV000121', N'Kết quả ' || i
        );

        INSERT INTO hospital.prescription (
            record_id, prescription_date, medicine_name, dosage
        ) VALUES (
            'T_BA' || LPAD(i, 3, '0'), SYSDATE - (30 - i), N'Thuốc ' || i, N'Liều lượng ' || i
        );
    END LOOP;
    COMMIT;
END;
/
