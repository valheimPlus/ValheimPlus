namespace ValheimPlus.Configurations.Sections
{
    public class GameConfiguration : ServerSyncConfig<GameConfiguration>
    {
        public float gameDifficultyDamageScale { get; internal set; } = 4f;
        public float gameDifficultyHealthScale { get; internal set; } = 40f;
        public int extraPlayerCountNearby { get; internal set; } = 0;
        public int setFixedPlayerCountTo { get; internal set; } = 0;
        public int difficultyScaleRange { get; internal set; } = 200;
        public bool disablePortals { get; internal set; } = false;
        public bool forceConsole { get; internal set; } = false;
        public bool bigPortalNames { get; internal set; } = false;
        public bool disableFog { get; internal set; } = false;
    }
}
