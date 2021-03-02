namespace ValheimPlus.Configurations.Sections
{
    public class FurnaceConfiguration : ServerSyncConfig<FurnaceConfiguration>
    {
        public int maximumOre { get; set; } = 10;
        public int maximumCoal { get; set; } = 20;
        public int coalUsedPerProduct { get; set; } = 2;
        public float productionSpeed { get; set; } = 30;
        public bool autoDeposit { get; set; } = false;
        public float autoDepositRange { get; set; } = 10;
    }

}
