using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValheimPlus.Configurations.Sections
{
    public class SleepConfiguration : ServerSyncConfig<SleepConfiguration>
    {
        public int numberOfPlayersToSleep { get; internal set; } = 1;
        public bool byPlayers { get; internal set; } = false;
        public bool byPercentage { get; internal set; } = false;
        public double percentageOfPlayersToSleep { get; internal set; } = 50;
    }
}
