using System;
using QuanLyYTe.Common;
using QuanLyYTe.Forms;
using QuanLyYTe.Forms.Coordinator;
using QuanLyYTe.Forms.Patient;
using QuanLyYTe.Forms.Technician;


namespace QuanLyYTe.Helpers
{
    public static class RoleRouter
    {
        public static Form Resolve(string username)
        {
            string role = AppSession.CurrentUserRole;

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
