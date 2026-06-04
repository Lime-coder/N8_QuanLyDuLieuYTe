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

        public DataTable LoadPersonalInfo()
        {
            return _repository.GetPersonalInfo();
        }

        public void UpdatePersonalInfo(string? phone = null, string? hometown = null)
        {
            // Only update allowed personal fields (phone, hometown) via technician-specific SP
            _repository.UpdatePersonalInfo(phone ?? string.Empty, hometown ?? string.Empty);
        }
    }
}