namespace ValheimPlus.Configurations.Sections
{
    public class WardConfiguration : ServerSyncConfig<WardConfiguration>
    {
        public float wardRange { get; internal set; } = 20;
        public float wardEnemySpawnRange { get; internal set; } = 0;
    }
}
