using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace QuanLyYTe.DAL
{
    internal class PatientRepository
    {
        public DataTable GetAllWithAccountStatus()
        {
            const string sql = @"
SELECT 
    p.patient_id,
    p.full_name,
    p.gender,
    p.birthdate,
    p.id_card,
    p.house_no,
    p.street,
    p.district,
    p.city_province,
    p.username_db,
    u.account_status,
    u.created,
    u.lock_date
FROM patient p
LEFT JOIN dba_users u
    ON u.username = UPPER(p.username_db)
ORDER BY p.patient_id";
            return OracleHelper.ExecuteQuery(sql);
        }

        public void CreatePatient(
            string patientId,
            string fullName,
            string gender,
            DateTime birthdate,
            string idCard,
            string houseNo,
            string street,
            string district,
            string cityProvince,
            string? medicalHistory,
            string? familyMedicalHistory,
            string? drugAllergies,
            string password)
        {
            if (string.IsNullOrWhiteSpace(patientId)) throw new ArgumentException("patientId rỗng");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("password rỗng");

            string usernameDb = GeneratePatientUsername();

            const string insertSql = @"
INSERT INTO patient(
    patient_id, full_name, gender, birthdate, id_card,
    house_no, street, district, city_province,
    medical_history, family_medical_history, drug_allergies,
    username_db
) VALUES (
    :patient_id, :full_name, :gender, :birthdate, :id_card,
    :house_no, :street, :district, :city_province,
    :medical_history, :family_medical_history, :drug_allergies,
    :username_db
)";

            OracleParameter[] p =
            {
                new OracleParameter("patient_id", OracleDbType.Varchar2, 10) { Value = patientId },
                new OracleParameter("full_name", OracleDbType.NVarchar2, 100) { Value = fullName },
                new OracleParameter("gender", OracleDbType.NVarchar2, 5) { Value = gender },
                new OracleParameter("birthdate", OracleDbType.Date) { Value = birthdate },
                new OracleParameter("id_card", OracleDbType.Varchar2, 20) { Value = idCard },
                new OracleParameter("house_no", OracleDbType.NVarchar2, 50) { Value = houseNo },
                new OracleParameter("street", OracleDbType.NVarchar2, 100) { Value = street },
                new OracleParameter("district", OracleDbType.NVarchar2, 100) { Value = district },
                new OracleParameter("city_province", OracleDbType.NVarchar2, 100) { Value = cityProvince },
                new OracleParameter("medical_history", OracleDbType.Clob) { Value = (object?)medicalHistory ?? DBNull.Value },
                new OracleParameter("family_medical_history", OracleDbType.Clob) { Value = (object?)familyMedicalHistory ?? DBNull.Value },
                new OracleParameter("drug_allergies", OracleDbType.Clob) { Value = (object?)drugAllergies ?? DBNull.Value },
                new OracleParameter("username_db", OracleDbType.Varchar2, 30) { Value = usernameDb },
            };

            OracleHelper.ExecuteNonQuery(insertSql, p);

            var sec = new SecurityAdminRepository();
            sec.CreateUser(usernameDb, password);
            GrantRoleForPatient(usernameDb);
        }

        public void UpdatePatient(
            string patientId,
            string fullName,
            string gender,
            DateTime birthdate,
            string idCard,
            string houseNo,
            string street,
            string district,
            string cityProvince,
            string? medicalHistory,
            string? familyMedicalHistory,
            string? drugAllergies)
        {
            const string sql = @"
UPDATE patient SET
    full_name = :full_name,
    gender = :gender,
    birthdate = :birthdate,
    id_card = :id_card,
    house_no = :house_no,
    street = :street,
    district = :district,
    city_province = :city_province,
    medical_history = :medical_history,
    family_medical_history = :family_medical_history,
    drug_allergies = :drug_allergies
WHERE patient_id = :patient_id";

            OracleParameter[] p =
            {
                new OracleParameter("full_name", OracleDbType.NVarchar2, 100) { Value = fullName },
                new OracleParameter("gender", OracleDbType.NVarchar2, 5) { Value = gender },
                new OracleParameter("birthdate", OracleDbType.Date) { Value = birthdate },
                new OracleParameter("id_card", OracleDbType.Varchar2, 20) { Value = idCard },
                new OracleParameter("house_no", OracleDbType.NVarchar2, 50) { Value = houseNo },
                new OracleParameter("street", OracleDbType.NVarchar2, 100) { Value = street },
                new OracleParameter("district", OracleDbType.NVarchar2, 100) { Value = district },
                new OracleParameter("city_province", OracleDbType.NVarchar2, 100) { Value = cityProvince },
                new OracleParameter("medical_history", OracleDbType.Clob) { Value = (object?)medicalHistory ?? DBNull.Value },
                new OracleParameter("family_medical_history", OracleDbType.Clob) { Value = (object?)familyMedicalHistory ?? DBNull.Value },
                new OracleParameter("drug_allergies", OracleDbType.Clob) { Value = (object?)drugAllergies ?? DBNull.Value },
                new OracleParameter("patient_id", OracleDbType.Varchar2, 10) { Value = patientId },
            };

            OracleHelper.ExecuteNonQuery(sql, p);
        }

        public void DeletePatient(string patientId, string usernameDb, bool dropDbUser = true)
        {
            const string sql = "DELETE FROM patient WHERE patient_id = :patient_id";
            OracleParameter[] p = { new OracleParameter("patient_id", OracleDbType.Varchar2, 10) { Value = patientId } };
            OracleHelper.ExecuteNonQuery(sql, p);

            if (dropDbUser)
            {
                var sec = new SecurityAdminRepository();
                sec.DropUser(usernameDb, cascade: true);
            }
        }

        private static void GrantRoleForPatient(string usernameDb)
        {
            string sql = $"GRANT RL_PATIENT TO {usernameDb}";
            OracleHelper.ExecuteNonQuery(sql);
        }

        private static string GeneratePatientUsername()
        {
            // Oracle identifier <= 30 chars; keep it simple & unique-ish
            string token = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpperInvariant();
            return "PT_" + token; // 15 chars
        }
    }
}
