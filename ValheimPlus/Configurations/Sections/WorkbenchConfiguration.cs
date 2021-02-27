// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Workbench settings")]
    public class WorkbenchConfiguration : BaseConfig<WorkbenchConfiguration>
    {
        [Configuration("Set the workbench radius in meters", ActivationTime.Immediately)]
        public float workbenchRange { get; internal set; } = 20;

        [Configuration("Disables the roof and exposure requirement to use a workbench", ActivationTime.Immediately)]
        public bool disableRoofCheck { get; internal set; } = false;
    }
}