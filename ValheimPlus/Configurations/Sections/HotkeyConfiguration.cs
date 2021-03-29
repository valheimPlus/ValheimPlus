using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class HotkeyConfiguration : BaseConfig<HotkeyConfiguration>
    {
        public KeyCode rollForwards { get; internal set; } = KeyCode.F9;
        public KeyCode rollBackwards { get; internal set; } = KeyCode.F10;
    }
}
