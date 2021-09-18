namespace ValheimPlus.Configurations.Sections
{
    public class BeehiveConfiguration : ServerSyncConfig<BeehiveConfiguration>
    {
        public float honeyProductionSpeed { get; internal set; } = 1200;
        public int maximumHoneyPerBeehive { get; internal set; } = 4;
        public bool autoDeposit { get; internal set; } = false;
        public float autoDepositRange { get; internal set; } = 10;
        public bool showDuration { get; internal set; } = false;
    }

}
