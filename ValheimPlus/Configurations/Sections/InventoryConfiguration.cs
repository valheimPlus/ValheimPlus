using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValheimPlus.Configurations.Sections
{
    public class InventoryConfiguration : ServerSyncConfig<InventoryConfiguration>
    {
        public int width { get; internal set; } = 8;

        public int height { get; internal set; } = 4;
    }
}
