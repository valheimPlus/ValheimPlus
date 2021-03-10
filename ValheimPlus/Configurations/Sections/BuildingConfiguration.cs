namespace ValheimPlus.Configurations.Sections
{
    public class BuildingConfiguration : ServerSyncConfig<BuildingConfiguration>
    {
        public bool noInvalidPlacementRestriction { get; set; } = false;
        public bool noWeatherDamage { get; set; } = false;
        public float maximumPlacementDistance { get; internal set; } = 5;
        public bool alwaysDropResources { get; internal set; } = false;
        public bool alwaysDropExcludedResources { get; internal set; } = false;
        public bool enableAreaRepair { get; internal set; } = false;
        public float areaRepairRadius { get; internal set; } = 7.5f;
    }
}
