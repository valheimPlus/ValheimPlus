namespace ValheimPlus.Configurations.Sections
{
    public class FermenterConfiguration : ServerSyncConfig<FermenterConfiguration>
    {
        public float fermenterDuration { get; internal set; } = 10;
        public int fermenterItemsProduced { get; internal set; } = 4;
        public bool showDuration { get; internal set; } = false;
        public bool autoDeposit { get; internal set; } = true;
        public bool autoFuel { get; internal set; } = true;
        public bool ignorePrivateAreaCheck { get; internal set; } = true;
        public float autoRange { get; internal set; } = 10;
    }

}
