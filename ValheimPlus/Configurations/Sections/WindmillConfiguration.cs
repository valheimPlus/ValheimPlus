namespace ValheimPlus.Configurations.Sections
{
    public class WindmillConfiguration : ServerSyncConfig<WindmillConfiguration>
    {
        public int maximumBarley { get; internal set; } = 50;
        public float productionSpeed { get; internal set; } = 10;
        public bool ignoreWindIntensity { get; internal set; } = false;
        public bool autoDeposit { get; internal set; } = false;
        public bool autoFuel { get; internal set; } = false;
        public bool ignorePrivateAreaCheck { get; internal set; } = true;
        public float autoRange { get; internal set; } = 10;
    }
}
