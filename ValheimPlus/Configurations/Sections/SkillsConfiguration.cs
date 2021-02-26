using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValheimPlus.Configurations.Sections
{
    public class SkillsConfiguration : ServerSyncConfig<SkillsConfiguration>
    {
        public float skillDeathLowerFactor { get; internal set; }
    }
}