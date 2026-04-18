namespace QuanLyYTe.Forms
{
    partial class SecurityAdminForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabUsers = new TabPage();
            dgvUsers = new DataGridView();
            panelUsers = new Panel();
            btnUserRefresh = new Button();
            lblUserSearch = new Label();
            txtUserSearch = new TextBox();
            btnUserCreate = new Button();
            btnUserEdit = new Button();
            btnUserDelete = new Button();
            btnUserLock = new Button();
            btnUserUnlock = new Button();
            tabRoles = new TabPage();
            dgvRoles = new DataGridView();
            panelRoles = new Panel();
            btnRoleRefresh = new Button();
            lblRoleSearch = new Label();
            txtRoleSearch = new TextBox();
            btnRoleCreate = new Button();
            btnRoleEdit = new Button();
            btnRoleDelete = new Button();
            tabControl1.SuspendLayout();
            tabUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            panelUsers.SuspendLayout();
            tabRoles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRoles).BeginInit();
            panelRoles.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabUsers);
            tabControl1.Controls.Add(tabRoles);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1100, 650);
            tabControl1.TabIndex = 0;
            // 
            // tabUsers
            // 
            tabUsers.Controls.Add(dgvUsers);
            tabUsers.Controls.Add(panelUsers);
            tabUsers.Location = new Point(4, 29);
            tabUsers.Name = "tabUsers";
            tabUsers.Padding = new Padding(3);
            tabUsers.Size = new Size(1092, 617);
            tabUsers.TabIndex = 0;
            tabUsers.Text = "Danh sách user";
            tabUsers.UseVisualStyleBackColor = true;
            // 
            // dgvUsers
            // 
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsers.Dock = DockStyle.Fill;
            dgvUsers.Location = new Point(3, 58);
            dgvUsers.MultiSelect = false;
            dgvUsers.Name = "dgvUsers";
            dgvUsers.ReadOnly = true;
            dgvUsers.RowHeadersWidth = 51;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.Size = new Size(1086, 556);
            dgvUsers.TabIndex = 1;
            dgvUsers.CellDoubleClick += dgvUsers_CellDoubleClick;
            dgvUsers.SelectionChanged += dgvUsers_SelectionChanged;
            // 
            // panelUsers
            // 
            panelUsers.Controls.Add(btnUserUnlock);
            panelUsers.Controls.Add(btnUserLock);
            panelUsers.Controls.Add(btnUserDelete);
            panelUsers.Controls.Add(btnUserEdit);
            panelUsers.Controls.Add(btnUserCreate);
            panelUsers.Controls.Add(txtUserSearch);
            panelUsers.Controls.Add(lblUserSearch);
            panelUsers.Controls.Add(btnUserRefresh);
            panelUsers.Dock = DockStyle.Top;
            panelUsers.Location = new Point(3, 3);
            panelUsers.Name = "panelUsers";
            panelUsers.Padding = new Padding(8);
            panelUsers.Size = new Size(1086, 55);
            panelUsers.TabIndex = 0;
            // 
            // btnUserRefresh
            // 
            btnUserRefresh.Location = new Point(327, 11);
            btnUserRefresh.Name = "btnUserRefresh";
            btnUserRefresh.Size = new Size(120, 32);
            btnUserRefresh.TabIndex = 3;
            btnUserRefresh.Text = "Làm mới";
            btnUserRefresh.UseVisualStyleBackColor = true;
            btnUserRefresh.Click += btnUserRefresh_Click;
            // 
            // lblUserSearch
            // 
            lblUserSearch.AutoSize = true;
            lblUserSearch.Location = new Point(11, 16);
            lblUserSearch.Name = "lblUserSearch";
            lblUserSearch.Size = new Size(55, 20);
            lblUserSearch.TabIndex = 0;
            lblUserSearch.Text = "Tìm:";
            // 
            // txtUserSearch
            // 
            txtUserSearch.Location = new Point(72, 12);
            txtUserSearch.Name = "txtUserSearch";
            txtUserSearch.PlaceholderText = "Nhập tên user...";
            txtUserSearch.Size = new Size(249, 27);
            txtUserSearch.TabIndex = 1;
            txtUserSearch.TextChanged += txtUserSearch_TextChanged;
            // 
            // btnUserCreate
            // 
            btnUserCreate.Location = new Point(453, 11);
            btnUserCreate.Name = "btnUserCreate";
            btnUserCreate.Size = new Size(120, 32);
            btnUserCreate.TabIndex = 4;
            btnUserCreate.Text = "Tạo";
            btnUserCreate.UseVisualStyleBackColor = true;
            btnUserCreate.Click += btnUserCreate_Click;
            // 
            // btnUserEdit
            // 
            btnUserEdit.Location = new Point(579, 11);
            btnUserEdit.Name = "btnUserEdit";
            btnUserEdit.Size = new Size(120, 32);
            btnUserEdit.TabIndex = 5;
            btnUserEdit.Text = "Sửa";
            btnUserEdit.UseVisualStyleBackColor = true;
            btnUserEdit.Click += btnUserEdit_Click;
            // 
            // btnUserDelete
            // 
            btnUserDelete.Location = new Point(705, 11);
            btnUserDelete.Name = "btnUserDelete";
            btnUserDelete.Size = new Size(120, 32);
            btnUserDelete.TabIndex = 6;
            btnUserDelete.Text = "Xóa";
            btnUserDelete.UseVisualStyleBackColor = true;
            btnUserDelete.Click += btnUserDelete_Click;
            // 
            // btnUserLock
            // 
            btnUserLock.Location = new Point(831, 11);
            btnUserLock.Name = "btnUserLock";
            btnUserLock.Size = new Size(120, 32);
            btnUserLock.TabIndex = 7;
            btnUserLock.Text = "Khóa";
            btnUserLock.UseVisualStyleBackColor = true;
            btnUserLock.Click += btnUserLock_Click;
            // 
            // btnUserUnlock
            // 
            btnUserUnlock.Location = new Point(957, 11);
            btnUserUnlock.Name = "btnUserUnlock";
            btnUserUnlock.Size = new Size(120, 32);
            btnUserUnlock.TabIndex = 8;
            btnUserUnlock.Text = "Mở khóa";
            btnUserUnlock.UseVisualStyleBackColor = true;
            btnUserUnlock.Click += btnUserUnlock_Click;
            // 
            // tabRoles
            // 
            tabRoles.Controls.Add(dgvRoles);
            tabRoles.Controls.Add(panelRoles);
            tabRoles.Location = new Point(4, 29);
            tabRoles.Name = "tabRoles";
            tabRoles.Padding = new Padding(3);
            tabRoles.Size = new Size(1092, 617);
            tabRoles.TabIndex = 1;
            tabRoles.Text = "Danh sách role";
            tabRoles.UseVisualStyleBackColor = true;
            // 
            // dgvRoles
            // 
            dgvRoles.AllowUserToAddRows = false;
            dgvRoles.AllowUserToDeleteRows = false;
            dgvRoles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRoles.Dock = DockStyle.Fill;
            dgvRoles.Location = new Point(3, 58);
            dgvRoles.MultiSelect = false;
            dgvRoles.Name = "dgvRoles";
            dgvRoles.ReadOnly = true;
            dgvRoles.RowHeadersWidth = 51;
            dgvRoles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRoles.Size = new Size(1086, 556);
            dgvRoles.TabIndex = 1;
            dgvRoles.CellDoubleClick += dgvRoles_CellDoubleClick;
            dgvRoles.SelectionChanged += dgvRoles_SelectionChanged;
            // 
            // panelRoles
            // 
            panelRoles.Controls.Add(btnRoleDelete);
            panelRoles.Controls.Add(btnRoleEdit);
            panelRoles.Controls.Add(btnRoleCreate);
            panelRoles.Controls.Add(txtRoleSearch);
            panelRoles.Controls.Add(lblRoleSearch);
            panelRoles.Controls.Add(btnRoleRefresh);
            panelRoles.Dock = DockStyle.Top;
            panelRoles.Location = new Point(3, 3);
            panelRoles.Name = "panelRoles";
            panelRoles.Padding = new Padding(8);
            panelRoles.Size = new Size(1086, 55);
            panelRoles.TabIndex = 0;
            // 
            // btnRoleRefresh
            // 
            btnRoleRefresh.Location = new Point(388, 11);
            btnRoleRefresh.Name = "btnRoleRefresh";
            btnRoleRefresh.Size = new Size(120, 32);
            btnRoleRefresh.TabIndex = 3;
            btnRoleRefresh.Text = "Làm mới";
            btnRoleRefresh.UseVisualStyleBackColor = true;
            btnRoleRefresh.Click += btnRoleRefresh_Click;
            // 
            // lblRoleSearch
            // 
            lblRoleSearch.AutoSize = true;
            lblRoleSearch.Location = new Point(11, 16);
            lblRoleSearch.Name = "lblRoleSearch";
            lblRoleSearch.Size = new Size(55, 20);
            lblRoleSearch.TabIndex = 0;
            lblRoleSearch.Text = "Tìm:";
            // 
            // txtRoleSearch
            // 
            txtRoleSearch.Location = new Point(72, 12);
            txtRoleSearch.Name = "txtRoleSearch";
            txtRoleSearch.PlaceholderText = "Nhập tên role...";
            txtRoleSearch.Size = new Size(310, 27);
            txtRoleSearch.TabIndex = 1;
            txtRoleSearch.TextChanged += txtRoleSearch_TextChanged;
            // 
            // btnRoleCreate
            // 
            btnRoleCreate.Location = new Point(514, 11);
            btnRoleCreate.Name = "btnRoleCreate";
            btnRoleCreate.Size = new Size(120, 32);
            btnRoleCreate.TabIndex = 4;
            btnRoleCreate.Text = "Tạo";
            btnRoleCreate.UseVisualStyleBackColor = true;
            btnRoleCreate.Click += btnRoleCreate_Click;
            // 
            // btnRoleEdit
            // 
            btnRoleEdit.Location = new Point(640, 11);
            btnRoleEdit.Name = "btnRoleEdit";
            btnRoleEdit.Size = new Size(120, 32);
            btnRoleEdit.TabIndex = 5;
            btnRoleEdit.Text = "Sửa";
            btnRoleEdit.UseVisualStyleBackColor = true;
            btnRoleEdit.Click += btnRoleEdit_Click;
            // 
            // btnRoleDelete
            // 
            btnRoleDelete.Location = new Point(766, 11);
            btnRoleDelete.Name = "btnRoleDelete";
            btnRoleDelete.Size = new Size(120, 32);
            btnRoleDelete.TabIndex = 6;
            btnRoleDelete.Text = "Xóa";
            btnRoleDelete.UseVisualStyleBackColor = true;
            btnRoleDelete.Click += btnRoleDelete_Click;
            // 
            // SecurityAdminForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 650);
            Controls.Add(tabControl1);
            Name = "SecurityAdminForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản trị bảo mật Oracle";
            Load += SecurityAdminForm_Load;
            tabControl1.ResumeLayout(false);
            tabUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            panelUsers.ResumeLayout(false);
            tabRoles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvRoles).EndInit();
            panelRoles.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabUsers;
        private TabPage tabRoles;
        private DataGridView dgvUsers;
        private DataGridView dgvRoles;
        private Panel panelUsers;
        private Panel panelRoles;
        private Button btnUserRefresh;
        private Label lblUserSearch;
        private TextBox txtUserSearch;
        private Button btnUserCreate;
        private Button btnUserEdit;
        private Button btnUserDelete;
        private Button btnUserLock;
        private Button btnUserUnlock;
        private Button btnRoleRefresh;
        private Label lblRoleSearch;
        private TextBox txtRoleSearch;
        private Button btnRoleCreate;
        private Button btnRoleEdit;
        private Button btnRoleDelete;
    }
}

