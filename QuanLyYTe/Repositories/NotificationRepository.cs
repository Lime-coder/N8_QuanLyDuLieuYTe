using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DataProvider;

namespace QuanLyYTe.Repositories
{
    public class NotificationRepository
    {
        private readonly OracleDbProvider _dbProvider = new OracleDbProvider();

        public DataTable GetNotifications()
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };
            
            try 
            {
                return _dbProvider.ExecuteQuerySP("hospital.USP_GET_NOTIFICATIONS", parameters);
            }
            catch (OracleException ex)
            {
                if (ex.Number == 942 || ex.Number == 4043)
                {
                    throw new Exception("Hệ thống thông báo chưa được cài đặt (OLS). Vui lòng chạy script OLS bằng tài khoản DBA.");
                }
                throw;
            }
        }
    }
}
