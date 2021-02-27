// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Furnace settings")]
    public class FurnaceConfiguration : ServerSyncConfig<FurnaceConfiguration>
    {
        [Configuration("Maximum amount of ore in a furnace", ActivationTime.AfterRestartServer)]
        public int maximumOre { get; set; } = 10;

        [Configuration("Maximum amount of coal in a furnace", ActivationTime.AfterRestartServer)]
        public int maximumCoal { get; set; } = 20;

        [Configuration("The total amount of coal used to produce a single smelted ingot.", ActivationTime.AfterRestartServer)]
        public int coalUsedPerProduct { get; set; } = 2;

        [Configuration("The time it takes for the furnace to produce a single ingot in seconds.", ActivationTime.AfterRestartServer)]
        public float productionSpeed { get; set; } = 30;

        [Configuration("not used yet", ActivationTime.Immediately)]
        public bool autoDeposit { get; set; } = false;

        [Configuration("not used yet", ActivationTime.Immediately)]
        public float autoDepositRange { get; set; } = 10;
    }
}