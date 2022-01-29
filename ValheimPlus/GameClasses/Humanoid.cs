using HarmonyLib;
using ValheimPlus.Configurations;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;

namespace ValheimPlus.GameClasses
{
    [HarmonyPatch(typeof(Humanoid), "GetCurrentWeapon")]
    public static class ModifyCurrentWeapon
    {
        private static ItemDrop.ItemData Postfix(ItemDrop.ItemData __weapon, ref Character __instance)
        {
            if (Configuration.Current.Player.IsEnabled)
            {
                if (__weapon != null)
                {
                    if (__weapon.m_shared.m_name == "Unarmed")
                    {
                        Player CharacterPlayerInstance = (Player)__instance;
                        __weapon.m_shared.m_damages.m_blunt = CharacterPlayerInstance.GetSkillFactor(Skills.SkillType.Unarmed) * Configuration.Current.Player.baseUnarmedDamage;
                        if (__weapon.m_shared.m_damages.m_blunt <= 2)
                            __weapon.m_shared.m_damages.m_blunt = 2;
                    }
                }
            }

            return __weapon;
        }
    }

    /// <summary>
    /// When equipping a one-handed weapon, also equip best shield from inventory.
    /// </summary>
    [HarmonyPatch(typeof(Humanoid), "EquipItem")]
    public static class Humanoid_EquipItem_Patch
    {
        private static bool Postfix(bool __result, Humanoid __instance, ItemDrop.ItemData item)
        {
            if (Configuration.Current.Player.IsEnabled &&
                Configuration.Current.Player.autoEquipShield &&
                __result && 
                __instance.IsPlayer() && 
                __instance.m_rightItem?.m_shared.m_itemType == ItemDrop.ItemData.ItemType.OneHandedWeapon &&
                item.m_shared.m_itemType != ItemDrop.ItemData.ItemType.Shield)
            {
                List<ItemDrop.ItemData> inventoryItems = __instance.m_inventory.GetAllItems();

                ItemDrop.ItemData bestShield = null;
                foreach (ItemDrop.ItemData inventoryItem in inventoryItems)
                {
                    if (inventoryItem.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Shield)
                    {
                        if (bestShield == null)
                        {
                            bestShield = inventoryItem;

                            continue;
                        }

                        if (bestShield.m_shared.m_blockPower < inventoryItem.m_shared.m_blockPower)
                        {
                            bestShield = inventoryItem;

                            continue;
                        }
                    }
                }

                if (bestShield != null)
                {
                    __instance.EquipItem(bestShield, false);
                }
            }

            return __result;
        }
    }


    /// <summary>
    /// When unequipping a one-handed weapon also unequip shield from inventory.
    /// </summary>
    [HarmonyPatch(typeof(Humanoid), "UnequipItem")]
    public static class Humanoid_UnequipItem_Patch
    {
        private static void Postfix(Humanoid __instance, ItemDrop.ItemData item)
        {
            if (Configuration.Current.Player.IsEnabled &&
                Configuration.Current.Player.autoUnequipShield &&
                item?.m_shared.m_itemType == ItemDrop.ItemData.ItemType.OneHandedWeapon &&
                __instance.IsPlayer())
            {
                List<ItemDrop.ItemData> inventoryItems = __instance.m_inventory.GetAllItems();

                foreach (ItemDrop.ItemData inventoryItem in inventoryItems)
                {
                    if (inventoryItem.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Shield)
                    {
                        if(inventoryItem.m_equiped)
                            __instance.UnequipItem(inventoryItem, false);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Re-equip items when leaving the water.
    /// </summary>
    public static class UpdateEquipmentState
    {
        public static bool shouldReequipItemsAfterSwimming = false;
    }
    [HarmonyPatch(typeof(Humanoid), "UpdateEquipment")]
    public static class Humanoid_UpdateEquipment_Patch
    {
        private static bool Prefix(Humanoid __instance)
        {
            if (!Configuration.Current.Player.IsEnabled || !Configuration.Current.Player.reequipItemsAfterSwimming || Configuration.Current.Player.dontUnequipItemsWhenSwimming)
                return true;

            if (__instance.IsPlayer() && __instance.IsSwiming() && !__instance.IsOnGround())
            {
                // The above is only enough to know we will eventually exit swimming, but we still don't know if the items were visible prior or not.
                // We only want to re-show them if they were shown to begin with, so we need to check.
                // This is also why this must be a prefix patch; in a postfix patch, the items are already hidden, and we don't know
                // if they were hidden by UpdateEquipment or by the user far earlier.

                if (__instance.m_leftItem != null || __instance.m_rightItem != null)
                    UpdateEquipmentState.shouldReequipItemsAfterSwimming = true;
            }
            else if (__instance.IsPlayer() && !__instance.IsSwiming() && __instance.IsOnGround() && UpdateEquipmentState.shouldReequipItemsAfterSwimming)
            {
                __instance.ShowHandItems();
                UpdateEquipmentState.shouldReequipItemsAfterSwimming = false;
            }

            return true;
        }
    }


    /// <summary>
    /// Removes the forced un-equip of items in your main and off-hand when entering water.
    /// </summary>
    [HarmonyPatch(typeof(Humanoid), nameof(Humanoid.UpdateEquipment))]
    public static class Player_Humanoid_UpdateEquipment
    {
        private static MethodInfo method_Humanoid_HideHandItems = AccessTools.Method(typeof(Humanoid), nameof(Humanoid.HideHandItems));
        private static MethodInfo method_HideHandItems = AccessTools.Method(typeof(Player_Humanoid_UpdateEquipment), nameof(Player_Humanoid_UpdateEquipment.HideHandItems));

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.Player.IsEnabled || !Configuration.Current.Player.dontUnequipItemsWhenSwimming) return instructions;

            List<CodeInstruction> il = instructions.ToList();

            for (int i = 0; i < il.Count; ++i)
            {
                if (il[i].Calls(method_Humanoid_HideHandItems))
                {
                    il[i - 1].opcode = OpCodes.Nop; // required to remove the this. index(0) stack value [ldarg.0]
                    il[i].operand = method_HideHandItems;
                    break;
                }
            }

            return il.AsEnumerable();
        }

        public static void HideHandItems()
        {
        }
    }


}
