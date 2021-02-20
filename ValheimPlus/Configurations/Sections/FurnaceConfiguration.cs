namespace ValheimPlus.Configurations.Sections
{
    public class FurnaceConfiguration : ServerSyncConfig<FurnaceConfiguration>
    {
        public int MaximumOre { get; set; } = 10;
        public int MaximumCoal { get; set; } = 20;
        public int CoalUsedPerProduct { get; set; } = 2;
        public float ProductionSpeed { get; set; } = 10;
    }

}
