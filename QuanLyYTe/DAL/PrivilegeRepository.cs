using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DAL;
using System.Data;

namespace QuanLyYTe.DAL
{
    /// <summary>
    /// Repository xử lý các nghiệp vụ liên quan đến quản lý quyền (Privilege).
    /// </summary>
    public class PrivilegeRepository
    {
        // ----------------------------------------------------------------
        // 1. Lấy danh sách User trong database
        // ----------------------------------------------------------------
        public DataTable GetUsers()
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            return OracleHelper.ExecuteQuerySP("USP_GET_USERS", parameters);
        }

        // ----------------------------------------------------------------
        // 2. Lấy danh sách Role trong database
        // ----------------------------------------------------------------
        public DataTable GetRoles()
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            return OracleHelper.ExecuteQuerySP("USP_GET_ROLES", parameters);
        }

        // ----------------------------------------------------------------
        // 3. Xem toàn bộ quyền của một User hoặc Role
        //    grantee: tên user hoặc tên role
        // ----------------------------------------------------------------
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

            return OracleHelper.ExecuteQuerySP("USP_GET_ALL_PRIVS", parameters);
        }

        // ----------------------------------------------------------------
        // 4. Xem quyền trên một đối tượng cụ thể (ai có quyền gì trên bảng/view/...)
        //    owner      : schema sở hữu đối tượng (ví dụ: HOSPITAL_DBA)
        //    objectName : tên đối tượng (ví dụ: PATIENT)
        // ----------------------------------------------------------------
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

            return OracleHelper.ExecuteQuerySP("USP_GET_PRIVS_ON_OBJ", parameters);
        }

        // ----------------------------------------------------------------
        // 5. Thu hồi quyền từ User hoặc Role
        //    type      : 'SYSTEM' | 'ROLE' | 'TABLE' | 'VIEW' | 'PROCEDURE' | 'FUNCTION' | 'COLUMN'
        //    privilege : Tên quyền hoặc tên role được cấp
        //    owner     : Schema sở hữu object (NULL nếu SYSTEM/ROLE)
        //    obj       : Tên object            (NULL nếu SYSTEM/ROLE)
        //    column    : Tên cột               (chỉ dùng khi type='COLUMN')
        //    grantee   : User hoặc role cần thu hồi
        // ----------------------------------------------------------------
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

            OracleHelper.ExecuteNonQuerySP("USP_REVOKE_PRIV", parameters);
        }

        /// <summary>
        /// Lấy danh sách Người dùng hoặc Vai trò (User/Role)
        /// Sử dụng SP: usp_GetGrantees
        /// </summary>
        public DataTable GetGrantees(string granteeType)
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_grantee_type", OracleDbType.Varchar2) { Value = granteeType },
                new OracleParameter("p_result_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };

            return OracleHelper.ExecuteQuerySP("USP_GET_GRANTEES", parameters);
        }

        /// <summary>
        /// Lấy danh sách các đối tượng (TABLE, VIEW, PROCEDURE, FUNCTION)
        /// Sử dụng SP: usp_GetObjects
        /// </summary>
        public DataTable GetObjects(string objectType)
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_object_type", OracleDbType.Varchar2) { Value = objectType },
                new OracleParameter("p_result_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };

            return OracleHelper.ExecuteQuerySP("USP_GET_OBJECTS", parameters);
        }

        /// <summary>
        /// Lấy danh sách cột cho một bảng hoặc view cụ thể
        /// Sử dụng SP: usp_GetColumns
        /// </summary>
        public DataTable GetColumns(string tableName)
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_table_name", OracleDbType.Varchar2) { Value = tableName },
                new OracleParameter("p_result_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };

            return OracleHelper.ExecuteQuerySP("USP_GET_COLUMNS", parameters);
        }

        /// <summary>
        /// Lấy danh sách các quyền hệ thống có sẵn
        /// Sử dụng SP: usp_GetAllSystemPrivileges
        /// </summary>
        public DataTable GetAllSystemPrivileges()
        {
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter("p_result_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };
            return OracleHelper.ExecuteQuerySP("USP_GET_SYSTEM_PRIVILEGES", parameters);
        }


        /// <summary>
        /// Thực thi cấp quyền trên đối tượng
        /// Sử dụng SP: USP_GRANT_OBJECT_PRIV
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

            OracleHelper.ExecuteNonQuerySP("USP_GRANT_OBJECT_PRIVILEGE", parameters);
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
            OracleHelper.ExecuteNonQuerySP("USP_GRANT_ROLE_TO_USER", parameters);
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
            OracleHelper.ExecuteNonQuerySP("USP_GRANT_SYSTEM_PRIVILEGE", parameters);
        }
    }
}