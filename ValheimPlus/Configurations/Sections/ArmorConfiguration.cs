namespace ValheimPlus.Configurations.Sections
{
    public class ArmorConfiguration : ServerSyncConfig<ArmorConfiguration>
    {
        public float helmets { get; internal set; } = 0;
        public float chests { get; internal set; } = 0;
        public float legs { get; internal set; } = 0;
        public float capes { get; internal set; } = 0;
    }
}
