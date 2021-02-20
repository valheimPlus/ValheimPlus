using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValheimPlus.Configurations.Sections
{
    public class TimeConfig : BaseConfig<TimeConfig>
    {
        public long dayTime { get; internal set; } = 1200L;
    }
}
