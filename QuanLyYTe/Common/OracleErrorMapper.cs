using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace QuanLyYTe.Common
{
    public static class OracleErrorMapper
    {
        public static string GetUserMessage(Exception ex)
        {
            if(ex is OracleException oraEx || ex.InnerException is OracleException innerOraEx)
            {
                var oracleException = (ex as OracleException) ?? (ex.InnerException as OracleException);
                switch (oracleException.Number)
                {
                    case 1017: return "Tên dang nh?p ho?c m?t kh?u không dúng.";
                    case 28000: return "Tài kho?n dã b? khóa.";
                    case 1031: return "B?n không có quy?n th?c hi?n thao tác này.";
                    case 1045: return "Tài kho?n này thi?u quy?n CREATE SESSION d? dang nh?p.";
                    case 1920: return "Tên User ho?c Role dã t?n t?i.";
                    case 1918: return "Tên User không t?n t?i.";
                    case 1919: return "Tên Role không t?n t?i.";
                    default: return $"L?i Oracle ({oracleException.Number}): {oracleException.Message}";
                }
            }
            return $"L?i h? th?ng: {ex.Message}";
        }
    }
}

