using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System;

namespace QuanLyYTe.Forms.Doctor
{
    public class frmDoctorProfile : frmDoctorBase
    {
        private Panel pnlCard = new Panel();
        private Label lblName, lblGender, lblBirth, lblCmnd, lblHome, lblPhone, lblRole, lblSpec;

        public frmDoctorProfile()
        {
            // 1. Gỡ bỏ các thành phần của lớp Cha
            this.Controls.Remove(Dgv);
            this.Controls.Remove(pnlSearch);
            this.BackColor = Color.FromArgb(245, 245, 250);

            // 2. Tiêu đề trang
            Label title = new Label
            {
                Text = "HỒ SƠ CÁ NHÂN BÁC SĨ",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(40, 30),
                AutoSize = true,
                ForeColor = Color.FromArgb(45, 45, 45)
            };

            // 3. Thiết kế "Thẻ hồ sơ" (Profile Card)
            pnlCard = new Panel
            {
                Location = new Point(40, 85),
                Size = new Size(700, 420),
                BackColor = Color.White,
                Padding = new Padding(30)
            };

            // Vẽ viền nhẹ cho Thẻ
            pnlCard.Paint += (s, e) => {
                ControlPaint.DrawBorder(e.Graphics, pnlCard.ClientRectangle, Color.FromArgb(225, 225, 225), ButtonBorderStyle.Solid);
            };

            int startY = 35;
            int gap = 42;

            // Hiển thị 8 dòng thông tin liên tục, không chia cắt
            lblName = CreatePair(pnlCard, "Họ và tên:", startY);
            lblGender = CreatePair(pnlCard, "Giới tính:", startY + gap);
            lblBirth = CreatePair(pnlCard, "Ngày sinh:", startY + gap * 2);
            lblCmnd = CreatePair(pnlCard, "CMND/CCCD:", startY + gap * 3);
            lblHome = CreatePair(pnlCard, "Quê quán:", startY + gap * 4);
            lblPhone = CreatePair(pnlCard, "Số điện thoại:", startY + gap * 5);
            lblRole = CreatePair(pnlCard, "Vai trò:", startY + gap * 6);
            lblSpec = CreatePair(pnlCard, "Chuyên khoa:", startY + gap * 7);

            // 4. Nút cập nhật
            btnE.Text = "CHỈNH SỬA THÔNG TIN";
            btnE.Location = new Point(40, 525);
            btnE.Size = new Size(220, 45);
            btnE.BackColor = Color.Orange;
            btnE.ForeColor = Color.White;
            btnE.FlatStyle = FlatStyle.Flat;
            btnE.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnE.FlatAppearance.BorderSize = 0;
            btnE.Cursor = Cursors.Hand;
            btnE.Visible = true;

            this.Controls.Add(title);
            this.Controls.Add(pnlCard);
            this.Controls.Add(btnE);

            LoadProfile();
        }

        private Label CreatePair(Panel container, string labelText, int y)
        {
            Label lblLabel = new Label
            {
                Text = labelText,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(35, y),
                ForeColor = Color.DimGray,
                AutoSize = true
            };

            Label lblValue = new Label
            {
                Text = "...",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Location = new Point(190, y - 2),
                ForeColor = Color.Black,
                AutoSize = true
            };

            container.Controls.Add(lblLabel);
            container.Controls.Add(lblValue);
            return lblValue;
        }

        private void LoadProfile()
        {
            DataTable dt = Svc.GetSelfInfo();
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                lblName.Text = r["FULL_NAME"].ToString();

                string sex = r["GENDER"].ToString();
                lblGender.Text = (sex == "M" || sex == "Nam") ? "Nam" : "Nữ";

                if (r["BIRTHDATE"] != DBNull.Value)
                {
                    DateTime dob = Convert.ToDateTime(r["BIRTHDATE"]);
                    lblBirth.Text = dob.ToString("dd/MM/yyyy");
                }

                lblCmnd.Text = r["ID_CARD"].ToString();
                lblHome.Text = r["HOMETOWN"].ToString();
                lblPhone.Text = r["PHONE"].ToString();
                lblRole.Text = r["STAFF_ROLE"].ToString();
                lblSpec.Text = r["DEPT_NAME"].ToString();
            }
        }

        protected override void FormE()
        {
            DataTable dt = Svc.GetSelfInfo();
            if (dt.Rows.Count == 0) return;
            DataRow r = dt.Rows[0];

            var f = new Dictionary<string, string>{
                {"Họ tên (Không sửa)", r["FULL_NAME"].ToString()},
                {"Quê quán", r["HOMETOWN"].ToString()},
                {"Số điện thoại", r["PHONE"].ToString()}
            };

            ShowDialog("Cập nhật quê quán & SĐT", f, res => {
                Svc.UpdateSelfInfo(res["Quê quán"], res["Số điện thoại"]);
                LoadProfile();
            });
        }
    }
}