namespace ValheimPlus.Configurations.Sections
{
    public class ServerConfiguration : BaseConfig<ServerConfiguration>
    {
        public int maxPlayers { get; set; } = 10;
        public bool disableServerPassword { get; set; } = false;
        public bool enforceMod { get; internal set; } = true;
        public bool serverSyncsConfig { get; internal set; } = true;
        public int dataRate { get; internal set; } = 60; // Code that applies this multiplies by 1024, so the default is 60(KB)
    }

}
