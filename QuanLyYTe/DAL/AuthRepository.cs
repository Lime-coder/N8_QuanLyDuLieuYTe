using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace QuanLyYTe.DAL
{
    public class AuthRepository
    {
        // Try to get the first role granted to a user via DBA_ROLE_PRIVS.
        // Requires DBA privilege — throws if the connected user is not DBA.
        public string GetGrantedRole(string username)
        {
            OracleParameter[] p =
            {
                new OracleParameter("p_user", OracleDbType.Varchar2) { Value = username.ToUpper() }
            };

            string sql = @"SELECT GRANTED_ROLE 
                           FROM DBA_ROLE_PRIVS 
                           WHERE GRANTEE = :p_user 
                           AND ROWNUM = 1";

            DataTable dt = OracleHelper.ExecuteQuery(sql, p);
            return dt.Rows.Count > 0 ? dt.Rows[0]["GRANTED_ROLE"].ToString() : null;
        }

        // Fallback: every user can query their own active session roles.
        // Used when the connected user lacks privilege to read DBA_ROLE_PRIVS.
        public string GetSessionRole()
        {
            string sql = "SELECT ROLE FROM SESSION_ROLES WHERE ROWNUM = 1";
            DataTable dt = OracleHelper.ExecuteQuery(sql, null);
            return dt.Rows.Count > 0 ? dt.Rows[0]["ROLE"].ToString() : null;
        }
    }
}