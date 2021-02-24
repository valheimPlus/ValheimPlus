
namespace ValheimPlusManager
{
    partial class ConfigEditor
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
            this.configCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.advancedBuildingModeTab = new System.Windows.Forms.TabPage();
            this.exitAdvancedBuildingModeTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.enterAdvancedBuildingModeTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.advancedEditingTab = new System.Windows.Forms.TabPage();
            this.confirmPlacementOfAdvancedEditingModeTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.abortAndExitAdvancedEditingModeTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.resetAdvancedEditingModeTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.enterAdvancedEditingModeTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.beehiveTab = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.maximumHoneyPerBeehiveNumeric = new System.Windows.Forms.NumericUpDown();
            this.honeyProductionSpeedNumeric = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.buildingTab = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.cameraTab = new System.Windows.Forms.TabPage();
            this.experienceTab = new System.Windows.Forms.TabPage();
            this.fermenterTab = new System.Windows.Forms.TabPage();
            this.fireplaceTab = new System.Windows.Forms.TabPage();
            this.foodTab = new System.Windows.Forms.TabPage();
            this.furnaceTab = new System.Windows.Forms.TabPage();
            this.gameTab = new System.Windows.Forms.TabPage();
            this.hotkeysTab = new System.Windows.Forms.TabPage();
            this.hudTab = new System.Windows.Forms.TabPage();
            this.itemsTab = new System.Windows.Forms.TabPage();
            this.kilnTab = new System.Windows.Forms.TabPage();
            this.mapTab = new System.Windows.Forms.TabPage();
            this.playerTab = new System.Windows.Forms.TabPage();
            this.serverTab = new System.Windows.Forms.TabPage();
            this.staminaTab = new System.Windows.Forms.TabPage();
            this.staminaUsageTab = new System.Windows.Forms.TabPage();
            this.structuralIntegrityTab = new System.Windows.Forms.TabPage();
            this.timeTab = new System.Windows.Forms.TabPage();
            this.wagonTab = new System.Windows.Forms.TabPage();
            this.wardTab = new System.Windows.Forms.TabPage();
            this.workbenchTab = new System.Windows.Forms.TabPage();
            this.saveConfigButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.saveChangesLabel = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.advancedBuildingModeTab.SuspendLayout();
            this.advancedEditingTab.SuspendLayout();
            this.beehiveTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maximumHoneyPerBeehiveNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.honeyProductionSpeedNumeric)).BeginInit();
            this.buildingTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // configCheckedListBox
            // 
            this.configCheckedListBox.CheckOnClick = true;
            this.configCheckedListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.configCheckedListBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.configCheckedListBox.FormattingEnabled = true;
            this.configCheckedListBox.Items.AddRange(new object[] {
            "Enable advanced building mode",
            "Enable advanced editing mode",
            "Enable beehive settings",
            "Enable building settings",
            "Enable camera settings",
            "Enable experience settings",
            "Enable fermenter settings",
            "Enable fireplace settings",
            "Enable food settings",
            "Enable furnace settings",
            "Enable game settings",
            "Enable hotkeys settings",
            "Enable hud settings",
            "Enable items settings",
            "Enable kiln settings",
            "Enable map settings",
            "Enable player settings",
            "Enable server settings",
            "Enable stamina settings",
            "Enable stamina usage settings",
            "Enable structural integrity settings",
            "Enable time settings",
            "Enable wagon settings",
            "Enable ward settings",
            "Enable workbench settings"});
            this.configCheckedListBox.Location = new System.Drawing.Point(0, 0);
            this.configCheckedListBox.Name = "configCheckedListBox";
            this.configCheckedListBox.Size = new System.Drawing.Size(259, 647);
            this.configCheckedListBox.Sorted = true;
            this.configCheckedListBox.TabIndex = 0;
            this.configCheckedListBox.ThreeDCheckBoxes = true;
            this.configCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.configCheckedListBox_ItemCheck);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.advancedBuildingModeTab);
            this.tabControl1.Controls.Add(this.advancedEditingTab);
            this.tabControl1.Controls.Add(this.beehiveTab);
            this.tabControl1.Controls.Add(this.buildingTab);
            this.tabControl1.Controls.Add(this.cameraTab);
            this.tabControl1.Controls.Add(this.experienceTab);
            this.tabControl1.Controls.Add(this.fermenterTab);
            this.tabControl1.Controls.Add(this.fireplaceTab);
            this.tabControl1.Controls.Add(this.foodTab);
            this.tabControl1.Controls.Add(this.furnaceTab);
            this.tabControl1.Controls.Add(this.gameTab);
            this.tabControl1.Controls.Add(this.hotkeysTab);
            this.tabControl1.Controls.Add(this.hudTab);
            this.tabControl1.Controls.Add(this.itemsTab);
            this.tabControl1.Controls.Add(this.kilnTab);
            this.tabControl1.Controls.Add(this.mapTab);
            this.tabControl1.Controls.Add(this.playerTab);
            this.tabControl1.Controls.Add(this.serverTab);
            this.tabControl1.Controls.Add(this.staminaTab);
            this.tabControl1.Controls.Add(this.staminaUsageTab);
            this.tabControl1.Controls.Add(this.structuralIntegrityTab);
            this.tabControl1.Controls.Add(this.timeTab);
            this.tabControl1.Controls.Add(this.wagonTab);
            this.tabControl1.Controls.Add(this.wardTab);
            this.tabControl1.Controls.Add(this.workbenchTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.tabControl1.Location = new System.Drawing.Point(259, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(862, 585);
            this.tabControl1.TabIndex = 1;
            // 
            // advancedBuildingModeTab
            // 
            this.advancedBuildingModeTab.Controls.Add(this.exitAdvancedBuildingModeTextBox);
            this.advancedBuildingModeTab.Controls.Add(this.label3);
            this.advancedBuildingModeTab.Controls.Add(this.label2);
            this.advancedBuildingModeTab.Controls.Add(this.enterAdvancedBuildingModeTextBox);
            this.advancedBuildingModeTab.Controls.Add(this.label1);
            this.advancedBuildingModeTab.Location = new System.Drawing.Point(4, 44);
            this.advancedBuildingModeTab.Name = "advancedBuildingModeTab";
            this.advancedBuildingModeTab.Padding = new System.Windows.Forms.Padding(3);
            this.advancedBuildingModeTab.Size = new System.Drawing.Size(854, 537);
            this.advancedBuildingModeTab.TabIndex = 0;
            this.advancedBuildingModeTab.Text = "Advanced building mode";
            this.advancedBuildingModeTab.UseVisualStyleBackColor = true;
            // 
            // exitAdvancedBuildingModeTextBox
            // 
            this.exitAdvancedBuildingModeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exitAdvancedBuildingModeTextBox.Location = new System.Drawing.Point(6, 127);
            this.exitAdvancedBuildingModeTextBox.Name = "exitAdvancedBuildingModeTextBox";
            this.exitAdvancedBuildingModeTextBox.Size = new System.Drawing.Size(367, 23);
            this.exitAdvancedBuildingModeTextBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(4, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(358, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Exit the advanced building mode with this key when building";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(4, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(368, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Enter the advanced building mode with this key when building";
            // 
            // enterAdvancedBuildingModeTextBox
            // 
            this.enterAdvancedBuildingModeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.enterAdvancedBuildingModeTextBox.Location = new System.Drawing.Point(6, 70);
            this.enterAdvancedBuildingModeTextBox.Name = "enterAdvancedBuildingModeTextBox";
            this.enterAdvancedBuildingModeTextBox.Size = new System.Drawing.Size(367, 23);
            this.enterAdvancedBuildingModeTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Advanced building mode";
            // 
            // advancedEditingTab
            // 
            this.advancedEditingTab.Controls.Add(this.confirmPlacementOfAdvancedEditingModeTextBox);
            this.advancedEditingTab.Controls.Add(this.label10);
            this.advancedEditingTab.Controls.Add(this.abortAndExitAdvancedEditingModeTextBox);
            this.advancedEditingTab.Controls.Add(this.label8);
            this.advancedEditingTab.Controls.Add(this.resetAdvancedEditingModeTextBox);
            this.advancedEditingTab.Controls.Add(this.label4);
            this.advancedEditingTab.Controls.Add(this.label5);
            this.advancedEditingTab.Controls.Add(this.enterAdvancedEditingModeTextBox);
            this.advancedEditingTab.Controls.Add(this.label6);
            this.advancedEditingTab.Location = new System.Drawing.Point(4, 44);
            this.advancedEditingTab.Name = "advancedEditingTab";
            this.advancedEditingTab.Padding = new System.Windows.Forms.Padding(3);
            this.advancedEditingTab.Size = new System.Drawing.Size(854, 537);
            this.advancedEditingTab.TabIndex = 1;
            this.advancedEditingTab.Text = "Advanced editing mode";
            this.advancedEditingTab.UseVisualStyleBackColor = true;
            // 
            // confirmPlacementOfAdvancedEditingModeTextBox
            // 
            this.confirmPlacementOfAdvancedEditingModeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.confirmPlacementOfAdvancedEditingModeTextBox.Location = new System.Drawing.Point(6, 238);
            this.confirmPlacementOfAdvancedEditingModeTextBox.Name = "confirmPlacementOfAdvancedEditingModeTextBox";
            this.confirmPlacementOfAdvancedEditingModeTextBox.Size = new System.Drawing.Size(385, 23);
            this.confirmPlacementOfAdvancedEditingModeTextBox.TabIndex = 15;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label10.Location = new System.Drawing.Point(4, 218);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(290, 17);
            this.label10.TabIndex = 14;
            this.label10.Text = "Confirm the placement of the object and place it";
            // 
            // abortAndExitAdvancedEditingModeTextBox
            // 
            this.abortAndExitAdvancedEditingModeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.abortAndExitAdvancedEditingModeTextBox.Location = new System.Drawing.Point(6, 182);
            this.abortAndExitAdvancedEditingModeTextBox.Name = "abortAndExitAdvancedEditingModeTextBox";
            this.abortAndExitAdvancedEditingModeTextBox.Size = new System.Drawing.Size(385, 23);
            this.abortAndExitAdvancedEditingModeTextBox.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(4, 162);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(387, 17);
            this.label8.TabIndex = 12;
            this.label8.Text = "Exit the advanced editing mode with this key and reset the object";
            // 
            // resetAdvancedEditingModeTextBox
            // 
            this.resetAdvancedEditingModeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resetAdvancedEditingModeTextBox.Location = new System.Drawing.Point(6, 127);
            this.resetAdvancedEditingModeTextBox.Name = "resetAdvancedEditingModeTextBox";
            this.resetAdvancedEditingModeTextBox.Size = new System.Drawing.Size(385, 23);
            this.resetAdvancedEditingModeTextBox.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(4, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(310, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Reset the object to its original position and rotation";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(4, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(276, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Enter the advanced editing mode with this key";
            // 
            // enterAdvancedEditingModeTextBox
            // 
            this.enterAdvancedEditingModeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.enterAdvancedEditingModeTextBox.Location = new System.Drawing.Point(6, 70);
            this.enterAdvancedEditingModeTextBox.Name = "enterAdvancedEditingModeTextBox";
            this.enterAdvancedEditingModeTextBox.Size = new System.Drawing.Size(385, 23);
            this.enterAdvancedEditingModeTextBox.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(4, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(251, 30);
            this.label6.TabIndex = 5;
            this.label6.Text = "Advanced editing mode";
            // 
            // beehiveTab
            // 
            this.beehiveTab.Controls.Add(this.label12);
            this.beehiveTab.Controls.Add(this.label11);
            this.beehiveTab.Controls.Add(this.maximumHoneyPerBeehiveNumeric);
            this.beehiveTab.Controls.Add(this.honeyProductionSpeedNumeric);
            this.beehiveTab.Controls.Add(this.label7);
            this.beehiveTab.Location = new System.Drawing.Point(4, 44);
            this.beehiveTab.Name = "beehiveTab";
            this.beehiveTab.Size = new System.Drawing.Size(854, 537);
            this.beehiveTab.TabIndex = 2;
            this.beehiveTab.Text = "Beehive";
            this.beehiveTab.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label12.Location = new System.Drawing.Point(4, 106);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(237, 17);
            this.label12.TabIndex = 8;
            this.label12.Text = "Maximum amount of honey in beehives";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label11.Location = new System.Drawing.Point(4, 50);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(308, 17);
            this.label11.TabIndex = 8;
            this.label11.Text = "Speed at which the bees produce honey in seconds";
            // 
            // maximumHoneyPerBeehiveNumeric
            // 
            this.maximumHoneyPerBeehiveNumeric.Location = new System.Drawing.Point(6, 126);
            this.maximumHoneyPerBeehiveNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.maximumHoneyPerBeehiveNumeric.Name = "maximumHoneyPerBeehiveNumeric";
            this.maximumHoneyPerBeehiveNumeric.Size = new System.Drawing.Size(306, 23);
            this.maximumHoneyPerBeehiveNumeric.TabIndex = 7;
            // 
            // honeyProductionSpeedNumeric
            // 
            this.honeyProductionSpeedNumeric.Location = new System.Drawing.Point(6, 70);
            this.honeyProductionSpeedNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.honeyProductionSpeedNumeric.Name = "honeyProductionSpeedNumeric";
            this.honeyProductionSpeedNumeric.Size = new System.Drawing.Size(306, 23);
            this.honeyProductionSpeedNumeric.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(4, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 30);
            this.label7.TabIndex = 6;
            this.label7.Text = "Beehive";
            // 
            // buildingTab
            // 
            this.buildingTab.Controls.Add(this.label13);
            this.buildingTab.Location = new System.Drawing.Point(4, 44);
            this.buildingTab.Name = "buildingTab";
            this.buildingTab.Size = new System.Drawing.Size(854, 537);
            this.buildingTab.TabIndex = 3;
            this.buildingTab.Text = "Building";
            this.buildingTab.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label13.Location = new System.Drawing.Point(4, 4);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(96, 30);
            this.label13.TabIndex = 7;
            this.label13.Text = "Building";
            // 
            // cameraTab
            // 
            this.cameraTab.Location = new System.Drawing.Point(4, 44);
            this.cameraTab.Name = "cameraTab";
            this.cameraTab.Size = new System.Drawing.Size(854, 537);
            this.cameraTab.TabIndex = 4;
            this.cameraTab.Text = "Camera";
            this.cameraTab.UseVisualStyleBackColor = true;
            // 
            // experienceTab
            // 
            this.experienceTab.Location = new System.Drawing.Point(4, 44);
            this.experienceTab.Name = "experienceTab";
            this.experienceTab.Size = new System.Drawing.Size(854, 537);
            this.experienceTab.TabIndex = 5;
            this.experienceTab.Text = "Experience";
            this.experienceTab.UseVisualStyleBackColor = true;
            // 
            // fermenterTab
            // 
            this.fermenterTab.Location = new System.Drawing.Point(4, 44);
            this.fermenterTab.Name = "fermenterTab";
            this.fermenterTab.Size = new System.Drawing.Size(854, 537);
            this.fermenterTab.TabIndex = 6;
            this.fermenterTab.Text = "Fermenter";
            this.fermenterTab.UseVisualStyleBackColor = true;
            // 
            // fireplaceTab
            // 
            this.fireplaceTab.Location = new System.Drawing.Point(4, 44);
            this.fireplaceTab.Name = "fireplaceTab";
            this.fireplaceTab.Size = new System.Drawing.Size(854, 537);
            this.fireplaceTab.TabIndex = 7;
            this.fireplaceTab.Text = "Fireplace";
            this.fireplaceTab.UseVisualStyleBackColor = true;
            // 
            // foodTab
            // 
            this.foodTab.Location = new System.Drawing.Point(4, 44);
            this.foodTab.Name = "foodTab";
            this.foodTab.Size = new System.Drawing.Size(854, 537);
            this.foodTab.TabIndex = 8;
            this.foodTab.Text = "Food";
            this.foodTab.UseVisualStyleBackColor = true;
            // 
            // furnaceTab
            // 
            this.furnaceTab.Location = new System.Drawing.Point(4, 44);
            this.furnaceTab.Name = "furnaceTab";
            this.furnaceTab.Size = new System.Drawing.Size(854, 537);
            this.furnaceTab.TabIndex = 9;
            this.furnaceTab.Text = "Furnace";
            this.furnaceTab.UseVisualStyleBackColor = true;
            // 
            // gameTab
            // 
            this.gameTab.Location = new System.Drawing.Point(4, 44);
            this.gameTab.Name = "gameTab";
            this.gameTab.Size = new System.Drawing.Size(854, 537);
            this.gameTab.TabIndex = 10;
            this.gameTab.Text = "Game";
            this.gameTab.UseVisualStyleBackColor = true;
            // 
            // hotkeysTab
            // 
            this.hotkeysTab.Location = new System.Drawing.Point(4, 44);
            this.hotkeysTab.Name = "hotkeysTab";
            this.hotkeysTab.Size = new System.Drawing.Size(854, 537);
            this.hotkeysTab.TabIndex = 11;
            this.hotkeysTab.Text = "Hotkeys";
            this.hotkeysTab.UseVisualStyleBackColor = true;
            // 
            // hudTab
            // 
            this.hudTab.Location = new System.Drawing.Point(4, 44);
            this.hudTab.Name = "hudTab";
            this.hudTab.Size = new System.Drawing.Size(854, 537);
            this.hudTab.TabIndex = 12;
            this.hudTab.Text = "Hud";
            this.hudTab.UseVisualStyleBackColor = true;
            // 
            // itemsTab
            // 
            this.itemsTab.Location = new System.Drawing.Point(4, 44);
            this.itemsTab.Name = "itemsTab";
            this.itemsTab.Size = new System.Drawing.Size(854, 537);
            this.itemsTab.TabIndex = 13;
            this.itemsTab.Text = "Items";
            this.itemsTab.UseVisualStyleBackColor = true;
            // 
            // kilnTab
            // 
            this.kilnTab.Location = new System.Drawing.Point(4, 44);
            this.kilnTab.Name = "kilnTab";
            this.kilnTab.Size = new System.Drawing.Size(854, 537);
            this.kilnTab.TabIndex = 14;
            this.kilnTab.Text = "Kiln";
            this.kilnTab.UseVisualStyleBackColor = true;
            // 
            // mapTab
            // 
            this.mapTab.Location = new System.Drawing.Point(4, 44);
            this.mapTab.Name = "mapTab";
            this.mapTab.Size = new System.Drawing.Size(854, 537);
            this.mapTab.TabIndex = 15;
            this.mapTab.Text = "Map";
            this.mapTab.UseVisualStyleBackColor = true;
            // 
            // playerTab
            // 
            this.playerTab.Location = new System.Drawing.Point(4, 44);
            this.playerTab.Name = "playerTab";
            this.playerTab.Size = new System.Drawing.Size(854, 537);
            this.playerTab.TabIndex = 16;
            this.playerTab.Text = "Player";
            this.playerTab.UseVisualStyleBackColor = true;
            // 
            // serverTab
            // 
            this.serverTab.Location = new System.Drawing.Point(4, 44);
            this.serverTab.Name = "serverTab";
            this.serverTab.Size = new System.Drawing.Size(854, 537);
            this.serverTab.TabIndex = 17;
            this.serverTab.Text = "Server";
            this.serverTab.UseVisualStyleBackColor = true;
            // 
            // staminaTab
            // 
            this.staminaTab.Location = new System.Drawing.Point(4, 44);
            this.staminaTab.Name = "staminaTab";
            this.staminaTab.Size = new System.Drawing.Size(854, 537);
            this.staminaTab.TabIndex = 18;
            this.staminaTab.Text = "Stamina";
            this.staminaTab.UseVisualStyleBackColor = true;
            // 
            // staminaUsageTab
            // 
            this.staminaUsageTab.Location = new System.Drawing.Point(4, 44);
            this.staminaUsageTab.Name = "staminaUsageTab";
            this.staminaUsageTab.Size = new System.Drawing.Size(854, 537);
            this.staminaUsageTab.TabIndex = 19;
            this.staminaUsageTab.Text = "Stamina usage";
            this.staminaUsageTab.UseVisualStyleBackColor = true;
            // 
            // structuralIntegrityTab
            // 
            this.structuralIntegrityTab.Location = new System.Drawing.Point(4, 44);
            this.structuralIntegrityTab.Name = "structuralIntegrityTab";
            this.structuralIntegrityTab.Size = new System.Drawing.Size(854, 537);
            this.structuralIntegrityTab.TabIndex = 20;
            this.structuralIntegrityTab.Text = "Structural integrity";
            this.structuralIntegrityTab.UseVisualStyleBackColor = true;
            // 
            // timeTab
            // 
            this.timeTab.Location = new System.Drawing.Point(4, 44);
            this.timeTab.Name = "timeTab";
            this.timeTab.Size = new System.Drawing.Size(854, 537);
            this.timeTab.TabIndex = 21;
            this.timeTab.Text = "Time";
            this.timeTab.UseVisualStyleBackColor = true;
            // 
            // wagonTab
            // 
            this.wagonTab.Location = new System.Drawing.Point(4, 44);
            this.wagonTab.Name = "wagonTab";
            this.wagonTab.Size = new System.Drawing.Size(854, 537);
            this.wagonTab.TabIndex = 22;
            this.wagonTab.Text = "Wagon";
            this.wagonTab.UseVisualStyleBackColor = true;
            // 
            // wardTab
            // 
            this.wardTab.Location = new System.Drawing.Point(4, 44);
            this.wardTab.Name = "wardTab";
            this.wardTab.Size = new System.Drawing.Size(854, 537);
            this.wardTab.TabIndex = 23;
            this.wardTab.Text = "Ward";
            this.wardTab.UseVisualStyleBackColor = true;
            // 
            // workbenchTab
            // 
            this.workbenchTab.Location = new System.Drawing.Point(4, 44);
            this.workbenchTab.Name = "workbenchTab";
            this.workbenchTab.Size = new System.Drawing.Size(854, 537);
            this.workbenchTab.TabIndex = 24;
            this.workbenchTab.Text = "Workbench";
            this.workbenchTab.UseVisualStyleBackColor = true;
            // 
            // saveConfigButton
            // 
            this.saveConfigButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveConfigButton.Location = new System.Drawing.Point(269, 592);
            this.saveConfigButton.Name = "saveConfigButton";
            this.saveConfigButton.Size = new System.Drawing.Size(151, 45);
            this.saveConfigButton.TabIndex = 2;
            this.saveConfigButton.Text = "Save all changes";
            this.saveConfigButton.UseVisualStyleBackColor = true;
            this.saveConfigButton.Click += new System.EventHandler(this.saveConfigButton_Click);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label9.Location = new System.Drawing.Point(202, 247);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(310, 17);
            this.label9.TabIndex = 10;
            this.label9.Text = "Reset the object to its original position and rotation";
            // 
            // textBox5
            // 
            this.textBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox5.Location = new System.Drawing.Point(204, 267);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(367, 23);
            this.textBox5.TabIndex = 11;
            // 
            // saveChangesLabel
            // 
            this.saveChangesLabel.AutoSize = true;
            this.saveChangesLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.saveChangesLabel.ForeColor = System.Drawing.Color.Green;
            this.saveChangesLabel.Location = new System.Drawing.Point(427, 604);
            this.saveChangesLabel.Name = "saveChangesLabel";
            this.saveChangesLabel.Size = new System.Drawing.Size(0, 20);
            this.saveChangesLabel.TabIndex = 3;
            // 
            // ConfigEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 647);
            this.Controls.Add(this.saveChangesLabel);
            this.Controls.Add(this.saveConfigButton);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.configCheckedListBox);
            this.Name = "ConfigEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ValheimPlus Manager";
            this.Load += new System.EventHandler(this.ConfigEditor_Load);
            this.tabControl1.ResumeLayout(false);
            this.advancedBuildingModeTab.ResumeLayout(false);
            this.advancedBuildingModeTab.PerformLayout();
            this.advancedEditingTab.ResumeLayout(false);
            this.advancedEditingTab.PerformLayout();
            this.beehiveTab.ResumeLayout(false);
            this.beehiveTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maximumHoneyPerBeehiveNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.honeyProductionSpeedNumeric)).EndInit();
            this.buildingTab.ResumeLayout(false);
            this.buildingTab.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox configCheckedListBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage advancedBuildingModeTab;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage advancedEditingTab;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox enterAdvancedBuildingModeTextBox;
        private System.Windows.Forms.Button saveConfigButton;
        private System.Windows.Forms.TabPage beehiveTab;
        private System.Windows.Forms.TabPage buildingTab;
        private System.Windows.Forms.TabPage cameraTab;
        private System.Windows.Forms.TabPage experienceTab;
        private System.Windows.Forms.TabPage fermenterTab;
        private System.Windows.Forms.TabPage fireplaceTab;
        private System.Windows.Forms.TabPage foodTab;
        private System.Windows.Forms.TabPage furnaceTab;
        private System.Windows.Forms.TabPage gameTab;
        private System.Windows.Forms.TabPage hotkeysTab;
        private System.Windows.Forms.TabPage hudTab;
        private System.Windows.Forms.TabPage itemsTab;
        private System.Windows.Forms.TabPage kilnTab;
        private System.Windows.Forms.TabPage mapTab;
        private System.Windows.Forms.TabPage playerTab;
        private System.Windows.Forms.TabPage serverTab;
        private System.Windows.Forms.TabPage staminaTab;
        private System.Windows.Forms.TabPage staminaUsageTab;
        private System.Windows.Forms.TabPage structuralIntegrityTab;
        private System.Windows.Forms.TabPage timeTab;
        private System.Windows.Forms.TabPage wagonTab;
        private System.Windows.Forms.TabPage wardTab;
        private System.Windows.Forms.TabPage workbenchTab;
        private System.Windows.Forms.TextBox exitAdvancedBuildingModeTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox resetAdvancedEditingModeTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox enterAdvancedEditingModeTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox confirmPlacementOfAdvancedEditingModeTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox abortAndExitAdvancedEditingModeTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox ex;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown honeyProductionSpeedNumeric;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown maximumHoneyPerBeehiveNumeric;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label saveChangesLabel;
    }
}