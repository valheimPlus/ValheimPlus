namespace ValheimPlus.Configurations.Sections
{
    public class PlayerConfiguration : BaseConfig<PlayerConfiguration>
    {
        public float BaseMaximumWeight { get; set; } = 300;
        public float BaseMegingjordBuff { get; set; } = 150;
        public float BaseAutoPickUpRange { get; set; } = 2;
        public bool DisableCameraShake { get; internal set; } = false;
    }

}
