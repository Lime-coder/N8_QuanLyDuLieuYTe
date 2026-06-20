using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyYTe.Forms.Patient
{
    partial class frmPatient
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pnlLogo = new System.Windows.Forms.Panel();
            this.lblLogoTitle = new System.Windows.Forms.Label();
            this.lblLogoSubtitle = new System.Windows.Forms.Label();
            this.pnlNav = new System.Windows.Forms.Panel();
            this.btnNavProfile = new System.Windows.Forms.Button();
            this.btnNavRecords = new System.Windows.Forms.Button();
            this.btnNavLogout = new System.Windows.Forms.Button();
            
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlTopbar = new System.Windows.Forms.Panel();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.lblPageBreadcrumb = new System.Windows.Forms.Label();
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.pnlDivider = new System.Windows.Forms.Panel();
            
            this.pnlContent = new System.Windows.Forms.Panel();
            
            this.pnlProfile = new System.Windows.Forms.Panel();
            this.lblPatientIdTitle = new System.Windows.Forms.Label();
            this.lblPatientId = new System.Windows.Forms.Label();
            this.lblFullNameTitle = new System.Windows.Forms.Label();
            this.lblFullName = new System.Windows.Forms.Label();
            this.lblGenderTitle = new System.Windows.Forms.Label();
            this.lblGender = new System.Windows.Forms.Label();
            this.lblBirthdateTitle = new System.Windows.Forms.Label();
            this.lblBirthdate = new System.Windows.Forms.Label();
            this.lblIdCardTitle = new System.Windows.Forms.Label();
            this.lblIdCard = new System.Windows.Forms.Label();
            
            this.lblMedicalHistory = new System.Windows.Forms.Label();
            this.txtMedicalHistory = new System.Windows.Forms.TextBox();
            this.lblFamilyMedicalHistory = new System.Windows.Forms.Label();
            this.txtFamilyMedicalHistory = new System.Windows.Forms.TextBox();
            this.lblDrugAllergies = new System.Windows.Forms.Label();
            this.txtDrugAllergies = new System.Windows.Forms.TextBox();
            
            this.lblContactAddress = new System.Windows.Forms.Label();
            this.tlpAddress = new System.Windows.Forms.TableLayoutPanel();
            this.pnlHouseNo = new System.Windows.Forms.Panel();
            this.lblHouseNoTitle = new System.Windows.Forms.Label();
            this.txtHouseNo = new System.Windows.Forms.TextBox();
            this.pnlStreet = new System.Windows.Forms.Panel();
            this.lblStreetTitle = new System.Windows.Forms.Label();
            this.txtStreet = new System.Windows.Forms.TextBox();
            this.pnlDistrict = new System.Windows.Forms.Panel();
            this.lblDistrictTitle = new System.Windows.Forms.Label();
            this.txtDistrict = new System.Windows.Forms.TextBox();
            this.pnlCityProvince = new System.Windows.Forms.Panel();
            this.lblCityProvinceTitle = new System.Windows.Forms.Label();
            this.txtCityProvince = new System.Windows.Forms.TextBox();
            
            this.btnSaveContact = new System.Windows.Forms.Button();

            this.pnlRecords = new System.Windows.Forms.Panel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTopTitle = new System.Windows.Forms.Label();
            this.pnlFilterBar = new System.Windows.Forms.TableLayoutPanel();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblToDate = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblDept = new System.Windows.Forms.Label();
            this.cboDept = new System.Windows.Forms.ComboBox();
            this.lblDoctor = new System.Windows.Forms.Label();
            this.cboDoctor = new System.Windows.Forms.ComboBox();
            this.txtSearchRecords = new System.Windows.Forms.TextBox();
            this.btnClearFilter = new System.Windows.Forms.Button();
            this.dgvRecords = new System.Windows.Forms.DataGridView();
            
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblDetailTitle = new System.Windows.Forms.Label();
            this.splitBottom = new System.Windows.Forms.TableLayoutPanel();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.lblLeft = new System.Windows.Forms.Label();
            this.dgvPrescriptions = new System.Windows.Forms.DataGridView();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.lblRight = new System.Windows.Forms.Label();
            this.dgvServices = new System.Windows.Forms.DataGridView();

            this.pnlSidebar.SuspendLayout();
            this.pnlLogo.SuspendLayout();
            this.pnlNav.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlTopbar.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlProfile.SuspendLayout();
            this.tlpAddress.SuspendLayout();
            this.pnlHouseNo.SuspendLayout();
            this.pnlStreet.SuspendLayout();
            this.pnlDistrict.SuspendLayout();
            this.pnlCityProvince.SuspendLayout();
            this.pnlRecords.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlFilterBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.splitBottom.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrescriptions)).BeginInit();
            this.pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServices)).BeginInit();
            this.SuspendLayout();

            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(28)))));
            this.pnlSidebar.Controls.Add(this.pnlNav);
            this.pnlSidebar.Controls.Add(this.pnlLogo);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(240, 700);
            this.pnlSidebar.TabIndex = 0;
            // 
            // pnlLogo
            // 
            this.pnlLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(35)))));
            this.pnlLogo.Controls.Add(this.lblLogoSubtitle);
            this.pnlLogo.Controls.Add(this.lblLogoTitle);
            this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogo.Location = new System.Drawing.Point(0, 0);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(240, 88);
            this.pnlLogo.TabIndex = 0;
            // 
            // lblLogoTitle
            // 
            this.lblLogoTitle.AutoSize = true;
            this.lblLogoTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblLogoTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(40)))));
            this.lblLogoTitle.Location = new System.Drawing.Point(20, 18);
            this.lblLogoTitle.Name = "lblLogoTitle";
            this.lblLogoTitle.Size = new System.Drawing.Size(146, 25);
            this.lblLogoTitle.TabIndex = 0;
            this.lblLogoTitle.Text = "Hospital Portal";
            // 
            // lblLogoSubtitle
            // 
            this.lblLogoSubtitle.AutoSize = true;
            this.lblLogoSubtitle.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this.lblLogoSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(130)))));
            this.lblLogoSubtitle.Location = new System.Drawing.Point(22, 52);
            this.lblLogoSubtitle.Name = "lblLogoSubtitle";
            this.lblLogoSubtitle.Size = new System.Drawing.Size(100, 12);
            this.lblLogoSubtitle.TabIndex = 1;
            this.lblLogoSubtitle.Text = "Patient Management";
            // 
            // pnlNav
            // 
            this.pnlNav.Controls.Add(this.btnNavLogout);
            this.pnlNav.Controls.Add(this.btnNavRecords);
            this.pnlNav.Controls.Add(this.btnNavProfile);
            this.pnlNav.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNav.Location = new System.Drawing.Point(0, 88);
            this.pnlNav.Name = "pnlNav";
            this.pnlNav.Size = new System.Drawing.Size(240, 612);
            this.pnlNav.TabIndex = 1;
            // 
            // btnNavProfile
            // 
            this.btnNavProfile.BackColor = System.Drawing.Color.Transparent;
            this.btnNavProfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavProfile.FlatAppearance.BorderSize = 0;
            this.btnNavProfile.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(58)))));
            this.btnNavProfile.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.btnNavProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavProfile.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnNavProfile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.btnNavProfile.Location = new System.Drawing.Point(12, 10);
            this.btnNavProfile.Name = "btnNavProfile";
            this.btnNavProfile.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnNavProfile.Size = new System.Drawing.Size(216, 46);
            this.btnNavProfile.TabIndex = 0;
            this.btnNavProfile.Text = "  Hồ sơ cá nhân";
            this.btnNavProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavProfile.UseVisualStyleBackColor = false;
            this.btnNavProfile.Click += new System.EventHandler(this.btnNavProfile_Click);
            // 
            // btnNavRecords
            // 
            this.btnNavRecords.BackColor = System.Drawing.Color.Transparent;
            this.btnNavRecords.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavRecords.FlatAppearance.BorderSize = 0;
            this.btnNavRecords.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(58)))));
            this.btnNavRecords.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.btnNavRecords.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavRecords.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnNavRecords.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(200)))));
            this.btnNavRecords.Location = new System.Drawing.Point(12, 62);
            this.btnNavRecords.Name = "btnNavRecords";
            this.btnNavRecords.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnNavRecords.Size = new System.Drawing.Size(216, 46);
            this.btnNavRecords.TabIndex = 1;
            this.btnNavRecords.Text = "  Lịch sử khám";
            this.btnNavRecords.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavRecords.UseVisualStyleBackColor = false;
            this.btnNavRecords.Click += new System.EventHandler(this.btnNavRecords_Click);
            // 
            // btnNavLogout
            // 
            this.btnNavLogout.BackColor = System.Drawing.Color.Transparent;
            this.btnNavLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavLogout.FlatAppearance.BorderSize = 0;
            this.btnNavLogout.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(58)))));
            this.btnNavLogout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.btnNavLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavLogout.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnNavLogout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnNavLogout.Location = new System.Drawing.Point(12, 114);
            this.btnNavLogout.Name = "btnNavLogout";
            this.btnNavLogout.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnNavLogout.Size = new System.Drawing.Size(216, 46);
            this.btnNavLogout.TabIndex = 2;
            this.btnNavLogout.Text = "  Đăng xuất";
            this.btnNavLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavLogout.UseVisualStyleBackColor = false;
            this.btnNavLogout.Click += new System.EventHandler(this.BtnLogout_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlContent);
            this.pnlMain.Controls.Add(this.pnlDivider);
            this.pnlMain.Controls.Add(this.pnlTopbar);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(240, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(860, 700);
            this.pnlMain.TabIndex = 1;
            // 
            // pnlTopbar
            // 
            this.pnlTopbar.BackColor = System.Drawing.Color.White;
            this.pnlTopbar.Controls.Add(this.lblUserInfo);
            this.pnlTopbar.Controls.Add(this.lblPageBreadcrumb);
            this.pnlTopbar.Controls.Add(this.lblPageTitle);
            this.pnlTopbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopbar.Location = new System.Drawing.Point(0, 0);
            this.pnlTopbar.Name = "pnlTopbar";
            this.pnlTopbar.Size = new System.Drawing.Size(860, 68);
            this.pnlTopbar.TabIndex = 0;
            // 
            // lblPageTitle
            // 
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblPageTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.lblPageTitle.Location = new System.Drawing.Point(28, 10);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(146, 28);
            this.lblPageTitle.TabIndex = 0;
            this.lblPageTitle.Text = "Page Title";
            // 
            // lblPageBreadcrumb
            // 
            this.lblPageBreadcrumb.AutoSize = true;
            this.lblPageBreadcrumb.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblPageBreadcrumb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(170)))));
            this.lblPageBreadcrumb.Location = new System.Drawing.Point(30, 42);
            this.lblPageBreadcrumb.Name = "lblPageBreadcrumb";
            this.lblPageBreadcrumb.Size = new System.Drawing.Size(74, 15);
            this.lblPageBreadcrumb.TabIndex = 1;
            this.lblPageBreadcrumb.Text = "Breadcrumb";
            // 
            // lblUserInfo
            // 
            this.lblUserInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserInfo.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblUserInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(40)))));
            this.lblUserInfo.Location = new System.Drawing.Point(536, 24);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(300, 20);
            this.lblUserInfo.TabIndex = 2;
            this.lblUserInfo.Text = "USER  ·  PATIENT";
            this.lblUserInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlDivider
            // 
            this.pnlDivider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(40)))));
            this.pnlDivider.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDivider.Location = new System.Drawing.Point(0, 68);
            this.pnlDivider.Name = "pnlDivider";
            this.pnlDivider.Size = new System.Drawing.Size(860, 3);
            this.pnlDivider.TabIndex = 1;
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.pnlProfile);
            this.pnlContent.Controls.Add(this.pnlRecords);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 71);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(24);
            this.pnlContent.Size = new System.Drawing.Size(860, 629);
            this.pnlContent.TabIndex = 2;
            // 
            // pnlProfile
            // 
            this.pnlProfile.BackColor = System.Drawing.Color.White;
            this.pnlProfile.Controls.Add(this.btnSaveContact);
            this.pnlProfile.Controls.Add(this.tlpAddress);
            this.pnlProfile.Controls.Add(this.lblContactAddress);
            this.pnlProfile.Controls.Add(this.txtDrugAllergies);
            this.pnlProfile.Controls.Add(this.lblDrugAllergies);
            this.pnlProfile.Controls.Add(this.txtFamilyMedicalHistory);
            this.pnlProfile.Controls.Add(this.lblFamilyMedicalHistory);
            this.pnlProfile.Controls.Add(this.txtMedicalHistory);
            this.pnlProfile.Controls.Add(this.lblMedicalHistory);
            this.pnlProfile.Controls.Add(this.lblIdCard);
            this.pnlProfile.Controls.Add(this.lblIdCardTitle);
            this.pnlProfile.Controls.Add(this.lblBirthdate);
            this.pnlProfile.Controls.Add(this.lblBirthdateTitle);
            this.pnlProfile.Controls.Add(this.lblGender);
            this.pnlProfile.Controls.Add(this.lblGenderTitle);
            this.pnlProfile.Controls.Add(this.lblFullName);
            this.pnlProfile.Controls.Add(this.lblFullNameTitle);
            this.pnlProfile.Controls.Add(this.lblPatientId);
            this.pnlProfile.Controls.Add(this.lblPatientIdTitle);
            this.pnlProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProfile.Location = new System.Drawing.Point(24, 24);
            this.pnlProfile.Name = "pnlProfile";
            this.pnlProfile.Padding = new System.Windows.Forms.Padding(24);
            this.pnlProfile.Size = new System.Drawing.Size(812, 581);
            this.pnlProfile.TabIndex = 0;
            // 
            // lblPatientIdTitle
            // 
            this.lblPatientIdTitle.AutoSize = true;
            this.lblPatientIdTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPatientIdTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(90)))));
            this.lblPatientIdTitle.Location = new System.Drawing.Point(24, 24);
            this.lblPatientIdTitle.Name = "lblPatientIdTitle";
            this.lblPatientIdTitle.Size = new System.Drawing.Size(98, 17);
            this.lblPatientIdTitle.TabIndex = 0;
            this.lblPatientIdTitle.Text = "Mã bệnh nhân:";
            // 
            // lblPatientId
            // 
            this.lblPatientId.AutoSize = true;
            this.lblPatientId.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblPatientId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.lblPatientId.Location = new System.Drawing.Point(180, 24);
            this.lblPatientId.Name = "lblPatientId";
            this.lblPatientId.Size = new System.Drawing.Size(20, 17);
            this.lblPatientId.TabIndex = 1;
            this.lblPatientId.Text = "—";
            // 
            // lblFullNameTitle
            // 
            this.lblFullNameTitle.AutoSize = true;
            this.lblFullNameTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblFullNameTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(90)))));
            this.lblFullNameTitle.Location = new System.Drawing.Point(24, 56);
            this.lblFullNameTitle.Name = "lblFullNameTitle";
            this.lblFullNameTitle.Size = new System.Drawing.Size(71, 17);
            this.lblFullNameTitle.TabIndex = 2;
            this.lblFullNameTitle.Text = "Họ và tên:";
            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblFullName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.lblFullName.Location = new System.Drawing.Point(180, 56);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(20, 17);
            this.lblFullName.TabIndex = 3;
            this.lblFullName.Text = "—";
            // 
            // lblGenderTitle
            // 
            this.lblGenderTitle.AutoSize = true;
            this.lblGenderTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblGenderTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(90)))));
            this.lblGenderTitle.Location = new System.Drawing.Point(24, 88);
            this.lblGenderTitle.Name = "lblGenderTitle";
            this.lblGenderTitle.Size = new System.Drawing.Size(66, 17);
            this.lblGenderTitle.TabIndex = 4;
            this.lblGenderTitle.Text = "Giới tính:";
            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblGender.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.lblGender.Location = new System.Drawing.Point(180, 88);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(20, 17);
            this.lblGender.TabIndex = 5;
            this.lblGender.Text = "—";
            // 
            // lblBirthdateTitle
            // 
            this.lblBirthdateTitle.AutoSize = true;
            this.lblBirthdateTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblBirthdateTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(90)))));
            this.lblBirthdateTitle.Location = new System.Drawing.Point(24, 120);
            this.lblBirthdateTitle.Name = "lblBirthdateTitle";
            this.lblBirthdateTitle.Size = new System.Drawing.Size(74, 17);
            this.lblBirthdateTitle.TabIndex = 6;
            this.lblBirthdateTitle.Text = "Ngày sinh:";
            // 
            // lblBirthdate
            // 
            this.lblBirthdate.AutoSize = true;
            this.lblBirthdate.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblBirthdate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.lblBirthdate.Location = new System.Drawing.Point(180, 120);
            this.lblBirthdate.Name = "lblBirthdate";
            this.lblBirthdate.Size = new System.Drawing.Size(20, 17);
            this.lblBirthdate.TabIndex = 7;
            this.lblBirthdate.Text = "—";
            // 
            // lblIdCardTitle
            // 
            this.lblIdCardTitle.AutoSize = true;
            this.lblIdCardTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblIdCardTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(90)))));
            this.lblIdCardTitle.Location = new System.Drawing.Point(24, 152);
            this.lblIdCardTitle.Name = "lblIdCardTitle";
            this.lblIdCardTitle.Size = new System.Drawing.Size(46, 17);
            this.lblIdCardTitle.TabIndex = 8;
            this.lblIdCardTitle.Text = "CCCD:";
            // 
            // lblIdCard
            // 
            this.lblIdCard.AutoSize = true;
            this.lblIdCard.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblIdCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            this.lblIdCard.Location = new System.Drawing.Point(180, 152);
            this.lblIdCard.Name = "lblIdCard";
            this.lblIdCard.Size = new System.Drawing.Size(20, 17);
            this.lblIdCard.TabIndex = 9;
            this.lblIdCard.Text = "—";
            // 
            // lblMedicalHistory
            // 
            this.lblMedicalHistory.AutoSize = true;
            this.lblMedicalHistory.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMedicalHistory.Location = new System.Drawing.Point(24, 196);
            this.lblMedicalHistory.Name = "lblMedicalHistory";
            this.lblMedicalHistory.Size = new System.Drawing.Size(94, 15);
            this.lblMedicalHistory.TabIndex = 10;
            this.lblMedicalHistory.Text = "Tiền sử bệnh lý:";
            // 
            // txtMedicalHistory
            // 
            this.txtMedicalHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMedicalHistory.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtMedicalHistory.Location = new System.Drawing.Point(24, 218);
            this.txtMedicalHistory.Multiline = true;
            this.txtMedicalHistory.Name = "txtMedicalHistory";
            this.txtMedicalHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMedicalHistory.Size = new System.Drawing.Size(764, 48);
            this.txtMedicalHistory.TabIndex = 11;
            // 
            // lblFamilyMedicalHistory
            // 
            this.lblFamilyMedicalHistory.AutoSize = true;
            this.lblFamilyMedicalHistory.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFamilyMedicalHistory.Location = new System.Drawing.Point(24, 278);
            this.lblFamilyMedicalHistory.Name = "lblFamilyMedicalHistory";
            this.lblFamilyMedicalHistory.Size = new System.Drawing.Size(100, 15);
            this.lblFamilyMedicalHistory.TabIndex = 12;
            this.lblFamilyMedicalHistory.Text = "Bệnh lý gia đình:";
            // 
            // txtFamilyMedicalHistory
            // 
            this.txtFamilyMedicalHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFamilyMedicalHistory.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtFamilyMedicalHistory.Location = new System.Drawing.Point(24, 300);
            this.txtFamilyMedicalHistory.Multiline = true;
            this.txtFamilyMedicalHistory.Name = "txtFamilyMedicalHistory";
            this.txtFamilyMedicalHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFamilyMedicalHistory.Size = new System.Drawing.Size(764, 48);
            this.txtFamilyMedicalHistory.TabIndex = 13;
            // 
            // lblDrugAllergies
            // 
            this.lblDrugAllergies.AutoSize = true;
            this.lblDrugAllergies.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDrugAllergies.Location = new System.Drawing.Point(24, 360);
            this.lblDrugAllergies.Name = "lblDrugAllergies";
            this.lblDrugAllergies.Size = new System.Drawing.Size(81, 15);
            this.lblDrugAllergies.TabIndex = 14;
            this.lblDrugAllergies.Text = "Dị ứng thuốc:";
            // 
            // txtDrugAllergies
            // 
            this.txtDrugAllergies.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDrugAllergies.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtDrugAllergies.Location = new System.Drawing.Point(24, 382);
            this.txtDrugAllergies.Multiline = true;
            this.txtDrugAllergies.Name = "txtDrugAllergies";
            this.txtDrugAllergies.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDrugAllergies.Size = new System.Drawing.Size(764, 36);
            this.txtDrugAllergies.TabIndex = 15;
            // 
            // lblContactAddress
            // 
            this.lblContactAddress.AutoSize = true;
            this.lblContactAddress.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblContactAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(40)))));
            this.lblContactAddress.Location = new System.Drawing.Point(24, 442);
            this.lblContactAddress.Name = "lblContactAddress";
            this.lblContactAddress.Size = new System.Drawing.Size(107, 15);
            this.lblContactAddress.TabIndex = 16;
            this.lblContactAddress.Text = "ĐỊA CHỈ LIÊN LẠC";
            // 
            // tlpAddress
            // 
            this.tlpAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpAddress.ColumnCount = 2;
            this.tlpAddress.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAddress.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAddress.Controls.Add(this.pnlHouseNo, 0, 0);
            this.tlpAddress.Controls.Add(this.pnlStreet, 1, 0);
            this.tlpAddress.Controls.Add(this.pnlDistrict, 0, 1);
            this.tlpAddress.Controls.Add(this.pnlCityProvince, 1, 1);
            this.tlpAddress.Location = new System.Drawing.Point(24, 470);
            this.tlpAddress.Name = "tlpAddress";
            this.tlpAddress.RowCount = 2;
            this.tlpAddress.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAddress.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAddress.Size = new System.Drawing.Size(764, 140);
            this.tlpAddress.TabIndex = 17;
            // 
            // pnlHouseNo
            // 
            this.pnlHouseNo.Controls.Add(this.lblHouseNoTitle);
            this.pnlHouseNo.Controls.Add(this.txtHouseNo);
            this.pnlHouseNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHouseNo.Location = new System.Drawing.Point(0, 0);
            this.pnlHouseNo.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.pnlHouseNo.Name = "pnlHouseNo";
            this.pnlHouseNo.Size = new System.Drawing.Size(362, 70);
            this.pnlHouseNo.TabIndex = 0;
            // 
            // lblHouseNoTitle
            // 
            this.lblHouseNoTitle.AutoSize = true;
            this.lblHouseNoTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblHouseNoTitle.Location = new System.Drawing.Point(0, 0);
            this.lblHouseNoTitle.Name = "lblHouseNoTitle";
            this.lblHouseNoTitle.Size = new System.Drawing.Size(48, 15);
            this.lblHouseNoTitle.TabIndex = 0;
            this.lblHouseNoTitle.Text = "Số nhà:";
            // 
            // txtHouseNo
            // 
            this.txtHouseNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHouseNo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtHouseNo.Location = new System.Drawing.Point(0, 22);
            this.txtHouseNo.Name = "txtHouseNo";
            this.txtHouseNo.Size = new System.Drawing.Size(362, 24);
            this.txtHouseNo.TabIndex = 1;
            // 
            // pnlStreet
            // 
            this.pnlStreet.Controls.Add(this.lblStreetTitle);
            this.pnlStreet.Controls.Add(this.txtStreet);
            this.pnlStreet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStreet.Location = new System.Drawing.Point(382, 0);
            this.pnlStreet.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.pnlStreet.Name = "pnlStreet";
            this.pnlStreet.Size = new System.Drawing.Size(362, 70);
            this.pnlStreet.TabIndex = 1;
            // 
            // lblStreetTitle
            // 
            this.lblStreetTitle.AutoSize = true;
            this.lblStreetTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStreetTitle.Location = new System.Drawing.Point(0, 0);
            this.lblStreetTitle.Name = "lblStreetTitle";
            this.lblStreetTitle.Size = new System.Drawing.Size(68, 15);
            this.lblStreetTitle.TabIndex = 0;
            this.lblStreetTitle.Text = "Tên đường:";
            // 
            // txtStreet
            // 
            this.txtStreet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStreet.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtStreet.Location = new System.Drawing.Point(0, 22);
            this.txtStreet.Name = "txtStreet";
            this.txtStreet.Size = new System.Drawing.Size(362, 24);
            this.txtStreet.TabIndex = 1;
            // 
            // pnlDistrict
            // 
            this.pnlDistrict.Controls.Add(this.lblDistrictTitle);
            this.pnlDistrict.Controls.Add(this.txtDistrict);
            this.pnlDistrict.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDistrict.Location = new System.Drawing.Point(0, 70);
            this.pnlDistrict.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.pnlDistrict.Name = "pnlDistrict";
            this.pnlDistrict.Size = new System.Drawing.Size(362, 70);
            this.pnlDistrict.TabIndex = 2;
            // 
            // lblDistrictTitle
            // 
            this.lblDistrictTitle.AutoSize = true;
            this.lblDistrictTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDistrictTitle.Location = new System.Drawing.Point(0, 0);
            this.lblDistrictTitle.Name = "lblDistrictTitle";
            this.lblDistrictTitle.Size = new System.Drawing.Size(79, 15);
            this.lblDistrictTitle.TabIndex = 0;
            this.lblDistrictTitle.Text = "Quận/Huyện:";
            // 
            // txtDistrict
            // 
            this.txtDistrict.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDistrict.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtDistrict.Location = new System.Drawing.Point(0, 22);
            this.txtDistrict.Name = "txtDistrict";
            this.txtDistrict.Size = new System.Drawing.Size(362, 24);
            this.txtDistrict.TabIndex = 1;
            // 
            // pnlCityProvince
            // 
            this.pnlCityProvince.Controls.Add(this.lblCityProvinceTitle);
            this.pnlCityProvince.Controls.Add(this.txtCityProvince);
            this.pnlCityProvince.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCityProvince.Location = new System.Drawing.Point(382, 70);
            this.pnlCityProvince.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.pnlCityProvince.Name = "pnlCityProvince";
            this.pnlCityProvince.Size = new System.Drawing.Size(362, 70);
            this.pnlCityProvince.TabIndex = 3;
            // 
            // lblCityProvinceTitle
            // 
            this.lblCityProvinceTitle.AutoSize = true;
            this.lblCityProvinceTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCityProvinceTitle.Location = new System.Drawing.Point(0, 0);
            this.lblCityProvinceTitle.Name = "lblCityProvinceTitle";
            this.lblCityProvinceTitle.Size = new System.Drawing.Size(96, 15);
            this.lblCityProvinceTitle.TabIndex = 0;
            this.lblCityProvinceTitle.Text = "Tỉnh/Thành phố:";
            // 
            // txtCityProvince
            // 
            this.txtCityProvince.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCityProvince.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtCityProvince.Location = new System.Drawing.Point(0, 22);
            this.txtCityProvince.Name = "txtCityProvince";
            this.txtCityProvince.Size = new System.Drawing.Size(362, 24);
            this.txtCityProvince.TabIndex = 1;
            // 
            // btnSaveContact
            // 
            this.btnSaveContact.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(40)))));
            this.btnSaveContact.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveContact.FlatAppearance.BorderSize = 0;
            this.btnSaveContact.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveContact.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSaveContact.ForeColor = System.Drawing.Color.White;
            this.btnSaveContact.Location = new System.Drawing.Point(24, 620);
            this.btnSaveContact.Name = "btnSaveContact";
            this.btnSaveContact.Size = new System.Drawing.Size(160, 36);
            this.btnSaveContact.TabIndex = 18;
            this.btnSaveContact.Text = "LƯU THÔNG TIN";
            this.btnSaveContact.UseVisualStyleBackColor = false;
            this.btnSaveContact.Click += new System.EventHandler(this.BtnSaveContact_Click);
            // 
            // pnlRecords
            // 
            this.pnlRecords.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(244)))), ((int)(((byte)(242)))));
            this.pnlRecords.Controls.Add(this.splitContainer);
            this.pnlRecords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRecords.Location = new System.Drawing.Point(24, 24);
            this.pnlRecords.Name = "pnlRecords";
            this.pnlRecords.Size = new System.Drawing.Size(812, 581);
            this.pnlRecords.TabIndex = 1;
            this.pnlRecords.Visible = false;
            // 
            // splitContainer
            // 
            this.splitContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(244)))), ((int)(((byte)(242)))));
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.pnlTop);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.pnlBottom);
            this.splitContainer.Size = new System.Drawing.Size(812, 581);
            this.splitContainer.SplitterDistance = 350;
            this.splitContainer.SplitterWidth = 8;
            this.splitContainer.TabIndex = 0;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.dgvRecords);
            this.pnlTop.Controls.Add(this.pnlFilterBar);
            this.pnlTop.Controls.Add(this.lblTopTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(12);
            this.pnlTop.Size = new System.Drawing.Size(812, 350);
            this.pnlTop.TabIndex = 0;
            // 
            // lblTopTitle
            // 
            this.lblTopTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTopTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTopTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(40)))));
            this.lblTopTitle.Location = new System.Drawing.Point(12, 12);
            this.lblTopTitle.Name = "lblTopTitle";
            this.lblTopTitle.Size = new System.Drawing.Size(788, 24);
            this.lblTopTitle.TabIndex = 0;
            this.lblTopTitle.Text = "DANH SÁCH HỒ SƠ KHÁM";
            // 
            // pnlFilterBar
            // 
            this.pnlFilterBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlFilterBar.ColumnCount = 10;
            this.pnlFilterBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlFilterBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.pnlFilterBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlFilterBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.pnlFilterBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlFilterBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.pnlFilterBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlFilterBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.pnlFilterBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.pnlFilterBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.pnlFilterBar.Controls.Add(this.lblFromDate, 0, 0);
            this.pnlFilterBar.Controls.Add(this.dtpFrom, 1, 0);
            this.pnlFilterBar.Controls.Add(this.lblToDate, 2, 0);
            this.pnlFilterBar.Controls.Add(this.dtpTo, 3, 0);
            this.pnlFilterBar.Controls.Add(this.lblDept, 4, 0);
            this.pnlFilterBar.Controls.Add(this.cboDept, 5, 0);
            this.pnlFilterBar.Controls.Add(this.lblDoctor, 6, 0);
            this.pnlFilterBar.Controls.Add(this.cboDoctor, 7, 0);
            this.pnlFilterBar.Controls.Add(this.txtSearchRecords, 8, 0);
            this.pnlFilterBar.Controls.Add(this.btnClearFilter, 9, 0);
            this.pnlFilterBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilterBar.Location = new System.Drawing.Point(12, 36);
            this.pnlFilterBar.Name = "pnlFilterBar";
            this.pnlFilterBar.Padding = new System.Windows.Forms.Padding(4, 8, 4, 0);
            this.pnlFilterBar.RowCount = 1;
            this.pnlFilterBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlFilterBar.Size = new System.Drawing.Size(788, 46);
            this.pnlFilterBar.TabIndex = 1;
            // 
            // lblFromDate
            // 
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblFromDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(90)))));
            this.lblFromDate.Location = new System.Drawing.Point(4, 13);
            this.lblFromDate.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(53, 15);
            this.lblFromDate.TabIndex = 0;
            this.lblFromDate.Text = "Từ ngày:";
            // 
            // dtpFrom
            // 
            this.dtpFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpFrom.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(61, 8);
            this.dtpFrom.Margin = new System.Windows.Forms.Padding(4, 0, 8, 0);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(61, 23);
            this.dtpFrom.TabIndex = 1;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.FilterRecords_Event);
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblToDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(90)))));
            this.lblToDate.Location = new System.Drawing.Point(130, 13);
            this.lblToDate.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(31, 15);
            this.lblToDate.TabIndex = 2;
            this.lblToDate.Text = "Đến:";
            // 
            // dtpTo
            // 
            this.dtpTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpTo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(165, 8);
            this.dtpTo.Margin = new System.Windows.Forms.Padding(4, 0, 8, 0);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(61, 23);
            this.dtpTo.TabIndex = 3;
            this.dtpTo.ValueChanged += new System.EventHandler(this.FilterRecords_Event);
            // 
            // lblDept
            // 
            this.lblDept.AutoSize = true;
            this.lblDept.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblDept.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(90)))));
            this.lblDept.Location = new System.Drawing.Point(234, 13);
            this.lblDept.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.lblDept.Name = "lblDept";
            this.lblDept.Size = new System.Drawing.Size(37, 15);
            this.lblDept.TabIndex = 4;
            this.lblDept.Text = "Khoa:";
            // 
            // cboDept
            // 
            this.cboDept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDept.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDept.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboDept.FormattingEnabled = true;
            this.cboDept.Location = new System.Drawing.Point(275, 8);
            this.cboDept.Margin = new System.Windows.Forms.Padding(4, 0, 8, 0);
            this.cboDept.Name = "cboDept";
            this.cboDept.Size = new System.Drawing.Size(61, 23);
            this.cboDept.TabIndex = 5;
            this.cboDept.SelectedIndexChanged += new System.EventHandler(this.FilterRecords_Event);
            // 
            // lblDoctor
            // 
            this.lblDoctor.AutoSize = true;
            this.lblDoctor.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblDoctor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(90)))));
            this.lblDoctor.Location = new System.Drawing.Point(344, 13);
            this.lblDoctor.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.lblDoctor.Name = "lblDoctor";
            this.lblDoctor.Size = new System.Drawing.Size(42, 15);
            this.lblDoctor.TabIndex = 6;
            this.lblDoctor.Text = "Bác sĩ:";
            // 
            // cboDoctor
            // 
            this.cboDoctor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDoctor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDoctor.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboDoctor.FormattingEnabled = true;
            this.cboDoctor.Location = new System.Drawing.Point(390, 8);
            this.cboDoctor.Margin = new System.Windows.Forms.Padding(4, 0, 8, 0);
            this.cboDoctor.Name = "cboDoctor";
            this.cboDoctor.Size = new System.Drawing.Size(61, 23);
            this.cboDoctor.TabIndex = 7;
            this.cboDoctor.SelectedIndexChanged += new System.EventHandler(this.FilterRecords_Event);
            // 
            // txtSearchRecords
            // 
            this.txtSearchRecords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchRecords.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSearchRecords.Location = new System.Drawing.Point(463, 8);
            this.txtSearchRecords.Margin = new System.Windows.Forms.Padding(4, 0, 8, 0);
            this.txtSearchRecords.Name = "txtSearchRecords";
            this.txtSearchRecords.PlaceholderText = "Tìm chẩn đoán, kết luận...";
            this.txtSearchRecords.Size = new System.Drawing.Size(242, 23);
            this.txtSearchRecords.TabIndex = 8;
            this.txtSearchRecords.TextChanged += new System.EventHandler(this.FilterRecords_Event);
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.btnClearFilter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearFilter.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(220)))));
            this.btnClearFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearFilter.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btnClearFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(90)))));
            this.btnClearFilter.Location = new System.Drawing.Point(717, 8);
            this.btnClearFilter.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(64, 25);
            this.btnClearFilter.TabIndex = 9;
            this.btnClearFilter.Text = "✕ Xóa";
            this.btnClearFilter.UseVisualStyleBackColor = false;
            this.btnClearFilter.Click += new System.EventHandler(this.BtnClearFilter_Click);
            // 
            // dgvRecords
            // 
            this.dgvRecords.AllowUserToAddRows = false;
            this.dgvRecords.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRecords.BackgroundColor = System.Drawing.Color.White;
            this.dgvRecords.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRecords.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvRecords.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(4, 8, 4, 8);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRecords.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRecords.ColumnHeadersHeight = 40;
            this.dgvRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(240)))), ((int)(((byte)(230)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRecords.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRecords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecords.EnableHeadersVisualStyles = false;
            this.dgvRecords.Location = new System.Drawing.Point(12, 82);
            this.dgvRecords.Name = "dgvRecords";
            this.dgvRecords.ReadOnly = true;
            this.dgvRecords.RowTemplate.Height = 36;
            this.dgvRecords.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecords.Size = new System.Drawing.Size(788, 256);
            this.dgvRecords.TabIndex = 2;
            this.dgvRecords.SelectionChanged += new System.EventHandler(this.DgvRecords_SelectionChanged);
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.White;
            this.pnlBottom.Controls.Add(this.splitBottom);
            this.pnlBottom.Controls.Add(this.lblDetailTitle);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(0, 0);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(12);
            this.pnlBottom.Size = new System.Drawing.Size(812, 223);
            this.pnlBottom.TabIndex = 0;
            // 
            // lblDetailTitle
            // 
            this.lblDetailTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDetailTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDetailTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(40)))));
            this.lblDetailTitle.Location = new System.Drawing.Point(12, 12);
            this.lblDetailTitle.Name = "lblDetailTitle";
            this.lblDetailTitle.Size = new System.Drawing.Size(788, 24);
            this.lblDetailTitle.TabIndex = 0;
            this.lblDetailTitle.Text = "CHI TIẾT HỒ SƠ: Chưa chọn";
            // 
            // splitBottom
            // 
            this.splitBottom.BackColor = System.Drawing.Color.White;
            this.splitBottom.ColumnCount = 2;
            this.splitBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.splitBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.splitBottom.Controls.Add(this.pnlLeft, 0, 0);
            this.splitBottom.Controls.Add(this.pnlRight, 1, 0);
            this.splitBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitBottom.Location = new System.Drawing.Point(12, 36);
            this.splitBottom.Name = "splitBottom";
            this.splitBottom.RowCount = 1;
            this.splitBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.splitBottom.Size = new System.Drawing.Size(788, 175);
            this.splitBottom.TabIndex = 1;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.dgvPrescriptions);
            this.pnlLeft.Controls.Add(this.lblLeft);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(3, 3);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.pnlLeft.Size = new System.Drawing.Size(388, 169);
            this.pnlLeft.TabIndex = 0;
            // 
            // lblLeft
            // 
            this.lblLeft.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLeft.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblLeft.Location = new System.Drawing.Point(0, 0);
            this.lblLeft.Name = "lblLeft";
            this.lblLeft.Size = new System.Drawing.Size(380, 20);
            this.lblLeft.TabIndex = 0;
            this.lblLeft.Text = "Đơn thuốc";
            // 
            // dgvPrescriptions
            // 
            this.dgvPrescriptions.AllowUserToAddRows = false;
            this.dgvPrescriptions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPrescriptions.BackgroundColor = System.Drawing.Color.White;
            this.dgvPrescriptions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPrescriptions.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvPrescriptions.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(4, 8, 4, 8);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPrescriptions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvPrescriptions.ColumnHeadersHeight = 40;
            this.dgvPrescriptions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(240)))), ((int)(((byte)(230)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPrescriptions.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvPrescriptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPrescriptions.EnableHeadersVisualStyles = false;
            this.dgvPrescriptions.Location = new System.Drawing.Point(0, 20);
            this.dgvPrescriptions.Name = "dgvPrescriptions";
            this.dgvPrescriptions.ReadOnly = true;
            this.dgvPrescriptions.RowTemplate.Height = 36;
            this.dgvPrescriptions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPrescriptions.Size = new System.Drawing.Size(380, 149);
            this.dgvPrescriptions.TabIndex = 1;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.dgvServices);
            this.pnlRight.Controls.Add(this.lblRight);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(397, 3);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.pnlRight.Size = new System.Drawing.Size(388, 169);
            this.pnlRight.TabIndex = 1;
            // 
            // lblRight
            // 
            this.lblRight.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRight.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblRight.Location = new System.Drawing.Point(8, 0);
            this.lblRight.Name = "lblRight";
            this.lblRight.Size = new System.Drawing.Size(380, 20);
            this.lblRight.TabIndex = 0;
            this.lblRight.Text = "Dịch vụ y tế";
            // 
            // dgvServices
            // 
            this.dgvServices.AllowUserToAddRows = false;
            this.dgvServices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvServices.BackgroundColor = System.Drawing.Color.White;
            this.dgvServices.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvServices.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvServices.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(4, 8, 4, 8);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvServices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvServices.ColumnHeadersHeight = 40;
            this.dgvServices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(240)))), ((int)(((byte)(230)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(32)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvServices.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvServices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvServices.EnableHeadersVisualStyles = false;
            this.dgvServices.Location = new System.Drawing.Point(8, 20);
            this.dgvServices.Name = "dgvServices";
            this.dgvServices.ReadOnly = true;
            this.dgvServices.RowTemplate.Height = 36;
            this.dgvServices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvServices.Size = new System.Drawing.Size(380, 149);
            this.dgvServices.TabIndex = 1;
            // 
            // frmPatient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(244)))), ((int)(((byte)(242)))));
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlSidebar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(950, 600);
            this.Name = "frmPatient";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hospital - Patient Portal";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlSidebar.ResumeLayout(false);
            this.pnlLogo.ResumeLayout(false);
            this.pnlLogo.PerformLayout();
            this.pnlNav.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlTopbar.ResumeLayout(false);
            this.pnlTopbar.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.pnlProfile.ResumeLayout(false);
            this.pnlProfile.PerformLayout();
            this.tlpAddress.ResumeLayout(false);
            this.pnlHouseNo.ResumeLayout(false);
            this.pnlHouseNo.PerformLayout();
            this.pnlStreet.ResumeLayout(false);
            this.pnlStreet.PerformLayout();
            this.pnlDistrict.ResumeLayout(false);
            this.pnlDistrict.PerformLayout();
            this.pnlCityProvince.ResumeLayout(false);
            this.pnlCityProvince.PerformLayout();
            this.pnlRecords.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlFilterBar.ResumeLayout(false);
            this.pnlFilterBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.splitBottom.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrescriptions)).EndInit();
            this.pnlRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvServices)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlLogo;
        private System.Windows.Forms.Label lblLogoTitle;
        private System.Windows.Forms.Label lblLogoSubtitle;
        private System.Windows.Forms.Panel pnlNav;
        private System.Windows.Forms.Button btnNavProfile;
        private System.Windows.Forms.Button btnNavRecords;
        private System.Windows.Forms.Button btnNavLogout;
        
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlTopbar;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Label lblPageBreadcrumb;
        private System.Windows.Forms.Label lblUserInfo;
        private System.Windows.Forms.Panel pnlDivider;
        private System.Windows.Forms.Panel pnlContent;
        
        private System.Windows.Forms.Panel pnlProfile;
        private System.Windows.Forms.Label lblPatientIdTitle;
        private System.Windows.Forms.Label lblPatientId;
        private System.Windows.Forms.Label lblFullNameTitle;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.Label lblGenderTitle;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.Label lblBirthdateTitle;
        private System.Windows.Forms.Label lblBirthdate;
        private System.Windows.Forms.Label lblIdCardTitle;
        private System.Windows.Forms.Label lblIdCard;
        
        private System.Windows.Forms.Label lblMedicalHistory;
        private System.Windows.Forms.TextBox txtMedicalHistory;
        private System.Windows.Forms.Label lblFamilyMedicalHistory;
        private System.Windows.Forms.TextBox txtFamilyMedicalHistory;
        private System.Windows.Forms.Label lblDrugAllergies;
        private System.Windows.Forms.TextBox txtDrugAllergies;
        
        private System.Windows.Forms.Label lblContactAddress;
        private System.Windows.Forms.TableLayoutPanel tlpAddress;
        private System.Windows.Forms.Panel pnlHouseNo;
        private System.Windows.Forms.Label lblHouseNoTitle;
        private System.Windows.Forms.TextBox txtHouseNo;
        private System.Windows.Forms.Panel pnlStreet;
        private System.Windows.Forms.Label lblStreetTitle;
        private System.Windows.Forms.TextBox txtStreet;
        private System.Windows.Forms.Panel pnlDistrict;
        private System.Windows.Forms.Label lblDistrictTitle;
        private System.Windows.Forms.TextBox txtDistrict;
        private System.Windows.Forms.Panel pnlCityProvince;
        private System.Windows.Forms.Label lblCityProvinceTitle;
        private System.Windows.Forms.TextBox txtCityProvince;
        
        private System.Windows.Forms.Button btnSaveContact;

        private System.Windows.Forms.Panel pnlRecords;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTopTitle;
        private System.Windows.Forms.TableLayoutPanel pnlFilterBar;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblDept;
        private System.Windows.Forms.ComboBox cboDept;
        private System.Windows.Forms.Label lblDoctor;
        private System.Windows.Forms.ComboBox cboDoctor;
        private System.Windows.Forms.TextBox txtSearchRecords;
        private System.Windows.Forms.Button btnClearFilter;
        private System.Windows.Forms.DataGridView dgvRecords;
        
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblDetailTitle;
        private System.Windows.Forms.TableLayoutPanel splitBottom;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Label lblLeft;
        private System.Windows.Forms.DataGridView dgvPrescriptions;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Label lblRight;
        private System.Windows.Forms.DataGridView dgvServices;
    }
}
