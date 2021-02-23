using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValheimPlus.Configurations.Sections
{
    public class UnarmedConfiguration : ServerSyncConfig<UnarmedConfiguration>
    {
        public float scaleDamage { get; internal set; } = 100;
    }
}
