using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValheimPlus.Configurations.Sections
{
    public class SleepConfiguration : ServerSyncConfig<SleepConfiguration>
    {
        public double numberOfPlayersToSleep { get; internal set; } = 1;
    }
}
