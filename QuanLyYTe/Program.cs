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
            //Application.Run(new Forms.Test_ExecuteNonQuerySP());
            //Application.Run(new Forms.Test_ExecuteQuerySP());
            //Application.Run(new Forms.SystemPrivilegeForm());
            //   Application.Run(new Forms.RoleAssignmentForm());
            Application.Run(new Forms.GrantPermissionForm());
        }
    }
}