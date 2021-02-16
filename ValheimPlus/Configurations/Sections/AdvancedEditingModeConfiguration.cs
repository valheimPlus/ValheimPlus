using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class AdvancedEditingModeConfiguration : BaseConfig<AdvancedEditingModeConfiguration>
    {
        public KeyCode EnterAdvancedEditingMode { get; internal set; }
        public KeyCode ResetAdvancedEditingMode { get; internal set; }
        public KeyCode AbortAndExitAdvancedEditingMode { get; internal set; }
        public KeyCode ConfirmPlacementOfAdvancedEditingMode { get; internal set; }
    }
}
