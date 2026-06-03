using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyYTe.Services;
using QuanLyYTe.Helpers;

namespace QuanLyYTe.Forms.Doctor
{
    public class frmDoctorBase : Form
    {
        protected readonly DoctorService Svc = new DoctorService();
        protected DataGridView Dgv = new DataGridView {
            Dock = DockStyle.Fill,
            BackgroundColor = Color.White, 
            BorderStyle = BorderStyle.None
        };

        protected TextBox TxtS = new TextBox { 
            Width = 200, Location = new Point(70, 25) 
        };

        protected Button btnS, btnA, btnE, btnD;

        public frmDoctorBase()
        {
            Color bg = Color.FromArgb(242, 242, 242);
            Panel p = new Panel { 
                Dock = DockStyle.Top, Height = 75, BackColor = bg 
            };

            Label lblS = new Label { 
                Text = "Tìm:", Location = new Point(25, 28), AutoSize = true, BackColor = bg 
            };

            btnS = CreateBtn("Tìm", Color.Gray, 290); btnS.Click += (s, e) => LoadD();
            btnA = CreateBtn("Tạo", Color.DodgerBlue, 405); btnA.Click += (s, e) => FormA();
            btnE = CreateBtn("Sửa", Color.Gold, 520); btnE.Click += (s, e) => FormE();
            btnD = CreateBtn("Xóa", Color.Crimson, 635); btnD.Click += (s, e) => FormD();

            p.Controls.AddRange(new Control[] { lblS, TxtS, btnS, btnA, btnE, btnD });
            this.Controls.AddRange(new Control[] { Dgv, p });
        }

        protected virtual void LoadD() { }
        protected virtual void FormA() { }
        protected virtual void FormE() { }
        protected virtual void FormD() { }

        protected Button CreateBtn(string t, Color c, int x) => new Button { 
            Text = t, BackColor = c, 
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
                Text = t, Size = new Size(550, 680), 
                StartPosition = FormStartPosition.CenterParent, 
                FormBorderStyle = FormBorderStyle.FixedDialog, 
                MaximizeBox = false 
            };
            int y = 25;
            var boxes = new Dictionary<string, TextBox>();

            foreach (var k in f.Keys)
            {
                string labelText = k.Replace("(Không sửa)", "").Trim();
                d.Controls.Add(new Label { 
                    Text = labelText + ":", 
                    Location = new Point(25, y), 
                    AutoSize = true, 
                    Font = new Font("Segoe UI", 9, FontStyle.Bold) 
                });

                var b = new TextBox { Text = f[k], Location = new Point(190, y - 3), Width = 300 };

                if (k.Contains("Không sửa"))
                {
                    b.ReadOnly = true; b.BackColor = Color.FromArgb(235, 235, 235); b.TabStop = false;
                }

                if (k.Contains("Tiền sử") || k.Contains("Dị ứng") || k.Contains("Chẩn đoán") || k.Contains("Điều trị") || k.Contains("Kết quả") || k.Contains("Kết luận"))
                {
                    b.Multiline = true; b.Height = 100; b.ScrollBars = ScrollBars.Vertical;
                    y += 120;
                }
                else
                {
                    y += 45;
                }
                boxes.Add(k, b); d.Controls.Add(b);
            }

            Button btn = new Button { 
                Text = "LƯU DỮ LIỆU", 
                Location = new Point(190, y + 10), 
                Size = new Size(130, 45), 
                BackColor = Color.Orange, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            btn.Click += (se, ee) => {
                var res = new Dictionary<string, string>();

                foreach (var k in boxes.Keys)
                {
                    string value = boxes[k].Text.Trim();
                    
                    if (!boxes[k].ReadOnly && string.IsNullOrWhiteSpace(value))
                    {
                        MessageBox.Show($"Vui lòng nhập đầy đủ thông tin cho trường '{k.Replace("(Không sửa)", "")}'!",
                                        "Dữ liệu trống", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        boxes[k].Focus();
                        return;
                    }
                    res.Add(k, value);
                }

                try
                {
                    s(res);
                    MessageBox.Show("Thao tác thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    d.Close(); LoadD();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ParseOracleError(ex.Message), "Cảnh báo dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            d.Controls.Add(btn); d.ShowDialog();
        }

        private string ParseOracleError(string rawError)
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