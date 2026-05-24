using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DataProvider;
using System.Data;

namespace QuanLyYTe.Repositories
{
    /// <summary>
    /// Repository xử lý các nghiệp vụ liên quan đến quản lý quyền (Privilege).
    /// </summary>
    public class PrivilegeRepository
    {
        private readonly OracleDbProvider _dbProvider = new OracleDbProvider();
        /// <summary>
        /// Lấy danh sách User trong database
        /// Sử dụng SP: USP_GET_USERS
        /// </summary>
        public DataTable GetUsers()
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            return _dbProvider.ExecuteQuerySP("USP_GET_USERS", parameters);
        }

        /// <summary>
        /// Lấy danh sách Role trong database
        /// Sử dụng SP: USP_GET_ROLES
        /// </summary>
        public DataTable GetRoles()
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            return _dbProvider.ExecuteQuerySP("USP_GET_ROLES", parameters);
        }

        /// <summary>
        /// Xem toàn bộ quyền của một User hoặc Role
        /// Sử dụng SP: USP_GET_ALL_PRIVS
        /// </summary>
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

            return _dbProvider.ExecuteQuerySP("USP_GET_ALL_PRIVS", parameters);
        }

        /// <summary>
        /// Xem quyền trên một đối tượng cụ thể (ai có quyền gì trên bảng/view/...)
        /// Sử dụng SP: USP_GET_PRIVS_ON_OBJ
        /// </summary>
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

            return _dbProvider.ExecuteQuerySP("USP_GET_PRIVS_ON_OBJ", parameters);
        }

        /// <summary>
        /// Thu hồi quyền từ User hoặc Role
        /// Sử dụng SP: USP_REVOKE_PRIV
        /// </summary>
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

            _dbProvider.ExecuteNonQuerySP("USP_REVOKE_PRIV", parameters);
        }

        /// <summary>
        /// Lấy danh sách các đối tượng nghiệp vụ (Bỏ qua View phân quyền)
        /// Sử dụng SP: USP_GET_BUSINESS_OBJECTS
        /// </summary>
        public DataTable GetBusinessObjects()
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            return _dbProvider.ExecuteQuerySP("USP_GET_BUSINESS_OBJECTS", parameters);
        }

        /// <summary>
        /// Lấy danh sách Người dùng hoặc Vai trò (User/Role)
        /// Sử dụng SP: USP_GET_GRANTEES
        /// </summary>
        public DataTable GetGrantees(string granteeType)
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_grantee_type", OracleDbType.Varchar2) { Value = granteeType },
                new OracleParameter("p_result_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };

            return _dbProvider.ExecuteQuerySP("USP_GET_GRANTEES", parameters);
        }

        /// <summary>
        /// Lấy danh sách các đối tượng (TABLE, VIEW, PROCEDURE, FUNCTION)
        /// Sử dụng SP: USP_GET_OBJECTS
        /// </summary>
        public DataTable GetObjects(string objectType)
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_object_type", OracleDbType.Varchar2) { Value = objectType },
                new OracleParameter("p_result_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };

            return _dbProvider.ExecuteQuerySP("USP_GET_OBJECTS", parameters);
        }

        /// <summary>
        /// Lấy danh sách cột cho một bảng hoặc view cụ thể
        /// Sử dụng SP: USP_GET_COLUMNS
        /// </summary>
        public DataTable GetColumns(string tableName)
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_table_name", OracleDbType.Varchar2) { Value = tableName },
                new OracleParameter("p_result_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };

            return _dbProvider.ExecuteQuerySP("USP_GET_COLUMNS", parameters);
        }

        /// <summary>
        /// Lấy danh sách các quyền hệ thống có sẵn
        /// Sử dụng SP: USP_GET_SYSTEM_PRIVILEGES
        /// </summary>
        public DataTable GetAllSystemPrivileges()
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_result_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };
            return _dbProvider.ExecuteQuerySP("USP_GET_SYSTEM_PRIVILEGES", parameters);
        }


        /// <summary>
        /// Thực thi cấp quyền trên đối tượng
        /// Sử dụng SP: USP_GRANT_OBJECT_PRIVILEGE
        /// </summary>
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

            _dbProvider.ExecuteNonQuerySP("USP_GRANT_OBJECT_PRIVILEGE", parameters);
        }

        /// <summary>
        /// Thực thi cấp Role cho User
        /// Sử dụng SP: USP_GRANT_ROLE_TO_USER
        /// </summary>
        public void GrantRoleToUser(string user, string role, int withAdmin)
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_user", OracleDbType.Varchar2) { Value = user },
                new OracleParameter("p_role", OracleDbType.Varchar2) { Value = role },
                new OracleParameter("p_with_admin", OracleDbType.Int32) { Value = withAdmin }
            };
            _dbProvider.ExecuteNonQuerySP("USP_GRANT_ROLE_TO_USER", parameters);
        }

        /// <summary>
        /// Thực thi cấp quyền hệ thống cho User hoặc Role
        /// Sử dụng SP: USP_GRANT_SYSTEM_PRIVILEGE
        /// </summary>
        public void GrantSystemPrivilege(string grantee, string priv, int withAdmin)
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                    new OracleParameter("p_grantee", OracleDbType.Varchar2) { Value = grantee },
                    new OracleParameter("p_privilege", OracleDbType.Varchar2) { Value = priv },
                    new OracleParameter("p_with_admin", OracleDbType.Int32) { Value = withAdmin }
            };
            _dbProvider.ExecuteNonQuerySP("USP_GRANT_SYSTEM_PRIVILEGE", parameters);
        }
    }
}