using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DataProvider;

namespace QuanLyYTe.Repositories
{
    public class CoordinatorRepository
    {
        private readonly OracleDbProvider _dbProvider = new OracleDbProvider();
        private const string AssignmentContext = "COORD_ASSIGN";

        private DataTable ExecuteAssignmentQuery(string sql)
        {
            DataTable dt = new DataTable();
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            {
                conn.Open();
                using (OracleCommand setContextCmd = new OracleCommand("BEGIN DBMS_SESSION.SET_IDENTIFIER(:p_identifier); END;", conn))
                {
                    setContextCmd.Parameters.Add(new OracleParameter("p_identifier", OracleDbType.Varchar2) { Value = AssignmentContext });
                    setContextCmd.ExecuteNonQuery();
                }
                using (OracleCommand queryCmd = new OracleCommand(sql, conn))
                {
                    using (OracleDataAdapter da = new OracleDataAdapter(queryCmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable GetDoctors() => GetDoctorsForAssignment();
        public DataTable GetTechnicians() => GetTechniciansForAssignment();

        public DataTable GetDoctorsForAssignment()
        {
            string sql = "SELECT username_db, staff_id, full_name, specialty, TO_NCHAR(full_name) || N' - ' || TO_NCHAR(specialty) AS display_name FROM hospital.VW_COORD_DOCTORS ORDER BY full_name";
            return ExecuteAssignmentQuery(sql);
        }

        public DataTable GetDoctorsByDepartment(string deptId)
        {
            string sql = "SELECT username_db, staff_id, full_name, specialty, TO_NCHAR(full_name) || N' - ' || TO_NCHAR(specialty) AS display_name FROM hospital.VW_COORD_DOCTORS WHERE dept_id = :dept_id ORDER BY full_name";
            DataTable dt = new DataTable();
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            {
                conn.Open();
                using (OracleCommand setContextCmd = new OracleCommand("BEGIN DBMS_SESSION.SET_IDENTIFIER(:p_identifier); END;", conn))
                {
                    setContextCmd.Parameters.Add(new OracleParameter("p_identifier", OracleDbType.Varchar2) { Value = AssignmentContext });
                    setContextCmd.ExecuteNonQuery();
                }
                using (OracleCommand queryCmd = new OracleCommand(sql, conn))
                {
                    queryCmd.Parameters.Add(new OracleParameter("dept_id", OracleDbType.Varchar2) { Value = deptId });
                    using (OracleDataAdapter da = new OracleDataAdapter(queryCmd)) { da.Fill(dt); }
                }
            }
            return dt;
        }

        public DataTable GetTechniciansForAssignment()
        {
            string sql = "SELECT username_db, staff_id, full_name, TO_NCHAR(full_name) || N' (' || TO_NCHAR(staff_id) || N')' AS display_name FROM hospital.VW_COORD_TECHNICIANS ORDER BY full_name";
            return ExecuteAssignmentQuery(sql);
        }

        public DataTable GetDepartments()
        {
            string sql = "SELECT dept_id, TO_NCHAR(dept_name) AS dept_name FROM hospital.department ORDER BY dept_id";
            return _dbProvider.ExecuteQuery(sql);
        }

        public void UpdateMedicalRecord(string recordId, string doctorId, string deptId)
        {
            string sql = "UPDATE hospital.medical_record SET doctor_id = :doctorId, dept_id = :deptId WHERE record_id = :recordId";
            _dbProvider.ExecuteNonQuery(sql, new OracleParameter[] {
                new OracleParameter("doctorId", OracleDbType.Varchar2) { Value = doctorId },
                new OracleParameter("deptId", OracleDbType.Varchar2) { Value = deptId },
                new OracleParameter("recordId", OracleDbType.Varchar2) { Value = recordId }
            });
        }

        public void UpdateServiceRecord(string recordId, string technicianId)
        {
            string sql = "UPDATE hospital.service_record SET technician_id = :technicianId WHERE record_id = :recordId";
            _dbProvider.ExecuteNonQuery(sql, new OracleParameter[] {
                new OracleParameter("technicianId", OracleDbType.Varchar2) { Value = technicianId },
                new OracleParameter("recordId", OracleDbType.Varchar2) { Value = recordId }
            });
        }

        public DataTable GetSelfStaffInfo()
        {
            string sql = "SELECT s.staff_id, s.full_name, s.staff_role, s.phone, s.hometown, d.dept_name AS specialty FROM hospital.staff s LEFT JOIN hospital.department d ON s.dept_id = d.dept_id WHERE s.username_db = SYS_CONTEXT('USERENV', 'SESSION_USER')";
            return _dbProvider.ExecuteQuery(sql);
        }

        public void UpdateSelfStaffInfo(string phone, string hometown)
        {
            string sql = "UPDATE hospital.staff SET phone = :phone, hometown = :hometown WHERE username_db = SYS_CONTEXT('USERENV', 'SESSION_USER')";
            _dbProvider.ExecuteNonQuery(sql, new OracleParameter[] {
                new OracleParameter("phone", OracleDbType.Varchar2) { Value = phone },
                new OracleParameter("hometown", OracleDbType.NVarchar2) { Value = hometown }
            });
        }

        // --- PatientRepository methods ---
        public DataTable GetAllPatients()
        {
            string sql = "SELECT * FROM hospital.patient ORDER BY patient_id";
            return _dbProvider.ExecuteQuery(sql);
        }

        public DataTable SearchPatients(string keyword)
        {
            string sql = "SELECT * FROM hospital.patient WHERE UPPER(patient_id) LIKE UPPER(:keyword) OR UPPER(full_name) LIKE UPPER(:keyword) OR UPPER(id_card) LIKE UPPER(:keyword) ORDER BY patient_id";
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            using (OracleCommand cmd = new OracleCommand(sql, conn))
            {
                cmd.Parameters.Add(new OracleParameter("keyword", OracleDbType.NVarchar2) { Value = "%" + keyword + "%" });
                DataTable dt = new DataTable();
                using (OracleDataAdapter da = new OracleDataAdapter(cmd)) { da.Fill(dt); }
                return dt;
            }
        }

        private bool PatientIdExists(string patientId)
        {
            string sql = "SELECT COUNT(*) FROM hospital.patient WHERE patient_id = :patient_id";
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            using (OracleCommand cmd = new OracleCommand(sql, conn))
            {
                cmd.Parameters.Add(new OracleParameter("patient_id", OracleDbType.Varchar2) { Value = patientId });
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private bool IdCardExists(string idCard, string excludePatientId = null)
        {
            string sql = excludePatientId == null ? "SELECT COUNT(*) FROM hospital.patient WHERE id_card = :id_card" : "SELECT COUNT(*) FROM hospital.patient WHERE id_card = :id_card AND patient_id != :exclude_id";
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            using (OracleCommand cmd = new OracleCommand(sql, conn))
            {
                cmd.Parameters.Add(new OracleParameter("id_card", OracleDbType.Varchar2) { Value = idCard });
                if (excludePatientId != null) cmd.Parameters.Add(new OracleParameter("exclude_id", OracleDbType.Varchar2) { Value = excludePatientId });
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private bool UsernameDbExists(string usernameDb, string excludePatientId = null)
        {
            if (string.IsNullOrEmpty(usernameDb)) return false;
            string sql = excludePatientId == null ? "SELECT COUNT(*) FROM hospital.patient WHERE username_db = :username_db" : "SELECT COUNT(*) FROM hospital.patient WHERE username_db = :username_db AND patient_id != :exclude_id";
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            using (OracleCommand cmd = new OracleCommand(sql, conn))
            {
                cmd.Parameters.Add(new OracleParameter("username_db", OracleDbType.Varchar2) { Value = usernameDb });
                if (excludePatientId != null) cmd.Parameters.Add(new OracleParameter("exclude_id", OracleDbType.Varchar2) { Value = excludePatientId });
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public void InsertPatient(string patientId, string fullName, string gender, DateTime birthDate, string idCard, string houseNo, string street, string district, string cityProvince, string medicalHistory, string familyMedicalHistory, string drugAllergies, string usernameDb)
        {
            if (PatientIdExists(patientId)) throw new InvalidOperationException("Mã bệnh nhân đã tồn tại.");
            if (IdCardExists(idCard)) throw new InvalidOperationException("Số CMND/CCCD đã tồn tại.");
            if (UsernameDbExists(usernameDb)) throw new InvalidOperationException("Username DB đã tồn tại.");
            string sql = "INSERT INTO hospital.patient (patient_id, full_name, gender, birthdate, id_card, house_no, street, district, city_province, medical_history, family_medical_history, drug_allergies, username_db) VALUES (:patient_id, :full_name, :gender, :birthdate, :id_card, :house_no, :street, :district, :city_province, :medical_history, :family_medical_history, :drug_allergies, :username_db)";
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            using (OracleCommand cmd = new OracleCommand(sql, conn))
            {
                cmd.Parameters.Add(new OracleParameter("patient_id", OracleDbType.Varchar2) { Value = patientId });
                cmd.Parameters.Add(new OracleParameter("full_name", OracleDbType.NVarchar2) { Value = fullName });
                cmd.Parameters.Add(new OracleParameter("gender", OracleDbType.NVarchar2) { Value = gender });
                cmd.Parameters.Add(new OracleParameter("birthdate", OracleDbType.Date) { Value = birthDate });
                cmd.Parameters.Add(new OracleParameter("id_card", OracleDbType.Varchar2) { Value = idCard });
                cmd.Parameters.Add(new OracleParameter("house_no", OracleDbType.NVarchar2) { Value = houseNo });
                cmd.Parameters.Add(new OracleParameter("street", OracleDbType.NVarchar2) { Value = street });
                cmd.Parameters.Add(new OracleParameter("district", OracleDbType.NVarchar2) { Value = district });
                cmd.Parameters.Add(new OracleParameter("city_province", OracleDbType.NVarchar2) { Value = cityProvince });
                cmd.Parameters.Add(new OracleParameter("medical_history", OracleDbType.NClob) { Value = string.IsNullOrEmpty(medicalHistory) ? DBNull.Value : medicalHistory });
                cmd.Parameters.Add(new OracleParameter("family_medical_history", OracleDbType.NClob) { Value = string.IsNullOrEmpty(familyMedicalHistory) ? DBNull.Value : familyMedicalHistory });
                cmd.Parameters.Add(new OracleParameter("drug_allergies", OracleDbType.NClob) { Value = string.IsNullOrEmpty(drugAllergies) ? DBNull.Value : drugAllergies });
                cmd.Parameters.Add(new OracleParameter("username_db", OracleDbType.Varchar2) { Value = usernameDb });
                conn.Open(); cmd.ExecuteNonQuery();
            }
        }

        public void UpdatePatient(string patientId, string fullName, string gender, DateTime birthDate, string idCard, string houseNo, string street, string district, string cityProvince, string medicalHistory, string familyMedicalHistory, string drugAllergies, string usernameDb)
        {
            if (!PatientIdExists(patientId)) throw new InvalidOperationException("Không tìm thấy bệnh nhân.");
            if (IdCardExists(idCard, patientId)) throw new InvalidOperationException("Số CMND/CCCD đã được dùng.");
            if (UsernameDbExists(usernameDb, patientId)) throw new InvalidOperationException("Username DB đã được dùng.");
            string sql = "UPDATE hospital.patient SET full_name = :full_name, gender = :gender, birthdate = :birthdate, id_card = :id_card, house_no = :house_no, street = :street, district = :district, city_province = :city_province, medical_history = :medical_history, family_medical_history = :family_medical_history, drug_allergies = :drug_allergies, username_db = :username_db WHERE patient_id = :patient_id";
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            using (OracleCommand cmd = new OracleCommand(sql, conn))
            {
                cmd.Parameters.Add(new OracleParameter("full_name", OracleDbType.NVarchar2) { Value = fullName });
                cmd.Parameters.Add(new OracleParameter("gender", OracleDbType.NVarchar2) { Value = gender });
                cmd.Parameters.Add(new OracleParameter("birthdate", OracleDbType.Date) { Value = birthDate });
                cmd.Parameters.Add(new OracleParameter("id_card", OracleDbType.Varchar2) { Value = idCard });
                cmd.Parameters.Add(new OracleParameter("house_no", OracleDbType.NVarchar2) { Value = houseNo });
                cmd.Parameters.Add(new OracleParameter("street", OracleDbType.NVarchar2) { Value = street });
                cmd.Parameters.Add(new OracleParameter("district", OracleDbType.NVarchar2) { Value = district });
                cmd.Parameters.Add(new OracleParameter("city_province", OracleDbType.NVarchar2) { Value = cityProvince });
                cmd.Parameters.Add(new OracleParameter("medical_history", OracleDbType.NClob) { Value = string.IsNullOrEmpty(medicalHistory) ? DBNull.Value : medicalHistory });
                cmd.Parameters.Add(new OracleParameter("family_medical_history", OracleDbType.NClob) { Value = string.IsNullOrEmpty(familyMedicalHistory) ? DBNull.Value : familyMedicalHistory });
                cmd.Parameters.Add(new OracleParameter("drug_allergies", OracleDbType.NClob) { Value = string.IsNullOrEmpty(drugAllergies) ? DBNull.Value : drugAllergies });
                cmd.Parameters.Add(new OracleParameter("username_db", OracleDbType.Varchar2) { Value = usernameDb });
                cmd.Parameters.Add(new OracleParameter("patient_id", OracleDbType.Varchar2) { Value = patientId });
                conn.Open(); cmd.ExecuteNonQuery();
            }
        }

        // --- MedicalRecordRepository methods ---
        public DataTable GetAllMedicalRecords()
        {
            string sql = "SELECT record_id, patient_id, record_date, TO_NCHAR(diagnosis) AS diagnosis, TO_NCHAR(treatment_plan) AS treatment_plan, doctor_id, dept_id, TO_NCHAR(conclusion) AS conclusion FROM hospital.medical_record ORDER BY record_id";
            return _dbProvider.ExecuteQuery(sql);
        }

        public void InsertMedicalRecord(string recordId, string patientId, DateTime recordDate, string doctorId, string deptId)
        {
            string sql = "INSERT INTO hospital.medical_record (record_id, patient_id, record_date, doctor_id, dept_id, diagnosis, treatment_plan, conclusion) VALUES (:record_id, :patient_id, :record_date, :doctor_id, :dept_id, :diagnosis, :treatment_plan, :conclusion)";
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            using (OracleCommand cmd = new OracleCommand(sql, conn))
            {
                cmd.Parameters.Add(new OracleParameter("record_id", OracleDbType.Varchar2) { Value = recordId });
                cmd.Parameters.Add(new OracleParameter("patient_id", OracleDbType.Varchar2) { Value = patientId });
                cmd.Parameters.Add(new OracleParameter("record_date", OracleDbType.Date) { Value = recordDate });
                cmd.Parameters.Add(new OracleParameter("doctor_id", OracleDbType.Varchar2) { Value = doctorId });
                cmd.Parameters.Add(new OracleParameter("dept_id", OracleDbType.Varchar2) { Value = deptId });
                cmd.Parameters.Add(new OracleParameter("diagnosis", OracleDbType.NVarchar2) { Value = "Chưa chẩn đoán" });
                cmd.Parameters.Add(new OracleParameter("treatment_plan", OracleDbType.NVarchar2) { Value = "Chưa điều trị" });
                cmd.Parameters.Add(new OracleParameter("conclusion", OracleDbType.NVarchar2) { Value = "Chưa kết luận" });
                conn.Open(); cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCoordinatorFields(string recordId, string doctorId, string deptId)
        {
            string sql = "UPDATE hospital.medical_record SET doctor_id = :doctor_id, dept_id = :dept_id WHERE record_id = :record_id";
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            using (OracleCommand cmd = new OracleCommand(sql, conn))
            {
                cmd.Parameters.Add(new OracleParameter("doctor_id", OracleDbType.Varchar2) { Value = doctorId });
                cmd.Parameters.Add(new OracleParameter("dept_id", OracleDbType.Varchar2) { Value = deptId });
                cmd.Parameters.Add(new OracleParameter("record_id", OracleDbType.Varchar2) { Value = recordId });
                conn.Open(); cmd.ExecuteNonQuery();
            }
        }

        // --- ServiceAssignmentRepository methods ---
        public DataTable GetAllServiceAssignments()
        {
            string sql = "SELECT record_id AS MAHSBA, service_type AS LOAIDV, service_date AS NGAYDV, technician_id AS MAKTV, service_result AS KETQUA FROM hospital.service_record WHERE technician_id IS NULL ORDER BY MAHSBA, NGAYDV";
            return _dbProvider.ExecuteQuery(sql);
        }

        public void UpdateTechnician(string recordId, string serviceType, DateTime serviceDate, string technicianId)
        {
            string sql = "UPDATE hospital.service_record SET technician_id = :technician_id WHERE record_id = :record_id AND service_type = :service_type AND service_date = :service_date";
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
            using (OracleCommand cmd = new OracleCommand(sql, conn))
            {
                cmd.Parameters.Add(new OracleParameter("technician_id", OracleDbType.Varchar2) { Value = technicianId });
                cmd.Parameters.Add(new OracleParameter("record_id", OracleDbType.Varchar2) { Value = recordId });
                cmd.Parameters.Add(new OracleParameter("service_type", OracleDbType.NVarchar2) { Value = serviceType });
                cmd.Parameters.Add(new OracleParameter("service_date", OracleDbType.Date) { Value = serviceDate });
                conn.Open(); cmd.ExecuteNonQuery();
            }
        }
    }
}
