namespace ValheimPlus.Configurations.Sections
{
    public class WorkbenchConfiguration : BaseConfig<WorkbenchConfiguration>
    {
        public float workbenchRange { get; internal set; } = 20;
        public bool disableRoofCheck { get; internal set; } = false;
    }
}
