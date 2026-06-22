-- ==============================================================================
-- 02_create_constraints.sql
-- Run as: hospital (or sysdba with CURRENT_SCHEMA=hospital)
-- ==============================================================================

ALTER SESSION SET CONTAINER = PDB_QLYT;
ALTER SESSION SET CURRENT_SCHEMA = hospital;

-- Foreign Keys for staff
ALTER TABLE staff ADD CONSTRAINT fk_staff_dept FOREIGN KEY (dept_id) REFERENCES department(dept_id);

-- Check constraints for staff
ALTER TABLE staff ADD CONSTRAINT chk_staff_role CHECK (staff_role IN (
    UNISTR('\0110i\1EC1u ph\1ED1i vi\00EAn'),
    UNISTR('B\00E1c s\0129'),
    UNISTR('K\1EF9 thu\1EADt vi\00EAn'),
    UNISTR('T\00E0i v\1EE5'),
    UNISTR('B\00E1c s\0129/Y s\0129')
));
ALTER TABLE staff ADD CONSTRAINT chk_staff_gender CHECK (gender IN ('Nam', UNISTR('N\01B0')));
ALTER TABLE staff ADD CONSTRAINT chk_staff_facility CHECK (facility IN (
    UNISTR('H\1ED3 Ch\00ED Minh'),
    UNISTR('H\1EA3i Ph\00F2ng'),
    UNISTR('H\00E0 N\1ED9i')
));

-- Check constraints for patient
ALTER TABLE patient ADD CONSTRAINT chk_patient_gender CHECK (gender IN ('Nam', UNISTR('N\01B0')));

-- Foreign Keys for medical_record
ALTER TABLE medical_record ADD CONSTRAINT fk_mr_patient FOREIGN KEY (patient_id) REFERENCES patient(patient_id);
ALTER TABLE medical_record ADD CONSTRAINT fk_mr_staff FOREIGN KEY (doctor_id) REFERENCES staff(staff_id);
ALTER TABLE medical_record ADD CONSTRAINT fk_mr_dept FOREIGN KEY (dept_id) REFERENCES department(dept_id);

-- Foreign Keys for service_record
ALTER TABLE service_record ADD CONSTRAINT fk_sr_record FOREIGN KEY (record_id) REFERENCES medical_record(record_id);
ALTER TABLE service_record ADD CONSTRAINT fk_sr_staff FOREIGN KEY (technician_id) REFERENCES staff(staff_id);

-- Foreign Keys for prescription
ALTER TABLE prescription ADD CONSTRAINT fk_presc_record FOREIGN KEY (record_id) REFERENCES medical_record(record_id);
