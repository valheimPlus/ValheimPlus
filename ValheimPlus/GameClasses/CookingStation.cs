using HarmonyLib;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus.GameClasses
{
    [HarmonyPatch(typeof(CookingStation), nameof(CookingStation.FindCookableItem))]
    public static class CookingStation_FindCookableItem_Transpiler
    {
        private static List<Container> nearbyChests = null;

        private static MethodInfo method_PullCookableItemFromNearbyChests = AccessTools.Method(typeof(CookingStation_FindCookableItem_Transpiler), nameof(CookingStation_FindCookableItem_Transpiler.PullCookableItemFromNearbyChests));

        /// <summary>
        /// Patches out the code that looks for cookable items in player inventory.
        /// When not cookables items have been found in the player inventory, check inside nearby chests.
        /// If found, remove the item from the chests it was in, instantiate a game object and returns it so it can be placed on the CookingStation.
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.CraftFromChest.IsEnabled) return instructions;

            List<CodeInstruction> il = instructions.ToList();
            int endIdx = -1;
            for (int i = 0; i < il.Count; i++)
            {
                if (il[i].opcode == OpCodes.Ldnull)
                {
                    il[i] = new CodeInstruction(OpCodes.Ldarg_0)
                    {
                        labels = il[i].labels
                    };
                    il.Insert(++i, new CodeInstruction(OpCodes.Call, method_PullCookableItemFromNearbyChests));
                    il.Insert(++i, new CodeInstruction(OpCodes.Stloc_3));
                    il.Insert(++i, new CodeInstruction(OpCodes.Ldloc_3));
                    endIdx = i;
                    break;
                }
            }
            if (endIdx == -1)
            {
                ZLog.LogError("Failed to apply CookingStation_FindCookableItem_Transpiler");
                return instructions;
            }

            return il.AsEnumerable();
        }

        private static ItemDrop.ItemData PullCookableItemFromNearbyChests(CookingStation station)
        {
            if (station.GetFreeSlot() == -1) return null;

            Stopwatch delta = GameObjectAssistant.GetStopwatch(station.gameObject);

            int lookupInterval = Helper.Clamp(Configuration.Current.CraftFromChest.lookupInterval, 1, 10) * 1000;
            if (!delta.IsRunning || delta.ElapsedMilliseconds > lookupInterval)
            {
                nearbyChests = InventoryAssistant.GetNearbyChests(station.gameObject, Helper.Clamp(Configuration.Current.CraftFromChest.range, 1, 50), !Configuration.Current.CraftFromChest.ignorePrivateAreaCheck);
                delta.Restart();
            }

            foreach (CookingStation.ItemConversion itemConversion in station.m_conversion)
            {
                ItemDrop.ItemData itemData = itemConversion.m_from.m_itemData;

                foreach (Container c in nearbyChests)
                {
                    if (c.GetInventory().HaveItem(itemData.m_shared.m_name))
                    {
                        // Remove one item from chest
                        InventoryAssistant.RemoveItemFromChest(c, itemData);
                        // Instantiate cookabled GameObject
                        GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(itemConversion.m_from.gameObject.name);

                        ZNetView.m_forceDisableInit = true;
                        GameObject cookabledItem = UnityEngine.Object.Instantiate<GameObject>(itemPrefab);
                        ZNetView.m_forceDisableInit = false;

                        return cookabledItem.GetComponent<ItemDrop>().m_itemData;
                    }
                }
            }
            return null;
        }
    }
}
