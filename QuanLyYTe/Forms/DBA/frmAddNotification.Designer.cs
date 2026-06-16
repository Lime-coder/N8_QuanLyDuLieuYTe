namespace QuanLyYTe.Forms.DBA
{
    partial class frmAddNotification
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlContainer;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.TextBox txtLocation;
        
        private System.Windows.Forms.GroupBox grpLevel;
        private System.Windows.Forms.RadioButton radBGD;
        private System.Windows.Forms.RadioButton radLDK;
        private System.Windows.Forms.RadioButton radNV;

        private System.Windows.Forms.GroupBox grpCompartments;
        private System.Windows.Forms.CheckBox chkTH;
        private System.Windows.Forms.CheckBox chkTK;
        private System.Windows.Forms.CheckBox chkTM;

        private System.Windows.Forms.GroupBox grpGroups;
        private System.Windows.Forms.CheckBox chkHCM;
        private System.Windows.Forms.CheckBox chkHN;
        private System.Windows.Forms.CheckBox chkHP;

        private System.Windows.Forms.RichTextBox rtbPreview;
        private System.Windows.Forms.Button btnSubmit;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            
            this.grpLevel = new System.Windows.Forms.GroupBox();
            this.radBGD = new System.Windows.Forms.RadioButton();
            this.radLDK = new System.Windows.Forms.RadioButton();
            this.radNV = new System.Windows.Forms.RadioButton();
            
            this.grpCompartments = new System.Windows.Forms.GroupBox();
            this.chkTH = new System.Windows.Forms.CheckBox();
            this.chkTK = new System.Windows.Forms.CheckBox();
            this.chkTM = new System.Windows.Forms.CheckBox();
            
            this.grpGroups = new System.Windows.Forms.GroupBox();
            this.chkHCM = new System.Windows.Forms.CheckBox();
            this.chkHN = new System.Windows.Forms.CheckBox();
            this.chkHP = new System.Windows.Forms.CheckBox();
            
            this.rtbPreview = new System.Windows.Forms.RichTextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            
            this.pnlContainer.SuspendLayout();
            this.grpLevel.SuspendLayout();
            this.grpCompartments.SuspendLayout();
            this.grpGroups.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // pnlContainer
            // 
            this.pnlContainer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlContainer.Controls.Add(this.btnSubmit);
            this.pnlContainer.Controls.Add(this.rtbPreview);
            this.pnlContainer.Controls.Add(this.grpGroups);
            this.pnlContainer.Controls.Add(this.grpCompartments);
            this.pnlContainer.Controls.Add(this.grpLevel);
            this.pnlContainer.Controls.Add(this.txtLocation);
            this.pnlContainer.Controls.Add(this.lblLocation);
            this.pnlContainer.Controls.Add(this.txtDescription);
            this.pnlContainer.Controls.Add(this.lblDescription);
            this.pnlContainer.Controls.Add(this.lblTitle);
            this.pnlContainer.Location = new System.Drawing.Point(20, 20);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(660, 650);
            this.pnlContainer.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(40)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(262, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Tạo thông báo OLS";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDescription.Location = new System.Drawing.Point(20, 60);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(86, 23);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "Nội dung:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(20, 90);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(620, 80);
            this.txtDescription.TabIndex = 2;
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblLocation.Location = new System.Drawing.Point(20, 180);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(83, 23);
            this.lblLocation.TabIndex = 3;
            this.lblLocation.Text = "Địa điểm:";
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(20, 210);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(620, 27);
            this.txtLocation.TabIndex = 4;
            // 
            // grpLevel
            // 
            this.grpLevel.Controls.Add(this.radNV);
            this.grpLevel.Controls.Add(this.radLDK);
            this.grpLevel.Controls.Add(this.radBGD);
            this.grpLevel.Location = new System.Drawing.Point(20, 260);
            this.grpLevel.Name = "grpLevel";
            this.grpLevel.Size = new System.Drawing.Size(620, 60);
            this.grpLevel.TabIndex = 5;
            this.grpLevel.TabStop = false;
            this.grpLevel.Text = "Cấp bậc nhận (Level)";
            // 
            // radBGD
            // 
            this.radBGD.AutoSize = true;
            this.radBGD.Location = new System.Drawing.Point(20, 25);
            this.radBGD.Name = "radBGD";
            this.radBGD.Size = new System.Drawing.Size(155, 24);
            this.radBGD.TabIndex = 0;
            this.radBGD.Text = "Ban Giám đốc (BGD)";
            this.radBGD.UseVisualStyleBackColor = true;
            // 
            // radLDK
            // 
            this.radLDK.AutoSize = true;
            this.radLDK.Location = new System.Drawing.Point(220, 25);
            this.radLDK.Name = "radLDK";
            this.radLDK.Size = new System.Drawing.Size(183, 24);
            this.radLDK.TabIndex = 1;
            this.radLDK.Text = "Lãnh đạo Khoa (LDK)";
            this.radLDK.UseVisualStyleBackColor = true;
            // 
            // radNV
            // 
            this.radNV.AutoSize = true;
            this.radNV.Checked = true;
            this.radNV.Location = new System.Drawing.Point(410, 25);
            this.radNV.Name = "radNV";
            this.radNV.Size = new System.Drawing.Size(150, 24);
            this.radNV.TabIndex = 2;
            this.radNV.TabStop = true;
            this.radNV.Text = "Toàn bộ Nhân viên (NV)";
            this.radNV.UseVisualStyleBackColor = true;
            // 
            // grpCompartments
            // 
            this.grpCompartments.Controls.Add(this.chkTM);
            this.grpCompartments.Controls.Add(this.chkTK);
            this.grpCompartments.Controls.Add(this.chkTH);
            this.grpCompartments.Location = new System.Drawing.Point(20, 330);
            this.grpCompartments.Name = "grpCompartments";
            this.grpCompartments.Size = new System.Drawing.Size(620, 60);
            this.grpCompartments.TabIndex = 6;
            this.grpCompartments.TabStop = false;
            this.grpCompartments.Text = "Phạm vi Khoa (Compartments)";
            // 
            // chkTH
            // 
            this.chkTH.AutoSize = true;
            this.chkTH.Location = new System.Drawing.Point(20, 25);
            this.chkTH.Name = "chkTH";
            this.chkTH.Size = new System.Drawing.Size(153, 24);
            this.chkTH.TabIndex = 0;
            this.chkTH.Text = "Khoa Tiêu hóa (TH)";
            this.chkTH.UseVisualStyleBackColor = true;
            // 
            // chkTK
            // 
            this.chkTK.AutoSize = true;
            this.chkTK.Location = new System.Drawing.Point(220, 25);
            this.chkTK.Name = "chkTK";
            this.chkTK.Size = new System.Drawing.Size(161, 24);
            this.chkTK.TabIndex = 1;
            this.chkTK.Text = "Khoa Thần kinh (TK)";
            this.chkTK.UseVisualStyleBackColor = true;
            // 
            // chkTM
            // 
            this.chkTM.AutoSize = true;
            this.chkTM.Location = new System.Drawing.Point(410, 25);
            this.chkTM.Name = "chkTM";
            this.chkTM.Size = new System.Drawing.Size(159, 24);
            this.chkTM.TabIndex = 2;
            this.chkTM.Text = "Khoa Tim mạch (TM)";
            this.chkTM.UseVisualStyleBackColor = true;
            // 
            // grpGroups
            // 
            this.grpGroups.Controls.Add(this.chkHP);
            this.grpGroups.Controls.Add(this.chkHN);
            this.grpGroups.Controls.Add(this.chkHCM);
            this.grpGroups.Location = new System.Drawing.Point(20, 400);
            this.grpGroups.Name = "grpGroups";
            this.grpGroups.Size = new System.Drawing.Size(620, 60);
            this.grpGroups.TabIndex = 7;
            this.grpGroups.TabStop = false;
            this.grpGroups.Text = "Phạm vi Cơ sở (Groups)";
            // 
            // chkHCM
            // 
            this.chkHCM.AutoSize = true;
            this.chkHCM.Location = new System.Drawing.Point(20, 25);
            this.chkHCM.Name = "chkHCM";
            this.chkHCM.Size = new System.Drawing.Size(162, 24);
            this.chkHCM.TabIndex = 0;
            this.chkHCM.Text = "Hồ Chí Minh (HCM)";
            this.chkHCM.UseVisualStyleBackColor = true;
            // 
            // chkHN
            // 
            this.chkHN.AutoSize = true;
            this.chkHN.Location = new System.Drawing.Point(220, 25);
            this.chkHN.Name = "chkHN";
            this.chkHN.Size = new System.Drawing.Size(110, 24);
            this.chkHN.TabIndex = 1;
            this.chkHN.Text = "Hà Nội (HN)";
            this.chkHN.UseVisualStyleBackColor = true;
            // 
            // chkHP
            // 
            this.chkHP.AutoSize = true;
            this.chkHP.Location = new System.Drawing.Point(410, 25);
            this.chkHP.Name = "chkHP";
            this.chkHP.Size = new System.Drawing.Size(126, 24);
            this.chkHP.TabIndex = 2;
            this.chkHP.Text = "Hải Phòng (HP)";
            this.chkHP.UseVisualStyleBackColor = true;
            // 
            // rtbPreview
            // 
            this.rtbPreview.BackColor = System.Drawing.Color.White;
            this.rtbPreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbPreview.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rtbPreview.ForeColor = System.Drawing.Color.DimGray;
            this.rtbPreview.Location = new System.Drawing.Point(20, 480);
            this.rtbPreview.Name = "rtbPreview";
            this.rtbPreview.ReadOnly = true;
            this.rtbPreview.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtbPreview.Size = new System.Drawing.Size(620, 45);
            this.rtbPreview.TabIndex = 8;
            this.rtbPreview.Text = "Preview";
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(40)))));
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.Location = new System.Drawing.Point(20, 530);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(200, 40);
            this.btnSubmit.TabIndex = 9;
            this.btnSubmit.Text = "Đăng thông báo";
            this.btnSubmit.UseVisualStyleBackColor = false;
            // 
            // frmAddNotification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(700, 700);
            this.Controls.Add(this.pnlContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmAddNotification";
            this.Text = "frmAddNotification";
            this.Resize += new System.EventHandler(this.frmAddNotification_Resize);
            
            this.pnlContainer.ResumeLayout(false);
            this.pnlContainer.PerformLayout();
            this.grpLevel.ResumeLayout(false);
            this.grpLevel.PerformLayout();
            this.grpCompartments.ResumeLayout(false);
            this.grpCompartments.PerformLayout();
            this.grpGroups.ResumeLayout(false);
            this.grpGroups.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
