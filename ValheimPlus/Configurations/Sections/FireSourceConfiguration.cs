namespace ValheimPlus.Configurations.Sections
{
    public class FireSourceConfiguration : ServerSyncConfig<FireSourceConfiguration>
    {
        public bool onlyTorches { get; internal set; } = false;
    }
}
