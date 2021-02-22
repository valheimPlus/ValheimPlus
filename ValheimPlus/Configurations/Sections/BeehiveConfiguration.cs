namespace ValheimPlus.Configurations.Sections
{
    public class BeehiveConfiguration : ServerSyncConfig<BeehiveConfiguration>
    {
        public float HoneyProductionSpeed { get; set; } = 10;
        public int MaximumHoneyPerBeehive { get; set; } = 4;
    }

}
