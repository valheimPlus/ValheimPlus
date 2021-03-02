namespace ValheimPlus.Configurations.Sections
{
    public class EffectsConfiguration : ServerSyncConfig<EffectsConfiguration>
    {
        public bool debug { get; internal set; } = false;
        public EffectsConfigurationItem cold { get; set; }
        public EffectsConfigurationItem corpseRun { get; set; }
        public EffectsConfigurationItem freezing { get; set; }
        public EffectsConfigurationItem rested { get; set; }
        public EffectsConfigurationItem resting { get; set; }
        public EffectsConfigurationItem warm { get; set; }
        public EffectsConfigurationItem wet { get; set; }

        public EffectsConfigurationItem eikthyr { get; set; }
        public EffectsConfigurationItem theElder { get; set; }
        public EffectsConfigurationItem bonemass { get; set; }
        public EffectsConfigurationItem yagluth { get; set; }
        public EffectsConfigurationItem moder { get; set; }

        public EffectsConfigurationItem frostResistanceMead { get; set; }
        public EffectsConfigurationItem poisonResistanceMead { get; set; }
        public EffectsConfigurationItem trollSetBonus { get; set; }
        public EffectsConfigurationItem wolfItemBonus { get; set; }
    }
}
