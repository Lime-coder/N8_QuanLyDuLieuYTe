using System;
using Oracle.ManagedDataAccess.Client;

namespace QuanLyYTe.Common
{
    public static class OracleErrorMapper
    {
        public static string GetUserMessage(Exception ex)
        {
            // Ghi log lỗi thật ra file để dễ debug
            Logger.LogError(ex);

            OracleException oracleException = ex as OracleException ?? ex.InnerException as OracleException;

            if (oracleException != null)
            {
                switch (oracleException.Number)
                {
                    case 1017: return "Tên đăng nhập hoặc mật khẩu không đúng.";
                    case 28000: return "Tài khoản đã bị khóa.";
                    case 1031: return "Bạn không có quyền thực hiện thao tác này.";
                    case 1045: return "Tài khoản này thiếu quyền CREATE SESSION để đăng nhập.";
                    case 1920: return "Tên User hoặc Role đã tồn tại.";
                    case 1918: return "Tên User không tồn tại.";
                    case 1919: return "Tên Role không tồn tại.";
                    default: return $"Lỗi Oracle ({oracleException.Number}): {oracleException.Message}";
                }
            }
            return $"Lỗi hệ thống: {ex.Message}";
        }
    }
}