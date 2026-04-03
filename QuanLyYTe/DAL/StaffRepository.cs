using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace QuanLyYTe.DAL
{
    public class StaffRepository
    {
        // ExecuteNonQuerySP test
        public DataTable Test()
        {
            // 1. Khai báo tham số Output cho Cursor
            OracleParameter p_cursor = new OracleParameter
            {
                ParameterName = "p_cursor",
                OracleDbType = OracleDbType.RefCursor,
                Direction = ParameterDirection.Output
            };

            // 2. Chỉ cần gọi Helper là xong, không cần try-catch hay using connection ở đây nữa
            return OracleHelper.ExecuteQuerySP("USP_TEST", new[] { p_cursor });
        }

        // ExecuteNonQuerySP test
        public (string deptName, string phone) Test2(string staffId)
        {
            // 1. Định nghĩa các tham số
            OracleParameter[] paramsArr = new OracleParameter[] {
                new OracleParameter("p_staff_id", OracleDbType.Varchar2) { Value = staffId },
                new OracleParameter("p_phone", OracleDbType.Varchar2, 20) { Direction = ParameterDirection.Output },
                new OracleParameter("p_dept_name", OracleDbType.NVarchar2, 100) { Direction = ParameterDirection.Output }
            };

            // 2. Gọi Helper
            OracleHelper.ExecuteNonQuerySP("USP_TEST2", paramsArr);

            // 3. Trích xuất giá trị từ các tham số Output sau khi chạy
            string phone = paramsArr[1].Value.ToString();
            string deptName = paramsArr[2].Value.ToString();

            return (deptName, phone);
        }
    }
}