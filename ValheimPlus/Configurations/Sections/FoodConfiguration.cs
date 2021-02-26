// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    public class FoodConfiguration : ServerSyncConfig<FoodConfiguration>
    {
        [Configuration("Increase or reduce the time that food lasts by %. The value 50 would cause food to run out 50% slower.")]
        public float foodDurationMultiplier { get; set; } = 0;
    }
}