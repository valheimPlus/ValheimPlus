namespace ValheimPlus.Configurations.Sections
{
    public class MapConfiguration : BaseConfig<MapConfiguration>
    {
        public bool ShareMapProgression { get; internal set; } = false;
        public float ExploreRadius { get; internal set; } = 100;
        public bool PlayerPositionPublicOnJoin { get; internal set; } = false;
        public bool PreventPlayerFromTurningOffPublicPosition { get; internal set; } = false;
        public bool RemoveDeathPinOnTombstoneEmpty { get; internal set; } = true;
    }
}
