using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus.GameClasses
{
    /// <summary>
    /// Alters teleportation prevention
    /// </summary>
    [HarmonyPatch(typeof(Inventory), "IsTeleportable")]
    public static class NoItemTeleportPrevention
    {
        private static void Postfix(ref bool __result)
        {
            if (!Configuration.Current.Items.IsEnabled) return;
            if (Configuration.Current.Items.noTeleportPrevention)
                __result = true;
        }
    }

    /// <summary>
    /// Makes all items fill inventories top to bottom instead of just tools and weapons
    /// </summary>
    [HarmonyPatch(typeof(Inventory), "TopFirst")]
    public static class Inventory_TopFirst_Patch
    {
        public static bool Prefix(ref bool __result)
        {
            if (!Configurations.Configuration.Current.Inventory.IsEnabled ||
                !Configurations.Configuration.Current.Inventory.inventoryFillTopToBottom) return true;
            __result = true;
            return false;
        }
    }

    /// <summary>
    /// Configure player inventory size
    /// </summary>
    [HarmonyPatch(typeof(Inventory), MethodType.Constructor, new Type[] { typeof(string), typeof(Sprite), typeof(int), typeof(int) })]
    public static class Inventory_Constructor_Patch
    {
        private const int PlayerInventoryMaxRows = 20;
        private const int PlayerInventoryMinRows = 4;
        private const int PlayerInventoryDefaultCols = 8;

        public static void Prefix(string name, ref int defaultWidth, ref int defaultHeight)
        {
            if (!Configuration.Current.Inventory.IsEnabled) return;
            // Player inventory
            if (defaultHeight == PlayerInventoryMinRows &&
                defaultWidth == PlayerInventoryDefaultCols || name == "Inventory")
            {
                defaultHeight = Helper.Clamp(Configuration.Current.Inventory.playerInventoryRows, PlayerInventoryMinRows, PlayerInventoryMaxRows);
            }
        }
    }


    public static class Inventory_NearbyChests_Cache
    {
        public static List<Container> Chests = new List<Container>();
        public static readonly Stopwatch Delta = new Stopwatch();
    }

    /// <summary>
    /// When merging another inventory, try to merge items with existing stacks.
    /// </summary>
    [HarmonyPatch(typeof(Inventory), "MoveAll")]
    public static class Inventory_MoveAll_Patch
    {
        private static void Prefix(ref Inventory __instance, ref Inventory fromInventory)
        {
            if (!Configuration.Current.Inventory.IsEnabled ||
                !Configuration.Current.Inventory.mergeWithExistingStacks) return;

            foreach (var itemData in fromInventory.GetAllItems()
                .Where(otherItem => otherItem.m_shared.m_maxStackSize > 1))
            {
                foreach (var myItem in __instance.m_inventory)
                {
                    if (ItemDataOrQualityDoesNotMatch(myItem, itemData)) continue;

                    var itemsToMove = Math.Min(myItem.m_shared.m_maxStackSize - myItem.m_stack, itemData.m_stack);
                    myItem.m_stack += itemsToMove;

                    if (itemData.m_stack == itemsToMove)
                    {
                        fromInventory.RemoveItem(itemData);
                        break;
                    }

                    itemData.m_stack -= itemsToMove;
                }
            }
        }

        private static bool ItemDataOrQualityDoesNotMatch(ItemDrop.ItemData myItem, ItemDrop.ItemData itemData)
        {
            return myItem.m_shared.m_name != itemData.m_shared.m_name ||
                   myItem.m_quality != itemData.m_quality;
        }
    }



}
