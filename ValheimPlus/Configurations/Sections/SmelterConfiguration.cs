namespace ValheimPlus.Configurations.Sections
{
    public class SmelterConfiguration : ServerSyncConfig<SmelterConfiguration>
    {
        public int maximumOre { get; set; } = 10;
        public int maximumCoal { get; set; } = 20;
        public int coalUsedPerProduct { get; set; } = 2;
        public float productionSpeed { get; set; } = 30;
        public bool autoDeposit { get; set; } = false;
        public bool autoFuel { get; set; } = false;
        public bool ignorePrivateAreaCheck { get; internal set; } = true;
        public float autoRange { get; set; } = 10;
    }

}
