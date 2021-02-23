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

        // Player settings
        public bool playerSettingsEnabled { get; set; } = false;
        public float baseMaximumWeight { get; set; } = 300;
        public float baseMegingjordBuff { get; set; } = 150;
        public float baseAutoPickUpRange { get; set; } = 2;
        public bool disableCameraShake { get; set; } = false;
        public float baseUnarmedDamage { get; set; } = 0;
    }
}
