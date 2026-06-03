using System.Data;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DataProvider;

namespace QuanLyYTe.Repositories
{
    public class DoctorRepository
    {
        private readonly OracleDbProvider _dbProvider = new OracleDbProvider();

        public DataTable GetMedicalRecordList(string s = "") => _dbProvider.ExecuteQuerySP("hospital.USP_GET_MEDICAL_RECORD", new[] { new OracleParameter("p_s", s), new OracleParameter("p_c", OracleDbType.RefCursor, ParameterDirection.Output) });
        public void AddMedicalRecord(string id, string pat, string dg, string tr, string cl) => _dbProvider.ExecuteNonQuerySP("hospital.USP_ADD_MEDICAL_RECORD", new[] { new OracleParameter("p_id", id), new OracleParameter("p_pat", pat), new OracleParameter("p_dg", dg), new OracleParameter("p_tr", tr), new OracleParameter("p_cl", cl) });
        public void UpdateMedicalRecord(string id, string dg, string tr, string cl) => _dbProvider.ExecuteNonQuerySP("hospital.USP_UPDATE_MEDICAL_RECORD", new[] { new OracleParameter("p_id", id), new OracleParameter("p_dg", dg), new OracleParameter("p_tr", tr), new OracleParameter("p_cl", cl) });

        public DataTable GetServices(string search = "")
        {
            OracleParameter[] p = {
                new OracleParameter("p_search", search),
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
            };
            return _dbProvider.ExecuteQuerySP("hospital.USP_GET_SERVICES", p);
        }

        public void AddService(string id, string type, string res)
        {
            OracleParameter[] p = {
                new OracleParameter("p_id", id),
                new OracleParameter("p_type", type),
                new OracleParameter("p_res", res)
            };
            _dbProvider.ExecuteNonQuerySP("hospital.USP_ADD_SERVICE", p);
        }

        public void DeleteService(string id, string type, System.DateTime date)
        {
            OracleParameter[] p = {
                new OracleParameter("p_id", id),
                new OracleParameter("p_type", type),
                new OracleParameter("p_date", OracleDbType.Date) { Value = date }
            };
            _dbProvider.ExecuteNonQuerySP("hospital.USP_DELETE_SERVICE", p);
        }

        public DataTable GetPrescriptions(string search = "")
        {
            OracleParameter[] p = {
                new OracleParameter("p_search", search),
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
            };
            return _dbProvider.ExecuteQuerySP("hospital.USP_GET_PRESCRIPTION", p);
        }

        public void ManagePrescription(string act, string id, string med, string dos, DateTime? dt = null, string oldMed = null)
        {
            OracleParameter[] p = {
                new OracleParameter("p_action", act), new OracleParameter("p_record_id", id),
                new OracleParameter("p_med_name", med), new OracleParameter("p_dosage", dos),
                new OracleParameter("p_date", OracleDbType.Date) { Value = (object)dt ?? DBNull.Value },
                new OracleParameter("p_old_med_name", oldMed ?? (object)DBNull.Value) // THÊM DÒNG NÀY
            };
            _dbProvider.ExecuteNonQuerySP("hospital.USP_MANAGE_PRESCRIPTION", p);
        }

        public DataTable GetPatients(string search = "")
        {
            OracleParameter[] p = { new OracleParameter("p_search", search), new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output) };
            return _dbProvider.ExecuteQuerySP("hospital.USP_GET_PATIENTS", p);
        }

        public void UpdatePatient(string id, string hist, string fam, string allergy)
        {
            OracleParameter[] p = {
                new OracleParameter("p_id", id), new OracleParameter("p_history", hist),
                new OracleParameter("p_family_history", fam), new OracleParameter("p_allergy", allergy)
            };
            _dbProvider.ExecuteNonQuerySP("hospital.USP_UPDATE_PATIENT", p);
        }
    }
}