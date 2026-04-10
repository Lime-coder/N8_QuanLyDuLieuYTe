using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace QuanLyYTe.DAL
{
    public static class OracleHelper
    {
        // Lấy chuỗi kết nối từ App.config
        private static string _connStr = ConfigurationManager.ConnectionStrings["HospitalDB"].ConnectionString;
        public static void SetConnectionString(string connStr)
        {
            _connStr = connStr;
        }

        public static void TestConnection(string connStr)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                conn.Open(); // throws if credentials are wrong
            }
        }

        // SP trả về một danh sách
        public static DataTable ExecuteQuerySP(string spName, OracleParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            using (OracleConnection conn = new OracleConnection(_connStr))
            {
                using (OracleCommand cmd = new OracleCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null) cmd.Parameters.AddRange(parameters);

                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                    {
                        try
                        {
                            da.Fill(dt);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Lỗi thực thi SP {spName}: {ex.Message}");
                        }
                    }
                }
            }
            return dt;
        }

        public static DataTable ExecuteQuery(string sql, OracleParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            using (OracleConnection conn = new OracleConnection(_connStr))
            {
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                    {
                        try
                        {
                            da.Fill(dt);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Lỗi thực thi query: {ex.Message}");
                        }
                    }
                }
            }
            return dt;
        }

        // SP trả về những tham số - Truyền vào mảng 
        // SP không trả về tham số
        public static void ExecuteNonQuerySP(string spName, OracleParameter[] parameters = null)
        {
            using (OracleConnection conn = new OracleConnection(_connStr))
            {
                using (OracleCommand cmd = new OracleCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null) cmd.Parameters.AddRange(parameters);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        // Sau khi Execute, các giá trị từ Oracle đã được nạp ngược lại vào mảng 'parameters'
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Lỗi Action SP {spName}: {ex.Message}");
                    }
                }
            }
        }

        public static void FormatGridView(DataGridView dgv)
        {
            // Quy định chung
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Format cột ngày tháng
            if (dgv.Columns.Contains("birthdate"))
                dgv.Columns["birthdate"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }
    }
}
