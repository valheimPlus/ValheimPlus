// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Stamina settings")]
    public class StaminaConfiguration : ServerSyncConfig<StaminaConfiguration>
    {
        [Configuration("Changes the flat amount of stamina cost of using the dodge roll", ActivationTime.AfterRestart)]
        public float dodgeStaminaUsage { get; set; } = 10;

        [Configuration("Changes the stamina drain of being overweight", ActivationTime.AfterRestart)]
        public float encumberedStaminaDrain { get; set; } = 10;

        [Configuration("Changes the stamina cost of jumping", ActivationTime.AfterRestart)]
        public float jumpStaminaDrain { get; set; } = 10;

        [Configuration("Changes the stamina cost of running", ActivationTime.AfterRestart)]
        public float runStaminaDrain { get; set; } = 10;

        [Configuration("Changes the stamina drain by sneaking", ActivationTime.AfterRestart)]
        public float sneakStaminaDrain { get; set; } = 10;

        [Configuration("Changes the total amount of stamina recovered per second", ActivationTime.AfterRestart)]
        public float staminaRegen { get; set; } = 5;

        [Configuration("Changes the delay until stamina regeneration sets in", ActivationTime.AfterRestart)]
        public float staminaRegenDelay { get; set; } = 0.5f;

        [Configuration("Changes the stamina drain of swimming", ActivationTime.AfterRestart)]
        public float swimStaminaDrain { get; set; } = 5;
    }
}