using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace QuanLyYTe.Forms
{
    public partial class PQNC : Form
    {
        // --- CẤU HÌNH ---
        private const string ConnectionString = "Data Source=localhost:1521/PDB_QLYT;User Id=hospital_dba;Password=123;";
        private const string TargetSchema = "HOSPITAL_DBA";

        // --- ĐIỀU KHIỂN GIAO DIỆN ---
        private ComboBox cbObjectType, cbGranteeType, cbGranteeName;
        private ListBox lbObjects;
        private CheckedListBox clbColumns;
        private CheckBox chkSelect, chkUpdate, chkInsert, chkDelete, chkExecute, chkWithGrantOption, chkWithAdminOption;
        private Button btnGrant;

        public PQNC()
        {
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

            // 1. Thông tin người nhận
            AddLabel("Người nhận:", labelX, 25);
            cbGranteeType = AddComboBox(controlX, 22, 100);
            cbGranteeType.Items.AddRange(new string[] { "USER", "ROLE" });
            cbGranteeType.SelectedIndex = 0;
            cbGranteeType.SelectedIndexChanged += (s, e) => LoadGranteeNames();

            cbGranteeName = AddComboBox(controlX + 110, 22, 200);

            // 2. Loại đối tượng
            AddLabel("Loại đối tượng:", labelX, 65);
            cbObjectType = AddComboBox(controlX, 62, 200);
            cbObjectType.Items.AddRange(new string[] { "TABLE", "VIEW", "PROCEDURE", "FUNCTION", "GÁN ROLE CHO USER" });
            cbObjectType.SelectedIndexChanged += cbObjectType_SelectedIndexChanged;

            // 3. Danh sách Đối tượng & Cột
            AddLabel("Chọn Đối tượng/Role:", labelX, 105);
            lbObjects = new ListBox() { Location = new Point(labelX, 130), Width = 370, Height = 150, BorderStyle = BorderStyle.FixedSingle };
            lbObjects.SelectedIndexChanged += lbObjects_SelectedIndexChanged;

            AddLabel("Phân quyền mức cột:", 410, 105);
            clbColumns = new CheckedListBox() { Location = new Point(410, 130), Width = 370, Height = 150, CheckOnClick = true, BorderStyle = BorderStyle.FixedSingle };

            // 4. Các quyền truy cập
            int checkY = 300;
            chkSelect = AddCheckBox("SELECT", labelX, checkY);
            chkInsert = AddCheckBox("INSERT", 140, checkY);
            chkUpdate = AddCheckBox("UPDATE", 250, checkY);
            chkDelete = AddCheckBox("DELETE", 360, checkY);
            chkExecute = AddCheckBox("EXECUTE", 470, checkY);

            // 5. Tùy chọn nâng cao
            chkWithGrantOption = AddCheckBox("WITH GRANT OPTION", labelX, 350, 250, Color.Blue);
            chkWithAdminOption = AddCheckBox("WITH ADMIN OPTION", 300, 350, 250, Color.DarkGreen);

            // 6. Nút thực thi
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

        #region Logic & Event Handlers
        private void LoadGranteeNames()
        {
            cbGranteeName.Items.Clear();
            string query = cbGranteeType.Text == "USER" ?
                "SELECT username FROM dba_users WHERE oracle_maintained = 'N'" :
                "SELECT role FROM dba_roles WHERE oracle_maintained = 'N'";
            ExecuteQuery(query, (dr) => cbGranteeName.Items.Add(dr[0].ToString()));
        }

        private void cbObjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = cbObjectType.Text;
            lbObjects.Items.Clear();
            clbColumns.Items.Clear();

            // Reset trạng thái checkbox
            TogglePrivileges(false, false, false, false);

            if (type == "GÁN ROLE CHO USER")
            {
                ExecuteQuery("SELECT role FROM dba_roles WHERE oracle_maintained = 'N'", (dr) => lbObjects.Items.Add(dr[0].ToString()));
                TogglePrivileges(false, false, false, true); // Chỉ mở Admin Option
            }
            else if (type == "PROCEDURE" || type == "FUNCTION")
            {
                ExecuteQuery($"SELECT object_name FROM all_objects WHERE object_type = '{type}' AND owner = '{TargetSchema}'", (dr) => lbObjects.Items.Add(dr[0].ToString()));
                chkExecute.Enabled = chkExecute.Checked = true;
                chkWithGrantOption.Enabled = true;
            }
            else // TABLE / VIEW
            {
                ExecuteQuery($"SELECT object_name FROM all_objects WHERE object_type = '{type}' AND owner = '{TargetSchema}'", (dr) => lbObjects.Items.Add(dr[0].ToString()));
                TogglePrivileges(true, true, true, false); // Mở Select/Insert/Update/Delete và Grant Option
            }
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
                string query = $"SELECT column_name FROM all_tab_columns WHERE table_name = '{lbObjects.SelectedItem}' AND owner = '{TargetSchema}'";
                ExecuteQuery(query, (dr) => clbColumns.Items.Add(dr[0].ToString()));
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

            // 2. Thu thập dữ liệu
            string grantee = cbGranteeName.Text;
            string target = lbObjects.Text;
            int gOpt = chkWithGrantOption.Checked ? 1 : 0;
            int aOpt = chkWithAdminOption.Checked ? 1 : 0;

            List<string> selectedCols = new List<string>();
            foreach (var item in clbColumns.CheckedItems) selectedCols.Add(item.ToString());
            string colList = selectedCols.Count > 0 ? string.Join(",", selectedCols) : null;

            // 3. Thực thi
            try
            {
                using (OracleConnection conn = new OracleConnection(ConnectionString))
                {
                    conn.Open();
                    if (isRoleAssign)
                    {
                        CallGrantSP(conn, grantee, target, null, null, 0, aOpt);
                        // Tự động kích hoạt Role mặc định nếu là User
                        if (cbGranteeType.Text == "USER")
                            new OracleCommand($"ALTER USER {grantee} DEFAULT ROLE ALL", conn).ExecuteNonQuery();
                    }
                    else
                    {
                        if (chkSelect.Checked) CallGrantSP(conn, grantee, "SELECT", target, colList, gOpt, 0);
                        if (chkInsert.Checked) CallGrantSP(conn, grantee, "INSERT", target, null, gOpt, 0);
                        if (chkUpdate.Checked) CallGrantSP(conn, grantee, "UPDATE", target, colList, gOpt, 0);
                        if (chkDelete.Checked) CallGrantSP(conn, grantee, "DELETE", target, null, gOpt, 0);
                        if (chkExecute.Checked) CallGrantSP(conn, grantee, "EXECUTE", target, null, gOpt, 0);
                    }
                    MessageBox.Show($"Đã cấp quyền thành công cho {grantee}!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi Oracle: " + ex.Message, "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        #endregion

        #region Data Access
        private void CallGrantSP(OracleConnection conn, string grantee, string priv, string obj, string cols, int gOpt, int aOpt)
        {
            using (OracleCommand cmd = new OracleCommand("usp_GrantPrivilege_Q3", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_grantee", OracleDbType.Varchar2).Value = grantee;
                cmd.Parameters.Add("p_priv_or_role", OracleDbType.Varchar2).Value = priv;
                cmd.Parameters.Add("p_object_name", OracleDbType.Varchar2).Value = (object)obj ?? DBNull.Value;
                cmd.Parameters.Add("p_column_list", OracleDbType.Varchar2).Value = (object)cols ?? DBNull.Value;
                cmd.Parameters.Add("p_with_grant", OracleDbType.Int32).Value = gOpt;
                cmd.Parameters.Add("p_with_admin", OracleDbType.Int32).Value = aOpt;
                cmd.ExecuteNonQuery();
            }
        }

        private void ExecuteQuery(string sql, Action<OracleDataReader> action)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(ConnectionString))
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand(sql, conn))
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read()) action(dr);
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("Query Error: " + ex.Message); }
        }
        #endregion
    }
}