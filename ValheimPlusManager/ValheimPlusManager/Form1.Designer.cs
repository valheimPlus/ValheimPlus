
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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(4, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(244, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "ValheimPlus Manager";
            // 
            // installClientButton
            // 
            this.installClientButton.Location = new System.Drawing.Point(12, 406);
            this.installClientButton.Name = "installClientButton";
            this.installClientButton.Size = new System.Drawing.Size(185, 32);
            this.installClientButton.TabIndex = 1;
            this.installClientButton.Text = "Install ValheimPlus on client";
            this.installClientButton.UseVisualStyleBackColor = true;
            // 
            // clientInstalledLabel
            // 
            this.clientInstalledLabel.AutoSize = true;
            this.clientInstalledLabel.Location = new System.Drawing.Point(8, 41);
            this.clientInstalledLabel.Name = "clientInstalledLabel";
            this.clientInstalledLabel.Size = new System.Drawing.Size(38, 15);
            this.clientInstalledLabel.TabIndex = 2;
            this.clientInstalledLabel.Text = "label2";
            // 
            // serverInstalledLabel
            // 
            this.serverInstalledLabel.AutoSize = true;
            this.serverInstalledLabel.Location = new System.Drawing.Point(8, 57);
            this.serverInstalledLabel.Name = "serverInstalledLabel";
            this.serverInstalledLabel.Size = new System.Drawing.Size(38, 15);
            this.serverInstalledLabel.TabIndex = 3;
            this.serverInstalledLabel.Text = "label3";
            // 
            // installServerButton
            // 
            this.installServerButton.Location = new System.Drawing.Point(202, 406);
            this.installServerButton.Name = "installServerButton";
            this.installServerButton.Size = new System.Drawing.Size(185, 32);
            this.installServerButton.TabIndex = 4;
            this.installServerButton.Text = "Install ValheimPlus on server";
            this.installServerButton.UseVisualStyleBackColor = true;
            this.installServerButton.Click += new System.EventHandler(this.installServerButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
    }
}

