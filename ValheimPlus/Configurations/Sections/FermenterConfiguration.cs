namespace ValheimPlus.Configurations.Sections
{
    public class FermenterConfiguration : ServerSyncConfig<FermenterConfiguration>
    {
        public float fermenterDuration { get; set; } = 2400;
        public int fermenterItemsProduced { get; set; } = 4;

        public bool showDuration { get; set; } = false;
    }

}
