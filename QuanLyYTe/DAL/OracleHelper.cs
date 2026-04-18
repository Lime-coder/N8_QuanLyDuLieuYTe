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
        private static string _connStr = ConfigurationManager.ConnectionStrings["HospitalDB"].ConnectionString;

        public static DataTable ExecuteQuerySP(string spName, OracleParameter[]? parameters = null)
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
                            throw new Exception($"Stored procedure execution failed ({spName}): {ex.Message}");
                        }
                    }
                }
            }
            return dt;
        }

        public static DataTable ExecuteQuery(string sql, OracleParameter[]? parameters = null)
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
                            throw new Exception($"Query execution failed: {ex.Message}");
                        }
                    }
                }
            }
            return dt;
        }

        public static void ExecuteNonQuerySP(string spName, OracleParameter[]? parameters = null)
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
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Stored procedure action failed ({spName}): {ex.Message}");
                    }
                }
            }
        }

        public static void ExecuteNonQuery(string sql, OracleParameter[]? parameters = null)
        {
            using (OracleConnection conn = new OracleConnection(_connStr))
            {
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    if (parameters != null) cmd.Parameters.AddRange(parameters);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Command execution failed: {ex.Message}");
                    }
                }
            }
        }

        public static void FormatGridView(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            string[] dateColumns = { "birthdate", "LOCK_DATE", "CREATED" };
            foreach (string col in dateColumns)
            {
                if (dgv.Columns.Contains(col))
                    dgv.Columns[col].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
        }
    }
}
