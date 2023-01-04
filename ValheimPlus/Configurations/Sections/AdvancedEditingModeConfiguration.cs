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
        
        public KeyCode copyObjectRotation { get; internal set; } = KeyCode.Keypad7;
        public KeyCode pasteObjectRotation { get; internal set; } = KeyCode.Keypad8;
        public KeyCode copyObjectRotationAndPosition { get; internal set; } = KeyCode.Keypad4;
        public KeyCode pasteObjectRotationAndPosition { get; internal set; } = KeyCode.Keypad5;

        public KeyCode increaseScrollSpeed { get; internal set; } = KeyCode.KeypadPlus;
        public KeyCode decreaseScrollSpeed { get; internal set; } = KeyCode.KeypadMinus;
    }
}
