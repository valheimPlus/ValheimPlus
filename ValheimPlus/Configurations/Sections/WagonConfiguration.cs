namespace ValheimPlus.Configurations.Sections
{
    public class WagonConfiguration : ServerSyncConfig<WagonConfiguration>
    {
        public float wagonExtraMassFromItems { get; internal set; } = 0;
        public float wagonBaseMass { get; internal set; } = 20;
    }
}
