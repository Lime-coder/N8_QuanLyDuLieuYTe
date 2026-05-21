using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyYTe.DAL;
using QuanLyYTe.Forms;
using QuanLyYTe.Forms.Coordinator;
using QuanLyYTe.Forms.Patient;
using QuanLyYTe.Forms.Technician;


namespace QuanLyYTe.Helper
{
    public static class RoleRouter
    {
        /// <summary>
        /// Reads the session role from Oracle and returns the appropriate main form.
        /// Throws InvalidOperationException if the role is unrecognised.
        /// </summary>
        public static Form Resolve(string username)
        {
            var repo = new AuthRepository();
            string role = repo.GetSessionRole();

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
