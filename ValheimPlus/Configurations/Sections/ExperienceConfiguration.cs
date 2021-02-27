// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Everything experience-related")]
    public class ExperienceConfiguration : ServerSyncConfig<ExperienceConfiguration>
    {
        [Configuration(
            "Each of these values represent the increase to experience gained by % increased. The value 50 would result in 50% increased experience gained for the respective skill by name.", ActivationTime.Immediately)]
        public float swords { get; set; } = 0;

        public float knives { get; set; } = 0;
        public float clubs { get; set; } = 0;
        public float polearms { get; set; } = 0;
        public float spears { get; set; } = 0;
        public float blocking { get; set; } = 0;
        public float axes { get; set; } = 0;
        public float bows { get; set; } = 0;
        public float fireMagic { get; set; } = 0;
        public float frostMagic { get; set; } = 0;
        public float unarmed { get; set; } = 0;
        public float pickaxes { get; set; } = 0;
        public float woodCutting { get; set; } = 0;
        public float jump { get; set; } = 0;
        public float sneak { get; set; } = 0;
        public float run { get; set; } = 0;
        public float swim { get; set; } = 0;
    }
}