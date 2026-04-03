using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using QuanLyYTe.DAL;

namespace QuanLyYTe.Forms
{
    public partial class Test_ExecuteQuerySP : Form
    {
        public Test_ExecuteQuerySP()
        {
            InitializeComponent();
        }

        private void btnLoadStaff_Click(object sender, EventArgs e)
        {
            try
            {
                StaffRepository repo = new StaffRepository();
                dataGridView1.DataSource = repo.Test();
                OracleHelper.FormatGridView(dataGridView1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}


