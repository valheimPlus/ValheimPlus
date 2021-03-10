namespace ValheimPlus.Configurations.Sections
{
    public class BeehiveConfiguration : ServerSyncConfig<BeehiveConfiguration>
    {
        public float honeyProductionSpeed { get; set; } = 1200;
        public int maximumHoneyPerBeehive { get; set; } = 4;
        public bool autoDeposit { get; set; } = false;
        public float autoDepositRange { get; set; } = 10;
        public bool showDuration { get; set; } = false;
    }

}
