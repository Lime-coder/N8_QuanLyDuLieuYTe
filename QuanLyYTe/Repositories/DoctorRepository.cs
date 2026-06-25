using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DataProvider;

namespace QuanLyYTe.Repositories
{
    public class DoctorRepository : BaseRepository
    {
        public DoctorRepository()
        {
            _spOwner = "hospital_dba";
        }

        // Lấy danh sách hồ sơ bệnh án (SP: USP_GET_MEDICAL_RECORD)
        public DataTable GetMedicalRecordList(string s = "")
        {
            OracleParameter[] p = {
                new OracleParameter("p_s", OracleDbType.NVarchar2) { Value = s },
                new OracleParameter("p_c", OracleDbType.RefCursor, ParameterDirection.Output)
            };
            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_MEDICAL_RECORD"), p);
        }

        // Cập nhật hồ sơ bệnh án (SP: USP_UPDATE_MEDICAL_RECORD)
        public void UpdateMedicalRecord(string id, string dg, string tr, string cl)
        {
            OracleParameter[] p = {
                new OracleParameter("p_id", OracleDbType.NVarchar2) { Value = id },
                new OracleParameter("p_dg", OracleDbType.NVarchar2) { Value = dg },
                new OracleParameter("p_tr", OracleDbType.NVarchar2) { Value = tr },
                new OracleParameter("p_cl", OracleDbType.NVarchar2) { Value = cl }
            };
            _dbProvider.ExecuteNonQuerySP(Sp("USP_UPDATE_MEDICAL_RECORD"), p);
        }

        // Lấy danh sách dịch vụ y tế (SP: USP_GET_SERVICES)
        public DataTable GetServices(string s = "")
        {
            OracleParameter[] p = {
                new OracleParameter("p_s", OracleDbType.NVarchar2) { Value = s },
                new OracleParameter("p_c", OracleDbType.RefCursor, ParameterDirection.Output)
            };
            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_SERVICES"), p);
        }

        // Đăng ký dịch vụ y tế (SP: USP_ADD_SERVICE)
        public void AddService(string id, string type)
        {
            OracleParameter[] p = {
                new OracleParameter("p_id", OracleDbType.NVarchar2) { Value = id },
                new OracleParameter("p_type", OracleDbType.NVarchar2) { Value = type }
            };
            _dbProvider.ExecuteNonQuerySP(Sp("USP_ADD_SERVICE"), p);
        }

        // Hủy đăng ký dịch vụ y tế (SP: USP_DELETE_SERVICE)
        public void DeleteService(string id, string type, DateTime date)
        {
            OracleParameter[] p = {
                new OracleParameter("p_id", OracleDbType.NVarchar2) { Value = id },
                new OracleParameter("p_type", OracleDbType.NVarchar2) { Value = type },
                new OracleParameter("p_date", OracleDbType.Date) { Value = date }
            };
            _dbProvider.ExecuteNonQuerySP(Sp("USP_DELETE_SERVICE"), p);
        }

        // Lấy danh sách đơn thuốc (SP: USP_GET_PRESCRIPTION)
        public DataTable GetPrescriptions(string s = "")
        {
            OracleParameter[] p = {
                new OracleParameter("p_s", OracleDbType.NVarchar2) { Value = s },
                new OracleParameter("p_c", OracleDbType.RefCursor, ParameterDirection.Output)
            };
            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_PRESCRIPTION"), p);
        }

        // Quản lý (Thêm/Sửa/Xóa) đơn thuốc (SP: USP_MANAGE_PRESCRIPTION)
        public void ManagePrescription(string act, string id, string med, string dos, DateTime? dt = null, string oldMed = null)
        {
            OracleParameter[] p = {
                new OracleParameter("p_action", OracleDbType.NVarchar2) { Value = act },
                new OracleParameter("p_record_id", OracleDbType.NVarchar2) { Value = id },
                new OracleParameter("p_med_name", OracleDbType.NVarchar2) { Value = med },
                new OracleParameter("p_dosage", OracleDbType.NVarchar2) { Value = dos },
                new OracleParameter("p_date", OracleDbType.Date) { Value = (object)dt ?? DBNull.Value },
                new OracleParameter("p_old_med_name", OracleDbType.NVarchar2) { Value = oldMed ?? (object)DBNull.Value }
            };
            _dbProvider.ExecuteNonQuerySP(Sp("USP_MANAGE_PRESCRIPTION"), p);
        }

        // Lấy danh sách bệnh nhân (SP: USP_GET_PATIENTS)
        public DataTable GetPatients(string s = "")
        {
            OracleParameter[] p = {
                new OracleParameter("p_s", OracleDbType.NVarchar2) { Value = s },
                new OracleParameter("p_c", OracleDbType.RefCursor, ParameterDirection.Output)
            };
            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_PATIENTS"), p);
        }

        // Cập nhật thông tin bệnh nhân (SP: USP_UPDATE_PATIENT)
        public void UpdatePatient(string id, string hist, string fam, string allergy)
        {
            OracleParameter[] p = {
                new OracleParameter("p_id", OracleDbType.NVarchar2) { Value = id },
                new OracleParameter("p_history", OracleDbType.NVarchar2) { Value = hist },
                new OracleParameter("p_family_history", OracleDbType.NVarchar2) { Value = fam },
                new OracleParameter("p_allergy", OracleDbType.NVarchar2) { Value = allergy }
            };
            _dbProvider.ExecuteNonQuerySP(Sp("USP_UPDATE_PATIENT"), p);
        }

        // Lấy thông tin cá nhân của bác sĩ (SP: USP_GET_SELF_INFO)
        public DataTable GetSelf()
        {
            OracleParameter[] p = { new OracleParameter("p_c", OracleDbType.RefCursor, ParameterDirection.Output) };
            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_SELF_INFO"), p);
        }

        // Cập nhật thông tin cá nhân của bác sĩ (SP: USP_UPDATE_SELF_INFO)
        public void UpdateSelf(string home, string phone)
        {
            OracleParameter[] p = {
                new OracleParameter("p_hometown", OracleDbType.NVarchar2) { Value = home },
                new OracleParameter("p_phone", OracleDbType.NVarchar2) { Value = phone }
            };
            _dbProvider.ExecuteNonQuerySP(Sp("USP_UPDATE_SELF_INFO"), p);
        }
    }
}


