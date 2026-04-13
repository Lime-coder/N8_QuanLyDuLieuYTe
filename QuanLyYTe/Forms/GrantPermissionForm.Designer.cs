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

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.cbGranteeType = new System.Windows.Forms.ComboBox();
            this.cbGranteeName = new System.Windows.Forms.ComboBox();
            this.cbObjectType = new System.Windows.Forms.ComboBox();
            this.lbObjects = new System.Windows.Forms.ListBox();
            this.clbColumns = new System.Windows.Forms.CheckedListBox();
            this.chkSelect = new System.Windows.Forms.CheckBox();
            this.chkInsert = new System.Windows.Forms.CheckBox();
            this.chkUpdate = new System.Windows.Forms.CheckBox();
            this.chkDelete = new System.Windows.Forms.CheckBox();
            this.chkExecute = new System.Windows.Forms.CheckBox();
            this.chkWithGrantOption = new System.Windows.Forms.CheckBox();
            this.btnGrant = new System.Windows.Forms.Button();
            this.SuspendLayout();

            int labelX = 30; int controlX = 160;

            // Grantee Info
            AddLabel("Người nhận:", labelX, 25);
            this.cbGranteeType = AddComboBox(controlX, 22, 100);
            this.cbGranteeType.Items.AddRange(new string[] { "USER", "ROLE" });
            this.cbGranteeName = AddComboBox(controlX + 110, 22, 200);

            // Object Type
            AddLabel("Loại đối tượng:", labelX, 65);
            this.cbObjectType = AddComboBox(controlX, 62, 200);
            this.cbObjectType.Items.AddRange(new string[] { "TABLE", "VIEW", "PROCEDURE", "FUNCTION" });

            // Lists
            AddLabel("Chọn Đối tượng:", labelX, 105);
            this.lbObjects = new System.Windows.Forms.ListBox() { Location = new System.Drawing.Point(labelX, 130), Width = 370, Height = 150, BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle };

            AddLabel("Phân quyền mức cột:", 410, 105);
            this.clbColumns = new System.Windows.Forms.CheckedListBox() { Location = new System.Drawing.Point(410, 130), Width = 370, Height = 150, CheckOnClick = true, BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle };

            // Privileges
            int checkY = 300;
            this.chkSelect = AddCheckBox("SELECT", labelX, checkY);
            this.chkInsert = AddCheckBox("INSERT", 140, checkY);
            this.chkUpdate = AddCheckBox("UPDATE", 250, checkY);
            this.chkDelete = AddCheckBox("DELETE", 360, checkY);
            this.chkExecute = AddCheckBox("EXECUTE", 470, checkY);

            // Options
            this.chkWithGrantOption = AddCheckBox("WITH GRANT OPTION", labelX, 350, 250, System.Drawing.Color.Blue);

            // Button
            this.btnGrant = new System.Windows.Forms.Button()
            {
                Text = "XÁC NHẬN CẤP QUYỀN ĐỐI TƯỢNG",
                Location = new System.Drawing.Point(labelX, 420),
                Width = 750,
                Height = 55,
                BackColor = System.Drawing.Color.FromArgb(45, 52, 54),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold)
            };

            // Form
            this.ClientSize = new System.Drawing.Size(850, 550);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                cbGranteeType, cbGranteeName, cbObjectType, lbObjects, clbColumns,
                chkSelect, chkInsert, chkUpdate, chkDelete, chkExecute, chkWithGrantOption, btnGrant
            });
            this.Text = "Quản Lý Cấp Quyền Đối Tượng - Phân Hệ 1";
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Font = new System.Drawing.Font("Segoe UI", 9);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void AddLabel(string text, int x, int y) => this.Controls.Add(new System.Windows.Forms.Label { Text = text, Location = new System.Drawing.Point(x, y), AutoSize = true });
        private System.Windows.Forms.ComboBox AddComboBox(int x, int y, int w) => new System.Windows.Forms.ComboBox { Location = new System.Drawing.Point(x, y), Width = w, DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList };
        private System.Windows.Forms.CheckBox AddCheckBox(string text, int x, int y, int w = 100, System.Drawing.Color? color = null) => new System.Windows.Forms.CheckBox { Text = text, Location = new System.Drawing.Point(x, y), Width = w, ForeColor = color ?? System.Drawing.Color.Black };
        #endregion

        private System.Windows.Forms.ComboBox cbObjectType, cbGranteeType, cbGranteeName;
        private System.Windows.Forms.ListBox lbObjects;
        private System.Windows.Forms.CheckedListBox clbColumns;
        private System.Windows.Forms.CheckBox chkSelect, chkUpdate, chkInsert, chkDelete, chkExecute, chkWithGrantOption;
        private System.Windows.Forms.Button btnGrant;
    }
}