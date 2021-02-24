using System;
using System.Drawing;
using System.Windows.Forms;
using ValheimPlusManager.Models;
using ValheimPlusManager.SupportClasses;

namespace ValheimPlusManager
{
    public partial class ConfigEditor : Form
    {
        private ValheimPlusConf ValheimPlusConf { get; set; }

        private bool ManageClient { get; set; }

        TabControl.TabPageCollection TabSetup { get; set; }

        public ConfigEditor(bool manageClient)
        {
            InitializeComponent();
            this.Icon = Properties.Resources.valheim_plus;

            this.ManageClient = manageClient;

            if(manageClient)
            {
                ValheimPlusConf = ConfigManager.ReadConfigFile(true);
            }
            else
            {
                ValheimPlusConf = ConfigManager.ReadConfigFile(false);
            }
            
            TabSetup = tabControl1.TabPages;

            // Advanced building mode settings
            enterAdvancedBuildingModeTextBox.Text = ValheimPlusConf.enterAdvancedBuildingMode;
            exitAdvancedBuildingModeTextBox.Text = ValheimPlusConf.exitAdvancedBuildingMode;

            // Advanced editing mode settings
            enterAdvancedEditingModeTextBox.Text = ValheimPlusConf.enterAdvancedEditingMode;
            resetAdvancedEditingModeTextBox.Text = ValheimPlusConf.resetAdvancedEditingMode;
            abortAndExitAdvancedEditingModeTextBox.Text = ValheimPlusConf.abortAndExitAdvancedEditingMode;
            confirmPlacementOfAdvancedEditingModeTextBox.Text = ValheimPlusConf.confirmPlacementOfAdvancedEditingMode;

            // Beehive
            honeyProductionSpeedNumeric.Value = (decimal)ValheimPlusConf.honeyProductionSpeed;
            maximumHoneyPerBeehiveNumeric.Value = ValheimPlusConf.maximumHoneyPerBeehive;

            // Building
            maximumPlacementDistanceNumeric.Value = (decimal)ValheimPlusConf.maximumPlacementDistance;
            noWeatherDamageCheckBox.Checked = ValheimPlusConf.noWeatherDamage;
            noInvalidPlacementRestrictionCheckBox.Checked = ValheimPlusConf.noInvalidPlacementRestriction;

            // Camera
            cameraMaximumZoomDistanceNumeric.Value = (decimal)ValheimPlusConf.cameraMaximumZoomDistance;
            cameraBoatMaximumZoomDistanceNumeric.Value = (decimal)ValheimPlusConf.cameraBoatMaximumZoomDistance;
            cameraFOVNumeric.Value = (decimal)ValheimPlusConf.cameraFOV;

            // Experience

            // Ward
            wardRangeNumeric.Value = (decimal)ValheimPlusConf.wardRange;

        }

        private void saveConfigButton_Click(object sender, EventArgs e)
        {
            // Advanced building mode settings
            ValheimPlusConf.enterAdvancedBuildingMode = enterAdvancedBuildingModeTextBox.Text;
            ValheimPlusConf.exitAdvancedBuildingMode = exitAdvancedBuildingModeTextBox.Text;

            // Advanced editing mode settings
            ValheimPlusConf.enterAdvancedEditingMode = enterAdvancedEditingModeTextBox.Text;
            ValheimPlusConf.resetAdvancedEditingMode = resetAdvancedEditingModeTextBox.Text;
            ValheimPlusConf.abortAndExitAdvancedEditingMode = abortAndExitAdvancedEditingModeTextBox.Text;
            ValheimPlusConf.confirmPlacementOfAdvancedEditingMode = confirmPlacementOfAdvancedEditingModeTextBox.Text;

            // Beehive
            ValheimPlusConf.honeyProductionSpeed = (float)honeyProductionSpeedNumeric.Value;
            ValheimPlusConf.maximumHoneyPerBeehive = (int)maximumHoneyPerBeehiveNumeric.Value;

            // Building
            ValheimPlusConf.maximumPlacementDistance = (float)maximumPlacementDistanceNumeric.Value;
            ValheimPlusConf.noWeatherDamage = noWeatherDamageCheckBox.Checked;
            ValheimPlusConf.noInvalidPlacementRestriction = noInvalidPlacementRestrictionCheckBox.Checked;

            // Camera
            ValheimPlusConf.cameraMaximumZoomDistance = (float)cameraMaximumZoomDistanceNumeric.Value;
            ValheimPlusConf.cameraBoatMaximumZoomDistance = (float)cameraBoatMaximumZoomDistanceNumeric.Value;
            ValheimPlusConf.cameraFOV = (float)cameraFOVNumeric.Value;

            // Ward
            ValheimPlusConf.wardRange = (float)wardRangeNumeric.Value;

            bool success = ConfigManager.WriteConfigFile(ValheimPlusConf, ManageClient);
            
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

            var t = new Timer
            {
                Interval = 3000 // it will Tick in 3 seconds
            };
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
                        TabSetup[i].Enabled = true;
                        switch (i)
                        {
                            case 0:
                                ValheimPlusConf.advancedBuildingModeEnabled = true;
                                break;
                            case 1:
                                ValheimPlusConf.advancedEditingModeEnabled = true;
                                break;
                            case 2:
                                ValheimPlusConf.beehiveSettingsEnabled = true;
                                break;
                            case 3:
                                ValheimPlusConf.buildingSettingsEnabled = true;
                                break;
                            case 4:
                                ValheimPlusConf.cameraSettingsEnabled = true;
                                break;
                            case 5:
                                ValheimPlusConf.experienceSettingsEnabled = true;
                                break;
                            case 6:
                                ValheimPlusConf.fermenterSettingsEnabled = true;
                                break;
                            case 7:
                                ValheimPlusConf.fireplaceSettingsEnabled = true;
                                break;
                            case 8:
                                ValheimPlusConf.foodSettingsEnabled = true;
                                break;
                            case 9:
                                ValheimPlusConf.furnaceSettingsEnabled = true;
                                break;
                            case 10:
                                ValheimPlusConf.gameSettingsEnabled = true;
                                break;
                            case 11:
                                ValheimPlusConf.hotkeysSettingsEnabled = true;
                                break;
                            case 12:
                                ValheimPlusConf.hudSettingsEnabled = true;
                                break;
                            case 13:
                                ValheimPlusConf.itemsSettingsEnabled = true;
                                break;
                            case 14:
                                ValheimPlusConf.kilnSettingsEnabled = true;
                                break;
                            case 15:
                                ValheimPlusConf.mapSettingsEnabled = true;
                                break;
                            case 16:
                                ValheimPlusConf.playerSettingsEnabled = true;
                                break;
                            case 17:
                                ValheimPlusConf.serverSettingsEnabled = true;
                                break;
                            case 18:
                                ValheimPlusConf.staminaSettingsEnabled = true;
                                break;
                            case 19:
                                ValheimPlusConf.staminaUsageSettingsEnabled = true;
                                break;
                            case 20:
                                ValheimPlusConf.structuralIntegritySettingsEnabled = true;
                                break;
                            case 21:
                                ValheimPlusConf.timeSettingsEnabled = true;
                                break;
                            case 22:
                                ValheimPlusConf.wagonSettingsEnabled = true;
                                break;
                            case 23:
                                ValheimPlusConf.wardSettingsEnabled = true;
                                break;
                            case 24:
                                ValheimPlusConf.workbenchSettingsEnabled = true;
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
                                ValheimPlusConf.advancedBuildingModeEnabled = false;
                                break;
                            case 1:
                                ValheimPlusConf.advancedEditingModeEnabled = false;
                                break;
                            case 2:
                                ValheimPlusConf.beehiveSettingsEnabled = false;
                                break;
                            case 3:
                                ValheimPlusConf.buildingSettingsEnabled = false;
                                break;
                            case 4:
                                ValheimPlusConf.cameraSettingsEnabled = false;
                                break;
                            case 5:
                                ValheimPlusConf.experienceSettingsEnabled = false;
                                break;
                            case 6:
                                ValheimPlusConf.fermenterSettingsEnabled = false;
                                break;
                            case 7:
                                ValheimPlusConf.fireplaceSettingsEnabled = false;
                                break;
                            case 8:
                                ValheimPlusConf.foodSettingsEnabled = false;
                                break;
                            case 9:
                                ValheimPlusConf.furnaceSettingsEnabled = false;
                                break;
                            case 10:
                                ValheimPlusConf.gameSettingsEnabled = false;
                                break;
                            case 11:
                                ValheimPlusConf.hotkeysSettingsEnabled = false;
                                break;
                            case 12:
                                ValheimPlusConf.hudSettingsEnabled = false;
                                break;
                            case 13:
                                ValheimPlusConf.itemsSettingsEnabled = false;
                                break;
                            case 14:
                                ValheimPlusConf.kilnSettingsEnabled = false;
                                break;
                            case 15:
                                ValheimPlusConf.mapSettingsEnabled = false;
                                break;
                            case 16:
                                ValheimPlusConf.playerSettingsEnabled = false;
                                break;
                            case 17:
                                ValheimPlusConf.serverSettingsEnabled = false;
                                break;
                            case 18:
                                ValheimPlusConf.staminaSettingsEnabled = false;
                                break;
                            case 19:
                                ValheimPlusConf.staminaUsageSettingsEnabled = false;
                                break;
                            case 20:
                                ValheimPlusConf.structuralIntegritySettingsEnabled = false;
                                break;
                            case 21:
                                ValheimPlusConf.timeSettingsEnabled = false;
                                break;
                            case 22:
                                ValheimPlusConf.wagonSettingsEnabled = false;
                                break;
                            case 23:
                                ValheimPlusConf.wardSettingsEnabled = false;
                                break;
                            case 24:
                                ValheimPlusConf.workbenchSettingsEnabled = false;
                                break;
                            default:
                                break;
                        }
                        TabSetup[i].Enabled = false;
                    }
                }
            }));
        }

        private void ConfigEditor_Load(object sender, EventArgs e)
        {
            if (ValheimPlusConf.advancedBuildingModeEnabled)
            {
                configCheckedListBox.SetItemChecked(0, ValheimPlusConf.advancedBuildingModeEnabled);
                advancedBuildingModeTab.Enabled = true;
            }
            if (ValheimPlusConf.advancedEditingModeEnabled)
            {
                configCheckedListBox.SetItemChecked(1, ValheimPlusConf.advancedEditingModeEnabled);
                advancedEditingTab.Enabled = true;
            }
            if (ValheimPlusConf.beehiveSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(2, ValheimPlusConf.beehiveSettingsEnabled);
                beehiveTab.Enabled = true;
            }
            if (ValheimPlusConf.buildingSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(3, ValheimPlusConf.buildingSettingsEnabled);
                buildingTab.Enabled = true;
            }
            if (ValheimPlusConf.cameraSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(4, ValheimPlusConf.cameraSettingsEnabled);
                cameraTab.Enabled = true;
            }
            if (ValheimPlusConf.experienceSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(5, ValheimPlusConf.experienceSettingsEnabled);
                experienceTab.Enabled = true;
            }
            if (ValheimPlusConf.fermenterSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(6, ValheimPlusConf.fermenterSettingsEnabled);
                fermenterTab.Enabled = true;
            }
            if (ValheimPlusConf.fireplaceSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(7, ValheimPlusConf.fireplaceSettingsEnabled);
                fireplaceTab.Enabled = true;
            }
            if (ValheimPlusConf.foodSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(8, ValheimPlusConf.foodSettingsEnabled);
                foodTab.Enabled = true;
            }
            if (ValheimPlusConf.furnaceSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(9, ValheimPlusConf.furnaceSettingsEnabled);
                furnaceTab.Enabled = true;
            }
            if (ValheimPlusConf.gameSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(10, ValheimPlusConf.gameSettingsEnabled);
                gameTab.Enabled = true;
            }
            if (ValheimPlusConf.hotkeysSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(11, ValheimPlusConf.hotkeysSettingsEnabled);
                hotkeysTab.Enabled = true;
            }
            if (ValheimPlusConf.hudSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(12, ValheimPlusConf.hudSettingsEnabled);
                hudTab.Enabled = true;
            }
            if (ValheimPlusConf.itemsSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(13, ValheimPlusConf.itemsSettingsEnabled);
                itemsTab.Enabled = true;
            }
            if (ValheimPlusConf.kilnSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(14, ValheimPlusConf.kilnSettingsEnabled);
                kilnTab.Enabled = true;
            }
            if (ValheimPlusConf.mapSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(15, ValheimPlusConf.mapSettingsEnabled);
                mapTab.Enabled = true;
            }
            if (ValheimPlusConf.playerSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(16, ValheimPlusConf.playerSettingsEnabled);
                playerTab.Enabled = true;
            }
            if (ValheimPlusConf.serverSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(17, ValheimPlusConf.serverSettingsEnabled);
                serverTab.Enabled = true;
            }
            if (ValheimPlusConf.staminaSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(18, ValheimPlusConf.staminaSettingsEnabled);
                staminaTab.Enabled = true;
            }
            if (ValheimPlusConf.staminaUsageSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(19, ValheimPlusConf.staminaUsageSettingsEnabled);
                staminaUsageTab.Enabled = true;
            }
            if (ValheimPlusConf.structuralIntegritySettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(20, ValheimPlusConf.structuralIntegritySettingsEnabled);
                structuralIntegrityTab.Enabled = true;
            }
            if (ValheimPlusConf.timeSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(21, ValheimPlusConf.timeSettingsEnabled);
                timeTab.Enabled = true;
            }
            if (ValheimPlusConf.wagonSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(22, ValheimPlusConf.wagonSettingsEnabled);
                wagonTab.Enabled = true;
            }
            if (ValheimPlusConf.wardSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(23, ValheimPlusConf.wardSettingsEnabled);
                wardTab.Enabled = true;
            }
            if (ValheimPlusConf.workbenchSettingsEnabled)
            {
                configCheckedListBox.SetItemChecked(24, ValheimPlusConf.workbenchSettingsEnabled);
                workbenchTab.Enabled = true;
            }

            for (int i = 0; i < configCheckedListBox.Items.Count; i++)
            {
                if (configCheckedListBox.GetItemChecked(i))
                {
                    TabSetup[i].Enabled = true;
                }
                else if (!configCheckedListBox.GetItemChecked(i))
                {
                    TabSetup[i].Enabled = false;
                }
            }
        }
    }
}
