namespace QuanLyYTe.Forms
{
    partial class FrmPrivilege
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
            cbUser = new ComboBox();
            btnLoad = new Button();
            dgvPrivilege = new DataGridView();
            btnRevoke = new Button();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvPrivilege).BeginInit();
            SuspendLayout();
            // 
            // cbUser
            // 
            cbUser.FormattingEnabled = true;
            cbUser.Location = new Point(178, 22);
            cbUser.Name = "cbUser";
            cbUser.Size = new Size(151, 28);
            cbUser.TabIndex = 0;
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(374, 22);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(94, 29);
            btnLoad.TabIndex = 1;
            btnLoad.Text = "Xem quyền";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // dgvPrivilege
            // 
            dgvPrivilege.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPrivilege.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPrivilege.Location = new Point(58, 91);
            dgvPrivilege.Name = "dgvPrivilege";
            dgvPrivilege.RowHeadersWidth = 51;
            dgvPrivilege.Size = new Size(661, 277);
            dgvPrivilege.TabIndex = 2;
            dgvPrivilege.CellContentClick += dgvPrivilege_CellContentClick;
            // 
            // btnRevoke
            // 
            btnRevoke.Location = new Point(674, 389);
            btnRevoke.Name = "btnRevoke";
            btnRevoke.Size = new Size(94, 29);
            btnRevoke.TabIndex = 3;
            btnRevoke.Text = "Thu hồi";
            btnRevoke.UseVisualStyleBackColor = true;
            btnRevoke.Click += btnRevoke_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(118, 30);
            label1.Name = "label1";
            label1.Size = new Size(41, 20);
            label1.TabIndex = 4;
            label1.Text = "User:";
            label1.Click += label1_Click;
            // 
            // FrmPrivilege
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(btnRevoke);
            Controls.Add(dgvPrivilege);
            Controls.Add(btnLoad);
            Controls.Add(cbUser);
            Name = "FrmPrivilege";
            Text = "FrmPrivilege";
            Load += FrmPrivilege_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPrivilege).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cbUser;
        private Button btnLoad;
        private DataGridView dgvPrivilege;
        private Button btnRevoke;
        private Label label1;
    }
}