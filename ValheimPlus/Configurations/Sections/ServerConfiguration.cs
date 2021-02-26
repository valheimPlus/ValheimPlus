// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Server settings")]
    public class ServerConfiguration : BaseConfig<ServerConfiguration>
    {
        [Configuration("Modify the amount of players on your Server")]
        public int maxPlayers { get; set; } = 10;

        [Configuration("Removes the requirement to have a server password")]
        public bool disableServerPassword { get; set; } = false;

        [Configuration(
            "This settings add a version control check to make sure that people that try to join your game or the server you try to join has V+ installed")]
        public bool enforceMod { get; internal set; } = true;

        [Configuration("The total amount of data that the server and client can send per second in kilobyte")]
        public int dataRate { get; internal set; } = 60; // 614440 == 60kbs

        [Configuration("The interval in seconds that the game auto saves at (client only)")]
        public float autoSaveInterval { get; internal set; } = 1200;
    }
}