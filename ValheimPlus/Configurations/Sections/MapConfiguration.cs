// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Map settings")]
    public class MapConfiguration : BaseConfig<MapConfiguration>
    {
        [Configuration("With this enabled you will receive the same exploration progression as other players on the server", ActivationTime.Immediately)]
        public bool shareMapProgression { get; set; } = false;

        [Configuration("The radius of the map that you explore when moving", ActivationTime.Immediately)]
        public float exploreRadius { get; set; } = 100;

        [Configuration("Automatically turn on the Map option to share your position when joining or starting a game", ActivationTime.AfterRelog)]
        public bool playerPositionPublicOnJoin { get; set; } = false;

        [Configuration("Prevents you and other people on the server to turn off their map sharing option", ActivationTime.Immediately)]
        public bool preventPlayerFromTurningOffPublicPosition { get; set; } = false;

        [Configuration("Remove the Map marker of your death when you have picked up your tombstone items\nNot used yet", ActivationTime.Immediately)]
        public bool removeDeathPinOnTombstoneEmpty { get; set; } = false;

        [Configuration("Show portals automatically on map\nIf it does not activate automatically, just rename a existing portal.", ActivationTime.AfterRelog)]
        public bool showPortalsOnMap { get; set; } = false;

    }
}