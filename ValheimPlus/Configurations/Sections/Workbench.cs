using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValheimPlus.Configurations.Sections
{
    public class WorkbenchC : BaseConfig<WorkbenchC>
    {
        public float workbenchRange { get; internal set; } = 20;
    }
}
