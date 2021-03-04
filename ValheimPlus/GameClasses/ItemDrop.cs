using HarmonyLib;
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


    [HarmonyPatch(typeof(ItemDrop.ItemData), "GetMaxDurability", new System.Type[] { typeof(int) })]
    public static class ItemDrop_GetMaxDurability_Patch
    {
        private static bool Prefix(ref ItemDrop.ItemData __instance, ref int quality, ref float __result)
        {
            if (!Configuration.Current.Durability.IsEnabled)
                return true;

            // Tools: Axe, How, Cultivator, Hammer, Pickaxe
            string itemName = __instance.m_shared.m_name.Replace("$item_", "");
            string itemType = itemName.Split(new char[] { '_' })[0];

            float multiplierForItem = 0;

            float maxDurability = (__instance.m_shared.m_maxDurability + (float)Mathf.Max(0, quality - 1) * __instance.m_shared.m_durabilityPerLevel);
            __result = maxDurability;

            bool modified = false;
            switch (itemType) {

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
                case ItemDrop.ItemData.ItemType.Helmet:
                case ItemDrop.ItemData.ItemType.Chest:
                case ItemDrop.ItemData.ItemType.Legs:
                case ItemDrop.ItemData.ItemType.Shoulder:
                    // ARMOR
                    if (!modified)
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
}
