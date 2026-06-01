using QuanLyYTe.Common;
using QuanLyYTe.DataProvider;
using QuanLyYTe.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyYTe.Services
{
    public class AuthService
    {
        private readonly AuthRepository _authRepo = new AuthRepository();

        public bool Login(string username, string password, string dataSource)
        {
            try
            {
                string connStr = $"User Id={username}; Password={password}; Data Source={dataSource};";

                OracleConnectionFactory.SetConnectionString(connStr);

                using (var conn = new Oracle.ManagedDataAccess.Client.OracleConnection(connStr))
                {
                    conn.Open();
                }
                
                AppSession.CurrentUsername = username;
                string role = GetSessionRole();
                AppSession.CurrentUserRole = role;

                if (role == null)
                    throw new Exception("Tài khoản này chưa được gán vai trò (Role) nào trong hệ thống.");

                if (role != "RL_DBA")
                {
                    AppSession.CurrentUserId = _authRepo.GetUserId(username, role);
                    if (AppSession.CurrentUserId == null)
                        throw new Exception("Không tìm thấy dữ liệu người dùng liên kết với tài khoản này.");
                }

                return true;
            }
            catch (Exception ex)
            {
                OracleConnectionFactory.SetConnectionString(null);
                AppSession.Clear();
                throw new Exception(OracleErrorMapper.GetUserMessage(ex));
            }
        }

        public string GetSessionRole() => _authRepo.GetSessionRole();
        public string GetGrantedRole(string username) => _authRepo.GetGrantedRole(username);

        public void Logout()
        {
            OracleConnectionFactory.SetConnectionString(null);
            AppSession.Clear();
        }
    }
}
