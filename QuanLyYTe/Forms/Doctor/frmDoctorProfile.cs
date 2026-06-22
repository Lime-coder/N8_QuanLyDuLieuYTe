using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System;

namespace QuanLyYTe.Forms.Doctor
{
    public partial class frmDoctorProfile : frmDoctorBase
    {
        public frmDoctorProfile()
        {
            InitializeComponent();
            pnlSearch.Visible = false;
            Dgv.Visible = false;
            LoadProfile();
        }

        private void pnlCard_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, pnlCard.ClientRectangle, Color.FromArgb(225, 225, 225), ButtonBorderStyle.Solid);
        }

        private void LoadProfile()
        {
            DataTable dt = Svc.GetSelfInfo();
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                lblMaNV.Text = r.Table.Columns.Contains("STAFF_ID") ? r["STAFF_ID"].ToString() : QuanLyYTe.Common.AppSession.CurrentUserId ?? "";
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
                lblFacility.Text = r["FACILITY"].ToString();
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
