using QuanLyYTe.Common;
using QuanLyYTe.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyYTe.Services
{
    public class SecurityAdminService
    {
        private readonly SecurityAdminRepository _service = new SecurityAdminRepository();

        public DataTable GetAllUsers() => _service.GetAllUsers();
        public DataTable GetAllRoles() => _service.GetAllRoles();
        public void LockUser(string username) => _service.LockUser(username);
        public void UnlockUser(string username) => _service.UnlockUser(username);
        public void ChangeUserPassword(string username, string newPass) => _service.ChangeUserPassword(username, newPass);
        public void CreateRole(string roleName, string password = null) => _service.CreateRole(roleName, password);
        public void DropRole(string roleName) => _service.DropRole(roleName);
        public void ChangeRolePassword(string roleName, string password) => _service.ChangeRolePassword(roleName, password);
        public void CreateUser(string username, string password)
        {
            try { _service.CreateUser(username, password); }
            catch (Exception ex) { throw new Exception(OracleErrorMapper.GetUserMessage(ex)); }
        }
        public void DropUser(string username, bool cascade = true)
        {
            try { _service.DropUser(username, cascade); }
            catch (Exception ex) { throw new Exception(OracleErrorMapper.GetUserMessage(ex)); }
        }
    }
}
