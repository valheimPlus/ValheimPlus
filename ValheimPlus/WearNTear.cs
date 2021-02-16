using HarmonyLib;
using System;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(WearNTear), "HaveRoof")]
    public static class RemoveWearNTear
    {
        
        private static void Postfix(ref Boolean __result)
        {
            if (Configuration.Current.Building.IsEnabled && Configuration.Current.Building.NoWeatherDamage)
            {
                __result = true;
            }
        }
    }
}
