namespace ValheimPlus.Configurations.Sections
{
    public class StructuralIntegrityConfiguration : ServerSyncConfig<StructuralIntegrityConfiguration>
    {
        public float wood { get; internal set; } = 0;
        public float stone { get; internal set; } = 0;
        public float iron { get; internal set; } = 0;
        public float hardWood { get; internal set; } = 0;
        public float marble { get; internal set; } = 0;
        public bool disableStructuralIntegrity { get; internal set; } = false;
        public bool disableDamageToPlayerStructures { get; internal set; } = false;
        public bool disableDamageToPlayerBoats { get; internal set; } = false;
        public bool disableDamageToPlayerCarts { get; internal set; } = false;
        public bool disableWaterDamageToPlayerBoats { get; internal set; } = false;
        public bool disableWaterDamageToPlayerCarts { get; internal set; } = false;
    }
}
