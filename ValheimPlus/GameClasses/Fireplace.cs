using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus.GameClasses
{
    class FireplaceFuel
    {
        [HarmonyPatch(typeof(Fireplace), "Awake")]
        public static class Fireplace_Awake_Patch
        {
            /// <summary>
            /// When fire source is created, check for configurations and set its start fuel to max fuel
            /// </summary>
            private static void Prefix(ref Fireplace __instance)
            {
                if (!Configuration.Current.FireSource.IsEnabled) return;

                if (FireplaceExtensions.IsTorch(__instance.m_name))
                {
                    if (Configuration.Current.FireSource.torches)
                    {
                        __instance.m_startFuel = __instance.m_maxFuel;
                    }
                }
                else if (Configuration.Current.FireSource.fires)
                {
                    __instance.m_startFuel = __instance.m_maxFuel;
                }
            }
        }

        [HarmonyPatch(typeof(Fireplace), "GetTimeSinceLastUpdate")]
        public static class Fireplace_GetTimeSinceLastUpdate_Patch
        {
            /// <summary>
            /// If fire source is configured to keep fire source lit, reset time since being lit to 0
            /// </summary>
            private static void Postfix(ref double __result, ref Fireplace __instance)
            {
                if (!Configuration.Current.FireSource.IsEnabled) return;

                if (FireplaceExtensions.IsTorch(__instance.m_name))
                {
                    if (Configuration.Current.FireSource.torches)
                    {
                        __result = 0.0;
                    }
                }
                else if (Configuration.Current.FireSource.fires)
                {
                    __result = 0.0;
                }
            }
        }

        [HarmonyPatch(typeof(Fireplace), nameof(Fireplace.UpdateFireplace))]
        public static class Fireplace_UpdateFireplace_Transpiler
        {
            private static MethodInfo method_ZNetView_IsOwner = AccessTools.Method(typeof(ZNetView), nameof(ZNetView.IsOwner));
            private static MethodInfo method_addFuelFromNearbyChests = AccessTools.Method(typeof(Fireplace_UpdateFireplace_Transpiler), nameof(Fireplace_UpdateFireplace_Transpiler.AddFuelFromNearbyChests));

            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                if (!Configuration.Current.FireSource.IsEnabled || !Configuration.Current.FireSource.autoFuel) return instructions;

                List<CodeInstruction> il = instructions.ToList();

                for (int i = 0; i < il.Count; i++)
                {
                    if (il[i].Calls(method_ZNetView_IsOwner))
                    {
                        ++i;
                        il.Insert(++i, new CodeInstruction(OpCodes.Ldarg_0));
                        il.Insert(++i, new CodeInstruction(OpCodes.Call, method_addFuelFromNearbyChests));

                        return il.AsEnumerable();
                    }
                }

                ZLog.LogError("Failed to apply Fireplace_UpdateFireplace_Transpiler");

                return instructions;
            }

            private static void AddFuelFromNearbyChests(Fireplace __instance)
            {
                int toMaxFuel = (int)__instance.m_maxFuel - (int)Math.Ceiling(__instance.m_nview.GetZDO().GetFloat("fuel"));

                if (toMaxFuel > 0)
                {
                    Stopwatch delta = GameObjectAssistant.GetStopwatch(__instance.gameObject);
                    
                    if (delta.IsRunning && delta.ElapsedMilliseconds < 1000) return;
                    delta.Restart();

                    ItemDrop.ItemData fuelItemData = __instance.m_fuelItem.m_itemData;

                    int addedFuel = InventoryAssistant.RemoveItemInAmountFromAllNearbyChests(__instance.gameObject, Helper.Clamp(Configuration.Current.FireSource.autoRange, 1, 50), fuelItemData, toMaxFuel, !Configuration.Current.FireSource.ignorePrivateAreaCheck);
                    for (int i = 0; i < addedFuel; i++)
                    {
                        __instance.m_nview.InvokeRPC("AddFuel", new object[] { });
                    }
                    if (addedFuel > 0)
                        ZLog.Log("Added " + addedFuel + " fuel(" + fuelItemData.m_shared.m_name + ") in " + __instance.m_name);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Fireplace), nameof(Fireplace.Interact))]
    public static class Fireplace_Interact_Transpiler
    {
        private static List<Container> nearbyChests = null;

        private static MethodInfo method_Inventory_HaveItem = AccessTools.Method(typeof(Inventory), nameof(Inventory.HaveItem));
        private static MethodInfo method_ReplaceInventoryRefByChest = AccessTools.Method(typeof(Fireplace_Interact_Transpiler), nameof(Fireplace_Interact_Transpiler.ReplaceInventoryRefByChest));

        /// <summary>
        /// Patches out the code that looks for fuel item.
        /// When no fuel item has been found in the player inventory, check inside nearby chests.
        /// If found, replace the reference to the player Inventory by the one from the chest.
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.CraftFromChest.IsEnabled) return instructions;

            List<CodeInstruction> il = instructions.ToList();

            for (int i = 0; i < il.Count; i++)
            {
                if (il[i].Calls(method_Inventory_HaveItem)) // look for the last access to user
                {
                    il[i - 6] = new CodeInstruction(OpCodes.Ldloca, 0);
                    il[i] = new CodeInstruction(OpCodes.Call, method_ReplaceInventoryRefByChest);
                    il.RemoveRange(i - 2, 2);
                    il.Insert(i - 2, new CodeInstruction(OpCodes.Ldarg_0));

                    return il.AsEnumerable();
                }
            }

            ZLog.LogError("Failed to apply Fireplace_Interact_Transpiler");

            return instructions;
        }

        private static bool ReplaceInventoryRefByChest(ref Inventory inventory, ItemDrop.ItemData item, Fireplace fireplace)
        {
            if (inventory.HaveItem(item.m_shared.m_name)) return true; // original code

            Stopwatch delta = GameObjectAssistant.GetStopwatch(fireplace.gameObject);
            int lookupInterval = Helper.Clamp(Configuration.Current.CraftFromChest.lookupInterval, 1, 10) * 1000;
            if (!delta.IsRunning || delta.ElapsedMilliseconds > lookupInterval)
            {
                nearbyChests = InventoryAssistant.GetNearbyChests(fireplace.gameObject, Helper.Clamp(Configuration.Current.CraftFromChest.range, 1, 50), !Configuration.Current.CraftFromChest.ignorePrivateAreaCheck);
                delta.Restart();
            }

            foreach (Container c in nearbyChests)
            {
                if (c.GetInventory().HaveItem(item.m_shared.m_name))
                {
                    inventory = c.GetInventory();
                    return true;
                }
            }

            return false;
        }
    }

    public static class FireplaceExtensions
    {
        static readonly string[] torchItemNames = new[]
        {
            "$piece_groundtorchwood", // standing wood torch
            "$piece_groundtorch", // standing iron torch
            "$piece_groundtorchgreen", // standing green torch
            "$piece_sconce", // sconce torch
            "$piece_brazierceiling01" // brazier
        };

        internal static bool IsTorch(string itemName)
        {
            return torchItemNames.Any(x => x.Equals(itemName));
        }
    }
}