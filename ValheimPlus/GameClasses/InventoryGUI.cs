using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;
using ValheimPlus.Configurations;

namespace ValheimPlus.GameClasses
{

    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.Show))]
    public class InventoryGui_Show_Patch
    {
        private const float oneRowSize = 70.5f;
        private const float containerOriginalY = -90.0f;
        private const float containerHeight = -340.0f;
        private static float lastValue = 0;

        public static void Postfix(ref InventoryGui __instance)
        {
            if (Configuration.Current.Inventory.IsEnabled)
            {
                RectTransform container = __instance.m_container;
                RectTransform player = __instance.m_player;
                GameObject playerGrid = InventoryGui.instance.m_playerGrid.gameObject;

                // Player inventory background size, only enlarge it up to 6x8 rows, after that use the scroll bar
                int playerInventoryBackgroundSize = Math.Min(6, Math.Max(4, Configuration.Current.Inventory.playerInventoryRows));
                float containerNewY = containerOriginalY - oneRowSize * playerInventoryBackgroundSize;
                // Resize player inventory
                player.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, playerInventoryBackgroundSize * oneRowSize);
                // Move chest inventory based on new player invetory size
                container.offsetMax = new Vector2(610, containerNewY);
                container.offsetMin = new Vector2(40, containerNewY + containerHeight);

                // Add player inventory scroll bar if it does not exist
                if (!playerGrid.GetComponent<InventoryGrid>().m_scrollbar)
                {
                    GameObject playerGridScroll = GameObject.Instantiate(InventoryGui.instance.m_containerGrid.m_scrollbar.gameObject, playerGrid.transform.parent);
                    playerGridScroll.name = "PlayerScroll";
                    playerGrid.GetComponent<RectMask2D>().enabled = true;
                    ScrollRect playerScrollRect = playerGrid.AddComponent<ScrollRect>();
                    playerGrid.GetComponent<RectTransform>().offsetMax = new Vector2(800f, playerGrid.GetComponent<RectTransform>().offsetMax.y);
                    playerGrid.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 1f);
                    playerScrollRect.content = playerGrid.GetComponent<InventoryGrid>().m_gridRoot;
                    playerScrollRect.viewport = __instance.m_player.GetComponentInChildren<RectTransform>();
                    playerScrollRect.verticalScrollbar = playerGridScroll.GetComponent<Scrollbar>();
                    playerGrid.GetComponent<InventoryGrid>().m_scrollbar = playerGridScroll.GetComponent<Scrollbar>();

                    playerScrollRect.horizontal = false;
                    playerScrollRect.movementType = ScrollRect.MovementType.Clamped;
                    playerScrollRect.scrollSensitivity = oneRowSize;
                    playerScrollRect.inertia = false;
                    playerScrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
                    Scrollbar playerScrollbar = playerGridScroll.GetComponent<Scrollbar>();
                    lastValue = playerScrollbar.value;
                }
            }
        }
    }

    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.RepairOneItem))]
    public static class InventoryGui_RepairOneItem_Transpiler
    {
        private static MethodInfo method_EffectList_Create = AccessTools.Method(typeof(EffectList), nameof(EffectList.Create));
        private static MethodInfo method_CreateNoop = AccessTools.Method(typeof(InventoryGui_RepairOneItem_Transpiler), nameof(InventoryGui_RepairOneItem_Transpiler.CreateNoop));

        /// <summary>
        /// Patches out the code that spawns an effect for each item repaired - when we repair multiple items, we only want
        /// one effect, otherwise it looks and sounds bad. The patch for InventoryGui.UpdateRepair will spawn the effect instead.
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpile(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.Player.IsEnabled) return instructions;

            List<CodeInstruction> il = instructions.ToList();

            if (Configuration.Current.Player.autoRepair)
            {
                // We look for a call to EffectList::Create and replace it with our own noop stub.
                for (int i = 0; i < il.Count; ++i)
                {
                    if (il[i].Calls(method_EffectList_Create))
                    {
                        il[i].opcode = OpCodes.Call; // original is callvirt, so we need to tweak it
                        il[i].operand = method_CreateNoop;
                    }
                }
            }

            return il.AsEnumerable();
        }

        private static GameObject[] CreateNoop(Vector3 _0, Quaternion _1, Transform _2, float _3, int _4)
        {
            return null;
        }
    }

    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.UpdateRepair))]
    public static class InventoryGui_UpdateRepair_Patch
    {
        /// <summary>
        /// When we're in a state where the InventoryGui is open and we have items available to repair,
        /// and we have an active crafting station, this patch is responsible for repairing all items
        /// that can be repaired and then spawning one instance of the repair effect if at least one item
        /// has been repaired.
        /// </summary>
        [HarmonyPrefix]
        public static void Prefix(InventoryGui __instance)
        {
            if (!Configuration.Current.Player.IsEnabled || !Configuration.Current.Player.autoRepair) return;

            CraftingStation curr_crafting_station = Player.m_localPlayer.GetCurrentCraftingStation();

            if (curr_crafting_station != null)
            {
                int repair_count = 0;

                while (__instance.HaveRepairableItems())
                {
                    __instance.RepairOneItem();
                    ++repair_count;
                }

                if (repair_count > 0)
                {
                    curr_crafting_station.m_repairItemDoneEffects.Create(curr_crafting_station.transform.position, Quaternion.identity, null, 1.0f);
                }
            }
        }
    }


    /// <summary>
    /// Inventory HUD setup
    /// </summary>
    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.SetupRequirement))]
    public static class InventoryGui_SetupRequirement_Patch
    {
        private static bool Prefix(Transform elementRoot, Piece.Requirement req, Player player, bool craft, int quality, ref bool __result)
        {
            if (!Configuration.Current.Hud.IsEnabled && !Configuration.Current.CraftFromChest.IsEnabled || !Configuration.Current.Hud.showRequiredItems && !Configuration.Current.CraftFromChest.IsEnabled) return true;

            Image component = elementRoot.transform.Find("res_icon").GetComponent<Image>();
            Text component2 = elementRoot.transform.Find("res_name").GetComponent<Text>();
            Text component3 = elementRoot.transform.Find("res_amount").GetComponent<Text>();
            UITooltip component4 = elementRoot.GetComponent<UITooltip>();

            if (req.m_resItem != null)
            {
                component.gameObject.SetActive(true);
                component2.gameObject.SetActive(true);
                component3.gameObject.SetActive(true);
                component.sprite = req.m_resItem.m_itemData.GetIcon();
                component.color = Color.white;
                component4.m_text = Localization.instance.Localize(req.m_resItem.m_itemData.m_shared.m_name);
                component2.text = Localization.instance.Localize(req.m_resItem.m_itemData.m_shared.m_name);
                int num = player.GetInventory().CountItems(req.m_resItem.m_itemData.m_shared.m_name);
                int amount = req.GetAmount(quality);

                if (amount <= 0)
                {
                    InventoryGui.HideRequirement(elementRoot);
                    __result = false;
                    return false;
                }

                if (Configuration.Current.CraftFromChest.IsEnabled)
                {
                    Stopwatch delta;

                    GameObject pos = player.GetCurrentCraftingStation()?.gameObject;
                    if (!pos || !Configuration.Current.CraftFromChest.checkFromWorkbench)
                    {
                        pos = player.gameObject;
                        delta = Inventory_NearbyChests_Cache.delta;
                    }
                    else
                        delta = GameObjectAssistant.GetStopwatch(pos);

                    int lookupInterval = Helper.Clamp(Configuration.Current.CraftFromChest.lookupInterval, 1, 10) * 1000;
                    if (!delta.IsRunning || delta.ElapsedMilliseconds > lookupInterval)
                    {
                        Inventory_NearbyChests_Cache.chests = InventoryAssistant.GetNearbyChests(pos, Helper.Clamp(Configuration.Current.CraftFromChest.range, 1, 50));
                        delta.Restart();
                    }
                    num += InventoryAssistant.GetItemAmountInItemList(InventoryAssistant.GetNearbyChestItemsByContainerList(Inventory_NearbyChests_Cache.chests), req.m_resItem.m_itemData);
                }

                component3.text = num + "/" + amount.ToString();

                if (num < amount)
                {
                    component3.color = ((Mathf.Sin(Time.time * 10f) > 0f) ? Color.red : Color.white);
                }
                else
                {
                    component3.color = Color.white;
                }

                component3.fontSize = 14;
                if (component3.text.Length > 5)
                {
                    component3.fontSize -= component3.text.Length - 5;
                }
            }

            __result = true;
            return false;
        }
    }

    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.DoCrafting))]
    public static class InventoryGui_DoCrafting_Transpiler
    {
        private static MethodInfo method_Player_Inventory_RemoveItem = AccessTools.Method(typeof(Inventory), nameof(Inventory.RemoveItem), new Type[] { typeof(string), typeof(int), typeof(int) });
        private static MethodInfo method_Player_GetFirstRequiredItem = AccessTools.Method(typeof(Player), nameof(Player.GetFirstRequiredItem));
        private static MethodInfo method_UseItemFromIventoryOrChest = AccessTools.Method(typeof(InventoryGui_DoCrafting_Transpiler), nameof(InventoryGui_DoCrafting_Transpiler.UseItemFromIventoryOrChest));
        private static MethodInfo method_GetFirstRequiredItemFromInventoryOrChest = AccessTools.Method(typeof(InventoryGui_DoCrafting_Transpiler), nameof(InventoryGui_DoCrafting_Transpiler.GetFirstRequiredItemFromInventoryOrChest));

        /// <summary>
        /// Patches out the code that's called when crafting.
        /// This changes the call `player.GetInventory().RemoveItem(itemData.m_shared.m_name, amount2, itemData.m_quality);`
        /// to allow crafting recipes with materials comming from containers when they have m_requireOnlyOneIngredient set to True.
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpile(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.CraftFromChest.IsEnabled) return instructions;

            List<CodeInstruction> il = instructions.ToList();

            for (int i = 0; i < il.Count; i++)
            {
                if (il[i].Calls(method_Player_GetFirstRequiredItem))
                {
                    il[i] = new CodeInstruction(OpCodes.Call, method_GetFirstRequiredItemFromInventoryOrChest);
                    il.RemoveRange(i - 6, 2);
                    break;
                }
            }
            for (int i = 0; i < il.Count; i++)
            {
                if (il[i].Calls(method_Player_Inventory_RemoveItem))
                {
                    il[i] = new CodeInstruction(OpCodes.Call, method_UseItemFromIventoryOrChest);
                    il.RemoveAt(i - 7); // removes calls to Player::GetInventory

                    return il.AsEnumerable();
                }
            }

            return instructions;
        }

        private static ItemDrop.ItemData GetFirstRequiredItemFromInventoryOrChest(Player player, Recipe recipe, int quality, out int quantity)
        {
            int extraAmount;
            ItemDrop.ItemData found = player.GetFirstRequiredItem(player.GetInventory(), recipe, quality, out quantity, out extraAmount);
            if (found != null) return found;

            GameObject pos = player.GetCurrentCraftingStation()?.gameObject;
            if (!pos || !Configuration.Current.CraftFromChest.checkFromWorkbench) pos = player.gameObject;

            List<Container> nearbyChests = InventoryAssistant.GetNearbyChests(pos, Helper.Clamp(Configuration.Current.CraftFromChest.range, 1, 50), !Configuration.Current.CraftFromChest.ignorePrivateAreaCheck);

            foreach (Container chest in nearbyChests)
            {
                found = player.GetFirstRequiredItem(chest.GetInventory(), recipe, quality, out quantity, out extraAmount);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        private static void UseItemFromIventoryOrChest(Player player, string itemName, int quantity, int quality)
        {
            Inventory playerInventory = player.GetInventory();
            if (playerInventory.CountItems(itemName, quality) >= quantity)
            {
                playerInventory.RemoveItem(itemName, quantity, quality);
                return;
            }

            GameObject pos = player.GetCurrentCraftingStation()?.gameObject;
            if (!pos || !Configuration.Current.CraftFromChest.checkFromWorkbench) pos = player.gameObject;

            List<Container> nearbyChests = InventoryAssistant.GetNearbyChests(pos, Helper.Clamp(Configuration.Current.CraftFromChest.range, 1, 50), !Configuration.Current.CraftFromChest.ignorePrivateAreaCheck);

            int toRemove = quantity;
            foreach (Container chest in nearbyChests)
            {
                Inventory chestInventory = chest.GetInventory();
                if (chestInventory.CountItems(itemName, quality) > 0)
                {
                    toRemove -= InventoryAssistant.RemoveItemFromChest(chest, itemName, toRemove);
                    if (toRemove == 0) return;
                }
            }
        }
    }
}
