namespace ValheimPlus.Configurations.Sections
{
    public class FireplaceConfiguration : ServerSyncConfig<FireplaceConfiguration>
    {
        public bool onlyTorches { get; internal set; } = false;
    }
}
