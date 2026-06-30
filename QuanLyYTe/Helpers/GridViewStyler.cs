using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyYTe.Helpers
{
    public class GridViewStyler
    {
        public static void Format(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            string[] dateColumns = { "birthdate", "LOCK_DATE", "CREATED" };
            foreach (string col in dateColumns)
            {
                if (dgv.Columns.Contains(col))
                    dgv.Columns[col].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
        }
    }
}
