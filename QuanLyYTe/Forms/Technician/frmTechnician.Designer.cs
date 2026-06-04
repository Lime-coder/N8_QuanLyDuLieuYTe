namespace QuanLyYTe.Forms.Technician
{
    partial class frmTechnician
    {
        /// <summary>
        ///
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
            panelHeader = new Panel();
            label1 = new Label();
            label2 = new Label();
            panelActions = new Panel();
            btnRefresh = new Button();
            btnNewService = new Button();
            dgvServices = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvServices).BeginInit();
            panelHeader.SuspendLayout();
            panelActions.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(30, 41, 59);
            panelHeader.Controls.Add(label2);
            panelHeader.Controls.Add(label1);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1000, 84);
            panelHeader.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(18, 14);
            label1.Name = "label1";
            label1.Size = new Size(253, 37);
            label1.TabIndex = 0;
            label1.Text = "Kỹ thuật viên";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = Color.Gainsboro;
            label2.Location = new Point(21, 49);
            label2.Name = "label2";
            label2.Size = new Size(302, 20);
            label2.TabIndex = 1;
            label2.Text = "Xem các dịch vụ được phân công và cập nhật kết quả";
            // 
            // panelActions
            // 
            panelActions.Controls.Add(btnRefresh);
            panelActions.Controls.Add(btnNewService);
            panelActions.Dock = DockStyle.Bottom;
            panelActions.Location = new Point(0, 554);
            panelActions.Name = "panelActions";
            panelActions.Padding = new Padding(16, 10, 16, 10);
            panelActions.Size = new Size(1000, 66);
            panelActions.TabIndex = 1;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.FromArgb(15, 118, 110);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(856, 12);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 38);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += new EventHandler(btnRefresh_Click);
            // 
            // btnNewService
            // 
            btnNewService.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNewService.BackColor = Color.FromArgb(37, 99, 235);
            btnNewService.FlatAppearance.BorderSize = 0;
            btnNewService.FlatStyle = FlatStyle.Flat;
            btnNewService.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnNewService.ForeColor = Color.White;
            btnNewService.Location = new Point(670, 12);
            btnNewService.Name = "btnNewService";
            btnNewService.Size = new Size(180, 38);
            btnNewService.TabIndex = 0;
            btnNewService.Text = "Cập nhật kết quả";
            btnNewService.UseVisualStyleBackColor = false;
            btnNewService.Click += new EventHandler(btnNewService_Click);
            // 
            // dgvServices
            // 
            dgvServices.AllowUserToAddRows = false;
            dgvServices.AllowUserToDeleteRows = false;
            dgvServices.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvServices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvServices.BackgroundColor = Color.White;
            dgvServices.BorderStyle = BorderStyle.None;
            dgvServices.ColumnHeadersHeight = 34;
            dgvServices.EnableHeadersVisualStyles = false;
            dgvServices.Location = new Point(16, 104);
            dgvServices.Name = "dgvServices";
            dgvServices.ReadOnly = true;
            dgvServices.RowHeadersVisible = false;
            dgvServices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvServices.Size = new Size(968, 438);
            dgvServices.TabIndex = 1;
            // 
            // frmTechnician
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(241, 245, 249);
            ClientSize = new Size(1000, 620);
            Controls.Add(dgvServices);
            Controls.Add(panelActions);
            Controls.Add(panelHeader);
            Name = "frmTechnician";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kỹ thuật viên - HSBA_DV";
            ((System.ComponentModel.ISupportInitialize)dgvServices).EndInit();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelActions.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panelHeader;
        private Label label1;
        private Label label2;
        private Panel panelActions;
        private Button btnRefresh;
        private Button btnNewService;
        private DataGridView dgvServices;
    }
}