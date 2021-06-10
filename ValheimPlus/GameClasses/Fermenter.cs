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

    [HarmonyPatch(typeof(Fermenter), "GetHoverText")]
    public static class Fermenter_GetHoverText_Patch
    {
        private static bool Prefix(ref Fermenter __instance, ref string __result)
        {
            if (!Configuration.Current.Fermenter.IsEnabled || !Configuration.Current.Fermenter.showDuration)
                return true;

            if (!PrivateArea.CheckAccess(__instance.transform.position, 0f, false, true))
            {
                __result = Localization.instance.Localize(__instance.m_name + "\n$piece_noaccess");
                return false;
            }
            switch (__instance.GetStatus())
            {
                case Fermenter.Status.Empty:
                    __result = Localization.instance.Localize(__instance.m_name + " ( $piece_container_empty )\n[<color=yellow><b>$KEY_Use</b></color>] $piece_fermenter_add");
                    return false;
                case Fermenter.Status.Fermenting:
                    {
                        string contentName = __instance.GetContentName();

                        if (__instance.m_exposed)
                        {
                            __result = Localization.instance.Localize(__instance.m_name + " ( " + contentName + ", $piece_fermenter_exposed )");
                            return false;
                        }

                        double durationUntilDone = (double)__instance.m_fermentationDuration - __instance.GetFermentationTime();

                        string info = "";

                        int minutes = (int)durationUntilDone / 60;

                        if (((int)durationUntilDone) >= 120)
                            info = minutes + " minutes";
                        else
                            info = (int)durationUntilDone + " seconds";

                        __result = Localization.instance.Localize(__instance.m_name + " ( " + contentName + ", $piece_fermenter_fermenting )") + " (" + info + ")";
                        return false;
                    }
                case Fermenter.Status.Ready:
                    {
                        string contentName2 = __instance.GetContentName();
                        __result = Localization.instance.Localize(__instance.m_name + " ( " + contentName2 + ", $piece_fermenter_ready )\n[<color=yellow><b>$KEY_Use</b></color>] $piece_fermenter_tap");
                        return false;
                    }
            }
            __result = __instance.m_name;

            return false;
        }
    }

    // Update 0.154.1 refactored the name to SlowUpdate from UpdateVis
    [HarmonyPatch(typeof(Fermenter), nameof(Fermenter.SlowUpdate))]
    public static class Fermenter_SlowUpdate_Transpiler
    {
        private static MethodInfo method_GameObject_SetActive = AccessTools.Method(typeof(GameObject), nameof(GameObject.SetActive));
        private static MethodInfo method_InvokeRPCTap = AccessTools.Method(typeof(Fermenter_SlowUpdate_Transpiler), nameof(Fermenter_SlowUpdate_Transpiler.InvokeRPCTap));
        private static MethodInfo method_AddItemFromNearbyChests = AccessTools.Method(typeof(Fermenter_SlowUpdate_Transpiler), nameof(Fermenter_SlowUpdate_Transpiler.AddItemFromNearbyChests));

        /// <summary>
        /// Patches out the code that check for Fermenter status.
        /// Enables autoFuel and autoDeposit features.
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.Fermenter.IsEnabled) return instructions;

            List<CodeInstruction> il = instructions.ToList();

            int hitCount = 0;
            bool found = false;
            for (int i = 0; i < il.Count; i++)
            {
                if (il[i].Calls(method_GameObject_SetActive))
                    hitCount++;
                if (hitCount == 3)
                {
                    il.Insert(++i, new CodeInstruction(OpCodes.Ldarg_0));
                    il.Insert(++i, new CodeInstruction(OpCodes.Call, method_AddItemFromNearbyChests));
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                ZLog.LogError("Failed to apply Fermenter_SlowUpdate_Transpiler 1");
                return instructions;
            }
            found = false;
            for (int i = il.Count - 1; i >= 0; i--)
            {
                if (il[i].Calls(method_GameObject_SetActive))
                {
                    il.Insert(++i, new CodeInstruction(OpCodes.Ldarg_0));
                    il.Insert(++i, new CodeInstruction(OpCodes.Call, method_InvokeRPCTap));

                    return il.AsEnumerable();
                }
            }

            ZLog.LogError("Failed to apply Fermenter_SlowUpdate_Transpiler 2");

            return instructions;
        }

        private static void InvokeRPCTap(Fermenter __instance)
        {
            if (!Configuration.Current.Fermenter.autoDeposit) return;

            __instance.m_nview.InvokeRPC("Tap", new object[] { });
        }

        private static void AddItemFromNearbyChests(Fermenter __instance)
        {
            if (!Configuration.Current.Fermenter.autoFuel || __instance.GetStatus() != Fermenter.Status.Empty || !__instance.m_nview.IsOwner()) return;

            Stopwatch delta = GameObjectAssistant.GetStopwatch(__instance.gameObject);
            if (!delta.IsRunning || delta.ElapsedMilliseconds > 1000)
            {
                List<Container> nearbyChests = InventoryAssistant.GetNearbyChests(__instance.gameObject, Configuration.Current.Fermenter.autoRange, !Configuration.Current.Fermenter.ignorePrivateAreaCheck);
                foreach (Container c in nearbyChests)
                {
                    ItemDrop.ItemData item = __instance.FindCookableItem(c.GetInventory());
                    if (item != null)
                    {
                        if (InventoryAssistant.RemoveItemFromChest(c, item) == 0) continue;

                        __instance.m_nview.InvokeRPC("AddItem", new object[] { item.m_dropPrefab.name });
                        ZLog.Log("Added " + item.m_shared.m_name + " to " + __instance.m_name);
                        break;
                    }
                }
                delta.Restart();
            }
        }
    }

    [HarmonyPatch(typeof(Fermenter), nameof(Fermenter.DelayedTap))]
    public static class Fermenter_DelayedTap_Transpiler
    {
        private static MethodInfo method_Object_Instantiate = AccessTools.Method(typeof(Object), nameof(Object.Instantiate), new System.Type[] { typeof(ItemDrop), typeof(Vector3), typeof(Quaternion) });
        private static MethodInfo method_DropItemToNearbyChest = AccessTools.Method(typeof(Fermenter_DelayedTap_Transpiler), nameof(Fermenter_DelayedTap_Transpiler.DropItemToNearbyChest));

        /// <summary>
        /// Patches out the code that that triggers when interacting with a Fermenter.
        /// This handles the autoDesposit feature.
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.Fermenter.IsEnabled || !Configuration.Current.Fermenter.autoDeposit) return instructions;

            List<CodeInstruction> il = instructions.ToList();

            int brFalsePos = -1;
            for (int i = 0; i < il.Count; i++)
            {
                if (il[i].opcode == OpCodes.Brfalse)
                {
                    brFalsePos = i;
                    il.Insert(++i, new CodeInstruction(OpCodes.Ldarg_0));
                    il.Insert(++i, new CodeInstruction(OpCodes.Ldloca, 0));
                    il.Insert(++i, new CodeInstruction(OpCodes.Call, method_DropItemToNearbyChest));
                    il.Insert(++i, new CodeInstruction(OpCodes.Brtrue, il[brFalsePos].operand));

                    return il.AsEnumerable();
                }
            }

            ZLog.LogError("Failed to apply Fermenter_DelayedTap_Transpiler");

            return instructions;
        }

        private static bool DropItemToNearbyChest(Fermenter __instance, ref Fermenter.ItemConversion itemConversion)
        {
            List<Container> nearbyChests = InventoryAssistant.GetNearbyChests(__instance.gameObject, Configuration.Current.Fermenter.autoRange, !Configuration.Current.Fermenter.ignorePrivateAreaCheck);

            int spawnedInChests = 0;
            for (int i = 0; i < itemConversion.m_producedItems; i++)
            {
                GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(itemConversion.m_to.gameObject.name);

                ZNetView.m_forceDisableInit = true;
                GameObject itemObject = Object.Instantiate<GameObject>(itemPrefab);
                ZNetView.m_forceDisableInit = false;

                ItemDrop comp = itemObject.GetComponent<ItemDrop>();

                bool result = spawnNearbyChest(comp, true);
                Object.Destroy(itemObject);
                if (!result)
                {
                    itemConversion.m_producedItems -= spawnedInChests;

                    return false;
                }
                spawnedInChests++;
            }

            return true;

            bool spawnNearbyChest(ItemDrop item, bool mustHaveItem)
            {
                foreach (Container chest in nearbyChests)
                {
                    Inventory cInventory = chest.GetInventory();
                    if (mustHaveItem && !cInventory.HaveItem(item.m_itemData.m_shared.m_name))
                        continue;

                    if (!cInventory.AddItem(item.m_itemData))
                    {
                        //Chest full, move to the next
                        continue;
                    }

                    InventoryAssistant.ConveyContainerToNetwork(chest);

                    return true;
                }

                if (mustHaveItem)
                    return spawnNearbyChest(item, false);

                return false;
            }
        }
    }
}
