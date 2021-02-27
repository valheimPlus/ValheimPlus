// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    public class CameraConfiguration : BaseConfig<CameraConfiguration>
    {
        [Configuration("maximum camera zoom distance", ActivationTime.Immediately)]
        public float cameraMaximumZoomDistance { get; set; } = 1146;

        [Configuration("maximum camera zoom distance on boats", ActivationTime.Immediately)]
        public float cameraBoatMaximumZoomDistance { get; set; } = 6;

        [Configuration("camera FOV (field of view)", ActivationTime.Immediately)]
        public float cameraFOV { get; set; } = 85;
    }
}