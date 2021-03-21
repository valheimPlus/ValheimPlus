namespace ValheimPlus.Configurations.Sections
{
    public class KilnConfiguration : ServerSyncConfig<KilnConfiguration>
    {
        public float productionSpeed { get; set; } = 10;
        public int maximumWood { get; internal set; } = 25;
        public bool autoDeposit { get; set; } = false;
        public bool autoFuel { get; set; } = false;
        public bool ignorePrivateAreaCheck { get; internal set; } = true;
        public float autoRange { get; set; } = 10;
    }

}
