using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class GridAlignmentConfiguration : ServerSyncConfig<GridAlignmentConfiguration>
    {
        public KeyCode align { get; set; } = KeyCode.LeftAlt;
        public KeyCode alignToggle { get; set; } = KeyCode.F7;
        public KeyCode changeDefaultAlignment { get; set; } = KeyCode.F6;
    }
}
