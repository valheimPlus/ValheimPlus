using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class AdvancedBuildingModeConfiguration : BaseConfig<AdvancedBuildingModeConfiguration>
    {
        public KeyCode enterAdvancedBuildingMode { get; set; } = KeyCode.F1;
        public KeyCode exitAdvancedBuildingMode { get; set; } = KeyCode.F3;

        public KeyCode copyObjectRotation { get; set; } = KeyCode.Keypad7;
        public KeyCode pasteObjectRotation { get; set; } = KeyCode.Keypad8;
    }
}
