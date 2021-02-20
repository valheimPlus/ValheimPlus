using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class AdvancedEditingModeConfiguration : BaseConfig<AdvancedEditingModeConfiguration>
    {
        public KeyCode EnterAdvancedEditingMode { get; internal set; } = KeyCode.Keypad0;
        public KeyCode ResetAdvancedEditingMode { get; internal set; } = KeyCode.F7;
        public KeyCode AbortAndExitAdvancedEditingMode { get; internal set; } =  KeyCode.F8;
        public KeyCode ConfirmPlacementOfAdvancedEditingMode { get; internal set; } = KeyCode.KeypadEnter;
    }
}
