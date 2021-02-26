// ValheimPlus

using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("https://docs.unity3d.com/ScriptReference/KeyCode.html <- a list of keycodes")]
    public class AdvancedEditingModeConfiguration : BaseConfig<AdvancedEditingModeConfiguration>
    {
        [Configuration("Enter the advanced editing mode with this key")]
        public KeyCode enterAdvancedEditingMode { get; internal set; } = KeyCode.Keypad0;

        [Configuration("Reset the object to its original position and rotation")]
        public KeyCode resetAdvancedEditingMode { get; internal set; } = KeyCode.F7;

        [Configuration("Exit the advanced editing mode with this key and reset the object")]
        public KeyCode abortAndExitAdvancedEditingMode { get; internal set; } = KeyCode.F8;

        [Configuration("Confirm the placement of the object and place it")]
        public KeyCode confirmPlacementOfAdvancedEditingMode { get; internal set; } = KeyCode.KeypadEnter;
    }
}