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
    }
}