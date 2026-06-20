using System;
using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DataProvider;

namespace QuanLyYTe.Repositories
{
    public class TechnicianRepository : BaseRepository
    {
        // Lấy danh sách dịch vụ được phân công (SP: GET_TECHNICIAN_SERVICE_RECORDS)
        public DataTable GetAssignedServiceRecords()
        {
            OracleParameter[] parameters =
            {
                new OracleParameter("P_CURSOR", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                }
            };

            return _dbProvider.ExecuteQuerySP(Sp("GET_TECHNICIAN_SERVICE_RECORDS"), parameters);
        }

        // Lấy thông tin cá nhân của kỹ thuật viên (SP: GET_TECHNICIAN_PERSONAL_INFO)
        public DataTable GetPersonalInfo()
        {
            OracleParameter[] parameters =
            {
                new OracleParameter("P_CURSOR", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
            };

            return _dbProvider.ExecuteQuerySP(Sp("GET_TECHNICIAN_PERSONAL_INFO"), parameters);
        }

        // Cập nhật thông tin cá nhân (SP: UPDATE_TECHNICIAN_PERSONAL_INFO)
        public void UpdatePersonalInfo(string phone, string hometown)
        {
            OracleParameter[] parameters =
            {
                new OracleParameter("P_PHONE",    OracleDbType.Varchar2)  { Value = (object?)phone    ?? DBNull.Value },
                new OracleParameter("P_HOMETOWN", OracleDbType.NVarchar2) { Value = (object?)hometown ?? DBNull.Value }
            };

            _dbProvider.ExecuteNonQuerySP(Sp("UPDATE_TECHNICIAN_PERSONAL_INFO"), parameters);
        }

        // Cập nhật kết quả dịch vụ (SP: UPDATE_TECHNICIAN_SERVICE_RESULT)
        public void UpdateServiceResult(string recordId, string serviceType, DateTime serviceDate, string serviceResult)
        {
            OracleParameter[] parameters =
            {
                new OracleParameter("P_RECORD_ID",      OracleDbType.Varchar2)  { Value = recordId },
                new OracleParameter("P_SERVICE_TYPE",   OracleDbType.NVarchar2) { Value = serviceType },
                new OracleParameter("P_SERVICE_DATE",   OracleDbType.Date)      { Value = serviceDate },
                new OracleParameter("P_SERVICE_RESULT", OracleDbType.NVarchar2) { Value = serviceResult }
            };

            _dbProvider.ExecuteNonQuerySP(Sp("UPDATE_TECHNICIAN_SERVICE_RESULT"), parameters);
        }
    }
}
