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
        public DataTable GetAllDepartments() => _service.GetAllDepartments();
        public DataTable GetUserInfo(string username) => _service.GetUserInfo(username);

        public void CreateUser(string username, string password, string fullName, string gender, DateTime birthdate, string idCard, string role, 
            string? phone = null, string? hometown = null, string? deptId = null, string? facility = null,
            string? houseNo = null, string? street = null, string? district = null, string? cityProvince = null,
            string? medicalHistory = null, string? familyMedicalHistory = null, string? drugAllergies = null)
        {
            try { _service.CreateUser(username, password, fullName, gender, birthdate, idCard, role, phone, hometown, deptId, facility, houseNo, street, district, cityProvince, medicalHistory, familyMedicalHistory, drugAllergies); }
            catch (Exception ex) { throw new Exception(OracleErrorMapper.GetUserMessage(ex)); }
        }

        public void UpdateUser(string username, string fullName, string gender, DateTime birthdate, string idCard, string role, 
            string? phone = null, string? hometown = null, string? deptId = null, string? facility = null,
            string? houseNo = null, string? street = null, string? district = null, string? cityProvince = null,
            string? medicalHistory = null, string? familyMedicalHistory = null, string? drugAllergies = null)
        {
            try { _service.UpdateUser(username, fullName, gender, birthdate, idCard, role, phone, hometown, deptId, facility, houseNo, street, district, cityProvince, medicalHistory, familyMedicalHistory, drugAllergies); }
            catch (Exception ex) { throw new Exception(OracleErrorMapper.GetUserMessage(ex)); }
        }

        public void DeactivateUser(string username)
        {
            try { _service.DeactivateUser(username); }
            catch (Exception ex) { throw new Exception(OracleErrorMapper.GetUserMessage(ex)); }
        }

        public string GetUserOlsLabel(string username)
        {
            try { return _service.GetUserOlsLabel(username); }
            catch (Exception ex) { throw new Exception(OracleErrorMapper.GetUserMessage(ex)); }
        }

        public void SetUserOlsLabel(string username, string label)
        {
            try { _service.SetUserOlsLabel(username, label); }
            catch (Exception ex) { throw new Exception(OracleErrorMapper.GetUserMessage(ex)); }
        }

        // Get 5 standard audit contexts (Requirement 3.2)
        public DataTable GetStandardAudit()
        {
            return _service.GetStandardAuditLogs();
        }

        // Get FGA logs for Prescription updates (Requirement 3.3a)
        public DataTable GetPrescriptionAudit()
        {
            return _service.GetFGAPrescriptionLogs();
        }

        // Get FGA + Unified logs for Medical Record (Requirement 3.3b, 3.3c)
        public DataTable GetMedicalInfoAudit()
        {
            return _service.GetFGAMedicalRecordLogs();
        }

        // Get Unified logs for Service Record illegal actions (Requirement 3.3d)
        public DataTable GetServiceRecordAudit()
        {
            return _service.GetUnifiedServiceRecordLogs();
        }
    }
}
