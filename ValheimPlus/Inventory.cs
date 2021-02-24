using HarmonyLib;
using System;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(Inventory), "IsTeleportable")]
    public static class noItemTeleportPrevention
    {
        private static void Postfix(ref Boolean __result)
        {
            if (Configuration.Current.Items.IsEnabled)
            {
                if (Configuration.Current.Items.noTeleportPrevention)
                    __result = true;
            }
        }
    }

    [HarmonyPatch(typeof(Inventory), MethodType.Constructor)]
    [HarmonyPatch(new Type[] { typeof(string), typeof(Sprite), typeof(int), typeof(int) })]
    public static class changeInventorySlotCount
    {
        static void Prefix(ref string name, ref Sprite bkg, ref int w, ref int h)
        {
            if (Configuration.Current.Inventory.IsEnabled)
            {
                // Wood chest
                if (h == 2 && w == 5)
                {
                    w = Configuration.Current.Inventory.woodChestColumns > 8 ? 8 : Configuration.Current.Inventory.woodChestColumns;
                    h = Configuration.Current.Inventory.woodChestRows > 10 ? 10 : Configuration.Current.Inventory.woodChestRows;
                }
                // Player inventory
                else if (h == 4 && w == 8)
                {
                    h = Configuration.Current.Inventory.playerInventoryRows > 7 ? 7 : Configuration.Current.Inventory.playerInventoryRows;
                }
                // Iron chest, cart, boat
                else if (h == 3 && w == 6)
                {
                    w = Configuration.Current.Inventory.ironChestColumns > 8 ? 8 : Configuration.Current.Inventory.ironChestColumns;
                    h = Configuration.Current.Inventory.ironChestRows > 10 ? 10 : Configuration.Current.Inventory.ironChestRows;
                }
            }
        }
    }

    [HarmonyPatch(typeof(InventoryGui), "Show")]
    public class InventoryGuiAwake
    {
        public static void Postfix(ref InventoryGui __instance)
        {
            if (Configuration.Current.Inventory.IsEnabled)
            {
                RectTransform container = __instance.m_container;
                RectTransform player = __instance.m_player;

                int playerInventoryRows = Configuration.Current.Inventory.playerInventoryRows > 7 ? 7 : Configuration.Current.Inventory.playerInventoryRows;

                float oneRowSize = 70.5f;
                float containerOriginalY = -90.0f;
                float containerHeight = -340.0f;

                float containerNewY = containerOriginalY - oneRowSize * playerInventoryRows;

                player.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, playerInventoryRows * oneRowSize);

                container.offsetMax = new Vector2(610, containerNewY);
                container.offsetMin = new Vector2(40, containerNewY + containerHeight);
            }
        }
    }
}