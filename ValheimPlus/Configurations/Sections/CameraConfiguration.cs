// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    public class CameraConfiguration : BaseConfig<CameraConfiguration>
    {
        [Configuration("maximum camera zoom distance")]
        public float cameraMaximumZoomDistance { get; internal set; } = 1146;

        [Configuration("maximum camera zoom distance on boats")]
        public float cameraBoatMaximumZoomDistance { get; internal set; } = 6;

        [Configuration("camera FOV (field of view)")]
        public float cameraFOV { get; internal set; } = 85;
    }
}