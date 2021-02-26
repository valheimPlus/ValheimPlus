
namespace ValheimPlusManager
{
    partial class SplashScreen
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreen));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.materialLabel1 = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.materialLabel3 = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.materialLabel4 = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.materialLabel5 = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.materialLabel6 = new MaterialSkin2DotNet.Controls.MaterialLabel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(639, 257);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.FontType = MaterialSkin2DotNet.MaterialSkinManager.fontType.Body2;
            this.materialLabel1.Location = new System.Drawing.Point(550, 473);
            this.materialLabel1.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(83, 17);
            this.materialLabel1.TabIndex = 8;
            this.materialLabel1.Text = "Version 0.1.0";
            // 
            // materialLabel2
            // 
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel2.FontType = MaterialSkin2DotNet.MaterialSkinManager.fontType.H3;
            this.materialLabel2.Location = new System.Drawing.Point(0, 247);
            this.materialLabel2.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(639, 73);
            this.materialLabel2.TabIndex = 9;
            this.materialLabel2.Text = "Manager";
            this.materialLabel2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // materialLabel3
            // 
            this.materialLabel3.Depth = 0;
            this.materialLabel3.Font = new System.Drawing.Font("Roboto", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel3.FontType = MaterialSkin2DotNet.MaterialSkinManager.fontType.H5;
            this.materialLabel3.Location = new System.Drawing.Point(0, 320);
            this.materialLabel3.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.materialLabel3.Name = "materialLabel3";
            this.materialLabel3.Size = new System.Drawing.Size(639, 35);
            this.materialLabel3.TabIndex = 10;
            this.materialLabel3.Text = "ValheimPlus";
            this.materialLabel3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // materialLabel4
            // 
            this.materialLabel4.Depth = 0;
            this.materialLabel4.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel4.Location = new System.Drawing.Point(0, 355);
            this.materialLabel4.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.materialLabel4.Name = "materialLabel4";
            this.materialLabel4.Size = new System.Drawing.Size(639, 56);
            this.materialLabel4.TabIndex = 11;
            this.materialLabel4.Text = "Kevin \'nx#8830\' J, Greg \'Zedle\' G, Bruno Vasconcelos, GaelicGamer, TheTerrasque, " +
    "\r\nPaige \'radmint\' N, MrPurple6411#0415\r\n";
            this.materialLabel4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // materialLabel5
            // 
            this.materialLabel5.Depth = 0;
            this.materialLabel5.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel5.Location = new System.Drawing.Point(0, 446);
            this.materialLabel5.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.materialLabel5.Name = "materialLabel5";
            this.materialLabel5.Size = new System.Drawing.Size(639, 30);
            this.materialLabel5.TabIndex = 13;
            this.materialLabel5.Text = "Robert \'rr222cy\' Roos";
            this.materialLabel5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // materialLabel6
            // 
            this.materialLabel6.Depth = 0;
            this.materialLabel6.Font = new System.Drawing.Font("Roboto", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel6.FontType = MaterialSkin2DotNet.MaterialSkinManager.fontType.H5;
            this.materialLabel6.Location = new System.Drawing.Point(0, 411);
            this.materialLabel6.MouseState = MaterialSkin2DotNet.MouseState.HOVER;
            this.materialLabel6.Name = "materialLabel6";
            this.materialLabel6.Size = new System.Drawing.Size(639, 35);
            this.materialLabel6.TabIndex = 12;
            this.materialLabel6.Text = "Manager";
            this.materialLabel6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 495);
            this.progressBar1.MarqueeAnimationSpeed = 50;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(639, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 0;
            // 
            // SplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 518);
            this.ControlBox = false;
            this.Controls.Add(this.materialLabel5);
            this.Controls.Add(this.materialLabel6);
            this.Controls.Add(this.materialLabel4);
            this.Controls.Add(this.materialLabel3);
            this.Controls.Add(this.materialLabel2);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.progressBar1);
            this.Name = "SplashScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ValheimPlus Manager";
            this.Load += new System.EventHandler(this.SplashScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private MaterialSkin2DotNet.Controls.MaterialLabel materialLabel1;
        private MaterialSkin2DotNet.Controls.MaterialLabel materialLabel2;
        private MaterialSkin2DotNet.Controls.MaterialLabel materialLabel3;
        private MaterialSkin2DotNet.Controls.MaterialLabel materialLabel4;
        private MaterialSkin2DotNet.Controls.MaterialLabel materialLabel5;
        private MaterialSkin2DotNet.Controls.MaterialLabel materialLabel6;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}