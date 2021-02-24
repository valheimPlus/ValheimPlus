using IniParser;
using IniParser.Model;
using ValheimPlusManager.Data;
using ValheimPlusManager.Models;

namespace ValheimPlusManager.SupportClasses
{
    public class ConfigManager
    {
        public static ValheimPlusConf ReadConfigFile(bool manageClient)
        {
            ValheimPlusConf valheimPlusConfiguration = new ValheimPlusConf();
            Settings settings = SettingsDAL.GetSettings();
            IniData data;

            var parser = new FileIniDataParser();
            if(manageClient)
            {
                data = parser.ReadFile(string.Format("{0}BepInEx/config/valheim_plus.cfg", settings.ClientInstallationPath));
            }
            else
            {
                data = parser.ReadFile(string.Format("{0}BepInEx/config/valheim_plus.cfg", settings.ServerInstallationPath));
            }
            
            // Advanced building mode settings
            valheimPlusConfiguration.advancedBuildingModeEnabled = bool.Parse(data["AdvancedBuildingMode"]["enabled"]);
            valheimPlusConfiguration.enterAdvancedBuildingMode = data["AdvancedBuildingMode"]["enterAdvancedBuildingMode"];
            valheimPlusConfiguration.exitAdvancedBuildingMode = data["AdvancedBuildingMode"]["exitAdvancedBuildingMode"];

            // Advanced editing mode settings
            valheimPlusConfiguration.advancedEditingModeEnabled = bool.Parse(data["AdvancedEditingMode"]["enabled"]);
            valheimPlusConfiguration.enterAdvancedEditingMode = data["AdvancedEditingMode"]["enterAdvancedEditingMode"];
            valheimPlusConfiguration.resetAdvancedEditingMode = data["AdvancedEditingMode"]["resetAdvancedEditingMode"];
            valheimPlusConfiguration.abortAndExitAdvancedEditingMode = data["AdvancedEditingMode"]["abortAndExitAdvancedEditingMode"];
            valheimPlusConfiguration.confirmPlacementOfAdvancedEditingMode = data["AdvancedEditingMode"]["confirmPlacementOfAdvancedEditingMode"];

            // Beehive
            valheimPlusConfiguration.beehiveSettingsEnabled = bool.Parse(data["Beehive"]["enabled"]);
            valheimPlusConfiguration.honeyProductionSpeed = float.Parse(data["Beehive"]["honeyProductionSpeed"]);
            valheimPlusConfiguration.maximumHoneyPerBeehive = int.Parse(data["Beehive"]["maximumHoneyPerBeehive"]);

            // Building
            valheimPlusConfiguration.buildingSettingsEnabled = bool.Parse(data["Building"]["enabled"]);
            valheimPlusConfiguration.noInvalidPlacementRestriction = bool.Parse(data["Building"]["noInvalidPlacementRestriction"]);
            valheimPlusConfiguration.noWeatherDamage = bool.Parse(data["Building"]["noWeatherDamage"]);
            valheimPlusConfiguration.maximumPlacementDistance = float.Parse(data["Building"]["maximumPlacementDistance"]);

            // Items
            valheimPlusConfiguration.itemsSettingsEnabled = bool.Parse(data["Items"]["enabled"]);
            valheimPlusConfiguration.noTeleportPrevention = bool.Parse(data["Items"]["noTeleportPrevention"]);
            valheimPlusConfiguration.baseItemWeightReduction = float.Parse(data["Items"]["baseItemWeightReduction"]);
            valheimPlusConfiguration.itemStackMultiplier = float.Parse(data["Items"]["itemStackMultiplier"]);

            // Fermenter
            valheimPlusConfiguration.fermenterSettingsEnabled = bool.Parse(data["Fermenter"]["enabled"]);

            // Fireplace
            valheimPlusConfiguration.fireplaceSettingsEnabled = bool.Parse(data["Fireplace"]["enabled"]);

            // Food
            valheimPlusConfiguration.foodSettingsEnabled = bool.Parse(data["Food"]["enabled"]);

            // Furnace
            valheimPlusConfiguration.furnaceSettingsEnabled = bool.Parse(data["Furnace"]["enabled"]);

            // Game
            valheimPlusConfiguration.gameSettingsEnabled = bool.Parse(data["Game"]["enabled"]);

            // Hotkeys
            valheimPlusConfiguration.hotkeysSettingsEnabled = bool.Parse(data["Hotkeys"]["enabled"]);

            // Hud
            valheimPlusConfiguration.hudSettingsEnabled = bool.Parse(data["Hud"]["enabled"]);

            // Kiln
            valheimPlusConfiguration.kilnSettingsEnabled = bool.Parse(data["Kiln"]["enabled"]);

            // Map
            valheimPlusConfiguration.mapSettingsEnabled = bool.Parse(data["Map"]["enabled"]);

            // Player
            valheimPlusConfiguration.playerSettingsEnabled = bool.Parse(data["Player"]["enabled"]);
            valheimPlusConfiguration.baseMaximumWeight = float.Parse(data["Player"]["baseMaximumWeight"]);
            valheimPlusConfiguration.baseMegingjordBuff = float.Parse(data["Player"]["baseMegingjordBuff"]);
            valheimPlusConfiguration.baseAutoPickUpRange = float.Parse(data["Player"]["baseAutoPickUpRange"]);
            valheimPlusConfiguration.disableCameraShake = bool.Parse(data["Player"]["disableCameraShake"]);
            valheimPlusConfiguration.baseUnarmedDamage = float.Parse(data["Player"]["baseUnarmedDamage"]);

            // Server
            valheimPlusConfiguration.serverSettingsEnabled = bool.Parse(data["Server"]["enabled"]);

            // Stamina
            valheimPlusConfiguration.staminaSettingsEnabled = bool.Parse(data["Stamina"]["enabled"]);

            // StaminaUsage
            valheimPlusConfiguration.staminaUsageSettingsEnabled = bool.Parse(data["StaminaUsage"]["enabled"]);

            // Workbench
            valheimPlusConfiguration.workbenchSettingsEnabled = bool.Parse(data["Workbench"]["enabled"]);

            // Time
            valheimPlusConfiguration.timeSettingsEnabled = bool.Parse(data["Time"]["enabled"]);

            // Ward
            valheimPlusConfiguration.wardSettingsEnabled = bool.Parse(data["Ward"]["enabled"]);

            // StructuralIntegrity
            valheimPlusConfiguration.structuralIntegritySettingsEnabled = bool.Parse(data["StructuralIntegrity"]["enabled"]);

            // Experience
            valheimPlusConfiguration.experienceSettingsEnabled = bool.Parse(data["Experience"]["enabled"]);

            // Camera
            valheimPlusConfiguration.cameraSettingsEnabled = bool.Parse(data["Camera"]["enabled"]);

            // Wagon
            valheimPlusConfiguration.wagonSettingsEnabled = bool.Parse(data["Wagon"]["enabled"]);

            return valheimPlusConfiguration;
        }

        public static bool WriteConfigFile(ValheimPlusConf valheimPlusConf, bool manageClient)
        {
            Settings settings = SettingsDAL.GetSettings();
            IniData data;

            var parser = new FileIniDataParser();

            // Reading the current configuration file
            if (manageClient)
            {
                data = parser.ReadFile(string.Format("{0}BepInEx/config/valheim_plus.cfg", settings.ClientInstallationPath));
            }
            else
            {
                data = parser.ReadFile(string.Format("{0}BepInEx/config/valheim_plus.cfg", settings.ServerInstallationPath));
            }

            // Advanced building mode settings
            data["AdvancedBuildingMode"]["enabled"] = valheimPlusConf.advancedBuildingModeEnabled.ToString().ToLower();

            // Advanced editing mode settings
            data["AdvancedEditingMode"]["enabled"] = valheimPlusConf.advancedEditingModeEnabled.ToString().ToLower();

            // Beehive
            data["Beehive"]["enabled"] = valheimPlusConf.beehiveSettingsEnabled.ToString().ToLower();
            data["Beehive"]["honeyProductionSpeed"] = valheimPlusConf.honeyProductionSpeed.ToString();
            data["Beehive"]["maximumHoneyPerBeehive"] = valheimPlusConf.maximumHoneyPerBeehive.ToString();

            // Building
            data["Building"]["enabled"] = valheimPlusConf.buildingSettingsEnabled.ToString().ToLower();

            // Items
            data["Items"]["enabled"] = valheimPlusConf.itemsSettingsEnabled.ToString().ToLower();

            // Fermenter
            data["Fermenter"]["enabled"] = valheimPlusConf.fermenterSettingsEnabled.ToString().ToLower();

            // Fireplace
            data["Fireplace"]["enabled"] = valheimPlusConf.fireplaceSettingsEnabled.ToString().ToLower();

            // Food
            data["Food"]["enabled"] = valheimPlusConf.foodSettingsEnabled.ToString().ToLower();

            // Furnace
            data["Furnace"]["enabled"] = valheimPlusConf.furnaceSettingsEnabled.ToString().ToLower();

            // Game
            data["Game"]["enabled"] = valheimPlusConf.gameSettingsEnabled.ToString().ToLower();

            // Hotkeys
            data["Hotkeys"]["enabled"] = valheimPlusConf.hotkeysSettingsEnabled.ToString().ToLower();

            // Hud
            data["Hud"]["enabled"] = valheimPlusConf.hudSettingsEnabled.ToString().ToLower();

            // Kiln
            data["Kiln"]["enabled"] = valheimPlusConf.kilnSettingsEnabled.ToString().ToLower();

            // Map
            data["Map"]["enabled"] = valheimPlusConf.mapSettingsEnabled.ToString().ToLower();

            // Player
            data["Player"]["enabled"] = valheimPlusConf.playerSettingsEnabled.ToString().ToLower();

            // Server
            data["Server"]["enabled"] = valheimPlusConf.serverSettingsEnabled.ToString().ToLower();

            // Stamina
            data["Stamina"]["enabled"] = valheimPlusConf.staminaSettingsEnabled.ToString().ToLower();

            // StaminaUsage
            data["StaminaUsage"]["enabled"] = valheimPlusConf.staminaUsageSettingsEnabled.ToString().ToLower();

            // Workbench
            data["Workbench"]["enabled"] = valheimPlusConf.workbenchSettingsEnabled.ToString().ToLower();

            // Time
            data["Time"]["enabled"] = valheimPlusConf.timeSettingsEnabled.ToString().ToLower();

            // Ward
            data["Ward"]["enabled"] = valheimPlusConf.wardSettingsEnabled.ToString().ToLower();

            // StructuralIntegrity
            data["StructuralIntegrity"]["enabled"] = valheimPlusConf.structuralIntegritySettingsEnabled.ToString().ToLower();

            // Experience
            data["Experience"]["enabled"] = valheimPlusConf.experienceSettingsEnabled.ToString().ToLower();

            // Camera
            data["Camera"]["enabled"] = valheimPlusConf.cameraSettingsEnabled.ToString().ToLower();

            // Wagon
            data["Wagon"]["enabled"] = valheimPlusConf.wagonSettingsEnabled.ToString().ToLower();

            // Writing the new settings to configuration file
            try
            {
                if (manageClient)
                {
                    parser.WriteFile(string.Format("{0}BepInEx/config/valheim_plus.cfg", settings.ClientInstallationPath), data);
                }
                else
                {
                    parser.WriteFile(string.Format("{0}BepInEx/config/valheim_plus.cfg", settings.ServerInstallationPath), data);
                }

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
            
        }
    }
}
