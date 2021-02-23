namespace ValheimPlus.Configurations.Sections
{
    public class BeehiveConfiguration : ServerSyncConfig<BeehiveConfiguration>
    {
        public float honeyProductionSpeed { get; set; } = 10;
        public int maximumHoneyPerBeehive { get; set; } = 4;
    }

}
