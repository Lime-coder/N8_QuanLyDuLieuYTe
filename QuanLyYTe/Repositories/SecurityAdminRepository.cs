using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DataProvider;

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
    /// Tạo mới một User
    /// Sử dụng SP: USP_CREATE_USER
    /// </summary>
    public void CreateUser(string username, string password)
    {
        OracleParameter[] p =
        {
            new OracleParameter("p_username", OracleDbType.Varchar2, 128) { Value = username },
            new OracleParameter("p_password", OracleDbType.Varchar2, 4000) { Value = password },
        };

        _dbProvider.ExecuteNonQuerySP(Sp("USP_CREATE_USER"), p);
    }

    /// <summary>
    /// Đổi mật khẩu của một User
    /// Sử dụng SP: USP_UPDATE_USER_PASSWORD
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
    /// Sử dụng SP: USP_LOCK_USER
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
    /// Sử dụng SP: USP_UNLOCK_USER
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
    /// Xóa User khỏi hệ thống
    /// Sử dụng SP: USP_DROP_USER
    /// </summary>
    public void DropUser(string username, bool cascade = true)
    {
        OracleParameter[] p =
        {
            new OracleParameter("p_username", OracleDbType.Varchar2, 128) { Value = username },
            new OracleParameter("p_cascade", OracleDbType.Varchar2, 3) { Value = cascade ? "YES" : "NO" },
        };

        _dbProvider.ExecuteNonQuerySP(Sp("USP_DROP_USER"), p);
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
}
