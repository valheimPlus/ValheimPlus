namespace ValheimPlus.Configurations.Sections
{
    public class MapConfiguration : BaseConfig<MapConfiguration>
    {
        public bool ShareMapProgression { get; set; } = false;
        public float ExploreRadius { get; set; } = 100;
        public bool PlayerPositionPublicOnJoin { get; set; } = false;
        public bool PreventPlayerFromTurningOffPublicPosition { get; set; } = false;
    }
}
