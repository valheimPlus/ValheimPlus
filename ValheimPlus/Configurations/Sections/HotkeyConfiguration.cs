// ValheimPlus

using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("https://docs.unity3d.com/ScriptReference/KeyCode.html <- a list of keycodes")]
    public class HotkeyConfiguration : BaseConfig<HotkeyConfiguration>
    {
        [Configuration("Roll forwards on key pressed")]
        public KeyCode rollForwards { get; set; } = KeyCode.F9;

        [Configuration("Roll backwards on key pressed")]
        public KeyCode rollBackwards { get; set; } = KeyCode.F10;
    }
}