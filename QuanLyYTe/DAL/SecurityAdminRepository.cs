//using System;
//using System.Data;
//using System.Text.RegularExpressions;

//namespace QuanLyYTe.DAL
//{
//    public class SecurityAdminRepository
//    {
//        private static readonly Regex OracleIdentifierRegex = new Regex(@"^[A-Za-z][A-Za-z0-9_$#]*$", RegexOptions.Compiled);

//        public DataTable GetUsers()
//        {
//            string sql = @"SELECT USERNAME, ACCOUNT_STATUS, LOCK_DATE, CREATED
//                           FROM DBA_USERS
//                           ORDER BY USERNAME";
//            return OracleHelper.ExecuteQuery(sql);
//        }

//        public DataTable GetRoles()
//        {
//            string sql = @"SELECT ROLE, PASSWORD_REQUIRED, AUTHENTICATION_TYPE, COMMON, ORACLE_MAINTAINED
//                           FROM DBA_ROLES
//                           ORDER BY ROLE";
//            return OracleHelper.ExecuteQuery(sql);
//        }

//        public void CreateUser(string username, string password)
//        {
//            string safeUser = NormalizeIdentifier(username);
//            string safePassword = EscapePassword(password);
//            string sql = $"CREATE USER {safeUser} IDENTIFIED BY \"{safePassword}\"";
//            OracleHelper.ExecuteNonQuery(sql);
//        }

//        public void AlterUserPassword(string username, string newPassword)
//        {
//            string safeUser = NormalizeIdentifier(username);
//            string safePassword = EscapePassword(newPassword);
//            string sql = $"ALTER USER {safeUser} IDENTIFIED BY \"{safePassword}\"";
//            OracleHelper.ExecuteNonQuery(sql);
//        }

//        public void DropUser(string username, bool cascade = true)
//        {
//            string safeUser = NormalizeIdentifier(username);
//            string sql = cascade ? $"DROP USER {safeUser} CASCADE" : $"DROP USER {safeUser}";
//            OracleHelper.ExecuteNonQuery(sql);
//        }

//        public void CreateRole(string roleName, string? password)
//        {
//            string safeRole = NormalizeIdentifier(roleName);
//            string sql = string.IsNullOrWhiteSpace(password)
//                ? $"CREATE ROLE {safeRole}"
//                : $"CREATE ROLE {safeRole} IDENTIFIED BY \"{EscapePassword(password)}\"";

//            OracleHelper.ExecuteNonQuery(sql);
//        }

//        public void AlterRolePassword(string roleName, string? password)
//        {
//            string safeRole = NormalizeIdentifier(roleName);
//            string sql = string.IsNullOrWhiteSpace(password)
//                ? $"ALTER ROLE {safeRole} NOT IDENTIFIED"
//                : $"ALTER ROLE {safeRole} IDENTIFIED BY \"{EscapePassword(password)}\"";

//            OracleHelper.ExecuteNonQuery(sql);
//        }

//        public void DropRole(string roleName)
//        {
//            string safeRole = NormalizeIdentifier(roleName);
//            string sql = $"DROP ROLE {safeRole}";
//            OracleHelper.ExecuteNonQuery(sql);
//        }

//        private static string NormalizeIdentifier(string identifier)
//        {
//            if (string.IsNullOrWhiteSpace(identifier))
//            {
//                throw new ArgumentException("Tên user/role không được để trống.");
//            }

//            string trimmed = identifier.Trim();
//            if (!OracleIdentifierRegex.IsMatch(trimmed))
//            {
//                throw new ArgumentException("Tên user/role không hợp lệ. Chỉ cho phép chữ cái, số và các ký tự _, $, #; ký tự đầu phải là chữ cái.");
//            }

//            return trimmed.ToUpperInvariant();
//        }

//        private static string EscapePassword(string password)
//        {
//            if (string.IsNullOrWhiteSpace(password))
//            {
//                throw new ArgumentException("Mật khẩu không được để trống.");
//            }

//            return password.Replace("\"", "\"\"");
//        }
//    }
//}

using System;
using System.Data;
using System.Text.RegularExpressions;

namespace QuanLyYTe.DAL
{
    public class SecurityAdminRepository
    {
        private static readonly Regex OracleIdentifierRegex =
            new Regex(@"^[A-Za-z][A-Za-z0-9_$#]*$", RegexOptions.Compiled);

        // ── Users ────────────────────────────────────────────────────
        public DataTable GetUsers()
        {
            string sql = @"SELECT USERNAME, ACCOUNT_STATUS, LOCK_DATE, CREATED
                           FROM DBA_USERS
                           ORDER BY USERNAME";
            return OracleHelper.ExecuteQuery(sql);
        }

        public void CreateUser(string username, string password)
        {
            string safeUser = NormalizeIdentifier(username);
            string safePass = EscapePassword(password);
            OracleHelper.ExecuteNonQuery($"CREATE USER {safeUser} IDENTIFIED BY \"{safePass}\"");
        }

        /// <summary>Đổi mật khẩu, khoá/mở khoá tài khoản (tuỳ chọn).</summary>
        public void AlterUser(string username, string? newPassword, LockAction lockAction)
        {
            string safeUser = NormalizeIdentifier(username);

            // Đổi mật khẩu
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                string safePass = EscapePassword(newPassword);
                OracleHelper.ExecuteNonQuery($"ALTER USER {safeUser} IDENTIFIED BY \"{safePass}\"");
            }

            // Khoá / mở khoá
            if (lockAction == LockAction.Lock)
                OracleHelper.ExecuteNonQuery($"ALTER USER {safeUser} ACCOUNT LOCK");
            else if (lockAction == LockAction.Unlock)
                OracleHelper.ExecuteNonQuery($"ALTER USER {safeUser} ACCOUNT UNLOCK");
        }

        // Giữ lại để tương thích nếu cần
        public void AlterUserPassword(string username, string newPassword) =>
            AlterUser(username, newPassword, LockAction.None);

        public void DropUser(string username, bool cascade = true)
        {
            string safeUser = NormalizeIdentifier(username);
            string sql = cascade ? $"DROP USER {safeUser} CASCADE" : $"DROP USER {safeUser}";
            OracleHelper.ExecuteNonQuery(sql);
        }

        // ── Roles ────────────────────────────────────────────────────
        public DataTable GetRoles()
        {
            string sql = @"SELECT ROLE, PASSWORD_REQUIRED, AUTHENTICATION_TYPE,
                                  COMMON, ORACLE_MAINTAINED
                           FROM DBA_ROLES
                           ORDER BY ROLE";
            return OracleHelper.ExecuteQuery(sql);
        }

        public void CreateRole(string roleName, string? password)
        {
            string safeRole = NormalizeIdentifier(roleName);
            string sql = string.IsNullOrWhiteSpace(password)
                ? $"CREATE ROLE {safeRole}"
                : $"CREATE ROLE {safeRole} IDENTIFIED BY \"{EscapePassword(password)}\"";
            OracleHelper.ExecuteNonQuery(sql);
        }

        /// <summary>Cập nhật mật khẩu role (null/empty = bỏ xác thực).</summary>
        public void AlterRolePassword(string roleName, string? password)
        {
            string safeRole = NormalizeIdentifier(roleName);
            string sql = string.IsNullOrWhiteSpace(password)
                ? $"ALTER ROLE {safeRole} NOT IDENTIFIED"
                : $"ALTER ROLE {safeRole} IDENTIFIED BY \"{EscapePassword(password)}\"";
            OracleHelper.ExecuteNonQuery(sql);
        }

        public void DropRole(string roleName)
        {
            string safeRole = NormalizeIdentifier(roleName);
            OracleHelper.ExecuteNonQuery($"DROP ROLE {safeRole}");
        }

        // ── Private helpers ──────────────────────────────────────────
        private static string NormalizeIdentifier(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Tên user/role không được để trống.");

            string t = id.Trim();
            if (!OracleIdentifierRegex.IsMatch(t))
                throw new ArgumentException(
                    "Tên không hợp lệ. Chỉ cho phép chữ cái, số, _, $, #; ký tự đầu phải là chữ cái.");

            return t.ToUpperInvariant();
        }

        private static string EscapePassword(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
                throw new ArgumentException("Mật khẩu không được để trống.");
            return pw.Replace("\"", "\"\"");
        }
    }

    public enum LockAction { None, Lock, Unlock }
}