namespace ValheimPlus.Configurations.Sections
{
    public class MapConfiguration : BaseConfig<MapConfiguration>
    {
        public bool shareMapProgression { get; internal set; } = false;
        public float exploreRadius { get; internal set; } = 100;
        public bool playerPositionPublicOnJoin { get; internal set; } = false;
        public bool preventPlayerFromTurningOffPublicPosition { get; internal set; } = false;
        public bool removeDeathPinOnTombstoneEmpty { get; internal set; } = false;
    }
}
