using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DataProvider;

namespace QuanLyYTe.Repositories
{
public class SecurityAdminRepository
{
    private readonly string? _spOwner;
    private readonly OracleDbProvider _dbProvider = new OracleDbProvider();

    public SecurityAdminRepository()
    {
        _spOwner = ConfigurationManager.AppSettings["ProcedureOwner"];
        if (!string.IsNullOrWhiteSpace(_spOwner))
            _spOwner = _spOwner.Trim().ToUpperInvariant();
    }

    private string Sp(string spName)
    {
        if (string.IsNullOrWhiteSpace(_spOwner)) return spName;
        return $"{_spOwner}.{spName}";
    }

    /// <summary>
    /// Lấy danh sách tất cả các User trong hệ thống
    /// Sử dụng SP: USP_GET_ALL_USERS
    /// </summary>
    public DataTable GetAllUsers()
    {
        OracleParameter p_cursor = new OracleParameter
        {
            ParameterName = "p_cursor",
            OracleDbType = OracleDbType.RefCursor,
            Direction = ParameterDirection.Output
        };

        return _dbProvider.ExecuteQuerySP(Sp("USP_GET_ALL_USERS"), new[] { p_cursor });
    }

    /// <summary>
    /// Lấy danh sách tất cả các Role trong hệ thống
    /// Sử dụng SP: USP_GET_ALL_ROLES
    /// </summary>
    public DataTable GetAllRoles()
    {
        OracleParameter p_cursor = new OracleParameter
        {
            ParameterName = "p_cursor",
            OracleDbType = OracleDbType.RefCursor,
            Direction = ParameterDirection.Output
        };

        return _dbProvider.ExecuteQuerySP(Sp("USP_GET_ALL_ROLES"), new[] { p_cursor });
    }

    /// <summary>
    /// Lấy danh sách phòng ban
    /// </summary>
    public DataTable GetAllDepartments()
    {
        OracleParameter[] p = {
            new OracleParameter("p_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
        };
        return _dbProvider.ExecuteQuerySP(Sp("USP_GET_ALL_DEPARTMENTS"), p);
    }

    /// <summary>
    /// Lấy thông tin chi tiết của user (từ staff hoặc patient)
    /// </summary>
    public DataTable GetUserInfo(string username)
    {
        OracleParameter[] p = {
            new OracleParameter("p_username", OracleDbType.Varchar2) { Value = username },
            new OracleParameter("p_cursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
        };
        return _dbProvider.ExecuteQuerySP(Sp("USP_GET_USER_INFO"), p);
    }

    /// <summary>
    /// Tạo mới một User liên kết với dữ liệu NV/BN
    /// </summary>
    public void CreateUser(string username, string password, string fullName, string gender, DateTime birthdate, string idCard, string role, 
        string? phone = null, string? hometown = null, string? deptId = null,
        string? houseNo = null, string? street = null, string? district = null, string? cityProvince = null,
        string? medicalHistory = null, string? familyMedicalHistory = null, string? drugAllergies = null)
    {
        OracleParameter[] p =
        {
            new OracleParameter("p_username", OracleDbType.Varchar2) { Value = username },
            new OracleParameter("p_password", OracleDbType.Varchar2) { Value = password },
            new OracleParameter("p_full_name", OracleDbType.NVarchar2) { Value = fullName },
            new OracleParameter("p_gender", OracleDbType.NVarchar2) { Value = gender },
            new OracleParameter("p_birthdate", OracleDbType.Date) { Value = birthdate },
            new OracleParameter("p_id_card", OracleDbType.Varchar2) { Value = idCard },
            new OracleParameter("p_role", OracleDbType.Varchar2) { Value = role },
            new OracleParameter("p_phone", OracleDbType.Varchar2) { Value = (object?)phone ?? DBNull.Value },
            new OracleParameter("p_hometown", OracleDbType.NVarchar2) { Value = (object?)hometown ?? DBNull.Value },
            new OracleParameter("p_dept_id", OracleDbType.Varchar2) { Value = (object?)deptId ?? DBNull.Value },
            new OracleParameter("p_house_no", OracleDbType.NVarchar2) { Value = (object?)houseNo ?? DBNull.Value },
            new OracleParameter("p_street", OracleDbType.NVarchar2) { Value = (object?)street ?? DBNull.Value },
            new OracleParameter("p_district", OracleDbType.NVarchar2) { Value = (object?)district ?? DBNull.Value },
            new OracleParameter("p_city_province", OracleDbType.NVarchar2) { Value = (object?)cityProvince ?? DBNull.Value },
            new OracleParameter("p_medical_history", OracleDbType.NClob) { Value = (object?)medicalHistory ?? DBNull.Value },
            new OracleParameter("p_family_medical_history", OracleDbType.NClob) { Value = (object?)familyMedicalHistory ?? DBNull.Value },
            new OracleParameter("p_drug_allergies", OracleDbType.NClob) { Value = (object?)drugAllergies ?? DBNull.Value }
        };

        _dbProvider.ExecuteNonQuerySP(Sp("USP_CREATE_USER_LINKED"), p);
    }

    /// <summary>
    /// Cập nhật thông tin User liên kết
    /// </summary>
    public void UpdateUser(string username, string fullName, string gender, DateTime birthdate, string idCard, string role,
        string? phone = null, string? hometown = null, string? deptId = null,
        string? houseNo = null, string? street = null, string? district = null, string? cityProvince = null,
        string? medicalHistory = null, string? familyMedicalHistory = null, string? drugAllergies = null)
    {
        OracleParameter[] p =
        {
            new OracleParameter("p_username", OracleDbType.Varchar2) { Value = username },
            new OracleParameter("p_full_name", OracleDbType.NVarchar2) { Value = fullName },
            new OracleParameter("p_gender", OracleDbType.NVarchar2) { Value = gender },
            new OracleParameter("p_birthdate", OracleDbType.Date) { Value = birthdate },
            new OracleParameter("p_id_card", OracleDbType.Varchar2) { Value = idCard },
            new OracleParameter("p_role", OracleDbType.Varchar2) { Value = role },
            new OracleParameter("p_phone", OracleDbType.Varchar2) { Value = (object?)phone ?? DBNull.Value },
            new OracleParameter("p_hometown", OracleDbType.NVarchar2) { Value = (object?)hometown ?? DBNull.Value },
            new OracleParameter("p_dept_id", OracleDbType.Varchar2) { Value = (object?)deptId ?? DBNull.Value },
            new OracleParameter("p_house_no", OracleDbType.NVarchar2) { Value = (object?)houseNo ?? DBNull.Value },
            new OracleParameter("p_street", OracleDbType.NVarchar2) { Value = (object?)street ?? DBNull.Value },
            new OracleParameter("p_district", OracleDbType.NVarchar2) { Value = (object?)district ?? DBNull.Value },
            new OracleParameter("p_city_province", OracleDbType.NVarchar2) { Value = (object?)cityProvince ?? DBNull.Value },
            new OracleParameter("p_medical_history", OracleDbType.NClob) { Value = (object?)medicalHistory ?? DBNull.Value },
            new OracleParameter("p_family_medical_history", OracleDbType.NClob) { Value = (object?)familyMedicalHistory ?? DBNull.Value },
            new OracleParameter("p_drug_allergies", OracleDbType.NClob) { Value = (object?)drugAllergies ?? DBNull.Value }
        };

        _dbProvider.ExecuteNonQuerySP(Sp("USP_UPDATE_USER_LINKED"), p);
    }

    /// <summary>
    /// Vô hiệu hóa User (Soft Delete: Lock and Set Inactive)
    /// </summary>
    public void DeactivateUser(string username)
    {
        OracleParameter[] p =
        {
            new OracleParameter("p_username", OracleDbType.Varchar2) { Value = username },
        };

        _dbProvider.ExecuteNonQuerySP(Sp("USP_DROP_USER_LINKED"), p);
    }

    /// <summary>
    /// Đổi mật khẩu của một User
    /// </summary>
    public void ChangeUserPassword(string username, string newPassword)
    {
        OracleParameter[] p =
        {
            new OracleParameter("p_username", OracleDbType.Varchar2, 128) { Value = username },
            new OracleParameter("p_new_password", OracleDbType.Varchar2, 4000) { Value = newPassword },
        };

        _dbProvider.ExecuteNonQuerySP(Sp("USP_UPDATE_USER_PASSWORD"), p);
    }

    /// <summary>
    /// Khóa tài khoản User
    /// </summary>
    public void LockUser(string username)
    {
        OracleParameter[] p =
        {
            new OracleParameter("p_username", OracleDbType.Varchar2, 128) { Value = username },
        };

        _dbProvider.ExecuteNonQuerySP(Sp("USP_LOCK_USER"), p);
    }

    /// <summary>
    /// Mở khóa tài khoản User
    /// </summary>
    public void UnlockUser(string username)
    {
        OracleParameter[] p =
        {
            new OracleParameter("p_username", OracleDbType.Varchar2, 128) { Value = username },
        };

        _dbProvider.ExecuteNonQuerySP(Sp("USP_UNLOCK_USER"), p);
    }

    /// <summary>
    /// Tạo mới một Role
    /// Sử dụng SP: USP_CREATE_ROLE
    /// </summary>
    public void CreateRole(string roleName, string? password = null)
    {
        OracleParameter[] p =
        {
            new OracleParameter("p_role_name", OracleDbType.Varchar2, 128) { Value = roleName },
            new OracleParameter("p_password", OracleDbType.Varchar2, 4000) { Value = (object?)password ?? DBNull.Value },
        };

        _dbProvider.ExecuteNonQuerySP(Sp("USP_CREATE_ROLE"), p);
    }

    /// <summary>
    /// Đổi mật khẩu của một Role
    /// Sử dụng SP: USP_UPDATE_ROLE_PASSWORD
    /// </summary>
    public void ChangeRolePassword(string roleName, string? password)
    {
        OracleParameter[] p =
        {
            new OracleParameter("p_role_name", OracleDbType.Varchar2, 128) { Value = roleName },
            new OracleParameter("p_password", OracleDbType.Varchar2, 4000) { Value = (object?)password ?? DBNull.Value },
        };

        _dbProvider.ExecuteNonQuerySP(Sp("USP_UPDATE_ROLE_PASSWORD"), p);
    }

    /// <summary>
    /// Xóa Role khỏi hệ thống
    /// Sử dụng SP: USP_DROP_ROLE
    /// </summary>
    public void DropRole(string roleName)
    {
        OracleParameter[] p =
        {
            new OracleParameter("p_role_name", OracleDbType.Varchar2, 128) { Value = roleName },
        };

        _dbProvider.ExecuteNonQuerySP(Sp("USP_DROP_ROLE"), p);
    }

        /// <summary>
        /// Fetch Standard Audit logs from Oracle (Req 3.2)
        /// </summary>
        public DataTable GetStandardAuditLogs()
        {
            OracleParameter[] p = {
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_REQ32_LOGS"), p);
        }

        /// <summary>
        /// Fetch FGA Audit logs for Prescription (Req 3.3a)
        /// </summary>
        public DataTable GetFGAPrescriptionLogs()
        {
            OracleParameter[] p = {
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
            };
            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_REQ33A_LOGS"), p);
        }

        /// <summary>
        /// Fetch FGA + Unified Audit logs for Medical Record (Req 3.3b, 3.3c)
        /// </summary>
        public DataTable GetFGAMedicalRecordLogs()
        {
            OracleParameter[] p = {
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
            };
            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_REQ33BC_LOGS"), p);
        }

        /// <summary>
        /// Fetch Unified Audit logs for Service Record (Req 3.3d)
        /// </summary>
        public DataTable GetUnifiedServiceRecordLogs()
        {
            OracleParameter[] p = {
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
            };
            return _dbProvider.ExecuteQuerySP(Sp("USP_GET_REQ33D_LOGS"), p);
        }
    }
}
