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

		private const int ironChestInventoryMaxRows = 20;
		private const int ironChestInventoryMinRows = 4;
		private const int ironChestInventoryMaxCol = 20;
		private const int ironChestInventoryMinCol = 4;

		public static void Prefix(string name, ref int w, ref int h)
		{
			if (Configuration.Current.Inventory.IsEnabled)
			{

				// Wood chest
				if (h == 2 && w == 5)
				{
					w = Math.Min(woodChestInventoryMaxCol, Math.Max(woodChestInventoryMinCol, Configuration.Current.Inventory.woodChestColumns));
					h = Math.Min(woodChestInventoryMaxRows, Math.Max(woodChestInventoryMinRows, Configuration.Current.Inventory.woodChestRows));
				}
				// Player inventory
				else if (h == 4 && w == 8)
				{
					h = Math.Min(playerInventoryMaxRows, Math.Max(playerInventoryMinRows, Configuration.Current.Inventory.playerInventoryRows));

				}
				// Iron chest, cart, boat
				else if (h == 3 && w == 6)
				{
					w = Math.Min(ironChestInventoryMaxCol, Math.Max(ironChestInventoryMinCol, Configuration.Current.Inventory.ironChestColumns));
					h = Math.Min(ironChestInventoryMaxRows, Math.Max(ironChestInventoryMinRows, Configuration.Current.Inventory.ironChestRows));
				}
			}
		}
	}
}
