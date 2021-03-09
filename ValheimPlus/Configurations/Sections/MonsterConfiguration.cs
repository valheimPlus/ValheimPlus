namespace ValheimPlus.Configurations.Sections
{
    public class MonsterConfiguration : ServerSyncConfig<MonsterConfiguration>
    {
        public float dropDelay { get; internal set; } = 0.01f;
    }
}
