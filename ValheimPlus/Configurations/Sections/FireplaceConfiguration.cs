// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("If changed to enabled all fireplaces do not need to be refilled.")]
    public class FireplaceConfiguration : ServerSyncConfig<FireplaceConfiguration>
    {
        [Configuration("If you enable this option only placed torches do not need to be refilled.", ActivationTime.Immediately)]
        public bool onlyTorches { get; internal set; } = false;
    }
}