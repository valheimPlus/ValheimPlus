using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValheimPlus.Configurations.Sections
{
    public class FireplaceC : ServerSyncConfig<FireplaceC>
    {
        public Boolean onlyTorches { get; internal set; } = false;
    }
}
