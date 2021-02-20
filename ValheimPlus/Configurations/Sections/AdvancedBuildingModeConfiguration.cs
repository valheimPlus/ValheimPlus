using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class AdvancedBuildingModeConfiguration : BaseConfig<AdvancedBuildingModeConfiguration>
    {
        public KeyCode EnterAdvancedBuildingMode { get; set; } = KeyCode.F1;
        public KeyCode ExitAdvancedBuildingMode { get; set; } = KeyCode.F3;
    }
}
