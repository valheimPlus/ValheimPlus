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

        private static GameObject[] CreateNoop(Vector3 _0, Quaternion _1, Transform _2, float _3)
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
    /*
    /// <summary>
    /// Setting up deconstruct feature
    /// </summary>
    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.Awake))]
    public static class InventoryGui_Awake_Patch
    {
        private static void Postfix(ref InventoryGui __instance)
        {
            if (Configuration.Current.Deconstruct.IsEnabled)
            {
                Deconstruct.Setup(ref __instance);
            }
        }
    }

    /// <summary>
    /// Setting deconstruct tab state on update crafting panel
    /// </summary>
    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.UpdateCraftingPanel))]
    public static class InventoryGui_UpdateCraftingPanel_Patch
    {
        private static void Postfix(ref InventoryGui __instance)
        {
            if (Configuration.Current.Deconstruct.IsEnabled)
            {
                Player localPlayer = Player.m_localPlayer;

                CraftingStation currentCraftingStation = localPlayer.GetCurrentCraftingStation();

                // don't show deconstruct tab if player isn't at a crafting station and doesn't have cheats enabled or is at a crafting station without deconstruct function
                if ((currentCraftingStation == null && !localPlayer.NoCostCheat()) || Deconstruct.nonDestructableCraftingStations.Any(x => x.Equals(currentCraftingStation?.m_name)))
                {
                    Deconstruct.m_tabDeconstruct.interactable = true;
                    Deconstruct.m_tabDeconstruct.gameObject.SetActive(false);
                }
                else
                {
                    Deconstruct.m_tabDeconstruct.gameObject.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// Updating recipe list for deconstruct if deconstruct is enabled
    /// </summary>
    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.UpdateRecipeList))]
    public static class InventoryGui_UpdateRecipeList_Patch
    {
        private static bool Prefix(ref InventoryGui __instance, List<Recipe> recipes)
        {
            if (Configuration.Current.Deconstruct.IsEnabled)
            {
                Player localPlayer = Player.m_localPlayer;

                // check for active tab
                if (__instance.m_tabCraft.interactable.Equals(true) && __instance.m_tabUpgrade.interactable.Equals(true) && localPlayer != null)
                {
                    Deconstruct.Deconstruct_UpdateRecipeList(ref localPlayer);
                    return false; // skipping original update otherwise recipe list would be overwritten
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Updating recipe for deconstruct if deconstruct is enabled
    /// </summary>
    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.UpdateRecipe))]
    public static class InventoryGui_UpdateRecipe_Patch
    {
        private static bool Prefix(ref InventoryGui __instance)
        {
            if (Configuration.Current.Deconstruct.IsEnabled)
            {
                Player localPlayer = Player.m_localPlayer;

                // check for active tab
                if (__instance.m_tabCraft.interactable.Equals(true) && __instance.m_tabUpgrade.interactable.Equals(true) && localPlayer != null)
                {
                    Deconstruct.Deconstruct_UpdateRecipe(ref localPlayer, Time.deltaTime);
                    return false; // skipping original update otherwise recipe would be overwritten
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Accounting for deconstruct tab in craft tab press
    /// </summary>
    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.OnCraftPressed))]
    public static class InventoryGui_OnCraftPressed_Patch
    {
        private static bool Prefix(ref InventoryGui __instance)
        {
            if (Configuration.Current.Deconstruct.IsEnabled)
            {
                if (Deconstruct.InDeconstructTab())
                {
                    Deconstruct.OnDeconstructPressed();
                    return false; // skipping original so that we only run deconstruct button function
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Accounting for deconstruct tab in craft tab press
    /// </summary>
    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.OnTabCraftPressed))]
    public static class InventoryGui_OnTabCraftPressed_Patch
    {
        private static void Prefix(ref InventoryGui __instance)
        {
            if (Configuration.Current.Deconstruct.IsEnabled)
            {
                Deconstruct.SetDeconstructTab(true);
            }
        }
    }

    /// <summary>
    /// Accounting for deconstruct tab in upgrade tab press
    /// </summary>
    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.OnTabUpgradePressed))]
    public static class InventoryGui_OnTabUpgradePressed_Patch
    {
        private static void Prefix(ref InventoryGui __instance)
        {
            if (Configuration.Current.Deconstruct.IsEnabled)
            {
                Deconstruct.SetDeconstructTab(true);
            }
        }
    }
    */

    /// <summary>
    /// Inventory HUD setup
    /// </summary>
    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.SetupRequirement))]
    public static class InventoryGui_SetupRequirement_Patch
    {
        private static bool Prefix(Transform elementRoot, Piece.Requirement req, Player player, bool craft, int quality, ref bool __result)
        {
            if (!Configuration.Current.Hud.IsEnabled || !Configuration.Current.Hud.showRequiredItems) return true;

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

    public static class AutoSplitState
    {
        public static bool altHeld = false;
        public static bool shiftHeld = false;
    }

    /// <summary>
    /// Note: Patch on InventoryGrid. Patch in detection for holding alt while clicking a stack, used in InventoryGui to auto-split stacks to what you can carry.
    /// </summary>
    [HarmonyPatch(typeof(InventoryGrid), nameof(InventoryGrid.OnLeftClick))]
    public class InventoryGrid_OnLeftClick_Patch
    {
        public static bool Prefix(InventoryGrid __instance)
        {
            AutoSplitState.altHeld = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
            AutoSplitState.shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            return true;
        }
    }

    /// <summary>
    /// Patch to allow alt+click (or alt+shift+click) to take as much of a stack as you can carry (or if encumbered, leave as little as necessary to get to your max carry weight).
    /// </summary>
    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.OnSelectedItem))]
    public class InventoryGui_OnSelectedItem_Transpiler
    {
        private static readonly FieldInfo field_altHeld = AccessTools.Field(typeof(AutoSplitState), nameof(AutoSplitState.altHeld));
        private static readonly MethodInfo method_calculateAndAutoSplit = AccessTools.Method(typeof(InventoryGui_OnSelectedItem_Transpiler), nameof(calculateAndAutoSplit));

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            if (!Configuration.Current.Inventory.IsEnabled || !Configuration.Current.Inventory.holdAltToAutoSplit)
                return instructions;

            List<CodeInstruction> il = instructions.ToList();

            for (int i = 0; i < il.Count; i++)
            {
                // Original header: private void OnSelectedItem(InventoryGrid grid, ItemDrop.ItemData item, Vector2i pos, InventoryGrid.Modifier mod)
                //
                // The original method is large and split into several pieces. First, if m_dragGo != null it handles a drag where the user input is done.
                // We can leave all that be. Then, it if item != null it *sets up* a drag. We need to patch in here prior to the original checks,
                // to override the original modifier (e.g. Move, Split), and return when we're done.
                // So our goal is roughly:
                // ... else if (item != null)
                // {
                //     if (AutoSplitState.altHeld) { calculateAndSplit(); return; } // Added by us
                //     if (mod == InventoryGrid.Modifier.Move) { ... }              // Original code
                // }

                // Insert our code between IL_01be: brfalse and IL_01c3: ldarg.s mod (start of the if/else-chain or switch statement).
                // As of this writing the first Ldarg_S in the IL uniquely identifies the correct location, but add a few checks just in case.
                // We want to jump to the original Ldarg_S in case altHeld is false.
                if (i + 2 >= il.Count || il[i].opcode != OpCodes.Brfalse || il[i + 1].opcode != OpCodes.Ldarg_S || il[i + 2].opcode != OpCodes.Ldc_I4_2)
                    continue;

                Label switchLabel = generator.DefineLabel();
                il[i + 1].labels.Add(switchLabel);

                // if (AutoSplitState.altHeld)
                il.Insert(++i, new CodeInstruction(OpCodes.Ldsfld, field_altHeld));
                il.Insert(++i, new CodeInstruction(OpCodes.Brfalse, switchLabel));

                // calculateAndSplit(...); push arguments and call.
                // We need "this"/__instance (arg 0), "grid" (arg 1), "item" (arg 2), plus the local variable "localPlayer" (local 0).
                // We push them in the same order as in the method signature for calculateAndSplit below.
                il.Insert(++i, new CodeInstruction(OpCodes.Ldarg_0));
                il.Insert(++i, new CodeInstruction(OpCodes.Ldloc_0));
                il.Insert(++i, new CodeInstruction(OpCodes.Ldarg_1));
                il.Insert(++i, new CodeInstruction(OpCodes.Ldarg_2));
                il.Insert(++i, new CodeInstruction(OpCodes.Call, method_calculateAndAutoSplit));

                // return
                il.Insert(++i, new CodeInstruction(OpCodes.Ret));

                return il.AsEnumerable();
            }

            ZLog.Log("Failed to transpile InventoryGui.OnSelectedItem to patch auto-split (alt-click stack in container)");

            return instructions;
        }

        /// <summary>
        /// Called by our patched OnSelectedItem to split/move stacks when alt is held.
        /// </summary>
        private static void calculateAndAutoSplit(InventoryGui inventoryGui, Player localPlayer, InventoryGrid grid, ItemDrop.ItemData item)
        {
            // If alt+click item in inventory: remove as few items as possible to become unencumbered. Move to container if one is open, drop if only player inventory is open.
            // If alt+click item in container: take as many items as possible without becoming encumbered.
            // Also holding shift (so shift+alt) toggles whether it moves automatically (like ctrl+click does) or whether you need to place the item yourself (like shift+click to split does).
            //
            // This method is essentially a method on InventoryGui where the variable inventoryGui is the "this" pointer.

            // containerInventory is null if we open the player inventory to drop items
            Inventory fromInventory = grid.GetInventory();
            Inventory playerInventory = localPlayer.GetInventory();
            Inventory containerInventory = inventoryGui.m_currentContainer != null ? inventoryGui.m_currentContainer.GetInventory() : null;
            if (fromInventory == null || playerInventory == null)
                return;

            int amountToMove = 0;

            // There are three main cases: player inventory to container, container to player inventory, and player inventory to nothing -- dropping the item.
            if (fromInventory == containerInventory)
            {
                amountToMove = calculateMoveFromContainer(localPlayer, item);
                if (amountToMove <= 0)
                    return;
            }
            else if (fromInventory == playerInventory)
            {
                // Note: This may be the amount to move to the container, or the amount to drop (if containerInventory == null).
                amountToMove = calculateMoveFromInventory(localPlayer, item, containerInventory);
                if (amountToMove <= 0)
                    return;
            }

            // We now know the amount; there are now three possible actions: drop, move and split (to let the user to select the destination slot).
            if (containerInventory == null)
            {
                // Drop this item
                if (localPlayer.DropItem(playerInventory, item, amountToMove))
                    inventoryGui.m_moveItemEffects.Create(inventoryGui.transform.position, Quaternion.identity, null, 1f);
                else
                    MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, $"$msg_cantdrop");
            }
            else if ((!Configuration.Current.Inventory.autoMoveByDefault && !AutoSplitState.shiftHeld) || (Configuration.Current.Inventory.autoMoveByDefault && AutoSplitState.shiftHeld))
            {
                // Setup a drag, to let the player store it in the slot of their choice (or drop it), similar to the game's "split"
                inventoryGui.SetupDragItem(item, fromInventory, amountToMove);
            }
            else if ((Configuration.Current.Inventory.autoMoveByDefault && !AutoSplitState.shiftHeld) || (!Configuration.Current.Inventory.autoMoveByDefault && AutoSplitState.shiftHeld))
            {
                // Move the item automatically (like plain ctrl) instead of making the user put it down in a slot (like split with plain shift)
                // I'd like this to be Ctrl+Alt, but right Alt is Alt Gr on many keyboards, which means holding just right Alt is the same as left Ctrl + left Alt.
                var targetInventory = (fromInventory == playerInventory) ? containerInventory : playerInventory;
                moveItemsAutomatically(inventoryGui, localPlayer, item, amountToMove, fromInventory, targetInventory);
            }
        }

        /// <summary>
        /// Calculates how many items we should move (or drop) from the player inventory
        /// </summary>
        /// <returns>Number of items to move/drop</returns>
        private static int calculateMoveFromInventory(Player localPlayer, ItemDrop.ItemData item, Inventory containerInventory)
        {
            Inventory playerInventory = localPlayer.GetInventory();

            // Handle the case of moving from the player's inventory to a container, or to drop. Move/drop as little as possible to become unencumbered.
            if (localPlayer.GetMaxCarryWeight() >= playerInventory.GetTotalWeight())
            {
                MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, "You are not encumbered");
                return 0;
            }
            else if (containerInventory != null && !containerInventory.CanAddItem(item, 1))
            {
                MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, "No room in container");
                return 0;
            }

            // Note: There's a possible rounding error here, not sure how to fix. I had a total weight of 300.1, and
            // this code moved 2 raspberries, but you're not encumbered if you only move 1.
            // 2 are moved because 300.1 - 300 results in 0.1000061, meaning moving 0.1 won't get you below 300,
            // so amountToMove becomes 2 due to the ceil operation.
            float amountOverMax = playerInventory.GetTotalWeight() - localPlayer.GetMaxCarryWeight();
            int amountToMove = Math.Min(item.m_stack, Mathf.CeilToInt(amountOverMax / item.m_shared.m_weight));
            if (containerInventory != null && !containerInventory.HaveEmptySlot())
                amountToMove = Math.Min(amountToMove, containerInventory.FindFreeStackSpace(item.m_shared.m_name));

            if (amountToMove <= 0)
                MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, "No room in container");

            return amountToMove;
        }

        /// <summary>
        /// Calculates how many items we should move from the container to the player inventory.
        /// </summary>
        /// <returns>Number of items to move</returns>
        private static int calculateMoveFromContainer(Player localPlayer, ItemDrop.ItemData item)
        {
            Inventory playerInventory = localPlayer.GetInventory();
            if (!playerInventory.CanAddItem(item, 1))
            {
                MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, "$msg_noroom");
                return 0;
            }

            int amountToMove = 0;

            if (localPlayer.GetMaxCarryWeight() >= item.GetWeight() + playerInventory.GetTotalWeight() && playerInventory.CanAddItem(item, item.m_stack))
            {
                amountToMove = item.m_stack;
            }
            else
            {
                // Take the minimum of three things: the stack size (can't take more than the stack!), what we can carry, and what fits in our inventory slots.
                float availableWeight = localPlayer.GetMaxCarryWeight() - playerInventory.GetTotalWeight();
                amountToMove = Math.Min(item.m_stack, Mathf.FloorToInt(availableWeight / item.m_shared.m_weight));
                if (!playerInventory.HaveEmptySlot())
                    amountToMove = Math.Min(amountToMove, playerInventory.FindFreeStackSpace(item.m_shared.m_name));
            }

            if (amountToMove <= 0)
                MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, "You can't carry any more");

            return amountToMove;
        }

        /// <summary>
        ///  Moves a stack of items, placing them in existing stacks first as far as possible.
        /// </summary>
        private static void moveItemsAutomatically(InventoryGui inventoryGui, Player localPlayer, ItemDrop.ItemData item, int amountToMove, Inventory fromInventory, Inventory targetInventory)
        {
            if (item.m_shared.m_questItem)
                return;

            localPlayer.RemoveFromEquipQueue(item);
            localPlayer.UnequipItem(item, true);

            var initialAmountToMove = amountToMove;

            // We may need to move parts of the stack into MULTIPLE target stacks before trying an empty slot (if needed).
            for (int y = 0; y < targetInventory.m_height && amountToMove > 0; y++)
            {
                for (int x = 0; x < targetInventory.m_width && amountToMove > 0; x++)
                {
                    // This has pretty bad time complexity, but the number of loops is usually "small" (like a few thousand or less).
                    var targetItem = targetInventory.GetItemAt(x, y);
                    if (targetItem == null || targetItem.m_shared.m_name != item.m_shared.m_name || targetItem.m_stack >= targetItem.m_shared.m_maxStackSize) continue;

                    // Note that MoveItemToThis also subtracts the amount from the source stack, and removes the item from the source if the stack becomes 0.
                    int amount = Math.Min(amountToMove, targetItem.m_shared.m_maxStackSize - targetItem.m_stack);
                    int oldStack = item.m_stack;
                    targetInventory.MoveItemToThis(fromInventory, item, amount, x, y);
                    amountToMove -= oldStack - item.m_stack;
                }
            }

            // We've now moved as much as possible into other stacks of the same item. Let's see if we still have stuff to move.
            if (amountToMove > 0)
            {
                var slot = targetInventory.FindEmptySlot(true);
                if (slot.x != -1 && slot.y != -1)
                {
                    targetInventory.MoveItemToThis(fromInventory, item, amountToMove, slot.x, slot.y);
                    amountToMove = 0;
                }
            }

            if (amountToMove != initialAmountToMove)
                inventoryGui.m_moveItemEffects.Create(inventoryGui.transform.position, Quaternion.identity, null, 1f);
        }
    }
}
