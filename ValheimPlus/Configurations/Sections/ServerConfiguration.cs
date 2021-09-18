namespace ValheimPlus.Configurations.Sections
{
    public class ServerConfiguration : BaseConfig<ServerConfiguration>
    {
        public int maxPlayers { get; internal set; } = 10;
        public bool disableServerPassword { get; internal set; } = false;
        public bool enforceMod { get; internal set; } = true;
        public bool serverSyncsConfig { get; internal set; } = true;
        public bool serverSyncHotkeys { get; internal set; } = true;
    }

}
