using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyYTe.Common;

namespace QuanLyYTe.Forms.Doctor
{
    public partial class frmDoctor : Form
    {
        public frmDoctor()
        {
            this.Size = new Size(1300, 850);
            this.Text = "Hospital Management - Doctor";
            this.WindowState = FormWindowState.Maximized;
            InitializeComponent();
            OpenPage("Quản lý HSBA", () => new frmMedicalRecordManagement());
        }

        private void AddNav(Panel p, string t, Action a)
        {
            Button b = new Button { 
                Text = t, 
                Dock = DockStyle.Top, 
                Height = 55, 
                FlatStyle = FlatStyle.Flat, 
                ForeColor = Color.White, 
                TextAlign = ContentAlignment.MiddleLeft, 
                Padding = new Padding(20, 0, 0, 0) 
            };

            b.FlatAppearance.BorderSize = 0;
            b.Click += (s, e) => a();
            p.Controls.Add(b);
            b.BringToFront();
        }

        private void OpenPage(string t, Func<Form> f)
        {
            lblPageTitle.Text = t; lblBreadcrumb.Text = "Dashboard / " + t;
            pnlContent.Controls.Clear();
            Form c = f(); 
            c.TopLevel = false; 
            c.FormBorderStyle = FormBorderStyle.None; 
            c.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(c); 
            c.Show();
        }
    }
}