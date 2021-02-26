// ValheimPlus

using System;
using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    [Serializable]
    [ConfigurationSection("https://docs.unity3d.com/ScriptReference/KeyCode.html <- a list of keycodes")]
    public class AdvancedBuildingModeConfiguration : BaseConfig<AdvancedBuildingModeConfiguration>
    {
        [Configuration("Enter the advanced building mode with this key when building")]
        public KeyCode enterAdvancedBuildingMode { get; set; } = KeyCode.F1;

        [Configuration("Exit the advanced building mode with this key when building")]
        public KeyCode exitAdvancedBuildingMode { get; set; } = KeyCode.F3;
    }
}