namespace ValheimPlus.Configurations.Sections
{
    public class FireplaceConfiguration : ServerSyncConfig<FireplaceConfiguration>
    {
        public bool OnlyTorches { get; internal set; } = false;
    }
}
