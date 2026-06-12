using System.Data;
using QuanLyYTe.Common;
using QuanLyYTe.Repositories;

namespace QuanLyYTe.Services
{
    public class PatientService
    {
        private readonly PatientRepository _repo = new PatientRepository();

        public DataTable GetProfile() => _repo.GetProfile();

        public DataTable GetMedicalRecords() => _repo.GetMedicalRecords();

        public DataTable GetPrescriptions(string recordId) => _repo.GetPrescriptions(recordId);

        public DataTable GetServices(string recordId) => _repo.GetServices(recordId);

        public void UpdateContact(string houseNo, string street, string district, string cityProvince, string medHistory, string famHistory, string allergies)
        {
            try
            {
                _repo.UpdateContact(houseNo, street, district, cityProvince, medHistory, famHistory, allergies);
            }
            catch (Exception ex)
            {
                throw new Exception(OracleErrorMapper.GetUserMessage(ex));
            }
        }
    }
}
