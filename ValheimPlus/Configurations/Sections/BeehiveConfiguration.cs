namespace ValheimPlus.Configurations.Sections
{
    public class BeehiveConfiguration : BaseConfig<BeehiveConfiguration>
    {
        public float HoneyProductionSpeed { get; set; } = 10;
        public int MaximumHoneyPerBeehive { get; set; } = 4;
    }

}
