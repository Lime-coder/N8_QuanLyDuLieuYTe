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

        private DataTable ExecuteAssignmentSP(string spName, OracleParameter[] parameters = null)
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
                using (OracleCommand queryCmd = new OracleCommand(spName, conn))
                {
                    queryCmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null) queryCmd.Parameters.AddRange(parameters);
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
            var parameters = new OracleParameter[] {
                new OracleParameter("p_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };
            return ExecuteAssignmentSP("hospital.SP_COORD_GET_DOCTORS", parameters);
        }

        public DataTable GetDoctorsByDepartment(string deptId)
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_dept_id", OracleDbType.Varchar2) { Value = deptId },
                new OracleParameter("p_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };
            return ExecuteAssignmentSP("hospital.SP_COORD_GET_DOC_DEPT", parameters);
        }

        public DataTable GetTechniciansForAssignment()
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };
            return ExecuteAssignmentSP("hospital.SP_COORD_GET_TECHS", parameters);
        }

        public DataTable GetDepartments()
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };
            return _dbProvider.ExecuteQuerySP("hospital.SP_COORD_GET_DEPTS", parameters);
        }

        public void UpdateMedicalRecord(string recordId, string doctorId, string deptId)
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_record_id", OracleDbType.Varchar2) { Value = recordId },
                new OracleParameter("p_doctor_id", OracleDbType.Varchar2) { Value = doctorId },
                new OracleParameter("p_dept_id", OracleDbType.Varchar2) { Value = deptId }
            };
            _dbProvider.ExecuteNonQuerySP("hospital.SP_COORD_UPD_MED_REC", parameters);
        }

        public void UpdateServiceRecord(string recordId, string technicianId)
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_record_id", OracleDbType.Varchar2) { Value = recordId },
                new OracleParameter("p_technician_id", OracleDbType.Varchar2) { Value = technicianId }
            };
            _dbProvider.ExecuteNonQuerySP("hospital.SP_COORD_UPD_SRV_REC", parameters);
        }

        public DataTable GetSelfStaffInfo()
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };
            return _dbProvider.ExecuteQuerySP("hospital.SP_COORD_GET_SELF", parameters);
        }

        public void UpdateSelfStaffInfo(string phone, string hometown)
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_phone", OracleDbType.Varchar2) { Value = phone },
                new OracleParameter("p_hometown", OracleDbType.NVarchar2) { Value = hometown }
            };
            _dbProvider.ExecuteNonQuerySP("hospital.SP_COORD_UPD_SELF", parameters);
        }

        // --- PatientRepository methods ---
        public DataTable GetAllPatients()
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };
            return _dbProvider.ExecuteQuerySP("hospital.SP_COORD_GET_PATS", parameters);
        }

        public DataTable SearchPatients(string keyword)
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_keyword", OracleDbType.NVarchar2) { Value = "%" + keyword + "%" },
                new OracleParameter("p_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };
            return _dbProvider.ExecuteQuerySP("hospital.SP_COORD_SEARCH_PATS", parameters);
        }

        private bool PatientIdExists(string patientId)
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_patient_id", OracleDbType.Varchar2) { Value = patientId },
                new OracleParameter("p_count", OracleDbType.Decimal) { Direction = ParameterDirection.Output }
            };
            _dbProvider.ExecuteNonQuerySP("hospital.SP_COORD_CHK_PAT_ID", parameters);
            return Convert.ToInt32(parameters[1].Value.ToString()) > 0;
        }

        private bool IdCardExists(string idCard, string excludePatientId = null)
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_id_card", OracleDbType.Varchar2) { Value = idCard },
                new OracleParameter("p_exclude_id", OracleDbType.Varchar2) { Value = (object)excludePatientId ?? DBNull.Value },
                new OracleParameter("p_count", OracleDbType.Decimal) { Direction = ParameterDirection.Output }
            };
            _dbProvider.ExecuteNonQuerySP("hospital.SP_COORD_CHK_IDCARD", parameters);
            return Convert.ToInt32(parameters[2].Value.ToString()) > 0;
        }

        private bool UsernameDbExists(string usernameDb, string excludePatientId = null)
        {
            if (string.IsNullOrEmpty(usernameDb)) return false;
            var parameters = new OracleParameter[] {
                new OracleParameter("p_username", OracleDbType.Varchar2) { Value = usernameDb },
                new OracleParameter("p_exclude_id", OracleDbType.Varchar2) { Value = (object)excludePatientId ?? DBNull.Value },
                new OracleParameter("p_count", OracleDbType.Decimal) { Direction = ParameterDirection.Output }
            };
            _dbProvider.ExecuteNonQuerySP("hospital.SP_COORD_CHK_USER", parameters);
            return Convert.ToInt32(parameters[2].Value.ToString()) > 0;
        }

        public void InsertPatient(string patientId, string fullName, string gender, DateTime birthDate, string idCard, string houseNo, string street, string district, string cityProvince, string medicalHistory, string familyMedicalHistory, string drugAllergies, string usernameDb)
        {
            if (PatientIdExists(patientId)) throw new InvalidOperationException("Mã bệnh nhân đã tồn tại.");
            if (IdCardExists(idCard)) throw new InvalidOperationException("Số CMND/CCCD đã tồn tại.");
            if (UsernameDbExists(usernameDb)) throw new InvalidOperationException("Username DB đã tồn tại.");
            
            var parameters = new OracleParameter[] {
                new OracleParameter("p_patient_id", OracleDbType.Varchar2) { Value = patientId },
                new OracleParameter("p_full_name", OracleDbType.NVarchar2) { Value = fullName },
                new OracleParameter("p_gender", OracleDbType.NVarchar2) { Value = gender },
                new OracleParameter("p_birthdate", OracleDbType.Date) { Value = birthDate },
                new OracleParameter("p_id_card", OracleDbType.Varchar2) { Value = idCard },
                new OracleParameter("p_house_no", OracleDbType.NVarchar2) { Value = houseNo },
                new OracleParameter("p_street", OracleDbType.NVarchar2) { Value = street },
                new OracleParameter("p_district", OracleDbType.NVarchar2) { Value = district },
                new OracleParameter("p_city_province", OracleDbType.NVarchar2) { Value = cityProvince },
                new OracleParameter("p_medical_history", OracleDbType.NClob) { Value = string.IsNullOrEmpty(medicalHistory) ? DBNull.Value : medicalHistory },
                new OracleParameter("p_family_medical_history", OracleDbType.NClob) { Value = string.IsNullOrEmpty(familyMedicalHistory) ? DBNull.Value : familyMedicalHistory },
                new OracleParameter("p_drug_allergies", OracleDbType.NClob) { Value = string.IsNullOrEmpty(drugAllergies) ? DBNull.Value : drugAllergies },
                new OracleParameter("p_username_db", OracleDbType.Varchar2) { Value = usernameDb }
            };
            _dbProvider.ExecuteNonQuerySP("hospital.SP_COORD_INS_PAT", parameters);
        }

        public void UpdatePatient(string patientId, string fullName, string gender, DateTime birthDate, string idCard, string houseNo, string street, string district, string cityProvince, string medicalHistory, string familyMedicalHistory, string drugAllergies, string usernameDb)
        {
            if (!PatientIdExists(patientId)) throw new InvalidOperationException("Không tìm thấy bệnh nhân.");
            if (IdCardExists(idCard, patientId)) throw new InvalidOperationException("Số CMND/CCCD đã được dùng.");
            if (UsernameDbExists(usernameDb, patientId)) throw new InvalidOperationException("Username DB đã được dùng.");
            
            var parameters = new OracleParameter[] {
                new OracleParameter("p_patient_id", OracleDbType.Varchar2) { Value = patientId },
                new OracleParameter("p_full_name", OracleDbType.NVarchar2) { Value = fullName },
                new OracleParameter("p_gender", OracleDbType.NVarchar2) { Value = gender },
                new OracleParameter("p_birthdate", OracleDbType.Date) { Value = birthDate },
                new OracleParameter("p_id_card", OracleDbType.Varchar2) { Value = idCard },
                new OracleParameter("p_house_no", OracleDbType.NVarchar2) { Value = houseNo },
                new OracleParameter("p_street", OracleDbType.NVarchar2) { Value = street },
                new OracleParameter("p_district", OracleDbType.NVarchar2) { Value = district },
                new OracleParameter("p_city_province", OracleDbType.NVarchar2) { Value = cityProvince },
                new OracleParameter("p_medical_history", OracleDbType.NClob) { Value = string.IsNullOrEmpty(medicalHistory) ? DBNull.Value : medicalHistory },
                new OracleParameter("p_family_medical_history", OracleDbType.NClob) { Value = string.IsNullOrEmpty(familyMedicalHistory) ? DBNull.Value : familyMedicalHistory },
                new OracleParameter("p_drug_allergies", OracleDbType.NClob) { Value = string.IsNullOrEmpty(drugAllergies) ? DBNull.Value : drugAllergies },
                new OracleParameter("p_username_db", OracleDbType.Varchar2) { Value = usernameDb }
            };
            _dbProvider.ExecuteNonQuerySP("hospital.SP_COORD_UPD_PAT", parameters);
        }

        // --- MedicalRecordRepository methods ---
        public DataTable GetAllMedicalRecords()
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };
            return _dbProvider.ExecuteQuerySP("hospital.SP_COORD_GET_ALL_MED", parameters);
        }

        public void InsertMedicalRecord(string recordId, string patientId, DateTime recordDate, string doctorId, string deptId)
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_record_id", OracleDbType.Varchar2) { Value = recordId },
                new OracleParameter("p_patient_id", OracleDbType.Varchar2) { Value = patientId },
                new OracleParameter("p_record_date", OracleDbType.Date) { Value = recordDate },
                new OracleParameter("p_doctor_id", OracleDbType.Varchar2) { Value = doctorId },
                new OracleParameter("p_dept_id", OracleDbType.Varchar2) { Value = deptId }
            };
            _dbProvider.ExecuteNonQuerySP("hospital.SP_COORD_INS_MED", parameters);
        }

        public void UpdateCoordinatorFields(string recordId, string doctorId, string deptId)
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_record_id", OracleDbType.Varchar2) { Value = recordId },
                new OracleParameter("p_doctor_id", OracleDbType.Varchar2) { Value = doctorId },
                new OracleParameter("p_dept_id", OracleDbType.Varchar2) { Value = deptId }
            };
            _dbProvider.ExecuteNonQuerySP("hospital.SP_COORD_UPD_MED_REC", parameters);
        }

        // --- ServiceAssignmentRepository methods ---
        public DataTable GetAllServiceAssignments()
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };
            return _dbProvider.ExecuteQuerySP("hospital.SP_COORD_GET_SRV_ASS", parameters);
        }

        public void UpdateTechnician(string recordId, string serviceType, DateTime serviceDate, string technicianId)
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_record_id", OracleDbType.Varchar2) { Value = recordId },
                new OracleParameter("p_service_type", OracleDbType.NVarchar2) { Value = serviceType },
                new OracleParameter("p_service_date", OracleDbType.Date) { Value = serviceDate },
                new OracleParameter("p_technician_id", OracleDbType.Varchar2) { Value = technicianId }
            };
            _dbProvider.ExecuteNonQuerySP("hospital.SP_COORD_UPD_TECH", parameters);
        }
    }
}
