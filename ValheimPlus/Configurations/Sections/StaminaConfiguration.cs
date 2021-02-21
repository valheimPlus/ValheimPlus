namespace ValheimPlus.Configurations.Sections
{
    public class StaminaConfiguration : ServerSyncConfig <StaminaConfiguration>
    {
        public float DodgeStaminaUsage { get; internal set; } = 10;
        public float EncumberedStaminaDrain { get; internal set; } = 10;
        public float JumpStaminaDrain { get; internal set; } = 10;
        public float RunStaminaDrain { get; internal set; } = 10;
        public float SneakStaminaDrain { get; internal set; } = 10;
        public float StaminaRegen { get; internal set; } = 5;
        public float StaminaRegenDelay { get; internal set; } = 0.5f;
        public float SwimStaminaDrain { get; internal set; } = 5;
    }
}
