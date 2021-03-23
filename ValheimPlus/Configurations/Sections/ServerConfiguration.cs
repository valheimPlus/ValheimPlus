namespace ValheimPlus.Configurations.Sections
{
    public class ServerConfiguration : BaseConfig<ServerConfiguration>
    {
        public int maxPlayers { get; set; } = 10;
        public bool disableServerPassword { get; set; } = false;
        public bool enforceMod { get; internal set; } = true;
        public bool serverSyncsConfig { get; internal set; } = true;
        public bool serverSyncHotkeys { get; internal set; } = true;
    }

}
