// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("HUD settings")]
    public class HudConfiguration : BaseConfig<HudConfiguration>
    {
        [Configuration("Shows the required amount of items AND the amount of items in your inventory in build mode and while crafting.\nNot used yet.",
            ActivationTime.Immediately)]
        public bool showRequiredItems { get; internal set; } = false;

        [Configuration("Shows small notifications about all skill experienced gained in the top left corner.", ActivationTime.Immediately)]
        public bool experienceGainedNotifications { get; internal set; } = false;

        [Configuration("not used yet", ActivationTime.Immediately)]
        public float chatMessageDistance { get; internal set; }
    }
}