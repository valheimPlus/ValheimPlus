// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Game-specific settings")]
    public class GameConfiguration : ServerSyncConfig<GameConfiguration>
    {
        [Configuration("The games damage multiplier per person nearby in difficultyScaleRange(m) radius.")]
        public float gameDifficultyDamageScale { get; internal set; } = 0.4f;

        [Configuration("The games health multiplier per person nearby in difficultyScaleRange(m) radius.")]
        public float gameDifficultyHealthScale { get; internal set; } = 0.4f;

        [Configuration("Adds additional players to the difficulty calculation in multiplayer unrelated to the actual amount")]
        public int extraPlayerCountNearby { get; internal set; } = 0;

        [Configuration("Sets the nearby player count always to this value + extraPlayerCountNearby")]
        public int setFixedPlayerCountTo { get; internal set; } = 0;

        [Configuration("The range in meters at which other players count towards nearby players for the difficulty scale")]
        public int difficultyScaleRange { get; internal set; } = 200;
    }
}