using System.Data;
using QuanLyYTe.Repositories;

namespace QuanLyYTe.Services
{
    public class DoctorService
    {
        private readonly DoctorRepository _doctorRepo = new DoctorRepository();

        public DataTable GetMedicalRecords(string s = "") => _doctorRepo.GetMedicalRecordList(s);
        public void AddMedicalRecord(string id, string pat, string dg, string tr, string cl) => _doctorRepo.AddMedicalRecord(id, pat, dg, tr, cl);
        public void SaveMedicalRecord(string id, string dg, string tr, string cl) => _doctorRepo.UpdateMedicalRecord(id, dg, tr, cl);

        public DataTable GetServices(string search = "")
        {
            return _doctorRepo.GetServices(search);
        }

        public void CreateService(string id, string ty, string rs)
        {
            _doctorRepo.AddService(id, ty, rs);
        }

        public void RemoveService(string id, string ty, System.DateTime dt)
        {
            _doctorRepo.DeleteService(id, ty, dt);
        }

        public DataTable GetPrescriptions(string s = "") => _doctorRepo.GetPrescriptions(s);
        public void SavePrescription(string a, string id, string m, string d, DateTime? dt = null, string om = null)
            => _doctorRepo.ManagePrescription(a, id, m, d, dt, om);

        public DataTable GetPatients(string s = "") => _doctorRepo.GetPatients(s);
        public void SavePatient(string id, string h, string f, string a) => _doctorRepo.UpdatePatient(id, h, f, a);
    }
}