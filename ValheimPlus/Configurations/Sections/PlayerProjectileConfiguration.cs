namespace ValheimPlus.Configurations.Sections
{
    public class PlayerProjectileConfiguration : ServerSyncConfig<PlayerProjectileConfiguration>
    {
        public float playerMinChargeVelocityMultiplier { get; internal set; } = 0;
        public float playerMaxChargeVelocityMultiplier { get; internal set; } = 0;

        public float playerMinChargeAccuracyMultiplier { get; internal set; } = 0;
        public float playerMaxChargeAccuracyMultiplier { get; internal set; } = 0;

        public bool enableScaleWithSkillLevel { get; internal set; } = false;
    }
}