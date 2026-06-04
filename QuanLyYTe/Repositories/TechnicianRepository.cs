using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DataProvider;

namespace QuanLyYTe.Repositories
{
    public class TechnicianRepository
    {
        private readonly OracleDbProvider _dbProvider = new OracleDbProvider();

        public DataTable GetAssignedServiceRecords()
        {
            OracleParameter[] parameters =
            {
                new OracleParameter("P_CURSOR", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                }
            };

            return _dbProvider.ExecuteQuerySP("HOSPITAL.GET_TECHNICIAN_SERVICE_RECORDS", parameters);
        }

        public DataTable GetPersonalInfo()
        {
            OracleParameter[] parameters =
            {
                new OracleParameter("P_CURSOR", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };

            return _dbProvider.ExecuteQuerySP("HOSPITAL.GET_TECHNICIAN_PERSONAL_INFO", parameters);
        }

        public void UpdatePersonalInfo(string phone, string hometown)
        {
            OracleParameter[] parameters =
            {
                new OracleParameter("P_PHONE", OracleDbType.Varchar2) { Value = (object?)phone ?? DBNull.Value },
                new OracleParameter("P_HOMETOWN", OracleDbType.NVarchar2) { Value = (object?)hometown ?? DBNull.Value }
            };

            _dbProvider.ExecuteNonQuerySP("HOSPITAL.UPDATE_TECHNICIAN_PERSONAL_INFO", parameters);
        }

        public void UpdateServiceResult(string recordId, string serviceType, DateTime serviceDate, string serviceResult)
        {
            OracleParameter[] parameters =
            {
                new OracleParameter("P_RECORD_ID", OracleDbType.Varchar2) { Value = recordId },
                new OracleParameter("P_SERVICE_TYPE", OracleDbType.NVarchar2) { Value = serviceType },
                new OracleParameter("P_SERVICE_DATE", OracleDbType.Date) { Value = serviceDate },
                new OracleParameter("P_SERVICE_RESULT", OracleDbType.NVarchar2) { Value = serviceResult }
            };

            _dbProvider.ExecuteNonQuerySP("HOSPITAL.UPDATE_TECHNICIAN_SERVICE_RESULT", parameters);
        }
    }
}