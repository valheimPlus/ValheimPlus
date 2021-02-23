namespace ValheimPlus.Configurations.Sections
{
    public class TimeConfiguration : ServerSyncConfig<TimeConfiguration>
    {
        public float totalDayTimeInSeconds { get; internal set; } = 1200;
        public float nightTimeSpeedMultiplier { get; internal set; } = 0;
    }
}
