using IniParser;
using IniParser.Model;
using System.Globalization;
using ValheimPlusManager.Data;
using ValheimPlusManager.Models;

namespace ValheimPlusManager.SupportClasses
{
    public sealed class ConfigManager
    {
        public static ValheimPlusConf ReadConfigFile(bool manageClient)
        {
            ValheimPlusConf valheimPlusConfiguration = new ValheimPlusConf();
            Settings settings = SettingsDAL.GetSettings();
            IniData data;

            var parser = new FileIniDataParser();
            if (manageClient)
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
            //valheimPlusConfiguration.enforceConfiguration = bool.Parse(data["Server"]["enforceConfiguration"]);
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

        public static bool WriteConfigFile(ValheimPlusConf valheimPlusConfiguration, bool manageClient)
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
            data["AdvancedBuildingMode"]["enabled"] = valheimPlusConfiguration.advancedBuildingModeEnabled.ToString().ToLower();

            // Advanced editing mode settings
            data["AdvancedEditingMode"]["enabled"] = valheimPlusConfiguration.advancedEditingModeEnabled.ToString().ToLower();

            // Beehive
            data["Beehive"]["enabled"] = valheimPlusConfiguration.beehiveSettingsEnabled.ToString().ToLower();
            data["Beehive"]["honeyProductionSpeed"] = valheimPlusConfiguration.honeyProductionSpeed.ToString();
            data["Beehive"]["maximumHoneyPerBeehive"] = valheimPlusConfiguration.maximumHoneyPerBeehive.ToString();

            // Building
            data["Building"]["enabled"] = valheimPlusConfiguration.buildingSettingsEnabled.ToString().ToLower();
            data["Building"]["maximumPlacementDistance"] = valheimPlusConfiguration.maximumPlacementDistance.ToString().ToLower();
            data["Building"]["noWeatherDamage"] = valheimPlusConfiguration.noWeatherDamage.ToString();
            data["Building"]["noInvalidPlacementRestriction"] = valheimPlusConfiguration.noInvalidPlacementRestriction.ToString();

            // Items
            data["Items"]["enabled"] = valheimPlusConfiguration.itemsSettingsEnabled.ToString().ToLower();
            data["Items"]["noTeleportPrevention"] = valheimPlusConfiguration.noTeleportPrevention.ToString().ToLower();
            data["Items"]["baseItemWeightReduction"] = valheimPlusConfiguration.baseItemWeightReduction.ToString();
            data["Items"]["itemStackMultiplier"] = valheimPlusConfiguration.itemStackMultiplier.ToString();

            // Fermenter
            data["Fermenter"]["enabled"] = valheimPlusConfiguration.fermenterSettingsEnabled.ToString().ToLower();
            data["Fermenter"]["fermenterDuration"] = valheimPlusConfiguration.fermenterDuration.ToString();
            data["Fermenter"]["fermenterItemsProduced"] = valheimPlusConfiguration.fermenterItemsProduced.ToString();

            // Fireplace
            data["Fireplace"]["enabled"] = valheimPlusConfiguration.fireplaceSettingsEnabled.ToString().ToLower();
            data["Fireplace"]["onlyTorches"] = valheimPlusConfiguration.onlyTorches.ToString().ToLower();

            // Food
            data["Food"]["enabled"] = valheimPlusConfiguration.foodSettingsEnabled.ToString().ToLower();
            data["Food"]["foodDurationMultiplier"] = valheimPlusConfiguration.foodDurationMultiplier.ToString();

            // Furnace
            data["Furnace"]["enabled"] = valheimPlusConfiguration.furnaceSettingsEnabled.ToString().ToLower();
            data["Furnace"]["maximumOre"] = valheimPlusConfiguration.maximumOre.ToString();
            data["Furnace"]["maximumCoal"] = valheimPlusConfiguration.maximumCoal.ToString();
            data["Furnace"]["coalUsedPerProduct"] = valheimPlusConfiguration.coalUsedPerProduct.ToString();
            data["Furnace"]["productionSpeed"] = valheimPlusConfiguration.furnaceProductionSpeed.ToString();

            // Game
            data["Game"]["enabled"] = valheimPlusConfiguration.gameSettingsEnabled.ToString().ToLower();
            data["Game"]["gameDifficultyDamageScale"] = valheimPlusConfiguration.gameDifficultyDamageScale.ToString();
            data["Game"]["gameDifficultyHealthScale"] = valheimPlusConfiguration.gameDifficultyHealthScale.ToString();
            data["Game"]["extraPlayerCountNearby"] = valheimPlusConfiguration.extraPlayerCountNearby.ToString();
            data["Game"]["setFixedPlayerCountTo"] = valheimPlusConfiguration.setFixedPlayerCountTo.ToString();
            data["Game"]["difficultyScaleRange"] = valheimPlusConfiguration.difficultyScaleRange.ToString();

            // Hotkeys
            data["Hotkeys"]["enabled"] = valheimPlusConfiguration.hotkeysSettingsEnabled.ToString().ToLower();
            data["Hotkeys"]["rollForwards"] = valheimPlusConfiguration.rollForwards.ToString().ToLower();
            data["Hotkeys"]["rollBackwards"] = valheimPlusConfiguration.rollBackwards.ToString().ToLower();

            // Hud
            data["Hud"]["enabled"] = valheimPlusConfiguration.hudSettingsEnabled.ToString().ToLower();
            data["Hud"]["showRequiredItems"] = valheimPlusConfiguration.showRequiredItems.ToString().ToLower();
            data["Hud"]["experienceGainedNotifications"] = valheimPlusConfiguration.experienceGainedNotifications.ToString().ToLower();

            // Kiln
            data["Kiln"]["enabled"] = valheimPlusConfiguration.kilnSettingsEnabled.ToString().ToLower();
            data["Kiln"]["maximumWood"] = valheimPlusConfiguration.maximumWood.ToString();
            data["Kiln"]["productionSpeed"] = valheimPlusConfiguration.kilnProductionSpeed.ToString();

            // Map
            data["Map"]["enabled"] = valheimPlusConfiguration.mapSettingsEnabled.ToString().ToLower();
            data["Map"]["shareMapProgression"] = valheimPlusConfiguration.shareMapProgression.ToString().ToLower();
            data["Map"]["exploreRadius"] = valheimPlusConfiguration.exploreRadius.ToString();
            data["Map"]["playerPositionPublicOnJoin"] = valheimPlusConfiguration.playerPositionPublicOnJoin.ToString().ToLower();
            data["Map"]["preventPlayerFromTurningOffPublicPosition"] = valheimPlusConfiguration.preventPlayerFromTurningOffPublicPosition.ToString().ToLower();
            data["Map"]["removeDeathPinOnTombstoneEmpty"] = valheimPlusConfiguration.removeDeathPinOnTombstoneEmpty.ToString().ToLower();

            // Player
            data["Player"]["enabled"] = valheimPlusConfiguration.playerSettingsEnabled.ToString().ToLower();
            data["Player"]["baseMaximumWeight"] = valheimPlusConfiguration.baseMaximumWeight.ToString();
            data["Player"]["baseMegingjordBuff"] = valheimPlusConfiguration.baseMegingjordBuff.ToString();
            data["Player"]["baseAutoPickUpRange"] = valheimPlusConfiguration.baseAutoPickUpRange.ToString();
            data["Player"]["disableCameraShake"] = valheimPlusConfiguration.disableCameraShake.ToString().ToLower();
            data["Player"]["baseUnarmedDamage"] = valheimPlusConfiguration.baseUnarmedDamage.ToString();

            // Server
            data["Server"]["enabled"] = valheimPlusConfiguration.serverSettingsEnabled.ToString().ToLower();
            data["Server"]["maxPlayers"] = valheimPlusConfiguration.maxPlayers.ToString();
            data["Server"]["disableServerPassword"] = valheimPlusConfiguration.disableServerPassword.ToString().ToLower();
            data["Server"]["enforceMod"] = valheimPlusConfiguration.enforceMod.ToString().ToLower();
            data["Server"]["dataRate"] = valheimPlusConfiguration.dataRate.ToString();
            data["Server"]["autoSaveInterval"] = valheimPlusConfiguration.autoSaveInterval.ToString();

            // Stamina
            data["Stamina"]["enabled"] = valheimPlusConfiguration.staminaSettingsEnabled.ToString().ToLower();
            data["Stamina"]["dodgeStaminaUsage"] = valheimPlusConfiguration.dodgeStaminaUsage.ToString();
            data["Stamina"]["encumberedStaminaDrain"] = valheimPlusConfiguration.encumberedStaminaDrain.ToString();
            data["Stamina"]["jumpStaminaDrain"] = valheimPlusConfiguration.jumpStaminaDrain.ToString();
            data["Stamina"]["runStaminaDrain"] = valheimPlusConfiguration.runStaminaDrain.ToString();
            data["Stamina"]["sneakStaminaDrain"] = valheimPlusConfiguration.sneakStaminaDrain.ToString();
            data["Stamina"]["staminaRegen"] = valheimPlusConfiguration.staminaRegen.ToString();
            data["Stamina"]["staminaRegenDelay"] = valheimPlusConfiguration.staminaRegenDelay.ToString();
            data["Stamina"]["swimStaminaDrain"] = valheimPlusConfiguration.swimStaminaDrain.ToString();

            // StaminaUsage
            data["StaminaUsage"]["enabled"] = valheimPlusConfiguration.staminaUsageSettingsEnabled.ToString().ToLower();
            data["StaminaUsage"]["axes"] = valheimPlusConfiguration.axes.ToString();
            data["StaminaUsage"]["bows"] = valheimPlusConfiguration.bows.ToString();
            data["StaminaUsage"]["clubs"] = valheimPlusConfiguration.clubs.ToString();
            data["StaminaUsage"]["knives"] = valheimPlusConfiguration.knives.ToString();
            data["StaminaUsage"]["pickaxes"] = valheimPlusConfiguration.pickaxes.ToString();
            data["StaminaUsage"]["polearms"] = valheimPlusConfiguration.polearms.ToString();
            data["StaminaUsage"]["spears"] = valheimPlusConfiguration.spears.ToString();
            data["StaminaUsage"]["swords"] = valheimPlusConfiguration.swords.ToString();
            data["StaminaUsage"]["unarmed"] = valheimPlusConfiguration.unarmed.ToString();
            data["StaminaUsage"]["hammer"] = valheimPlusConfiguration.hammer.ToString();
            data["StaminaUsage"]["hoe"] = valheimPlusConfiguration.hoe.ToString();

            // Workbench
            data["Workbench"]["enabled"] = valheimPlusConfiguration.workbenchSettingsEnabled.ToString().ToLower();
            data["Workbench"]["workbenchRange"] = valheimPlusConfiguration.workbenchRange.ToString();
            data["Workbench"]["disableRoofCheck"] = valheimPlusConfiguration.disableRoofCheck.ToString().ToLower();

            // Time
            data["Time"]["enabled"] = valheimPlusConfiguration.timeSettingsEnabled.ToString().ToLower();
            data["Time"]["totalDayTimeInSeconds"] = valheimPlusConfiguration.totalDayTimeInSeconds.ToString();
            data["Time"]["nightTimeSpeedMultiplier"] = valheimPlusConfiguration.nightTimeSpeedMultiplier.ToString();

            // Ward
            data["Ward"]["enabled"] = valheimPlusConfiguration.wardSettingsEnabled.ToString().ToLower();
            data["Ward"]["wardRange"] = valheimPlusConfiguration.wardRange.ToString().ToLower();

            // StructuralIntegrity
            data["StructuralIntegrity"]["enabled"] = valheimPlusConfiguration.structuralIntegritySettingsEnabled.ToString().ToLower();

            // Experience
            data["Experience"]["enabled"] = valheimPlusConfiguration.experienceSettingsEnabled.ToString().ToLower();
            data["Experience"]["swords"] = valheimPlusConfiguration.experienceSwords.ToString();
            data["Experience"]["knives"] = valheimPlusConfiguration.experienceKnives.ToString();
            data["Experience"]["clubs"] = valheimPlusConfiguration.experienceClubs.ToString();
            data["Experience"]["polearms"] = valheimPlusConfiguration.experiencePolearms.ToString();
            data["Experience"]["spears"] = valheimPlusConfiguration.experienceSpears.ToString();
            data["Experience"]["blocking"] = valheimPlusConfiguration.experienceBlocking.ToString();
            data["Experience"]["axes"] = valheimPlusConfiguration.experienceAxes.ToString();
            data["Experience"]["bows"] = valheimPlusConfiguration.experienceBows.ToString();
            data["Experience"]["fireMagic"] = valheimPlusConfiguration.experienceFireMagic.ToString();
            data["Experience"]["frostMagic"] = valheimPlusConfiguration.experienceFrostMagic.ToString();
            data["Experience"]["unarmed"] = valheimPlusConfiguration.experienceUnarmed.ToString();
            data["Experience"]["pickaxes"] = valheimPlusConfiguration.experiencePickaxes.ToString();
            data["Experience"]["woodCutting"] = valheimPlusConfiguration.experienceWoodCutting.ToString();
            data["Experience"]["jump"] = valheimPlusConfiguration.experienceJump.ToString();
            data["Experience"]["sneak"] = valheimPlusConfiguration.experienceSneak.ToString();
            data["Experience"]["run"] = valheimPlusConfiguration.experienceRun.ToString();
            data["Experience"]["swim"] = valheimPlusConfiguration.experienceSwim.ToString();

            // Camera
            data["Camera"]["enabled"] = valheimPlusConfiguration.cameraSettingsEnabled.ToString().ToLower();
            data["Camera"]["cameraMaximumZoomDistance"] = valheimPlusConfiguration.cameraMaximumZoomDistance.ToString().ToLower();
            data["Camera"]["cameraBoatMaximumZoomDistance"] = valheimPlusConfiguration.cameraBoatMaximumZoomDistance.ToString().ToLower();
            data["Camera"]["cameraFOV"] = valheimPlusConfiguration.cameraFOV.ToString().ToLower();

            // Wagon
            data["Wagon"]["enabled"] = valheimPlusConfiguration.wagonSettingsEnabled.ToString().ToLower();
            data["Wagon"]["wagonBaseMass"] = valheimPlusConfiguration.wagonBaseMass.ToString();
            data["Wagon"]["wagonExtraMassFromItems"] = valheimPlusConfiguration.wagonExtraMassFromItems.ToString();

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

        private ConfigManager()
        {
        }
        private static ConfigManager instance = null;
        public static ConfigManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConfigManager();
                }
                return instance;
            }
        }
    }
}
