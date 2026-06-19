using System;
using System.Windows.Forms;

namespace QuanLyYTe.Forms.Doctor
{
    partial class frmDoctorBase
    {
        // Required designer variable.
        protected System.ComponentModel.IContainer components = null;
        // Clean up any resources being used.
        // <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        // Required method for Designer support - do not modify
        // the contents of this method with the code editor.
        private void InitializeComponent()
        {
            Dgv = new DataGridView();
            TxtS = new TextBox();
            pnlSearch = new Panel();
            lblS = new Label();
            ((System.ComponentModel.ISupportInitialize)Dgv).BeginInit();
            pnlSearch.SuspendLayout();
            SuspendLayout();
            // 
            // Dgv
            // 
            Dgv.AllowUserToAddRows = false;
            Dgv.BackgroundColor = Color.White;
            Dgv.BorderStyle = BorderStyle.None;
            Dgv.ColumnHeadersHeight = 29;
            Dgv.Dock = DockStyle.Fill;
            Dgv.Location = new Point(0, 75);
            Dgv.Name = "Dgv";
            Dgv.RowHeadersWidth = 51;
            Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Dgv.Size = new Size(322, 172);
            Dgv.TabIndex = 0;
            // 
            // TxtS
            // 
            TxtS.Location = new Point(70, 25);
            TxtS.Name = "TxtS";
            TxtS.Size = new Size(200, 27);
            TxtS.TabIndex = 1;
            // 
            // pnlSearch
            // 
            pnlSearch.BackColor = Color.FromArgb(242, 242, 242);
            pnlSearch.Controls.Add(lblS);
            pnlSearch.Controls.Add(TxtS);
            pnlSearch.Dock = DockStyle.Top;
            pnlSearch.Location = new Point(0, 0);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Size = new Size(322, 75);
            pnlSearch.TabIndex = 1;
            // 
            // lblS
            // 
            lblS.AutoSize = true;
            lblS.Location = new Point(25, 28);
            lblS.Name = "lblS";
            lblS.Size = new Size(37, 20);
            lblS.TabIndex = 0;
            lblS.Text = "Tìm:";
            // 
            // frmDoctorBase
            // 
            ClientSize = new Size(322, 247);
            Controls.Add(Dgv);
            Controls.Add(pnlSearch);
            Name = "frmDoctorBase";
            ((System.ComponentModel.ISupportInitialize)Dgv).EndInit();
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        protected System.Windows.Forms.DataGridView Dgv;
        protected System.Windows.Forms.TextBox TxtS;
        protected System.Windows.Forms.Panel pnlSearch;
        protected System.Windows.Forms.Button btnS;
        protected System.Windows.Forms.Button btnA;
        protected System.Windows.Forms.Button btnE;
        protected System.Windows.Forms.Button btnD;
        private Label lblS;
    }
}
