using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValheimPlus.Configurations.Sections
{
    public class Unarmed : ServerSyncConfig<Unarmed>
    {
        public float baseDamage { get; internal set; } = 0;
    }
}
