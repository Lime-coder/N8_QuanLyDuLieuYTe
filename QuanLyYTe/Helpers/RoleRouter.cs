using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyYTe.Services;
using QuanLyYTe.Forms;
using QuanLyYTe.Forms.Coordinator;
using QuanLyYTe.Forms.Patient;
using QuanLyYTe.Forms.Technician;
using QuanLyYTe.Forms.Doctor;


namespace QuanLyYTe.Helpers
{
    public static class RoleRouter
    {
        public static Form Resolve(string username)
        {
            var service = new AuthService();
            string role = service.GetSessionRole();

            return role?.Trim().ToUpperInvariant() switch
            {
                "RL_DBA" => new Dashboard(username),
                "RL_DOCTOR" => new frmDoctor(),
                "RL_COORDINATOR" => new frmCoordinator(),
                "RL_TECHNICIAN" => new frmTechnician(),
                "RL_PATIENT" => new frmPatient(),

                null => throw new InvalidOperationException(
                    "Không xác định được vai trò của tài khoản này."),

                _ => throw new InvalidOperationException(
                    $"Vai trò '{role}' chưa được hỗ trợ.")
            };
        }
    }
}
