namespace ValheimPlus.Configurations.Sections
{
    public class StructuralIntegrityConfiguration : ServerSyncConfig<StructuralIntegrityConfiguration>
    {
        public float wood { get; internal set; } = 0;
        public float stone { get; internal set; } = 0;
        public float iron { get; internal set; } = 0;
        public float hardWood { get; internal set; } = 0;
        public bool disableStructuralIntegrity { get; set; } = false;
        public bool disableDamageToPlayerStructures { get; set; } = false;
        public bool disableDamageToPlayerBoats { get; set; } = false;
    }
}
