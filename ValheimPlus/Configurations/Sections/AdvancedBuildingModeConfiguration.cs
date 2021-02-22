using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class AdvancedBuildingModeConfiguration : BaseConfig<AdvancedBuildingModeConfiguration>
    {
        public KeyCode enterAdvancedBuildingMode { get; set; } = KeyCode.F1;
        public KeyCode exitAdvancedBuildingMode { get; set; } = KeyCode.F3;
    }
}
