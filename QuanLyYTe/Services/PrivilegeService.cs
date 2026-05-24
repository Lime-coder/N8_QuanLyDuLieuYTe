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
    public class PrivilegeService
    {
        private readonly PrivilegeRepository _service = new PrivilegeRepository();

        public DataTable GetUsers() => _service.GetUsers();
        public DataTable GetRoles() => _service.GetRoles();
        public DataTable GetAllPrivileges(string grantee) => _service.GetAllPrivileges(grantee);
        public DataTable GetPrivsOnObject(string owner, string objName) => _service.GetPrivsOnObject(owner, objName);
        public DataTable GetBusinessObjects() => _service.GetBusinessObjects();
        public DataTable GetGrantees(string type) => _service.GetGrantees(type);
        public DataTable GetObjects(string type) => _service.GetObjects(type);
        public DataTable GetColumns(string table) => _service.GetColumns(table);
        public DataTable GetAllSystemPrivileges() => _service.GetAllSystemPrivileges();
        public void RevokePrivilege(string type, string privilege, string owner, string obj, string column, string grantee)
        {
            try { _service.RevokePrivilege(type, privilege, owner, obj, column, grantee); }
            catch (Exception ex) { throw new Exception(OracleErrorMapper.GetUserMessage(ex)); }
        }
        public void GrantObjectPrivilege(string grantee, string priv, string obj, string cols, int withGrant)
        {
            try { _service.GrantObjectPrivilege(grantee, priv, obj, cols, withGrant); }
            catch (Exception ex) { throw new Exception(OracleErrorMapper.GetUserMessage(ex)); }
        }
        public void GrantRoleToUser(string user, string role, int withAdmin)
        {
            try { _service.GrantRoleToUser(user, role, withAdmin); }
            catch (Exception ex) { throw new Exception(OracleErrorMapper.GetUserMessage(ex)); }
        }
        public void GrantSystemPrivilege(string grantee, string priv, int withAdmin)
        {
            try { _service.GrantSystemPrivilege(grantee, priv, withAdmin); }
            catch (Exception ex) { throw new Exception(OracleErrorMapper.GetUserMessage(ex)); }
        }
    }
}
