namespace ValheimPlus.Configurations.Sections
{
    public class HudConfiguration : BaseConfig<HudConfiguration>
    {
        public bool showRequiredItems { get; internal set; } = false;
        public bool experienceGainedNotifications { get; internal set; } = false;
        public bool removeDamageFlash { get; internal set; } = false;
        public int displayBowAmmoCounts { get; internal set; } = 0;
    }
}
