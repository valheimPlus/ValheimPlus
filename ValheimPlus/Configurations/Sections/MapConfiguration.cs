namespace ValheimPlus.Configurations.Sections
{
    public class MapConfiguration : ServerSyncConfig<MapConfiguration>
    {
        public bool ShareMapProgression { get; set; } = true;
        public float ExploreRadius { get; set; } = 100;
    }

}
