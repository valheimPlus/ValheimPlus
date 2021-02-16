using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValheimPlus.Configurations.Sections
{
    public class StaminaConfiguration : BaseConfig<StaminaConfiguration>
    {
        public float DodgeStaminaUsage { get; internal set; }
        public float EncumberedStaminaDrain { get; internal set; }
        public float SneakStaminaDrain { get; internal set; }
        public float RunStaminaDrain { get; internal set; }
        public float StaminaRegenDelay { get; internal set; }
        public float StaminaRegen { get; internal set; }
        public float SwimStaminaDrain { get; internal set; }
        public float JumpStaminaUsage { get; internal set; }
    }
}
