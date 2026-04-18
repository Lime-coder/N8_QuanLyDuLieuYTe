using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace QuanLyYTe.DAL
{
    public class SecurityAdminRepository
    {
        private readonly string? _spOwner;

        public SecurityAdminRepository()
        {
            _spOwner = ConfigurationManager.AppSettings["ProcedureOwner"];
            if (!string.IsNullOrWhiteSpace(_spOwner))
                _spOwner = _spOwner.Trim().ToUpperInvariant();
        }

        private string Sp(string spName)
        {
            if (string.IsNullOrWhiteSpace(_spOwner)) return spName;
            return $"{_spOwner}.{spName}";
        }

        public DataTable GetAllUsers()
        {
            OracleParameter p_cursor = new OracleParameter
            {
                ParameterName = "p_cursor",
                OracleDbType = OracleDbType.RefCursor,
                Direction = ParameterDirection.Output
            };

            return OracleHelper.ExecuteQuerySP(Sp("USP_GET_ALL_USERS"), new[] { p_cursor });
        }

        public DataTable GetAllRoles()
        {
            OracleParameter p_cursor = new OracleParameter
            {
                ParameterName = "p_cursor",
                OracleDbType = OracleDbType.RefCursor,
                Direction = ParameterDirection.Output
            };

            return OracleHelper.ExecuteQuerySP(Sp("USP_GET_ALL_ROLES"), new[] { p_cursor });
        }

        public void CreateUser(string username, string password)
        {
            OracleParameter[] p =
            {
                new OracleParameter("p_username", OracleDbType.Varchar2, 128) { Value = username },
                new OracleParameter("p_password", OracleDbType.Varchar2, 4000) { Value = password },
            };

            OracleHelper.ExecuteNonQuerySP(Sp("USP_CREATE_USER"), p);
        }

        public void ChangeUserPassword(string username, string newPassword)
        {
            OracleParameter[] p =
            {
                new OracleParameter("p_username", OracleDbType.Varchar2, 128) { Value = username },
                new OracleParameter("p_new_password", OracleDbType.Varchar2, 4000) { Value = newPassword },
            };

            OracleHelper.ExecuteNonQuerySP(Sp("USP_UPDATE_USER_PASSWORD"), p);
        }

        public void LockUser(string username)
        {
            OracleParameter[] p =
            {
                new OracleParameter("p_username", OracleDbType.Varchar2, 128) { Value = username },
            };

            OracleHelper.ExecuteNonQuerySP(Sp("USP_LOCK_USER"), p);
        }

        public void UnlockUser(string username)
        {
            OracleParameter[] p =
            {
                new OracleParameter("p_username", OracleDbType.Varchar2, 128) { Value = username },
            };

            OracleHelper.ExecuteNonQuerySP(Sp("USP_UNLOCK_USER"), p);
        }

        public void DropUser(string username, bool cascade = true)
        {
            OracleParameter[] p =
            {
                new OracleParameter("p_username", OracleDbType.Varchar2, 128) { Value = username },
                new OracleParameter("p_cascade", OracleDbType.Varchar2, 3) { Value = cascade ? "YES" : "NO" },
            };

            OracleHelper.ExecuteNonQuerySP(Sp("USP_DROP_USER"), p);
        }

        public void CreateRole(string roleName, string? password = null)
        {
            OracleParameter[] p =
            {
                new OracleParameter("p_role_name", OracleDbType.Varchar2, 128) { Value = roleName },
                new OracleParameter("p_password", OracleDbType.Varchar2, 4000) { Value = (object?)password ?? DBNull.Value },
            };

            OracleHelper.ExecuteNonQuerySP(Sp("USP_CREATE_ROLE"), p);
        }

        public void ChangeRolePassword(string roleName, string? password)
        {
            OracleParameter[] p =
            {
                new OracleParameter("p_role_name", OracleDbType.Varchar2, 128) { Value = roleName },
                new OracleParameter("p_password", OracleDbType.Varchar2, 4000) { Value = (object?)password ?? DBNull.Value },
            };

            OracleHelper.ExecuteNonQuerySP(Sp("USP_UPDATE_ROLE_PASSWORD"), p);
        }

        public void DropRole(string roleName)
        {
            OracleParameter[] p =
            {
                new OracleParameter("p_role_name", OracleDbType.Varchar2, 128) { Value = roleName },
            };

            OracleHelper.ExecuteNonQuerySP(Sp("USP_DROP_ROLE"), p);
        }
    }
}
