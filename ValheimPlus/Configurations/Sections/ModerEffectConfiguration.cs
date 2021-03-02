namespace ValheimPlus.Configurations.Sections
{
    public class ModerEffectConfiguration : BaseConfig<ModerEffectConfiguration>
    {
        public float cooldown { get; set; } = 1200;
        public float duration { get; set; } = 300;
    }
}
