
namespace ValheimPlusManager
{
    partial class MainForm
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
            this.statusLabel = new System.Windows.Forms.Label();
            this.installClientUpdateMaterialButton = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.manageClientMaterialButton = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.checkCLientUpdatesMaterialButton = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.clientInstalledMaterialLabel = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.materialLabel1 = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.installClientMaterialButton = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.clientMaterialCard = new MaterialSkin2DotNet.Controls.MaterialCard();
            this.serverMaterialCard = new MaterialSkin2DotNet.Controls.MaterialCard();
            this.installServerUpdateMaterialButton = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.materialLabel2 = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.manageServerMaterialButton = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.serverInstalledMaterialLabel = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.installServerMaterialButton = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.checkServerUpdatesMaterialButton = new MaterialSkin2DotNet.Controls.MaterialButton();
            this.clientMaterialCard.SuspendLayout();
            this.serverMaterialCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.statusLabel.ForeColor = System.Drawing.Color.Red;
            this.statusLabel.Location = new System.Drawing.Point(9, 663);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(50, 20);
            this.statusLabel.TabIndex = 11;
            this.statusLabel.Text = "Status";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // installClientUpdateMaterialButton
            // 
            this.installClientUpdateMaterialButton.AutoSize = false;
            this.installClientUpdateMaterialButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.installClientUpdateMaterialButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.installClientUpdateMaterialButton.Depth = 0;
            this.installClientUpdateMaterialButton.DrawShadows = true;
            this.installClientUpdateMaterialButton.HighEmphasis = true;
            this.installClientUpdateMaterialButton.Icon = null;
            this.installClientUpdateMaterialButton.Location = new System.Drawing.Point(11, 221);
            this.installClientUpdateMaterialButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.installClientUpdateMaterialButton.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.installClientUpdateMaterialButton.Name = "installClientUpdateMaterialButton";
            this.installClientUpdateMaterialButton.Size = new System.Drawing.Size(438, 36);
            this.installClientUpdateMaterialButton.TabIndex = 17;
            this.installClientUpdateMaterialButton.Text = "Install update";
            this.installClientUpdateMaterialButton.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.installClientUpdateMaterialButton.UseAccentColor = false;
            this.installClientUpdateMaterialButton.UseVisualStyleBackColor = true;
            this.installClientUpdateMaterialButton.Click += new System.EventHandler(this.installClientUpdateMaterialButton_Click);
            // 
            // manageClientMaterialButton
            // 
            this.manageClientMaterialButton.AutoSize = false;
            this.manageClientMaterialButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.manageClientMaterialButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.manageClientMaterialButton.Depth = 0;
            this.manageClientMaterialButton.DrawShadows = true;
            this.manageClientMaterialButton.HighEmphasis = true;
            this.manageClientMaterialButton.Icon = null;
            this.manageClientMaterialButton.Location = new System.Drawing.Point(11, 125);
            this.manageClientMaterialButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.manageClientMaterialButton.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.manageClientMaterialButton.Name = "manageClientMaterialButton";
            this.manageClientMaterialButton.Size = new System.Drawing.Size(438, 36);
            this.manageClientMaterialButton.TabIndex = 16;
            this.manageClientMaterialButton.Text = "Manage settings";
            this.manageClientMaterialButton.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.manageClientMaterialButton.UseAccentColor = false;
            this.manageClientMaterialButton.UseVisualStyleBackColor = true;
            this.manageClientMaterialButton.Click += new System.EventHandler(this.manageClientMaterialButton_Click);
            // 
            // checkCLientUpdatesMaterialButton
            // 
            this.checkCLientUpdatesMaterialButton.AutoSize = false;
            this.checkCLientUpdatesMaterialButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.checkCLientUpdatesMaterialButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkCLientUpdatesMaterialButton.Depth = 0;
            this.checkCLientUpdatesMaterialButton.DrawShadows = true;
            this.checkCLientUpdatesMaterialButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkCLientUpdatesMaterialButton.HighEmphasis = true;
            this.checkCLientUpdatesMaterialButton.Icon = null;
            this.checkCLientUpdatesMaterialButton.ImageKey = "(none)";
            this.checkCLientUpdatesMaterialButton.Location = new System.Drawing.Point(11, 173);
            this.checkCLientUpdatesMaterialButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.checkCLientUpdatesMaterialButton.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.checkCLientUpdatesMaterialButton.Name = "checkCLientUpdatesMaterialButton";
            this.checkCLientUpdatesMaterialButton.Size = new System.Drawing.Size(438, 36);
            this.checkCLientUpdatesMaterialButton.TabIndex = 15;
            this.checkCLientUpdatesMaterialButton.Text = "Check for updates";
            this.checkCLientUpdatesMaterialButton.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.checkCLientUpdatesMaterialButton.UseAccentColor = true;
            this.checkCLientUpdatesMaterialButton.UseVisualStyleBackColor = true;
            this.checkCLientUpdatesMaterialButton.Click += new System.EventHandler(this.checkCLientUpdatesMaterialButton_Click);
            // 
            // clientInstalledMaterialLabel
            // 
            this.clientInstalledMaterialLabel.AutoSize = true;
            this.clientInstalledMaterialLabel.Depth = 0;
            this.clientInstalledMaterialLabel.Font = new System.Drawing.Font("Roboto", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.clientInstalledMaterialLabel.FontType = MaterialSkin2DotNet.MaterialSkinManager.fontType.Body2;
            this.clientInstalledMaterialLabel.ForeColor = System.Drawing.Color.Green;
            this.clientInstalledMaterialLabel.Location = new System.Drawing.Point(12, 38);
            this.clientInstalledMaterialLabel.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.clientInstalledMaterialLabel.Name = "clientInstalledMaterialLabel";
            this.clientInstalledMaterialLabel.Size = new System.Drawing.Size(188, 17);
            this.clientInstalledMaterialLabel.TabIndex = 14;
            this.clientInstalledMaterialLabel.Text = "ValheimPlus installed on client";
            this.clientInstalledMaterialLabel.UseAccent = true;
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.FontType = MaterialSkin2DotNet.MaterialSkinManager.fontType.H5;
            this.materialLabel1.Location = new System.Drawing.Point(11, 6);
            this.materialLabel1.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(129, 29);
            this.materialLabel1.TabIndex = 13;
            this.materialLabel1.Text = "Game client";
            // 
            // installClientMaterialButton
            // 
            this.installClientMaterialButton.AutoSize = false;
            this.installClientMaterialButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.installClientMaterialButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.installClientMaterialButton.Depth = 0;
            this.installClientMaterialButton.DrawShadows = true;
            this.installClientMaterialButton.HighEmphasis = true;
            this.installClientMaterialButton.Icon = null;
            this.installClientMaterialButton.Location = new System.Drawing.Point(11, 77);
            this.installClientMaterialButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.installClientMaterialButton.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.installClientMaterialButton.Name = "installClientMaterialButton";
            this.installClientMaterialButton.Size = new System.Drawing.Size(438, 36);
            this.installClientMaterialButton.TabIndex = 12;
            this.installClientMaterialButton.Text = "Install ValheimPlus on client";
            this.installClientMaterialButton.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.installClientMaterialButton.UseAccentColor = false;
            this.installClientMaterialButton.UseVisualStyleBackColor = true;
            this.installClientMaterialButton.Click += new System.EventHandler(this.installClientMaterialButton_Click);
            // 
            // clientMaterialCard
            // 
            this.clientMaterialCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.clientMaterialCard.Controls.Add(this.installClientUpdateMaterialButton);
            this.clientMaterialCard.Controls.Add(this.materialLabel1);
            this.clientMaterialCard.Controls.Add(this.manageClientMaterialButton);
            this.clientMaterialCard.Controls.Add(this.installClientMaterialButton);
            this.clientMaterialCard.Controls.Add(this.checkCLientUpdatesMaterialButton);
            this.clientMaterialCard.Controls.Add(this.clientInstalledMaterialLabel);
            this.clientMaterialCard.Depth = 0;
            this.clientMaterialCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.clientMaterialCard.Location = new System.Drawing.Point(12, 78);
            this.clientMaterialCard.Margin = new System.Windows.Forms.Padding(14);
            this.clientMaterialCard.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.clientMaterialCard.Name = "clientMaterialCard";
            this.clientMaterialCard.Padding = new System.Windows.Forms.Padding(14);
            this.clientMaterialCard.Size = new System.Drawing.Size(460, 282);
            this.clientMaterialCard.TabIndex = 14;
            // 
            // serverMaterialCard
            // 
            this.serverMaterialCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.serverMaterialCard.Controls.Add(this.installServerUpdateMaterialButton);
            this.serverMaterialCard.Controls.Add(this.materialLabel2);
            this.serverMaterialCard.Controls.Add(this.manageServerMaterialButton);
            this.serverMaterialCard.Controls.Add(this.serverInstalledMaterialLabel);
            this.serverMaterialCard.Controls.Add(this.installServerMaterialButton);
            this.serverMaterialCard.Controls.Add(this.checkServerUpdatesMaterialButton);
            this.serverMaterialCard.Depth = 0;
            this.serverMaterialCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.serverMaterialCard.Location = new System.Drawing.Point(12, 378);
            this.serverMaterialCard.Margin = new System.Windows.Forms.Padding(14);
            this.serverMaterialCard.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.serverMaterialCard.Name = "serverMaterialCard";
            this.serverMaterialCard.Padding = new System.Windows.Forms.Padding(14);
            this.serverMaterialCard.Size = new System.Drawing.Size(460, 279);
            this.serverMaterialCard.TabIndex = 15;
            // 
            // installServerUpdateMaterialButton
            // 
            this.installServerUpdateMaterialButton.AutoSize = false;
            this.installServerUpdateMaterialButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.installServerUpdateMaterialButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.installServerUpdateMaterialButton.Depth = 0;
            this.installServerUpdateMaterialButton.DrawShadows = true;
            this.installServerUpdateMaterialButton.Enabled = false;
            this.installServerUpdateMaterialButton.HighEmphasis = true;
            this.installServerUpdateMaterialButton.Icon = null;
            this.installServerUpdateMaterialButton.Location = new System.Drawing.Point(11, 220);
            this.installServerUpdateMaterialButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.installServerUpdateMaterialButton.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.installServerUpdateMaterialButton.Name = "installServerUpdateMaterialButton";
            this.installServerUpdateMaterialButton.Size = new System.Drawing.Size(438, 36);
            this.installServerUpdateMaterialButton.TabIndex = 21;
            this.installServerUpdateMaterialButton.Text = "Install update";
            this.installServerUpdateMaterialButton.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.installServerUpdateMaterialButton.UseAccentColor = false;
            this.installServerUpdateMaterialButton.UseVisualStyleBackColor = true;
            this.installServerUpdateMaterialButton.Click += new System.EventHandler(this.installServerUpdateMaterialButton_Click);
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel2.FontType = MaterialSkin2DotNet.MaterialSkinManager.fontType.H5;
            this.materialLabel2.Location = new System.Drawing.Point(12, 6);
            this.materialLabel2.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(134, 29);
            this.materialLabel2.TabIndex = 18;
            this.materialLabel2.Text = "Server client";
            // 
            // manageServerMaterialButton
            // 
            this.manageServerMaterialButton.AutoSize = false;
            this.manageServerMaterialButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.manageServerMaterialButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.manageServerMaterialButton.Depth = 0;
            this.manageServerMaterialButton.DrawShadows = true;
            this.manageServerMaterialButton.HighEmphasis = true;
            this.manageServerMaterialButton.Icon = null;
            this.manageServerMaterialButton.Location = new System.Drawing.Point(11, 124);
            this.manageServerMaterialButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.manageServerMaterialButton.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.manageServerMaterialButton.Name = "manageServerMaterialButton";
            this.manageServerMaterialButton.Size = new System.Drawing.Size(438, 36);
            this.manageServerMaterialButton.TabIndex = 20;
            this.manageServerMaterialButton.Text = "Manage settings";
            this.manageServerMaterialButton.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.manageServerMaterialButton.UseAccentColor = false;
            this.manageServerMaterialButton.UseVisualStyleBackColor = true;
            this.manageServerMaterialButton.Click += new System.EventHandler(this.manageServerMaterialButton_Click);
            // 
            // serverInstalledMaterialLabel
            // 
            this.serverInstalledMaterialLabel.AutoSize = true;
            this.serverInstalledMaterialLabel.Depth = 0;
            this.serverInstalledMaterialLabel.Font = new System.Drawing.Font("Roboto", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.serverInstalledMaterialLabel.FontType = MaterialSkin2DotNet.MaterialSkinManager.fontType.Body2;
            this.serverInstalledMaterialLabel.ForeColor = System.Drawing.Color.Green;
            this.serverInstalledMaterialLabel.Location = new System.Drawing.Point(13, 38);
            this.serverInstalledMaterialLabel.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.serverInstalledMaterialLabel.Name = "serverInstalledMaterialLabel";
            this.serverInstalledMaterialLabel.Size = new System.Drawing.Size(193, 17);
            this.serverInstalledMaterialLabel.TabIndex = 19;
            this.serverInstalledMaterialLabel.Text = "ValheimPlus installed on server";
            this.serverInstalledMaterialLabel.UseAccent = true;
            // 
            // installServerMaterialButton
            // 
            this.installServerMaterialButton.AutoSize = false;
            this.installServerMaterialButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.installServerMaterialButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.installServerMaterialButton.Depth = 0;
            this.installServerMaterialButton.DrawShadows = true;
            this.installServerMaterialButton.HighEmphasis = true;
            this.installServerMaterialButton.Icon = null;
            this.installServerMaterialButton.Location = new System.Drawing.Point(11, 76);
            this.installServerMaterialButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.installServerMaterialButton.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.installServerMaterialButton.Name = "installServerMaterialButton";
            this.installServerMaterialButton.Size = new System.Drawing.Size(438, 36);
            this.installServerMaterialButton.TabIndex = 18;
            this.installServerMaterialButton.Text = "Install ValheimPlus on server";
            this.installServerMaterialButton.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.installServerMaterialButton.UseAccentColor = false;
            this.installServerMaterialButton.UseVisualStyleBackColor = true;
            this.installServerMaterialButton.Click += new System.EventHandler(this.installServerMaterialButton_Click);
            // 
            // checkServerUpdatesMaterialButton
            // 
            this.checkServerUpdatesMaterialButton.AutoSize = false;
            this.checkServerUpdatesMaterialButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.checkServerUpdatesMaterialButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkServerUpdatesMaterialButton.Depth = 0;
            this.checkServerUpdatesMaterialButton.DrawShadows = true;
            this.checkServerUpdatesMaterialButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkServerUpdatesMaterialButton.HighEmphasis = true;
            this.checkServerUpdatesMaterialButton.Icon = null;
            this.checkServerUpdatesMaterialButton.ImageKey = "(none)";
            this.checkServerUpdatesMaterialButton.Location = new System.Drawing.Point(11, 172);
            this.checkServerUpdatesMaterialButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.checkServerUpdatesMaterialButton.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.checkServerUpdatesMaterialButton.Name = "checkServerUpdatesMaterialButton";
            this.checkServerUpdatesMaterialButton.Size = new System.Drawing.Size(438, 36);
            this.checkServerUpdatesMaterialButton.TabIndex = 19;
            this.checkServerUpdatesMaterialButton.Text = "Check for updates";
            this.checkServerUpdatesMaterialButton.Type = MaterialSkin2DotNet.Controls.MaterialButton.MaterialButtonType.Contained;
            this.checkServerUpdatesMaterialButton.UseAccentColor = true;
            this.checkServerUpdatesMaterialButton.UseVisualStyleBackColor = true;
            this.checkServerUpdatesMaterialButton.Click += new System.EventHandler(this.checkServerUpdatesMaterialButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(484, 688);
            this.Controls.Add(this.serverMaterialCard);
            this.Controls.Add(this.clientMaterialCard);
            this.Controls.Add(this.statusLabel);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ValheimPlus Manager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.clientMaterialCard.ResumeLayout(false);
            this.clientMaterialCard.PerformLayout();
            this.serverMaterialCard.ResumeLayout(false);
            this.serverMaterialCard.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label statusLabel;
        private MaterialSkin2DotNet.Controls.MaterialButton installClientMaterialButton;
        private MaterialSkin2DotNet.Controls.MaterialLabel materialLabel1;
        private MaterialSkin2DotNet.Controls.MaterialLabel clientInstalledMaterialLabel;
        private MaterialSkin2DotNet.Controls.MaterialButton checkCLientUpdatesMaterialButton;
        private MaterialSkin2DotNet.Controls.MaterialButton manageClientMaterialButton;
        private MaterialSkin2DotNet.Controls.MaterialCard clientMaterialCard;
        private MaterialSkin2DotNet.Controls.MaterialCard serverMaterialCard;
        private MaterialSkin2DotNet.Controls.MaterialLabel materialLabel2;
        private MaterialSkin2DotNet.Controls.MaterialLabel serverInstalledMaterialLabel;
        private MaterialSkin2DotNet.Controls.MaterialButton installServerUpdateMaterialButton;
        private MaterialSkin2DotNet.Controls.MaterialButton manageServerMaterialButton;
        private MaterialSkin2DotNet.Controls.MaterialButton installServerMaterialButton;
        private MaterialSkin2DotNet.Controls.MaterialButton checkServerUpdatesMaterialButton;
        private MaterialSkin2DotNet.Controls.MaterialButton installClientUpdateMaterialButton;
    }
}

