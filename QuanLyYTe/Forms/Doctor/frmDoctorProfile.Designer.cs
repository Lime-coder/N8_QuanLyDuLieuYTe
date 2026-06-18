using System;
using System.Windows.Forms;

namespace QuanLyYTe.Forms.Doctor
{
    partial class frmDoctorProfile
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
            this.pnlCard = new System.Windows.Forms.Panel();
            this.lblName = new System.Windows.Forms.Label();
            this.lblGender = new System.Windows.Forms.Label();
            this.lblBirth = new System.Windows.Forms.Label();
            this.lblCmnd = new System.Windows.Forms.Label();
            this.lblHome = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblRole = new System.Windows.Forms.Label();
            this.lblSpec = new System.Windows.Forms.Label();
            System.Windows.Forms.Label title = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lblLabelName = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lblLabelGender = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lblLabelBirth = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lblLabelCmnd = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lblLabelHome = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lblLabelPhone = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lblLabelRole = new System.Windows.Forms.Label();
            System.Windows.Forms.Label lblLabelSpec = new System.Windows.Forms.Label();
            this.pnlCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // Dgv and pnlSearch removal (inherited from frmDoctorBase)
            // 
            this.Controls.Remove(this.Dgv);
            this.Controls.Remove(this.pnlSearch);
            this.BackColor = System.Drawing.Color.FromArgb(245, 245, 250);
            // 
            // title
            // 
            title.Text = "HỒ SƠ CÁ NHÂN BÁC SĨ";
            title.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            title.Location = new System.Drawing.Point(40, 30);
            title.AutoSize = true;
            title.ForeColor = System.Drawing.Color.FromArgb(45, 45, 45);
            // 
            // pnlCard
            // 
            this.pnlCard.Location = new System.Drawing.Point(40, 85);
            this.pnlCard.Size = new System.Drawing.Size(700, 420);
            this.pnlCard.BackColor = System.Drawing.Color.White;
            this.pnlCard.Padding = new System.Windows.Forms.Padding(30);
            this.pnlCard.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCard_Paint);
            // 
            // lblLabelName
            // 
            lblLabelName.Text = "Họ và tên:";
            lblLabelName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblLabelName.Location = new System.Drawing.Point(35, 35);
            lblLabelName.ForeColor = System.Drawing.Color.DimGray;
            lblLabelName.AutoSize = true;
            // 
            // lblName
            // 
            this.lblName.Text = "...";
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular);
            this.lblName.Location = new System.Drawing.Point(190, 33);
            this.lblName.ForeColor = System.Drawing.Color.Black;
            this.lblName.AutoSize = true;
            // 
            // lblLabelGender
            // 
            lblLabelGender.Text = "Giới tính:";
            lblLabelGender.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblLabelGender.Location = new System.Drawing.Point(35, 77);
            lblLabelGender.ForeColor = System.Drawing.Color.DimGray;
            lblLabelGender.AutoSize = true;
            // 
            // lblGender
            // 
            this.lblGender.Text = "...";
            this.lblGender.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular);
            this.lblGender.Location = new System.Drawing.Point(190, 75);
            this.lblGender.ForeColor = System.Drawing.Color.Black;
            this.lblGender.AutoSize = true;
            // 
            // lblLabelBirth
            // 
            lblLabelBirth.Text = "Ngày sinh:";
            lblLabelBirth.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblLabelBirth.Location = new System.Drawing.Point(35, 119);
            lblLabelBirth.ForeColor = System.Drawing.Color.DimGray;
            lblLabelBirth.AutoSize = true;
            // 
            // lblBirth
            // 
            this.lblBirth.Text = "...";
            this.lblBirth.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular);
            this.lblBirth.Location = new System.Drawing.Point(190, 117);
            this.lblBirth.ForeColor = System.Drawing.Color.Black;
            this.lblBirth.AutoSize = true;
            // 
            // lblLabelCmnd
            // 
            lblLabelCmnd.Text = "CMND/CCCD:";
            lblLabelCmnd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblLabelCmnd.Location = new System.Drawing.Point(35, 161);
            lblLabelCmnd.ForeColor = System.Drawing.Color.DimGray;
            lblLabelCmnd.AutoSize = true;
            // 
            // lblCmnd
            // 
            this.lblCmnd.Text = "...";
            this.lblCmnd.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular);
            this.lblCmnd.Location = new System.Drawing.Point(190, 159);
            this.lblCmnd.ForeColor = System.Drawing.Color.Black;
            this.lblCmnd.AutoSize = true;
            // 
            // lblLabelHome
            // 
            lblLabelHome.Text = "Quê quán:";
            lblLabelHome.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblLabelHome.Location = new System.Drawing.Point(35, 203);
            lblLabelHome.ForeColor = System.Drawing.Color.DimGray;
            lblLabelHome.AutoSize = true;
            // 
            // lblHome
            // 
            this.lblHome.Text = "...";
            this.lblHome.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular);
            this.lblHome.Location = new System.Drawing.Point(190, 201);
            this.lblHome.ForeColor = System.Drawing.Color.Black;
            this.lblHome.AutoSize = true;
            // 
            // lblLabelPhone
            // 
            lblLabelPhone.Text = "Số điện thoại:";
            lblLabelPhone.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblLabelPhone.Location = new System.Drawing.Point(35, 245);
            lblLabelPhone.ForeColor = System.Drawing.Color.DimGray;
            lblLabelPhone.AutoSize = true;
            // 
            // lblPhone
            // 
            this.lblPhone.Text = "...";
            this.lblPhone.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular);
            this.lblPhone.Location = new System.Drawing.Point(190, 243);
            this.lblPhone.ForeColor = System.Drawing.Color.Black;
            this.lblPhone.AutoSize = true;
            // 
            // lblLabelRole
            // 
            lblLabelRole.Text = "Vai trò:";
            lblLabelRole.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblLabelRole.Location = new System.Drawing.Point(35, 287);
            lblLabelRole.ForeColor = System.Drawing.Color.DimGray;
            lblLabelRole.AutoSize = true;
            // 
            // lblRole
            // 
            this.lblRole.Text = "...";
            this.lblRole.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular);
            this.lblRole.Location = new System.Drawing.Point(190, 285);
            this.lblRole.ForeColor = System.Drawing.Color.Black;
            this.lblRole.AutoSize = true;
            // 
            // lblLabelSpec
            // 
            lblLabelSpec.Text = "Chuyên khoa:";
            lblLabelSpec.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblLabelSpec.Location = new System.Drawing.Point(35, 329);
            lblLabelSpec.ForeColor = System.Drawing.Color.DimGray;
            lblLabelSpec.AutoSize = true;
            // 
            // lblSpec
            // 
            this.lblSpec.Text = "...";
            this.lblSpec.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular);
            this.lblSpec.Location = new System.Drawing.Point(190, 327);
            this.lblSpec.ForeColor = System.Drawing.Color.Black;
            this.lblSpec.AutoSize = true;
            // 
            // btnE (inherited from frmDoctorBase)
            // 
            this.btnE.Text = "CHỈNH SỬA THÔNG TIN";
            this.btnE.Location = new System.Drawing.Point(40, 525);
            this.btnE.Size = new System.Drawing.Size(220, 45);
            this.btnE.BackColor = System.Drawing.Color.Orange;
            this.btnE.ForeColor = System.Drawing.Color.White;
            this.btnE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnE.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnE.FlatAppearance.BorderSize = 0;
            this.btnE.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnE.Visible = true;
            // 
            // pnlCard Controls addition
            // 
            this.pnlCard.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblLabelName, this.lblName,
                lblLabelGender, this.lblGender,
                lblLabelBirth, this.lblBirth,
                lblLabelCmnd, this.lblCmnd,
                lblLabelHome, this.lblHome,
                lblLabelPhone, this.lblPhone,
                lblLabelRole, this.lblRole,
                lblLabelSpec, this.lblSpec
            });
            // 
            // frmDoctorProfile
            // 
            this.Controls.AddRange(new System.Windows.Forms.Control[] { title, this.pnlCard, this.btnE });
            this.pnlCard.ResumeLayout(false);
            this.pnlCard.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlCard;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.Label lblBirth;
        private System.Windows.Forms.Label lblCmnd;
        private System.Windows.Forms.Label lblHome;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.Label lblSpec;
    }
}
