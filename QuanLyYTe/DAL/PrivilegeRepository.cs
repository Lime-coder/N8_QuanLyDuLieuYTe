using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DAL;

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
        /// Thực thi cấp quyền trên đối tượng
        /// Sử dụng SP: USP_GRANT_OBJECT_PRIV
        /// </summary>
        public void GrantObjectPrivilege(string grantee, string priv, string obj, string cols, int withGrant)
        {
            // Xử lý giá trị columns nếu rỗng thì truyền NULL vào DB
            object dbCols = string.IsNullOrEmpty(cols) ? (object)DBNull.Value : cols;

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
    }
}