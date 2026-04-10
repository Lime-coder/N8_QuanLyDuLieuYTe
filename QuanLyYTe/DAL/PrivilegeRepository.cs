using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace QuanLyYTe.DAL
{
    public class PrivilegeRepository
    {
        private readonly string _connectionString;

        public PrivilegeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Fetch list of users or roles using usp_GetGrantees
        /// </summary>
        public List<string> GetGrantees(string granteeType)
        {
            List<string> result = new List<string>();
            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = new OracleCommand("usp_GetGrantees", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_grantee_type", OracleDbType.Varchar2).Value = granteeType;
                    cmd.Parameters.Add("p_result_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    conn.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read()) result.Add(dr[0].ToString());
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Fetch list of database objects using usp_GetObjects
        /// </summary>
        public List<string> GetObjects(string objectType)
        {
            List<string> result = new List<string>();
            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = new OracleCommand("usp_GetObjects", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_object_type", OracleDbType.Varchar2).Value = objectType;
                    cmd.Parameters.Add("p_result_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    conn.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read()) result.Add(dr[0].ToString());
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Fetch columns of a table/view using usp_GetColumns
        /// </summary>
        public List<string> GetColumns(string tableName)
        {
            List<string> result = new List<string>();
            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = new OracleCommand("usp_GetColumns", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_table_name", OracleDbType.Varchar2).Value = tableName;
                    cmd.Parameters.Add("p_result_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    conn.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read()) result.Add(dr[0].ToString());
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Execute granting logic using usp_HandleGrantPrivilege
        /// </summary>
        public void ExecuteGrant(string grantee, string priv, string obj, string cols, int gOpt, int aOpt)
        {
            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = new OracleCommand("usp_HandleGrantPrivilege", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_grantee", OracleDbType.Varchar2).Value = grantee;
                    cmd.Parameters.Add("p_priv_or_role", OracleDbType.Varchar2).Value = priv;
                    cmd.Parameters.Add("p_object_name", OracleDbType.Varchar2).Value = (object)obj ?? DBNull.Value;
                    cmd.Parameters.Add("p_column_list", OracleDbType.Varchar2).Value = (object)cols ?? DBNull.Value;
                    cmd.Parameters.Add("p_with_grant", OracleDbType.Int32).Value = gOpt;
                    cmd.Parameters.Add("p_with_admin", OracleDbType.Int32).Value = aOpt;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}