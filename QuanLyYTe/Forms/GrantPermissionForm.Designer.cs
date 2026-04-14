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
            this.tcMain = new TabControl();
            this.tpObject = new TabPage();
            this.tpRole = new TabPage();

            // Tab 1 Controls (Quyền đối tượng)
            this.cbGranteeType = new ComboBox();
            this.cbGranteeName = new ComboBox();
            this.cbObjectType = new ComboBox();
            this.lbObjects = new ListBox();
            this.clbColumns = new CheckedListBox();
            this.chkSelect = new CheckBox();
            this.chkInsert = new CheckBox();
            this.chkUpdate = new CheckBox();
            this.chkDelete = new CheckBox();
            this.chkExecute = new CheckBox();
            this.chkWithGrantOption = new CheckBox();
            this.btnGrant = new Button();

            // Tab 2 Controls (Cấp Role)
            this.cbRoleUser = new ComboBox();
            this.cbRoleToGrant = new ComboBox();
            this.chkWithAdminOption = new CheckBox();
            this.btnGrantRole = new Button();

            this.tcMain.SuspendLayout();
            this.tpObject.SuspendLayout();
            this.tpRole.SuspendLayout();
            this.SuspendLayout();

            // tcMain
            this.tcMain.Controls.Add(this.tpObject);
            this.tcMain.Controls.Add(this.tpRole);
            this.tcMain.Dock = DockStyle.Fill;
            this.tcMain.Location = new Point(0, 0);

            // tpObject Setup
            this.tpObject.Text = "Quyền đối tượng";
            this.tpObject.BackColor = Color.WhiteSmoke;
            SetupTabObjectPrivileges();

            // tpRole Setup
            this.tpRole.Text = "Cấp Role cho User";
            this.tpRole.BackColor = Color.WhiteSmoke;
            SetupTabRoleGrant();

            // GrantPermissionForm
            this.ClientSize = new Size(850, 600);
            this.Controls.Add(this.tcMain);
            this.Font = new Font("Segoe UI", 9F);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Cấp quyền - Grant Permission";
            this.tcMain.ResumeLayout(false);
            this.tpObject.ResumeLayout(false);
            this.tpRole.ResumeLayout(false);
            this.ResumeLayout(false);
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
            this.cbRoleUser = new ComboBox { Location = new Point(320, 97), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblRole = new Label { Text = "Role cần cấp:", Location = new Point(150, 180), AutoSize = true };
            this.cbRoleToGrant = new ComboBox { Location = new Point(320, 177), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };

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
        private ComboBox CreateComboBox(int x, int y, int w) => new ComboBox { Location = new Point(x, y), Width = w, DropDownStyle = ComboBoxStyle.DropDownList };
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