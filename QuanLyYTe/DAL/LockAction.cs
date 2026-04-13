namespace QuanLyYTe.DAL
{
    /// <summary>
    /// Enum để quản lý trạng thái khoá/mở khoá của User
    /// </summary>
    public enum LockAction
    {
        /// <summary>Không thay đổi trạng thái</summary>
        None = 0,

        /// <summary>Khoá tài khoản</summary>
        Lock = 1,

        /// <summary>Mở khoá tài khoản</summary>
        Unlock = 2
    }
}
