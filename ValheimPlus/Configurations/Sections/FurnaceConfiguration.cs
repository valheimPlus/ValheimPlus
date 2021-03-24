namespace ValheimPlus.Configurations.Sections
{
    public class FurnaceConfiguration : ServerSyncConfig<FurnaceConfiguration>
    {
        public int maximumOre { get; internal set; } = 10;
        public int maximumCoal { get; internal set; } = 20;
        public int coalUsedPerProduct { get; internal set; } = 2;
        public float productionSpeed { get; internal set; } = 30;
        public bool autoDeposit { get; internal set; } = false;
        public bool autoFuel { get; internal set; } = false;
        public bool ignorePrivateAreaCheck { get; internal set; } = true;
        public float autoRange { get; internal set; } = 10;
    }

}
