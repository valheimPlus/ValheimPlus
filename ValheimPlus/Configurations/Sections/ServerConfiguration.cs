namespace ValheimPlus.Configurations.Sections
{
    public class ServerConfiguration : BaseConfig<ServerConfiguration>
    {
        public int maxPlayers { get; set; } = 10;
        public bool disableServerPassword { get; set; } = false;
        public bool enforceConfiguration { get; internal set; } = true;
        public bool enforceMod { get; internal set; } = true;
        public int dataRate { get; internal set; } = (60 * 1024); // 614440 == 60kbs
        public float autoSaveInterval { get; internal set; } = 1200;
    }

}
