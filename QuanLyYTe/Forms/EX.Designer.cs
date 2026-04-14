namespace QuanLyYTe.Forms
{
    partial class EX
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tabControl = new TabControl();
            tpObject = new TabPage();
            gbObjectInfo = new GroupBox();
            lblObjType = new Label();
            cbObjectType = new ComboBox();
            lblObjName = new Label();
            cbObjectName = new ComboBox();
            gbGranteeInfo = new GroupBox();
            cbGranteeType = new ComboBox();
            cbGranteeName = new ComboBox();
            chkWithGrant = new CheckBox();
            gbPrivs = new GroupBox();
            clbPrivs = new CheckedListBox();
            lblCols = new Label();
            clbCols = new CheckedListBox();
            btnGrantObj = new Button();
            tpRole = new TabPage();
            gbUserRoleInfo = new GroupBox();
            lblUserForRole = new Label();
            cbUserForRole = new ComboBox();
            gbRoleSelection = new GroupBox();
            lstRolesToAssign = new ListBox();
            chkWithAdmin = new CheckBox();
            btnGrantRole = new Button();
            tabControl.SuspendLayout();
            tpObject.SuspendLayout();
            gbObjectInfo.SuspendLayout();
            gbGranteeInfo.SuspendLayout();
            gbPrivs.SuspendLayout();
            tpRole.SuspendLayout();
            gbUserRoleInfo.SuspendLayout();
            gbRoleSelection.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tpObject);
            tabControl.Controls.Add(tpRole);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(880, 500);
            tabControl.TabIndex = 0;
            // 
            // tpObject
            // 
            tpObject.Controls.Add(gbObjectInfo);
            tpObject.Controls.Add(gbGranteeInfo);
            tpObject.Controls.Add(gbPrivs);
            tpObject.Controls.Add(btnGrantObj);
            tpObject.Location = new Point(4, 29);
            tpObject.Name = "tpObject";
            tpObject.Size = new Size(872, 467);
            tpObject.TabIndex = 0;
            tpObject.Text = "Quyền đối tượng (Object Privilege)";
            // 
            // gbObjectInfo
            // 
            gbObjectInfo.Controls.Add(lblObjType);
            gbObjectInfo.Controls.Add(cbObjectType);
            gbObjectInfo.Controls.Add(lblObjName);
            gbObjectInfo.Controls.Add(cbObjectName);
            gbObjectInfo.Location = new Point(20, 20);
            gbObjectInfo.Name = "gbObjectInfo";
            gbObjectInfo.Size = new Size(350, 130);
            gbObjectInfo.TabIndex = 0;
            gbObjectInfo.TabStop = false;
            gbObjectInfo.Text = "Đối tượng được cấp quyền";
            // 
            // lblObjType
            // 
            lblObjType.Location = new Point(15, 30);
            lblObjType.Name = "lblObjType";
            lblObjType.Size = new Size(100, 23);
            lblObjType.TabIndex = 0;
            lblObjType.Text = "Loại đối tượng:";
            // 
            // cbObjectType
            // 
            cbObjectType.Items.AddRange(new object[] { "TABLE", "VIEW", "PROCEDURE", "FUNCTION" });
            cbObjectType.Location = new Point(130, 27);
            cbObjectType.Name = "cbObjectType";
            cbObjectType.Size = new Size(200, 28);
            cbObjectType.TabIndex = 1;
            cbObjectType.SelectedIndexChanged += cbObjectType_SelectedIndexChanged;
            // 
            // lblObjName
            // 
            lblObjName.Location = new Point(15, 75);
            lblObjName.Name = "lblObjName";
            lblObjName.Size = new Size(100, 23);
            lblObjName.TabIndex = 2;
            lblObjName.Text = "Tên đối tượng:";
            // 
            // cbObjectName
            // 
            cbObjectName.Location = new Point(130, 72);
            cbObjectName.Name = "cbObjectName";
            cbObjectName.Size = new Size(200, 28);
            cbObjectName.TabIndex = 3;
            cbObjectName.SelectedIndexChanged += cbObjectName_SelectedIndexChanged;
            // 
            // gbGranteeInfo
            // 
            gbGranteeInfo.Controls.Add(cbGranteeType);
            gbGranteeInfo.Controls.Add(cbGranteeName);
            gbGranteeInfo.Controls.Add(chkWithGrant);
            gbGranteeInfo.Location = new Point(20, 160);
            gbGranteeInfo.Name = "gbGranteeInfo";
            gbGranteeInfo.Size = new Size(350, 120);
            gbGranteeInfo.TabIndex = 1;
            gbGranteeInfo.TabStop = false;
            gbGranteeInfo.Text = "Cấp cho (Grantee)";
            // 
            // cbGranteeType
            // 
            cbGranteeType.Items.AddRange(new object[] { "User", "Role" });
            cbGranteeType.Location = new Point(15, 35);
            cbGranteeType.Name = "cbGranteeType";
            cbGranteeType.Size = new Size(100, 28);
            cbGranteeType.TabIndex = 0;
            cbGranteeType.SelectedIndexChanged += cbGranteeType_SelectedIndexChanged;
            // 
            // cbGranteeName
            // 
            cbGranteeName.Location = new Point(130, 35);
            cbGranteeName.Name = "cbGranteeName";
            cbGranteeName.Size = new Size(200, 28);
            cbGranteeName.TabIndex = 1;
            // 
            // chkWithGrant
            // 
            chkWithGrant.Location = new Point(15, 80);
            chkWithGrant.Name = "chkWithGrant";
            chkWithGrant.Size = new Size(320, 25);
            chkWithGrant.TabIndex = 2;
            chkWithGrant.Text = "WITH GRANT OPTION (cho phép cấp tiếp)";
            // 
            // gbPrivs
            // 
            gbPrivs.Controls.Add(clbPrivs);
            gbPrivs.Controls.Add(lblCols);
            gbPrivs.Controls.Add(clbCols);
            gbPrivs.Location = new Point(400, 20);
            gbPrivs.Name = "gbPrivs";
            gbPrivs.Size = new Size(450, 350);
            gbPrivs.TabIndex = 2;
            gbPrivs.TabStop = false;
            gbPrivs.Text = "Chọn quyền";
            // 
            // clbPrivs
            // 
            clbPrivs.Items.AddRange(new object[] { "SELECT", "INSERT", "UPDATE", "DELETE", "EXECUTE" });
            clbPrivs.Location = new Point(15, 30);
            clbPrivs.Name = "clbPrivs";
            clbPrivs.Size = new Size(200, 92);
            clbPrivs.TabIndex = 0;
            clbPrivs.ItemCheck += clbPrivs_ItemCheck;
            // 
            // lblCols
            // 
            lblCols.Location = new Point(230, 15);
            lblCols.Name = "lblCols";
            lblCols.Size = new Size(100, 23);
            lblCols.TabIndex = 1;
            lblCols.Text = "Cột:";
            lblCols.Visible = false;
            // 
            // clbCols
            // 
            clbCols.Location = new Point(230, 35);
            clbCols.Name = "clbCols";
            clbCols.Size = new Size(200, 290);
            clbCols.TabIndex = 2;
            clbCols.Visible = false;
            // 
            // btnGrantObj
            // 
            btnGrantObj.Location = new Point(20, 320);
            btnGrantObj.Name = "btnGrantObj";
            btnGrantObj.Size = new Size(150, 45);
            btnGrantObj.TabIndex = 3;
            btnGrantObj.Text = "Thực hiện GRANT";
            btnGrantObj.Click += btnGrantObj_Click;
            // 
            // tpRole
            // 
            tpRole.Controls.Add(gbUserRoleInfo);
            tpRole.Controls.Add(gbRoleSelection);
            tpRole.Location = new Point(4, 29);
            tpRole.Name = "tpRole";
            tpRole.Padding = new Padding(10);
            tpRole.Size = new Size(872, 467);
            tpRole.TabIndex = 1;
            tpRole.Text = "Cấp Role cho User";
            // 
            // gbUserRoleInfo
            // 
            gbUserRoleInfo.Controls.Add(lblUserForRole);
            gbUserRoleInfo.Controls.Add(cbUserForRole);
            gbUserRoleInfo.Location = new Point(20, 20);
            gbUserRoleInfo.Name = "gbUserRoleInfo";
            gbUserRoleInfo.Size = new Size(450, 80);
            gbUserRoleInfo.TabIndex = 0;
            gbUserRoleInfo.TabStop = false;
            gbUserRoleInfo.Text = "User nhận Role";
            gbUserRoleInfo.Enter += gbUserRoleInfo_Enter;
            // 
            // lblUserForRole
            // 
            lblUserForRole.Location = new Point(15, 35);
            lblUserForRole.Name = "lblUserForRole";
            lblUserForRole.Size = new Size(100, 23);
            lblUserForRole.TabIndex = 0;
            lblUserForRole.Text = "User:";
            // 
            // cbUserForRole
            // 
            cbUserForRole.Location = new Point(120, 32);
            cbUserForRole.Name = "cbUserForRole";
            cbUserForRole.Size = new Size(300, 28);
            cbUserForRole.TabIndex = 1;
            // 
            // gbRoleSelection
            // 
            gbRoleSelection.Controls.Add(lstRolesToAssign);
            gbRoleSelection.Controls.Add(chkWithAdmin);
            gbRoleSelection.Controls.Add(btnGrantRole);
            gbRoleSelection.Location = new Point(20, 110);
            gbRoleSelection.Name = "gbRoleSelection";
            gbRoleSelection.Size = new Size(450, 320);
            gbRoleSelection.TabIndex = 1;
            gbRoleSelection.TabStop = false;
            gbRoleSelection.Text = "Role cần gán";
            // 
            // lstRolesToAssign
            // 
            lstRolesToAssign.Location = new Point(15, 30);
            lstRolesToAssign.Name = "lstRolesToAssign";
            lstRolesToAssign.Size = new Size(420, 164);
            lstRolesToAssign.TabIndex = 0;
            // 
            // chkWithAdmin
            // 
            chkWithAdmin.Location = new Point(15, 225);
            chkWithAdmin.Name = "chkWithAdmin";
            chkWithAdmin.Size = new Size(420, 30);
            chkWithAdmin.TabIndex = 1;
            chkWithAdmin.Text = "WITH ADMIN OPTION (gán tiếp cho user khác)";
            // 
            // btnGrantRole
            // 
            btnGrantRole.Location = new Point(15, 265);
            btnGrantRole.Name = "btnGrantRole";
            btnGrantRole.Size = new Size(420, 40);
            btnGrantRole.TabIndex = 2;
            btnGrantRole.Text = "Gán Role cho User";
            btnGrantRole.Click += btnGrantRole_Click;
            // 
            // EX
            // 
            ClientSize = new Size(880, 500);
            Controls.Add(tabControl);
            Name = "EX";
            Text = "Cấp quyền - Grant Permission";
            Load += GrantPermissionForm_Load;
            tabControl.ResumeLayout(false);
            tpObject.ResumeLayout(false);
            gbObjectInfo.ResumeLayout(false);
            gbGranteeInfo.ResumeLayout(false);
            gbPrivs.ResumeLayout(false);
            tpRole.ResumeLayout(false);
            gbUserRoleInfo.ResumeLayout(false);
            gbRoleSelection.ResumeLayout(false);
            ResumeLayout(false);
        }

        // --- TẤT CẢ BIẾN KHAI BÁO (GIỮ NGUYÊN CỦA EM VÀ THÊM MỚI) ---
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpObject, tpRole;
        private System.Windows.Forms.GroupBox gbObjectInfo, gbGranteeInfo, gbPrivs;
        private System.Windows.Forms.ComboBox cbObjectType, cbObjectName, cbGranteeType, cbGranteeName;
        private System.Windows.Forms.CheckedListBox clbPrivs, clbCols;
        private System.Windows.Forms.CheckBox chkWithGrant;
        private System.Windows.Forms.Label lblObjType, lblObjName, lblCols;
        private System.Windows.Forms.Button btnGrantObj;

        // BIẾN CHO TAB 2
        private System.Windows.Forms.GroupBox gbUserRoleInfo, gbRoleSelection;
        private System.Windows.Forms.Label lblUserForRole;
        private System.Windows.Forms.ComboBox cbUserForRole;
        private System.Windows.Forms.ListBox lstRolesToAssign;
        private System.Windows.Forms.CheckBox chkWithAdmin;
        private System.Windows.Forms.Button btnGrantRole;
    }
}