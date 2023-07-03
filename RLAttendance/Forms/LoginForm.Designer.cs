namespace AttendanceSolution.Forms
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::RLAttendance.SplashScreen), false, false);
            this.lblIndicate = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditSegments = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.textEditUsername = new DevExpress.XtraEditors.TextEdit();
            this.textEditPassword = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.hyperlinkLabelControl1 = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.btnLogin = new DevExpress.XtraEditors.SimpleButton();
            this.hyLblRegister = new DevExpress.XtraEditors.HyperlinkLabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditSegments.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditUsername.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPassword.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblIndicate
            // 
            this.lblIndicate.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("labelControl1.Appearance.Font")));
            this.lblIndicate.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.lblIndicate, "lblIndicate");
            this.lblIndicate.Name = "lblIndicate";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("labelControl2.Appearance.Font")));
            this.labelControl2.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.labelControl2, "labelControl2");
            this.labelControl2.Name = "labelControl2";
            // 
            // lookUpEditSegments
            // 
            resources.ApplyResources(this.lookUpEditSegments, "lookUpEditSegments");
            this.lookUpEditSegments.Name = "lookUpEditSegments";
            this.lookUpEditSegments.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("lookUpEdit1.Properties.Buttons"))))});
            this.lookUpEditSegments.Properties.NullText = resources.GetString("lookUpEdit1.Properties.NullText");
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("labelControl3.Appearance.Font")));
            this.labelControl3.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.labelControl3, "labelControl3");
            this.labelControl3.Name = "labelControl3";
            // 
            // textEditUsername
            // 
            resources.ApplyResources(this.textEditUsername, "textEditUsername");
            this.textEditUsername.Name = "textEditUsername";
            // 
            // textEditPassword
            // 
            resources.ApplyResources(this.textEditPassword, "textEditPassword");
            this.textEditPassword.Name = "textEditPassword";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("labelControl4.Appearance.Font")));
            this.labelControl4.Appearance.Options.UseFont = true;
            resources.ApplyResources(this.labelControl4, "labelControl4");
            this.labelControl4.Name = "labelControl4";
            // 
            // hyperlinkLabelControl1
            // 
            resources.ApplyResources(this.hyperlinkLabelControl1, "hyperlinkLabelControl1");
            this.hyperlinkLabelControl1.Name = "hyperlinkLabelControl1";
            // 
            // btnLogin
            // 
            resources.ApplyResources(this.btnLogin, "btnLogin");
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // hyLblRegister
            // 
            resources.ApplyResources(this.hyLblRegister, "hyLblRegister");
            this.hyLblRegister.Name = "hyLblRegister";
            this.hyLblRegister.Click += new System.EventHandler(this.hyLblRegister_Click);
            // 
            // splashScreenManager1
            // 
            splashScreenManager1.ClosingDelay = 3000;
            // 
            // LoginForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hyLblRegister);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.hyperlinkLabelControl1);
            this.Controls.Add(this.textEditPassword);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.textEditUsername);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.lookUpEditSegments);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.lblIndicate);
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditSegments.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditUsername.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPassword.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblIndicate;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditSegments;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit textEditUsername;
        private DevExpress.XtraEditors.TextEdit textEditPassword;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.HyperlinkLabelControl hyperlinkLabelControl1;
        private DevExpress.XtraEditors.SimpleButton btnLogin;
        private DevExpress.XtraEditors.HyperlinkLabelControl hyLblRegister;
    }
}