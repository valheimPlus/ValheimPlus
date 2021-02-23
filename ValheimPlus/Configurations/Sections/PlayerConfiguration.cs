namespace ValheimPlus.Configurations.Sections
{
    public class PlayerConfiguration : ServerSyncConfig<PlayerConfiguration>
    {
        public float baseMaximumWeight { get; internal set; } = 300;
        public float baseMegingjordBuff { get; internal set; } = 150;
        public float baseAutoPickUpRange { get; internal set; } = 2;
        public bool disableCameraShake { get; internal set; } = false;
        public float baseUnarmedDamage { get; internal set; } = 0;

    }
}
