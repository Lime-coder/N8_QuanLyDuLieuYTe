using System;
using System.Windows.Forms;

namespace QuanLyYTe.Forms.Doctor
{
    partial class frmDoctorProfile
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
            pnlCard = new Panel();
            lblLabelName = new Label();
            lblName = new Label();
            lblLabelGender = new Label();
            lblGender = new Label();
            lblLabelBirth = new Label();
            lblBirth = new Label();
            lblLabelCmnd = new Label();
            lblCmnd = new Label();
            lblLabelHome = new Label();
            lblHome = new Label();
            lblLabelPhone = new Label();
            lblPhone = new Label();
            lblLabelRole = new Label();
            lblRole = new Label();
            lblLabelSpec = new Label();
            lblSpec = new Label();
            title = new Label();
            pnlSearch.SuspendLayout();
            pnlCard.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSearch
            // 
            pnlSearch.Size = new Size(839, 75);
            // 
            // btnE
            // 
            btnE.BackColor = Color.Orange;
            btnE.Cursor = Cursors.Hand;
            btnE.FlatAppearance.BorderSize = 0;
            btnE.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnE.Location = new Point(40, 525);
            btnE.Size = new Size(220, 45);
            btnE.Text = "CHỈNH SỬA THÔNG TIN";
            // 
            // pnlCard
            // 
            pnlCard.BackColor = Color.White;
            pnlCard.Controls.Add(lblLabelName);
            pnlCard.Controls.Add(lblName);
            pnlCard.Controls.Add(lblLabelGender);
            pnlCard.Controls.Add(lblGender);
            pnlCard.Controls.Add(lblLabelBirth);
            pnlCard.Controls.Add(lblBirth);
            pnlCard.Controls.Add(lblLabelCmnd);
            pnlCard.Controls.Add(lblCmnd);
            pnlCard.Controls.Add(lblLabelHome);
            pnlCard.Controls.Add(lblHome);
            pnlCard.Controls.Add(lblLabelPhone);
            pnlCard.Controls.Add(lblPhone);
            pnlCard.Controls.Add(lblLabelRole);
            pnlCard.Controls.Add(lblRole);
            pnlCard.Controls.Add(lblLabelSpec);
            pnlCard.Controls.Add(lblSpec);
            pnlCard.Location = new Point(40, 85);
            pnlCard.Name = "pnlCard";
            pnlCard.Padding = new Padding(30);
            pnlCard.Size = new Size(700, 420);
            pnlCard.TabIndex = 3;
            pnlCard.Paint += pnlCard_Paint;
            // 
            // lblLabelName
            // 
            lblLabelName.AutoSize = true;
            lblLabelName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblLabelName.ForeColor = Color.DimGray;
            lblLabelName.Location = new Point(35, 35);
            lblLabelName.Name = "lblLabelName";
            lblLabelName.Size = new Size(92, 23);
            lblLabelName.TabIndex = 0;
            lblLabelName.Text = "Họ và tên:";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI", 11F);
            lblName.ForeColor = Color.Black;
            lblName.Location = new Point(190, 33);
            lblName.Name = "lblName";
            lblName.Size = new Size(24, 25);
            lblName.TabIndex = 1;
            lblName.Text = "...";
            // 
            // lblLabelGender
            // 
            lblLabelGender.AutoSize = true;
            lblLabelGender.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblLabelGender.ForeColor = Color.DimGray;
            lblLabelGender.Location = new Point(35, 77);
            lblLabelGender.Name = "lblLabelGender";
            lblLabelGender.Size = new Size(85, 23);
            lblLabelGender.TabIndex = 2;
            lblLabelGender.Text = "Giới tính:";
            // 
            // lblGender
            // 
            lblGender.AutoSize = true;
            lblGender.Font = new Font("Segoe UI", 11F);
            lblGender.ForeColor = Color.Black;
            lblGender.Location = new Point(190, 75);
            lblGender.Name = "lblGender";
            lblGender.Size = new Size(24, 25);
            lblGender.TabIndex = 3;
            lblGender.Text = "...";
            // 
            // lblLabelBirth
            // 
            lblLabelBirth.AutoSize = true;
            lblLabelBirth.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblLabelBirth.ForeColor = Color.DimGray;
            lblLabelBirth.Location = new Point(35, 119);
            lblLabelBirth.Name = "lblLabelBirth";
            lblLabelBirth.Size = new Size(94, 23);
            lblLabelBirth.TabIndex = 4;
            lblLabelBirth.Text = "Ngày sinh:";
            // 
            // lblBirth
            // 
            lblBirth.AutoSize = true;
            lblBirth.Font = new Font("Segoe UI", 11F);
            lblBirth.ForeColor = Color.Black;
            lblBirth.Location = new Point(190, 117);
            lblBirth.Name = "lblBirth";
            lblBirth.Size = new Size(24, 25);
            lblBirth.TabIndex = 5;
            lblBirth.Text = "...";
            // 
            // lblLabelCmnd
            // 
            lblLabelCmnd.AutoSize = true;
            lblLabelCmnd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblLabelCmnd.ForeColor = Color.DimGray;
            lblLabelCmnd.Location = new Point(35, 161);
            lblLabelCmnd.Name = "lblLabelCmnd";
            lblLabelCmnd.Size = new Size(120, 23);
            lblLabelCmnd.TabIndex = 6;
            lblLabelCmnd.Text = "CMND/CCCD:";
            // 
            // lblCmnd
            // 
            lblCmnd.AutoSize = true;
            lblCmnd.Font = new Font("Segoe UI", 11F);
            lblCmnd.ForeColor = Color.Black;
            lblCmnd.Location = new Point(190, 159);
            lblCmnd.Name = "lblCmnd";
            lblCmnd.Size = new Size(24, 25);
            lblCmnd.TabIndex = 7;
            lblCmnd.Text = "...";
            // 
            // lblLabelHome
            // 
            lblLabelHome.AutoSize = true;
            lblLabelHome.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblLabelHome.ForeColor = Color.DimGray;
            lblLabelHome.Location = new Point(35, 203);
            lblLabelHome.Name = "lblLabelHome";
            lblLabelHome.Size = new Size(92, 23);
            lblLabelHome.TabIndex = 8;
            lblLabelHome.Text = "Quê quán:";
            // 
            // lblHome
            // 
            lblHome.AutoSize = true;
            lblHome.Font = new Font("Segoe UI", 11F);
            lblHome.ForeColor = Color.Black;
            lblHome.Location = new Point(190, 201);
            lblHome.Name = "lblHome";
            lblHome.Size = new Size(24, 25);
            lblHome.TabIndex = 9;
            lblHome.Text = "...";
            // 
            // lblLabelPhone
            // 
            lblLabelPhone.AutoSize = true;
            lblLabelPhone.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblLabelPhone.ForeColor = Color.DimGray;
            lblLabelPhone.Location = new Point(35, 245);
            lblLabelPhone.Name = "lblLabelPhone";
            lblLabelPhone.Size = new Size(121, 23);
            lblLabelPhone.TabIndex = 10;
            lblLabelPhone.Text = "Số điện thoại:";
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.Font = new Font("Segoe UI", 11F);
            lblPhone.ForeColor = Color.Black;
            lblPhone.Location = new Point(190, 243);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(24, 25);
            lblPhone.TabIndex = 11;
            lblPhone.Text = "...";
            // 
            // lblLabelRole
            // 
            lblLabelRole.AutoSize = true;
            lblLabelRole.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblLabelRole.ForeColor = Color.DimGray;
            lblLabelRole.Location = new Point(35, 287);
            lblLabelRole.Name = "lblLabelRole";
            lblLabelRole.Size = new Size(68, 23);
            lblLabelRole.TabIndex = 12;
            lblLabelRole.Text = "Vai trò:";
            // 
            // lblRole
            // 
            lblRole.AutoSize = true;
            lblRole.Font = new Font("Segoe UI", 11F);
            lblRole.ForeColor = Color.Black;
            lblRole.Location = new Point(190, 285);
            lblRole.Name = "lblRole";
            lblRole.Size = new Size(24, 25);
            lblRole.TabIndex = 13;
            lblRole.Text = "...";
            // 
            // lblLabelSpec
            // 
            lblLabelSpec.AutoSize = true;
            lblLabelSpec.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblLabelSpec.ForeColor = Color.DimGray;
            lblLabelSpec.Location = new Point(35, 329);
            lblLabelSpec.Name = "lblLabelSpec";
            lblLabelSpec.Size = new Size(118, 23);
            lblLabelSpec.TabIndex = 14;
            lblLabelSpec.Text = "Chuyên khoa:";
            // 
            // lblSpec
            // 
            lblSpec.AutoSize = true;
            lblSpec.Font = new Font("Segoe UI", 11F);
            lblSpec.ForeColor = Color.Black;
            lblSpec.Location = new Point(190, 327);
            lblSpec.Name = "lblSpec";
            lblSpec.Size = new Size(24, 25);
            lblSpec.TabIndex = 15;
            lblSpec.Text = "...";
            // 
            // title
            // 
            title.AutoSize = true;
            title.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            title.ForeColor = Color.FromArgb(45, 45, 45);
            title.Location = new Point(40, 30);
            title.Name = "title";
            title.Size = new Size(362, 41);
            title.TabIndex = 2;
            title.Text = "HỒ SƠ CÁ NHÂN BÁC SĨ";
            // 
            // frmDoctorProfile
            // 
            BackColor = Color.FromArgb(245, 245, 250);
            ClientSize = new Size(839, 598);
            Controls.Add(title);
            Controls.Add(pnlCard);
            Controls.Add(btnE);
            Name = "frmDoctorProfile";
            Controls.SetChildIndex(btnE, 0);
            Controls.SetChildIndex(pnlCard, 0);
            Controls.SetChildIndex(title, 0);
            Controls.SetChildIndex(pnlSearch, 0);
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            pnlCard.ResumeLayout(false);
            pnlCard.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private Label lblLabelName;
        private Label lblLabelGender;
        private Label lblLabelBirth;
        private Label lblLabelCmnd;
        private Label lblLabelHome;
        private Label lblLabelPhone;
        private Label lblLabelRole;
        private Label lblLabelSpec;
        private Label title;
    }
}
