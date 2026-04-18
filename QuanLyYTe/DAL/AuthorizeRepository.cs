using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace QuanLyYTe.DAL
{
    public class AuthRepository
    {
        private static string Sp(string name)
        {
            string owner = ConfigurationManager.AppSettings["ProcedureOwner"];
            return string.IsNullOrEmpty(owner) ? name : $"{owner}.{name}";
        }

        // Primary: works for any user, no DBA privilege needed
        public string GetSessionRole()
        {
            OracleParameter[] p =
            {
            new OracleParameter("p_cursor", OracleDbType.RefCursor)
                { Direction = ParameterDirection.Output }
        };
            DataTable dt = OracleHelper.ExecuteQuerySP(Sp("USP_GET_SESSION_ROLE"), p);
            return dt.Rows.Count > 0 ? dt.Rows[0]["ROLE"].ToString() : null;
        }

        // Secondary: only call this if the logged-in user IS a DBA
        // and you need to inspect another user's roles
        public string GetGrantedRole(string username)
        {
            OracleParameter[] p =
            {
            new OracleParameter("p_user", OracleDbType.Varchar2, 128)
                { Value = username.ToUpper() },
            new OracleParameter("p_cursor", OracleDbType.RefCursor)
                { Direction = ParameterDirection.Output }
        };
            DataTable dt = OracleHelper.ExecuteQuerySP(Sp("USP_GET_GRANTED_ROLE"), p);
            return dt.Rows.Count > 0 ? dt.Rows[0]["GRANTED_ROLE"].ToString() : null;
        }
    }
}