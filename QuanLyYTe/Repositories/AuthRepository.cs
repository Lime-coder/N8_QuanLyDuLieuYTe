using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DataProvider;

namespace QuanLyYTe.Repositories
{
    public class AuthRepository : BaseRepository
    {

        // Lấy Role đang active (Session Role) của người dùng hiện tại
        // Sử dụng SP: USP_GET_SESSION_ROLE
        public string GetSessionRole()
        {
            OracleParameter[] p =
            {
                new OracleParameter("p_cursor", OracleDbType.RefCursor)
                    { Direction = ParameterDirection.Output }
            };
            DataTable dt = _dbProvider.ExecuteQuerySP(Sp("USP_GET_SESSION_ROLE"), p);
            return dt.Rows.Count > 0 ? dt.Rows[0]["ROLE"].ToString() : null;
        }
        // Lấy Role được cấp (Granted Role) của một user cụ thể
        // Sử dụng SP: USP_GET_GRANTED_ROLE
        public string GetGrantedRole(string username)
        {
            OracleParameter[] p =
            {
                new OracleParameter("p_user", OracleDbType.Varchar2, 128)
                    { Value = username.ToUpper() },
                new OracleParameter("p_cursor", OracleDbType.RefCursor)
                    { Direction = ParameterDirection.Output }
            };
            DataTable dt = _dbProvider.ExecuteQuerySP(Sp("USP_GET_GRANTED_ROLE"), p);
            return dt.Rows.Count > 0 ? dt.Rows[0]["GRANTED_ROLE"].ToString() : null;
        }
        // Lấy ID của người dùng (staff_id hoặc patient_id) dựa trên role và username
        public string GetUserId(string username, string role)
        {
            OracleParameter[] p =
            {
                new OracleParameter("p_username", OracleDbType.Varchar2) { Value = username.ToUpper() },
                new OracleParameter("p_role", OracleDbType.Varchar2) { Value = role.ToUpper() },
                new OracleParameter("p_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };
            DataTable dt = _dbProvider.ExecuteQuerySP(Sp("USP_GET_USER_ID"), p);
            return dt.Rows.Count > 0 ? dt.Rows[0]["ID"].ToString() : null;
        }
    }
}
