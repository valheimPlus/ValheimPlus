using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class AdvancedEditingModeConfiguration : BaseConfig<AdvancedEditingModeConfiguration>
    {
        public KeyCode enterAdvancedEditingMode { get; internal set; } = KeyCode.Keypad0;
        public KeyCode resetAdvancedEditingMode { get; internal set; } = KeyCode.F7;
        public KeyCode abortAndExitAdvancedEditingMode { get; internal set; } =  KeyCode.F8;
        public KeyCode confirmPlacementOfAdvancedEditingMode { get; internal set; } = KeyCode.KeypadEnter;
    }
}
