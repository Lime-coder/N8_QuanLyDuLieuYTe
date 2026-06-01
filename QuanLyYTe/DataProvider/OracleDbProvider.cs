using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace QuanLyYTe.DataProvider
{
    public class OracleDbProvider
    {
        public DataTable ExecuteQuery(string sql, OracleParameter[]? parameters = null)
        {
            DataTable dt = new DataTable();
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
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
                            Debug.WriteLine($"[OracleDbProvider] Query failed: {ex}");
                            Console.Error.WriteLine($"[OracleDbProvider] Query failed: {ex}");
                            throw new Exception($"Query execution failed: {ex.Message}", ex);
                        }
                    }
                }
            }
            return dt;
        }

        public DataTable ExecuteQuerySP(string spName, OracleParameter[]? parameters = null)
        {
            DataTable dt = new DataTable();
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
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
                            Debug.WriteLine($"[OracleDbProvider] SP query failed ({spName}): {ex}");
                            Console.Error.WriteLine($"[OracleDbProvider] SP query failed ({spName}): {ex}");
                            throw new Exception($"Stored procedure execution failed ({spName}): {ex.Message}", ex);
                        }
                    }
                }
            }
            return dt;
        }

        public void ExecuteNonQuerySP(string spName, OracleParameter[]? parameters = null)
        {
            using (OracleConnection conn = new OracleConnection(OracleConnectionFactory.GetConnectionString()))
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
                        Debug.WriteLine($"[OracleDbProvider] SP action failed ({spName}): {ex}");
                        Console.Error.WriteLine($"[OracleDbProvider] SP action failed ({spName}): {ex}");
                        throw new Exception($"Stored procedure action failed ({spName}): {ex.Message}", ex);
                    }
                }
            }
        }
    }
}
