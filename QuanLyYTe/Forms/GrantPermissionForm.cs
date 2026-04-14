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

            cbGranteeType.SelectedIndexChanged += (s, e) => LoadGrantees();
            cbObjectType.SelectedIndexChanged += (s, e) => LoadObjects();
            lbObjects.SelectedIndexChanged += (s, e) => LoadColumns();
            chkSelect.CheckedChanged += PrivilegeCheckBox_CheckedChanged;
            chkUpdate.CheckedChanged += PrivilegeCheckBox_CheckedChanged;
            btnGrant.Click += btnGrant_Click;

            cbGranteeType.SelectedIndex = 0;
            cbObjectType.SelectedIndex = 0;
        }

        #region Logic Tải Dữ Liệu

        private void LoadGrantees()
        {
            try
            {
                DataTable dt = _repository.GetGrantees(cbGranteeType.Text);
                cbGranteeName.DataSource = dt;
                cbGranteeName.DisplayMember = dt.Columns[0].ColumnName;

                // CHỈNH SỬA: Ngăn lỗi từ UI - Nếu chọn ROLE thì không cho phép chọn WITH GRANT OPTION
                if (cbGranteeType.Text == "ROLE")
                {
                    chkWithGrantOption.Checked = false;
                    chkWithGrantOption.Enabled = false;
                }
                else
                {
                    chkWithGrantOption.Enabled = true;
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải người nhận: " + ex.Message); }
        }

        private void LoadObjects()
        {
            try
            {
                string type = cbObjectType.Text;
                DataTable dt = _repository.GetObjects(type);
                lbObjects.DataSource = dt;
                lbObjects.DisplayMember = dt.Columns[0].ColumnName;

                bool isCode = (type == "PROCEDURE" || type == "FUNCTION");
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

            string type = cbObjectType.Text;
            if (type == "TABLE" || type == "VIEW")
            {
                try
                {
                    string targetObj = lbObjects.GetItemText(lbObjects.SelectedItem);
                    DataTable dt = _repository.GetColumns(targetObj);
                    foreach (DataRow row in dt.Rows)
                        clbColumns.Items.Add(row[0].ToString());
                }
                catch (Exception ex) { MessageBox.Show("Lỗi tải danh sách cột: " + ex.Message); }
            }
        }

        #endregion

        #region Xử lý Sự kiện & Thực thi

        private void PrivilegeCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            string type = cbObjectType.Text;
            if (type == "TABLE" || type == "VIEW")
            {
                clbColumns.Enabled = chkSelect.Checked || chkUpdate.Checked;
                if (!clbColumns.Enabled)
                {
                    for (int i = 0; i < clbColumns.Items.Count; i++)
                        clbColumns.SetItemChecked(i, false);
                }
            }
        }

        // BỔ SUNG: Hàm lọc lỗi Oracle để hiển thị thông báo sạch lên UI
        private string CleanOracleError(string message)
        {
            if (string.IsNullOrEmpty(message)) return "Lỗi không xác định.";

            // Tìm vị trí của mã lỗi tự định nghĩa (ORA-20000 -> ORA-20999)
            if (message.Contains("ORA-20"))
            {
                // Cắt lấy phần thông báo sau dấu hai chấm đầu tiên và trước dòng ORA-06512
                int start = message.IndexOf(":") + 1;
                int end = message.IndexOf("ORA-06512");
                if (end > start)
                    return message.Substring(start, end - start).Trim();
            }
            return message;
        }

        private void btnGrant_Click(object? sender, EventArgs e)
        {
            if (cbGranteeName.SelectedValue == null || lbObjects.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Người nhận và Đối tượng!");
                return;
            }

            // CHỈNH SỬA: Thêm kiểm tra logic tại UI trước khi gửi lên DB (Ngăn lỗi ORA-20004)
            if (cbGranteeType.Text == "ROLE" && chkWithGrantOption.Checked)
            {
                MessageBox.Show("Chính sách bảo mật: Không được cấp 'WITH GRANT OPTION' cho một Vai trò (Role).",
                                "Thông báo bảo mật", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!(chkSelect.Checked || chkInsert.Checked || chkUpdate.Checked || chkDelete.Checked || chkExecute.Checked))
            {
                MessageBox.Show("Vui lòng chọn ít nhất một quyền!");
                return;
            }

            string grantee = cbGranteeName.GetItemText(cbGranteeName.SelectedItem);
            string objName = lbObjects.GetItemText(lbObjects.SelectedItem);
            int withGrant = chkWithGrantOption.Checked ? 1 : 0;

            string colList = null;
            if (clbColumns.Enabled && clbColumns.CheckedItems.Count > 0)
            {
                var cols = clbColumns.CheckedItems.Cast<string>().ToList();
                colList = string.Join(",", cols);
            }

            try
            {
                if (chkSelect.Checked) _repository.GrantObjectPrivilege(grantee, "SELECT", objName, colList, withGrant);
                if (chkInsert.Checked) _repository.GrantObjectPrivilege(grantee, "INSERT", objName, null, withGrant);
                if (chkUpdate.Checked) _repository.GrantObjectPrivilege(grantee, "UPDATE", objName, colList, withGrant);
                if (chkDelete.Checked) _repository.GrantObjectPrivilege(grantee, "DELETE", objName, null, withGrant);
                if (chkExecute.Checked) _repository.GrantObjectPrivilege(grantee, "EXECUTE", objName, null, withGrant);

                MessageBox.Show($"Đã cấp quyền cho {grantee} thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // CHỈNH SỬA: Sử dụng hàm CleanOracleError để hiển thị lỗi sạch
                string cleanMsg = CleanOracleError(ex.Message);
                MessageBox.Show(cleanMsg, "Thông báo từ hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TogglePrivileges(bool tablePrivs, bool cols, bool grantOpt)
        {
            chkSelect.Enabled = chkInsert.Enabled = chkUpdate.Enabled = chkDelete.Enabled = tablePrivs;
            chkSelect.Checked = chkInsert.Checked = chkUpdate.Checked = chkDelete.Checked = false;
            clbColumns.Enabled = cols;

            // CHỈNH SỬA: Nếu là ROLE thì dù Toggle thế nào cũng không cho tick Grant Option
            chkWithGrantOption.Enabled = (cbGranteeType.Text == "USER") && grantOpt;
            chkWithGrantOption.Checked = false;
        }
        #endregion
    }
}