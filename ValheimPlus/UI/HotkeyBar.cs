using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using ValheimPlus.Configurations;
using System.Linq;

namespace ValheimPlus.UI
{
    /// <summary>
    /// Shows current and total ammo counts below bow icon, if bow is equipped in hotbar
    /// </summary>
    [HarmonyPatch(typeof(HotkeyBar), nameof(HotkeyBar.UpdateIcons))]
    public static class HotkeyBar_UpdateIcons_Patch
    {
        private const string hudObjectName = "BowAmmoCounts";
        private const string noAmmoDisplay = "No Ammo";

        private static void Postfix(ref HotkeyBar __instance, ref Player player)
        {
            if (Configuration.Current.Hud.IsEnabled && Configuration.Current.Hud.displayBowAmmoCounts > 0)
                DisplayAmmoCountsUnderBowHotbarIcon(__instance, player);
        }

        private static void DisplayAmmoCountsUnderBowHotbarIcon(HotkeyBar __instance, Player player)
        {
            GameObject ammoCounter = GameObject.Find(hudObjectName);
            
            // Find the bow in the hotbar
            ItemDrop.ItemData bow = null;
            foreach (ItemDrop.ItemData item in __instance.m_items)
            {
                if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Bow)
                {
                    if (bow == null || player.IsItemEquiped(item))
                        bow = item;
                }
            }

            // If there is no bow or it is not equipped, remove the text element
            if (bow == null || (Configuration.Current.Hud.displayBowAmmoCounts == 1 && !player.IsItemEquiped(bow)))
            {
                if (ammoCounter != null)
                {
                    GameObject.Destroy(ammoCounter);
                    ammoCounter = null;
                }
                return;
            }
            
            // Make sure we have a valid index
            if (__instance.m_elements.Count >= bow.m_gridPos.x && bow.m_gridPos.x >= 0)
            {
                // Create a new text element to display the ammo counts
                HotkeyBar.ElementData element = __instance.m_elements[bow.m_gridPos.x];
                if (ammoCounter == null)
                {
                    ammoCounter = GameObject.Instantiate(element.m_amount.gameObject, element.m_amount.gameObject.transform.parent, false);
                    ammoCounter.name = hudObjectName;
                    ammoCounter.SetActive(true);
                    Vector3 offset = element.m_amount.gameObject.transform.position - element.m_icon.transform.position;
                    ammoCounter.transform.Translate(offset);
                    Text ammoText = ammoCounter.GetComponentInChildren<Text>();
                    ammoText.fontSize -= 2;

                }

                // Attach it to the hotbar icon
                ammoCounter.gameObject.transform.SetParent(element.m_amount.gameObject.transform.parent, false);

                // Find the active ammo being used for thebow
                ItemDrop.ItemData ammoItem = player.m_ammoItem;
                if (ammoItem == null)
                    ammoItem = player.GetInventory().GetAmmoItem(bow.m_shared.m_ammoType);

                // Calculate totals to display for current ammo type and all types
                int currentAmmo = 0;
                int totalAmmo = 0;
                var inventoryItems = player.GetInventory().GetAllItems();
                foreach (ItemDrop.ItemData inventoryItem in inventoryItems)
                {
                    if (inventoryItem.m_shared.m_ammoType == bow.m_shared.m_ammoType &&
                        (inventoryItem.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Ammo || inventoryItem.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Consumable))
                    {
                        totalAmmo += inventoryItem.m_stack;

                        if (inventoryItem.m_shared.m_name == ammoItem.m_shared.m_name)
                            currentAmmo += inventoryItem.m_stack;
                    }
                }

                // Change the visual display text for the UI
                Text ammoCounterText = ammoCounter.GetComponentInChildren<Text>();
                if (totalAmmo == 0)
                    ammoCounterText.text = noAmmoDisplay;
                else
                    ammoCounterText.text = ammoItem.m_shared.m_name.Split('_').Last() + " " + currentAmmo + "/" + totalAmmo;
            }
        }
    }
}
