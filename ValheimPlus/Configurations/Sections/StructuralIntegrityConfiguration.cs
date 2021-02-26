// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Settings for building's structural integrity")]
    public class StructuralIntegrityConfiguration : ServerSyncConfig<StructuralIntegrityConfiguration>
    {
        [Configuration(
            "Each of these values reduce the loss of structural integrity by % less. The value 100 would result in disabled structural integrity and allow placement in free air.")]
        public float wood { get; internal set; } = 0;

        public float stone { get; internal set; } = 0;
        public float iron { get; internal set; } = 0;
        public float hardWood { get; internal set; } = 0;
        public bool disableStructuralIntegrity { get; set; } = false;
    }
}