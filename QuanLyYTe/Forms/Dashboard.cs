using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLyYTe.Forms
{
    public partial class Dashboard : Form
    {
        // ── Theme ─────────────────────────────────────────────────────
        private static readonly Color Orange = Color.FromArgb(255, 140, 40);
        private static readonly Color OrangeHover = Color.FromArgb(255, 165, 80);
        private static readonly Color ActiveBg = Color.FromArgb(45, 255, 140, 40);

        private Button _activeNavBtn = null;

        // ── Feature definitions ───────────────────────────────────────
        private readonly (string Label, string Title, string Desc, Func<Form> Factory)[] _features;

        public Dashboard()
        {
            InitializeComponent();

            _features = new (string, string, string, Func<Form>)[]
            {
                ("USR", "Quản lý User",  "Tạo, sửa, xóa tài khoản người dùng Oracle",    () => new frmUserMangement()),
                ("ROL", "Quản lý Role",  "Tạo, sửa, xóa role trong hệ thống Oracle",     () => new frmRoleManagement()),
                ("GRT", "Cấp Quyền",     "Cấp quyền cho user / role, WITH GRANT OPTION",  () => new frmGrantPermission()),
                ("REV", "Thu Hồi Quyền", "Thu hồi quyền đã cấp từ user hoặc role",        () => new frmRevokePermission()),
                ("VIW", "Xem Quyền",     "Xem chi tiết quyền của mỗi user / role",        () => new frmViewPermissions()),
            };

            BuildCards();
            WireNavButtons();
        }

        // ── Build quick-launch cards ──────────────────────────────────
        private void BuildCards()
        {
            pnlCards.Controls.Clear();

            foreach (var (label, title, desc, factory) in _features)
            {
                var capturedTitle = title;
                var capturedFactory = factory; 
                var card = CreateCard(label, title, desc,
                    () => OpenFeature(capturedTitle, "Dashboard / " + capturedTitle, capturedFactory));
                pnlCards.Controls.Add(card);
            }
        }

        private Panel CreateCard(string shortLabel, string title, string desc, Action onClick)
        {
            var card = new Panel
            {
                Size = new Size(195, 128),
                BackColor = Color.White,
                // FlowLayoutPanel uses Margin for spacing between cards
                Margin = new Padding(0, 0, 14, 14),
                Cursor = Cursors.Hand,
            };

            // Orange top accent bar
            var accent = new Panel
            {
                Dock = DockStyle.Top,
                Height = 4,
                BackColor = Orange,
            };

            // Short text badge instead of emoji (reliable across all systems)
            var lblBadge = new Label
            {
                Text = shortLabel,
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

            // Hover/click wiring on every child so the whole card responds
            void OnEnter(object s, EventArgs e) { card.BackColor = Color.FromArgb(255, 252, 248); accent.BackColor = OrangeHover; lblBadge.BackColor = OrangeHover; }
            void OnLeave(object s, EventArgs e) { card.BackColor = Color.White; accent.BackColor = Orange; lblBadge.BackColor = Orange; }
            void OnClick(object s, EventArgs e) => onClick?.Invoke();

            foreach (Control ctrl in new Control[] { card, accent, lblBadge, lblTitle, lblDesc })
            {
                ctrl.MouseEnter += OnEnter;
                ctrl.MouseLeave += OnLeave;
                ctrl.Click += OnClick;
            }

            // Subtle border
            card.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(225, 223, 220), 1f);
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            return card;
        }

        // ── Wire sidebar nav buttons ──────────────────────────────────
        private void WireNavButtons()
        {
            var map = new (Button Btn, int FeatureIndex)[]
            {
                (btnNavUsers,    0),
                (btnNavRoles,    1),
                (btnNavGrant,    2),
                (btnNavRevoke,   3),
                (btnNavPermView, 4),
            };

            foreach (var (btn, idx) in map)
            {
                var capturedBtn = btn;
                var capturedIdx = idx;
                btn.Click += (s, e) =>
                {
                    SetActiveNav(capturedBtn);
                    var f = _features[capturedIdx];
                    OpenFeature(f.Title, "Dashboard / " + f.Title, f.Factory);
                };
            }
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

        // ── Open a feature form embedded in pnlContent ────────────────
        private void OpenFeature(string title, string breadcrumb, Func<Form> factory)
        {
            lblPageTitle.Text = title;
            lblPageBreadcrumb.Text = breadcrumb;

            // Hide home view
            pnlWelcome.Visible = false;
            pnlCards.Visible = false;

            // Remove any previously embedded form
            var toRemove = new System.Collections.Generic.List<Control>();
            foreach (Control ctrl in pnlContent.Controls)
                if (ctrl.Tag?.ToString() == "embedded")
                    toRemove.Add(ctrl);
            foreach (var ctrl in toRemove) { pnlContent.Controls.Remove(ctrl); ctrl.Dispose(); }

            try
            {
                Form child = factory();
                child.TopLevel = false;
                child.FormBorderStyle = FormBorderStyle.None;
                child.Dock = DockStyle.Fill;
                child.Tag = "embedded";

                pnlContent.Controls.Add(child);
                child.BringToFront();
                child.Show();
            }
            catch (Exception ex)
            {
                // Placeholder when form not yet connected
                var ph = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Tag = "embedded" };
                var lbl = new Label
                {
                    Text = $"⚠  Form [{title}] chưa được kết nối.\n\n{ex.Message}",
                    Font = new Font("Segoe UI", 10f),
                    ForeColor = Color.FromArgb(160, 100, 40),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                };
                ph.Controls.Add(lbl);
                pnlContent.Controls.Add(ph);
            }
        }

        // ── Return to home dashboard ──────────────────────────────────
        private void ShowHome()
        {
            lblPageTitle.Text = "Dashboard";
            lblPageBreadcrumb.Text = "Trang chủ";
            pnlWelcome.Visible = true;
            pnlCards.Visible = true;

            var toRemove = new System.Collections.Generic.List<Control>();
            foreach (Control ctrl in pnlContent.Controls)
                if (ctrl.Tag?.ToString() == "embedded") toRemove.Add(ctrl);
            foreach (var ctrl in toRemove) { pnlContent.Controls.Remove(ctrl); ctrl.Dispose(); }

            if (_activeNavBtn != null)
            {
                _activeNavBtn.ForeColor = Color.FromArgb(190, 190, 200);
                _activeNavBtn.BackColor = Color.Transparent;
                _activeNavBtn = null;
            }
        }

        // Click logo label → go home
        private void lblAppTitle_Click(object sender, EventArgs e) => ShowHome();
    }
}