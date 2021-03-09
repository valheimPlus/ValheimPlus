﻿namespace ValheimPlus.Configurations.Sections
{
    public class FoodConfiguration : ServerSyncConfig<FoodConfiguration>
    {
        public float foodDurationMultiplier { get; set; } = 0;
        public bool foodDegradesOverTime { get; set; } = true;
    }

}
