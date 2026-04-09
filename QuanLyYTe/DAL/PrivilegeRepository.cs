using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DAL;
using System.Data;

public class PrivilegeRepository
{
    public DataTable GetAllPrivileges(string user)
    {
        OracleParameter[] parameters = new OracleParameter[]
        {
            new OracleParameter("p_grantee", user),
            new OracleParameter("p_cursor", OracleDbType.RefCursor)
            {
                Direction = ParameterDirection.Output
            }
        };

        return OracleHelper.ExecuteQuerySP("USP_GET_ALL_PRIVS", parameters);
    }

    public DataTable GetUsers()
    {
        OracleParameter[] parameters = new OracleParameter[]
        {
        new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
        };

        return OracleHelper.ExecuteQuerySP("USP_GET_USERS", parameters);
    }

    public void RevokePrivilege(string type, string privilege, string owner, string obj, string column, string user)
    {
        OracleParameter[] parameters = new OracleParameter[]
        {
        new OracleParameter("p_type",      type),
        new OracleParameter("p_privilege", privilege),
        new OracleParameter("p_owner",     (object)owner  ?? DBNull.Value),
        new OracleParameter("p_object",    (object)obj    ?? DBNull.Value),
        new OracleParameter("p_column",    (object)column ?? DBNull.Value),
        new OracleParameter("p_grantee",   user)
        };

        OracleHelper.ExecuteNonQuerySP("USP_REVOKE_PRIV", parameters);
    }
}