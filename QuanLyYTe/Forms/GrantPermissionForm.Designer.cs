namespace QuanLyYTe.Forms
{
    partial class GrantPermissionForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tcMain = new TabControl();
            tpObject = new TabPage();
            tpRole = new TabPage();
            cbGranteeType = new ComboBox();
            cbGranteeName = new ComboBox();
            cbObjectType = new ComboBox();
            lbObjects = new ListBox();
            clbColumns = new CheckedListBox();
            chkSelect = new CheckBox();
            chkInsert = new CheckBox();
            chkUpdate = new CheckBox();
            chkDelete = new CheckBox();
            chkExecute = new CheckBox();
            chkWithGrantOption = new CheckBox();
            btnGrant = new Button();
            cbRoleUser = new ComboBox();
            cbRoleToGrant = new ComboBox();
            chkWithAdminOption = new CheckBox();
            btnGrantRole = new Button();
            tcMain.SuspendLayout();
            SuspendLayout();
            // 
            // tcMain
            // 
            tcMain.Controls.Add(tpObject);
            tcMain.Controls.Add(tpRole);
            tcMain.Dock = DockStyle.Fill;
            tcMain.Location = new Point(0, 0);
            tcMain.Name = "tcMain";
            tcMain.SelectedIndex = 0;
            tcMain.Size = new Size(850, 600);
            tcMain.TabIndex = 0;
            // 
            // tpObject
            // 
            tpObject.BackColor = Color.WhiteSmoke;
            tpObject.Location = new Point(4, 29);
            tpObject.Name = "tpObject";
            tpObject.Size = new Size(842, 567);
            tpObject.TabIndex = 0;
            tpObject.Text = "Quyền đối tượng";
            // 
            // tpRole
            // 
            tpRole.BackColor = Color.WhiteSmoke;
            tpRole.Location = new Point(4, 29);
            tpRole.Name = "tpRole";
            tpRole.Size = new Size(842, 567);
            tpRole.TabIndex = 1;
            tpRole.Text = "Cấp Role cho User";
            // 
            // cbGranteeType
            // 
            cbGranteeType.Location = new Point(0, 0);
            cbGranteeType.Name = "cbGranteeType";
            cbGranteeType.Size = new Size(121, 28);
            cbGranteeType.TabIndex = 0;
            // 
            // cbGranteeName
            // 
            cbGranteeName.Location = new Point(0, 0);
            cbGranteeName.Name = "cbGranteeName";
            cbGranteeName.Size = new Size(121, 28);
            cbGranteeName.TabIndex = 0;
            // 
            // cbObjectType
            // 
            cbObjectType.Location = new Point(0, 0);
            cbObjectType.Name = "cbObjectType";
            cbObjectType.Size = new Size(121, 28);
            cbObjectType.TabIndex = 0;
            // 
            // lbObjects
            // 
            lbObjects.Location = new Point(0, 0);
            lbObjects.Name = "lbObjects";
            lbObjects.Size = new Size(120, 96);
            lbObjects.TabIndex = 0;
            // 
            // clbColumns
            // 
            clbColumns.Location = new Point(0, 0);
            clbColumns.Name = "clbColumns";
            clbColumns.Size = new Size(120, 96);
            clbColumns.TabIndex = 0;
            // 
            // chkSelect
            // 
            chkSelect.Location = new Point(0, 0);
            chkSelect.Name = "chkSelect";
            chkSelect.Size = new Size(104, 24);
            chkSelect.TabIndex = 0;
            // 
            // chkInsert
            // 
            chkInsert.Location = new Point(0, 0);
            chkInsert.Name = "chkInsert";
            chkInsert.Size = new Size(104, 24);
            chkInsert.TabIndex = 0;
            // 
            // chkUpdate
            // 
            chkUpdate.Location = new Point(0, 0);
            chkUpdate.Name = "chkUpdate";
            chkUpdate.Size = new Size(104, 24);
            chkUpdate.TabIndex = 0;
            // 
            // chkDelete
            // 
            chkDelete.Location = new Point(0, 0);
            chkDelete.Name = "chkDelete";
            chkDelete.Size = new Size(104, 24);
            chkDelete.TabIndex = 0;
            // 
            // chkExecute
            // 
            chkExecute.Location = new Point(0, 0);
            chkExecute.Name = "chkExecute";
            chkExecute.Size = new Size(104, 24);
            chkExecute.TabIndex = 0;
            // 
            // chkWithGrantOption
            // 
            chkWithGrantOption.Location = new Point(0, 0);
            chkWithGrantOption.Name = "chkWithGrantOption";
            chkWithGrantOption.Size = new Size(104, 24);
            chkWithGrantOption.TabIndex = 0;
            // 
            // btnGrant
            // 
            btnGrant.Location = new Point(0, 0);
            btnGrant.Name = "btnGrant";
            btnGrant.Size = new Size(75, 23);
            btnGrant.TabIndex = 0;
            // 
            // cbRoleUser
            // 
            cbRoleUser.Location = new Point(0, 0);
            cbRoleUser.Name = "cbRoleUser";
            cbRoleUser.Size = new Size(121, 28);
            cbRoleUser.TabIndex = 0;
            // 
            // cbRoleToGrant
            // 
            cbRoleToGrant.Location = new Point(0, 0);
            cbRoleToGrant.Name = "cbRoleToGrant";
            cbRoleToGrant.Size = new Size(121, 28);
            cbRoleToGrant.TabIndex = 0;
            // 
            // chkWithAdminOption
            // 
            chkWithAdminOption.Location = new Point(0, 0);
            chkWithAdminOption.Name = "chkWithAdminOption";
            chkWithAdminOption.Size = new Size(104, 24);
            chkWithAdminOption.TabIndex = 0;
            // 
            // btnGrantRole
            // 
            btnGrantRole.Location = new Point(0, 0);
            btnGrantRole.Name = "btnGrantRole";
            btnGrantRole.Size = new Size(75, 23);
            btnGrantRole.TabIndex = 0;
            // 
            // GrantPermissionForm
            // 
            ClientSize = new Size(850, 600);
            Controls.Add(tcMain);
            Font = new Font("Segoe UI", 9F);
            Name = "GrantPermissionForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cấp quyền - Grant Permission";
            Load += GrantPermissionForm_Load;
            tcMain.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void SetupTabObjectPrivileges()
        {
            // TẠO KHUNG BAO QUANH CHO TAB QUYỀN ĐỐI TƯỢNG
            Panel pnlObj = new Panel
            {
                Size = new Size(800, 510),
                Location = new Point(20, 15),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            int labelX = 25; int controlX = 150;

            pnlObj.Controls.Add(CreateLabel("Người nhận:", labelX, 25));
            this.cbGranteeType = CreateComboBox(controlX, 22, 100);
            this.cbGranteeType.Items.AddRange(new object[] { "USER", "ROLE" });
            this.cbGranteeName = CreateComboBox(controlX + 110, 22, 215);

            pnlObj.Controls.Add(CreateLabel("Loại đối tượng:", labelX, 65));
            this.cbObjectType = CreateComboBox(controlX, 62, 325);
            this.cbObjectType.Items.AddRange(new object[] { "TABLE", "VIEW", "PROCEDURE", "FUNCTION" });

            pnlObj.Controls.Add(CreateLabel("Chọn Đối tượng:", labelX, 105));
            this.lbObjects = new ListBox() { Location = new Point(labelX, 130), Width = 360, Height = 150, BorderStyle = BorderStyle.FixedSingle };

            pnlObj.Controls.Add(CreateLabel("Phân quyền mức cột:", 410, 105));
            this.clbColumns = new CheckedListBox() { Location = new Point(410, 130), Width = 360, Height = 150, CheckOnClick = true, BorderStyle = BorderStyle.FixedSingle };

            int checkY = 300;
            this.chkSelect = CreateCheckBox("SELECT", labelX, checkY);
            this.chkInsert = CreateCheckBox("INSERT", 130, checkY);
            this.chkUpdate = CreateCheckBox("UPDATE", 235, checkY);
            this.chkDelete = CreateCheckBox("DELETE", 340, checkY);
            this.chkExecute = CreateCheckBox("EXECUTE", 445, checkY);
            this.chkWithGrantOption = CreateCheckBox("WITH GRANT OPTION", labelX, 350, 250, Color.Blue);

            this.btnGrant = new Button
            {
                Text = "Xác nhận cấp quyền đối tượng",
                Location = new Point(labelX, 410),
                Width = 745,
                Height = 60,
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            pnlObj.Controls.AddRange(new Control[] { cbGranteeType, cbGranteeName, cbObjectType, lbObjects, clbColumns, chkSelect, chkInsert, chkUpdate, chkDelete, chkExecute, chkWithGrantOption, btnGrant });

            // Thêm panel vào TabPage
            this.tpObject.Controls.Add(pnlObj);
        }

        private void SetupTabRoleGrant()
        {
            Panel pnl = new Panel
            {
                Size = new Size(780, 480),
                Location = new Point(30, 25),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            Label lblUser = new Label { Text = "Người nhận (User):", Location = new Point(150, 100), AutoSize = true };
            this.cbRoleUser = CreateComboBox(320, 97, 300);

            Label lblRole = new Label { Text = "Role cần cấp:", Location = new Point(150, 180), AutoSize = true };
            this.cbRoleToGrant = CreateComboBox(320, 177, 300);

            this.chkWithAdminOption = new CheckBox { Text = "WITH ADMIN OPTION", Location = new Point(150, 250), AutoSize = true };

            this.btnGrantRole = new Button
            {
                Text = "Xác nhận cấp Role cho User",
                Location = new Point(150, 320),
                Width = 470,
                Height = 55,
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            pnl.Controls.AddRange(new Control[] { lblUser, cbRoleUser, lblRole, cbRoleToGrant, chkWithAdminOption, btnGrantRole });
            this.tpRole.Controls.Add(pnl);
        }

        private Label CreateLabel(string text, int x, int y) => new Label { Text = text, Location = new Point(x, y), AutoSize = true };
        private System.Windows.Forms.ComboBox CreateComboBox(int x, int y, int w) => new System.Windows.Forms.ComboBox
        {
            Location = new System.Drawing.Point(x, y),
            Width = w,
            DropDownStyle = ComboBoxStyle.DropDown, // Đổi từ DropDownList sang DropDown để cho phép gõ
            AutoCompleteMode = AutoCompleteMode.SuggestAppend, // Bật gợi ý
            AutoCompleteSource = AutoCompleteSource.ListItems  // Lấy nguồn từ danh sách Item
        };
        private CheckBox CreateCheckBox(string text, int x, int y, int w = 100, Color? color = null) => new CheckBox { Text = text, Location = new Point(x, y), Width = w, ForeColor = color ?? Color.Black };

        private TabControl tcMain;
        private TabPage tpObject, tpRole;
        // Tab 1 UI
        private ComboBox cbObjectType, cbGranteeType, cbGranteeName;
        private ListBox lbObjects;
        private CheckedListBox clbColumns;
        private CheckBox chkSelect, chkUpdate, chkInsert, chkDelete, chkExecute, chkWithGrantOption;
        private Button btnGrant;
        // Tab 2 UI
        private ComboBox cbRoleUser, cbRoleToGrant;
        private CheckBox chkWithAdminOption;
        private Button btnGrantRole;
    }
}