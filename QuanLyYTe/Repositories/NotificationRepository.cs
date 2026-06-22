using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DataProvider;

namespace QuanLyYTe.Repositories
{
    public class NotificationRepository : BaseRepository
    {

        // Lấy danh sách thông báo OLS (SP: USP_GET_NOTIFICATIONS)
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

        // Thêm mới thông báo OLS (SP: USP_ADD_NOTIFICATION)
        public void AddNotification(string description, string location, string labelStr)
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_description", OracleDbType.NVarchar2) { Value = description },
                new OracleParameter("p_location", OracleDbType.NVarchar2) { Value = location },
                new OracleParameter("p_label_str", OracleDbType.Varchar2) { Value = labelStr }
            };

            try 
            {
                _dbProvider.ExecuteNonQuerySP("hospital.USP_ADD_NOTIFICATION", parameters);
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

