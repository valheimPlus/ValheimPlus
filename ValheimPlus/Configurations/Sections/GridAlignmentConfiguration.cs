using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class GridAlignmentConfiguration : ServerSyncConfig<GridAlignmentConfiguration>
    {
        public KeyCode align { get; internal set; } = KeyCode.LeftAlt;
        public KeyCode alignToggle { get; internal set; } = KeyCode.F7;
        public KeyCode changeDefaultAlignment { get; internal set; } = KeyCode.F6;
    }
}
