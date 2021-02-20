namespace ValheimPlus.Configurations.Sections
{
    public class PlayerConfiguration : ServerSyncConfig<PlayerConfiguration>
    {
        public float BaseMaximumWeight { get; internal set; } = 300;
        public float BaseMegingjordBuff { get; internal set; } = 150;
        public float BaseAutoPickUpRange { get; internal set; } = 2;
        public bool DisableCameraShake { get; internal set; } = false;
        public bool ExperienceGainedNotifications { get; internal set; } = false;
        public float BaseUnarmedDamage { get; internal set; } = 0;
    }
}
