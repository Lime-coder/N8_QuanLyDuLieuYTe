using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyYTe.Forms
{
    partial class SecurityAdminForm
    {
        // ── Controls ─────────────────────────────────────────────────
        private TabControl tabMain;
        private TabPage tabUsers, tabRoles;
        private DataGridView dgvUsers, dgvRoles;

        private Button btnCreateUser, btnEditUser, btnDropUser, btnRefreshUsers;
        private Button btnCreateRole, btnEditRole, btnDropRole, btnRefreshRoles;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Text = "Quản trị CSDL Oracle – Users & Roles";
            Size = new Size(920, 580);
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(700, 450);

            tabMain = new TabControl { Dock = DockStyle.Fill };
            tabUsers = new TabPage("👤  Người dùng (Users)");
            tabRoles = new TabPage("🔑  Vai trò (Roles)");
            tabMain.TabPages.AddRange(new[] { tabUsers, tabRoles });
            Controls.Add(tabMain);

            BuildUserTab();
            BuildRoleTab();
        }

        #endregion

        // ─────────────── TAB USERS ───────────────────────────────────
        private void BuildUserTab()
        {
            dgvUsers = CreateGrid();
            dgvUsers.Dock = DockStyle.Fill;

            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 46,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(6, 6, 0, 0)
            };

            btnCreateUser = MakeBtn("➕ Tạo User", Color.FromArgb(40, 167, 69));
            btnEditUser = MakeBtn("✏️ Sửa User", Color.FromArgb(0, 123, 255));
            btnDropUser = MakeBtn("🗑️ Xóa User", Color.FromArgb(220, 53, 69));
            btnRefreshUsers = MakeBtn("🔄 Làm mới", Color.FromArgb(108, 117, 125));

            btnCreateUser.Click += (s, e) => ShowCreateUserDialog();
            btnEditUser.Click += (s, e) => ShowEditUserDialog();
            btnDropUser.Click += (s, e) => DropSelectedUser();
            btnRefreshUsers.Click += (s, e) => LoadUsers();

            panel.Controls.AddRange(new Control[]
                { btnCreateUser, btnEditUser, btnDropUser, btnRefreshUsers });

            tabUsers.Controls.Add(dgvUsers);
            tabUsers.Controls.Add(panel);
        }

        // ─────────────── TAB ROLES ───────────────────────────────────
        private void BuildRoleTab()
        {
            dgvRoles = CreateGrid();
            dgvRoles.Dock = DockStyle.Fill;

            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 46,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(6, 6, 0, 0)
            };

            btnCreateRole = MakeBtn("➕ Tạo Role", Color.FromArgb(40, 167, 69));
            btnEditRole = MakeBtn("✏️ Sửa Role", Color.FromArgb(0, 123, 255));
            btnDropRole = MakeBtn("🗑️ Xóa Role", Color.FromArgb(220, 53, 69));
            btnRefreshRoles = MakeBtn("🔄 Làm mới", Color.FromArgb(108, 117, 125));

            btnCreateRole.Click += (s, e) => ShowCreateRoleDialog();
            btnEditRole.Click += (s, e) => ShowEditRoleDialog();
            btnDropRole.Click += (s, e) => DropSelectedRole();
            btnRefreshRoles.Click += (s, e) => LoadRoles();

            panel.Controls.AddRange(new Control[]
                { btnCreateRole, btnEditRole, btnDropRole, btnRefreshRoles });

            tabRoles.Controls.Add(dgvRoles);
            tabRoles.Controls.Add(panel);
        }

        // ── Factory helpers ──────────────────────────────────────────
        private static DataGridView CreateGrid() => new DataGridView
        {
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            RowHeadersVisible = false,
            Font = new Font("Segoe UI", 9.5f)
        };

        private static Button MakeBtn(string text, Color back) => new Button
        {
            Text = text,
            Height = 30,
            Width = 148,
            BackColor = back,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9f, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
    }
}
