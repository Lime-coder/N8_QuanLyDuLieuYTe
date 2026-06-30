using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DataProvider;

namespace QuanLyYTe.Repositories
{
    public class PatientRepository : BaseRepository
    {
        public PatientRepository()
        {
            _spOwner = "hospital_dba";
        }

        // Lấy thông tin cá nhân bệnh nhân đang đăng nhập
        // Sử dụng SP: USP_GET_PATIENT_PROFILE
        public DataTable GetProfile()
        {
            OracleParameter[] p =
            {
                new OracleParameter("p_cursor", OracleDbType.RefCursor)
                    { Direction = ParameterDirection.Output }
            };
            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_PATIENT_PROFILE"), p);
        }
        // Lấy danh sách hồ sơ bệnh án của bệnh nhân đang đăng nhập
        // Sử dụng SP: USP_GET_PATIENT_RECORDS
        public DataTable GetMedicalRecords()
        {
            OracleParameter[] p =
            {
                new OracleParameter("p_cursor", OracleDbType.RefCursor)
                    { Direction = ParameterDirection.Output }
            };
            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_PATIENT_RECORDS"), p);
        }
        // Lấy danh sách đơn thuốc theo mã hồ sơ bệnh án
        // Sử dụng SP: USP_GET_PATIENT_PRESCRIPTIONS
        public DataTable GetPrescriptions(string recordId)
        {
            OracleParameter[] p =
            {
                new OracleParameter("p_record_id", OracleDbType.Varchar2) { Value = recordId },
                new OracleParameter("p_cursor", OracleDbType.RefCursor)
                    { Direction = ParameterDirection.Output }
            };
            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_PATIENT_PRESCRIPTIONS"), p);
        }
        // Lấy danh sách dịch vụ y tế theo mã hồ sơ bệnh án
        // Sử dụng SP: USP_GET_PATIENT_SERVICES
        public DataTable GetServices(string recordId)
        {
            OracleParameter[] p =
            {
                new OracleParameter("p_record_id", OracleDbType.Varchar2) { Value = recordId },
                new OracleParameter("p_cursor", OracleDbType.RefCursor)
                    { Direction = ParameterDirection.Output }
            };
            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_PATIENT_SERVICES"), p);
        }
        // Cập nhật thông tin liên lạc và tiền sử bệnh lý của bệnh nhân đang đăng nhập
        // Sử dụng SP: USP_UPDATE_PATIENT_CONTACT
        public void UpdateContact(string houseNo, string street, string district, string cityProvince, string medHistory, string famHistory, string allergies)
        {
            OracleParameter[] p =
            {
                new OracleParameter("p_house_no",               OracleDbType.NVarchar2) { Value = (object)houseNo      ?? DBNull.Value },
                new OracleParameter("p_street",                 OracleDbType.NVarchar2) { Value = (object)street        ?? DBNull.Value },
                new OracleParameter("p_district",               OracleDbType.NVarchar2) { Value = (object)district      ?? DBNull.Value },
                new OracleParameter("p_city_province",          OracleDbType.NVarchar2) { Value = (object)cityProvince ?? DBNull.Value },
                new OracleParameter("p_medical_history",        OracleDbType.NVarchar2) { Value = (object)medHistory   ?? DBNull.Value },
                new OracleParameter("p_family_medical_history", OracleDbType.NVarchar2) { Value = (object)famHistory   ?? DBNull.Value },
                new OracleParameter("p_drug_allergies",         OracleDbType.NVarchar2) { Value = (object)allergies    ?? DBNull.Value }
            };
            _dbProvider.ExecuteNonQuerySP(Sp("USP_UPDATE_PATIENT_CONTACT"), p);
        }
    }
}

