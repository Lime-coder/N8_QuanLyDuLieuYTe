namespace QuanLyYTe.Forms.DBA
{
    partial class frmEditRole
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
            lblRole = new Label();
            lblPassword = new Label();

            txtRole = new TextBox();
            chkNotIdentified = new CheckBox();
            txtPassword = new TextBox();
            btnOk = new Button();
            btnCancel = new Button();

            SuspendLayout();

            lblRole.AutoSize = true;
            lblRole.Location = new Point(16, 18);
            lblRole.Text = "Tên role";

            txtRole.Location = new Point(110, 14);
            txtRole.Size = new Size(260, 27);

            chkNotIdentified.AutoSize = true;
            chkNotIdentified.Location = new Point(110, 54);
            chkNotIdentified.Text = "NOT IDENTIFIED (không mật khẩu)";
            chkNotIdentified.CheckedChanged += new System.EventHandler(this.chkNotIdentified_CheckedChanged);

            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(16, 90);
            lblPassword.Text = "Mật khẩu";

            txtPassword.Location = new Point(110, 86);
            txtPassword.Size = new Size(260, 27);
            txtPassword.UseSystemPasswordChar = true;

            btnOk.Location = new Point(214, 130);
            btnOk.Size = new Size(75, 30);
            btnOk.Text = "Đồng ý";
            btnOk.DialogResult = DialogResult.OK;

            btnCancel.Location = new Point(295, 130);
            btnCancel.Size = new Size(75, 30);
            btnCancel.Text = "Hủy";
            btnCancel.DialogResult = DialogResult.Cancel;

            AcceptButton = btnOk;
            CancelButton = btnCancel;
            ClientSize = new Size(392, 178);
            Controls.Add(lblRole);
            Controls.Add(txtRole);
            Controls.Add(chkNotIdentified);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblRole;
        private Label lblPassword;
        private TextBox txtRole;
        private CheckBox chkNotIdentified;
        private TextBox txtPassword;
        private Button btnOk;
        private Button btnCancel;
    }
}
