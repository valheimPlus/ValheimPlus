namespace ValheimPlus.Configurations.Sections
{
    public class CameraConfiguration : BaseConfig<CameraConfiguration>
    {
        public float cameraMaximumZoomDistance { get; internal set; } = 1146;
        public float cameraBoatMaximumZoomDistance { get; internal set; } = 6;
        public float cameraFOV { get; internal set; } = 85;
    }
}
