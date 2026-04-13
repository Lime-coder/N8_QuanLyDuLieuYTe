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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpObject = new System.Windows.Forms.TabPage();
            this.gbObjectInfo = new System.Windows.Forms.GroupBox();
            this.lblObjType = new System.Windows.Forms.Label();
            this.cbObjectType = new System.Windows.Forms.ComboBox();
            this.lblObjName = new System.Windows.Forms.Label();
            this.cbObjectName = new System.Windows.Forms.ComboBox();
            this.gbGranteeInfo = new System.Windows.Forms.GroupBox();
            this.cbGranteeType = new System.Windows.Forms.ComboBox();
            this.cbGranteeName = new System.Windows.Forms.ComboBox();
            this.chkWithGrant = new System.Windows.Forms.CheckBox();

            // --- TÁCH THÀNH 2 KHUNG MỚI ---
            this.gbPrivilegeSelection = new System.Windows.Forms.GroupBox();
            this.clbPrivs = new System.Windows.Forms.CheckedListBox();
            this.gbColumnSelection = new System.Windows.Forms.GroupBox();
            this.clbCols = new System.Windows.Forms.CheckedListBox();

            this.btnGrantObj = new System.Windows.Forms.Button();
            this.tpRole = new System.Windows.Forms.TabPage();

            // Khai báo Tab 2 (Giữ nguyên của em)
            this.gbUserRoleInfo = new System.Windows.Forms.GroupBox();
            this.lblUserForRole = new System.Windows.Forms.Label();
            this.cbUserForRole = new System.Windows.Forms.ComboBox();
            this.gbRoleSelection = new System.Windows.Forms.GroupBox();
            this.lstRolesToAssign = new System.Windows.Forms.ListBox();
            this.chkWithAdmin = new System.Windows.Forms.CheckBox();
            this.btnGrantRole = new System.Windows.Forms.Button();

            this.tabControl.SuspendLayout();
            this.tpObject.SuspendLayout();
            this.gbObjectInfo.SuspendLayout();
            this.gbGranteeInfo.SuspendLayout();
            this.gbPrivilegeSelection.SuspendLayout();
            this.gbColumnSelection.SuspendLayout();
            this.tpRole.SuspendLayout();
            this.gbUserRoleInfo.SuspendLayout();
            this.gbRoleSelection.SuspendLayout();
            this.SuspendLayout();

            // --- TabControl ---
            this.tabControl.Controls.Add(this.tpObject);
            this.tabControl.Controls.Add(this.tpRole);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Size = new System.Drawing.Size(900, 520);

            // --- TAB 1: QUYỀN ĐỐI TƯỢNG ---
            this.tpObject.Controls.Add(this.gbObjectInfo);
            this.tpObject.Controls.Add(this.gbGranteeInfo);
            this.tpObject.Controls.Add(this.gbPrivilegeSelection);
            this.tpObject.Controls.Add(this.gbColumnSelection);
            this.tpObject.Controls.Add(this.btnGrantObj);
            this.tpObject.Text = "Quyền đối tượng (Object Privilege)";

            // KHUNG 1: ĐỐI TƯỢNG (Giữ nguyên)
            this.gbObjectInfo.Text = "Đối tượng được cấp quyền";
            this.gbObjectInfo.Location = new System.Drawing.Point(20, 20);
            this.gbObjectInfo.Size = new System.Drawing.Size(350, 130);
            this.gbObjectInfo.Controls.Add(this.lblObjType);
            this.gbObjectInfo.Controls.Add(this.cbObjectType);
            this.gbObjectInfo.Controls.Add(this.lblObjName);
            this.gbObjectInfo.Controls.Add(this.cbObjectName);
            this.lblObjType.Text = "Loại đối tượng:";
            this.lblObjType.Location = new System.Drawing.Point(15, 30);
            this.cbObjectType.Items.AddRange(new object[] { "TABLE", "VIEW", "PROCEDURE", "FUNCTION" });
            this.cbObjectType.Location = new System.Drawing.Point(130, 27);
            this.cbObjectType.Size = new System.Drawing.Size(200, 25);
            this.cbObjectType.SelectedIndexChanged += new System.EventHandler(this.cbObjectType_SelectedIndexChanged);
            this.lblObjName.Text = "Tên đối tượng:";
            this.lblObjName.Location = new System.Drawing.Point(15, 75);
            this.cbObjectName.Location = new System.Drawing.Point(130, 72);
            this.cbObjectName.Size = new System.Drawing.Size(200, 25);
            this.cbObjectName.SelectedIndexChanged += new System.EventHandler(this.cbObjectName_SelectedIndexChanged);

            // KHUNG 2: CẤP CHO (Giữ nguyên)
            this.gbGranteeInfo.Text = "Cấp cho (Grantee)";
            this.gbGranteeInfo.Location = new System.Drawing.Point(20, 160);
            this.gbGranteeInfo.Size = new System.Drawing.Size(350, 120);
            this.gbGranteeInfo.Controls.Add(this.cbGranteeType);
            this.gbGranteeInfo.Controls.Add(this.cbGranteeName);
            this.gbGranteeInfo.Controls.Add(this.chkWithGrant);
            this.cbGranteeType.Items.AddRange(new object[] { "User", "Role" });
            this.cbGranteeType.Location = new System.Drawing.Point(15, 35);
            this.cbGranteeType.Size = new System.Drawing.Size(100, 25);
            this.cbGranteeType.SelectedIndexChanged += new System.EventHandler(this.cbGranteeType_SelectedIndexChanged);
            this.cbGranteeName.Location = new System.Drawing.Point(130, 35);
            this.cbGranteeName.Size = new System.Drawing.Size(200, 25);
            this.chkWithGrant.Text = "WITH GRANT OPTION (cho phép cấp tiếp)";
            this.chkWithGrant.Location = new System.Drawing.Point(15, 80);
            this.chkWithGrant.Size = new System.Drawing.Size(320, 25);

            // --- KHUNG 3 MỚI: CHỌN QUYỀN ---
            this.gbPrivilegeSelection.Text = "Chọn quyền";
            this.gbPrivilegeSelection.Location = new System.Drawing.Point(390, 20);
            this.gbPrivilegeSelection.Size = new System.Drawing.Size(200, 260);
            this.gbPrivilegeSelection.Controls.Add(this.clbPrivs);

            this.clbPrivs.Items.AddRange(new object[] { "SELECT", "INSERT", "UPDATE", "DELETE", "EXECUTE" });
            this.clbPrivs.Location = new System.Drawing.Point(15, 30);
            this.clbPrivs.Size = new System.Drawing.Size(170, 200);
            this.clbPrivs.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbPrivs_ItemCheck);

            // --- KHUNG 4 MỚI: CHỌN CỘT ---
            this.gbColumnSelection.Text = "Cột (Chỉ dùng cho Select/Update)";
            this.gbColumnSelection.Location = new System.Drawing.Point(610, 20);
            this.gbColumnSelection.Size = new System.Drawing.Size(250, 350);
            this.gbColumnSelection.Controls.Add(this.clbCols);

            this.clbCols.Location = new System.Drawing.Point(15, 30);
            this.clbCols.Size = new System.Drawing.Size(220, 300);
            this.clbCols.Visible = true; // Luôn luôn hiện

            this.btnGrantObj.Text = "Thực hiện GRANT";
            this.btnGrantObj.Location = new System.Drawing.Point(20, 320);
            this.btnGrantObj.Size = new System.Drawing.Size(150, 45);
            this.btnGrantObj.Click += new System.EventHandler(this.btnGrantObj_Click);

            // --- TAB 2: CẤP ROLE (Giữ nguyên của em) ---
            this.tpRole.Controls.Add(this.gbUserRoleInfo);
            this.tpRole.Controls.Add(this.gbRoleSelection);
            this.tpRole.Text = "Cấp Role cho User";
            this.tpRole.Padding = new System.Windows.Forms.Padding(10);
            this.gbUserRoleInfo.Text = "User nhận Role";
            this.gbUserRoleInfo.Location = new System.Drawing.Point(20, 20);
            this.gbUserRoleInfo.Size = new System.Drawing.Size(450, 80);
            this.gbUserRoleInfo.Controls.Add(this.lblUserForRole);
            this.gbUserRoleInfo.Controls.Add(this.cbUserForRole);
            this.lblUserForRole.Text = "User:";
            this.lblUserForRole.Location = new System.Drawing.Point(15, 35);
            this.cbUserForRole.Location = new System.Drawing.Point(120, 32);
            this.cbUserForRole.Size = new System.Drawing.Size(300, 25);
            this.gbRoleSelection.Text = "Role cần gán";
            this.gbRoleSelection.Location = new System.Drawing.Point(20, 110);
            this.gbRoleSelection.Size = new System.Drawing.Size(450, 320);
            this.gbRoleSelection.Controls.Add(this.lstRolesToAssign);
            this.gbRoleSelection.Controls.Add(this.chkWithAdmin);
            this.gbRoleSelection.Controls.Add(this.btnGrantRole);
            this.lstRolesToAssign.Location = new System.Drawing.Point(15, 30);
            this.lstRolesToAssign.Size = new System.Drawing.Size(420, 180);
            this.chkWithAdmin.Text = "WITH ADMIN OPTION (gán tiếp cho user khác)";
            this.chkWithAdmin.Location = new System.Drawing.Point(15, 225);
            this.chkWithAdmin.Size = new System.Drawing.Size(420, 30);
            this.btnGrantRole.Text = "Gán Role cho User";
            this.btnGrantRole.Location = new System.Drawing.Point(15, 265);
            this.btnGrantRole.Size = new System.Drawing.Size(420, 40);
            this.btnGrantRole.Click += new System.EventHandler(this.btnGrantRole_Click);

            // Form
            this.ClientSize = new System.Drawing.Size(900, 520);
            this.Controls.Add(this.tabControl);
            this.Text = "Cấp quyền - Grant Permission";
            this.Load += new System.EventHandler(this.GrantPermissionForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tpObject.ResumeLayout(false);
            this.gbObjectInfo.ResumeLayout(false);
            this.gbGranteeInfo.ResumeLayout(false);
            this.gbPrivilegeSelection.ResumeLayout(false);
            this.gbColumnSelection.ResumeLayout(false);
            this.tpRole.ResumeLayout(false);
            this.gbUserRoleInfo.ResumeLayout(false);
            this.gbRoleSelection.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpObject, tpRole;
        private System.Windows.Forms.GroupBox gbObjectInfo, gbGranteeInfo, gbPrivilegeSelection, gbColumnSelection, gbUserRoleInfo, gbRoleSelection;
        private System.Windows.Forms.ComboBox cbObjectType, cbObjectName, cbGranteeType, cbGranteeName, cbUserForRole;
        private System.Windows.Forms.CheckedListBox clbPrivs, clbCols;
        private System.Windows.Forms.ListBox lstRolesToAssign;
        private System.Windows.Forms.CheckBox chkWithGrant, chkWithAdmin;
        private System.Windows.Forms.Label lblObjType, lblObjName, lblUserForRole;
        private System.Windows.Forms.Button btnGrantObj, btnGrantRole;
    }
}