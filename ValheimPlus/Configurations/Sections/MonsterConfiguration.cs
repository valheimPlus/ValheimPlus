namespace ValheimPlus.Configurations.Sections
{
    public class MonsterConfiguration : BaseConfig<MonsterConfiguration>
    {
        public float dropDelay { get; internal set; } = 0.01f;
    }
}
