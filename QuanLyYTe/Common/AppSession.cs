using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyYTe.Common
{
    public static class AppSession
    {
        public static string CurrentUsername { get; set; }
        public static void Clear()
        {
            CurrentUsername = null;
        }
    }
}
