using HarmonyLib;
using System;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(Inventory), "IsTeleportable")]
    public static class noItemTeleportPrevention
    {
        private static bool Prefix(ref Boolean __result)
        {
            if (Configuration.Current.Items.IsEnabled)
            {
                if (Configuration.Current.Items.noTeleportPrevention) 
                {
                    __result = true;
                    return false; // Stop any unnecessary calcs by interrupting the normal func
                }
            } 
            return true; // continue as normal
        }
    }
}
