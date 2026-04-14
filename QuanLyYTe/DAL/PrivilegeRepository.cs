using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace QuanLyYTe.DAL
{
    public class PrivilegeRepository
    {
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

            return OracleHelper.ExecuteQuerySP("usp_GetGrantees", parameters);
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

            return OracleHelper.ExecuteQuerySP("usp_GetObjects", parameters);
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

            return OracleHelper.ExecuteQuerySP("usp_GetColumns", parameters);
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
            return OracleHelper.ExecuteQuerySP("usp_GetAllSystemPrivileges", parameters);
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
                new OracleParameter("p_priv_or_role", OracleDbType.Varchar2) { Value = priv },
                new OracleParameter("p_object_name", OracleDbType.Varchar2) { Value = obj },
                new OracleParameter("p_column_list", OracleDbType.Varchar2) { Value = dbCols },
                new OracleParameter("p_with_grant", OracleDbType.Int32) { Value = withGrant }
            };

            OracleHelper.ExecuteNonQuerySP("USP_GRANT_OBJECT_PRIV", parameters);
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