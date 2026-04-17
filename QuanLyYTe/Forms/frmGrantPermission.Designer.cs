namespace QuanLyYTe.Forms
{
    partial class frmGrantPermission
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

            // tcMain 
            tcMain.Controls.Add(tpObject);
            tcMain.Controls.Add(tpRole);
            tcMain.Controls.Add(tpSystem);
            tcMain.Dock = DockStyle.Fill;
            tcMain.Location = new Point(0, 0);
            tcMain.Name = "tcMain";
            tcMain.SelectedIndex = 0;
            tcMain.Size = new Size(850, 600);
            tcMain.TabIndex = 0;

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
                BackColor = Color.FromArgb(248, 249, 250),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            int labelX = 25; int controlX = 150;

            // Hàng 1: Loại người nhận và Tên người nhận
            pnlObj.Controls.Add(CreateLabel("Người nhận:", labelX, 30, true));
            this.cbGranteeType = CreateComboBox(controlX, 27, 100);
            this.cbGranteeType.Items.AddRange(new object[] { "USER", "ROLE" });
            this.cbGranteeName = CreateComboBox(controlX + 110, 27, 300);
            // this.cbGranteeName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // Hàng 2: Loại đối tượng 
            pnlObj.Controls.Add(CreateLabel("Loại đối tượng:", labelX, 70, true));
            this.cbObjectType = CreateComboBox(controlX, 67, 410);
            this.cbObjectType.Items.AddRange(new object[] { "TABLE", "VIEW", "PROCEDURE", "FUNCTION" });

            // Hàng 3: Chọn đối tượng
            pnlObj.Controls.Add(CreateLabel("Chọn Đối tượng:", labelX, 110, true));
            this.lbObjects = CreateComboBox(controlX, 107, 410);


            // Hàng 4: Panels Quyền và Cột
            pnlObj.Controls.Add(CreateLabel("Chọn quyền:", labelX, 160, true));
            Panel pnlPrivs = new Panel
            {
                Location = new Point(labelX, 185),
                Size = new Size(350, 165),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
            };
            this.chkSelect = CreateCheckBox("SELECT", 15, 10);
            this.chkInsert = CreateCheckBox("INSERT", 15, 40);
            this.chkUpdate = CreateCheckBox("UPDATE", 15, 70);
            this.chkDelete = CreateCheckBox("DELETE", 15, 100);
            this.chkExecute = CreateCheckBox("EXECUTE", 15, 130);
            pnlPrivs.Controls.AddRange(new Control[] { chkSelect, chkInsert, chkUpdate, chkDelete, chkExecute });

            pnlObj.Controls.Add(CreateLabel("Phân quyền mức cột:", 410, 160, true));
            this.clbColumns = new CheckedListBox()
            {
                Location = new Point(410, 185),
                Size = new Size(360, 165),
                BorderStyle = BorderStyle.FixedSingle,
                CheckOnClick = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            // Hàng 5: Options
            this.chkWithGrantOption = CreateCheckBox("WITH GRANT OPTION", labelX, 365, 250, Color.Blue);
            this.chkWithGrantOption.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            // Hàng 6: Nút bấm
            this.btnGrant = new Button
            {
                Text = "Xác nhận cấp quyền đối tượng",
                Location = new Point(labelX, 415),
                Width = 740,
                Height = 65,
                BackColor = Color.FromArgb(255, 140, 40),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };


            pnlObj.Controls.AddRange(new Control[] { cbGranteeType, cbGranteeName, cbObjectType, lbObjects, pnlPrivs, clbColumns, chkWithGrantOption, btnGrant });
            this.tpObject.Controls.Add(pnlObj);
        }

        private void SetupTabRoleGrant()
        {
            Panel pnlOuter = new Panel
            {
                Size = new Size(780, 480),
                Location = new Point(30, 25),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 249, 250),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };


            TableLayoutPanel tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 7, BackColor = Color.FromArgb(248, 249, 250) };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F)); // Lề trái
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F)); // Nội dung
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F)); // Lề phải


            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));  // Khoảng trống trên
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Người nhận
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Role cần cấp
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Checkbox
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F)); // Spacer giữa input và button
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F)); // Button
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));  // Khoảng trống dưới

            tlp.Controls.Add(new Label { Text = "Người nhận (User):", Font = new Font("Segoe UI", 10F, FontStyle.Bold), Dock = DockStyle.Bottom }, 1, 0);
            this.cbRoleUser = CreateComboBox(0, 0, 400); cbRoleUser.Dock = DockStyle.Fill;
            tlp.Controls.Add(cbRoleUser, 1, 1);

            tlp.Controls.Add(new Label { Text = "Role cần cấp:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), Dock = DockStyle.Bottom }, 1, 2);
            this.cbRoleToGrant = CreateComboBox(0, 0, 400); cbRoleToGrant.Dock = DockStyle.Fill;
            tlp.Controls.Add(cbRoleToGrant, 1, 3);

            this.chkWithAdminOption = CreateCheckBox("WITH ADMIN OPTION", 0, 0, 200);
            tlp.Controls.Add(chkWithAdminOption, 1, 4);

            this.btnGrantRole = new Button
            {
                Text = "Xác nhận cấp role cho user",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(255, 140, 40),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            tlp.Controls.Add(btnGrantRole, 1, 5);

            pnlOuter.Controls.Add(tlp);
            this.tpRole.Controls.Add(pnlOuter);
        }

        private void SetupTabSystemPrivileges()
        {

            Panel pnlOuter = new Panel
            {
                Size = new Size(780, 480),
                Location = new Point(30, 25),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 249, 250),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            TableLayoutPanel tlpSys = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 7,
                BackColor = Color.Transparent
            };


            tlpSys.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tlpSys.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tlpSys.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));

            tlpSys.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));  // Khoảng trống trên
            tlpSys.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Dòng chọn Người nhận
            tlpSys.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Nhãn chọn quyền
            tlpSys.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Ô chọn quyền
            tlpSys.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Checkbox
            tlpSys.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F)); // Nút bấm
            tlpSys.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));  // Khoảng trống dưới



            // Hàng 1: Nhãn Người nhận 
            tlpSys.Controls.Add(new Label
            {
                Text = "Người nhận:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Dock = DockStyle.Bottom
            }, 1, 0);

            // Hàng 2: ComboBox chọn loại người nhận và tên người nhận trong cùng một hàng
            TableLayoutPanel tlpGranteeGroup = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0, 10, 0, 0)
            };
            tlpGranteeGroup.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpGranteeGroup.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75F));
            tlpGranteeGroup.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            this.cbSysGranteeType = CreateComboBox(0, 0, 0);
            this.cbSysGranteeType.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.cbSysGranteeType.Margin = new Padding(0, 0, 5, 0);
            this.cbSysGranteeType.Items.AddRange(new object[] { "USER", "ROLE" });

            this.cbSysGranteeName = CreateComboBox(0, 0, 0);
            this.cbSysGranteeName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.cbSysGranteeName.Margin = new Padding(5, 0, 0, 0);
            tlpGranteeGroup.Controls.Add(cbSysGranteeType, 0, 0);
            tlpGranteeGroup.Controls.Add(cbSysGranteeName, 1, 0);
            tlpSys.Controls.Add(tlpGranteeGroup, 1, 1);


            // Hàng 3: Nhãn Chọn quyền hệ thống
            tlpSys.Controls.Add(new Label
            {
                Text = "Chọn quyền hệ thống:",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Dock = DockStyle.Bottom
            }, 1, 2);

            // Hàng 4: Ô chọn quyền
            this.cbSysPrivilege = CreateComboBox(0, 0, 0);
            this.cbSysPrivilege.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.cbSysPrivilege.Margin = new Padding(0, 10, 0, 0);
            tlpSys.Controls.Add(cbSysPrivilege, 1, 3);
            // Hàng 5: Checkbox Admin Option
            this.chkWithAdminOptionSys = CreateCheckBox("WITH ADMIN OPTION", 0, 0, 200);
            tlpSys.Controls.Add(chkWithAdminOptionSys, 1, 4);

            // Hàng 6: Nút bấm xác nhận
            this.btnGrantSystem = new Button
            {
                Text = "Xác nhận cấp quyền hệ thống",
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 10, 0, 0),
                BackColor = Color.FromArgb(255, 140, 40),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)

            };
            tlpSys.Controls.Add(btnGrantSystem, 1, 5);

            pnlOuter.Controls.Add(tlpSys);
            this.tpSystem.Controls.Add(pnlOuter);
        }
        public Label CreateLabel(string text, int x, int y, bool isBold = false)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Location = new Point(x, y);
            lbl.AutoSize = true;

            if (isBold)
            {
                lbl.Font = new Font(lbl.Font, FontStyle.Bold);
            }

            return lbl;
        }
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