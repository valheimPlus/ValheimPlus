using HarmonyLib;
using BepInEx;
using ValheimPlus.Configurations;
using UnityEngine;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(ItemDrop), "Awake")]
    public static class ChangeTooltip
    {
        private static void Prefix(ref ItemDrop __instance)
        {

            if (Configuration.Current.Items.IsEnabled && Configuration.Current.Items.noTeleportPrevention)
            {
                __instance.m_itemData.m_shared.m_teleportable = true;
            }

            /* Disabled for now. Need to hook the Item ToolTip function properly instead due to the way the game handles food durations.
            if (Settings.isEnabled("Food"))
            {
                float food_multiplier = Settings.getFloat("Food", "foodDuration");
                if (food_multiplier > 0)
                {
                    if (Convert.ToInt32(__instance.m_itemData.m_shared.m_itemType) == 2) // Item Type = Food
                        __instance.m_itemData.m_shared.m_foodBurnTime = __instance.m_itemData.m_shared.m_foodBurnTime + ((__instance.m_itemData.m_shared.m_foodBurnTime / 100) * food_multiplier);
                }
                if (food_multiplier < 0 && food_multiplier >= -100)
                {
                    if (Convert.ToInt32(__instance.m_itemData.m_shared.m_itemType) == 2) // Item Type = Food
                        __instance.m_itemData.m_shared.m_foodBurnTime = __instance.m_itemData.m_shared.m_foodBurnTime - ((__instance.m_itemData.m_shared.m_foodBurnTime / 100) * (food_multiplier * -1));
                }

            }*/

            if (Configuration.Current.Items.IsEnabled)
            {
                // 50 results in a float point error
                float itemWeigthReduction = (Configuration.Current.Items.baseItemWeightReduction == 50 ? 51 : Configuration.Current.Items.baseItemWeightReduction);
                itemWeigthReduction = (Configuration.Current.Items.baseItemWeightReduction == -50 ? -51 : Configuration.Current.Items.baseItemWeightReduction);

                if (itemWeigthReduction > 0)
                {
                    __instance.m_itemData.m_shared.m_weight = (float)(__instance.m_itemData.m_shared.m_weight + ((__instance.m_itemData.m_shared.m_weight / 100) * itemWeigthReduction));
                }
                if (itemWeigthReduction < 0)
                {
                    __instance.m_itemData.m_shared.m_weight = (float)(__instance.m_itemData.m_shared.m_weight - ((__instance.m_itemData.m_shared.m_weight / 100) * (itemWeigthReduction * -1)));
                }

                float itemStackMultiplier = Configuration.Current.Items.itemStackMultiplier;
                if(__instance.m_itemData.m_shared.m_maxStackSize > 1)
                {
                    if (itemStackMultiplier >= 1)
                    {
                        __instance.m_itemData.m_shared.m_maxStackSize = __instance.m_itemData.m_shared.m_maxStackSize + (int)(((float)(__instance.m_itemData.m_shared.m_maxStackSize) / 100) * itemStackMultiplier);
                    }

                }
            }

        }
    }

}
