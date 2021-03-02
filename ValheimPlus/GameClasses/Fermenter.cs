using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    /// <summary>
    /// Altering fermenter production speed
    /// </summary>
    [HarmonyPatch(typeof(Fermenter), "Awake")]
    public static class ApplyFermenterChanges
    {
        private static bool Prefix(ref float ___m_fermentationDuration, ref Fermenter __instance)
        {
            if (Configuration.Current.Fermenter.IsEnabled)
            {
                float fermenterDuration = Configuration.Current.Fermenter.fermenterDuration;
                if (fermenterDuration > 0)
                {
                    ___m_fermentationDuration = fermenterDuration;
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Altering fermeter items produced
    /// </summary>
    [HarmonyPatch(typeof(Fermenter), "GetItemConversion")]
    public static class ApplyFermenterItemCountChanges
    {
        private static void Postfix(ref Fermenter.ItemConversion __result)
        {
            if (Configuration.Current.Fermenter.IsEnabled)
            {
                int fermenterItemCount = Configuration.Current.Fermenter.fermenterItemsProduced;
                if (fermenterItemCount > 0)
                {
                    __result.m_producedItems = fermenterItemCount;
                }
            }
        }
    }
}
