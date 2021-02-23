namespace ValheimPlus.Configurations.Sections
{
    public class WardConfiguration : ServerSyncConfig<WardConfiguration>
    {
        public float wardRange { get; internal set; } = 20;
    }
}
