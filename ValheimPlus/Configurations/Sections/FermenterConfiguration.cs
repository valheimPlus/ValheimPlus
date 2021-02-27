// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Fermenter settings")]
    public class FermenterConfiguration : ServerSyncConfig<FermenterConfiguration>
    {
        [Configuration("configure the time that the fermenter takes to produce its product, 2400 seconds are 48 ingame hours", ActivationTime.Immediately)]
        public float fermenterDuration { get; set; } = 2400;

        [Configuration("configure the total amount of produced items from a fermenter", ActivationTime.Immediately)]
        public int fermenterItemsProduced { get; set; } = 4;
    }
}