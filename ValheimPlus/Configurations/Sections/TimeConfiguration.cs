// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Day and night length settings")]
    public class TimeConfiguration : ServerSyncConfig<TimeConfiguration>
    {
        [Configuration("Total amount of time one complete day and night circle takes to complete", ActivationTime.AfterRestartServer)]
        public float totalDayTimeInSeconds { get; internal set; } = 1200;

        [Configuration("Increase the speed at which time passes at night by %. The value 50 would result in a 50% reduced amount of night time.", ActivationTime.Immediately)]
        public float nightTimeSpeedMultiplier { get; internal set; } = 0;
    }
}