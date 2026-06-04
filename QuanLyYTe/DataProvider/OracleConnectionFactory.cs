using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyYTe.DataProvider
{
    public class OracleConnectionFactory
    {
        /// <summary>
        /// Stores the active connection string. In a single-user WinForms app this is acceptable.
        /// NOTE: The password is held in plaintext in memory. If multi-user or service deployment
        /// is ever needed, switch to SecureString or Oracle Wallet authentication.
        /// </summary>
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
