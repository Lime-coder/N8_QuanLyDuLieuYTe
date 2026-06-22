using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyYTe.Common
{
    public static class AppSession
    {
        public static string? CurrentUsername { get; set; }
        public static string? CurrentUserId { get; set; }
        public static string? CurrentUserRole { get; set; }
        public static string? CurrentFullName { get; set; }

        public static void Clear()
        {
            CurrentUsername = null;
            CurrentUserId = null;
            CurrentUserRole = null;
            CurrentFullName = null;
        }
    }
}
