namespace ValheimPlus.Configurations.Sections
{
    public class StaminaUsageConfiguration : ServerSyncConfig<StaminaUsageConfiguration>
    {
        public float axes { get; internal set; } = 0; 
        public float bows { get; internal set; } = 0;
        public float clubs { get; internal set; } = 0;
        public float knives { get; internal set; } = 0;
        public float pickaxes { get; internal set; } = 0;
        public float polearms { get; internal set; } = 0;
        public float spears { get; internal set; } = 0; 
        public float swords { get; internal set; } = 0;
        public float unarmed { get; internal set; } = 0;
        public float hammer { get; internal set; } = 0;
        public float hoe { get; internal set; } = 0;
    }
}
