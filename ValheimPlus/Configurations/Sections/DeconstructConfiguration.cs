namespace ValheimPlus.Configurations.Sections
{
    public class DeconstructConfiguration : BaseConfig<DeconstructConfiguration>
    {
        public int percentageOfReturnedResource { get; internal set; } = 100;
    }
}