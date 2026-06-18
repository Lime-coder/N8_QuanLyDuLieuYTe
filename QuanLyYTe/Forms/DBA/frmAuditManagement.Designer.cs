namespace QuanLyYTe.Forms.DBA
{
    partial class frmAuditManagement
    {
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
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.Size = new Size(1150, 700);

            pnlHeader = new Panel { Dock = DockStyle.Top, Height = 120, BackColor = Color.White };

            lblTitle = new Label
            {
                Text = "NHẬT KÝ KIỂM TOÁN (AUDITING)",
                ForeColor = Color.FromArgb(255, 140, 40),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 22),
                AutoSize = true
            };

            lblSearch = new Label
            {
                Text = "Tìm:",
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Location = new Point(20, 75),
                AutoSize = true
            };

            txtSearch = new TextBox
            {
                Width = 300,
                Location = new Point(65, 73),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle
            };

            btnSearch = new Button
            {
                Text = "Tìm",
                Size = new Size(80, 32),
                Location = new Point(375, 73),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            cboAuditType = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 250,
                Location = new Point(480, 73),
                Font = new Font("Segoe UI", 11)
            };
            cboAuditType.Items.AddRange(new object[] {
                "Kiểm toán hệ thống",
                "Cập nhật Đơn thuốc",
                "Cập nhật thông tin HSBA",
                "Thay đổi thông tin Dịch vụ"
            });
            cboAuditType.SelectedIndex = 0;
            cboAuditType.SelectedIndexChanged += new System.EventHandler(this.cboAuditType_SelectedIndexChanged);

            btnRefresh = new Button
            {
                Text = "Làm mới",
                Size = new Size(100, 35),
                Location = new Point(750, 71),
                BackColor = Color.FromArgb(255, 140, 40),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            dgvAudit = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                EnableHeadersVisualStyles = false,
                GridColor = Color.Black,
            };

            dgvAudit.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvAudit.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvAudit.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvAudit.ColumnHeadersHeight = 40;

            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblSearch, txtSearch, btnSearch, cboAuditType, btnRefresh });
            this.Controls.AddRange(new Control[] { dgvAudit, pnlHeader });

            btnExport = new Button
            {
                Text = "Xuất CSV",
                Size = new Size(100, 35),
                Location = new Point(860, 71),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += new System.EventHandler(this.btnExport_Click);

            pnlHeader.Controls.Add(btnExport);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblSearch;
        private TextBox txtSearch;
        private Button btnSearch;
        private ComboBox cboAuditType;
        private Button btnRefresh;
        private DataGridView dgvAudit;
        private Button btnExport;
    }
}
