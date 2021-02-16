namespace ValheimPlus.Configurations.Sections
{
    public class FurnaceConfiguration : BaseConfig<FurnaceConfiguration>
    {
        public int MaximumOre { get; set; } = 10;
        public int MaximumCoal { get; set; } = 10;
        public int CoalUsedPerProduct { get; set; } = 4;
        public float ProductionSpeed { get; set; } = 10;
    }

}
