// ValheimPlus

using System;

namespace ValheimPlus.Configurations
{
    public class SectionStatusChangeEventArgs : EventArgs
    {
        public bool Enabled { get; set; }

        public SectionStatusChangeEventArgs(bool enabled)
        {
            Enabled = enabled;
        }
    }
}