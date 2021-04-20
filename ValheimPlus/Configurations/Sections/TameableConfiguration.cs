using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValheimPlus.Configurations.Sections
{
    public class TameableConfiguration : ServerSyncConfig<TameableConfiguration>
    {
        public int mortality { get; internal set; } = 0;
        public bool ownerDamageOverride { get; internal set; } = false;
        public float stunRecoveryTime { get; internal set; } = 10f;
    }
}
