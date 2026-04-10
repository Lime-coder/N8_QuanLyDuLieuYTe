using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using QuanLyYTe.DAL; // Import the DAL namespace

namespace QuanLyYTe.Forms
{
    public partial class PQNC : Form
    {
        // --- CONFIGURATION ---
        private const string ConnectionString = "Data Source=localhost:1521/PDB_QLYT;User Id=hospital_dba;Password=123;";
        private readonly PrivilegeRepository _repository;

        // --- UI CONTROLS ---
        private ComboBox cbObjectType, cbGranteeType, cbGranteeName;
        private ListBox lbObjects;
        private CheckedListBox clbColumns;
        private CheckBox chkSelect, chkUpdate, chkInsert, chkDelete, chkExecute, chkWithGrantOption, chkWithAdminOption;
        private Button btnGrant;

        public PQNC()
        {
            _repository = new PrivilegeRepository(ConnectionString);
            SetupCustomInterface();
            LoadGranteeNames();
        }

        #region UI Setup
        private void SetupCustomInterface()
        {
            this.Text = "Quản Trị Phân Quyền Oracle - Phân Hệ 1";
            this.Size = new Size(850, 600);
            this.Font = new Font("Segoe UI", 9);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            int labelX = 30; int controlX = 160;

            // 1. Grantee Information
            AddLabel("Người nhận:", labelX, 25);
            cbGranteeType = AddComboBox(controlX, 22, 100);
            cbGranteeType.Items.AddRange(new string[] { "USER", "ROLE" });
            cbGranteeType.SelectedIndex = 0;
            cbGranteeType.SelectedIndexChanged += (s, e) => LoadGranteeNames();

            cbGranteeName = AddComboBox(controlX + 110, 22, 200);

            // 2. Object Type
            AddLabel("Loại đối tượng:", labelX, 65);
            cbObjectType = AddComboBox(controlX, 62, 200);
            cbObjectType.Items.AddRange(new string[] { "TABLE", "VIEW", "PROCEDURE", "FUNCTION", "GÁN ROLE CHO USER" });
            cbObjectType.SelectedIndexChanged += cbObjectType_SelectedIndexChanged;

            // 3. Object & Column List
            AddLabel("Chọn Đối tượng/Role:", labelX, 105);
            lbObjects = new ListBox() { Location = new Point(labelX, 130), Width = 370, Height = 150, BorderStyle = BorderStyle.FixedSingle };
            lbObjects.SelectedIndexChanged += lbObjects_SelectedIndexChanged;

            AddLabel("Phân quyền mức cột:", 410, 105);
            clbColumns = new CheckedListBox() { Location = new Point(410, 130), Width = 370, Height = 150, CheckOnClick = true, BorderStyle = BorderStyle.FixedSingle };

            // 4. Access Privileges
            int checkY = 300;
            chkSelect = AddCheckBox("SELECT", labelX, checkY);
            chkInsert = AddCheckBox("INSERT", 140, checkY);
            chkUpdate = AddCheckBox("UPDATE", 250, checkY);
            chkDelete = AddCheckBox("DELETE", 360, checkY);
            chkExecute = AddCheckBox("EXECUTE", 470, checkY);

            chkSelect.CheckedChanged += PrivilegeCheckBox_CheckedChanged;
            chkUpdate.CheckedChanged += PrivilegeCheckBox_CheckedChanged;

            // 5. Advanced Options
            chkWithGrantOption = AddCheckBox("WITH GRANT OPTION", labelX, 350, 250, Color.Blue);
            chkWithAdminOption = AddCheckBox("WITH ADMIN OPTION", 300, 350, 250, Color.DarkGreen);

            // 6. Execution Button
            btnGrant = new Button()
            {
                Text = "XÁC NHẬN CẤP QUYỀN",
                Location = new Point(labelX, 420),
                Width = 750,
                Height = 55,
                BackColor = Color.FromArgb(45, 52, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            btnGrant.Click += btnGrant_Click;

            this.Controls.AddRange(new Control[] {
                cbGranteeType, cbGranteeName, cbObjectType, lbObjects, clbColumns,
                chkSelect, chkInsert, chkUpdate, chkDelete, chkExecute,
                chkWithGrantOption, chkWithAdminOption, btnGrant
            });
        }

        private void AddLabel(string text, int x, int y) => this.Controls.Add(new Label { Text = text, Location = new Point(x, y), AutoSize = true });
        private ComboBox AddComboBox(int x, int y, int w) => new ComboBox { Location = new Point(x, y), Width = w, DropDownStyle = ComboBoxStyle.DropDownList };
        private CheckBox AddCheckBox(string text, int x, int y, int w = 100, Color? color = null) => new CheckBox { Text = text, Location = new Point(x, y), Width = w, ForeColor = color ?? Color.Black };
        #endregion

        #region UI Event Handlers

        private void PrivilegeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            string type = cbObjectType.Text;
            if (type == "TABLE" || type == "VIEW")
            {
                // Enable column list only if SELECT or UPDATE is checked
                clbColumns.Enabled = chkSelect.Checked || chkUpdate.Checked;

                // Reset column selection if disabled
                if (!clbColumns.Enabled)
                {
                    for (int i = 0; i < clbColumns.Items.Count; i++)
                        clbColumns.SetItemChecked(i, false);
                }
            }
        }

        private void LoadGranteeNames()
        {
            try
            {
                cbGranteeName.Items.Clear();
                var grantees = _repository.GetGrantees(cbGranteeType.Text);
                foreach (var name in grantees) cbGranteeName.Items.Add(name);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải danh sách người nhận: " + ex.Message); }
        }

        private void cbObjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = cbObjectType.Text;
            lbObjects.Items.Clear();
            clbColumns.Items.Clear();

            TogglePrivileges(false, false, false, false);

            try
            {
                if (type == "GÁN ROLE CHO USER")
                {
                    var roles = _repository.GetGrantees("ROLE");
                    foreach (var r in roles) lbObjects.Items.Add(r);
                    TogglePrivileges(false, false, false, true);
                }
                else if (type == "PROCEDURE" || type == "FUNCTION")
                {
                    var procs = _repository.GetObjects(type);
                    foreach (var p in procs) lbObjects.Items.Add(p);
                    chkExecute.Enabled = chkExecute.Checked = true;
                    chkWithGrantOption.Enabled = true;
                }
                else // TABLE / VIEW
                {
                    var tables = _repository.GetObjects(type);
                    foreach (var t in tables) lbObjects.Items.Add(t);
                    TogglePrivileges(true, false, true, false);
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải danh sách đối tượng: " + ex.Message); }
        }

        private void TogglePrivileges(bool tablePrivs, bool cols, bool grantOpt, bool adminOpt)
        {
            chkSelect.Enabled = chkInsert.Enabled = chkUpdate.Enabled = chkDelete.Enabled = tablePrivs;
            chkSelect.Checked = chkInsert.Checked = chkUpdate.Checked = chkDelete.Checked = false;
            chkExecute.Enabled = chkExecute.Checked = false;
            clbColumns.Enabled = cols;
            chkWithGrantOption.Enabled = grantOpt;
            chkWithAdminOption.Enabled = adminOpt;
            chkWithGrantOption.Checked = chkWithAdminOption.Checked = false;
        }

        private void lbObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            clbColumns.Items.Clear();
            if (lbObjects.SelectedItem == null) return;

            string type = cbObjectType.Text;
            if (type == "TABLE" || type == "VIEW")
            {
                try
                {
                    var columns = _repository.GetColumns(lbObjects.SelectedItem.ToString());
                    foreach (var col in columns) clbColumns.Items.Add(col);
                }
                catch (Exception ex) { MessageBox.Show("Lỗi tải danh sách cột: " + ex.Message); }
            }
        }

        private void btnGrant_Click(object sender, EventArgs e)
        {
            // 1. Validation
            if (cbGranteeName.SelectedItem == null || lbObjects.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ thông tin Người nhận và Đối tượng!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool isRoleAssign = cbObjectType.Text.Contains("ROLE");
            if (!isRoleAssign && !(chkSelect.Checked || chkInsert.Checked || chkUpdate.Checked || chkDelete.Checked || chkExecute.Checked))
            {
                MessageBox.Show("Vui lòng chọn ít nhất một quyền để cấp!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Data Gathering
            string grantee = cbGranteeName.Text;
            string target = lbObjects.Text;
            int gOpt = chkWithGrantOption.Checked ? 1 : 0;
            int aOpt = chkWithAdminOption.Checked ? 1 : 0;

            List<string> selectedCols = new List<string>();
            foreach (var item in clbColumns.CheckedItems) selectedCols.Add(item.ToString());
            string colList = selectedCols.Count > 0 ? string.Join(",", selectedCols) : null;

            // 3. Execution via Repository
            try
            {
                if (isRoleAssign)
                {
                    _repository.ExecuteGrant(grantee, target, null, null, 0, aOpt);
                }
                else
                {
                    if (chkSelect.Checked) _repository.ExecuteGrant(grantee, "SELECT", target, colList, gOpt, 0);
                    if (chkInsert.Checked) _repository.ExecuteGrant(grantee, "INSERT", target, null, gOpt, 0);
                    if (chkUpdate.Checked) _repository.ExecuteGrant(grantee, "UPDATE", target, colList, gOpt, 0);
                    if (chkDelete.Checked) _repository.ExecuteGrant(grantee, "DELETE", target, null, gOpt, 0);
                    if (chkExecute.Checked) _repository.ExecuteGrant(grantee, "EXECUTE", target, null, gOpt, 0);
                }
                MessageBox.Show($"Đã cấp quyền thành công cho {grantee}!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Oracle: " + ex.Message, "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
