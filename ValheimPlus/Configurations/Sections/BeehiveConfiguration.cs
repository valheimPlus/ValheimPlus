// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    public class BeehiveConfiguration : ServerSyncConfig<BeehiveConfiguration>
    {
        // Values will be set, but effective only after they get recreated again by the engine
        // To force this easily just teleport somewhere and come back

        [Configuration("configure the speed at which the bees produce honey in seconds, 1200 seconds are 24 ingame hours", ActivationTime.Immediately)]
        public float honeyProductionSpeed { get; set; } = 1200;

        [Configuration("configure the maximum amount of honey in beehives", ActivationTime.Immediately)]
        public int maximumHoneyPerBeehive { get; set; } = 4;
    }
}