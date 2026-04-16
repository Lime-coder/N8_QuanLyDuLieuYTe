using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using QuanLyYTe.DAL;

namespace QuanLyYTe.Forms
{
    public partial class frmGrantPermission : Form
    {
        private readonly PrivilegeRepository _repository;

        public frmGrantPermission()
        {
            InitializeComponent();
            _repository = new PrivilegeRepository();

            //Tab 1
            cbGranteeType.SelectedIndexChanged += (s, e) => LoadGrantees();
            cbObjectType.SelectedIndexChanged += (s, e) => LoadObjects();
            lbObjects.SelectedIndexChanged += (s, e) => LoadColumns();
            chkSelect.CheckedChanged += PrivilegeCheckBox_CheckedChanged;
            chkUpdate.CheckedChanged += PrivilegeCheckBox_CheckedChanged;
            btnGrant.Click += btnGrant_Click;

            //Tab 2
            btnGrantRole.Click += btnGrantRole_Click;
            tcMain.SelectedIndexChanged += tcMain_SelectedIndexChanged;

            //Tab 3
            cbSysGranteeType.SelectedIndexChanged += (s, e) => LoadSysGrantees();
            btnGrantSystem.Click += btnGrantSystem_Click;

            cbGranteeType.SelectedIndex = 0;
            cbObjectType.SelectedIndex = 0;
            cbSysGranteeType.SelectedIndex = 0;
        }

        #region Logic Tab 1: Grant Object Privileges
        private void LoadGrantees()
        {
            try
            {
                DataTable dt = _repository.GetGrantees(cbGranteeType.Text);

                cbGranteeName.DataSource = null;
                cbGranteeName.Items.Clear();
                cbGranteeName.Text = "";

                if (dt != null && dt.Rows.Count > 0)
                {

                    cbGranteeName.DisplayMember = dt.Columns[0].ColumnName;
                    cbGranteeName.DataSource = dt;


                    cbGranteeName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    cbGranteeName.AutoCompleteSource = AutoCompleteSource.ListItems;
                }

                cbGranteeName.SelectedIndex = -1;
                chkWithGrantOption.Enabled = (cbGranteeType.Text == "USER");
                if (!chkWithGrantOption.Enabled) chkWithGrantOption.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách người nhận: " + ex.Message);
            }
        }

        private void LoadObjects()
        {
            try
            {
                DataTable dt = _repository.GetObjects(cbObjectType.Text);
                lbObjects.DataSource = dt;
                lbObjects.DisplayMember = dt.Columns[0].ColumnName;
                bool isCode = (cbObjectType.Text == "PROCEDURE" || cbObjectType.Text == "FUNCTION");
                TogglePrivileges(!isCode, false, true);
                chkExecute.Enabled = isCode;
                chkExecute.Checked = isCode;
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải đối tượng: " + ex.Message); }
        }

        private void LoadColumns()
        {
            clbColumns.Items.Clear();
            if (lbObjects.SelectedValue == null) return;
            try
            {
                DataTable dt = _repository.GetColumns(lbObjects.GetItemText(lbObjects.SelectedItem));
                foreach (DataRow row in dt.Rows) clbColumns.Items.Add(row[0].ToString());
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải cột: " + ex.Message); }
        }

        private void btnGrant_Click(object? sender, EventArgs e)
        {
            string grantee = cbGranteeName.Text.Trim();
            string objName = lbObjects.Text.Trim();

            if (string.IsNullOrEmpty(grantee) || string.IsNullOrEmpty(objName))
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập đầy đủ thông tin Người nhận và Đối tượng!");
                return;
            }

            string colList = (clbColumns.Enabled && clbColumns.CheckedItems.Count > 0)
                             ? string.Join(",", clbColumns.CheckedItems.Cast<string>()) : null;

            try
            {
                int gOpt = chkWithGrantOption.Checked ? 1 : 0;
                bool hasGranted = false;

                if (chkSelect.Checked) { _repository.GrantObjectPrivilege(grantee, "SELECT", objName, colList, gOpt); hasGranted = true; }
                if (chkInsert.Checked) { _repository.GrantObjectPrivilege(grantee, "INSERT", objName, null, gOpt); hasGranted = true; }
                if (chkUpdate.Checked) { _repository.GrantObjectPrivilege(grantee, "UPDATE", objName, colList, gOpt); hasGranted = true; }
                if (chkDelete.Checked) { _repository.GrantObjectPrivilege(grantee, "DELETE", objName, null, gOpt); hasGranted = true; }
                if (chkExecute.Checked) { _repository.GrantObjectPrivilege(grantee, "EXECUTE", objName, null, gOpt); hasGranted = true; }

                if (hasGranted)
                    MessageBox.Show($"Đã cấp quyền trên {objName} cho {grantee} thành công!");
                else
                    MessageBox.Show("Vui lòng tick chọn ít nhất một quyền (SELECT, INSERT...)");
            }
            catch (Exception ex) { MessageBox.Show(CleanOracleError(ex.Message), "Lỗi Oracle", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        #endregion

        #region Logic Tab 2: Grant Role to User
        private void tcMain_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (tcMain.SelectedTab == tpRole)
            {
                try
                {
                    DataTable dtUser = _repository.GetGrantees("USER");
                    cbRoleUser.DisplayMember = dtUser.Columns[0].ColumnName;
                    cbRoleUser.DataSource = dtUser;
                    cbRoleUser.SelectedIndex = -1; cbRoleUser.Text = "";

                    DataTable dtRole = _repository.GetGrantees("ROLE");
                    cbRoleToGrant.DisplayMember = dtRole.Columns[0].ColumnName;
                    cbRoleToGrant.DataSource = dtRole;
                    cbRoleToGrant.SelectedIndex = -1; cbRoleToGrant.Text = "";
                }
                catch (Exception ex) { MessageBox.Show("Lỗi tải Tab Role: " + ex.Message); }
            }
            else if (tcMain.SelectedTab == tpSystem)
            {

                if (cbSysGranteeType.SelectedIndex == -1) cbSysGranteeType.SelectedIndex = 0;

                LoadSysGrantees();
                LoadAllSystemPrivileges();
            }
        }

        private void btnGrantRole_Click(object? sender, EventArgs e)
        {
            string user = cbRoleUser.Text.Trim();
            string role = cbRoleToGrant.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Vui lòng chọn đầy đủ User và Role!");
                return;
            }

            try
            {
                _repository.GrantRoleToUser(user, role, chkWithAdminOption.Checked ? 1 : 0);
                MessageBox.Show($"Đã gán Role {role} cho User {user} thành công!");
            }
            catch (Exception ex) { MessageBox.Show(CleanOracleError(ex.Message), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        #endregion

        #region Logic Tab 3: Grant System Privileges
        private void LoadSysGrantees()
        {
            try
            {
                DataTable dt = _repository.GetGrantees(cbSysGranteeType.Text);
                cbSysGranteeName.DataSource = null;
                cbSysGranteeName.Text = "";

                if (dt != null && dt.Columns.Count > 0)
                {
                    cbSysGranteeName.DataSource = null;
                    cbSysGranteeName.Items.Clear();
                    cbSysGranteeName.Text = "";

                    cbSysGranteeName.DisplayMember = dt.Columns[0].ColumnName;
                    cbSysGranteeName.DataSource = dt;
                }
                cbSysGranteeName.SelectedIndex = -1;
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void LoadAllSystemPrivileges()
        {
            try
            {
                DataTable dt = _repository.GetAllSystemPrivileges();
                cbSysPrivilege.DisplayMember = dt.Columns[0].ColumnName;
                cbSysPrivilege.DataSource = dt;
                cbSysPrivilege.SelectedIndex = -1;
                cbSysPrivilege.Text = "";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnGrantSystem_Click(object? sender, EventArgs e)
        {
            string grantee = cbSysGranteeName.Text.Trim();
            string priv = cbSysPrivilege.Text.Trim();

            if (string.IsNullOrEmpty(grantee) || string.IsNullOrEmpty(priv))
            {
                MessageBox.Show("Vui lòng chọn Người nhận và Quyền hệ thống!");
                return;
            }

            try
            {
                _repository.GrantSystemPrivilege(grantee, priv, chkWithAdminOptionSys.Checked ? 1 : 0);
                MessageBox.Show($"Đã cấp quyền hệ thống {priv} cho {grantee} thành công!");
            }
            catch (Exception ex) { MessageBox.Show(CleanOracleError(ex.Message), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        #endregion 

        #region Helpers
        private string CleanOracleError(string message)
        {
            if (message.Contains("ORA-20"))
            {
                int start = message.IndexOf(":") + 1;
                int end = message.IndexOf("ORA-06512");
                if (end > start) return message.Substring(start, end - start).Trim();
            }
            return message;
        }

        private void PrivilegeCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (cbObjectType.Text == "TABLE" || cbObjectType.Text == "VIEW")
            {
                clbColumns.Enabled = chkSelect.Checked || chkUpdate.Checked;
            }
        }

        private void TogglePrivileges(bool tablePrivs, bool cols, bool grantOpt)
        {
            chkSelect.Enabled = chkInsert.Enabled = chkUpdate.Enabled = chkDelete.Enabled = tablePrivs;
            chkSelect.Checked = chkInsert.Checked = chkUpdate.Checked = chkDelete.Checked = false;
            clbColumns.Enabled = cols;
            chkWithGrantOption.Enabled = (cbGranteeType.Text == "USER") && grantOpt;
        }
        #endregion

        private void GrantPermissionForm_Load(object sender, EventArgs e) { }
    }
}
