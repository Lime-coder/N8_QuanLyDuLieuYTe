using QuanLyYTe.Forms;

namespace QuanLyYTe
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            // Application.Run(new Forms.Dashboard("Hospital_dba"));
            // Application.Run(new Forms.frmGrantPermission());
            // Application.Run(new Forms.frmUserManagement());
            // Application.Run(new Forms.frmRevokePermission());
            Application.Run(new Forms.LoginForm());
        }
    }
}