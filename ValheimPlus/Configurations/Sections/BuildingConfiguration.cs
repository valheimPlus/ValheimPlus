namespace ValheimPlus.Configurations.Sections
{
    public class BuildingConfiguration : ServerSyncConfig<BuildingConfiguration>
    {
        public bool noInvalidPlacementRestriction { get; set; } = false;
        public bool noWeatherDamage { get; set; } = false;
        public float maximumPlacementDistance { get; internal set; } = 5;

    }

}
