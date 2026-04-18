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
            // Search Box (Top)
            pnlTop = new System.Windows.Forms.Panel();
            gbSearch = new System.Windows.Forms.GroupBox();
            
            lblSearchType = new System.Windows.Forms.Label();
            cbSearchType = new System.Windows.Forms.ComboBox();

            lblSearchName = new System.Windows.Forms.Label();
            cbSearchName = new System.Windows.Forms.ComboBox();

            lblKeyword = new System.Windows.Forms.Label();
            txtKeyword = new System.Windows.Forms.TextBox();

            lblFilterType = new System.Windows.Forms.Label();
            cbFilterType = new System.Windows.Forms.ComboBox();

            btnView = new System.Windows.Forms.Button();
            btnRevoke = new System.Windows.Forms.Button();

            // Summary text
            pnlSummary = new System.Windows.Forms.Panel();
            lblSummary = new System.Windows.Forms.Label();

            // Datagrid
            dgvPrivilege = new System.Windows.Forms.DataGridView();

            ((System.ComponentModel.ISupportInitialize)dgvPrivilege).BeginInit();
            pnlTop.SuspendLayout();
            gbSearch.SuspendLayout();
            pnlSummary.SuspendLayout();
            SuspendLayout();

            // =============================================================
            // FORM
            // =============================================================
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1050, 700);
            MinimumSize = new System.Drawing.Size(1000, 500);
            Text = "Phân công / Xem quyển";
            Font = new System.Drawing.Font("Segoe UI", 10F);
            BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Load += FrmPrivilege_Load;

            // =============================================================
            // SEARCH BOX
            // =============================================================
            pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTop.Height = 110;
            pnlTop.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            pnlTop.BackColor = System.Drawing.Color.White;

            gbSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            gbSearch.Text = "TÌM KIẾM";
            gbSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            gbSearch.ForeColor = System.Drawing.Color.FromArgb(100, 100, 110);

            // -- Loại (Type) --
            lblSearchType.Text = "Loại";
            lblSearchType.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            lblSearchType.AutoSize = true;
            lblSearchType.Location = new System.Drawing.Point(20, 25);

            cbSearchType.Location = new System.Drawing.Point(20, 50);
            cbSearchType.Size = new System.Drawing.Size(120, 28);
            cbSearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbSearchType.Font = new System.Drawing.Font("Segoe UI", 10F);
            cbSearchType.Items.AddRange(new object[] { "User", "Role", "Đối tượng" });
            cbSearchType.SelectedIndexChanged += cbSearchType_SelectedIndexChanged;

            // -- Tên (Name) --
            lblSearchName.Text = "Tên User / Role";
            lblSearchName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            lblSearchName.AutoSize = true;
            lblSearchName.Location = new System.Drawing.Point(150, 25);

            cbSearchName.Location = new System.Drawing.Point(150, 50);
            cbSearchName.Size = new System.Drawing.Size(200, 28);
            cbSearchName.Font = new System.Drawing.Font("Segoe UI", 10F);
            cbSearchName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cbSearchName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cbSearchName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;

            // -- Từ khóa (Keyword Text Search) --
            lblKeyword.Text = "Từ khóa";
            lblKeyword.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            lblKeyword.AutoSize = true;
            lblKeyword.Location = new System.Drawing.Point(360, 25);

            txtKeyword.Location = new System.Drawing.Point(360, 50);
            txtKeyword.Size = new System.Drawing.Size(180, 28);
            txtKeyword.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtKeyword.TextChanged += txtKeyword_TextChanged;

            // -- BỘ LỌC (Filter) --
            lblFilterType.Text = "Bộ lọc hiển thị";
            lblFilterType.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            lblFilterType.AutoSize = true;
            lblFilterType.Location = new System.Drawing.Point(550, 25);

            cbFilterType.Location = new System.Drawing.Point(550, 50);
            cbFilterType.Size = new System.Drawing.Size(180, 28);
            cbFilterType.Font = new System.Drawing.Font("Segoe UI", 10F);
            cbFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbFilterType.Items.AddRange(new object[] {
                "Tất cả",
                "Quyền đối tượng",
                "Quyền theo cột",
                "Quyền hệ thống",
                "Role được cấp"
            });
            cbFilterType.SelectedIndexChanged += cbFilterType_SelectedIndexChanged;

            // -- View Button --
            btnView.Text = "Xem quyền";
            btnView.Size = new System.Drawing.Size(120, 36);
            btnView.Location = new System.Drawing.Point(740, 46);
            btnView.BackColor = System.Drawing.Color.FromArgb(255, 140, 40); // Orange Theme
            btnView.ForeColor = System.Drawing.Color.White;
            btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnView.FlatAppearance.BorderSize = 0;
            btnView.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnView.Cursor = System.Windows.Forms.Cursors.Hand;
            btnView.Click += btnView_Click;

            // -- Revoke Button --
            btnRevoke.Text = "Thu hồi quyền";
            btnRevoke.Size = new System.Drawing.Size(120, 36);
            btnRevoke.Location = new System.Drawing.Point(870, 46);
            btnRevoke.BackColor = System.Drawing.Color.FromArgb(220, 50, 60); // Red
            btnRevoke.ForeColor = System.Drawing.Color.White;
            btnRevoke.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRevoke.FlatAppearance.BorderSize = 0;
            btnRevoke.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnRevoke.Cursor = System.Windows.Forms.Cursors.Hand;
            btnRevoke.Click += btnRevoke_Click;

            gbSearch.Controls.Add(lblSearchType);
            gbSearch.Controls.Add(cbSearchType);
            gbSearch.Controls.Add(lblSearchName);
            gbSearch.Controls.Add(cbSearchName);
            gbSearch.Controls.Add(lblKeyword);
            gbSearch.Controls.Add(txtKeyword);
            gbSearch.Controls.Add(lblFilterType);
            gbSearch.Controls.Add(cbFilterType);
            gbSearch.Controls.Add(btnView);
            gbSearch.Controls.Add(btnRevoke);

            pnlTop.Controls.Add(gbSearch);

            // =============================================================
            // SUMMARY STATS
            // =============================================================
            pnlSummary.Dock = System.Windows.Forms.DockStyle.Top;
            pnlSummary.Height = 40;
            pnlSummary.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            pnlSummary.Padding = new System.Windows.Forms.Padding(20, 10, 0, 0);

            lblSummary.Text = "Hãy chọn đối tượng và bấm nút [Xem quyền].";
            lblSummary.ForeColor = System.Drawing.Color.FromArgb(255, 140, 40); // Orange color
            lblSummary.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblSummary.AutoSize = true;
            lblSummary.Dock = System.Windows.Forms.DockStyle.Left;

            pnlSummary.Controls.Add(lblSummary);

            // =============================================================
            // DATAGRIDVIEW
            // =============================================================
            dgvPrivilege.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvPrivilege.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvPrivilege.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Add constraints ordering
            Controls.Add(dgvPrivilege); // Center
            Controls.Add(pnlSummary);   // Top
            Controls.Add(pnlTop);       // Top

            ((System.ComponentModel.ISupportInitialize)dgvPrivilege).EndInit();
            pnlTop.ResumeLayout(false);
            gbSearch.ResumeLayout(false);
            gbSearch.PerformLayout();
            pnlSummary.ResumeLayout(false);
            pnlSummary.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.GroupBox gbSearch;
        
        private System.Windows.Forms.Label lblSearchType;
        private System.Windows.Forms.ComboBox cbSearchType;
        
        private System.Windows.Forms.Label lblSearchName;
        private System.Windows.Forms.ComboBox cbSearchName;

        private System.Windows.Forms.Label lblKeyword;
        private System.Windows.Forms.TextBox txtKeyword;
        
        private System.Windows.Forms.Label lblFilterType;
        private System.Windows.Forms.ComboBox cbFilterType;
        
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnRevoke;

        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.Label lblSummary;

        private System.Windows.Forms.DataGridView dgvPrivilege;
    }
}