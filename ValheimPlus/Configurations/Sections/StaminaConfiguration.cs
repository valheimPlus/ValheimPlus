// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Stamina settings")]
    public class StaminaConfiguration : ServerSyncConfig<StaminaConfiguration>
    {
        [Configuration("Changes the flat amount of stamina cost of using the dodge roll")]
        public float dodgeStaminaUsage { get; internal set; } = 10;

        [Configuration("Changes the stamina drain of being overweight")]
        public float encumberedStaminaDrain { get; internal set; } = 10;

        [Configuration("Changes the stamina cost of jumping")]
        public float jumpStaminaDrain { get; internal set; } = 10;

        [Configuration("Changes the stamina cost of running")]
        public float runStaminaDrain { get; internal set; } = 10;

        [Configuration("Changes the stamina drain by sneaking")]
        public float sneakStaminaDrain { get; internal set; } = 10;

        [Configuration("Changes the total amount of stamina recovered per second")]
        public float staminaRegen { get; internal set; } = 5;

        [Configuration("Changes the delay until stamina regeneration sets in")]
        public float staminaRegenDelay { get; internal set; } = 0.5f;

        [Configuration("Changes the stamina drain of swimming")]
        public float swimStaminaDrain { get; internal set; } = 5;
    }
}