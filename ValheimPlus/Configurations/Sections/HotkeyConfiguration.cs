using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class HotkeyConfiguration : BaseConfig<HotkeyConfiguration>
    {
        public KeyCode RollForwards { get; set; } = KeyCode.F9;
        public KeyCode RollBackwards { get; set; } = KeyCode.F10;
    }
}
