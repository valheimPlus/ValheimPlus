using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    /// <summary>
    /// Item weight reduction and teleport prevention changes
    /// </summary>
    [HarmonyPatch(typeof(ItemDrop), "Awake")]
    public static class ChangeItemData
    {
        private static void Prefix(ref ItemDrop __instance)
        {
            if (Configuration.Current.Items.IsEnabled && Configuration.Current.Items.noTeleportPrevention)
            {
                __instance.m_itemData.m_shared.m_teleportable = true;
            }

            if (Configuration.Current.Items.IsEnabled)
            {
                
                __instance.m_itemData.m_shared.m_weight = Helper.applyModifierValue(__instance.m_itemData.m_shared.m_weight, Configuration.Current.Items.baseItemWeightReduction);

                if (__instance.m_itemData.m_shared.m_maxStackSize > 1)
                {
                    if (Configuration.Current.Items.itemStackMultiplier >= 1)
                    {
                        __instance.m_itemData.m_shared.m_maxStackSize = (int)Helper.applyModifierValue(__instance.m_itemData.m_shared.m_maxStackSize, Configuration.Current.Items.itemStackMultiplier);
                    }
                }
            }
        }

    }


}
