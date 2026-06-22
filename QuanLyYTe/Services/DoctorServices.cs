using System;
using System.Data;
using QuanLyYTe.Repositories;

namespace QuanLyYTe.Services
{
    public class DoctorService
    {
        private readonly DoctorRepository _doctorRepo = new DoctorRepository();

        public DataTable GetMedicalRecords(string s = "") 
            => _doctorRepo.GetMedicalRecordList(s);

        public void SaveMedicalRecord(string id, string dg, string tr, string cl) 
            => _doctorRepo.UpdateMedicalRecord(id, dg, tr, cl);

        public DataTable GetServices(string s = "") 
            => _doctorRepo.GetServices(s);

        public void CreateService(string id, string ty) 
            => _doctorRepo.AddService(id, ty);
        public void RemoveService(string id, string ty, DateTime dt) 
            => _doctorRepo.DeleteService(id, ty, dt);

        public DataTable GetPrescriptions(string s = "") 
            => _doctorRepo.GetPrescriptions(s);

        public void SavePrescription(string a, string id, string m, string d, DateTime? dt = null, string om = null) 
            => _doctorRepo.ManagePrescription(a, id, m, d, dt, om);

        public DataTable GetPatients(string s = "") 
            => _doctorRepo.GetPatients(s);

        public void SavePatient(string id, string h, string f, string a) 
            => _doctorRepo.UpdatePatient(id, h, f, a);

        public DataTable GetSelfInfo() 
            => _doctorRepo.GetSelf();

        public void UpdateSelfInfo(string h, string p) 
            => _doctorRepo.UpdateSelf(h, p);
    }
}
