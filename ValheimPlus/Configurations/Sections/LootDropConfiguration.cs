namespace ValheimPlus.Configurations.Sections
{
    public class LootDropConfiguration : ServerSyncConfig<LootDropConfiguration>
    {
        public float lootDropAmountMultiplier { get; internal set; } = 0;
        public float lootDropChanceMultiplier { get; internal set; } = 0;
    }
}
