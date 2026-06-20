using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using QuanLyYTe.Services;

namespace QuanLyYTe.Forms
{
    public partial class frmGrantPermission : Form
    {
        private readonly PrivilegeService _service;
        private readonly SecurityAdminService _secService;

        public frmGrantPermission()
        {
            InitializeComponent();
            _service = new PrivilegeService();
            _secService = new SecurityAdminService();

            cbGranteeType.SelectedIndexChanged += (s, e) => LoadGrantees();
            cbObjectType.SelectedIndexChanged += (s, e) => LoadObjects();
            lbObjects.SelectedIndexChanged += (s, e) => LoadColumns();
            chkSelect.CheckedChanged += PrivilegeCheckBox_CheckedChanged;
            chkUpdate.CheckedChanged += PrivilegeCheckBox_CheckedChanged;
            btnGrant.Click += btnGrant_Click;


            btnGrantRole.Click += btnGrantRole_Click;
            tcMain.SelectedIndexChanged += tcMain_SelectedIndexChanged;


            cbSysGranteeType.SelectedIndexChanged += (s, e) => {
                LoadSysGrantees();
                chkWithAdminOptionSys.Enabled = (cbSysGranteeType.Text == "USER");
                if (!chkWithAdminOptionSys.Enabled) chkWithAdminOptionSys.Checked = false;
            };
            btnGrantSystem.Click += btnGrantSystem_Click;

            // OLS Label Tab Events
            lbOlsUsers.SelectedIndexChanged += LbOlsUsers_SelectedIndexChanged;
            txtOlsSearch.TextChanged += TxtOlsSearch_TextChanged;
            radBGD.CheckedChanged += OlsConfig_Changed;
            radLDK.CheckedChanged += OlsConfig_Changed;
            radNV.CheckedChanged += OlsConfig_Changed;
            chkTH.CheckedChanged += OlsConfig_Changed;
            chkTK.CheckedChanged += OlsConfig_Changed;
            chkTM.CheckedChanged += OlsConfig_Changed;
            chkHCM.CheckedChanged += OlsConfig_Changed;
            chkHN.CheckedChanged += OlsConfig_Changed;
            chkHP.CheckedChanged += OlsConfig_Changed;
            btnSaveOlsLabel.Click += BtnSaveOlsLabel_Click;

            cbGranteeType.SelectedIndex = 0;
            cbObjectType.SelectedIndex = 0;
            cbSysGranteeType.SelectedIndex = 0;
        }

        #region Logic Tab 1: Grant Object Privileges
        private void LoadGrantees()
        {
            try
            {
                DataTable dt = _service.GetGrantees(cbGranteeType.Text);

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
                MessageBox.Show("L?i t?i danh s�ch ng�?i nh?n: " + ex.Message);
            }
        }

        private void LoadObjects()
        {
            try
            {
                DataTable dt = _service.GetObjects(cbObjectType.Text);


                lbObjects.DisplayMember = dt.Columns[0].ColumnName;
                lbObjects.DataSource = dt;


                lbObjects.SelectedIndex = -1;
                lbObjects.Text = "";

                bool isCode = (cbObjectType.Text == "PROCEDURE" || cbObjectType.Text == "FUNCTION");
                TogglePrivileges(!isCode, false, true);
                chkExecute.Enabled = isCode;
                chkExecute.Checked = isCode;
            }
            catch (Exception ex) { MessageBox.Show("L?i t?i �?i t�?ng: " + ex.Message); }
        }

        private void LoadColumns()
        {
            clbColumns.Items.Clear();
            if (lbObjects.SelectedIndex == -1 || lbObjects.SelectedItem == null) return;

            try
            {
                DataTable dt = _service.GetColumns(lbObjects.GetItemText(lbObjects.SelectedItem));
                foreach (DataRow row in dt.Rows) clbColumns.Items.Add(row[0].ToString());
            }
            catch (Exception ex) { MessageBox.Show("L?i t?i c?t: " + ex.Message); }
        }

        private void btnGrant_Click(object? sender, EventArgs e)
        {
            string grantee = cbGranteeName.Text.Trim();
            string objName = lbObjects.Text.Trim();

            if (string.IsNullOrEmpty(grantee) || string.IsNullOrEmpty(objName))
            {
                MessageBox.Show("Vui l?ng ch?n ho?c nh?p �?y �? th�ng tin Ng�?i nh?n v� �?i t�?ng!");
                return;
            }

            string colList = (clbColumns.Enabled && clbColumns.CheckedItems.Count > 0)
                             ? string.Join(",", clbColumns.CheckedItems.Cast<string>()) : null;

            try
            {
                int gOpt = chkWithGrantOption.Checked ? 1 : 0;
                bool hasGranted = false;

                if (chkSelect.Checked) { _service.GrantObjectPrivilege(grantee, "SELECT", objName, colList, gOpt); hasGranted = true; }
                if (chkInsert.Checked) { _service.GrantObjectPrivilege(grantee, "INSERT", objName, null, gOpt); hasGranted = true; }
                if (chkUpdate.Checked) { _service.GrantObjectPrivilege(grantee, "UPDATE", objName, colList, gOpt); hasGranted = true; }
                if (chkDelete.Checked) { _service.GrantObjectPrivilege(grantee, "DELETE", objName, null, gOpt); hasGranted = true; }
                if (chkExecute.Checked) { _service.GrantObjectPrivilege(grantee, "EXECUTE", objName, null, gOpt); hasGranted = true; }

                if (hasGranted)
                    MessageBox.Show($"�? c?p quy?n tr�n {objName} cho {grantee} th�nh c�ng!");
                else
                    MessageBox.Show("Vui l?ng tick ch?n �t nh?t m?t quy?n (SELECT, INSERT...)");
            }
            catch (Exception ex) { MessageBox.Show(CleanOracleError(ex.Message), "L?i Oracle", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        #endregion

        #region Logic Tab 2: Grant Role to User
        private void tcMain_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (tcMain.SelectedTab == tpRole)
            {
                try
                {
                    DataTable dtUser = _service.GetGrantees("USER");
                    cbRoleUser.DisplayMember = dtUser.Columns[0].ColumnName;
                    cbRoleUser.DataSource = dtUser;
                    cbRoleUser.SelectedIndex = -1; cbRoleUser.Text = "";

                    DataTable dtRole = _service.GetGrantees("ROLE");
                    cbRoleToGrant.DisplayMember = dtRole.Columns[0].ColumnName;
                    cbRoleToGrant.DataSource = dtRole;
                    cbRoleToGrant.SelectedIndex = -1; cbRoleToGrant.Text = "";
                }
                catch (Exception ex) { MessageBox.Show("L?i t?i Tab Role: " + ex.Message); }
            }
            else if (tcMain.SelectedTab == tpSystem)
            {
        
                if (cbSysGranteeType.SelectedIndex == -1) cbSysGranteeType.SelectedIndex = 0;

                LoadSysGrantees();
                LoadAllSystemPrivileges();
            }
            else if (tcMain.SelectedTab == tpOlsLabel)
            {
                LoadOlsUsers();
            }
        }

        private void btnGrantRole_Click(object? sender, EventArgs e)
        {
            string user = cbRoleUser.Text.Trim();
            string role = cbRoleToGrant.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Vui l?ng ch?n �?y �? User v� Role!");
                return;
            }

            try
            {
                _service.GrantRoleToUser(user, role, chkWithAdminOption.Checked ? 1 : 0);
                MessageBox.Show($"�? g�n Role {role} cho User {user} th�nh c�ng!");
            }
            catch (Exception ex) { MessageBox.Show(CleanOracleError(ex.Message), "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        #endregion

        #region Logic Tab 3: Grant System Privileges
        private void LoadSysGrantees()
        {
            try
            {
                DataTable dt = _service.GetGrantees(cbSysGranteeType.Text);
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
            catch (Exception ex) { MessageBox.Show("L?i: " + ex.Message); }
        }

        private void LoadAllSystemPrivileges()
        {
            try
            {
                DataTable dt = _service.GetAllSystemPrivileges();
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
                MessageBox.Show("Vui l?ng ch?n Ng�?i nh?n v� Quy?n h? th?ng!");
                return;
            }

            try
            {
                _service.GrantSystemPrivilege(grantee, priv, chkWithAdminOptionSys.Checked ? 1 : 0);
                MessageBox.Show($"�? c?p quy?n h? th?ng {priv} cho {grantee} th�nh c�ng!");
            }
            catch (Exception ex) { MessageBox.Show(CleanOracleError(ex.Message), "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        #endregion 

        #region Logic Tab 4: OLS Label Management
        private DataTable _dtOlsUsers;
        private bool _isParsingOls = false;

        private void LoadOlsUsers()
        {
            try
            {
                _dtOlsUsers = _service.GetGrantees("USER");
                if (_dtOlsUsers != null && _dtOlsUsers.Columns.Count > 0)
                {
                    FilterOlsUsers("");
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải danh sách User: " + ex.Message); }
        }

        private void FilterOlsUsers(string searchText)
        {
            if (_dtOlsUsers == null) return;
            var filteredRows = _dtOlsUsers.AsEnumerable()
                .Where(r => r[0].ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(r => r[0].ToString())
                .ToArray();
            
            lbOlsUsers.Items.Clear();
            lbOlsUsers.Items.AddRange(filteredRows);
        }

        private void TxtOlsSearch_TextChanged(object? sender, EventArgs e)
        {
            FilterOlsUsers(txtOlsSearch.Text.Trim());
        }

        private void LbOlsUsers_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (lbOlsUsers.SelectedItem == null) return;
            string username = lbOlsUsers.SelectedItem.ToString();
            
            try
            {
                string oldLabel = _secService.GetUserOlsLabel(username);
                ParseOlsLabel(oldLabel);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy nhãn OLS: " + ex.Message);
            }
        }

        private void ParseOlsLabel(string label)
        {
            _isParsingOls = true;
            
            // Reset
            radBGD.Checked = radLDK.Checked = radNV.Checked = false;
            chkTH.Checked = chkTK.Checked = chkTM.Checked = false;
            chkHCM.Checked = chkHN.Checked = chkHP.Checked = false;

            if (!string.IsNullOrEmpty(label))
            {
                string[] parts = label.Split(':');
                if (parts.Length > 0)
                {
                    string level = parts[0];
                    if (level == "BGD") radBGD.Checked = true;
                    else if (level == "LDK") radLDK.Checked = true;
                    else if (level == "NV") radNV.Checked = true;
                }
                if (parts.Length > 1)
                {
                    string[] comps = parts[1].Split(',');
                    if (comps.Contains("TH")) chkTH.Checked = true;
                    if (comps.Contains("TK")) chkTK.Checked = true;
                    if (comps.Contains("TM")) chkTM.Checked = true;
                }
                if (parts.Length > 2)
                {
                    string[] groups = parts[2].Split(',');
                    if (groups.Contains("HCM")) chkHCM.Checked = true;
                    if (groups.Contains("HN")) chkHN.Checked = true;
                    if (groups.Contains("HP")) chkHP.Checked = true;
                }
            }

            _isParsingOls = false;
            UpdateOlsPreview();
        }

        private string BuildOlsLabel()
        {
            string level = "NV";
            if (radBGD.Checked) level = "BGD";
            else if (radLDK.Checked) level = "LDK";

            List<string> comps = new List<string>();
            if (chkTH.Checked) comps.Add("TH");
            if (chkTK.Checked) comps.Add("TK");
            if (chkTM.Checked) comps.Add("TM");

            List<string> groups = new List<string>();
            if (chkHCM.Checked) groups.Add("HCM");
            if (chkHN.Checked) groups.Add("HN");
            if (chkHP.Checked) groups.Add("HP");

            string label = level;
            if (comps.Count > 0) label += ":" + string.Join(",", comps);
            if (groups.Count > 0) label += ":" + string.Join(",", groups);

            return label;
        }

        private void AppendPreviewText(RichTextBox rtb, string text, bool highlight)
        {
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;
            
            if (highlight)
            {
                rtb.SelectionColor = Color.FromArgb(255, 140, 40); // Orange
                rtb.SelectionFont = new Font(rtb.Font, FontStyle.Bold);
            }
            else
            {
                rtb.SelectionColor = Color.DimGray;
                rtb.SelectionFont = new Font(rtb.Font, FontStyle.Italic);
            }
            
            rtb.AppendText(text);
        }

        private void UpdateOlsPreview()
        {
            if (_isParsingOls) return;
            
            rtbOlsPreview.Clear();
            string label = BuildOlsLabel();
            if (string.IsNullOrEmpty(label)) return;

            AppendPreviewText(rtbOlsPreview, "Chuỗi nhãn được tạo: ", false);
            AppendPreviewText(rtbOlsPreview, label + "\n\n", true);

            string levelText = "";
            if (radBGD.Checked) levelText = "Ban Giám đốc";
            else if (radLDK.Checked) levelText = "Lãnh đạo Khoa";
            else if (radNV.Checked) levelText = "Toàn bộ Nhân viên";

            List<string> compText = new List<string>();
            if (chkTH.Checked) compText.Add("Khoa Tiêu hóa");
            if (chkTK.Checked) compText.Add("Khoa Thần kinh");
            if (chkTM.Checked) compText.Add("Khoa Tim mạch");

            List<string> groupText = new List<string>();
            if (chkHCM.Checked) groupText.Add("Hồ Chí Minh");
            if (chkHN.Checked) groupText.Add("Hà Nội");
            if (chkHP.Checked) groupText.Add("Hải Phòng");

            AppendPreviewText(rtbOlsPreview, "Tức là người dùng này sẽ có quyền truy cập dữ liệu của ", false);
            AppendPreviewText(rtbOlsPreview, levelText, true);

            if (compText.Count > 0)
            {
                AppendPreviewText(rtbOlsPreview, " thuộc ", false);
                AppendPreviewText(rtbOlsPreview, string.Join(", ", compText), true);
            }
            else
            {
                AppendPreviewText(rtbOlsPreview, " thuộc ", false);
                AppendPreviewText(rtbOlsPreview, "Bất kỳ khoa nào", true);
            }

            if (groupText.Count > 0)
            {
                AppendPreviewText(rtbOlsPreview, " tại cơ sở ", false);
                AppendPreviewText(rtbOlsPreview, string.Join(", ", groupText), true);
                AppendPreviewText(rtbOlsPreview, ".", false);
            }
            else
            {
                AppendPreviewText(rtbOlsPreview, " tại ", false);
                AppendPreviewText(rtbOlsPreview, "Bất kỳ cơ sở nào", true);
                AppendPreviewText(rtbOlsPreview, ".", false);
            }
        }

        private void OlsConfig_Changed(object? sender, EventArgs e)
        {
            UpdateOlsPreview();
        }

        private void BtnSaveOlsLabel_Click(object? sender, EventArgs e)
        {
            if (lbOlsUsers.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một User để cấp nhãn!");
                return;
            }

            string username = lbOlsUsers.SelectedItem.ToString();
            string label = BuildOlsLabel();

            try
            {
                _secService.SetUserOlsLabel(username, label);
                MessageBox.Show($"Đã gán nhãn OLS: [{label}] cho User: {username} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu nhãn OLS: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void GrantPermissionForm_Load(object sender, EventArgs e) { }
    }
}


