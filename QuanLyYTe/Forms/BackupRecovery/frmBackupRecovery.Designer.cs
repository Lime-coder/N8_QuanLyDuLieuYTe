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
            this.mainTabs = new System.Windows.Forms.TabControl();
            this.tabDataPump = new System.Windows.Forms.TabPage();
            this.tblDataPump = new System.Windows.Forms.TableLayoutPanel();
            this.grpBackupManagement = new System.Windows.Forms.GroupBox();
            this.dgvBackupHistory = new System.Windows.Forms.DataGridView();
            this.pnlBackupControls = new System.Windows.Forms.FlowLayoutPanel();
            this.btnBackupNow = new System.Windows.Forms.Button();
            this.lblInterval = new System.Windows.Forms.Label();
            this.cboInterval = new System.Windows.Forms.ComboBox();
            this.btnEnableAutoBackup = new System.Windows.Forms.Button();
            this.btnDisableAutoBackup = new System.Windows.Forms.Button();
            this.btnRefreshBackup = new System.Windows.Forms.Button();
            this.btnOpenErrorLog = new System.Windows.Forms.Button();
            this.lblSchedulerStatus = new System.Windows.Forms.Label();
            this.grpImportDump = new System.Windows.Forms.GroupBox();
            this.dgvRestoreHistory = new System.Windows.Forms.DataGridView();
            this.pnlImportControls = new System.Windows.Forms.Panel();
            this.lblImportDumpFile = new System.Windows.Forms.Label();
            this.txtImportDumpFile = new System.Windows.Forms.TextBox();
            this.btnImportDataPump = new System.Windows.Forms.Button();
            this.tabFlashback = new System.Windows.Forms.TabPage();
            this.tblFlashback = new System.Windows.Forms.TableLayoutPanel();
            this.grpFlashbackAudit = new System.Windows.Forms.GroupBox();
            this.dgvFgaAudit = new System.Windows.Forms.DataGridView();
            this.pnlAuditActions = new System.Windows.Forms.Panel();
            this.tabFlashbackTables = new System.Windows.Forms.TabControl();
            this.tabPatientAudit = new System.Windows.Forms.TabPage();
            this.tabMedicalAudit = new System.Windows.Forms.TabPage();
            this.btnLoadAudit = new System.Windows.Forms.Button();
            this.btnPreviewFlashback = new System.Windows.Forms.Button();
            this.btnBackupRestore = new System.Windows.Forms.Button();
            this.lblSelectedAudit = new System.Windows.Forms.Label();
            this.grpFlashbackPreview = new System.Windows.Forms.GroupBox();
            this.tblPreview = new System.Windows.Forms.TableLayoutPanel();
            this.dgvCurrentRow = new System.Windows.Forms.DataGridView();
            this.dgvFlashbackRow = new System.Windows.Forms.DataGridView();
            this.lblCurrentRow = new System.Windows.Forms.Label();
            this.lblFlashbackRow = new System.Windows.Forms.Label();
            this.grpDataView = new System.Windows.Forms.GroupBox();
            this.tabControlData = new System.Windows.Forms.TabControl();
            this.tabMedicalRecord = new System.Windows.Forms.TabPage();
            this.dgvMedicalRecord = new System.Windows.Forms.DataGridView();
            this.grpCurrentData = new System.Windows.Forms.GroupBox();
            this.dgvStandardAudit = new System.Windows.Forms.DataGridView();
            this.pnlCurrentDataActions = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblTotalRows = new System.Windows.Forms.Label();
            this.mainTabs.SuspendLayout();
            this.tabDataPump.SuspendLayout();
            this.tblDataPump.SuspendLayout();
            this.grpBackupManagement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBackupHistory)).BeginInit();
            this.pnlBackupControls.SuspendLayout();
            this.grpImportDump.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRestoreHistory)).BeginInit();
            this.pnlImportControls.SuspendLayout();
            this.tabFlashback.SuspendLayout();
            this.tblFlashback.SuspendLayout();
            this.grpFlashbackAudit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFgaAudit)).BeginInit();
            this.pnlAuditActions.SuspendLayout();
            this.tabFlashbackTables.SuspendLayout();
            this.grpFlashbackPreview.SuspendLayout();
            this.tblPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurrentRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFlashbackRow)).BeginInit();
            this.grpDataView.SuspendLayout();
            this.tabControlData.SuspendLayout();
            this.tabMedicalRecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMedicalRecord)).BeginInit();
            this.grpCurrentData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStandardAudit)).BeginInit();
            this.pnlCurrentDataActions.SuspendLayout();
            this.SuspendLayout();
            //
            // mainTabs
            //
            this.mainTabs.Controls.Add(this.tabDataPump);
            this.mainTabs.Controls.Add(this.tabFlashback);
            this.mainTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabs.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.mainTabs.Location = new System.Drawing.Point(0, 0);
            this.mainTabs.Name = "mainTabs";
            this.mainTabs.SelectedIndex = 0;
            this.mainTabs.Size = new System.Drawing.Size(1180, 720);
            this.mainTabs.TabIndex = 0;
            //
            // tabDataPump
            //
            this.tabDataPump.Controls.Add(this.tblDataPump);
            this.tabDataPump.Location = new System.Drawing.Point(4, 29);
            this.tabDataPump.Name = "tabDataPump";
            this.tabDataPump.Padding = new System.Windows.Forms.Padding(10);
            this.tabDataPump.Size = new System.Drawing.Size(1172, 687);
            this.tabDataPump.TabIndex = 0;
            this.tabDataPump.Text = "Data Pump";
            this.tabDataPump.UseVisualStyleBackColor = true;
            //
            // tblDataPump
            //
            this.tblDataPump.ColumnCount = 1;
            this.tblDataPump.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblDataPump.Controls.Add(this.grpBackupManagement, 0, 0);
            this.tblDataPump.Controls.Add(this.grpImportDump, 0, 1);
            this.tblDataPump.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblDataPump.Location = new System.Drawing.Point(10, 10);
            this.tblDataPump.Name = "tblDataPump";
            this.tblDataPump.RowCount = 2;
            this.tblDataPump.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.tblDataPump.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.tblDataPump.Size = new System.Drawing.Size(1152, 667);
            this.tblDataPump.TabIndex = 0;
            //
            // grpBackupManagement
            //
            this.grpBackupManagement.Controls.Add(this.dgvBackupHistory);
            this.grpBackupManagement.Controls.Add(this.pnlBackupControls);
            this.grpBackupManagement.Controls.Add(this.lblSchedulerStatus);
            this.grpBackupManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBackupManagement.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpBackupManagement.Location = new System.Drawing.Point(3, 3);
            this.grpBackupManagement.Name = "grpBackupManagement";
            this.grpBackupManagement.Padding = new System.Windows.Forms.Padding(8);
            this.grpBackupManagement.Size = new System.Drawing.Size(1146, 407);
            this.grpBackupManagement.TabIndex = 0;
            this.grpBackupManagement.TabStop = false;
            this.grpBackupManagement.Text = "Sao lưu Data Pump";
            //
            // dgvBackupHistory
            //
            this.dgvBackupHistory.AllowUserToAddRows = false;
            this.dgvBackupHistory.AllowUserToDeleteRows = false;
            this.dgvBackupHistory.BackgroundColor = System.Drawing.Color.White;
            this.dgvBackupHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBackupHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBackupHistory.Location = new System.Drawing.Point(8, 82);
            this.dgvBackupHistory.MultiSelect = false;
            this.dgvBackupHistory.Name = "dgvBackupHistory";
            this.dgvBackupHistory.ReadOnly = true;
            this.dgvBackupHistory.RowHeadersWidth = 51;
            this.dgvBackupHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBackupHistory.Size = new System.Drawing.Size(1130, 287);
            this.dgvBackupHistory.TabIndex = 1;
            this.dgvBackupHistory.SelectionChanged += new System.EventHandler(this.dgvBackupHistory_SelectionChanged);
            //
            // pnlBackupControls
            //
            this.pnlBackupControls.Controls.Add(this.btnBackupNow);
            this.pnlBackupControls.Controls.Add(this.lblInterval);
            this.pnlBackupControls.Controls.Add(this.cboInterval);
            this.pnlBackupControls.Controls.Add(this.btnEnableAutoBackup);
            this.pnlBackupControls.Controls.Add(this.btnDisableAutoBackup);
            this.pnlBackupControls.Controls.Add(this.btnRefreshBackup);
            this.pnlBackupControls.Controls.Add(this.btnOpenErrorLog);
            this.pnlBackupControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBackupControls.Location = new System.Drawing.Point(8, 30);
            this.pnlBackupControls.Name = "pnlBackupControls";
            this.pnlBackupControls.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.pnlBackupControls.Size = new System.Drawing.Size(1130, 52);
            this.pnlBackupControls.TabIndex = 0;
            //
            // btnBackupNow
            //
            this.btnBackupNow.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBackupNow.Location = new System.Drawing.Point(3, 7);
            this.btnBackupNow.Name = "btnBackupNow";
            this.btnBackupNow.Size = new System.Drawing.Size(130, 34);
            this.btnBackupNow.TabIndex = 0;
            this.btnBackupNow.Text = "Sao lưu ngay";
            this.btnBackupNow.UseVisualStyleBackColor = true;
            this.btnBackupNow.Click += new System.EventHandler(this.btnBackupNow_Click);
            //
            // lblInterval
            //
            this.lblInterval.AutoSize = true;
            this.lblInterval.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblInterval.Location = new System.Drawing.Point(149, 14);
            this.lblInterval.Margin = new System.Windows.Forms.Padding(13, 10, 3, 0);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(56, 20);
            this.lblInterval.TabIndex = 1;
            this.lblInterval.Text = "Chu kỳ:";
            //
            // cboInterval
            //
            this.cboInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInterval.FormattingEnabled = true;
            this.cboInterval.Items.AddRange(new object[] {
            "1 phút",
            "1 ngày"});
            this.cboInterval.Location = new System.Drawing.Point(211, 10);
            this.cboInterval.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.cboInterval.Name = "cboInterval";
            this.cboInterval.Size = new System.Drawing.Size(110, 28);
            this.cboInterval.TabIndex = 2;
            //
            // btnEnableAutoBackup
            //
            this.btnEnableAutoBackup.Location = new System.Drawing.Point(331, 7);
            this.btnEnableAutoBackup.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
            this.btnEnableAutoBackup.Name = "btnEnableAutoBackup";
            this.btnEnableAutoBackup.Size = new System.Drawing.Size(150, 34);
            this.btnEnableAutoBackup.TabIndex = 3;
            this.btnEnableAutoBackup.Text = "Bật tự động";
            this.btnEnableAutoBackup.UseVisualStyleBackColor = true;
            this.btnEnableAutoBackup.Click += new System.EventHandler(this.btnEnableAutoBackup_Click);
            //
            // btnDisableAutoBackup
            //
            this.btnDisableAutoBackup.Location = new System.Drawing.Point(487, 7);
            this.btnDisableAutoBackup.Name = "btnDisableAutoBackup";
            this.btnDisableAutoBackup.Size = new System.Drawing.Size(150, 34);
            this.btnDisableAutoBackup.TabIndex = 4;
            this.btnDisableAutoBackup.Text = "Tắt tự động";
            this.btnDisableAutoBackup.UseVisualStyleBackColor = true;
            this.btnDisableAutoBackup.Click += new System.EventHandler(this.btnDisableAutoBackup_Click);
            //
            // btnRefreshBackup
            //
            this.btnRefreshBackup.Location = new System.Drawing.Point(647, 7);
            this.btnRefreshBackup.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
            this.btnRefreshBackup.Name = "btnRefreshBackup";
            this.btnRefreshBackup.Size = new System.Drawing.Size(110, 34);
            this.btnRefreshBackup.TabIndex = 5;
            this.btnRefreshBackup.Text = "Làm mới";
            this.btnRefreshBackup.UseVisualStyleBackColor = true;
            this.btnRefreshBackup.Click += new System.EventHandler(this.btnRefreshBackup_Click);
            //
            // btnOpenErrorLog
            //
            this.btnOpenErrorLog.Location = new System.Drawing.Point(763, 7);
            this.btnOpenErrorLog.Name = "btnOpenErrorLog";
            this.btnOpenErrorLog.Size = new System.Drawing.Size(110, 34);
            this.btnOpenErrorLog.TabIndex = 6;
            this.btnOpenErrorLog.Text = "Mở log";
            this.btnOpenErrorLog.UseVisualStyleBackColor = true;
            this.btnOpenErrorLog.Click += new System.EventHandler(this.btnOpenErrorLog_Click);
            //
            // lblSchedulerStatus
            //
            this.lblSchedulerStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblSchedulerStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSchedulerStatus.ForeColor = System.Drawing.Color.FromArgb(96, 96, 104);
            this.lblSchedulerStatus.Location = new System.Drawing.Point(8, 369);
            this.lblSchedulerStatus.Name = "lblSchedulerStatus";
            this.lblSchedulerStatus.Size = new System.Drawing.Size(1130, 30);
            this.lblSchedulerStatus.TabIndex = 2;
            this.lblSchedulerStatus.Text = "Trạng thái scheduler: Tắt";
            this.lblSchedulerStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // grpImportDump
            //
            this.grpImportDump.Controls.Add(this.dgvRestoreHistory);
            this.grpImportDump.Controls.Add(this.pnlImportControls);
            this.grpImportDump.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpImportDump.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpImportDump.Location = new System.Drawing.Point(3, 416);
            this.grpImportDump.Name = "grpImportDump";
            this.grpImportDump.Padding = new System.Windows.Forms.Padding(8);
            this.grpImportDump.Size = new System.Drawing.Size(1146, 248);
            this.grpImportDump.TabIndex = 1;
            this.grpImportDump.TabStop = false;
            this.grpImportDump.Text = "Import dump vào HOSPITAL_RESTORE";
            //
            // dgvRestoreHistory
            //
            this.dgvRestoreHistory.AllowUserToAddRows = false;
            this.dgvRestoreHistory.AllowUserToDeleteRows = false;
            this.dgvRestoreHistory.BackgroundColor = System.Drawing.Color.White;
            this.dgvRestoreHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRestoreHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRestoreHistory.Location = new System.Drawing.Point(8, 78);
            this.dgvRestoreHistory.MultiSelect = false;
            this.dgvRestoreHistory.Name = "dgvRestoreHistory";
            this.dgvRestoreHistory.ReadOnly = true;
            this.dgvRestoreHistory.RowHeadersWidth = 51;
            this.dgvRestoreHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRestoreHistory.Size = new System.Drawing.Size(1130, 162);
            this.dgvRestoreHistory.TabIndex = 1;
            this.dgvRestoreHistory.SelectionChanged += new System.EventHandler(this.dgvBackupHistory_SelectionChanged);
            //
            // pnlImportControls
            //
            this.pnlImportControls.Controls.Add(this.lblImportDumpFile);
            this.pnlImportControls.Controls.Add(this.txtImportDumpFile);
            this.pnlImportControls.Controls.Add(this.btnImportDataPump);
            this.pnlImportControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlImportControls.Location = new System.Drawing.Point(8, 30);
            this.pnlImportControls.Name = "pnlImportControls";
            this.pnlImportControls.Size = new System.Drawing.Size(1130, 48);
            this.pnlImportControls.TabIndex = 0;
            //
            // lblImportDumpFile
            //
            this.lblImportDumpFile.AutoSize = true;
            this.lblImportDumpFile.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblImportDumpFile.Location = new System.Drawing.Point(3, 14);
            this.lblImportDumpFile.Name = "lblImportDumpFile";
            this.lblImportDumpFile.Size = new System.Drawing.Size(76, 20);
            this.lblImportDumpFile.TabIndex = 0;
            this.lblImportDumpFile.Text = "File dump:";
            //
            // txtImportDumpFile
            //
            this.txtImportDumpFile.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtImportDumpFile.Location = new System.Drawing.Point(85, 10);
            this.txtImportDumpFile.Name = "txtImportDumpFile";
            this.txtImportDumpFile.Size = new System.Drawing.Size(390, 27);
            this.txtImportDumpFile.TabIndex = 1;
            //
            // btnImportDataPump
            //
            this.btnImportDataPump.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnImportDataPump.Location = new System.Drawing.Point(485, 7);
            this.btnImportDataPump.Name = "btnImportDataPump";
            this.btnImportDataPump.Size = new System.Drawing.Size(150, 34);
            this.btnImportDataPump.TabIndex = 2;
            this.btnImportDataPump.Text = "Import dump";
            this.btnImportDataPump.UseVisualStyleBackColor = true;
            this.btnImportDataPump.Click += new System.EventHandler(this.btnImportDataPump_Click);
            //
            // tabFlashback
            //
            this.tabFlashback.Controls.Add(this.tblFlashback);
            this.tabFlashback.Location = new System.Drawing.Point(4, 29);
            this.tabFlashback.Name = "tabFlashback";
            this.tabFlashback.Padding = new System.Windows.Forms.Padding(10);
            this.tabFlashback.Size = new System.Drawing.Size(1172, 687);
            this.tabFlashback.TabIndex = 1;
            this.tabFlashback.Text = "Flashback";
            this.tabFlashback.UseVisualStyleBackColor = true;
            //
            // tblFlashback
            //
            this.tblFlashback.ColumnCount = 2;
            this.tblFlashback.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tblFlashback.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tblFlashback.Controls.Add(this.grpFlashbackAudit, 0, 0);
            this.tblFlashback.Controls.Add(this.grpFlashbackPreview, 0, 1);
            this.tblFlashback.Controls.Add(this.grpDataView, 1, 0);
            this.tblFlashback.Controls.Add(this.grpCurrentData, 1, 1);
            this.tblFlashback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblFlashback.Location = new System.Drawing.Point(10, 10);
            this.tblFlashback.Name = "tblFlashback";
            this.tblFlashback.RowCount = 2;
            this.tblFlashback.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.tblFlashback.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.tblFlashback.Size = new System.Drawing.Size(1152, 667);
            this.tblFlashback.TabIndex = 0;
            //
            // grpFlashbackAudit
            //
            this.grpFlashbackAudit.Controls.Add(this.dgvFgaAudit);
            this.grpFlashbackAudit.Controls.Add(this.pnlAuditActions);
            this.grpFlashbackAudit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpFlashbackAudit.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpFlashbackAudit.Location = new System.Drawing.Point(3, 3);
            this.grpFlashbackAudit.Name = "grpFlashbackAudit";
            this.grpFlashbackAudit.Padding = new System.Windows.Forms.Padding(8);
            this.grpFlashbackAudit.Size = new System.Drawing.Size(685, 407);
            this.grpFlashbackAudit.TabIndex = 0;
            this.grpFlashbackAudit.TabStop = false;
            this.grpFlashbackAudit.Text = "Audit DML theo bảng";
            //
            // dgvFgaAudit
            //
            this.dgvFgaAudit.AllowUserToAddRows = false;
            this.dgvFgaAudit.AllowUserToDeleteRows = false;
            this.dgvFgaAudit.BackgroundColor = System.Drawing.Color.White;
            this.dgvFgaAudit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFgaAudit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFgaAudit.Location = new System.Drawing.Point(8, 112);
            this.dgvFgaAudit.MultiSelect = false;
            this.dgvFgaAudit.Name = "dgvFgaAudit";
            this.dgvFgaAudit.ReadOnly = true;
            this.dgvFgaAudit.RowHeadersWidth = 51;
            this.dgvFgaAudit.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFgaAudit.Size = new System.Drawing.Size(669, 287);
            this.dgvFgaAudit.TabIndex = 1;
            this.dgvFgaAudit.SelectionChanged += new System.EventHandler(this.dgvFgaAudit_SelectionChanged);
            //
            // pnlAuditActions
            //
            this.pnlAuditActions.Controls.Add(this.tabFlashbackTables);
            this.pnlAuditActions.Controls.Add(this.btnLoadAudit);
            this.pnlAuditActions.Controls.Add(this.btnPreviewFlashback);
            this.pnlAuditActions.Controls.Add(this.btnBackupRestore);
            this.pnlAuditActions.Controls.Add(this.lblSelectedAudit);
            this.pnlAuditActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAuditActions.Location = new System.Drawing.Point(8, 30);
            this.pnlAuditActions.Name = "pnlAuditActions";
            this.pnlAuditActions.Size = new System.Drawing.Size(669, 82);
            this.pnlAuditActions.TabIndex = 0;
            //
            // tabFlashbackTables
            //
            this.tabFlashbackTables.Controls.Add(this.tabPatientAudit);
            this.tabFlashbackTables.Controls.Add(this.tabMedicalAudit);
            this.tabFlashbackTables.Location = new System.Drawing.Point(3, 3);
            this.tabFlashbackTables.Name = "tabFlashbackTables";
            this.tabFlashbackTables.SelectedIndex = 0;
            this.tabFlashbackTables.Size = new System.Drawing.Size(400, 34);
            this.tabFlashbackTables.TabIndex = 0;
            this.tabFlashbackTables.SelectedIndexChanged += new System.EventHandler(this.tabFlashbackTables_SelectedIndexChanged);
            //
            // tabPatientAudit
            //
            this.tabPatientAudit.Location = new System.Drawing.Point(4, 29);
            this.tabPatientAudit.Name = "tabPatientAudit";
            this.tabPatientAudit.Size = new System.Drawing.Size(392, 1);
            this.tabPatientAudit.TabIndex = 0;
            this.tabPatientAudit.Text = "Bệnh nhân";
            this.tabPatientAudit.UseVisualStyleBackColor = true;
            //
            // tabMedicalAudit
            //
            this.tabMedicalAudit.Location = new System.Drawing.Point(4, 29);
            this.tabMedicalAudit.Name = "tabMedicalAudit";
            this.tabMedicalAudit.Size = new System.Drawing.Size(392, 1);
            this.tabMedicalAudit.TabIndex = 1;
            this.tabMedicalAudit.Text = "HSBA";
            this.tabMedicalAudit.UseVisualStyleBackColor = true;
            //
            // btnLoadAudit
            //
            this.btnLoadAudit.Location = new System.Drawing.Point(413, 3);
            this.btnLoadAudit.Name = "btnLoadAudit";
            this.btnLoadAudit.Size = new System.Drawing.Size(80, 34);
            this.btnLoadAudit.TabIndex = 1;
            this.btnLoadAudit.Text = "Tải";
            this.btnLoadAudit.UseVisualStyleBackColor = true;
            this.btnLoadAudit.Click += new System.EventHandler(this.btnLoadAudit_Click);
            //
            // btnPreviewFlashback
            //
            this.btnPreviewFlashback.Location = new System.Drawing.Point(499, 3);
            this.btnPreviewFlashback.Name = "btnPreviewFlashback";
            this.btnPreviewFlashback.Size = new System.Drawing.Size(80, 34);
            this.btnPreviewFlashback.TabIndex = 2;
            this.btnPreviewFlashback.Text = "Preview";
            this.btnPreviewFlashback.UseVisualStyleBackColor = true;
            this.btnPreviewFlashback.Click += new System.EventHandler(this.btnPreviewFlashback_Click);
            //
            // btnBackupRestore
            //
            this.btnBackupRestore.Location = new System.Drawing.Point(585, 3);
            this.btnBackupRestore.Name = "btnBackupRestore";
            this.btnBackupRestore.Size = new System.Drawing.Size(80, 34);
            this.btnBackupRestore.TabIndex = 3;
            this.btnBackupRestore.Text = "Restore";
            this.btnBackupRestore.UseVisualStyleBackColor = true;
            this.btnBackupRestore.Click += new System.EventHandler(this.btnBackupRestore_Click);
            //
            // lblSelectedAudit
            //
            this.lblSelectedAudit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)
            | System.Windows.Forms.AnchorStyles.Top)));
            this.lblSelectedAudit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSelectedAudit.ForeColor = System.Drawing.Color.FromArgb(96, 96, 104);
            this.lblSelectedAudit.Location = new System.Drawing.Point(3, 44);
            this.lblSelectedAudit.Name = "lblSelectedAudit";
            this.lblSelectedAudit.Size = new System.Drawing.Size(663, 30);
            this.lblSelectedAudit.TabIndex = 4;
            this.lblSelectedAudit.Text = "Chọn audit có parse key hợp lệ để preview/restore.";
            this.lblSelectedAudit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // grpFlashbackPreview
            //
            this.grpFlashbackPreview.Controls.Add(this.tblPreview);
            this.grpFlashbackPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpFlashbackPreview.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpFlashbackPreview.Location = new System.Drawing.Point(3, 416);
            this.grpFlashbackPreview.Name = "grpFlashbackPreview";
            this.grpFlashbackPreview.Padding = new System.Windows.Forms.Padding(8);
            this.grpFlashbackPreview.Size = new System.Drawing.Size(685, 248);
            this.grpFlashbackPreview.TabIndex = 1;
            this.grpFlashbackPreview.TabStop = false;
            this.grpFlashbackPreview.Text = "Preview dữ liệu hiện tại và dữ liệu trước SCN";
            //
            // tblPreview
            //
            this.tblPreview.ColumnCount = 1;
            this.tblPreview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblPreview.Controls.Add(this.lblCurrentRow, 0, 0);
            this.tblPreview.Controls.Add(this.dgvCurrentRow, 0, 1);
            this.tblPreview.Controls.Add(this.lblFlashbackRow, 0, 2);
            this.tblPreview.Controls.Add(this.dgvFlashbackRow, 0, 3);
            this.tblPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblPreview.Location = new System.Drawing.Point(8, 30);
            this.tblPreview.Name = "tblPreview";
            this.tblPreview.RowCount = 4;
            this.tblPreview.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblPreview.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPreview.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblPreview.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPreview.Size = new System.Drawing.Size(669, 210);
            this.tblPreview.TabIndex = 0;
            //
            // lblCurrentRow
            //
            this.lblCurrentRow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCurrentRow.Location = new System.Drawing.Point(3, 0);
            this.lblCurrentRow.Name = "lblCurrentRow";
            this.lblCurrentRow.Size = new System.Drawing.Size(663, 24);
            this.lblCurrentRow.TabIndex = 0;
            this.lblCurrentRow.Text = "Hiện tại";
            this.lblCurrentRow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblFlashbackRow
            //
            this.lblFlashbackRow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFlashbackRow.Location = new System.Drawing.Point(3, 105);
            this.lblFlashbackRow.Name = "lblFlashbackRow";
            this.lblFlashbackRow.Size = new System.Drawing.Size(663, 24);
            this.lblFlashbackRow.TabIndex = 1;
            this.lblFlashbackRow.Text = "AS OF SCN";
            this.lblFlashbackRow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // dgvCurrentRow
            //
            this.dgvCurrentRow.AllowUserToAddRows = false;
            this.dgvCurrentRow.AllowUserToDeleteRows = false;
            this.dgvCurrentRow.BackgroundColor = System.Drawing.Color.White;
            this.dgvCurrentRow.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvCurrentRow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCurrentRow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCurrentRow.Location = new System.Drawing.Point(3, 27);
            this.dgvCurrentRow.Name = "dgvCurrentRow";
            this.dgvCurrentRow.ReadOnly = true;
            this.dgvCurrentRow.RowHeadersWidth = 51;
            this.dgvCurrentRow.Size = new System.Drawing.Size(663, 75);
            this.dgvCurrentRow.TabIndex = 2;
            //
            // dgvFlashbackRow
            //
            this.dgvFlashbackRow.AllowUserToAddRows = false;
            this.dgvFlashbackRow.AllowUserToDeleteRows = false;
            this.dgvFlashbackRow.BackgroundColor = System.Drawing.Color.White;
            this.dgvFlashbackRow.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvFlashbackRow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFlashbackRow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFlashbackRow.Location = new System.Drawing.Point(3, 132);
            this.dgvFlashbackRow.Name = "dgvFlashbackRow";
            this.dgvFlashbackRow.ReadOnly = true;
            this.dgvFlashbackRow.RowHeadersWidth = 51;
            this.dgvFlashbackRow.Size = new System.Drawing.Size(663, 75);
            this.dgvFlashbackRow.TabIndex = 3;
            //
            // grpDataView
            //
            this.grpDataView.Controls.Add(this.tabControlData);
            this.grpDataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDataView.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpDataView.Location = new System.Drawing.Point(694, 3);
            this.grpDataView.Name = "grpDataView";
            this.grpDataView.Padding = new System.Windows.Forms.Padding(8);
            this.grpDataView.Size = new System.Drawing.Size(455, 407);
            this.grpDataView.TabIndex = 2;
            this.grpDataView.TabStop = false;
            this.grpDataView.Text = "Dữ liệu hiện tại";
            //
            // tabControlData
            //
            this.tabControlData.Controls.Add(this.tabMedicalRecord);
            this.tabControlData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlData.Location = new System.Drawing.Point(8, 30);
            this.tabControlData.Name = "tabControlData";
            this.tabControlData.SelectedIndex = 0;
            this.tabControlData.Size = new System.Drawing.Size(439, 369);
            this.tabControlData.TabIndex = 0;
            //
            // tabMedicalRecord
            //
            this.tabMedicalRecord.Controls.Add(this.dgvMedicalRecord);
            this.tabMedicalRecord.Location = new System.Drawing.Point(4, 29);
            this.tabMedicalRecord.Name = "tabMedicalRecord";
            this.tabMedicalRecord.Padding = new System.Windows.Forms.Padding(3);
            this.tabMedicalRecord.Size = new System.Drawing.Size(431, 336);
            this.tabMedicalRecord.TabIndex = 1;
            this.tabMedicalRecord.Text = "Hồ sơ bệnh án";
            this.tabMedicalRecord.UseVisualStyleBackColor = true;
            //
            // dgvMedicalRecord
            //
            this.dgvMedicalRecord.AllowUserToAddRows = false;
            this.dgvMedicalRecord.AllowUserToDeleteRows = false;
            this.dgvMedicalRecord.BackgroundColor = System.Drawing.Color.White;
            this.dgvMedicalRecord.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMedicalRecord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMedicalRecord.Location = new System.Drawing.Point(3, 3);
            this.dgvMedicalRecord.Name = "dgvMedicalRecord";
            this.dgvMedicalRecord.ReadOnly = true;
            this.dgvMedicalRecord.RowHeadersWidth = 51;
            this.dgvMedicalRecord.Size = new System.Drawing.Size(425, 330);
            this.dgvMedicalRecord.TabIndex = 0;
            //
            // grpCurrentData
            //
            this.grpCurrentData.Controls.Add(this.dgvStandardAudit);
            this.grpCurrentData.Controls.Add(this.pnlCurrentDataActions);
            this.grpCurrentData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCurrentData.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpCurrentData.Location = new System.Drawing.Point(694, 416);
            this.grpCurrentData.Name = "grpCurrentData";
            this.grpCurrentData.Padding = new System.Windows.Forms.Padding(8);
            this.grpCurrentData.Size = new System.Drawing.Size(455, 248);
            this.grpCurrentData.TabIndex = 3;
            this.grpCurrentData.TabStop = false;
            this.grpCurrentData.Text = "Standard Audit";
            //
            // dgvStandardAudit
            //
            this.dgvStandardAudit.AllowUserToAddRows = false;
            this.dgvStandardAudit.AllowUserToDeleteRows = false;
            this.dgvStandardAudit.BackgroundColor = System.Drawing.Color.White;
            this.dgvStandardAudit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStandardAudit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStandardAudit.Location = new System.Drawing.Point(8, 70);
            this.dgvStandardAudit.MultiSelect = false;
            this.dgvStandardAudit.Name = "dgvStandardAudit";
            this.dgvStandardAudit.ReadOnly = true;
            this.dgvStandardAudit.RowHeadersWidth = 51;
            this.dgvStandardAudit.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStandardAudit.Size = new System.Drawing.Size(439, 170);
            this.dgvStandardAudit.TabIndex = 1;
            //
            // pnlCurrentDataActions
            //
            this.pnlCurrentDataActions.Controls.Add(this.btnRefresh);
            this.pnlCurrentDataActions.Controls.Add(this.lblTotalRows);
            this.pnlCurrentDataActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCurrentDataActions.Location = new System.Drawing.Point(8, 30);
            this.pnlCurrentDataActions.Name = "pnlCurrentDataActions";
            this.pnlCurrentDataActions.Size = new System.Drawing.Size(439, 40);
            this.pnlCurrentDataActions.TabIndex = 0;
            //
            // btnRefresh
            //
            this.btnRefresh.Location = new System.Drawing.Point(3, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(110, 32);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // lblTotalRows
            //
            this.lblTotalRows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalRows.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblTotalRows.ForeColor = System.Drawing.Color.FromArgb(96, 96, 104);
            this.lblTotalRows.Location = new System.Drawing.Point(250, 8);
            this.lblTotalRows.Name = "lblTotalRows";
            this.lblTotalRows.Size = new System.Drawing.Size(180, 24);
            this.lblTotalRows.TabIndex = 1;
            this.lblTotalRows.Text = "Tổng số dòng: 0";
            this.lblTotalRows.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // frmBackupRecovery
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 720);
            this.Controls.Add(this.mainTabs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmBackupRecovery";
            this.Text = "Sao lưu và phục hồi dữ liệu";
            this.Load += new System.EventHandler(this.frmBackupRecovery_Load);
            this.mainTabs.ResumeLayout(false);
            this.tabDataPump.ResumeLayout(false);
            this.tblDataPump.ResumeLayout(false);
            this.grpBackupManagement.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBackupHistory)).EndInit();
            this.pnlBackupControls.ResumeLayout(false);
            this.pnlBackupControls.PerformLayout();
            this.grpImportDump.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRestoreHistory)).EndInit();
            this.pnlImportControls.ResumeLayout(false);
            this.pnlImportControls.PerformLayout();
            this.tabFlashback.ResumeLayout(false);
            this.tblFlashback.ResumeLayout(false);
            this.grpFlashbackAudit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFgaAudit)).EndInit();
            this.pnlAuditActions.ResumeLayout(false);
            this.tabFlashbackTables.ResumeLayout(false);
            this.grpFlashbackPreview.ResumeLayout(false);
            this.tblPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurrentRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFlashbackRow)).EndInit();
            this.grpDataView.ResumeLayout(false);
            this.tabControlData.ResumeLayout(false);
            this.tabMedicalRecord.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMedicalRecord)).EndInit();
            this.grpCurrentData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStandardAudit)).EndInit();
            this.pnlCurrentDataActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mainTabs;
        private System.Windows.Forms.TabPage tabDataPump;
        private System.Windows.Forms.TableLayoutPanel tblDataPump;
        private System.Windows.Forms.GroupBox grpBackupManagement;
        private System.Windows.Forms.DataGridView dgvBackupHistory;
        private System.Windows.Forms.FlowLayoutPanel pnlBackupControls;
        private System.Windows.Forms.Button btnBackupNow;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.ComboBox cboInterval;
        private System.Windows.Forms.Button btnEnableAutoBackup;
        private System.Windows.Forms.Button btnDisableAutoBackup;
        private System.Windows.Forms.Button btnRefreshBackup;
        private System.Windows.Forms.Button btnOpenErrorLog;
        private System.Windows.Forms.Label lblSchedulerStatus;
        private System.Windows.Forms.GroupBox grpImportDump;
        private System.Windows.Forms.DataGridView dgvRestoreHistory;
        private System.Windows.Forms.Panel pnlImportControls;
        private System.Windows.Forms.Label lblImportDumpFile;
        private System.Windows.Forms.TextBox txtImportDumpFile;
        private System.Windows.Forms.Button btnImportDataPump;
        private System.Windows.Forms.TabPage tabFlashback;
        private System.Windows.Forms.TableLayoutPanel tblFlashback;
        private System.Windows.Forms.GroupBox grpFlashbackAudit;
        private System.Windows.Forms.DataGridView dgvFgaAudit;
        private System.Windows.Forms.Panel pnlAuditActions;
        private System.Windows.Forms.TabControl tabFlashbackTables;
        private System.Windows.Forms.TabPage tabPatientAudit;
        private System.Windows.Forms.TabPage tabMedicalAudit;
        private System.Windows.Forms.Button btnLoadAudit;
        private System.Windows.Forms.Button btnPreviewFlashback;
        private System.Windows.Forms.Button btnBackupRestore;
        private System.Windows.Forms.Label lblSelectedAudit;
        private System.Windows.Forms.GroupBox grpFlashbackPreview;
        private System.Windows.Forms.TableLayoutPanel tblPreview;
        private System.Windows.Forms.DataGridView dgvCurrentRow;
        private System.Windows.Forms.DataGridView dgvFlashbackRow;
        private System.Windows.Forms.Label lblCurrentRow;
        private System.Windows.Forms.Label lblFlashbackRow;
        private System.Windows.Forms.GroupBox grpDataView;
        private System.Windows.Forms.TabControl tabControlData;
        private System.Windows.Forms.TabPage tabMedicalRecord;
        private System.Windows.Forms.DataGridView dgvMedicalRecord;
        private System.Windows.Forms.GroupBox grpCurrentData;
        private System.Windows.Forms.DataGridView dgvStandardAudit;
        private System.Windows.Forms.Panel pnlCurrentDataActions;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblTotalRows;
    }
}
