using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using QuanLyYTe.DAL;

namespace QuanLyYTe.Forms
{
    public partial class GrantPermissionForm : Form
    {
        private readonly PrivilegeRepository _repository;

        public GrantPermissionForm()
        {
            InitializeComponent();
            _repository = new PrivilegeRepository();

            // Đăng ký sự kiện Tab 1
            cbGranteeType.SelectedIndexChanged += (s, e) => LoadGrantees();
            cbObjectType.SelectedIndexChanged += (s, e) => LoadObjects();
            lbObjects.SelectedIndexChanged += (s, e) => LoadColumns();
            chkSelect.CheckedChanged += PrivilegeCheckBox_CheckedChanged;
            chkUpdate.CheckedChanged += PrivilegeCheckBox_CheckedChanged;
            btnGrant.Click += btnGrant_Click;

            // Đăng ký sự kiện Tab 2
            btnGrantRole.Click += btnGrantRole_Click;
            tcMain.SelectedIndexChanged += tcMain_SelectedIndexChanged;

            // Init mặc định
            cbGranteeType.SelectedIndex = 0;
            cbObjectType.SelectedIndex = 0;
        }

        #region Logic Tab 1: Quyền đối tượng
        private void LoadGrantees()
        {
            try
            {
                DataTable dt = _repository.GetGrantees(cbGranteeType.Text);
                cbGranteeName.DataSource = dt;
                cbGranteeName.DisplayMember = dt.Columns[0].ColumnName;
                chkWithGrantOption.Enabled = (cbGranteeType.Text == "USER");
                if (!chkWithGrantOption.Enabled) chkWithGrantOption.Checked = false;
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải người nhận: " + ex.Message); }
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
            if (cbGranteeName.SelectedValue == null || lbObjects.SelectedValue == null) return;
            string grantee = cbGranteeName.GetItemText(cbGranteeName.SelectedItem);
            string objName = lbObjects.GetItemText(lbObjects.SelectedItem);
            string cols = clbColumns.Enabled && clbColumns.CheckedItems.Count > 0
                         ? string.Join(",", clbColumns.CheckedItems.Cast<string>()) : null;

            try
            {
                if (chkSelect.Checked) _repository.GrantObjectPrivilege(grantee, "SELECT", objName, cols, chkWithGrantOption.Checked ? 1 : 0);
                if (chkInsert.Checked) _repository.GrantObjectPrivilege(grantee, "INSERT", objName, null, chkWithGrantOption.Checked ? 1 : 0);
                if (chkUpdate.Checked) _repository.GrantObjectPrivilege(grantee, "UPDATE", objName, cols, chkWithGrantOption.Checked ? 1 : 0);
                if (chkDelete.Checked) _repository.GrantObjectPrivilege(grantee, "DELETE", objName, null, chkWithGrantOption.Checked ? 1 : 0);
                if (chkExecute.Checked) _repository.GrantObjectPrivilege(grantee, "EXECUTE", objName, null, chkWithGrantOption.Checked ? 1 : 0);
                MessageBox.Show("Cấp quyền đối tượng thành công!");
            }
            catch (Exception ex) { MessageBox.Show(CleanOracleError(ex.Message), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        #endregion

        #region Logic Tab 2: Cấp Role
        private void tcMain_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (tcMain.SelectedTab == tpRole)
            {
                try
                {
                    // Load danh sách User
                    DataTable dtUser = _repository.GetGrantees("USER");
                    cbRoleUser.DisplayMember = dtUser.Columns[0].ColumnName; // Lấy tên cột đầu tiên (UPPER(USERNAME))
                    cbRoleUser.DataSource = dtUser;

                    // Load danh sách Role
                    DataTable dtRole = _repository.GetGrantees("ROLE");
                    cbRoleToGrant.DisplayMember = dtRole.Columns[0].ColumnName; // Lấy tên cột đầu tiên (UPPER(ROLE))
                    cbRoleToGrant.DataSource = dtRole;
                }
                catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu Tab Role: " + ex.Message); }
            }
        }

        private void btnGrantRole_Click(object? sender, EventArgs e)
        {
            if (cbRoleUser.SelectedItem == null || cbRoleToGrant.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ User và Role!");
                return;
            }

            try
            {
                // Sử dụng GetItemText để lấy đúng chuỗi hiển thị trên ComboBox
                string userName = cbRoleUser.GetItemText(cbRoleUser.SelectedItem);
                string roleName = cbRoleToGrant.GetItemText(cbRoleToGrant.SelectedItem);
                int withAdmin = chkWithAdminOption.Checked ? 1 : 0;

                _repository.GrantRoleToUser(userName, roleName, withAdmin);
                MessageBox.Show($"Đã gán Role {roleName} cho User {userName} thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(CleanOracleError(ex.Message), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
    }
}