namespace ValheimPlus.Configurations.Sections
{
    public class KilnConfiguration : BaseConfig<KilnConfiguration>
    {
        public float ProductionSpeed { get; set; } = 10;
        public int MaximumWood { get; internal set; } = 25;
    }

}
