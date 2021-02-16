namespace ValheimPlus.Configurations.Sections
{
    public class ServerConfiguration : BaseConfig<ServerConfiguration>
    {
        public int MaxPlayers { get; set; } = 10;
        public bool DisableServerPassword { get; set; } = false;
    }

}
