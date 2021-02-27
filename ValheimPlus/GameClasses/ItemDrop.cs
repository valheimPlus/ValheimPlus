using HarmonyLib;
using System;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    /// <summary>
    /// Item weight reduction and teleport prevention changes
    /// </summary>
    [HarmonyPatch(typeof(ItemDrop), "Awake")]
    public static class ChangeItemData
    {
        private const int defaultSpawnTimeSeconds = 3600;

        private static void Prefix(ref ItemDrop __instance)
        {
            if (Configuration.Current.Items.IsEnabled && Configuration.Current.Items.noTeleportPrevention)
            {
                __instance.m_itemData.m_shared.m_teleportable = true;
            }

            // Disabled for now. Need to hook the Item ToolTip function properly instead due to the way the game handles food durations.

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
                if (__instance.m_itemData.m_shared.m_maxStackSize > 1)
                {
                    if (itemStackMultiplier >= 1)
                    {
                        __instance.m_itemData.m_shared.m_maxStackSize = __instance.m_itemData.m_shared.m_maxStackSize + (int)(((float)(__instance.m_itemData.m_shared.m_maxStackSize) / 100) * itemStackMultiplier);
                    }
                }
            }
        }

        private static void Postfix(ref ItemDrop __instance)
        {
            if (!Configuration.Current.Items.IsEnabled) return; // if items config not enabled, continue with original method
            if (Configuration.Current.Items.droppedItemOnGroundDurationInSeconds.Equals(defaultSpawnTimeSeconds)) return; // if set to default, continue with original method
            if (!(bool)(UnityEngine.Object)__instance.m_nview || !__instance.m_nview.IsValid()) return;
            if (!__instance.m_nview.IsOwner()) return;

            // Get a DateTime value that is the current server time + item drop duration modifier
            DateTime serverTimeWithTimeChange = ZNet.instance.GetTime().AddSeconds(Configuration.Current.Items.droppedItemOnGroundDurationInSeconds - defaultSpawnTimeSeconds);

            // Re-set spawn time of item to the configured percentage of the original duration
            __instance.m_nview.GetZDO().Set("SpawnTime", serverTimeWithTimeChange.Ticks);
        }
    }
}
