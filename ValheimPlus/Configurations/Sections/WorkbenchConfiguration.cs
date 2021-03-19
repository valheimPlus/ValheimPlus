namespace ValheimPlus.Configurations.Sections
{
    public class WorkbenchConfiguration : ServerSyncConfig<WorkbenchConfiguration>
    {
        public float workbenchRange { get; internal set; } = 20;
        public float workbenchAttachmentRange { get; internal set; } = 5.0f;
        public bool disableRoofCheck { get; internal set; } = false;
    }
}
