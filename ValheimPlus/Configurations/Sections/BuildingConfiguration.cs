namespace ValheimPlus.Configurations.Sections
{
    public class BuildingConfiguration : ServerSyncConfig<BuildingConfiguration>
    {
        public bool NoInvalidPlacementRestriction { get; set; } = false;
        public bool NoWeatherDamage { get; set; } = false;
        public float MaximumPlacementDistance { get; internal set; } = 5;
    }

}
