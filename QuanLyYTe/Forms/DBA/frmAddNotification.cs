using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using QuanLyYTe.Repositories;

namespace QuanLyYTe.Forms.DBA
{
    public partial class frmAddNotification : Form
    {
        private readonly NotificationRepository _repo = new NotificationRepository();

        public frmAddNotification()
        {
            InitializeComponent();
            
            // Hook up events
            radBGD.CheckedChanged += OnLabelComponentChanged;
            radLDK.CheckedChanged += OnLabelComponentChanged;
            radNV.CheckedChanged += OnLabelComponentChanged;
            
            chkTH.CheckedChanged += OnLabelComponentChanged;
            chkTK.CheckedChanged += OnLabelComponentChanged;
            chkTM.CheckedChanged += OnLabelComponentChanged;
            
            chkHCM.CheckedChanged += OnLabelComponentChanged;
            chkHN.CheckedChanged += OnLabelComponentChanged;
            chkHP.CheckedChanged += OnLabelComponentChanged;
            
            btnSubmit.Click += btnSubmit_Click;
            
            // Initial preview update
            UpdatePreview();
        }

        private void frmAddNotification_Resize(object sender, EventArgs e)
        {
            // Center the container panel
            pnlContainer.Location = new Point(
                (this.ClientSize.Width - pnlContainer.Width) / 2,
                (this.ClientSize.Height - pnlContainer.Height) / 2
            );
        }

        private void OnLabelComponentChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private string BuildOlsLabel()
        {
            // 1. Lấy Level (Bắt buộc)
            string level = "NV"; // default
            if (radBGD.Checked) level = "BGD";
            else if (radLDK.Checked) level = "LDK";
            else if (radNV.Checked) level = "NV";

            // 2. Lấy Compartments (Khoa)
            List<string> compartments = new List<string>();
            if (chkTH.Checked) compartments.Add("TH");
            if (chkTK.Checked) compartments.Add("TK");
            if (chkTM.Checked) compartments.Add("TM");

            // 3. Lấy Groups (Cơ sở)
            List<string> groups = new List<string>();
            if (chkHCM.Checked) groups.Add("HCM");
            if (chkHN.Checked) groups.Add("HN");
            if (chkHP.Checked) groups.Add("HP");

            // 4. Lắp ráp chuỗi OLS
            string finalLabel = level;

            // Nếu có chọn Khoa, thêm dấu ':' và nối các khoa bằng dấu ','
            if (compartments.Count > 0)
            {
                finalLabel += ":" + string.Join(",", compartments);
            }
            else if (groups.Count > 0)
            {
                // Nếu không chọn Khoa nhưng CÓ chọn Cơ sở, OLS bắt buộc phải có đủ số lượng dấu ':' 
                finalLabel += ":"; 
            }

            // Nếu có chọn Cơ sở, thêm dấu ':' và nối các cơ sở bằng dấu ','
            if (groups.Count > 0)
            {
                finalLabel += ":" + string.Join(",", groups);
            }

            return finalLabel; 
        }

        private void AppendPreviewText(string text, bool highlight)
        {
            rtbPreview.SelectionStart = rtbPreview.TextLength;
            rtbPreview.SelectionLength = 0;
            
            if (highlight)
            {
                rtbPreview.SelectionColor = Color.FromArgb(255, 140, 40); // Orange
                rtbPreview.SelectionFont = new Font(rtbPreview.Font, FontStyle.Bold);
            }
            else
            {
                rtbPreview.SelectionColor = Color.DimGray;
                rtbPreview.SelectionFont = new Font(rtbPreview.Font, FontStyle.Italic);
            }
            
            rtbPreview.AppendText(text);
        }

        private void UpdatePreview()
        {
            rtbPreview.Clear();

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

            AppendPreviewText("Thông báo này sẽ hiển thị cho ", false);
            AppendPreviewText(levelText, true);

            if (compText.Count > 0)
            {
                AppendPreviewText(" thuộc ", false);
                AppendPreviewText(string.Join(", ", compText), true);
            }
            else
            {
                AppendPreviewText(" thuộc ", false);
                AppendPreviewText("Bất kỳ khoa nào", true);
            }

            if (groupText.Count > 0)
            {
                AppendPreviewText(" tại cơ sở ", false);
                AppendPreviewText(string.Join(", ", groupText), true);
                AppendPreviewText(".", false);
            }
            else
            {
                AppendPreviewText(" tại ", false);
                AppendPreviewText("Bất kỳ cơ sở nào", true);
                AppendPreviewText(".", false);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string olsLabel = BuildOlsLabel();
            string noiDung = txtDescription.Text.Trim();
            string diaDiem = txtLocation.Text.Trim();

            if (string.IsNullOrEmpty(noiDung) || string.IsNullOrEmpty(diaDiem))
            {
                MessageBox.Show("Vui lòng điền đầy đủ Nội dung và Địa điểm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _repo.AddNotification(noiDung, diaDiem, olsLabel);
                MessageBox.Show($"Đã tạo thông báo với nhãn bảo mật:\n{olsLabel}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Clear fields
                txtDescription.Text = "";
                txtLocation.Text = "";
                // Reset check/radio
                radNV.Checked = true;
                chkTH.Checked = false;
                chkTK.Checked = false;
                chkTM.Checked = false;
                chkHCM.Checked = false;
                chkHN.Checked = false;
                chkHP.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tạo thông báo:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
