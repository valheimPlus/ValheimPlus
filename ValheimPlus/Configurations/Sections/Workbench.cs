using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValheimPlus.Configurations.Sections
{
    public class WorkbenchConfiguration : BaseConfig<WorkbenchConfiguration>
    {
        public float workbenchRange { get; internal set; } = 20;
    }
}
