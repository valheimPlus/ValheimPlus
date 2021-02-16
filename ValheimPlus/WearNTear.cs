using HarmonyLib;
using System;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(WearNTear), "ApplyDamage")]
    public static class RemoveWearNTear
    {
        private static Boolean Prefix()
        {
            if (Configuration.Current.Building.IsEnabled && Configuration.Current.Building.NoWeatherDamage)
            {
                return false;
            }
            return true;
        }
    }
}
