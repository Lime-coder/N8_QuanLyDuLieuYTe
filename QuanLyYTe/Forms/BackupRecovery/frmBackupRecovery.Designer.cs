namespace QuanLyYTe.Forms.BackupRecovery
{
    partial class frmBackupRecovery
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tblMainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.grpCurrentData = new System.Windows.Forms.GroupBox();
            this.dgvDonThuoc = new System.Windows.Forms.DataGridView();
            this.pnlCurrentDataActions = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.grpBackupManagement = new System.Windows.Forms.GroupBox();
            this.dgvBackupHistory = new System.Windows.Forms.DataGridView();
            this.pnlBackupControls = new System.Windows.Forms.Panel();
            this.btnOpenErrorLog = new System.Windows.Forms.Button();
            this.lblSchedulerStatus = new System.Windows.Forms.Label();
            this.btnDisableAutoBackup = new System.Windows.Forms.Button();
            this.btnEnableAutoBackup = new System.Windows.Forms.Button();
            this.cboInterval = new System.Windows.Forms.ComboBox();
            this.lblInterval = new System.Windows.Forms.Label();
            this.btnBackupNow = new System.Windows.Forms.Button();
            this.grpIncidentSimulation = new System.Windows.Forms.GroupBox();
            this.pnlIncidentButtons = new System.Windows.Forms.Panel();
            this.btnSimulateWrongUpdate = new System.Windows.Forms.Button();
            this.btnSimulateDelete = new System.Windows.Forms.Button();
            this.grpAuditLog = new System.Windows.Forms.GroupBox();
            this.dgvAudit = new System.Windows.Forms.DataGridView();
            this.pnlAuditActions = new System.Windows.Forms.Panel();
            this.btnLoadAudit = new System.Windows.Forms.Button();
            this.grpRestore = new System.Windows.Forms.GroupBox();
            this.pnlRestoreControls = new System.Windows.Forms.Panel();
            this.btnImportDataPump = new System.Windows.Forms.Button();
            this.txtImportDumpFile = new System.Windows.Forms.TextBox();
            this.lblImportDumpFile = new System.Windows.Forms.Label();
            this.btnBackupRestore = new System.Windows.Forms.Button();
            this.cboBackupVersion = new System.Windows.Forms.ComboBox();
            this.lblBackupVersion = new System.Windows.Forms.Label();
            this.lblTotalRows = new System.Windows.Forms.Label();
            this.btnRefreshBackup = new System.Windows.Forms.Button();
            this.tblMainLayout.SuspendLayout();
            this.grpCurrentData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDonThuoc)).BeginInit();
            this.pnlCurrentDataActions.SuspendLayout();
            this.grpBackupManagement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBackupHistory)).BeginInit();
            this.pnlBackupControls.SuspendLayout();
            this.grpIncidentSimulation.SuspendLayout();
            this.pnlIncidentButtons.SuspendLayout();
            this.grpAuditLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAudit)).BeginInit();
            this.pnlAuditActions.SuspendLayout();
            this.grpRestore.SuspendLayout();
            this.pnlRestoreControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblMainLayout
            // 
            this.tblMainLayout.ColumnCount = 2;
            this.tblMainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblMainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblMainLayout.Controls.Add(this.grpCurrentData, 0, 0);
            this.tblMainLayout.Controls.Add(this.grpBackupManagement, 1, 0);
            this.tblMainLayout.Controls.Add(this.grpIncidentSimulation, 0, 1);
            this.tblMainLayout.Controls.Add(this.grpAuditLog, 1, 1);
            this.tblMainLayout.Controls.Add(this.grpRestore, 0, 2);
            this.tblMainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMainLayout.Location = new System.Drawing.Point(0, 0);
            this.tblMainLayout.Name = "tblMainLayout";
            this.tblMainLayout.Padding = new System.Windows.Forms.Padding(12);
            this.tblMainLayout.RowCount = 3;
            this.tblMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 42F));
            this.tblMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.tblMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblMainLayout.Size = new System.Drawing.Size(1100, 650);
            this.tblMainLayout.TabIndex = 0;
            // 
            // grpCurrentData
            // 
            this.grpCurrentData.Controls.Add(this.dgvDonThuoc);
            this.grpCurrentData.Controls.Add(this.pnlCurrentDataActions);
            this.grpCurrentData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCurrentData.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpCurrentData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.grpCurrentData.Location = new System.Drawing.Point(15, 15);
            this.grpCurrentData.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.grpCurrentData.Name = "grpCurrentData";
            this.grpCurrentData.Padding = new System.Windows.Forms.Padding(8);
            this.grpCurrentData.Size = new System.Drawing.Size(523, 256);
            this.grpCurrentData.TabIndex = 0;
            this.grpCurrentData.TabStop = false;
            this.grpCurrentData.Text = "Dữ liệu bảng PRESCRIPTION";
            // 
            // dgvDonThuoc
            // 
            this.dgvDonThuoc.AllowUserToAddRows = false;
            this.dgvDonThuoc.AllowUserToDeleteRows = false;
            this.dgvDonThuoc.BackgroundColor = System.Drawing.Color.White;
            this.dgvDonThuoc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDonThuoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDonThuoc.Location = new System.Drawing.Point(8, 70);
            this.dgvDonThuoc.MultiSelect = false;
            this.dgvDonThuoc.Name = "dgvDonThuoc";
            this.dgvDonThuoc.ReadOnly = true;
            this.dgvDonThuoc.RowHeadersWidth = 51;
            this.dgvDonThuoc.Size = new System.Drawing.Size(507, 178);
            this.dgvDonThuoc.TabIndex = 1;
            // 
            // pnlCurrentDataActions
            // 
            this.pnlCurrentDataActions.Controls.Add(this.lblTotalRows);
            this.pnlCurrentDataActions.Controls.Add(this.btnRefresh);
            this.pnlCurrentDataActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCurrentDataActions.Location = new System.Drawing.Point(8, 30);
            this.pnlCurrentDataActions.Name = "pnlCurrentDataActions";
            this.pnlCurrentDataActions.Size = new System.Drawing.Size(507, 40);
            this.pnlCurrentDataActions.TabIndex = 0;
            // 
            // lblTotalRows
            // 
            this.lblTotalRows.AutoSize = true;
            this.lblTotalRows.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTotalRows.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblTotalRows.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(130)))));
            this.lblTotalRows.Location = new System.Drawing.Point(350, 10);
            this.lblTotalRows.Name = "lblTotalRows";
            this.lblTotalRows.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lblTotalRows.Size = new System.Drawing.Size(120, 30);
            this.lblTotalRows.TabIndex = 1;
            this.lblTotalRows.Text = "Tổng số dòng: 0";
            this.lblTotalRows.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRefresh.Location = new System.Drawing.Point(3, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(120, 32);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // grpBackupManagement
            // 
            this.grpBackupManagement.Controls.Add(this.dgvBackupHistory);
            this.grpBackupManagement.Controls.Add(this.pnlBackupControls);
            this.grpBackupManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBackupManagement.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpBackupManagement.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.grpBackupManagement.Location = new System.Drawing.Point(558, 15);
            this.grpBackupManagement.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.grpBackupManagement.Name = "grpBackupManagement";
            this.grpBackupManagement.Padding = new System.Windows.Forms.Padding(8);
            this.grpBackupManagement.Size = new System.Drawing.Size(530, 256);
            this.grpBackupManagement.TabIndex = 1;
            this.grpBackupManagement.TabStop = false;
            this.grpBackupManagement.Text = "Quản lý sao lưu";
            // 
            // dgvBackupHistory
            // 
            this.dgvBackupHistory.AllowUserToAddRows = false;
            this.dgvBackupHistory.AllowUserToDeleteRows = false;
            this.dgvBackupHistory.BackgroundColor = System.Drawing.Color.White;
            this.dgvBackupHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBackupHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBackupHistory.Location = new System.Drawing.Point(8, 110);
            this.dgvBackupHistory.MultiSelect = false;
            this.dgvBackupHistory.Name = "dgvBackupHistory";
            this.dgvBackupHistory.ReadOnly = true;
            this.dgvBackupHistory.RowHeadersWidth = 51;
            this.dgvBackupHistory.Size = new System.Drawing.Size(514, 138);
            this.dgvBackupHistory.TabIndex = 1;
            this.dgvBackupHistory.SelectionChanged += new System.EventHandler(this.dgvBackupHistory_SelectionChanged);
            // 
            // pnlBackupControls
            // 
            this.pnlBackupControls.Controls.Add(this.btnOpenErrorLog);
            this.pnlBackupControls.Controls.Add(this.btnRefreshBackup);
            this.pnlBackupControls.Controls.Add(this.lblSchedulerStatus);
            this.pnlBackupControls.Controls.Add(this.btnDisableAutoBackup);
            this.pnlBackupControls.Controls.Add(this.btnEnableAutoBackup);
            this.pnlBackupControls.Controls.Add(this.cboInterval);
            this.pnlBackupControls.Controls.Add(this.lblInterval);
            this.pnlBackupControls.Controls.Add(this.btnBackupNow);
            this.pnlBackupControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBackupControls.Location = new System.Drawing.Point(8, 30);
            this.pnlBackupControls.Name = "pnlBackupControls";
            this.pnlBackupControls.Size = new System.Drawing.Size(514, 115);
            this.pnlBackupControls.TabIndex = 0;
            // 
            // btnOpenErrorLog
            //
            this.btnOpenErrorLog.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnOpenErrorLog.Location = new System.Drawing.Point(270, 80);
            this.btnOpenErrorLog.Name = "btnOpenErrorLog";
            this.btnOpenErrorLog.Size = new System.Drawing.Size(110, 30);
            this.btnOpenErrorLog.TabIndex = 7;
            this.btnOpenErrorLog.Text = "Mo log loi";
            this.btnOpenErrorLog.UseVisualStyleBackColor = true;
            this.btnOpenErrorLog.Click += new System.EventHandler(this.btnOpenErrorLog_Click);
            //
            // btnRefreshBackup
            // 
            this.btnRefreshBackup.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRefreshBackup.Location = new System.Drawing.Point(390, 80);
            this.btnRefreshBackup.Name = "btnRefreshBackup";
            this.btnRefreshBackup.Size = new System.Drawing.Size(110, 30);
            this.btnRefreshBackup.TabIndex = 6;
            this.btnRefreshBackup.Text = "Làm mới";
            this.btnRefreshBackup.UseVisualStyleBackColor = true;
            this.btnRefreshBackup.Click += new System.EventHandler(this.btnRefreshBackup_Click);
            // 
            // lblSchedulerStatus
            // 
            this.lblSchedulerStatus.AutoSize = true;
            this.lblSchedulerStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSchedulerStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(130)))));
            this.lblSchedulerStatus.Location = new System.Drawing.Point(3, 85);
            this.lblSchedulerStatus.Name = "lblSchedulerStatus";
            this.lblSchedulerStatus.Size = new System.Drawing.Size(184, 20);
            this.lblSchedulerStatus.TabIndex = 5;
            this.lblSchedulerStatus.Text = "Trạng thái scheduler: Tắt";
            // 
            // btnDisableAutoBackup
            // 
            this.btnDisableAutoBackup.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDisableAutoBackup.Location = new System.Drawing.Point(340, 43);
            this.btnDisableAutoBackup.Name = "btnDisableAutoBackup";
            this.btnDisableAutoBackup.Size = new System.Drawing.Size(160, 32);
            this.btnDisableAutoBackup.TabIndex = 4;
            this.btnDisableAutoBackup.Text = "Tắt sao lưu tự động";
            this.btnDisableAutoBackup.UseVisualStyleBackColor = true;
            this.btnDisableAutoBackup.Click += new System.EventHandler(this.btnDisableAutoBackup_Click);
            // 
            // btnEnableAutoBackup
            // 
            this.btnEnableAutoBackup.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnEnableAutoBackup.Location = new System.Drawing.Point(340, 5);
            this.btnEnableAutoBackup.Name = "btnEnableAutoBackup";
            this.btnEnableAutoBackup.Size = new System.Drawing.Size(160, 32);
            this.btnEnableAutoBackup.TabIndex = 3;
            this.btnEnableAutoBackup.Text = "Bật sao lưu tự động";
            this.btnEnableAutoBackup.UseVisualStyleBackColor = true;
            this.btnEnableAutoBackup.Click += new System.EventHandler(this.btnEnableAutoBackup_Click);
            // 
            // cboInterval
            // 
            this.cboInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInterval.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboInterval.FormattingEnabled = true;
            this.cboInterval.Items.AddRange(new object[] {
            "1 phút",
            "1 ngày"});
            this.cboInterval.Location = new System.Drawing.Point(212, 8);
            this.cboInterval.Name = "cboInterval";
            this.cboInterval.Size = new System.Drawing.Size(110, 28);
            this.cboInterval.TabIndex = 2;
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblInterval.Location = new System.Drawing.Point(145, 12);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(56, 20);
            this.lblInterval.TabIndex = 1;
            this.lblInterval.Text = "Chu kỳ:";
            // 
            // btnBackupNow
            // 
            this.btnBackupNow.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBackupNow.Location = new System.Drawing.Point(3, 5);
            this.btnBackupNow.Name = "btnBackupNow";
            this.btnBackupNow.Size = new System.Drawing.Size(125, 70);
            this.btnBackupNow.TabIndex = 0;
            this.btnBackupNow.Text = "Sao lưu chủ động";
            this.btnBackupNow.UseVisualStyleBackColor = true;
            this.btnBackupNow.Click += new System.EventHandler(this.btnBackupNow_Click);
            // 
            // grpIncidentSimulation
            // 
            this.grpIncidentSimulation.Controls.Add(this.pnlIncidentButtons);
            this.grpIncidentSimulation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpIncidentSimulation.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpIncidentSimulation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.grpIncidentSimulation.Location = new System.Drawing.Point(15, 277);
            this.grpIncidentSimulation.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.grpIncidentSimulation.Name = "grpIncidentSimulation";
            this.grpIncidentSimulation.Padding = new System.Windows.Forms.Padding(8);
            this.grpIncidentSimulation.Size = new System.Drawing.Size(523, 231);
            this.grpIncidentSimulation.TabIndex = 2;
            this.grpIncidentSimulation.TabStop = false;
            this.grpIncidentSimulation.Text = "Giả lập sự cố";
            // 
            // pnlIncidentButtons
            // 
            this.pnlIncidentButtons.Controls.Add(this.btnSimulateWrongUpdate);
            this.pnlIncidentButtons.Controls.Add(this.btnSimulateDelete);
            this.pnlIncidentButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlIncidentButtons.Location = new System.Drawing.Point(8, 30);
            this.pnlIncidentButtons.Name = "pnlIncidentButtons";
            this.pnlIncidentButtons.Size = new System.Drawing.Size(507, 193);
            this.pnlIncidentButtons.TabIndex = 0;
            // 
            // btnSimulateWrongUpdate
            // 
            this.btnSimulateWrongUpdate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSimulateWrongUpdate.Location = new System.Drawing.Point(3, 62);
            this.btnSimulateWrongUpdate.Name = "btnSimulateWrongUpdate";
            this.btnSimulateWrongUpdate.Size = new System.Drawing.Size(350, 42);
            this.btnSimulateWrongUpdate.TabIndex = 1;
            this.btnSimulateWrongUpdate.Text = "Giả lập cập nhật sai dữ liệu BA000001";
            this.btnSimulateWrongUpdate.UseVisualStyleBackColor = true;
            this.btnSimulateWrongUpdate.Click += new System.EventHandler(this.btnSimulateWrongUpdate_Click);
            // 
            // btnSimulateDelete
            // 
            this.btnSimulateDelete.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSimulateDelete.Location = new System.Drawing.Point(3, 10);
            this.btnSimulateDelete.Name = "btnSimulateDelete";
            this.btnSimulateDelete.Size = new System.Drawing.Size(350, 42);
            this.btnSimulateDelete.TabIndex = 0;
            this.btnSimulateDelete.Text = "Giả lập xóa dữ liệu BA000001";
            this.btnSimulateDelete.UseVisualStyleBackColor = true;
            this.btnSimulateDelete.Click += new System.EventHandler(this.btnSimulateDelete_Click);
            // 
            // grpAuditLog
            // 
            this.grpAuditLog.Controls.Add(this.dgvAudit);
            this.grpAuditLog.Controls.Add(this.pnlAuditActions);
            this.grpAuditLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpAuditLog.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpAuditLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.grpAuditLog.Location = new System.Drawing.Point(558, 277);
            this.grpAuditLog.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.tblMainLayout.SetRowSpan(this.grpAuditLog, 2);
            this.grpAuditLog.Name = "grpAuditLog";
            this.grpAuditLog.Padding = new System.Windows.Forms.Padding(8);
            this.grpAuditLog.Size = new System.Drawing.Size(530, 358);
            this.grpAuditLog.TabIndex = 3;
            this.grpAuditLog.TabStop = false;
            this.grpAuditLog.Text = "Nhật ký kiểm toán";
            // 
            // dgvAudit
            // 
            this.dgvAudit.AllowUserToAddRows = false;
            this.dgvAudit.AllowUserToDeleteRows = false;
            this.dgvAudit.BackgroundColor = System.Drawing.Color.White;
            this.dgvAudit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAudit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAudit.Location = new System.Drawing.Point(8, 70);
            this.dgvAudit.MultiSelect = false;
            this.dgvAudit.Name = "dgvAudit";
            this.dgvAudit.ReadOnly = true;
            this.dgvAudit.RowHeadersWidth = 51;
            this.dgvAudit.Size = new System.Drawing.Size(514, 280);
            this.dgvAudit.TabIndex = 1;
            // 
            // pnlAuditActions
            // 
            this.pnlAuditActions.Controls.Add(this.btnLoadAudit);
            this.pnlAuditActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAuditActions.Location = new System.Drawing.Point(8, 30);
            this.pnlAuditActions.Name = "pnlAuditActions";
            this.pnlAuditActions.Size = new System.Drawing.Size(514, 40);
            this.pnlAuditActions.TabIndex = 0;
            // 
            // btnLoadAudit
            // 
            this.btnLoadAudit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnLoadAudit.Location = new System.Drawing.Point(3, 3);
            this.btnLoadAudit.Name = "btnLoadAudit";
            this.btnLoadAudit.Size = new System.Drawing.Size(120, 32);
            this.btnLoadAudit.TabIndex = 0;
            this.btnLoadAudit.Text = "Tải Nhật ký";
            this.btnLoadAudit.UseVisualStyleBackColor = true;
            this.btnLoadAudit.Click += new System.EventHandler(this.btnLoadAudit_Click);
            // 
            // grpRestore
            // 
            this.grpRestore.Controls.Add(this.pnlRestoreControls);
            this.grpRestore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpRestore.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpRestore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.grpRestore.Location = new System.Drawing.Point(15, 514);
            this.grpRestore.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.grpRestore.Name = "grpRestore";
            this.grpRestore.Padding = new System.Windows.Forms.Padding(8);
            this.grpRestore.Size = new System.Drawing.Size(523, 121);
            this.grpRestore.TabIndex = 4;
            this.grpRestore.TabStop = false;
            this.grpRestore.Text = "Khôi phục dữ liệu";
            // 
            // pnlRestoreControls
            // 
            this.pnlRestoreControls.Controls.Add(this.btnImportDataPump);
            this.pnlRestoreControls.Controls.Add(this.txtImportDumpFile);
            this.pnlRestoreControls.Controls.Add(this.lblImportDumpFile);
            this.pnlRestoreControls.Controls.Add(this.btnBackupRestore);
            this.pnlRestoreControls.Controls.Add(this.cboBackupVersion);
            this.pnlRestoreControls.Controls.Add(this.lblBackupVersion);
            this.pnlRestoreControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRestoreControls.Location = new System.Drawing.Point(8, 30);
            this.pnlRestoreControls.Name = "pnlRestoreControls";
            this.pnlRestoreControls.Size = new System.Drawing.Size(507, 83);
            this.pnlRestoreControls.TabIndex = 0;
            //
            // btnImportDataPump
            //
            this.btnImportDataPump.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnImportDataPump.Location = new System.Drawing.Point(395, 47);
            this.btnImportDataPump.Name = "btnImportDataPump";
            this.btnImportDataPump.Size = new System.Drawing.Size(110, 32);
            this.btnImportDataPump.TabIndex = 5;
            this.btnImportDataPump.Text = "Import dump";
            this.btnImportDataPump.UseVisualStyleBackColor = true;
            this.btnImportDataPump.Click += new System.EventHandler(this.btnImportDataPump_Click);
            //
            // txtImportDumpFile
            //
            this.txtImportDumpFile.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtImportDumpFile.Location = new System.Drawing.Point(85, 49);
            this.txtImportDumpFile.Name = "txtImportDumpFile";
            this.txtImportDumpFile.Size = new System.Drawing.Size(300, 27);
            this.txtImportDumpFile.TabIndex = 4;
            //
            // lblImportDumpFile
            //
            this.lblImportDumpFile.AutoSize = true;
            this.lblImportDumpFile.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblImportDumpFile.Location = new System.Drawing.Point(3, 53);
            this.lblImportDumpFile.Name = "lblImportDumpFile";
            this.lblImportDumpFile.Size = new System.Drawing.Size(72, 20);
            this.lblImportDumpFile.TabIndex = 3;
            this.lblImportDumpFile.Text = "File dump:";
            // 
            // btnBackupRestore
            // 
            this.btnBackupRestore.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBackupRestore.Location = new System.Drawing.Point(395, 9);
            this.btnBackupRestore.Name = "btnBackupRestore";
            this.btnBackupRestore.Size = new System.Drawing.Size(110, 32);
            this.btnBackupRestore.TabIndex = 2;
            this.btnBackupRestore.Text = "Khôi phục";
            this.btnBackupRestore.UseVisualStyleBackColor = true;
            this.btnBackupRestore.Click += new System.EventHandler(this.btnBackupRestore_Click);
            // 
            // cboBackupVersion
            // 
            this.cboBackupVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBackupVersion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboBackupVersion.FormattingEnabled = true;
            this.cboBackupVersion.Location = new System.Drawing.Point(135, 11);
            this.cboBackupVersion.Name = "cboBackupVersion";
            this.cboBackupVersion.Size = new System.Drawing.Size(250, 28);
            this.cboBackupVersion.TabIndex = 1;
            // 
            // lblBackupVersion
            // 
            this.lblBackupVersion.AutoSize = true;
            this.lblBackupVersion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBackupVersion.Location = new System.Drawing.Point(3, 14);
            this.lblBackupVersion.Name = "lblBackupVersion";
            this.lblBackupVersion.Size = new System.Drawing.Size(126, 20);
            this.lblBackupVersion.TabIndex = 0;
            this.lblBackupVersion.Text = "Chọn bản sao lưu:";
            // 
            // frmBackupRecovery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 650);
            this.Controls.Add(this.tblMainLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmBackupRecovery";
            this.Text = "Sao lưu và phục hồi dữ liệu";
            this.Load += new System.EventHandler(this.frmBackupRecovery_Load);
            this.tblMainLayout.ResumeLayout(false);
            this.grpCurrentData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDonThuoc)).EndInit();
            this.pnlCurrentDataActions.ResumeLayout(false);
            this.grpBackupManagement.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBackupHistory)).EndInit();
            this.pnlBackupControls.ResumeLayout(false);
            this.pnlBackupControls.PerformLayout();
            this.grpIncidentSimulation.ResumeLayout(false);
            this.pnlIncidentButtons.ResumeLayout(false);
            this.grpAuditLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAudit)).EndInit();
            this.pnlAuditActions.ResumeLayout(false);
            this.grpRestore.ResumeLayout(false);
            this.pnlRestoreControls.ResumeLayout(false);
            this.pnlRestoreControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblMainLayout;
        private System.Windows.Forms.GroupBox grpCurrentData;
        private System.Windows.Forms.Panel pnlCurrentDataActions;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvDonThuoc;
        private System.Windows.Forms.GroupBox grpBackupManagement;
        private System.Windows.Forms.Panel pnlBackupControls;
        private System.Windows.Forms.Button btnBackupNow;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.ComboBox cboInterval;
        private System.Windows.Forms.Button btnEnableAutoBackup;
        private System.Windows.Forms.Button btnDisableAutoBackup;
        private System.Windows.Forms.Label lblSchedulerStatus;
        private System.Windows.Forms.DataGridView dgvBackupHistory;
        private System.Windows.Forms.GroupBox grpIncidentSimulation;
        private System.Windows.Forms.Panel pnlIncidentButtons;
        private System.Windows.Forms.Button btnSimulateDelete;
        private System.Windows.Forms.Button btnSimulateWrongUpdate;
        private System.Windows.Forms.GroupBox grpAuditLog;
        private System.Windows.Forms.Panel pnlAuditActions;
        private System.Windows.Forms.Button btnLoadAudit;
        private System.Windows.Forms.DataGridView dgvAudit;
        private System.Windows.Forms.GroupBox grpRestore;
        private System.Windows.Forms.Panel pnlRestoreControls;
        private System.Windows.Forms.Label lblBackupVersion;
        private System.Windows.Forms.ComboBox cboBackupVersion;
        private System.Windows.Forms.Button btnBackupRestore;
        private System.Windows.Forms.Label lblImportDumpFile;
        private System.Windows.Forms.TextBox txtImportDumpFile;
        private System.Windows.Forms.Button btnImportDataPump;
        private System.Windows.Forms.Label lblTotalRows;
        private System.Windows.Forms.Button btnRefreshBackup;
        private System.Windows.Forms.Button btnOpenErrorLog;
    }
}
