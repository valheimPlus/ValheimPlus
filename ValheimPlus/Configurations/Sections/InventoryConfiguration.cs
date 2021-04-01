namespace ValheimPlus.Configurations.Sections
{
    public class InventoryConfiguration : ServerSyncConfig<InventoryConfiguration>
    {
        public bool inventoryFillTopToBottom { get; internal set; } = false;
        public bool mergeWithExistingStacks { get; internal set; } = false;
        public bool holdAltToAutoSplit { get; internal set; } = false;
        public bool autoMoveByDefault { get; internal set; } = true;
        public int playerInventoryRows { get; internal set; } = 4;
        public int woodChestColumns { get; internal set; } = 5;
        public int woodChestRows { get; internal set; } = 2;
        public int personalChestColumns { get; internal set; } = 3;
        public int personalChestRows { get; internal set; } = 2;
        public int ironChestColumns { get; internal set; } = 8;
        public int ironChestRows { get; internal set; } = 4;
        public int cartInventoryColumns { get; internal set; } = 8;
        public int cartInventoryRows { get; internal set; } = 3;
        public int karveInventoryColumns { get; internal set; } = 2;
        public int karveInventoryRows { get; internal set; } = 2;
        public int longboatInventoryColumns { get; internal set; } = 8;
        public int longboatInventoryRows { get; internal set; } = 3;
    }
}
