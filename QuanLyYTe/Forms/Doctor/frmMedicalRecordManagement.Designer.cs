using System;
using System.Windows.Forms;

namespace QuanLyYTe.Forms.Doctor
{
    partial class frmMedicalRecordManagement
    {
        // Required designer variable.
        private new System.ComponentModel.IContainer components = null;
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
            dgvSub = new DataGridView();
            lblSubTitle = new Label();
            split = new SplitContainer();
            pnlSubAction = new Panel();
            pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSub).BeginInit();
            ((System.ComponentModel.ISupportInitialize)split).BeginInit();
            split.Panel2.SuspendLayout();
            split.SuspendLayout();
            pnlSubAction.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSearch
            // 
            pnlSearch.Size = new Size(922, 75);
            // 
            // dgvSub
            // 
            dgvSub.AllowUserToAddRows = false;
            dgvSub.BackgroundColor = Color.White;
            dgvSub.BorderStyle = BorderStyle.None;
            dgvSub.ColumnHeadersHeight = 29;
            dgvSub.Dock = DockStyle.Fill;
            dgvSub.Location = new Point(0, 40);
            dgvSub.Name = "dgvSub";
            dgvSub.ReadOnly = true;
            dgvSub.RowHeadersWidth = 51;
            dgvSub.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSub.Size = new Size(922, 88);
            dgvSub.TabIndex = 0;
            // 
            // lblSubTitle
            // 
            lblSubTitle.Dock = DockStyle.Left;
            lblSubTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSubTitle.ForeColor = Color.FromArgb(0, 120, 215);
            lblSubTitle.Location = new Point(0, 0);
            lblSubTitle.Name = "lblSubTitle";
            lblSubTitle.Padding = new Padding(10, 0, 0, 0);
            lblSubTitle.Size = new Size(300, 40);
            lblSubTitle.TabIndex = 0;
            lblSubTitle.Text = "▶ CHI TIẾT DỊCH VỤ CẬN LÂM SÀNG";
            lblSubTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // split
            // 
            split.BackColor = Color.FromArgb(200, 200, 200);
            split.Dock = DockStyle.Fill;
            split.Location = new Point(0, 0);
            split.Name = "split";
            split.Orientation = Orientation.Horizontal;
            // 
            // split.Panel2
            // 
            split.Panel2.Controls.Add(dgvSub);
            split.Panel2.Controls.Add(pnlSubAction);
            split.Size = new Size(922, 457);
            split.SplitterDistance = 324;
            split.SplitterWidth = 5;
            split.TabIndex = 2;
            // 
            // pnlSubAction
            // 
            pnlSubAction.BackColor = Color.FromArgb(235, 235, 235);
            pnlSubAction.Controls.Add(lblSubTitle);
            pnlSubAction.Dock = DockStyle.Top;
            pnlSubAction.Location = new Point(0, 0);
            pnlSubAction.Name = "pnlSubAction";
            pnlSubAction.Size = new Size(922, 40);
            pnlSubAction.TabIndex = 1;
            // 
            // frmMedicalRecordManagement
            // 
            ClientSize = new Size(922, 457);
            Controls.Add(split);
            Name = "frmMedicalRecordManagement";
            Controls.SetChildIndex(split, 0);
            Controls.SetChildIndex(pnlSearch, 0);
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSub).EndInit();
            split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)split).EndInit();
            split.ResumeLayout(false);
            pnlSubAction.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSub;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.SplitContainer split;
        private System.Windows.Forms.Panel pnlSubAction;
        private System.Windows.Forms.Button btnSubAdd;
        private System.Windows.Forms.Button btnSubDel;
    }
}
