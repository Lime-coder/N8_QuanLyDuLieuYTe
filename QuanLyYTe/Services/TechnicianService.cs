using System;
using System.Data;
using QuanLyYTe.Repositories;

namespace QuanLyYTe.Services
{
    public class TechnicianService
    {
        private readonly TechnicianRepository _repository = new TechnicianRepository();

        public DataTable LoadAssignedServices()
        {
            return _repository.GetAssignedServiceRecords();
        }

        public void SaveServiceResult(string recordId, string serviceType, DateTime serviceDate, string serviceResult)
        {
            _repository.UpdateServiceResult(recordId, serviceType, serviceDate, serviceResult);
        }
    }
}