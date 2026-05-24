using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyYTe.DataProvider
{
    public class OracleConnectionFactory
    {
        private static string _connectionString;

        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static string GetConnectionString()
        {
            if(string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Chưa đăng nhập. Vui lòng đăng nhập trước.");
            }

            return _connectionString;
        }
    }
}
