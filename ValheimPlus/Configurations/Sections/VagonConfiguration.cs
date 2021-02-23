namespace ValheimPlus.Configurations.Sections
{
    public class VagonConfiguration : ServerSyncConfig<VagonConfiguration>
    {
        public float wagonItemWeight { get; internal set; } = 0;
        public float wagonBaseMass { get; internal set; } = 20;
    }
}
