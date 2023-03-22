namespace ValheimPlus.Configurations.Sections
{
    public class EitrRefineryConfiguration : ServerSyncConfig<EitrRefineryConfiguration>
    {
        public int maximumSoftTissue { get; internal set; } = 20;
        public int maximumSap { get; internal set; } = 20;
        public float productionSpeed { get; internal set; } = 40;
        public bool autoDeposit { get; internal set; } = false;
        public bool autoFuel { get; internal set; } = false;
        public bool ignorePrivateAreaCheck { get; internal set; } = true;
        public float autoRange { get; internal set; } = 10;
    }
}