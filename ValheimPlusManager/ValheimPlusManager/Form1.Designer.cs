
namespace ValheimPlusManager
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.installClientButton = new System.Windows.Forms.Button();
            this.clientInstalledLabel = new System.Windows.Forms.Label();
            this.serverInstalledLabel = new System.Windows.Forms.Label();
            this.installServerButton = new System.Windows.Forms.Button();
            this.manageServerButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.manageClientButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(-1, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(404, 43);
            this.label1.TabIndex = 0;
            this.label1.Text = "ValheimPlus Manager";
            // 
            // installClientButton
            // 
            this.installClientButton.Location = new System.Drawing.Point(8, 110);
            this.installClientButton.Name = "installClientButton";
            this.installClientButton.Size = new System.Drawing.Size(185, 32);
            this.installClientButton.TabIndex = 1;
            this.installClientButton.Text = "Install ValheimPlus on client";
            this.installClientButton.UseVisualStyleBackColor = true;
            // 
            // clientInstalledLabel
            // 
            this.clientInstalledLabel.AutoSize = true;
            this.clientInstalledLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.clientInstalledLabel.Location = new System.Drawing.Point(8, 74);
            this.clientInstalledLabel.Name = "clientInstalledLabel";
            this.clientInstalledLabel.Size = new System.Drawing.Size(167, 15);
            this.clientInstalledLabel.TabIndex = 2;
            this.clientInstalledLabel.Text = "ValheimPlus installed on client";
            // 
            // serverInstalledLabel
            // 
            this.serverInstalledLabel.AutoSize = true;
            this.serverInstalledLabel.Location = new System.Drawing.Point(8, 194);
            this.serverInstalledLabel.Name = "serverInstalledLabel";
            this.serverInstalledLabel.Size = new System.Drawing.Size(169, 15);
            this.serverInstalledLabel.TabIndex = 3;
            this.serverInstalledLabel.Text = "ValheimPlus installed on server";
            // 
            // installServerButton
            // 
            this.installServerButton.Location = new System.Drawing.Point(8, 228);
            this.installServerButton.Name = "installServerButton";
            this.installServerButton.Size = new System.Drawing.Size(185, 32);
            this.installServerButton.TabIndex = 4;
            this.installServerButton.Text = "Install ValheimPlus on server";
            this.installServerButton.UseVisualStyleBackColor = true;
            this.installServerButton.Click += new System.EventHandler(this.installServerButton_Click);
            // 
            // manageServerButton
            // 
            this.manageServerButton.Location = new System.Drawing.Point(205, 228);
            this.manageServerButton.Name = "manageServerButton";
            this.manageServerButton.Size = new System.Drawing.Size(185, 32);
            this.manageServerButton.TabIndex = 5;
            this.manageServerButton.Text = "Manage ValheimPlus settings";
            this.manageServerButton.UseVisualStyleBackColor = true;
            this.manageServerButton.Click += new System.EventHandler(this.manageServerButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 21);
            this.label2.TabIndex = 6;
            this.label2.Text = "Client";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(6, 169);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 21);
            this.label3.TabIndex = 7;
            this.label3.Text = "Server";
            // 
            // manageClientButton
            // 
            this.manageClientButton.Location = new System.Drawing.Point(205, 110);
            this.manageClientButton.Name = "manageClientButton";
            this.manageClientButton.Size = new System.Drawing.Size(185, 32);
            this.manageClientButton.TabIndex = 8;
            this.manageClientButton.Text = "Manage ValheimPlus settings";
            this.manageClientButton.UseVisualStyleBackColor = true;
            this.manageClientButton.Click += new System.EventHandler(this.manageClientButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(402, 269);
            this.Controls.Add(this.manageClientButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.manageServerButton);
            this.Controls.Add(this.installServerButton);
            this.Controls.Add(this.serverInstalledLabel);
            this.Controls.Add(this.clientInstalledLabel);
            this.Controls.Add(this.installClientButton);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ValheimPlus Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button installClientButton;
        private System.Windows.Forms.Label clientInstalledLabel;
        private System.Windows.Forms.Label serverInstalledLabel;
        private System.Windows.Forms.Button installServerButton;
        private System.Windows.Forms.Button manageServerButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button manageClientButton;
    }
}

