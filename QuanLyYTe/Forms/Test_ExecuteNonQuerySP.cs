using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using QuanLyYTe.DAL;

namespace QuanLyYTe.Forms
{
    public partial class Test_ExecuteNonQuerySP : Form
    {
        public Test_ExecuteNonQuerySP()
        {
            InitializeComponent();
        }


        private void Test2_Load(object sender, EventArgs e)
        {
            StaffRepository repo = new StaffRepository();
            (string, string) result = repo.Test2("NV001");
            MessageBox.Show(result.ToString());
            this.Close();
        }
    }
}
