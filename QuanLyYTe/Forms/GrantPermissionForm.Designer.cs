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
            tpSystem = new TabPage();

            tcMain.SuspendLayout();
            SuspendLayout();
            // 
            // tcMain
            // 
            tcMain.Controls.Add(tpObject);
            tcMain.Controls.Add(tpRole);
            tcMain.Controls.Add(tpSystem);

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
            // tpSystem
            // 
            tpSystem.BackColor = Color.WhiteSmoke;
            tpSystem.Location = new Point(4, 29);
            tpSystem.Name = "tpSystem";
            tpSystem.Size = new Size(842, 567);
            tpSystem.TabIndex = 2;
            tpSystem.Text = "Quyền hệ thống";

            // GrantPermissionForm
            this.ClientSize = new Size(850, 600);
            this.Controls.Add(this.tcMain);
            this.Font = new Font("Segoe UI", 9F);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Cấp quyền";


            cbGranteeType = new ComboBox();
            cbGranteeName = new ComboBox();
            cbObjectType = new ComboBox();
            lbObjects = new ComboBox();
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



            this.tcMain.ResumeLayout(false);
            this.tpObject.ResumeLayout(false);
            this.tpRole.ResumeLayout(false);
            this.tpSystem.ResumeLayout(false);
            this.ResumeLayout(false);

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

            SetupTabObjectPrivileges();
            SetupTabRoleGrant();
            SetupTabSystemPrivileges();

            tcMain.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void SetupTabObjectPrivileges()
        {
            Panel pnlObj = new Panel
            {
                Size = new Size(800, 510),
                Location = new Point(20, 15),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            int labelX = 25; int controlX = 150;

            // Hàng 1: Loại người nhận và Tên người nhận
            pnlObj.Controls.Add(CreateLabel("Người nhận:", labelX, 30));
            this.cbGranteeType = CreateComboBox(controlX, 27, 100);
            this.cbGranteeType.Items.AddRange(new object[] { "USER", "ROLE" });
            this.cbGranteeName = CreateComboBox(controlX + 110, 27, 300);

            // Hàng 2: Loại đối tượng 
            pnlObj.Controls.Add(CreateLabel("Loại đối tượng:", labelX, 70));
            this.cbObjectType = CreateComboBox(controlX, 67, 410);
            this.cbObjectType.Items.AddRange(new object[] { "TABLE", "VIEW", "PROCEDURE", "FUNCTION" });

            // Hàng 3: Chọn đối tượng
            pnlObj.Controls.Add(CreateLabel("Chọn Đối tượng:", labelX, 110));
            this.lbObjects = CreateComboBox(controlX, 107, 410);


            // Hàng 4: Panels Quyền và Cột
            pnlObj.Controls.Add(CreateLabel("Chọn quyền:", labelX, 160));
            Panel pnlPrivs = new Panel
            {
                Location = new Point(labelX, 185),
                Size = new Size(350, 165),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            this.chkSelect = CreateCheckBox("SELECT", 15, 10);
            this.chkInsert = CreateCheckBox("INSERT", 15, 40);
            this.chkUpdate = CreateCheckBox("UPDATE", 15, 70);
            this.chkDelete = CreateCheckBox("DELETE", 15, 100);
            this.chkExecute = CreateCheckBox("EXECUTE", 15, 130);
            pnlPrivs.Controls.AddRange(new Control[] { chkSelect, chkInsert, chkUpdate, chkDelete, chkExecute });

            pnlObj.Controls.Add(CreateLabel("Phân quyền mức cột:", 410, 160));
            this.clbColumns = new CheckedListBox()
            {
                Location = new Point(410, 185),
                Size = new Size(360, 165),
                BorderStyle = BorderStyle.FixedSingle,
                CheckOnClick = true
            };

            // Hàng 5: Options
            this.chkWithGrantOption = CreateCheckBox("WITH GRANT OPTION", labelX, 365, 250, Color.Blue);

            // Hàng 6: Nút bấm
            this.btnGrant = new Button
            {
                Text = "Xác nhận cấp quyền đối tượng",
                Location = new Point(labelX, 415),
                Width = 740,
                Height = 65,
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

          
            pnlObj.Controls.AddRange(new Control[] { cbGranteeType, cbGranteeName, cbObjectType, lbObjects, pnlPrivs, clbColumns, chkWithGrantOption, btnGrant });
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
        private void SetupTabSystemPrivileges()
        {
            Panel pnl = new Panel
            {
                Size = new Size(780, 480),
                Location = new Point(30, 25),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            int labelX = 150; int controlX = 320;

            // Dòng 1: Người nhận
            pnl.Controls.Add(CreateLabel("Người nhận:", labelX, 100));
            this.cbSysGranteeType = CreateComboBox(controlX, 97, 100);
            this.cbSysGranteeType.Items.AddRange(new object[] { "USER", "ROLE" });
            this.cbSysGranteeName = CreateComboBox(controlX + 110, 97, 190);

            // Dòng 2: Chọn quyền
            pnl.Controls.Add(CreateLabel("Chọn quyền:", labelX, 180));
            this.cbSysPrivilege = CreateComboBox(controlX, 177, 300);

            // Dòng 3: Admin Option
            this.chkWithAdminOptionSys = new CheckBox
            {
                Text = "WITH ADMIN OPTION",
                Location = new Point(labelX, 250),
                AutoSize = true
            };

            // Dòng 4: Nút xác nhận
            this.btnGrantSystem = new Button
            {
                Text = "Xác nhận cấp quyền hệ thống",
                Location = new Point(labelX, 320),
                Width = 470,
                Height = 55,
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            pnl.Controls.AddRange(new Control[] { cbSysGranteeType, cbSysGranteeName, cbSysPrivilege, chkWithAdminOptionSys, btnGrantSystem });
            this.tpSystem.Controls.Add(pnl);
        }

        private Label CreateLabel(string text, int x, int y) => new Label { Text = text, Location = new Point(x, y), AutoSize = true };
        private System.Windows.Forms.ComboBox CreateComboBox(int x, int y, int w) => new System.Windows.Forms.ComboBox
        {
            Location = new System.Drawing.Point(x, y),
            Width = w,
            DropDownStyle = ComboBoxStyle.DropDown,
            AutoCompleteMode = AutoCompleteMode.SuggestAppend,
            AutoCompleteSource = AutoCompleteSource.ListItems
        };
        private CheckBox CreateCheckBox(string text, int x, int y, int w = 100, Color? color = null) => new CheckBox { Text = text, Location = new Point(x, y), Width = w, ForeColor = color ?? Color.Black };

        private TabControl tcMain;
        private TabPage tpObject, tpRole, tpSystem;
        // Tab 1 UI
        private ComboBox cbObjectType, cbGranteeType, cbGranteeName, lbObjects;
        private CheckedListBox clbColumns;
        private CheckBox chkSelect, chkUpdate, chkInsert, chkDelete, chkExecute, chkWithGrantOption;
        private Button btnGrant;
        // Tab 2 UI
        private ComboBox cbRoleUser, cbRoleToGrant;
        private CheckBox chkWithAdminOption;
        private Button btnGrantRole;
        // Tab 3 UI
        private ComboBox cbSysGranteeType, cbSysGranteeName, cbSysPrivilege;
        private CheckBox chkWithAdminOptionSys;
        private Button btnGrantSystem;
    }
}