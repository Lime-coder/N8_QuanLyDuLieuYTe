using System;
using Oracle.ManagedDataAccess.Client;

class Program
{
    static void Main()
    {
        string connStr = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=PDB_QLYT)));User Id=hospital_dba;Password=123;";
        using (var conn = new OracleConnection(connStr))
        {
            conn.Open();
            using (var cmd = new OracleCommand("SELECT text FROM ALL_SOURCE WHERE NAME = 'FN_VPD_RECORD_DETAIL_DOCTOR' ORDER BY LINE", conn))
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                    Console.Write($"{reader[0]}");
        }
    }
}
