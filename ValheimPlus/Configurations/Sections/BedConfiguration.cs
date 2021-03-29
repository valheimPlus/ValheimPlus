using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class BedConfiguration : BaseConfig<BedConfiguration>
    {
        public bool sleepWithoutSpawn { get; set; } = false;
        public bool unclaimedBedsOnly { get; set; } = false;
    }
}