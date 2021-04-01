using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class AdvancedBuildingModeConfiguration : BaseConfig<AdvancedBuildingModeConfiguration>
    {
        public KeyCode enterAdvancedBuildingMode { get; internal set; } = KeyCode.F1;
        public KeyCode exitAdvancedBuildingMode { get; internal set; } = KeyCode.F3;

        public KeyCode copyObjectRotation { get; internal set; } = KeyCode.Keypad7;
        public KeyCode pasteObjectRotation { get; internal set; } = KeyCode.Keypad8;

        public KeyCode increaseScrollSpeed { get; internal set; } = KeyCode.KeypadPlus;
        public KeyCode decreaseScrollSpeed { get; internal set; } = KeyCode.KeypadMinus;
    }
}
