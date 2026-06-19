using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DataProvider;
using System.Configuration;
using System.Data;

namespace QuanLyYTe.Repositories
{
    // Repository xử lý các nghiệp vụ liên quan đến quản lý quyền (Privilege).
    public class PrivilegeRepository : BaseRepository
    {
        // Lấy danh sách User trong database
        // Sử dụng SP: USP_GET_USERS
        public DataTable GetUsers()
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_USERS"), parameters);
        }
        // Lấy danh sách Role trong database
        // Sử dụng SP: USP_GET_ROLES
        public DataTable GetRoles()
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_ROLES"), parameters);
        }
        // Xem toàn bộ quyền của một User hoặc Role
        // Sử dụng SP: USP_GET_ALL_PRIVS
        public DataTable GetAllPrivileges(string grantee)
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_grantee", grantee),
                new OracleParameter("p_cursor",  OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                }
            };

            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_ALL_PRIVS"), parameters);
        }
        // Xem quyền trên một đối tượng cụ thể (ai có quyền gì trên bảng/view/...)
        // Sử dụng SP: USP_GET_PRIVS_ON_OBJ
        public DataTable GetPrivsOnObject(string owner, string objectName)
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_owner",       owner),
                new OracleParameter("p_object_name", objectName),
                new OracleParameter("p_cursor",      OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                }
            };

            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_PRIVS_ON_OBJ"), parameters);
        }
        // Thu hồi quyền từ User hoặc Role
        // Sử dụng SP: USP_REVOKE_PRIV
        public void RevokePrivilege(string type, string privilege, string owner, string obj, string column, string grantee)
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_type",      type),
                new OracleParameter("p_privilege", privilege),
                new OracleParameter("p_owner",     (object)owner  ?? DBNull.Value),
                new OracleParameter("p_object",    (object)obj    ?? DBNull.Value),
                new OracleParameter("p_column",    (object)column ?? DBNull.Value),
                new OracleParameter("p_grantee",   grantee)
            };

            _dbProvider.ExecuteNonQuerySP(Sp("USP_REVOKE_PRIV"), parameters);
        }
        // Lấy danh sách các đối tượng nghiệp vụ (Bỏ qua View phân quyền)
        // Sử dụng SP: USP_GET_BUSINESS_OBJECTS
        public DataTable GetBusinessObjects()
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_BUSINESS_OBJECTS"), parameters);
        }
        // Lấy danh sách Người dùng hoặc Vai trò (User/Role)
        // Sử dụng SP: USP_GET_GRANTEES
        public DataTable GetGrantees(string granteeType)
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_grantee_type", OracleDbType.Varchar2) { Value = granteeType },
                new OracleParameter("p_result_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };

            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_GRANTEES"), parameters);
        }
        // Lấy danh sách các đối tượng (TABLE, VIEW, PROCEDURE, FUNCTION)
        // Sử dụng SP: USP_GET_OBJECTS
        public DataTable GetObjects(string objectType)
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_object_type", OracleDbType.Varchar2) { Value = objectType },
                new OracleParameter("p_result_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };

            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_OBJECTS"), parameters);
        }
        // Lấy danh sách cột cho một bảng hoặc view cụ thể
        // Sử dụng SP: USP_GET_COLUMNS
        public DataTable GetColumns(string tableName)
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_table_name", OracleDbType.Varchar2) { Value = tableName },
                new OracleParameter("p_result_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };

            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_COLUMNS"), parameters);
        }
        // Lấy danh sách các quyền hệ thống có sẵn
        // Sử dụng SP: USP_GET_SYSTEM_PRIVILEGES
        public DataTable GetAllSystemPrivileges()
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_result_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };
            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_SYSTEM_PRIVILEGES"), parameters);
        }
        // Thực thi cấp quyền trên đối tượng
        // Sử dụng SP: USP_GRANT_OBJECT_PRIVILEGE
        public void GrantObjectPrivilege(string grantee, string priv, string obj, string cols, int withGrant)
        {
            // Xử lý giá trị columns nếu rỗng thì truyền NULL vào DB
            object dbCols = string.IsNullOrEmpty(cols) ? (object)DBNull.Value : cols.Trim();

            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_grantee", OracleDbType.Varchar2) { Value = grantee },
                new OracleParameter("p_privilege", OracleDbType.Varchar2) { Value = priv },
                new OracleParameter("p_object_name", OracleDbType.Varchar2) { Value = obj },
                new OracleParameter("p_column_list", OracleDbType.Varchar2) { Value = dbCols },
                new OracleParameter("p_with_grant", OracleDbType.Int32) { Value = withGrant }
            };

            _dbProvider.ExecuteNonQuerySP(Sp("USP_GRANT_OBJECT_PRIVILEGE"), parameters);
        }
        // Thực thi cấp Role cho User
        // Sử dụng SP: USP_GRANT_ROLE_TO_USER
        public void GrantRoleToUser(string user, string role, int withAdmin)
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_user", OracleDbType.Varchar2) { Value = user },
                new OracleParameter("p_role", OracleDbType.Varchar2) { Value = role },
                new OracleParameter("p_with_admin", OracleDbType.Int32) { Value = withAdmin }
            };
            _dbProvider.ExecuteNonQuerySP(Sp("USP_GRANT_ROLE_TO_USER"), parameters);
        }
        // Thực thi cấp quyền hệ thống cho User hoặc Role
        // Sử dụng SP: USP_GRANT_SYSTEM_PRIVILEGE
        public void GrantSystemPrivilege(string grantee, string priv, int withAdmin)
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                    new OracleParameter("p_grantee", OracleDbType.Varchar2) { Value = grantee },
                    new OracleParameter("p_privilege", OracleDbType.Varchar2) { Value = priv },
                    new OracleParameter("p_with_admin", OracleDbType.Int32) { Value = withAdmin }
            };
            _dbProvider.ExecuteNonQuerySP(Sp("USP_GRANT_SYSTEM_PRIVILEGE"), parameters);
        }
    }
}