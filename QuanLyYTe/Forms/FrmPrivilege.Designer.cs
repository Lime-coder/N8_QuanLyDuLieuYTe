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
            pnlTop = new Panel();
            gbSearch = new GroupBox();
            lblSearchType = new Label();
            cbSearchType = new ComboBox();
            lblSearchName = new Label();
            cbSearchName = new ComboBox();
            lblFilterType = new Label();
            cbFilterType = new ComboBox();
            btnView = new Button();
            btnRevoke = new Button();
            pnlSummary = new Panel();
            lblSummary = new Label();
            dgvPrivilege = new DataGridView();
            pnlTop.SuspendLayout();
            gbSearch.SuspendLayout();
            pnlSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPrivilege).BeginInit();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.White;
            pnlTop.Controls.Add(gbSearch);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(20, 10, 20, 10);
            pnlTop.Size = new Size(1050, 110);
            pnlTop.TabIndex = 2;
            // 
            // gbSearch
            // 
            gbSearch.Controls.Add(lblSearchType);
            gbSearch.Controls.Add(cbSearchType);
            gbSearch.Controls.Add(lblSearchName);
            gbSearch.Controls.Add(cbSearchName);
            gbSearch.Controls.Add(lblFilterType);
            gbSearch.Controls.Add(cbFilterType);
            gbSearch.Controls.Add(btnView);
            gbSearch.Controls.Add(btnRevoke);
            gbSearch.Dock = DockStyle.Fill;
            gbSearch.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            gbSearch.ForeColor = Color.FromArgb(100, 100, 110);
            gbSearch.Location = new Point(20, 10);
            gbSearch.Name = "gbSearch";
            gbSearch.Size = new Size(1010, 90);
            gbSearch.TabIndex = 0;
            gbSearch.TabStop = false;
            gbSearch.Text = "TÌM KIẾM";
            // 
            // lblSearchType
            // 
            lblSearchType.AutoSize = true;
            lblSearchType.Font = new Font("Segoe UI", 9.5F);
            lblSearchType.Location = new Point(20, 25);
            lblSearchType.Name = "lblSearchType";
            lblSearchType.Size = new Size(39, 21);
            lblSearchType.TabIndex = 0;
            lblSearchType.Text = "Loại";
            // 
            // cbSearchType
            // 
            cbSearchType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSearchType.Font = new Font("Segoe UI", 10F);
            cbSearchType.Items.AddRange(new object[] { "User", "Role", "Đối tượng" });
            cbSearchType.Location = new Point(20, 50);
            cbSearchType.Name = "cbSearchType";
            cbSearchType.Size = new Size(150, 31);
            cbSearchType.TabIndex = 1;
            cbSearchType.SelectedIndexChanged += cbSearchType_SelectedIndexChanged;
            // 
            // lblSearchName
            // 
            lblSearchName.AutoSize = true;
            lblSearchName.Font = new Font("Segoe UI", 9.5F);
            lblSearchName.Location = new Point(180, 25);
            lblSearchName.Name = "lblSearchName";
            lblSearchName.Size = new Size(114, 21);
            lblSearchName.TabIndex = 2;
            lblSearchName.Text = "Tên User / Role";
            // 
            // cbSearchName
            // 
            cbSearchName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbSearchName.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbSearchName.Font = new Font("Segoe UI", 10F);
            cbSearchName.Location = new Point(180, 50);
            cbSearchName.Name = "cbSearchName";
            cbSearchName.Size = new Size(240, 31);
            cbSearchName.TabIndex = 3;
            // 
            // lblFilterType
            // 
            lblFilterType.AutoSize = true;
            lblFilterType.Font = new Font("Segoe UI", 9.5F);
            lblFilterType.Location = new Point(430, 25);
            lblFilterType.Name = "lblFilterType";
            lblFilterType.Size = new Size(108, 21);
            lblFilterType.TabIndex = 4;
            lblFilterType.Text = "Bộ lọc hiển thị";
            // 
            // cbFilterType
            // 
            cbFilterType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterType.Font = new Font("Segoe UI", 10F);
            cbFilterType.Items.AddRange(new object[] { "Tất cả", "Quyền đối tượng", "Quyền theo cột", "Quyền hệ thống", "Role được cấp" });
            cbFilterType.Location = new Point(430, 50);
            cbFilterType.Name = "cbFilterType";
            cbFilterType.Size = new Size(200, 31);
            cbFilterType.TabIndex = 5;
            cbFilterType.SelectedIndexChanged += cbFilterType_SelectedIndexChanged;
            // 
            // btnView
            // 
            btnView.BackColor = Color.FromArgb(255, 140, 40);
            btnView.Cursor = Cursors.Hand;
            btnView.FlatAppearance.BorderSize = 0;
            btnView.FlatStyle = FlatStyle.Flat;
            btnView.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnView.ForeColor = Color.White;
            btnView.Location = new Point(640, 46);
            btnView.Name = "btnView";
            btnView.Size = new Size(130, 36);
            btnView.TabIndex = 6;
            btnView.Text = "Xem quyền";
            btnView.UseVisualStyleBackColor = false;
            btnView.Click += btnView_Click;
            // 
            // btnRevoke
            // 
            btnRevoke.BackColor = Color.FromArgb(220, 50, 60);
            btnRevoke.Cursor = Cursors.Hand;
            btnRevoke.FlatAppearance.BorderSize = 0;
            btnRevoke.FlatStyle = FlatStyle.Flat;
            btnRevoke.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRevoke.ForeColor = Color.White;
            btnRevoke.Location = new Point(780, 46);
            btnRevoke.Name = "btnRevoke";
            btnRevoke.Size = new Size(150, 36);
            btnRevoke.TabIndex = 7;
            btnRevoke.Text = "Thu hồi quyền";
            btnRevoke.UseVisualStyleBackColor = false;
            btnRevoke.Click += btnRevoke_Click;
            // 
            // pnlSummary
            // 
            pnlSummary.BackColor = Color.FromArgb(245, 246, 250);
            pnlSummary.Controls.Add(lblSummary);
            pnlSummary.Dock = DockStyle.Top;
            pnlSummary.Location = new Point(0, 110);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Padding = new Padding(20, 10, 0, 0);
            pnlSummary.Size = new Size(1050, 40);
            pnlSummary.TabIndex = 1;
            // 
            // lblSummary
            // 
            lblSummary.AutoSize = true;
            lblSummary.Dock = DockStyle.Left;
            lblSummary.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSummary.ForeColor = Color.FromArgb(255, 140, 40);
            lblSummary.Location = new Point(20, 10);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(378, 23);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "Hãy chọn đối tượng và bấm nút [Xem quyền].";
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
            dgvPrivilege.Size = new Size(1050, 550);
            dgvPrivilege.TabIndex = 0;
            // 
            // FrmPrivilege
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 246, 250);
            ClientSize = new Size(1050, 700);
            Controls.Add(dgvPrivilege);
            Controls.Add(pnlSummary);
            Controls.Add(pnlTop);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(950, 500);
            Name = "FrmPrivilege";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Phân công / Xem quyển";
            Load += FrmPrivilege_Load;
            pnlTop.ResumeLayout(false);
            gbSearch.ResumeLayout(false);
            gbSearch.PerformLayout();
            pnlSummary.ResumeLayout(false);
            pnlSummary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPrivilege).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.GroupBox gbSearch;
        
        private System.Windows.Forms.Label lblSearchType;
        private System.Windows.Forms.ComboBox cbSearchType;
        
        private System.Windows.Forms.Label lblSearchName;
        private System.Windows.Forms.ComboBox cbSearchName;
        
        private System.Windows.Forms.Label lblFilterType;
        private System.Windows.Forms.ComboBox cbFilterType;
        
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnRevoke;

        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.Label lblSummary;

        private System.Windows.Forms.DataGridView dgvPrivilege;
    }
}