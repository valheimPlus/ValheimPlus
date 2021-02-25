using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.UI;
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

    [HarmonyPatch(typeof(Inventory), "TopFirst", new Type[] { typeof(ItemDrop.ItemData) })]
    public static class changeInventoryFillOrder
    {
        public static bool Prefix(ref Boolean __result)
        {
            if (Configuration.Current.Inventory.IsEnabled &&
                Configuration.Current.Inventory.inventoryFillTopToBottom)
            {
                __result = true;
                return false;
            }
            else
            {
                return true;
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
                // Player inventory number of rows, clamp value in range 4-20
                int playerInventoryRows = Math.Min(20, Math.Max(4, Configuration.Current.Inventory.playerInventoryRows));

                // Wood chest number of columns, clamp value in range 5-8
                int woodChestColumns = Math.Min(8, Math.Max(5, Configuration.Current.Inventory.woodChestColumns));

                // Wood chest number of rows, clamp value in range 2-10
                int woodChestRows = Math.Min(10, Math.Max(2, Configuration.Current.Inventory.woodChestRows));

                // Iron chest number of columns, clamp value in range 6-8
                int ironChestColumns = Math.Min(8, Math.Max(6, Configuration.Current.Inventory.ironChestColumns));

                // Iron chest number of rows, clamp value in range 3-20
                int ironChestRows = Math.Min(20, Math.Max(3, Configuration.Current.Inventory.ironChestRows));

                // Wood chest
                if (h == 2 && w == 5)
                {
                    w = woodChestColumns;
                    h = woodChestRows;
                }
                // Player inventory
                else if (h == 4 && w == 8)
                {
                    h = playerInventoryRows;
                }
                // Iron chest, cart, boat
                else if (h == 3 && w == 6)
                {
                    w = ironChestColumns;
                    h = ironChestRows;
                }
            }
        }
    }

    [HarmonyPatch(typeof(InventoryGui), "Show")]
    public class changeInventorySize
    {
        public static void Postfix(ref InventoryGui __instance)
        {
            if (Configuration.Current.Inventory.IsEnabled)
            {
                RectTransform container = __instance.m_container;
                RectTransform player = __instance.m_player;

                // Player inventory background size, only enlarge it up to 6x8 rows, after that use the scroll bar
                int playerInventoryBackgroundSize = Math.Min(6, Math.Max(4, Configuration.Current.Inventory.playerInventoryRows));

                float oneRowSize = 70.5f;
                float containerOriginalY = -90.0f;
                float containerHeight = -340.0f;

                float containerNewY = containerOriginalY - oneRowSize * playerInventoryBackgroundSize;
                // Resize player inventory
                player.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, playerInventoryBackgroundSize * oneRowSize);
                // Move chest inventory based on new player invetory size
                container.offsetMax = new Vector2(610, containerNewY);
                container.offsetMin = new Vector2(40, containerNewY + containerHeight);

                GameObject playerGrid = InventoryGui.instance.m_playerGrid.gameObject;
                // Add player inventory scroll bar if it does not exist
                if (!playerGrid.GetComponent<InventoryGrid>().m_scrollbar)
                {
                    GameObject playerGridScroll = GameObject.Instantiate(InventoryGui.instance.m_containerGrid.m_scrollbar.gameObject, playerGrid.transform.parent);
                    playerGridScroll.name = "PlayerScroll";

                    playerGrid.GetComponent<RectMask2D>().enabled = true;
                    ScrollRect playerScrollRect = playerGrid.AddComponent<ScrollRect>();
                    playerScrollRect.content = playerGrid.GetComponent<InventoryGrid>().m_gridRoot;
                    playerScrollRect.viewport = __instance.m_player.GetComponentInChildren<RectTransform>();
                    playerScrollRect.verticalScrollbar = playerGridScroll.GetComponent<Scrollbar>();
                    playerGrid.GetComponent<InventoryGrid>().m_scrollbar = playerGridScroll.GetComponent<Scrollbar>();

                    playerScrollRect.horizontal = false;
                    playerScrollRect.movementType = ScrollRect.MovementType.Clamped;
                    playerScrollRect.scrollSensitivity = 40;
                    playerScrollRect.inertia = false;
                    playerScrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;

                }
            }
        }
    }
}