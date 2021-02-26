// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Everything experience-related")]
    public class ExperienceConfiguration : ServerSyncConfig<ExperienceConfiguration>
    {
        [Configuration(
            "Each of these values represent the increase to experience gained by % increased. The value 50 would result in 50% increased experience gained for the respective skill by name.")]
        public float swords { get; internal set; } = 0;

        public float knives { get; internal set; } = 0;
        public float clubs { get; internal set; } = 0;
        public float polearms { get; internal set; } = 0;
        public float spears { get; internal set; } = 0;
        public float blocking { get; internal set; } = 0;
        public float axes { get; internal set; } = 0;
        public float bows { get; internal set; } = 0;
        public float fireMagic { get; internal set; } = 0;
        public float frostMagic { get; internal set; } = 0;
        public float unarmed { get; internal set; } = 0;
        public float pickaxes { get; internal set; } = 0;
        public float woodCutting { get; internal set; } = 0;
        public float jump { get; internal set; } = 0;
        public float sneak { get; internal set; } = 0;
        public float run { get; internal set; } = 0;
        public float swim { get; internal set; } = 0;
    }
}