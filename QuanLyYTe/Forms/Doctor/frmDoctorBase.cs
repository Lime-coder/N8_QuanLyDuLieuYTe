using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using QuanLyYTe.Services;

namespace QuanLyYTe.Forms.Doctor
{
    public partial class frmDoctorBase : Form
    {
        protected readonly DoctorService Svc = new DoctorService();

        public frmDoctorBase()
        {
            InitializeComponent();
            btnS = CreateBtn("Tìm", Color.Gray, 290);
            btnS.Click += btnS_Click;
            btnA = CreateBtn("Thêm", Color.DodgerBlue, 405);
            btnA.Click += btnA_Click;
            btnE = CreateBtn("Sửa", Color.Gold, 520);
            btnE.Click += btnE_Click;
            btnD = CreateBtn("Xóa", Color.Crimson, 635);
            btnD.Click += btnD_Click;
            pnlSearch.Controls.AddRange(new Control[] { btnS, btnA, btnE, btnD });
        }

        private void btnS_Click(object sender, EventArgs e) => LoadD();
        private void btnA_Click(object sender, EventArgs e) => FormA();
        private void btnE_Click(object sender, EventArgs e) => FormE();
        private void btnD_Click(object sender, EventArgs e) => FormD();

        protected virtual void LoadD() { }
        protected virtual void FormA() { }
        protected virtual void FormE() { }
        protected virtual void FormD() { }

        protected Button CreateBtn(string t, Color c, int x) => new Button { 
            Text = t, 
            BackColor = c, 
            ForeColor = Color.White, 
            FlatStyle = FlatStyle.Flat, 
            Location = new Point(x, 20), 
            Size = new Size(105, 35), 
            Font = new Font("Segoe UI", 9, FontStyle.Bold), 
            Cursor = Cursors.Hand 
        };

        protected void ShowDialog(string t, Dictionary<string, string> f, Action<Dictionary<string, string>> s)
        {
            Form d = new Form { 
                Text = t, 
                Width = 520, 
                StartPosition = FormStartPosition.CenterParent, 
                FormBorderStyle = FormBorderStyle.FixedDialog, 
                MaximizeBox = false 
            };

            int y = 20;
            var boxes = new Dictionary<string, TextBox>();
            foreach (var k in f.Keys)
            {
                d.Controls.Add(new Label { 
                    Text = k.Replace("(Không sửa)", "") + ":", 
                    Location = new Point(25, y), 
                    AutoSize = true, 
                    Font = new Font("Segoe UI", 9, FontStyle.Bold) 
                });

                var b = new TextBox { 
                    Text = f[k], 
                    Location = new Point(175, y - 3), 
                    Width = 300 
                };

                if (k.Contains("Không sửa")) { 
                    b.ReadOnly = true; 
                    b.BackColor = Color.FromArgb(235, 235, 235); 
                }

                if (k.Contains("Chẩn đoán") || k.Contains("Điều trị") || k.Contains("Kết luận") || k.Contains("Tiền sử") || k.Contains("Dị ứng") || k.Contains("Kết quả"))
                {
                    b.Multiline = true; b.Height = 85; y += 100;
                }
                else { 
                    y += 40; 
                }

                boxes.Add(k, b); d.Controls.Add(b);
            }

            Button btn = new Button { 
                Text = "LƯU DỮ LIỆU", 
                Location = new Point(175, y + 10), 
                Size = new Size(130, 40), 
                BackColor = Color.Orange, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat, 
                Font = new Font("Segoe UI", 9, FontStyle.Bold) 
            };

            btn.Click += (se, ee) => {
                var res = new Dictionary<string, string>();
                foreach (var k in boxes.Keys)
                {
                    if (!boxes[k].ReadOnly && string.IsNullOrWhiteSpace(boxes[k].Text))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        boxes[k].Focus(); return;
                    }
                    res.Add(k, boxes[k].Text.Trim());
                }
                try
                {
                    s(res);
                    MessageBox.Show("Thao tác thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    d.Close();
                    LoadD();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ParseOracleError(ex.Message), "Cảnh báo dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            d.Controls.Add(btn); d.Height = btn.Bottom + 50; d.ShowDialog();
        }

        protected string ParseOracleError(string rawError)
        {
            try
            {
                if (rawError.Contains("ORA-20"))
                {
                    int oraIdx = rawError.IndexOf("ORA-20");
                    int firstColon = rawError.IndexOf(':', oraIdx);
                    if (firstColon != -1)
                    {
                        int nextOra = rawError.IndexOf("ORA-", firstColon + 1);
                        string finalMsg = (nextOra != -1)
                            ? rawError.Substring(firstColon + 1, nextOra - firstColon - 1).Trim()
                            : rawError.Substring(firstColon + 1).Trim();
                        if (!string.IsNullOrWhiteSpace(finalMsg)) return finalMsg;
                    }
                }

                if (rawError.Contains("ORA-01400") || rawError.Contains("ORA-01407") || rawError.Contains("ORA-01403"))
                    return "Lỗi: Bạn không được để trống các thông tin bắt buộc!";

                if (rawError.Contains("ORA-02291")) return "Lỗi: Mã tham chiếu (Mã BN/HSBA) không tồn tại!";
                if (rawError.Contains("ORA-00001")) return "Lỗi: Dữ liệu bị trùng lặp, vui lòng kiểm tra lại!";
                if (rawError.Contains("ORA-12899")) return "Lỗi: Dữ liệu nhập vào quá dài so với quy định!";
            }
            catch { }

            return "Phát hiện lỗi: " + (rawError.Length > 100 ? rawError.Substring(0, 100) + "..." : rawError);
        }
    }
}