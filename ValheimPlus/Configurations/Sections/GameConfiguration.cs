// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Game-specific settings")]
    public class GameConfiguration : ServerSyncConfig<GameConfiguration>
    {
        [Configuration("The games damage multiplier per person nearby in difficultyScaleRange(m) radius.", ActivationTime.Immediately)]
        public float gameDifficultyDamageScale { get; internal set; } = 0.4f;

        [Configuration("The games health multiplier per person nearby in difficultyScaleRange(m) radius.", ActivationTime.Immediately)]
        public float gameDifficultyHealthScale { get; internal set; } = 0.4f;

        [Configuration("Adds additional players to the difficulty calculation in multiplayer unrelated to the actual amount", ActivationTime.Immediately)]
        public int extraPlayerCountNearby { get; internal set; } = 0;

        [Configuration("Sets the nearby player count always to this value + extraPlayerCountNearby", ActivationTime.Immediately)]
        public int setFixedPlayerCountTo { get; internal set; } = 0;

        [Configuration("The range in meters at which other players count towards nearby players for the difficulty scale", ActivationTime.Immediately)]
        public int difficultyScaleRange { get; internal set; } = 200;
    }
}