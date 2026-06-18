using System;
using System.Windows.Forms;

namespace QuanLyYTe.Forms.Doctor
{
    partial class frmMedicalRecordManagement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvSub = new System.Windows.Forms.DataGridView();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.split = new System.Windows.Forms.SplitContainer();
            this.pnlSubAction = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.split)).BeginInit();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.pnlSubAction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSub)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSub
            // 
            this.dgvSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSub.BackgroundColor = System.Drawing.Color.White;
            this.dgvSub.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSub.AllowUserToAddRows = false;
            this.dgvSub.ReadOnly = true;
            this.dgvSub.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.Text = "▶ CHI TIẾT DỊCH VỤ CẬN LÂM SÀNG";
            this.lblSubTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSubTitle.Width = 300;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSubTitle.ForeColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lblSubTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSubTitle.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            // 
            // split
            // 
            this.split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.split.SplitterDistance = 450;
            this.split.SplitterWidth = 5;
            this.split.BackColor = System.Drawing.Color.FromArgb(200, 200, 200);
            // 
            // split.Panel1 Controls removal and addition
            // 
            this.Controls.Remove(this.Dgv);
            this.split.Panel1.Controls.Add(this.Dgv);
            // 
            // pnlSubAction
            // 
            this.pnlSubAction.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSubAction.Height = 40;
            this.pnlSubAction.BackColor = System.Drawing.Color.FromArgb(235, 235, 235);
            // 
            // pnlSubAction Controls
            // 
            this.pnlSubAction.Controls.AddRange(new System.Windows.Forms.Control[] { this.lblSubTitle });
            // 
            // split.Panel2 Controls
            // 
            this.split.Panel2.Controls.AddRange(new System.Windows.Forms.Control[] { this.dgvSub, this.pnlSubAction });
            // 
            // frmMedicalRecordManagement
            // 
            this.Controls.Add(this.split);
            // 
            // layout cleanup
            // 
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split)).EndInit();
            this.split.ResumeLayout(false);
            this.pnlSubAction.ResumeLayout(false);
            this.pnlSubAction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSub)).EndInit();
            this.ResumeLayout(false);
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
