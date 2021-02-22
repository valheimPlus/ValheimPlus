using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class HotkeyConfiguration : BaseConfig<HotkeyConfiguration>
    {
        public KeyCode rollForwards { get; set; } = KeyCode.F9;
        public KeyCode rollBackwards { get; set; } = KeyCode.F10;
    }
}
