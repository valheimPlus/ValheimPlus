namespace ValheimPlus.Configurations.Sections
{
    public class InventoryConfiguration : ServerSyncConfig<InventoryConfiguration>
    {
        public bool inventoryFillTopToBottom { get; set; } = false;
        public int playerInventoryRows { get; set; } = 4;
        public int woodChestColumns { get; set; } = 5;
        public int woodChestRows { get; set; } = 2;
        public int ironChestColumns { get; set; } = 8;
        public int ironChestRows { get; set; } = 3;
    }
}
