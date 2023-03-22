namespace ValheimPlus.Configurations.Sections
{
    public class WispSpawnerConfiguration : ServerSyncConfig<WispSpawnerConfiguration>
    {
        public int maximumWisps { get; set; } = 3;
        public bool onlySpawnAtNight { get; internal set; } = true;
        public float wispSpawnIntervalMultiplier { get; internal set; } = 0;
        public float wispSpawnChanceMultiplier { get; internal set; } = 0;
    }
}