namespace AttendanceSolution.Forms
{
    partial class CreateUser
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
            this.lblIndicate = new DevExpress.XtraEditors.LabelControl();
            this.textEditPassword = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.textEditUsername = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditSegments = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.hyperlinkLabelControl1 = new DevExpress.XtraEditors.HyperlinkLabelControl();
            this.btnRegister = new DevExpress.XtraEditors.SimpleButton();
            this.btnClear = new DevExpress.XtraEditors.SimpleButton();
            this.textEditConfirm = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditEmployee = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditUsername.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditSegments.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditConfirm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditEmployee.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblIndicate
            // 
            this.lblIndicate.Appearance.Font = new System.Drawing.Font("Tahoma", 17F, System.Drawing.FontStyle.Bold);
            this.lblIndicate.Appearance.Options.UseFont = true;
            this.lblIndicate.Location = new System.Drawing.Point(45, 24);
            this.lblIndicate.Name = "lblIndicate";
            this.lblIndicate.Size = new System.Drawing.Size(329, 28);
            this.lblIndicate.TabIndex = 1;
            this.lblIndicate.Text = "Perfect Attendance Solution";
            // 
            // textEditPassword
            // 
            this.textEditPassword.Location = new System.Drawing.Point(152, 150);
            this.textEditPassword.Name = "textEditPassword";
            this.textEditPassword.Properties.UseMaskAsDisplayFormat = true;
            this.textEditPassword.Properties.UseSystemPasswordChar = true;
            this.textEditPassword.Size = new System.Drawing.Size(210, 20);
            this.textEditPassword.TabIndex = 13;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(45, 153);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(54, 13);
            this.labelControl4.TabIndex = 12;
            this.labelControl4.Text = "Password";
            // 
            // textEditUsername
            // 
            this.textEditUsername.Location = new System.Drawing.Point(152, 124);
            this.textEditUsername.Name = "textEditUsername";
            this.textEditUsername.Size = new System.Drawing.Size(210, 20);
            this.textEditUsername.TabIndex = 11;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(45, 127);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(58, 13);
            this.labelControl3.TabIndex = 10;
            this.labelControl3.Text = "Username";
            // 
            // lookUpEditSegments
            // 
            this.lookUpEditSegments.Location = new System.Drawing.Point(152, 72);
            this.lookUpEditSegments.Name = "lookUpEditSegments";
            this.lookUpEditSegments.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditSegments.Properties.NullText = "Please Select  Segment";
            this.lookUpEditSegments.Size = new System.Drawing.Size(210, 20);
            this.lookUpEditSegments.TabIndex = 9;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(45, 75);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(51, 13);
            this.labelControl2.TabIndex = 8;
            this.labelControl2.Text = "Segment";
            // 
            // hyperlinkLabelControl1
            // 
            this.hyperlinkLabelControl1.Location = new System.Drawing.Point(45, 209);
            this.hyperlinkLabelControl1.Name = "hyperlinkLabelControl1";
            this.hyperlinkLabelControl1.Size = new System.Drawing.Size(78, 13);
            this.hyperlinkLabelControl1.TabIndex = 14;
            this.hyperlinkLabelControl1.Text = "Already Sign In?";
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(43, 232);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(98, 35);
            this.btnRegister.TabIndex = 15;
            this.btnRegister.Text = "Sign Up";
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(147, 232);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(98, 35);
            this.btnClear.TabIndex = 16;
            this.btnClear.Text = "Clear";
            // 
            // textEditConfirm
            // 
            this.textEditConfirm.Location = new System.Drawing.Point(152, 176);
            this.textEditConfirm.Name = "textEditConfirm";
            this.textEditConfirm.Properties.UseMaskAsDisplayFormat = true;
            this.textEditConfirm.Properties.UseSystemPasswordChar = true;
            this.textEditConfirm.Size = new System.Drawing.Size(210, 20);
            this.textEditConfirm.TabIndex = 18;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(45, 179);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(101, 13);
            this.labelControl1.TabIndex = 17;
            this.labelControl1.Text = "Confirm Password";
            // 
            // lookUpEditEmployee
            // 
            this.lookUpEditEmployee.Location = new System.Drawing.Point(152, 98);
            this.lookUpEditEmployee.Name = "lookUpEditEmployee";
            this.lookUpEditEmployee.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditEmployee.Properties.NullText = "Please Select Employee";
            this.lookUpEditEmployee.Size = new System.Drawing.Size(210, 20);
            this.lookUpEditEmployee.TabIndex = 20;
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(45, 101);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(55, 13);
            this.labelControl5.TabIndex = 19;
            this.labelControl5.Text = "Employee";
            // 
            // CreateUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 309);
            this.Controls.Add(this.lookUpEditEmployee);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.textEditConfirm);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.hyperlinkLabelControl1);
            this.Controls.Add(this.textEditPassword);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.textEditUsername);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.lookUpEditSegments);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.lblIndicate);
            this.MaximizeBox = false;
            this.Name = "CreateUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create User";
            this.Load += new System.EventHandler(this.CreateUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textEditPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditUsername.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditSegments.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditConfirm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditEmployee.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblIndicate;
        private DevExpress.XtraEditors.TextEdit textEditPassword;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit textEditUsername;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditSegments;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.HyperlinkLabelControl hyperlinkLabelControl1;
        private DevExpress.XtraEditors.SimpleButton btnRegister;
        private DevExpress.XtraEditors.SimpleButton btnClear;
        private DevExpress.XtraEditors.TextEdit textEditConfirm;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditEmployee;
        private DevExpress.XtraEditors.LabelControl labelControl5;
    }
}