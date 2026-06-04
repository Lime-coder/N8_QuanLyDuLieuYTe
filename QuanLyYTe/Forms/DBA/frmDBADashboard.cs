using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyYTe.Common;
using QuanLyYTe.Services;

namespace QuanLyYTe.Forms
{
    public partial class Dashboard : Form
    {

        private static readonly Color Orange = Color.FromArgb(255, 140, 40);
        private static readonly Color OrangeHov = Color.FromArgb(255, 165, 80);
        private static readonly Color ActiveBg = Color.FromArgb(45, 255, 140, 40);

        private Button _activeNavBtn = null;
        private readonly string _username;


        private (string Badge, string Title, string Desc, Func<Form> Factory)[] _features;

        public Dashboard(string username)
        {
            InitializeComponent();
            _username = username;

            _features = new (string Badge, string Title, string Desc, Func<Form> Factory)[]
            {
                ("USR", "Quản lý user và role",  "Tạo, sửa, xóa tài khoản người dùng Oracle", () => new frmUserManagement()),
                ("GRT", "Cấp Quyền",     "Cấp quyền cho user / role",                  () => new frmGrantPermission()),
                ("REV", "Xem và thu hồi quyền", "Xem và thu hồi quyền đã cấp",                       () => new frmRevokePermission()),
            };

            WireNavButtons();
        }


        private void Dashboard_Load(object sender, EventArgs e)
        {
            PositionUserInfo();
            pnlTopbar.Resize += (s, _) => PositionUserInfo();
            LoadUserInfo();
            BuildCards();
        }

        private void PositionUserInfo()
        {
            lblUserInfo.Location = new Point(
                pnlTopbar.Width - lblUserInfo.Width - 24,
                (pnlTopbar.Height - lblUserInfo.Height) / 2);
        }


        private void LoadUserInfo()
        {
            string role = AppSession.CurrentUserRole;

            if (role != null && role.StartsWith("RL_"))
            {
                role = role.Substring(3);
            }

            lblUserInfo.Text = $"{_username.ToUpper()}  ·  {role}";
        }


        private void BuildCards()
        {
            pnlCards.Controls.Clear();
            foreach (var f in _features)
            {
                var captured = f;
                var card = CreateCard(
                    captured.Badge,
                    captured.Title,
                    captured.Desc,
                    () => OpenFeature(captured.Title, "Dashboard / " + captured.Title, captured.Factory));
                pnlCards.Controls.Add(card);
            }
        }

        private Panel CreateCard(string badge, string title, string desc, Action onClick)
        {
            var card = new Panel
            {
                Size = new Size(195, 128),
                BackColor = Color.White,
                Margin = new Padding(0, 0, 14, 14),
                Cursor = Cursors.Hand,
            };

            var accent = new Panel
            {
                Dock = DockStyle.Top,
                Height = 4,
                BackColor = Orange,
            };

            var lblBadge = new Label
            {
                Text = badge,
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                AutoSize = false,
                Size = new Size(38, 22),
                Location = new Point(14, 18),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(28, 28, 32),
                AutoSize = false,
                Size = new Size(167, 20),
                Location = new Point(14, 50),
            };

            var lblDesc = new Label
            {
                Text = desc,
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = Color.FromArgb(140, 140, 150),
                AutoSize = false,
                Size = new Size(167, 40),
                Location = new Point(14, 74),
            };

            card.Controls.AddRange(new Control[] { accent, lblBadge, lblTitle, lblDesc });

            void OnEnter(object s, EventArgs e)
            {
                card.BackColor = Color.FromArgb(255, 252, 248);
                accent.BackColor = OrangeHov;
                lblBadge.BackColor = OrangeHov;
            }
            void OnLeave(object s, EventArgs e)
            {
                card.BackColor = Color.White;
                accent.BackColor = Orange;
                lblBadge.BackColor = Orange;
            }
            void OnClick(object s, EventArgs e) => onClick?.Invoke();

            foreach (Control c in new Control[] { card, accent, lblBadge, lblTitle, lblDesc })
            {
                c.MouseEnter += OnEnter;
                c.MouseLeave += OnLeave;
                c.Click += OnClick;
            }

            card.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(225, 223, 220), 1f);
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            return card;
        }


        private void WireNavButtons()
        {
            var map = new (Button Btn, int Index)[]
            {
                (btnNavUsers,    0),
                (btnNavGrant,    1),
                (btnNavRevoke,   2),
            };

            foreach (var (btn, idx) in map)
            {
                var b = btn;
                var i = idx;
                b.Click += (s, e) =>
                {
                    SetActiveNav(b);
                    var f = _features[i];
                    OpenFeature(f.Title, "Dashboard / " + f.Title, f.Factory);
                };
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất?", "Đăng xuất",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            new AuthService().Logout(); // clear session

            Application.Restart();
        }

        private void SetActiveNav(Button btn)
        {
            if (_activeNavBtn != null)
            {
                _activeNavBtn.ForeColor = Color.FromArgb(190, 190, 200);
                _activeNavBtn.BackColor = Color.Transparent;
            }
            btn.ForeColor = Orange;
            btn.BackColor = ActiveBg;
            _activeNavBtn = btn;
        }


        private void OpenFeature(string title, string breadcrumb, Func<Form> factory)
        {
            SetActivePage(title, breadcrumb);
            pnlWelcome.Visible = false;
            pnlCards.Visible = false;

            var toRemove = new System.Collections.Generic.List<Control>();
            foreach (Control ctrl in pnlContent.Controls)
                if (ctrl.Tag?.ToString() == "embedded")
                    toRemove.Add(ctrl);
            foreach (var ctrl in toRemove)
            {
                pnlContent.Controls.Remove(ctrl);
                ctrl.Dispose();
            }

            try
            {
                Form child = factory();
                child.TopLevel = false;
                child.FormBorderStyle = FormBorderStyle.None;
                child.Dock = DockStyle.Fill;
                child.Tag = "embedded";
                var scrollWrapper = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    Tag = "embedded"
                };
                scrollWrapper.Controls.Add(child);
                pnlContent.Controls.Add(scrollWrapper);
                scrollWrapper.BringToFront();
                child.Show();
            }
            catch (Exception ex)
            {
                var ph = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.White,
                    Tag = "embedded"
                };
                var lbl = new Label
                {
                    Text = $"Lỗi tải form:\n\n{ex.Message}",
                    Font = new Font("Segoe UI", 10f),
                    ForeColor = Color.FromArgb(180, 40, 40),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                };
                ph.Controls.Add(lbl);
                pnlContent.Controls.Add(ph);
            }
        }

        private void SetActivePage(string title, string breadcrumb)
        {
            lblPageTitle.Text = title;
            lblPageBreadcrumb.Text = breadcrumb;
        }


        private void lblAppTitle_Click(object sender, EventArgs e)
        {
            SetActivePage("Dashboard", "Trang chủ");
            pnlWelcome.Visible = true;
            pnlCards.Visible = true;

            var toRemove = new System.Collections.Generic.List<Control>();
            foreach (Control ctrl in pnlContent.Controls)
                if (ctrl.Tag?.ToString() == "embedded")
                    toRemove.Add(ctrl);
            foreach (var ctrl in toRemove)
            {
                pnlContent.Controls.Remove(ctrl);
                ctrl.Dispose();
            }

            if (_activeNavBtn != null)
            {
                _activeNavBtn.ForeColor = Color.FromArgb(190, 190, 200);
                _activeNavBtn.BackColor = Color.Transparent;
                _activeNavBtn = null;
            }
        }
    }
}