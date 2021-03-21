namespace ValheimPlus.Configurations.Sections
{
    public class FireSourceConfiguration : ServerSyncConfig<FireSourceConfiguration>
    {
        public bool torches { get; internal set; } = false;
        public bool fires { get; internal set; } = false;
        public bool autoFuel { get; internal set; } = false;
        public bool ignorePrivateAreaCheck { get; internal set; } = true;
        public float autoRange { get; internal set; } = 10;
    }
}
