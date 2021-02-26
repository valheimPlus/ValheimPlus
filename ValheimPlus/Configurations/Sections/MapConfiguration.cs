// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Map settings")]
    public class MapConfiguration : BaseConfig<MapConfiguration>
    {
        [Configuration("With this enabled you will receive the same exploration progression as other players on the server")]
        public bool shareMapProgression { get; internal set; } = false;

        [Configuration("The radius of the map that you explore when moving")]
        public float exploreRadius { get; internal set; } = 100;

        [Configuration("Automatically turn on the Map option to share your position when joining or starting a game")]
        public bool playerPositionPublicOnJoin { get; internal set; } = false;

        [Configuration("Prevents you and other people on the server to turn off their map sharing option")]
        public bool preventPlayerFromTurningOffPublicPosition { get; internal set; } = false;

        [Configuration("Remove the Map marker of your death when you have picked up your tombstone items")]
        public bool removeDeathPinOnTombstoneEmpty { get; internal set; } = false;
    }
}