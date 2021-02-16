using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(Smelter), "Awake")]
    public static class ApplyFurnaceChanges
    {
        private static void Prefix(ref Smelter __instance)
        {
            if (!__instance.m_addWoodSwitch && Configuration.Current.Kiln.IsEnabled)
            {
                // is kiln
                __instance.m_maxOre = Configuration.Current.Kiln.MaximumWood;
                __instance.m_secPerProduct = Configuration.Current.Kiln.ProductionSpeed;
            }
            else
            {
                // is furnace
                if (Configuration.Current.Furnace.IsEnabled)
                {
                    __instance.m_maxOre = Configuration.Current.Furnace.MaximumOre;
                    __instance.m_maxFuel = Configuration.Current.Furnace.MaximumCoal;
                    __instance.m_secPerProduct = Configuration.Current.Furnace.ProductionSpeed;
                    __instance.m_fuelPerProduct = Configuration.Current.Furnace.CoalUsedPerProduct;
                }
            }
        }
    }
}
