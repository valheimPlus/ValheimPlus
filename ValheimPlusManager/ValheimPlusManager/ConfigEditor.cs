using System;
using System.Drawing;
using System.Windows.Forms;
using ValheimPlusManager.Models;
using ValheimPlusManager.SupportClasses;

namespace ValheimPlusManager
{
    public partial class ConfigEditor : Form
    {
        private ValheimPlusConf valheimPlusConf { get; set; }

        private bool manageClient { get; set; }

        TabControl.TabPageCollection tabSetup { get; set; }

        public ConfigEditor(bool manageClient)
        {
            InitializeComponent();
            this.Icon = Properties.Resources.valheim_plus;

            this.manageClient = manageClient;

            if(manageClient)
            {
                valheimPlusConf = ConfigManager.ReadConfigFile(true);
            }
            else
            {
                valheimPlusConf = ConfigManager.ReadConfigFile(false);
            }
            
            tabSetup = tabControl1.TabPages;

            // Advanced building mode settings
            enterAdvancedBuildingModeTextBox.Text = valheimPlusConf.enterAdvancedBuildingMode;
            exitAdvancedBuildingModeTextBox.Text = valheimPlusConf.exitAdvancedBuildingMode;

            // Advanced editing mode settings
            enterAdvancedEditingModeTextBox.Text = valheimPlusConf.enterAdvancedEditingMode;
            resetAdvancedEditingModeTextBox.Text = valheimPlusConf.resetAdvancedEditingMode;
            abortAndExitAdvancedEditingModeTextBox.Text = valheimPlusConf.abortAndExitAdvancedEditingMode;
            confirmPlacementOfAdvancedEditingModeTextBox.Text = valheimPlusConf.confirmPlacementOfAdvancedEditingMode;

            // Beehive
            honeyProductionSpeedNumeric.Value = (decimal)valheimPlusConf.honeyProductionSpeed;
            maximumHoneyPerBeehiveNumeric.Value = valheimPlusConf.maximumHoneyPerBeehive;

            // Building
            maximumPlacementDistanceNumeric.Value = (decimal)valheimPlusConf.maximumPlacementDistance;
            noWeatherDamageCheckBox.Checked = valheimPlusConf.noWeatherDamage;
            noInvalidPlacementRestrictionCheckBox.Checked = valheimPlusConf.noInvalidPlacementRestriction;

            // Camera
            cameraMaximumZoomDistanceNumeric.Value = (decimal)valheimPlusConf.cameraMaximumZoomDistance;
            cameraBoatMaximumZoomDistanceNumeric.Value = (decimal)valheimPlusConf.cameraBoatMaximumZoomDistance;
            cameraFOVNumeric.Value = (decimal)valheimPlusConf.cameraFOV;

            // Experience

            // Ward
            wardRangeNumeric.Value = (decimal)valheimPlusConf.wardRange;

        }

        private void saveConfigButton_Click(object sender, EventArgs e)
        {
            // Advanced building mode settings
            valheimPlusConf.enterAdvancedBuildingMode = enterAdvancedBuildingModeTextBox.Text;
            valheimPlusConf.exitAdvancedBuildingMode = exitAdvancedBuildingModeTextBox.Text;

            // Advanced editing mode settings
            valheimPlusConf.enterAdvancedEditingMode = enterAdvancedEditingModeTextBox.Text;
            valheimPlusConf.resetAdvancedEditingMode = resetAdvancedEditingModeTextBox.Text;
            valheimPlusConf.abortAndExitAdvancedEditingMode = abortAndExitAdvancedEditingModeTextBox.Text;
            valheimPlusConf.confirmPlacementOfAdvancedEditingMode = confirmPlacementOfAdvancedEditingModeTextBox.Text;

            // Beehive
            valheimPlusConf.honeyProductionSpeed = (float)honeyProductionSpeedNumeric.Value;
            valheimPlusConf.maximumHoneyPerBeehive = (int)maximumHoneyPerBeehiveNumeric.Value;

            // Building
            valheimPlusConf.maximumPlacementDistance = (float)maximumPlacementDistanceNumeric.Value;
            valheimPlusConf.noWeatherDamage = noWeatherDamageCheckBox.Checked;
            valheimPlusConf.noInvalidPlacementRestriction = noInvalidPlacementRestrictionCheckBox.Checked;

            // Camera
            valheimPlusConf.cameraMaximumZoomDistance = (float)cameraMaximumZoomDistanceNumeric.Value;
            valheimPlusConf.cameraBoatMaximumZoomDistance = (float)cameraBoatMaximumZoomDistanceNumeric.Value;
            valheimPlusConf.cameraFOV = (float)cameraFOVNumeric.Value;

            // Ward
            valheimPlusConf.wardRange = (float)wardRangeNumeric.Value;

            bool success = ConfigManager.WriteConfigFile(valheimPlusConf, manageClient);
            
            if(success)
            {
                saveChangesLabel.Text = String.Format("Changes saved!");
                saveChangesLabel.ForeColor = Color.Green;
            }
            else
            {
                saveChangesLabel.Text = String.Format("Error, changes not saved!");
                saveChangesLabel.ForeColor = Color.Red;
            }

            var t = new Timer();
            t.Interval = 3000; // it will Tick in 3 seconds
            t.Tick += (s, e) =>
            {
                saveChangesLabel.Hide();
                t.Stop();
            };
            t.Start();
        }

        private void configCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                // ToDo optimize this code block, good enough for alpha testing
                for (int i = 0; i < configCheckedListBox.Items.Count; i++)
                {
                    if (configCheckedListBox.GetItemChecked(i))
                    {
                        tabSetup[i].Enabled = true;
                        switch (i)
                        {
                            case 0:
                                valheimPlusConf.advancedBuildingModeEnabled = true;
                                break;
                            case 1:
                                valheimPlusConf.advancedEditingModeEnabled = true;
                                break;
                            case 2:
                                valheimPlusConf.beehiveSettingsEnabled = true;
                                break;
                            case 3:
                                valheimPlusConf.buildingSettingsEnabled = true;
                                break;
                            case 4:
                                valheimPlusConf.cameraSettingsEnabled = true;
                                break;
                            case 5:
                                valheimPlusConf.experienceSettingsEnabled = true;
                                break;
                            case 6:
                                valheimPlusConf.fermenterSettingsEnabled = true;
                                break;
                            case 7:
                                valheimPlusConf.fireplaceSettingsEnabled = true;
                                break;
                            case 8:
                                valheimPlusConf.foodSettingsEnabled = true;
                                break;
                            case 9:
                                valheimPlusConf.furnaceSettingsEnabled = true;
                                break;
                            case 10:
                                valheimPlusConf.gameSettingsEnabled = true;
                                break;
                            case 11:
                                valheimPlusConf.hotkeysSettingsEnabled = true;
                                break;
                            case 12:
                                valheimPlusConf.hudSettingsEnabled = true;
                                break;
                            case 13:
                                valheimPlusConf.itemsSettingsEnabled = true;
                                break;
                            case 14:
                                valheimPlusConf.kilnSettingsEnabled = true;
                                break;
                            case 15:
                                valheimPlusConf.mapSettingsEnabled = true;
                                break;
                            case 16:
                                valheimPlusConf.playerSettingsEnabled = true;
                                break;
                            case 17:
                                valheimPlusConf.serverSettingsEnabled = true;
                                break;
                            case 18:
                                valheimPlusConf.staminaSettingsEnabled = true;
                                break;
                            case 19:
                                valheimPlusConf.staminaUsageSettingsEnabled = true;
                                break;
                            case 20:
                                valheimPlusConf.structuralIntegritySettingsEnabled = true;
                                break;
                            case 21:
                                valheimPlusConf.timeSettingsEnabled = true;
                                break;
                            case 22:
                                valheimPlusConf.wagonSettingsEnabled = true;
                                break;
                            case 23:
                                valheimPlusConf.wardSettingsEnabled = true;
                                break;
                            case 24:
                                valheimPlusConf.workbenchSettingsEnabled = true;
                                break;
                            default:
                                break;
                        }
                    }
                    else if (!configCheckedListBox.GetItemChecked(i))
                    {
                        switch (i)
                        {
                            case 0:
                                valheimPlusConf.advancedBuildingModeEnabled = false;
                                break;
                            case 1:
                                valheimPlusConf.advancedEditingModeEnabled = false;
                                break;
                            case 2:
                                valheimPlusConf.beehiveSettingsEnabled = false;
                                break;
                            case 3:
                                valheimPlusConf.buildingSettingsEnabled = false;
                                break;
                            case 4:
                                valheimPlusConf.cameraSettingsEnabled = false;
                                break;
                            case 5:
                                valheimPlusConf.experienceSettingsEnabled = false;
                                break;
                            case 6:
                                valheimPlusConf.fermenterSettingsEnabled = false;
                                break;
                            case 7:
                                valheimPlusConf.fireplaceSettingsEnabled = false;
                                break;
                            case 8:
                                valheimPlusConf.foodSettingsEnabled = false;
                                break;
                            case 9:
                                valheimPlusConf.furnaceSettingsEnabled = false;
                                break;
                            case 10:
                                valheimPlusConf.gameSettingsEnabled = false;
                                break;
                            case 11:
                                valheimPlusConf.hotkeysSettingsEnabled = false;
                                break;
                            case 12:
                                valheimPlusConf.hudSettingsEnabled = false;
                                break;
                            case 13:
                                valheimPlusConf.itemsSettingsEnabled = false;
                                break;
                            case 14:
                                valheimPlusConf.kilnSettingsEnabled = false;
                                break;
                            case 15:
                                valheimPlusConf.mapSettingsEnabled = false;
                                break;
                            case 16:
                                valheimPlusConf.playerSettingsEnabled = false;
                                break;
                            case 17:
                                valheimPlusConf.serverSettingsEnabled = false;
                                break;
                            case 18:
                                valheimPlusConf.staminaSettingsEnabled = false;
                                break;
                            case 19:
                                valheimPlusConf.staminaUsageSettingsEnabled = false;
                                break;
                            case 20:
                                valheimPlusConf.structuralIntegritySettingsEnabled = false;
                                break;
                            case 21:
                                valheimPlusConf.timeSettingsEnabled = false;
                                break;
                            case 22:
                                valheimPlusConf.wagonSettingsEnabled = false;
                                break;
                            case 23:
                                valheimPlusConf.wardSettingsEnabled = false;
                                break;
                            case 24:
                                valheimPlusConf.workbenchSettingsEnabled = false;
                                break;
                            default:
                                break;
                        }
                        tabSetup[i].Enabled = false;
                    }
                }
            }));
        }

        private void ConfigEditor_Load(object sender, EventArgs e)
        {
            if (valheimPlusConf.advancedBuildingModeEnabled)
            {
                configCheckedListBox.SetItemChecked(0, valheimPlusConf.advancedBuildingModeEnabled);
                advancedBuildingModeTab.Enabled = true;
            }
            if (valheimPlusConf.advancedEditingModeEnabled)
            {
                configCheckedListBox.SetItemChecked(1, valheimPlusConf.advancedEditingModeEnabled);
                advancedEditingTab.Enabled = true;
            }
            if (valheimPlusConf.beehiveSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(2, valheimPlusConf.beehiveSettingsEnabled);
                beehiveTab.Enabled = true;
            }
            if (valheimPlusConf.buildingSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(3, valheimPlusConf.buildingSettingsEnabled);
                buildingTab.Enabled = true;
            }
            if (valheimPlusConf.cameraSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(4, valheimPlusConf.cameraSettingsEnabled);
                cameraTab.Enabled = true;
            }
            if (valheimPlusConf.experienceSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(5, valheimPlusConf.experienceSettingsEnabled);
                experienceTab.Enabled = true;
            }
            if (valheimPlusConf.fermenterSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(6, valheimPlusConf.fermenterSettingsEnabled);
                fermenterTab.Enabled = true;
            }
            if (valheimPlusConf.fireplaceSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(7, valheimPlusConf.fireplaceSettingsEnabled);
                fireplaceTab.Enabled = true;
            }
            if (valheimPlusConf.foodSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(8, valheimPlusConf.foodSettingsEnabled);
                foodTab.Enabled = true;
            }
            if (valheimPlusConf.furnaceSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(9, valheimPlusConf.furnaceSettingsEnabled);
                furnaceTab.Enabled = true;
            }
            if (valheimPlusConf.gameSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(10, valheimPlusConf.gameSettingsEnabled);
                gameTab.Enabled = true;
            }
            if (valheimPlusConf.hotkeysSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(11, valheimPlusConf.hotkeysSettingsEnabled);
                hotkeysTab.Enabled = true;
            }
            if (valheimPlusConf.hudSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(12, valheimPlusConf.hudSettingsEnabled);
                hudTab.Enabled = true;
            }
            if (valheimPlusConf.itemsSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(13, valheimPlusConf.itemsSettingsEnabled);
                itemsTab.Enabled = true;
            }
            if (valheimPlusConf.kilnSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(14, valheimPlusConf.kilnSettingsEnabled);
                kilnTab.Enabled = true;
            }
            if (valheimPlusConf.mapSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(15, valheimPlusConf.mapSettingsEnabled);
                mapTab.Enabled = true;
            }
            if (valheimPlusConf.playerSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(16, valheimPlusConf.playerSettingsEnabled);
                playerTab.Enabled = true;
            }
            if (valheimPlusConf.serverSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(17, valheimPlusConf.serverSettingsEnabled);
                serverTab.Enabled = true;
            }
            if (valheimPlusConf.staminaSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(18, valheimPlusConf.staminaSettingsEnabled);
                staminaTab.Enabled = true;
            }
            if (valheimPlusConf.staminaUsageSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(19, valheimPlusConf.staminaUsageSettingsEnabled);
                staminaUsageTab.Enabled = true;
            }
            if (valheimPlusConf.structuralIntegritySettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(20, valheimPlusConf.structuralIntegritySettingsEnabled);
                structuralIntegrityTab.Enabled = true;
            }
            if (valheimPlusConf.timeSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(21, valheimPlusConf.timeSettingsEnabled);
                timeTab.Enabled = true;
            }
            if (valheimPlusConf.wagonSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(22, valheimPlusConf.wagonSettingsEnabled);
                wagonTab.Enabled = true;
            }
            if (valheimPlusConf.wardSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(23, valheimPlusConf.wardSettingsEnabled);
                wardTab.Enabled = true;
            }
            if (valheimPlusConf.workbenchSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(24, valheimPlusConf.workbenchSettingsEnabled);
                workbenchTab.Enabled = true;
            }

            for (int i = 0; i < configCheckedListBox.Items.Count; i++)
            {
                if (configCheckedListBox.GetItemChecked(i))
                {
                    tabSetup[i].Enabled = true;
                }
                else if (!configCheckedListBox.GetItemChecked(i))
                {
                    tabSetup[i].Enabled = false;
                }
            }
        }
    }
}
