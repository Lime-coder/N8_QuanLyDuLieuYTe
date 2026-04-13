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
            _repository = new PrivilegeRepository(); // Sử dụng Repository từ DAL

            // Đăng ký sự kiện
            cbGranteeType.SelectedIndexChanged += (s, e) => LoadGrantees();
            cbObjectType.SelectedIndexChanged += (s, e) => LoadObjects();
            lbObjects.SelectedIndexChanged += (s, e) => LoadColumns();
            chkSelect.CheckedChanged += PrivilegeCheckBox_CheckedChanged;
            chkUpdate.CheckedChanged += PrivilegeCheckBox_CheckedChanged;
            btnGrant.Click += btnGrant_Click;

            // Khởi tạo mặc định
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
                // Dựa trên SP: SELECT UPPER(username/role) trả về cột đầu tiên
                cbGranteeName.DisplayMember = dt.Columns[0].ColumnName;
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
                lbObjects.DisplayMember = dt.Columns[0].ColumnName; // UPPER(OBJECT_NAME)

                // Cấu hình UI theo loại đối tượng
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
                    // Lấy giá trị chuỗi từ DataRowView của ListBox
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
            // Chỉ bật phân quyền mức cột cho TABLE/VIEW khi chọn SELECT/UPDATE
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

        private void btnGrant_Click(object? sender, EventArgs e)
        {
            // 1. Kiểm tra đầu vào
            if (cbGranteeName.SelectedValue == null || lbObjects.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Người nhận và Đối tượng!");
                return;
            }

            if (!(chkSelect.Checked || chkInsert.Checked || chkUpdate.Checked || chkDelete.Checked || chkExecute.Checked))
            {
                MessageBox.Show("Vui lòng chọn ít nhất một quyền!");
                return;
            }

            // 2. Thu thập dữ liệu
            string grantee = cbGranteeName.GetItemText(cbGranteeName.SelectedItem);
            string objName = lbObjects.GetItemText(lbObjects.SelectedItem);
            int withGrant = chkWithGrantOption.Checked ? 1 : 0;

            // Lấy danh sách cột
            string colList = null;
            if (clbColumns.Enabled && clbColumns.CheckedItems.Count > 0)
            {
                var cols = clbColumns.CheckedItems.Cast<string>().ToList();
                colList = string.Join(",", cols);
            }

            // 3. Thực thi gọi tầng DAL
            try
            {
                if (chkSelect.Checked) _repository.GrantObjectPrivilege(grantee, "SELECT", objName, colList, withGrant);
                if (chkInsert.Checked) _repository.GrantObjectPrivilege(grantee, "INSERT", objName, null, withGrant);
                if (chkUpdate.Checked) _repository.GrantObjectPrivilege(grantee, "UPDATE", objName, colList, withGrant);
                if (chkDelete.Checked) _repository.GrantObjectPrivilege(grantee, "DELETE", objName, null, withGrant);
                if (chkExecute.Checked) _repository.GrantObjectPrivilege(grantee, "EXECUTE", objName, null, withGrant);

                MessageBox.Show($"Đã thực hiện cấp quyền cho {grantee} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi thực thi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TogglePrivileges(bool tablePrivs, bool cols, bool grantOpt)
        {
            chkSelect.Enabled = chkInsert.Enabled = chkUpdate.Enabled = chkDelete.Enabled = tablePrivs;
            chkSelect.Checked = chkInsert.Checked = chkUpdate.Checked = chkDelete.Checked = false;
            clbColumns.Enabled = cols;
            chkWithGrantOption.Enabled = grantOpt;
            chkWithGrantOption.Checked = false;
        }
        #endregion
    }
}