using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace QuanLyYTe.DAL
{
    public class StaffRepository
    {
        public DataTable GetAllWithAccountStatus()
        {
            const string sql = @"
SELECT 
    s.staff_id,
    s.full_name,
    s.gender,
    s.birthdate,
    s.id_card,
    s.hometown,
    s.phone,
    s.dept_id,
    s.staff_role,
    s.username_db,
    u.account_status,
    u.created,
    u.lock_date
FROM staff s
LEFT JOIN dba_users u
    ON u.username = UPPER(s.username_db)
ORDER BY s.staff_id";
            return OracleHelper.ExecuteQuery(sql);
        }

        public void CreateStaff(
            string staffId,
            string fullName,
            string gender,
            DateTime birthdate,
            string idCard,
            string? hometown,
            string phone,
            string deptId,
            string staffRole,
            string password)
        {
            if (string.IsNullOrWhiteSpace(staffId)) throw new ArgumentException("staffId rỗng");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("password rỗng");

            string usernameDb = staffId; // theo script: username_db = staff_id

            const string insertSql = @"
INSERT INTO staff(
    staff_id, full_name, gender, birthdate, id_card, hometown, phone, dept_id, staff_role, username_db
) VALUES (
    :staff_id, :full_name, :gender, :birthdate, :id_card, :hometown, :phone, :dept_id, :staff_role, :username_db
)";

            OracleParameter[] p =
            {
                new OracleParameter("staff_id", OracleDbType.Varchar2, 10) { Value = staffId },
                new OracleParameter("full_name", OracleDbType.NVarchar2, 100) { Value = fullName },
                new OracleParameter("gender", OracleDbType.NVarchar2, 5) { Value = gender },
                new OracleParameter("birthdate", OracleDbType.Date) { Value = birthdate },
                new OracleParameter("id_card", OracleDbType.Varchar2, 20) { Value = idCard },
                new OracleParameter("hometown", OracleDbType.NVarchar2, 200) { Value = (object?)hometown ?? DBNull.Value },
                new OracleParameter("phone", OracleDbType.Varchar2, 15) { Value = phone },
                new OracleParameter("dept_id", OracleDbType.Varchar2, 10) { Value = deptId },
                new OracleParameter("staff_role", OracleDbType.NVarchar2, 50) { Value = staffRole },
                new OracleParameter("username_db", OracleDbType.Varchar2, 30) { Value = usernameDb },
            };

            OracleHelper.ExecuteNonQuery(insertSql, p);

            // Create Oracle user + grant role
            var sec = new SecurityAdminRepository();
            sec.CreateUser(usernameDb, password);
            GrantRoleForStaff(usernameDb, staffRole);
        }

        public void UpdateStaff(
            string staffId,
            string fullName,
            string gender,
            DateTime birthdate,
            string idCard,
            string? hometown,
            string phone,
            string deptId,
            string staffRole)
        {
            const string sql = @"
UPDATE staff SET
    full_name = :full_name,
    gender = :gender,
    birthdate = :birthdate,
    id_card = :id_card,
    hometown = :hometown,
    phone = :phone,
    dept_id = :dept_id,
    staff_role = :staff_role
WHERE staff_id = :staff_id";

            OracleParameter[] p =
            {
                new OracleParameter("full_name", OracleDbType.NVarchar2, 100) { Value = fullName },
                new OracleParameter("gender", OracleDbType.NVarchar2, 5) { Value = gender },
                new OracleParameter("birthdate", OracleDbType.Date) { Value = birthdate },
                new OracleParameter("id_card", OracleDbType.Varchar2, 20) { Value = idCard },
                new OracleParameter("hometown", OracleDbType.NVarchar2, 200) { Value = (object?)hometown ?? DBNull.Value },
                new OracleParameter("phone", OracleDbType.Varchar2, 15) { Value = phone },
                new OracleParameter("dept_id", OracleDbType.Varchar2, 10) { Value = deptId },
                new OracleParameter("staff_role", OracleDbType.NVarchar2, 50) { Value = staffRole },
                new OracleParameter("staff_id", OracleDbType.Varchar2, 10) { Value = staffId },
            };

            OracleHelper.ExecuteNonQuery(sql, p);

            // Ensure Oracle role is granted (idempotent-ish: may error if already granted; ignore ORA-01919/ORA-01920?)
            // We do a best effort grant.
            try { GrantRoleForStaff(staffId, staffRole); } catch { }
        }

        public void DeleteStaff(string staffId, bool dropDbUser = true)
        {
            const string sql = "DELETE FROM staff WHERE staff_id = :staff_id";
            OracleParameter[] p = { new OracleParameter("staff_id", OracleDbType.Varchar2, 10) { Value = staffId } };
            OracleHelper.ExecuteNonQuery(sql, p);

            if (dropDbUser)
            {
                var sec = new SecurityAdminRepository();
                sec.DropUser(staffId, cascade: true);
            }
        }

        private static void GrantRoleForStaff(string usernameDb, string staffRole)
        {
            string role = staffRole switch
            {
                "Điều phối viên" => "RL_COORDINATOR",
                "Bác sĩ" => "RL_DOCTOR",
                "Kỹ thuật viên" => "RL_TECHNICIAN",
                _ => throw new Exception($"Staff role không hợp lệ: {staffRole}")
            };

            // Role/user names are object identifiers; using bind params isn't supported for identifiers.
            // Inputs here come from controlled UI values.
            string sql = $"GRANT {role} TO {usernameDb}";
            OracleHelper.ExecuteNonQuery(sql);
        }

        // ExecuteNonQuerySP test
        public DataTable Test()
        {
            // 1. Khai báo tham số Output cho Cursor
            OracleParameter p_cursor = new OracleParameter
            {
                ParameterName = "p_cursor",
                OracleDbType = OracleDbType.RefCursor,
                Direction = ParameterDirection.Output
            };

            // 2. Chỉ cần gọi Helper là xong, không cần try-catch hay using connection ở đây nữa
            return OracleHelper.ExecuteQuerySP("USP_TEST", new[] { p_cursor });
        }

        // ExecuteNonQuerySP test
        public (string deptName, string phone) Test2(string staffId)
        {
            // 1. Định nghĩa các tham số
            OracleParameter[] paramsArr = new OracleParameter[] {
                new OracleParameter("p_staff_id", OracleDbType.Varchar2) { Value = staffId },
                new OracleParameter("p_phone", OracleDbType.Varchar2, 20) { Direction = ParameterDirection.Output },
                new OracleParameter("p_dept_name", OracleDbType.NVarchar2, 100) { Direction = ParameterDirection.Output }
            };

            // 2. Gọi Helper
            OracleHelper.ExecuteNonQuerySP("USP_TEST2", paramsArr);

            // 3. Trích xuất giá trị từ các tham số Output sau khi chạy
            string phone = Convert.ToString(paramsArr[1].Value) ?? string.Empty;
            string deptName = Convert.ToString(paramsArr[2].Value) ?? string.Empty;

            return (deptName, phone);
        }
    }
}