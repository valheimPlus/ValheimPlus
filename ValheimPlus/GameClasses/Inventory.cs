using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using ValheimPlus.Configurations;
using ValheimPlus.UI;

namespace ValheimPlus
{
    /// <summary>
    /// Alters teleportation prevention
    /// </summary>
    [HarmonyPatch(typeof(Inventory), "IsTeleportable")]
    public static class noItemTeleportPrevention
    {
        private static void Postfix(ref bool __result)
        {
            if (Configuration.Current.Items.IsEnabled)
            {
                if (Configuration.Current.Items.noTeleportPrevention)
                    __result = true;
            }
        }
    }

    [HarmonyPatch(typeof(Inventory), "TopFirst")]
    public static class Inventory_TopFirst_Patch
    {
        public static bool Prefix(ref bool __result)
        {
            if (Configurations.Configuration.Current.Inventory.IsEnabled &&
                Configurations.Configuration.Current.Inventory.inventoryFillTopToBottom)
            {
                __result = true;
                return false;
            }
            else return true;
        }
    }

    [HarmonyPatch(typeof(Inventory), MethodType.Constructor, new Type[] { typeof(string), typeof(Sprite), typeof(int), typeof(int) })]
    public static class Inventory_Constructor_Patch
    {
        private const int playerInventoryMaxRows = 20;
        private const int playerInventoryMinRows = 4;

        private const int woodChestInventoryMaxRows = 10;
        private const int woodChestInventoryMinRows = 2;
        private const int woodChestInventoryMaxCol = 8;
        private const int woodChestInventoryMinCol = 5;

        private const int personalChestInventoryMaxRows = 20;
        private const int personalChestInventoryMinRows = 2;
        private const int personalChestInventoryMaxCol = 8;
        private const int personalChestInventoryMinCol = 3;

        private const int ironChestInventoryMaxRows = 20;
        private const int ironChestInventoryMinRows = 3;
        private const int ironChestInventoryMaxCol = 8;
        private const int ironChestInventoryMinCol = 6;

        public static void Prefix(string name, ref int w, ref int h)
        {
            if (Configuration.Current.Inventory.IsEnabled)
            {
                // Player inventory
                if (h == 4 && w == 8)
                {
                    h = Math.Min(playerInventoryMaxRows, Math.Max(playerInventoryMinRows, Configuration.Current.Inventory.playerInventoryRows));
                }
                // Wood chest
                else if (h == 2 && w == 5)
                {
                    w = Math.Min(woodChestInventoryMaxCol, Math.Max(woodChestInventoryMinCol, Configuration.Current.Inventory.woodChestColumns));
                    h = Math.Min(woodChestInventoryMaxRows, Math.Max(woodChestInventoryMinRows, Configuration.Current.Inventory.woodChestRows));
                }
                // Personal chest
                else if (h == 2 && w == 3)
                {
                    w = Math.Min(personalChestInventoryMaxCol, Math.Max(personalChestInventoryMinCol, Configuration.Current.Inventory.personalChestColumns));
                    h = Math.Min(personalChestInventoryMaxRows, Math.Max(personalChestInventoryMinRows, Configuration.Current.Inventory.personalChestRows));
                }
                // Karve (small boat)
                else if (h == 2 && w == 2)
                {
                    w = Math.Min(woodChestInventoryMaxCol, Math.Max(woodChestInventoryMinCol, Configuration.Current.Inventory.woodChestColumns));
                    h = Math.Min(woodChestInventoryMaxRows, Math.Max(woodChestInventoryMinRows, Configuration.Current.Inventory.woodChestRows));
                }
                // Iron chest, cart, longboat
                else if (h == 3 && w == 6)
                {
                    w = Math.Min(ironChestInventoryMaxCol, Math.Max(ironChestInventoryMinCol, Configuration.Current.Inventory.ironChestColumns));
                    h = Math.Min(ironChestInventoryMaxRows, Math.Max(ironChestInventoryMinRows, Configuration.Current.Inventory.ironChestRows));
                }
            }
        }
    }
}
