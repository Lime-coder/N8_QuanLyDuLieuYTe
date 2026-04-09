using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyYTe.Forms
{
    public partial class FrmPrivilege : Form
    {
        PrivilegeRepository repo = new PrivilegeRepository();

        public FrmPrivilege()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbUser.Text))
            {
                MessageBox.Show("Vui lòng chọn user");
                return;
            }

            dgvPrivilege.DataSource = repo.GetAllPrivileges(cbUser.Text);
        }

        private void btnRevoke_Click(object sender, EventArgs e)
        {
            if (dgvPrivilege.CurrentRow == null)
            {
                MessageBox.Show("Chọn quyền cần thu hồi");
                return;
            }

            string type = dgvPrivilege.CurrentRow.Cells["TYPE"].Value.ToString();
            string privilege = dgvPrivilege.CurrentRow.Cells["PRIVILEGE"].Value.ToString();
            string owner = dgvPrivilege.CurrentRow.Cells["OWNER"]?.Value?.ToString();
            string obj = dgvPrivilege.CurrentRow.Cells["OBJECT_NAME"]?.Value?.ToString();
            string column = dgvPrivilege.CurrentRow.Cells["COLUMN_NAME"]?.Value?.ToString();
            string user = cbUser.Text;

            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn thu hồi quyền này?",
                "Xác nhận",
                MessageBoxButtons.YesNo
            );

            if (result == DialogResult.No) return;

            repo.RevokePrivilege(type, privilege, owner, obj, column, user);

            MessageBox.Show("Thu hồi thành công");

            dgvPrivilege.DataSource = repo.GetAllPrivileges(user);
        }

        private void LoadUsers()
        {
            DataTable dt = repo.GetUsers();

            cbUser.DataSource = dt;
            cbUser.DisplayMember = "USERNAME";
            cbUser.ValueMember = "USERNAME";

            if (cbUser.Items.Count > 0)
                cbUser.SelectedIndex = 0;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dgvPrivilege_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void FrmPrivilege_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }
    }
}
