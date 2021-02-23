using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(Smelter), "Awake")]
    public static class ApplyFurnaceChanges
    {
        private static void Prefix(ref Smelter __instance)
        {
            if (!__instance.m_addWoodSwitch)
            {
                // is kiln
                if (Configuration.Current.Kiln.IsEnabled)
                {
                    __instance.m_maxOre = Configuration.Current.Kiln.maximumWood;
                    __instance.m_secPerProduct = Configuration.Current.Kiln.productionSpeed;
                }
            }
            else
            {
                // is furnace
                if (Configuration.Current.Furnace.IsEnabled)
                {
                    __instance.m_maxOre = Configuration.Current.Furnace.maximumOre;
                    __instance.m_maxFuel = Configuration.Current.Furnace.maximumCoal;
                    __instance.m_secPerProduct = Configuration.Current.Furnace.productionSpeed;
                    __instance.m_fuelPerProduct = Configuration.Current.Furnace.coalUsedPerProduct;
                }
            }
        }
    }
}
