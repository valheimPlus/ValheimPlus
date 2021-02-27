// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("")]
    public class StaminaUsageConfiguration : ServerSyncConfig<StaminaUsageConfiguration>
    {
        [Configuration("Reduces the stamina drain by %. The value 50 would result in 50% less stamina cost.", ActivationTime.Immediately)]
        public float axes { get; internal set; } = 0;

        [Configuration("Reduces the stamina drain by %. The value 50 would result in 50% less stamina cost.", ActivationTime.Immediately)]
        public float bows { get; internal set; } = 0;

        [Configuration("Reduces the stamina drain by %. The value 50 would result in 50% less stamina cost.", ActivationTime.Immediately)]
        public float clubs { get; internal set; } = 0;

        [Configuration("Reduces the stamina drain by %. The value 50 would result in 50% less stamina cost.", ActivationTime.Immediately)]
        public float knives { get; internal set; } = 0;

        [Configuration("Reduces the stamina drain by %. The value 50 would result in 50% less stamina cost.", ActivationTime.Immediately)]
        public float pickaxes { get; internal set; } = 0;

        [Configuration("Reduces the stamina drain by %. The value 50 would result in 50% less stamina cost.", ActivationTime.Immediately)]
        public float polearms { get; internal set; } = 0;

        [Configuration("Reduces the stamina drain by %. The value 50 would result in 50% less stamina cost.", ActivationTime.Immediately)]
        public float spears { get; internal set; } = 0;

        [Configuration("Reduces the stamina drain by %. The value 50 would result in 50% less stamina cost.", ActivationTime.Immediately)]
        public float swords { get; internal set; } = 0;

        [Configuration("Reduces the stamina drain by %. The value 50 would result in 50% less stamina cost.", ActivationTime.Immediately)]
        public float unarmed { get; internal set; } = 0;

        [Configuration("Reduces the stamina drain by %. The value 50 would result in 50% less stamina cost.", ActivationTime.Immediately)]
        public float hammer { get; internal set; } = 0;

        [Configuration("Reduces the stamina drain by %. The value 50 would result in 50% less stamina cost.", ActivationTime.Immediately)]
        public float hoe { get; internal set; } = 0;
    }
}