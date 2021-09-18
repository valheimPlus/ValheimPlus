using UnityEngine;

namespace ValheimPlus.Configurations.Sections
{
    public class FreePlacementRotationConfiguration : BaseConfig<FreePlacementRotationConfiguration>
    {
        public KeyCode rotateY { get; internal set; } = KeyCode.LeftAlt;
        public KeyCode rotateX { get; internal set; } = KeyCode.C;
        public KeyCode rotateZ { get; internal set; } = KeyCode.V;
        
        public KeyCode copyRotationParallel  { get; internal set; } = KeyCode.F;
        public KeyCode copyRotationPerpendicular  { get; internal set; } = KeyCode.G;
    }
}
