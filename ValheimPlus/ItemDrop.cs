using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(ItemDrop), "Awake")]
    public static class ChangeTooltip
    {
        private static void Prefix(ref ItemDrop __instance)
        {

            if (Configuration.Current.Items.IsEnabled && Configuration.Current.Items.NoTeleportPrevention)
            {
                __instance.m_itemData.m_shared.m_teleportable = true;
            }

            /* Disabled for now. Need to hook the Item ToolTip function properly instead.
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
                float itemWeigthReduction = Configuration.Current.Items.BaseItemWeightReduction;
                if (itemWeigthReduction > 0 && itemWeigthReduction <= 100)
                {
                    __instance.m_itemData.m_shared.m_weight = __instance.m_itemData.m_shared.m_weight - ((__instance.m_itemData.m_shared.m_weight / 100) * itemWeigthReduction);
                }
                if (itemWeigthReduction < 0)
                {
                    __instance.m_itemData.m_shared.m_weight = __instance.m_itemData.m_shared.m_weight + ((__instance.m_itemData.m_shared.m_weight / 100) * (itemWeigthReduction * -1));
                }
            }

        }
    }

}
