namespace ValheimPlusManager.Models
{
    public class ValheimPlusConf
    {
        // Advanced building mode
        public bool advancedBuildingModeEnabled { get; set; } = false;
        public string enterAdvancedBuildingMode { get; set; } = "F1";
        public string exitAdvancedBuildingMode { get; set; } = "F3";

        // Advanced editing mode
        public bool advancedEditingModeEnabled { get; set; } = false;
        public string enterAdvancedEditingMode { get; set; } = "NumPad0";
        public string resetAdvancedEditingMode { get; set; } = "F7";
        public string abortAndExitAdvancedEditingMode { get; set; } = "F8";
        public string confirmPlacementOfAdvancedEditingMode { get; set; } = "KeypadEnter";

        // Beehive
        public bool beehiveSettingsEnabled { get; set; } = false;
        public float honeyProductionSpeed { get; set; } = 1200;
        public int maximumHoneyPerBeehive { get; set; } = 4;

        // Building
        public bool buildingSettingsEnabled { get; set; } = false;
        public bool noInvalidPlacementRestriction { get; set; } = false;
        public bool noWeatherDamage { get; set; } = false;
        public float maximumPlacementDistance { get; set; } = 5;

        // Items
        public bool itemsSettingsEnabled { get; set; } = false;
        public bool noTeleportPrevention { get; set; } = false;
        public float baseItemWeightReduction { get; set; } = 0;
        public float itemStackMultiplier { get; set; } = 1;

        // Fermenter
        public bool fermenterSettingsEnabled { get; set; } = false;
        public float fermenterDuration { get; set; } = 2400;
        public int fermenterItemsProduced { get; set; } = 4;

        // Fireplace
        public bool fireplaceSettingsEnabled { get; set; } = false;
        public bool onlyTorches { get; set; } = false;

        // Food
        public bool foodSettingsEnabled { get; set; } = false;
        public float foodDurationMultiplier { get; set; } = 0;

        // Furnace
        public bool furnaceSettingsEnabled { get; set; } = false;
        public int maximumOre { get; set; } = 10;
        public int maximumCoal { get; set; } = 20;
        public int coalUsedPerProduct { get; set; } = 2;
        public float furnaceProductionSpeed { get; set; } = 10;

        // Game
        public bool gameSettingsEnabled { get; set; } = false;
        public float gameDifficultyDamageScale { get; set; } = 0.4f;
        public float gameDifficultyHealthScale { get; set; } = 0.4f;
        public int extraPlayerCountNearby { get; set; } = 0;
        public int setFixedPlayerCountTo { get; set; } = 0;
        public float autoSaveInterval { get; set; } = 1200;
        public int difficultyScaleRange { get; set; } = 200;

        // Hotkeys
        public bool hotkeysSettingsEnabled { get; set; } = false;
        public string rollForwards { get; set; } = "F9";
        public string rollBackwards { get; set; } = "F10";

        // Hud
        public bool hudSettingsEnabled { get; set; } = false;
        public bool showRequiredItems { get; set; } = false;
        public bool experienceGainedNotifications { get; set; } = false;

        // Kiln
        public bool kilnSettingsEnabled { get; set; } = false;
        public float kilnProductionSpeed { get; set; } = 30;
        public int maximumWood { get; set; } = 25;

        // Map
        public bool mapSettingsEnabled { get; set; } = false;
        public bool shareMapProgression { get; set; } = false;
        public float exploreRadius { get; set; } = 100;
        public bool playerPositionPublicOnJoin { get; set; } = false;
        public bool preventPlayerFromTurningOffPublicPosition { get; set; } = false;
        public bool removeDeathPinOnTombstoneEmpty { get; set; } = false;

        // Player settings
        public bool playerSettingsEnabled { get; set; } = false;
        public float baseMaximumWeight { get; set; } = 300;
        public float baseMegingjordBuff { get; set; } = 150;
        public float baseAutoPickUpRange { get; set; } = 2;
        public bool disableCameraShake { get; set; } = false;
        public float baseUnarmedDamage { get; set; } = 0;

        // Server
        public bool serverSettingsEnabled { get; set; } = false;
        public int maxPlayers { get; set; } = 10;
        public bool disableServerPassword { get; set; } = false;
        public bool enforceConfiguration { get; set; } = true;
        public bool enforceMod { get; set; } = true;
        public int dataRate { get; set; } = (60 * 1024); // 614440 == 60kbs

        // Stamina
        public bool staminaSettingsEnabled { get; set; } = false;
        public float dodgeStaminaUsage { get; set; } = 10;
        public float encumberedStaminaDrain { get; set; } = 10;
        public float jumpStaminaDrain { get; set; } = 10;
        public float runStaminaDrain { get; set; } = 10;
        public float sneakStaminaDrain { get; set; } = 10;
        public float staminaRegen { get; set; } = 5;
        public float staminaRegenDelay { get; set; } = 0.5f;
        public float swimStaminaDrain { get; set; } = 5;

        // Stamina usage
        public bool staminaUsageSettingsEnabled { get; set; } = false;
        public float axes { get; set; } = 0;
        public float bows { get; set; } = 0;
        public float clubs { get; set; } = 0;
        public float knives { get; set; } = 0;
        public float pickaxes { get; set; } = 0;
        public float polearms { get; set; } = 0;
        public float spears { get; set; } = 0;
        public float swords { get; set; } = 0;
        public float unarmed { get; set; } = 0;
        public float hammer { get; set; } = 0;
        public float hoe { get; set; } = 0;

        // Workbench
        public bool workbenchSettingsEnabled { get; set; } = false;
        public float workbenchRange { get; set; } = 20;
        public bool disableRoofCheck { get; set; } = false;

        // Time
        public bool timeSettingsEnabled { get; set; } = false;
        public float totalDayTimeInSeconds { get; set; } = 1200;
        public float nightTimeSpeedMultiplier { get; set; } = 0;

        // Ward
        public bool wardSettingsEnabled { get; set; } = false;
        public float wardRange { get; set; } = 20;

        // Structural integrity
        public bool structuralIntegritySettingsEnabled { get; set; } = false;
        public float wood { get; set; } = 0;
        public float stone { get; set; } = 0;
        public float iron { get; set; } = 0;
        public float hardWood { get; set; } = 0;
        public bool disableStructuralIntegrity { get; set; } = false;

        // Experience
        public bool experienceSettingsEnabled { get; set; } = false;
        public float experienceSwords { get; set; } = 0;
        public float experienceKnives { get; set; } = 0;
        public float experienceClubs { get; set; } = 0;
        public float experiencePolearms { get; set; } = 0;
        public float experienceSpears { get; set; } = 0;
        public float experienceBlocking { get; set; } = 0;
        public float experienceAxes { get; set; } = 0;
        public float experienceBows { get; set; } = 0;
        public float experienceFireMagic { get; set; } = 0;
        public float experienceFrostMagic { get; set; } = 0;
        public float experienceUnarmed { get; set; } = 0;
        public float experiencePickaxes { get; set; } = 0;
        public float experienceWoodCutting { get; set; } = 0;
        public float experienceJump { get; set; } = 0;
        public float experienceSneak { get; set; } = 0;
        public float experienceRun { get; set; } = 0;
        public float experienceSwim { get; set; } = 0;

        // Camera
        public bool cameraSettingsEnabled { get; set; } = false;
        public float cameraMaximumZoomDistance { get; set; } = 1146;
        public float cameraBoatMaximumZoomDistance { get; set; } = 6;
        public float cameraFOV { get; set; } = 85;

        // Wagon
        public bool wagonSettingsEnabled { get; set; } = false;
        public float wagonExtraMassFromItems { get; set; } = 0;
        public float wagonBaseMass { get; set; } = 20;
    }
}
