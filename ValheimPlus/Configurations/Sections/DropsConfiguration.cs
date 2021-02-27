namespace ValheimPlus.Configurations.Sections
{
    public class DropsConfiguration : ServerSyncConfig<DropsConfiguration>
    {
        public int baseIncreasedDropMultiplier { get; internal set; } = 1;
    }

}
