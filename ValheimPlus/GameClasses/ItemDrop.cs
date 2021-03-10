using HarmonyLib;
using System;
using ValheimPlus.Configurations;
using UnityEngine;

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


    [HarmonyPatch(typeof(ItemDrop.ItemData), "GetMaxDurability", new System.Type[] { typeof(int) })]
    public static class ItemDrop_GetMaxDurability_Patch
    {
        private static bool Prefix(ref ItemDrop.ItemData __instance, ref int quality, ref float __result)
        {
            if (!Configuration.Current.Durability.IsEnabled)
                return true;

            string itemName = __instance.m_shared.m_name.Replace("$item_", "");
            string itemType = itemName.Split(new char[] { '_' })[0];

            float multiplierForItem = 0;

            float maxDurability = (__instance.m_shared.m_maxDurability + (float)Mathf.Max(0, quality - 1) * __instance.m_shared.m_durabilityPerLevel);
            __result = maxDurability;

            bool modified = false;
            switch (itemType)
            {

                // pickaxes
                case "pickaxe":
                    modified = true;
                    multiplierForItem = Helper.applyModifierValue(maxDurability, Configuration.Current.Durability.pickaxes);
                    break;

                // axes
                case "axe":
                    modified = true;
                    multiplierForItem = Helper.applyModifierValue(maxDurability, Configuration.Current.Durability.axes);
                    break;

                // hammer
                case "hammer":
                    modified = true;
                    multiplierForItem = Helper.applyModifierValue(maxDurability, Configuration.Current.Durability.hammer);
                    break;

                // cultivator
                case "cultivator":
                    modified = true;
                    multiplierForItem = Helper.applyModifierValue(maxDurability, Configuration.Current.Durability.cultivator);
                    break;

                // hoe
                case "hoe":
                    modified = true;
                    multiplierForItem = Helper.applyModifierValue(maxDurability, Configuration.Current.Durability.hoe);
                    break;

                case "torch":
                    modified = true;
                    multiplierForItem = Helper.applyModifierValue(maxDurability, Configuration.Current.Durability.torch);
                    break;

                default:
                    break;
            }

            switch (__instance.m_shared.m_itemType)
            {
                case ItemDrop.ItemData.ItemType.TwoHandedWeapon:
                case ItemDrop.ItemData.ItemType.OneHandedWeapon:
                    // WEAPONS
                    if (!modified) // Some tools are considered to be OneHandedWeapons
                        multiplierForItem = Helper.applyModifierValue(maxDurability, Configuration.Current.Durability.weapons);
                    break;
                case ItemDrop.ItemData.ItemType.Bow:
                    // BOW
                    if (!modified)
                        multiplierForItem = Helper.applyModifierValue(maxDurability, Configuration.Current.Durability.bows);
                    break;
                case ItemDrop.ItemData.ItemType.Shield:
                    // Shields
                    if (!modified)
                        multiplierForItem = Helper.applyModifierValue(maxDurability, Configuration.Current.Durability.shields);
                    break;
                case ItemDrop.ItemData.ItemType.Helmet:
                case ItemDrop.ItemData.ItemType.Chest:
                case ItemDrop.ItemData.ItemType.Legs:
                case ItemDrop.ItemData.ItemType.Shoulder:
                    // ARMOR
                    if (!modified && __instance.m_shared.m_itemType != ItemDrop.ItemData.ItemType.Shield)
                        multiplierForItem = Helper.applyModifierValue(maxDurability, Configuration.Current.Durability.armor);
                    break;
                default:
                    break;
            }



            if (multiplierForItem != maxDurability)
                __result = multiplierForItem;

            return false;
        }
    }

    [HarmonyPatch(typeof(ItemDrop.ItemData), "GetArmor", new System.Type[] { typeof(int) })]
    public static class ItemDrop_GetArmor_Patch
    {
        private static bool Prefix(ref ItemDrop.ItemData __instance, ref int quality, ref float __result)
        {
            if (!Configuration.Current.Armor.IsEnabled)
                return true;

            float armor = __instance.m_shared.m_armor + (float)Mathf.Max(0, quality - 1) * __instance.m_shared.m_armorPerLevel;
            __result = armor;

            float modifiedArmorValue = armor;
            switch (__instance.m_shared.m_itemType)
            {
                case ItemDrop.ItemData.ItemType.Helmet:
                    modifiedArmorValue = Helper.applyModifierValue(modifiedArmorValue, Configuration.Current.Armor.helmets);
                    break;
                case ItemDrop.ItemData.ItemType.Chest:
                    modifiedArmorValue = Helper.applyModifierValue(modifiedArmorValue, Configuration.Current.Armor.chests);
                    break;
                case ItemDrop.ItemData.ItemType.Legs:
                    modifiedArmorValue = Helper.applyModifierValue(modifiedArmorValue, Configuration.Current.Armor.legs);
                    break;
                case ItemDrop.ItemData.ItemType.Shoulder:
                    modifiedArmorValue = Helper.applyModifierValue(modifiedArmorValue, Configuration.Current.Armor.capes);
                    break;
                default:
                    break;
            }

            if (modifiedArmorValue != armor)
                __result = modifiedArmorValue;

            return false;
        }
    }

    [HarmonyPatch(typeof(ItemDrop.ItemData), "GetBaseBlockPower", new System.Type[] { typeof(int) })]
    public static class ItemDrop_GetBaseBlockPower_Patch
    { 
        private static bool Prefix(ref ItemDrop.ItemData __instance, ref int quality, ref float __result)
        {
            if (!Configuration.Current.Shields.IsEnabled)
                return true;

            float blockValue = __instance.m_shared.m_blockPower + (float)Mathf.Max(0, quality - 1) * __instance.m_shared.m_blockPowerPerLevel;
            __result = Helper.applyModifierValue(blockValue, Configuration.Current.Shields.blockRating);
            return false;
        }
    }
}
