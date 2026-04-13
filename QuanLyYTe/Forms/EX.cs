using System;
using System.Data;
using System.Windows.Forms;

namespace QuanLyYTe.Forms
{
    public partial class EX : Form
    {
        public EX()
        {
            InitializeComponent();
        }

        private void GrantPermissionForm_Load(object sender, EventArgs e)
        {
            cbObjectType.SelectedIndex = 0;
            cbGranteeType.SelectedIndex = 0;
            LoadMockDataForRoleTab();

            // Đảm bảo khung cột luôn hiện từ đầu
            gbColumnSelection.Visible = true;
            clbCols.Visible = true;
        }

        private void clbPrivs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Ngay khi nhấn vào SELECT hoặc UPDATE, chỉ cập nhật lại danh sách cột chứ không ẩn khung
            this.BeginInvoke(new Action(() => {
                if (cbObjectType.Text == "TABLE")
                {
                    LoadDummyCols();
                }
            }));
        }

        private void cbObjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Khi chọn bảng khác, tự nạp lại cột
            LoadDummyCols();
        }

        private void LoadDummyCols()
        {
            clbCols.Items.Clear();
            string tableName = cbObjectName.Text;
            if (tableName == "BENHNHAN")
                clbCols.Items.AddRange(new string[] { "MABN", "TENBN", "CCCD", "DIACHI" });
            else if (tableName == "NHANVIEN")
                clbCols.Items.AddRange(new string[] { "MANV", "HOTEN", "LUONG" });
            else
                clbCols.Items.AddRange(new string[] { "COL_1", "COL_2", "COL_3" });
        }

        // --- CÁC HÀM CÒN LẠI GIỮ NGUYÊN ---
        private void cbGranteeType_SelectedIndexChanged(object sender, EventArgs e) { /* code của em */ }
        private void cbObjectType_SelectedIndexChanged(object sender, EventArgs e) { /* code của em */ }
        private void btnGrantObj_Click(object sender, EventArgs e) { /* code của em */ }
        private void LoadMockDataForRoleTab() { /* code của em */ }
        private void btnGrantRole_Click(object sender, EventArgs e) { /* code của em */ }
    }
}