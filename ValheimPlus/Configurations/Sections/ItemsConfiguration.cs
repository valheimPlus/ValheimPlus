// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Item settings")]
    public class ItemsConfiguration : ServerSyncConfig<ItemsConfiguration>
    {
        [Configuration("Enables you to teleport with ores and other usually restricted objects")]
        public bool noTeleportPrevention { get; set; } = false;

        [Configuration("Increase or reduce item weight by % percent. The value -50 will reduce item weight of every object by 50%.")]
        public float baseItemWeightReduction { get; set; } = 0;

        [Configuration("Increase the size of all item stacks by %. The value 50 would set a usual item stack of 100 to be 150.")]
        public float itemStackMultiplier { get; internal set; } = 1;
    }
}