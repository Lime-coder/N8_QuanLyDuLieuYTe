namespace QuanLyYTe.Forms
{
    partial class FrmPrivilege
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblSubtitle = new Label();
            lblTitle = new Label();
            pnlFilter = new Panel();
            rbByGrantee = new RadioButton();
            rbByObject = new RadioButton();
            pnlGrantee = new Panel();
            lblGrantee = new Label();
            cbGrantee = new ComboBox();
            pnlObject = new Panel();
            lblOwner = new Label();
            txtOwner = new TextBox();
            lblObject = new Label();
            txtObject = new TextBox();
            btnView = new Button();
            dgvPrivilege = new DataGridView();
            pnlFooter = new Panel();
            lblCount = new Label();
            btnRevoke = new Button();
            pnlHeader.SuspendLayout();
            pnlFilter.SuspendLayout();
            pnlGrantee.SuspendLayout();
            pnlObject.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPrivilege).BeginInit();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(30, 100, 160);
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1050, 60);
            pnlHeader.TabIndex = 3;
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 9F);
            lblSubtitle.ForeColor = Color.FromArgb(190, 220, 245);
            lblSubtitle.Location = new Point(18, 36);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(491, 20);
            lblSubtitle.TabIndex = 0;
            lblSubtitle.Text = "Xem & Thu hồi quyền của User / Role trên các đối tượng dữ liệu  (Câu 4 & 5)";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(16, 6);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(297, 35);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Quản lý Quyền Truy Cập";
            // 
            // pnlFilter
            // 
            pnlFilter.BackColor = Color.FromArgb(235, 240, 248);
            pnlFilter.Controls.Add(rbByGrantee);
            pnlFilter.Controls.Add(rbByObject);
            pnlFilter.Controls.Add(pnlGrantee);
            pnlFilter.Controls.Add(pnlObject);
            pnlFilter.Controls.Add(btnView);
            pnlFilter.Dock = DockStyle.Top;
            pnlFilter.Location = new Point(0, 60);
            pnlFilter.Name = "pnlFilter";
            pnlFilter.Size = new Size(1050, 90);
            pnlFilter.TabIndex = 2;
            // 
            // rbByGrantee
            // 
            rbByGrantee.AutoSize = true;
            rbByGrantee.Checked = true;
            rbByGrantee.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            rbByGrantee.ForeColor = Color.FromArgb(30, 30, 30);
            rbByGrantee.Location = new Point(16, 10);
            rbByGrantee.Name = "rbByGrantee";
            rbByGrantee.Size = new Size(163, 27);
            rbByGrantee.TabIndex = 0;
            rbByGrantee.TabStop = true;
            rbByGrantee.Text = "Theo User / Role";
            rbByGrantee.CheckedChanged += rbByGrantee_CheckedChanged;
            // 
            // rbByObject
            // 
            rbByObject.AutoSize = true;
            rbByObject.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            rbByObject.ForeColor = Color.FromArgb(30, 30, 30);
            rbByObject.Location = new Point(200, 10);
            rbByObject.Name = "rbByObject";
            rbByObject.Size = new Size(217, 27);
            rbByObject.TabIndex = 1;
            rbByObject.Text = "Theo đối tượng dữ liệu";
            rbByObject.CheckedChanged += rbByGrantee_CheckedChanged;
            // 
            // pnlGrantee
            // 
            pnlGrantee.BackColor = Color.Transparent;
            pnlGrantee.Controls.Add(lblGrantee);
            pnlGrantee.Controls.Add(cbGrantee);
            pnlGrantee.Location = new Point(16, 46);
            pnlGrantee.Name = "pnlGrantee";
            pnlGrantee.Size = new Size(450, 34);
            pnlGrantee.TabIndex = 2;
            // 
            // lblGrantee
            // 
            lblGrantee.AutoSize = true;
            lblGrantee.Font = new Font("Segoe UI", 10F);
            lblGrantee.Location = new Point(0, 6);
            lblGrantee.Name = "lblGrantee";
            lblGrantee.Size = new Size(98, 23);
            lblGrantee.TabIndex = 0;
            lblGrantee.Text = "User / Role:";
            // 
            // cbGrantee
            // 
            cbGrantee.DropDownStyle = ComboBoxStyle.DropDownList;
            cbGrantee.Font = new Font("Segoe UI", 10F);
            cbGrantee.Location = new Point(120, 3);
            cbGrantee.Name = "cbGrantee";
            cbGrantee.Size = new Size(330, 31);
            cbGrantee.TabIndex = 1;
            // 
            // pnlObject
            // 
            pnlObject.BackColor = Color.Transparent;
            pnlObject.Controls.Add(lblOwner);
            pnlObject.Controls.Add(txtOwner);
            pnlObject.Controls.Add(lblObject);
            pnlObject.Controls.Add(txtObject);
            pnlObject.Location = new Point(16, 46);
            pnlObject.Name = "pnlObject";
            pnlObject.Size = new Size(620, 34);
            pnlObject.TabIndex = 3;
            pnlObject.Visible = false;
            // 
            // lblOwner
            // 
            lblOwner.AutoSize = true;
            lblOwner.Font = new Font("Segoe UI", 10F);
            lblOwner.Location = new Point(0, 6);
            lblOwner.Name = "lblOwner";
            lblOwner.Size = new Size(74, 23);
            lblOwner.TabIndex = 0;
            lblOwner.Text = "Schema:";
            // 
            // txtOwner
            // 
            txtOwner.BorderStyle = BorderStyle.FixedSingle;
            txtOwner.Font = new Font("Segoe UI", 10F);
            txtOwner.Location = new Point(76, 2);
            txtOwner.Name = "txtOwner";
            txtOwner.Size = new Size(160, 30);
            txtOwner.TabIndex = 1;
            txtOwner.Text = "HOSPITAL_DBA";
            // 
            // lblObject
            // 
            lblObject.AutoSize = true;
            lblObject.Font = new Font("Segoe UI", 10F);
            lblObject.Location = new Point(254, 6);
            lblObject.Name = "lblObject";
            lblObject.Size = new Size(91, 23);
            lblObject.TabIndex = 2;
            lblObject.Text = "Đối tượng:";
            // 
            // txtObject
            // 
            txtObject.BorderStyle = BorderStyle.FixedSingle;
            txtObject.Font = new Font("Segoe UI", 10F);
            txtObject.Location = new Point(450, 2);
            txtObject.Name = "txtObject";
            txtObject.PlaceholderText = "vd: PATIENT";
            txtObject.Size = new Size(170, 30);
            txtObject.TabIndex = 3;
            // 
            // btnView
            // 
            btnView.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnView.BackColor = Color.FromArgb(30, 100, 160);
            btnView.Cursor = Cursors.Hand;
            btnView.FlatAppearance.BorderSize = 0;
            btnView.FlatStyle = FlatStyle.Flat;
            btnView.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnView.ForeColor = Color.White;
            btnView.Location = new Point(876, 44);
            btnView.Name = "btnView";
            btnView.Size = new Size(160, 38);
            btnView.TabIndex = 4;
            btnView.Text = "🔍  Xem quyền";
            btnView.UseVisualStyleBackColor = false;
            btnView.Click += btnView_Click;
            // 
            // dgvPrivilege
            // 
            dgvPrivilege.BorderStyle = BorderStyle.None;
            dgvPrivilege.ColumnHeadersHeight = 29;
            dgvPrivilege.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvPrivilege.Dock = DockStyle.Fill;
            dgvPrivilege.Location = new Point(0, 150);
            dgvPrivilege.Name = "dgvPrivilege";
            dgvPrivilege.RowHeadersWidth = 51;
            dgvPrivilege.Size = new Size(1050, 444);
            dgvPrivilege.TabIndex = 0;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(228, 232, 238);
            pnlFooter.Controls.Add(lblCount);
            pnlFooter.Controls.Add(btnRevoke);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 594);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(1050, 56);
            pnlFooter.TabIndex = 1;
            // 
            // lblCount
            // 
            lblCount.AutoSize = true;
            lblCount.Font = new Font("Segoe UI", 9.5F, FontStyle.Italic);
            lblCount.ForeColor = Color.FromArgb(80, 80, 80);
            lblCount.Location = new Point(16, 18);
            lblCount.Name = "lblCount";
            lblCount.Size = new Size(353, 21);
            lblCount.TabIndex = 0;
            lblCount.Text = "Chọn User/Role rồi bấm \"Xem quyền\" để bắt đầu.";
            // 
            // btnRevoke
            // 
            btnRevoke.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRevoke.BackColor = Color.FromArgb(192, 40, 30);
            btnRevoke.Cursor = Cursors.Hand;
            btnRevoke.FlatAppearance.BorderSize = 0;
            btnRevoke.FlatStyle = FlatStyle.Flat;
            btnRevoke.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRevoke.ForeColor = Color.White;
            btnRevoke.Location = new Point(776, 9);
            btnRevoke.Name = "btnRevoke";
            btnRevoke.Size = new Size(260, 38);
            btnRevoke.TabIndex = 1;
            btnRevoke.Text = "🚫  Thu hồi quyền đang chọn";
            btnRevoke.UseVisualStyleBackColor = false;
            btnRevoke.Click += btnRevoke_Click;
            // 
            // FrmPrivilege
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1050, 650);
            Controls.Add(dgvPrivilege);
            Controls.Add(pnlFooter);
            Controls.Add(pnlFilter);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(860, 500);
            Name = "FrmPrivilege";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý Quyền – Phân hệ 1  (Câu 4 & 5)";
            Load += FrmPrivilege_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlFilter.ResumeLayout(false);
            pnlFilter.PerformLayout();
            pnlGrantee.ResumeLayout(false);
            pnlGrantee.PerformLayout();
            pnlObject.ResumeLayout(false);
            pnlObject.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPrivilege).EndInit();
            pnlFooter.ResumeLayout(false);
            pnlFooter.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        // ── Field declarations ──────────────────────────────────────
        private System.Windows.Forms.Panel        pnlHeader;
        private System.Windows.Forms.Label        lblTitle;
        private System.Windows.Forms.Label        lblSubtitle;

        private System.Windows.Forms.Panel        pnlFilter;
        private System.Windows.Forms.RadioButton  rbByGrantee;
        private System.Windows.Forms.RadioButton  rbByObject;
        private System.Windows.Forms.Panel        pnlGrantee;
        private System.Windows.Forms.Label        lblGrantee;
        private System.Windows.Forms.ComboBox     cbGrantee;
        private System.Windows.Forms.Panel        pnlObject;
        private System.Windows.Forms.Label        lblOwner;
        private System.Windows.Forms.TextBox      txtOwner;
        private System.Windows.Forms.Label        lblObject;
        private System.Windows.Forms.TextBox      txtObject;
        private System.Windows.Forms.Button       btnView;

        private System.Windows.Forms.DataGridView dgvPrivilege;

        private System.Windows.Forms.Panel        pnlFooter;
        private System.Windows.Forms.Label        lblCount;
        private System.Windows.Forms.Button       btnRevoke;
    }
}