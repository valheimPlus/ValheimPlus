// ValheimPlus

using System;

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Settings for building's structural integrity")]
    public class StructuralIntegrityConfiguration : ServerSyncConfig<StructuralIntegrityConfiguration>
    {
        [Configuration("Reduce the loss of structural integrity by % less. The value 100 would result in disabled structural integrity and allow placement in free air.", ActivationTime.Immediately)]
        public float wood { get; internal set; } = 0;

        [Configuration("Reduce the loss of structural integrity by % less. The value 100 would result in disabled structural integrity and allow placement in free air.", ActivationTime.Immediately)]
        public float stone { get; internal set; } = 0;
        [Configuration("Reduce the loss of structural integrity by % less. The value 100 would result in disabled structural integrity and allow placement in free air.", ActivationTime.Immediately)]
        public float iron { get; internal set; } = 0;
        [Configuration("Reduce the loss of structural integrity by % less. The value 100 would result in disabled structural integrity and allow placement in free air.", ActivationTime.Immediately)]
        public float hardWood { get; internal set; } = 0;

        [Configuration("Disable structural integrity completely", ActivationTime.Immediately)]
        public bool disableStructuralIntegrity { get; set; } = false;
    }
}