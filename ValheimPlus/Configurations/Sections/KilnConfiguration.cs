// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Kiln settings")]
    public class KilnConfiguration : ServerSyncConfig<KilnConfiguration>
    {
        [Configuration("The time it takes for the Kiln to produce a single piece of coal in seconds.", ActivationTime.AfterRestartServer)]
        public float productionSpeed { get; set; } = 10;

        [Configuration("Maximum amount of wood in a Kiln", ActivationTime.AfterRestartServer)]
        public int maximumWood { get; internal set; } = 25;

        [Configuration("not used yet", ActivationTime.AfterRestartServer)] public bool autoDeposit { get; set; } = false;

        [Configuration("not used yet", ActivationTime.AfterRestartServer)] public float autoDepositRange { get; set; } = 10;
    }
}