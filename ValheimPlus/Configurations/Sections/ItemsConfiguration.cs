namespace ValheimPlus.Configurations.Sections
{
    public class ItemsConfiguration : ServerSyncConfig<ItemsConfiguration>
    {
        public bool NoTeleportPrevention { get; set; } = false;
        public float BaseItemWeightReduction { get; set; } = 0;
        public float ItemStackMultiplier { get; internal set; } = 1;
    }

}
