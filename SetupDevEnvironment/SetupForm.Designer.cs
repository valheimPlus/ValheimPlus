﻿namespace SetupDevEnvironment
{
    partial class SetupForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbValheimPlusInstallDir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btBrowseVPlusInstallDir = new System.Windows.Forms.Button();
            this.btStartInstallation = new System.Windows.Forms.Button();
            this.btBrowseValheimInstallDir = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbValheimInstallDir = new System.Windows.Forms.TextBox();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.btEditConfig = new System.Windows.Forms.Button();
            this.btStartDnSpy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbValheimPlusInstallDir
            // 
            this.tbValheimPlusInstallDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbValheimPlusInstallDir.Enabled = false;
            this.tbValheimPlusInstallDir.Location = new System.Drawing.Point(12, 84);
            this.tbValheimPlusInstallDir.Name = "tbValheimPlusInstallDir";
            this.tbValheimPlusInstallDir.PlaceholderText = "Where would you like to create the Valheim Plus folder?";
            this.tbValheimPlusInstallDir.Size = new System.Drawing.Size(739, 23);
            this.tbValheimPlusInstallDir.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(18, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Valheim Plus Installation Folder:";
            // 
            // btBrowseVPlusInstallDir
            // 
            this.btBrowseVPlusInstallDir.Location = new System.Drawing.Point(198, 59);
            this.btBrowseVPlusInstallDir.Name = "btBrowseVPlusInstallDir";
            this.btBrowseVPlusInstallDir.Size = new System.Drawing.Size(75, 23);
            this.btBrowseVPlusInstallDir.TabIndex = 2;
            this.btBrowseVPlusInstallDir.Text = "Browse...";
            this.btBrowseVPlusInstallDir.UseVisualStyleBackColor = true;
            this.btBrowseVPlusInstallDir.Click += new System.EventHandler(this.btBrowseVPlusInstallDir_Click);
            // 
            // btStartInstallation
            // 
            this.btStartInstallation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btStartInstallation.Enabled = false;
            this.btStartInstallation.Location = new System.Drawing.Point(12, 460);
            this.btStartInstallation.Name = "btStartInstallation";
            this.btStartInstallation.Size = new System.Drawing.Size(161, 62);
            this.btStartInstallation.TabIndex = 3;
            this.btStartInstallation.Text = "Setup Dev Environment";
            this.btStartInstallation.UseVisualStyleBackColor = true;
            this.btStartInstallation.Click += new System.EventHandler(this.btStartInstallation_Click);
            // 
            // btBrowseValheimInstallDir
            // 
            this.btBrowseValheimInstallDir.Location = new System.Drawing.Point(198, 5);
            this.btBrowseValheimInstallDir.Name = "btBrowseValheimInstallDir";
            this.btBrowseValheimInstallDir.Size = new System.Drawing.Size(75, 23);
            this.btBrowseValheimInstallDir.TabIndex = 6;
            this.btBrowseValheimInstallDir.Text = "Browse...";
            this.btBrowseValheimInstallDir.UseVisualStyleBackColor = true;
            this.btBrowseValheimInstallDir.Click += new System.EventHandler(this.btBrowseValheimInstallDir_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(18, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Valheim Installation Folder:";
            // 
            // tbValheimInstallDir
            // 
            this.tbValheimInstallDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbValheimInstallDir.Enabled = false;
            this.tbValheimInstallDir.Location = new System.Drawing.Point(12, 30);
            this.tbValheimInstallDir.Name = "tbValheimInstallDir";
            this.tbValheimInstallDir.PlaceholderText = "Where has Steam installed Valheim?";
            this.tbValheimInstallDir.Size = new System.Drawing.Size(739, 23);
            this.tbValheimInstallDir.TabIndex = 4;
            // 
            // tbLog
            // 
            this.tbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLog.Location = new System.Drawing.Point(12, 113);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(739, 341);
            this.tbLog.TabIndex = 7;
            this.tbLog.Visible = false;
            // 
            // btEditConfig
            // 
            this.btEditConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btEditConfig.Enabled = false;
            this.btEditConfig.Location = new System.Drawing.Point(590, 460);
            this.btEditConfig.Name = "btEditConfig";
            this.btEditConfig.Size = new System.Drawing.Size(161, 62);
            this.btEditConfig.TabIndex = 8;
            this.btEditConfig.Text = "Edit Valheim+ config";
            this.btEditConfig.UseVisualStyleBackColor = true;
            this.btEditConfig.Click += new System.EventHandler(this.btEditConfig_Click);
            // 
            // btStartDnSpy
            // 
            this.btStartDnSpy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btStartDnSpy.Enabled = false;
            this.btStartDnSpy.Location = new System.Drawing.Point(423, 460);
            this.btStartDnSpy.Name = "btStartDnSpy";
            this.btStartDnSpy.Size = new System.Drawing.Size(161, 62);
            this.btStartDnSpy.TabIndex = 9;
            this.btStartDnSpy.Text = "Start DnSpy";
            this.btStartDnSpy.UseVisualStyleBackColor = true;
            this.btStartDnSpy.Click += new System.EventHandler(this.btStartDnSpy_Click);
            // 
            // SetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SetupDevEnvironment.Properties.Resources.Valheim_Wallpaper;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(763, 534);
            this.Controls.Add(this.btStartDnSpy);
            this.Controls.Add(this.btEditConfig);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.btBrowseValheimInstallDir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbValheimInstallDir);
            this.Controls.Add(this.btStartInstallation);
            this.Controls.Add(this.btBrowseVPlusInstallDir);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbValheimPlusInstallDir);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SetupForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Valheim Plus Development Plus Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox tbValheimPlusInstallDir;
        private Label label1;
        private Button btBrowseVPlusInstallDir;
        private Button btStartInstallation;
        private Button btBrowseValheimInstallDir;
        private Label label2;
        private TextBox tbValheimInstallDir;
        private TextBox tbLog;
        private Button btEditConfig;
        private Button btStartDnSpy;
    }
}