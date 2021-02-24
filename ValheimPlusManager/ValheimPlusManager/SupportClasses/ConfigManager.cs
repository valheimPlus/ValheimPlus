using IniParser;
using IniParser.Model;
using System.Globalization;
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
            valheimPlusConfiguration.honeyProductionSpeed = float.Parse(data["Beehive"]["honeyProductionSpeed"], CultureInfo.InvariantCulture.NumberFormat);
            valheimPlusConfiguration.maximumHoneyPerBeehive = int.Parse(data["Beehive"]["maximumHoneyPerBeehive"]);

            // Building
            valheimPlusConfiguration.buildingSettingsEnabled = bool.Parse(data["Building"]["enabled"]);
            valheimPlusConfiguration.noInvalidPlacementRestriction = bool.Parse(data["Building"]["noInvalidPlacementRestriction"]);
            valheimPlusConfiguration.noWeatherDamage = bool.Parse(data["Building"]["noWeatherDamage"]);
            valheimPlusConfiguration.maximumPlacementDistance = float.Parse(data["Building"]["maximumPlacementDistance"], CultureInfo.InvariantCulture.NumberFormat);

            // Items
            valheimPlusConfiguration.itemsSettingsEnabled = bool.Parse(data["Items"]["enabled"]);
            valheimPlusConfiguration.noTeleportPrevention = bool.Parse(data["Items"]["noTeleportPrevention"]);
            valheimPlusConfiguration.baseItemWeightReduction = float.Parse(data["Items"]["baseItemWeightReduction"], CultureInfo.InvariantCulture.NumberFormat);
            valheimPlusConfiguration.itemStackMultiplier = float.Parse(data["Items"]["itemStackMultiplier"], CultureInfo.InvariantCulture.NumberFormat);

            // Fermenter
            valheimPlusConfiguration.fermenterSettingsEnabled = bool.Parse(data["Fermenter"]["enabled"]);
            valheimPlusConfiguration.fermenterDuration = float.Parse(data["Fermenter"]["fermenterDuration"], CultureInfo.InvariantCulture.NumberFormat);
            valheimPlusConfiguration.fermenterItemsProduced = int.Parse(data["Fermenter"]["fermenterItemsProduced"]);

            // Fireplace
            valheimPlusConfiguration.fireplaceSettingsEnabled = bool.Parse(data["Fireplace"]["enabled"]);
            valheimPlusConfiguration.onlyTorches = bool.Parse(data["Fireplace"]["onlyTorches"]);

            // Food
            valheimPlusConfiguration.foodSettingsEnabled = bool.Parse(data["Food"]["enabled"]);
            valheimPlusConfiguration.foodDurationMultiplier = float.Parse(data["Food"]["foodDurationMultiplier"], CultureInfo.InvariantCulture.NumberFormat);

            // Furnace
            valheimPlusConfiguration.furnaceSettingsEnabled = bool.Parse(data["Furnace"]["enabled"]);
            valheimPlusConfiguration.maximumOre = int.Parse(data["Furnace"]["maximumOre"]);
            valheimPlusConfiguration.maximumCoal = int.Parse(data["Furnace"]["maximumCoal"]);
            valheimPlusConfiguration.coalUsedPerProduct = int.Parse(data["Furnace"]["coalUsedPerProduct"]);
            valheimPlusConfiguration.furnaceProductionSpeed = int.Parse(data["Furnace"]["productionSpeed"]);

            // Game
            valheimPlusConfiguration.gameSettingsEnabled = bool.Parse(data["Game"]["enabled"]);
            valheimPlusConfiguration.gameDifficultyDamageScale = float.Parse(data["Game"]["gameDifficultyDamageScale"], CultureInfo.InvariantCulture.NumberFormat);
            valheimPlusConfiguration.gameDifficultyHealthScale = float.Parse(data["Game"]["gameDifficultyHealthScale"], CultureInfo.InvariantCulture.NumberFormat);
            valheimPlusConfiguration.extraPlayerCountNearby = int.Parse(data["Game"]["extraPlayerCountNearby"]);
            valheimPlusConfiguration.setFixedPlayerCountTo = int.Parse(data["Game"]["setFixedPlayerCountTo"]);
            valheimPlusConfiguration.difficultyScaleRange = int.Parse(data["Game"]["difficultyScaleRange"]);

            // Hotkeys
            valheimPlusConfiguration.hotkeysSettingsEnabled = bool.Parse(data["Hotkeys"]["enabled"]);
            valheimPlusConfiguration.rollForwards = data["Hotkeys"]["rollForwards"];
            valheimPlusConfiguration.rollBackwards = data["Hotkeys"]["rollBackwards"];

            // Hud
            valheimPlusConfiguration.hudSettingsEnabled = bool.Parse(data["Hud"]["enabled"]);
            valheimPlusConfiguration.showRequiredItems = bool.Parse(data["Hud"]["showRequiredItems"]);
            valheimPlusConfiguration.experienceGainedNotifications = bool.Parse(data["Hud"]["experienceGainedNotifications"]);

            // Kiln
            valheimPlusConfiguration.kilnSettingsEnabled = bool.Parse(data["Kiln"]["enabled"]);
            valheimPlusConfiguration.maximumWood = int.Parse(data["Kiln"]["maximumWood"]);
            valheimPlusConfiguration.kilnProductionSpeed = int.Parse(data["Kiln"]["productionSpeed"]);

            // Map
            valheimPlusConfiguration.mapSettingsEnabled = bool.Parse(data["Map"]["enabled"]);
            valheimPlusConfiguration.shareMapProgression = bool.Parse(data["Map"]["shareMapProgression"]);
            valheimPlusConfiguration.exploreRadius = int.Parse(data["Map"]["exploreRadius"]);
            valheimPlusConfiguration.playerPositionPublicOnJoin = bool.Parse(data["Map"]["playerPositionPublicOnJoin"]);
            valheimPlusConfiguration.preventPlayerFromTurningOffPublicPosition = bool.Parse(data["Map"]["preventPlayerFromTurningOffPublicPosition"]);
            valheimPlusConfiguration.removeDeathPinOnTombstoneEmpty = bool.Parse(data["Map"]["removeDeathPinOnTombstoneEmpty"]);

            // Player
            valheimPlusConfiguration.playerSettingsEnabled = bool.Parse(data["Player"]["enabled"]);
            valheimPlusConfiguration.baseMaximumWeight = float.Parse(data["Player"]["baseMaximumWeight"], CultureInfo.InvariantCulture.NumberFormat);
            valheimPlusConfiguration.baseMegingjordBuff = float.Parse(data["Player"]["baseMegingjordBuff"], CultureInfo.InvariantCulture.NumberFormat);
            valheimPlusConfiguration.baseAutoPickUpRange = float.Parse(data["Player"]["baseAutoPickUpRange"], CultureInfo.InvariantCulture.NumberFormat);
            valheimPlusConfiguration.disableCameraShake = bool.Parse(data["Player"]["disableCameraShake"]);
            valheimPlusConfiguration.baseUnarmedDamage = float.Parse(data["Player"]["baseUnarmedDamage"], CultureInfo.InvariantCulture.NumberFormat);

            // Server
            valheimPlusConfiguration.serverSettingsEnabled = bool.Parse(data["Server"]["enabled"]);
            valheimPlusConfiguration.maxPlayers = int.Parse(data["Server"]["maxPlayers"]);
            valheimPlusConfiguration.disableServerPassword = bool.Parse(data["Server"]["disableServerPassword"]);
            valheimPlusConfiguration.enforceConfiguration = bool.Parse(data["Server"]["enforceConfiguration"]);
            valheimPlusConfiguration.enforceMod = bool.Parse(data["Server"]["enforceMod"]);
            valheimPlusConfiguration.dataRate = int.Parse(data["Server"]["dataRate"]);
            valheimPlusConfiguration.autoSaveInterval = int.Parse(data["Server"]["autoSaveInterval"]);

            // Stamina
            valheimPlusConfiguration.staminaSettingsEnabled = bool.Parse(data["Stamina"]["enabled"]);
            valheimPlusConfiguration.dodgeStaminaUsage = int.Parse(data["Stamina"]["dodgeStaminaUsage"]);
            valheimPlusConfiguration.encumberedStaminaDrain = int.Parse(data["Stamina"]["encumberedStaminaDrain"]);
            valheimPlusConfiguration.jumpStaminaDrain = int.Parse(data["Stamina"]["jumpStaminaDrain"]);
            valheimPlusConfiguration.runStaminaDrain = int.Parse(data["Stamina"]["runStaminaDrain"]);
            valheimPlusConfiguration.sneakStaminaDrain = int.Parse(data["Stamina"]["sneakStaminaDrain"]);
            valheimPlusConfiguration.staminaRegen = int.Parse(data["Stamina"]["staminaRegen"]);
            valheimPlusConfiguration.staminaRegenDelay = float.Parse(data["Stamina"]["staminaRegenDelay"], CultureInfo.InvariantCulture.NumberFormat);
            valheimPlusConfiguration.swimStaminaDrain = int.Parse(data["Stamina"]["swimStaminaDrain"]);

            // StaminaUsage
            valheimPlusConfiguration.staminaUsageSettingsEnabled = bool.Parse(data["StaminaUsage"]["enabled"]);
            valheimPlusConfiguration.axes = int.Parse(data["StaminaUsage"]["axes"]);
            valheimPlusConfiguration.bows = int.Parse(data["StaminaUsage"]["bows"]);
            valheimPlusConfiguration.clubs = int.Parse(data["StaminaUsage"]["clubs"]);
            valheimPlusConfiguration.knives = int.Parse(data["StaminaUsage"]["knives"]);
            valheimPlusConfiguration.pickaxes = int.Parse(data["StaminaUsage"]["pickaxes"]);
            valheimPlusConfiguration.polearms = int.Parse(data["StaminaUsage"]["polearms"]);
            valheimPlusConfiguration.spears = int.Parse(data["StaminaUsage"]["spears"]);
            valheimPlusConfiguration.swords = int.Parse(data["StaminaUsage"]["swords"]);
            valheimPlusConfiguration.unarmed = int.Parse(data["StaminaUsage"]["unarmed"]);
            valheimPlusConfiguration.hammer = int.Parse(data["StaminaUsage"]["hammer"]);
            valheimPlusConfiguration.hoe = int.Parse(data["StaminaUsage"]["hoe"]);

            // Workbench
            valheimPlusConfiguration.workbenchSettingsEnabled = bool.Parse(data["Workbench"]["enabled"]);
            valheimPlusConfiguration.workbenchRange = int.Parse(data["Workbench"]["workbenchRange"]);
            valheimPlusConfiguration.disableRoofCheck = bool.Parse(data["Workbench"]["disableRoofCheck"]);

            // Time
            valheimPlusConfiguration.timeSettingsEnabled = bool.Parse(data["Time"]["enabled"]);
            valheimPlusConfiguration.totalDayTimeInSeconds = int.Parse(data["Time"]["totalDayTimeInSeconds"]);
            valheimPlusConfiguration.nightTimeSpeedMultiplier = int.Parse(data["Time"]["nightTimeSpeedMultiplier"]);

            // Ward
            valheimPlusConfiguration.wardSettingsEnabled = bool.Parse(data["Ward"]["enabled"]);
            valheimPlusConfiguration.wardRange = int.Parse(data["Ward"]["wardRange"]);

            // StructuralIntegrity
            valheimPlusConfiguration.structuralIntegritySettingsEnabled = bool.Parse(data["StructuralIntegrity"]["enabled"]);
            valheimPlusConfiguration.disableStructuralIntegrity = bool.Parse(data["StructuralIntegrity"]["disableStructuralIntegrity"]);
            valheimPlusConfiguration.wood = int.Parse(data["StructuralIntegrity"]["wood"]);
            valheimPlusConfiguration.stone = int.Parse(data["StructuralIntegrity"]["stone"]);
            valheimPlusConfiguration.iron = int.Parse(data["StructuralIntegrity"]["iron"]);
            valheimPlusConfiguration.hardWood = int.Parse(data["StructuralIntegrity"]["hardWood"]);

            // Experience
            valheimPlusConfiguration.experienceSettingsEnabled = bool.Parse(data["Experience"]["enabled"]);
            valheimPlusConfiguration.experienceSwords = int.Parse(data["Experience"]["swords"]);
            valheimPlusConfiguration.experienceKnives = int.Parse(data["Experience"]["knives"]);
            valheimPlusConfiguration.experienceClubs = int.Parse(data["Experience"]["clubs"]);
            valheimPlusConfiguration.experiencePolearms = int.Parse(data["Experience"]["polearms"]);
            valheimPlusConfiguration.experienceSpears = int.Parse(data["Experience"]["spears"]);
            valheimPlusConfiguration.experienceBlocking = int.Parse(data["Experience"]["blocking"]);
            valheimPlusConfiguration.experienceAxes = int.Parse(data["Experience"]["axes"]);
            valheimPlusConfiguration.experienceBows = int.Parse(data["Experience"]["bows"]);
            valheimPlusConfiguration.experienceFireMagic = int.Parse(data["Experience"]["fireMagic"]);
            valheimPlusConfiguration.experienceFrostMagic = int.Parse(data["Experience"]["frostMagic"]);
            valheimPlusConfiguration.experienceUnarmed = int.Parse(data["Experience"]["unarmed"]);
            valheimPlusConfiguration.experiencePickaxes = int.Parse(data["Experience"]["pickaxes"]);
            valheimPlusConfiguration.experienceWoodCutting = int.Parse(data["Experience"]["woodCutting"]);
            valheimPlusConfiguration.experienceJump = int.Parse(data["Experience"]["jump"]);
            valheimPlusConfiguration.experienceSneak = int.Parse(data["Experience"]["sneak"]);
            valheimPlusConfiguration.experienceRun = int.Parse(data["Experience"]["run"]);
            valheimPlusConfiguration.experienceSwim = int.Parse(data["Experience"]["swim"]);

            // Camera
            valheimPlusConfiguration.cameraSettingsEnabled = bool.Parse(data["Camera"]["enabled"]);
            valheimPlusConfiguration.cameraMaximumZoomDistance = int.Parse(data["Camera"]["cameraMaximumZoomDistance"]);
            valheimPlusConfiguration.cameraBoatMaximumZoomDistance = int.Parse(data["Camera"]["cameraBoatMaximumZoomDistance"]);
            valheimPlusConfiguration.cameraFOV = int.Parse(data["Camera"]["cameraFOV"]);

            // Wagon
            valheimPlusConfiguration.wagonSettingsEnabled = bool.Parse(data["Wagon"]["enabled"]);
            valheimPlusConfiguration.wagonBaseMass = int.Parse(data["Wagon"]["wagonBaseMass"]);
            valheimPlusConfiguration.wagonExtraMassFromItems = int.Parse(data["Wagon"]["wagonExtraMassFromItems"]);

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
            data["Building"]["maximumPlacementDistance"] = valheimPlusConf.maximumPlacementDistance.ToString().ToLower();
            data["Building"]["noWeatherDamage"] = valheimPlusConf.noWeatherDamage.ToString();
            data["Building"]["noInvalidPlacementRestriction"] = valheimPlusConf.noInvalidPlacementRestriction.ToString();

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
            data["Ward"]["wardRange"] = valheimPlusConf.wardRange.ToString().ToLower();

            // StructuralIntegrity
            data["StructuralIntegrity"]["enabled"] = valheimPlusConf.structuralIntegritySettingsEnabled.ToString().ToLower();

            // Experience
            data["Experience"]["enabled"] = valheimPlusConf.experienceSettingsEnabled.ToString().ToLower();

            // Camera
            data["Camera"]["enabled"] = valheimPlusConf.cameraSettingsEnabled.ToString().ToLower();
            data["Camera"]["cameraMaximumZoomDistance"] = valheimPlusConf.cameraMaximumZoomDistance.ToString().ToLower();
            data["Camera"]["cameraBoatMaximumZoomDistance"] = valheimPlusConf.cameraBoatMaximumZoomDistance.ToString().ToLower();
            data["Camera"]["cameraFOV"] = valheimPlusConf.cameraFOV.ToString().ToLower();

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
