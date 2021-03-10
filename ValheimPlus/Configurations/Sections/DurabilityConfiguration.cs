namespace ValheimPlus.Configurations.Sections
{
    public class DurabilityConfiguration : ServerSyncConfig<DurabilityConfiguration>
    {
        public float axes { get; internal set; } = 0;
        public float pickaxes { get; internal set; } = 0;
        public float hammer { get; internal set; } = 0;
        public float cultivator { get; internal set; } = 0;
        public float hoe { get; internal set; } = 0;
        public float weapons { get; internal set; } = 0;
        public float armor { get; internal set; } = 0;
        public float bows { get; internal set; } = 0;
        public float shields { get; internal set; } = 0;
        public float torch { get; internal set; } = 0;
    }
}
