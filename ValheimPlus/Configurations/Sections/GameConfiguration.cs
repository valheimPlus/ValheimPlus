namespace ValheimPlus.Configurations.Sections
{
    public class GameConfiguration : BaseConfig<GameConfiguration>
    {
        public float gameDifficultyDamageScale { get; internal set; } = 0.4f;
        public float gameDifficultyHealthScale { get; internal set; } = 0.4f;
        public int extraPlayerCountNearby { get; internal set; } = 0;
        public int setFixedPlayerCountTo { get; internal set; } = 0;
        public float autoSaveInterval { get; internal set; } = 1200;
        public int difficultyScaleRange { get; internal set; } = 200;
    }
}
