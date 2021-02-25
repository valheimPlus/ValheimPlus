// ---------------------------------------------------------------------------------------
// Solution: ValheimPlus
// Project: ValheimPlus
// Filename: PlantConfiguration.cs
// 
// Last modified: 2021-02-25 17:36
// Created:       2021-02-25 17:36
// 
// Copyright: 2021 Walter Wissing & Co
// ---------------------------------------------------------------------------------------

namespace ValheimPlus.Configurations.Sections
{
    public class PlantConfiguration : ServerSyncConfig<PlantConfiguration>
    {
        public bool noWrongBiome { get; set; } = false;

        public bool growWithoutSun { get; set; } = false;
    }
}