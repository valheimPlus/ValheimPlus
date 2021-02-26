// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    public class BeehiveConfiguration : ServerSyncConfig<BeehiveConfiguration>
    {
        [Configuration("configure the speed at which the bees produce honey in seconds, 1200 seconds are 24 ingame hours")]
        public float honeyProductionSpeed { get; set; } = 1200;

        [Configuration("configure the maximum amount of honey in beehives")]
        public int maximumHoneyPerBeehive { get; set; } = 4;
    }
}