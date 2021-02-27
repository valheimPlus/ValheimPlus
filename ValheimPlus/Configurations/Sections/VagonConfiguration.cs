// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Wagon settings")]
    public class VagonConfiguration : ServerSyncConfig<VagonConfiguration>
    {
        [Configuration("Change the base vagon physical mass of the vagon object", ActivationTime.Immediately)]
        public float wagonExtraMassFromItems { get; internal set; } = 0;

        [Configuration(
            "This value changes the game physical weight of Vagons by +/- more/less from item weight inside.\nThe value 50 would increase the weight by 50% more. The value -100 would remove the entire extra weight.", ActivationTime.Immediately)]
        public float wagonBaseMass { get; internal set; } = 20;
    }
}