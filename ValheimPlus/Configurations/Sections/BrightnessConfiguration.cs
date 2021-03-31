namespace ValheimPlus.Configurations.Sections
{
    public class BrightnessConfiguration : BaseConfig<BrightnessConfiguration>
    {
        public float morningBrightnessMultiplier { get; set; } = 0f;
        public float dayBrightnessMultiplier { get; set; } = 0f;
        public float eveningBrightnessMultiplier { get; set; } = 0f;
        public float nightBrightnessMultiplier { get; set; } = 0f;
    }
}