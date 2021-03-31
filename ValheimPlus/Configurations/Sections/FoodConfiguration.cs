namespace ValheimPlus.Configurations.Sections
{
    public class FoodConfiguration : ServerSyncConfig<FoodConfiguration>
    {
        public float foodDurationMultiplier { get; internal set; } = 0;
        public bool disableFoodDegradation { get; internal set; } = false;
    }
}
