using System;
using System.Data;
using QuanLyYTe.Repositories;

namespace QuanLyYTe.Services
{
    public class CoordinatorService
    {
        private readonly CoordinatorRepository _coordinatorRepo = new CoordinatorRepository();

        public DataTable GetDoctors() => GetDoctorsForAssignment();
        public DataTable GetDoctorsForAssignment() => _coordinatorRepo.GetDoctorsForAssignment();
        public DataTable GetDoctorsByDepartment(string deptId) => string.IsNullOrEmpty(deptId) ? new DataTable() : _coordinatorRepo.GetDoctorsByDepartment(deptId);
        public DataTable GetTechnicians() => GetTechniciansForAssignment();
        public DataTable GetTechniciansForAssignment() => _coordinatorRepo.GetTechniciansForAssignment();
        public DataTable GetDepartments() => _coordinatorRepo.GetDepartments();
        public void UpdateMedicalRecord(string recordId, string doctorId, string deptId) => _coordinatorRepo.UpdateMedicalRecord(recordId, doctorId, deptId);
        public void UpdateServiceRecord(string recordId, string technicianId) => _coordinatorRepo.UpdateServiceRecord(recordId, technicianId);
        public DataTable GetSelfStaffInfo() => _coordinatorRepo.GetSelfStaffInfo();
        public void UpdateSelfStaffInfo(string phone, string hometown) => _coordinatorRepo.UpdateSelfStaffInfo(phone, hometown);

        // --- PatientService ---
        public DataTable GetAllPatients() => _coordinatorRepo.GetAllPatients();
        public DataTable SearchPatients(string keyword) => string.IsNullOrWhiteSpace(keyword) ? _coordinatorRepo.GetAllPatients() : _coordinatorRepo.SearchPatients(keyword.Trim());
        public void InsertPatient(string patientId, string fullName, string gender, DateTime birthDate, string idCard, string houseNo, string street, string district, string cityProvince, string medicalHistory, string familyMedicalHistory, string drugAllergies, string usernameDb)
        {
            if (string.IsNullOrWhiteSpace(patientId)) throw new ArgumentException("Mã bệnh nhân không được để trống.");
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Họ tên bệnh nhân không được để trống.");
            _coordinatorRepo.InsertPatient(patientId, fullName, gender, birthDate, idCard, houseNo, street, district, cityProvince, medicalHistory, familyMedicalHistory, drugAllergies, usernameDb);
        }
        public void UpdatePatient(string patientId, string fullName, string gender, DateTime birthDate, string idCard, string houseNo, string street, string district, string cityProvince, string medicalHistory, string familyMedicalHistory, string drugAllergies, string usernameDb)
        {
            if (string.IsNullOrWhiteSpace(patientId)) throw new ArgumentException("Mã bệnh nhân không được để trống.");
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Họ tên bệnh nhân không được để trống.");
            _coordinatorRepo.UpdatePatient(patientId, fullName, gender, birthDate, idCard, houseNo, street, district, cityProvince, medicalHistory, familyMedicalHistory, drugAllergies, usernameDb);
        }

        // --- MedicalRecordService ---
        public DataTable GetAllMedicalRecords() => _coordinatorRepo.GetAllMedicalRecords();
        public void InsertMedicalRecord(string recordId, string patientId, DateTime recordDate, string doctorId, string deptId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentException("Mã hồ sơ bệnh án không được để trống.");
            if (string.IsNullOrWhiteSpace(patientId)) throw new ArgumentException("Mã bệnh nhân không được để trống.");
            _coordinatorRepo.InsertMedicalRecord(recordId, patientId, recordDate, doctorId, deptId);
        }
        public void UpdateCoordinatorFields(string recordId, string doctorId, string deptId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentException("Mã hồ sơ bệnh án không được để trống.");
            _coordinatorRepo.UpdateCoordinatorFields(recordId, doctorId, deptId);
        }

        // --- ServiceAssignmentService ---
        public DataTable GetAllServiceAssignments() => _coordinatorRepo.GetAllServiceAssignments();
        public void UpdateTechnician(string recordId, string serviceType, DateTime serviceDate, string technicianId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) throw new ArgumentException("Mã hồ sơ bệnh án không được để trống.");
            if (string.IsNullOrWhiteSpace(serviceType)) throw new ArgumentException("Mã dịch vụ không được để trống.");
            _coordinatorRepo.UpdateTechnician(recordId, serviceType, serviceDate, technicianId);
        }
    }
}
