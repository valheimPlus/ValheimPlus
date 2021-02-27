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

        static void Prefix(ref string name, ref Sprite bkg, ref int w, ref int h)
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

    [HarmonyPatch(typeof(InventoryGui), "Show")]
    public class changeInventorySize
    {
        private const float oneRowSize = 70.5f;
        private const float containerOriginalY = -90.0f;
        private const float containerHeight = -340.0f;

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