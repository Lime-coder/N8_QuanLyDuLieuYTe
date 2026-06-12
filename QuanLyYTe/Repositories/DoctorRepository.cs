using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DataProvider;

namespace QuanLyYTe.Repositories
{
    public class DoctorRepository
    {
        private readonly OracleDbProvider _dbProvider = new OracleDbProvider();
        private string ProcedureOwner = "hospital";

        private string Sp(string name) => $"{ProcedureOwner}.{name}";

        public DataTable GetMedicalRecordList(string s = "")
        {
            OracleParameter[] p = {
                new OracleParameter("p_s", s),
                new OracleParameter("p_c", OracleDbType.RefCursor, ParameterDirection.Output)
            };
            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_MEDICAL_RECORD"), p);
        }

        public void UpdateMedicalRecord(string id, string dg, string tr, string cl)
        {
            OracleParameter[] p = {
                new OracleParameter("p_id", id),
                new OracleParameter("p_dg", dg),
                new OracleParameter("p_tr", tr),
                new OracleParameter("p_cl", cl)
            };
            _dbProvider.ExecuteNonQuerySP(Sp("USP_UPDATE_MEDICAL_RECORD"), p);
        }

        public DataTable GetServices(string s = "")
        {
            OracleParameter[] p = {
                new OracleParameter("p_s", s),
                new OracleParameter("p_c", OracleDbType.RefCursor, ParameterDirection.Output)
            };
            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_SERVICES"), p);
        }

        public void AddService(string id, string type, string res)
        {
            OracleParameter[] p = {
                new OracleParameter("p_id", id),
                new OracleParameter("p_type", type),
                new OracleParameter("p_res", res)
            };
            _dbProvider.ExecuteNonQuerySP(Sp("USP_ADD_SERVICE"), p);
        }

        public void DeleteService(string id, string type, DateTime date)
        {
            OracleParameter[] p = {
                new OracleParameter("p_id", id),
                new OracleParameter("p_type", type),
                new OracleParameter("p_date", OracleDbType.Date) { Value = date }
            };
            _dbProvider.ExecuteNonQuerySP(Sp("USP_DELETE_SERVICE"), p);
        }

        public DataTable GetPrescriptions(string s = "")
        {
            OracleParameter[] p = {
                new OracleParameter("p_s", s),
                new OracleParameter("p_c", OracleDbType.RefCursor, ParameterDirection.Output)
            };
            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_PRESCRIPTION"), p);
        }

        public void ManagePrescription(string act, string id, string med, string dos, DateTime? dt = null, string oldMed = null)
        {
            OracleParameter[] p = {
                new OracleParameter("p_action", act),
                new OracleParameter("p_record_id", id),
                new OracleParameter("p_med_name", med),
                new OracleParameter("p_dosage", dos),
                new OracleParameter("p_date", OracleDbType.Date) { Value = (object)dt ?? DBNull.Value },
                new OracleParameter("p_old_med_name", oldMed ?? (object)DBNull.Value)
            };
            _dbProvider.ExecuteNonQuerySP(Sp("USP_MANAGE_PRESCRIPTION"), p);
        }

        public DataTable GetPatients(string s = "")
        {
            OracleParameter[] p = {
                new OracleParameter("p_s", s),
                new OracleParameter("p_c", OracleDbType.RefCursor, ParameterDirection.Output)
            };
            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_PATIENTS"), p);
        }

        public void UpdatePatient(string id, string hist, string fam, string allergy)
        {
            OracleParameter[] p = {
                new OracleParameter("p_id", id),
                new OracleParameter("p_history", hist),
                new OracleParameter("p_family_history", fam),
                new OracleParameter("p_allergy", allergy)
            };
            _dbProvider.ExecuteNonQuerySP(Sp("USP_UPDATE_PATIENT"), p);
        }

        public DataTable GetSelf()
        {
            OracleParameter[] p = { new OracleParameter("p_c", OracleDbType.RefCursor, ParameterDirection.Output) };
            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_SELF_INFO"), p);
        }

        public void UpdateSelf(string home, string phone)
        {
            OracleParameter[] p = {
                new OracleParameter("p_hometown", home),
                new OracleParameter("p_phone", phone)
            };
            _dbProvider.ExecuteNonQuerySP(Sp("USP_UPDATE_SELF_INFO"), p);
        }
    }
}