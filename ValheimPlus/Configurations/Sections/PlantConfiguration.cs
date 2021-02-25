namespace ValheimPlus.Configurations.Sections
{
    public class PlantConfiguration : ServerSyncConfig<PlantConfiguration>
    {
        public bool noWrongBiome { get; set; } = false;

        public bool growWithoutSun { get; set; } = false;
    }
}