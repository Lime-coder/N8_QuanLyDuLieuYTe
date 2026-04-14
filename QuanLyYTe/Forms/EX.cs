using System;
using System.Data;
using System.Windows.Forms;

namespace QuanLyYTe.Forms
{
    public partial class EX : Form
    {
        public EX()
        {
            InitializeComponent();
        }

        private void GrantPermissionForm_Load(object sender, EventArgs e)
        {
            cbObjectType.SelectedIndex = 0;
            cbGranteeType.SelectedIndex = 0;
            LoadMockDataForRoleTab(); // Nạp dữ liệu giả cho Tab Role
        }

        // --- XỬ LÝ TAB 1: QUYỀN ĐỐI TƯỢNG ---

        private void cbGranteeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbGranteeName.Items.Clear();
            if (cbGranteeType.Text == "User")
                cbGranteeName.Items.AddRange(new string[] { "DOCTOR_01", "NURSE_01", "PATIENT_001" });
            else
                cbGranteeName.Items.AddRange(new string[] { "ROLE_DOCTOR", "ROLE_PATIENT", "ROLE_TECH" });

            if (cbGranteeName.Items.Count > 0) cbGranteeName.SelectedIndex = 0;
        }

        private void cbObjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = cbObjectType.Text;
            cbObjectName.Items.Clear();
            if (type == "TABLE") cbObjectName.Items.AddRange(new string[] { "BENHNHAN", "NHANVIEN", "HSBA" });
            else if (type == "VIEW") cbObjectName.Items.AddRange(new string[] { "V_BENHNHAN_INFO" });
            else cbObjectName.Items.AddRange(new string[] { "SP_UPDATE_DATA" });

            if (cbObjectName.Items.Count > 0) cbObjectName.SelectedIndex = 0;
        }

        private void clbPrivs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                bool showCols = false;
                foreach (object item in clbPrivs.CheckedItems)
                {
                    string priv = item.ToString();
                    if (priv == "SELECT" || priv == "UPDATE") { showCols = true; break; }
                }
                lblCols.Visible = clbCols.Visible = (showCols && cbObjectType.Text == "TABLE");
                if (clbCols.Visible) LoadDummyCols();
            }));
        }

        private void cbObjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clbCols.Visible) LoadDummyCols();
        }

        private void LoadDummyCols()
        {
            clbCols.Items.Clear();
            if (cbObjectName.Text == "BENHNHAN")
                clbCols.Items.AddRange(new string[] { "MABN", "TENBN", "CCCD", "DIACHI" });
            else
                clbCols.Items.AddRange(new string[] { "COL_1", "COL_2" });
        }

        private void btnGrantObj_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đã mô phỏng GRANT Quyền đối tượng thành công!");
        }


        // --- XỬ LÝ TAB 2: CẤP ROLE CHO USER ---

        private void LoadMockDataForRoleTab()
        {
            // Nạp danh sách User giả
            cbUserForRole.Items.Clear();
            cbUserForRole.Items.AddRange(new string[] { "PATIENT_001", "DOCTOR_001", "ADMIN_001" });
            cbUserForRole.SelectedIndex = 0;

            // Nạp danh sách Role giả
            lstRolesToAssign.Items.Clear();
            lstRolesToAssign.Items.AddRange(new string[] {
                "ROLE_DOCTOR",
                "ROLE_COORDINATOR",
                "ROLE_TECHNICIAN",
                "ROLE_PATIENT",
                "CONNECT",
                "RESOURCE"
            });
        }

        private void btnGrantRole_Click(object sender, EventArgs e)
        {
            string user = cbUserForRole.Text;
            string role = lstRolesToAssign.SelectedItem?.ToString();
            string adminOpt = chkWithAdmin.Checked ? " WITH ADMIN OPTION" : "";

            if (string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Vui lòng chọn một Role từ danh sách!");
                return;
            }

            MessageBox.Show($"MÔ PHỎNG GRANT ROLE:\nGRANT {role} TO {user}{adminOpt};", "Thành công");
        }

        private void gbUserRoleInfo_Enter(object sender, EventArgs e)
        {

        }
    }
}