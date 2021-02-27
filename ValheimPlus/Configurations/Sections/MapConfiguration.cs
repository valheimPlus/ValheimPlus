// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Map settings")]
    public class MapConfiguration : BaseConfig<MapConfiguration>
    {
        [Configuration("Remove the Map marker of your death when you have picked up your tombstone items\nNot used yet", ActivationTime.Immediately)]
        public bool removeDeathPinOnTombstoneEmpty { get; set; } = false;

        [Configuration("Show portals automatically on map\nIf it does not activate automatically, just rename a existing portal.", ActivationTime.AfterRelog)]
        public bool showPortalsOnMap { get; set; } = false;

    }
}