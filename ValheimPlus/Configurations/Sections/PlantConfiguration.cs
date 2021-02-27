// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    public class PlantConfiguration : ServerSyncConfig<PlantConfiguration>
    {
        [Configuration("Remove biome restrictions for plant growth", ActivationTime.Immediately)]
        public bool noWrongBiome { get; set; } = false;

        [Configuration("Enable indoor growth", ActivationTime.Immediately)]
        public bool growWithoutSun { get; set; } = false;

    }
}