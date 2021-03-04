using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.UI;
using ValheimPlus.Configurations;

namespace ValheimPlus 
{
	/// <summary>
	/// Inventory HUD setup
	/// </summary>
	[HarmonyPatch(typeof(InventoryGui), "SetupRequirement")]
	public static class AddCurrentItemsAvailable 
	{
		private static bool Prefix(Transform elementRoot, Piece.Requirement req, Player player, bool craft, int quality, ref bool __result)
		{
			Image component = elementRoot.transform.Find("res_icon").GetComponent < Image > ();
			Text component2 = elementRoot.transform.Find("res_name").GetComponent < Text > ();
			Text component3 = elementRoot.transform.Find("res_amount").GetComponent < Text > ();
			UITooltip component4 = elementRoot.GetComponent < UITooltip > ();

			if (req.m_resItem != null) {
				component.gameObject.SetActive(true);
				component2.gameObject.SetActive(true);
				component3.gameObject.SetActive(true);
				component.sprite = req.m_resItem.m_itemData.GetIcon();
				component.color = Color.white;
				component4.m_text = Localization.instance.Localize(req.m_resItem.m_itemData.m_shared.m_name);
				component2.text = Localization.instance.Localize(req.m_resItem.m_itemData.m_shared.m_name);
				int num = player.GetInventory().CountItems(req.m_resItem.m_itemData.m_shared.m_name);
				int amount = req.GetAmount(quality);

				if (amount <= 0) {
					InventoryGui.HideRequirement(elementRoot);
					__result = false;
					return false;
				}
				component3.text = num + "/" + amount.ToString();

				if (num < amount) {
					component3.color = ((Mathf.Sin(Time.time * 10f) > 0f) ? Color.red: Color.white);
				}
				else {
					component3.color = Color.white;
				}

				component3.fontSize = 14;
				if (component3.text.Length > 5) {
					component3.fontSize -= component3.text.Length - 5;
				}
			}

			__result = true;
			return false;
		}
	}

	[HarmonyPatch(typeof(InventoryGui), "Show")]
	public class InventoryGUI_Show_Patch 
	{
		private const float oneRowSize = 70.5f;
		private const float containerOriginalY = -90.0f;
		private const float containerHeight = -340.0f;
		private static float lastValue = 0;

		public static void Postfix(ref InventoryGui __instance) 
		{
			if (Configuration.Current.Inventory.IsEnabled) {
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
				if (!playerGrid.GetComponent < InventoryGrid > ().m_scrollbar) 
				{
					GameObject playerGridScroll = GameObject.Instantiate(InventoryGui.instance.m_containerGrid.m_scrollbar.gameObject, playerGrid.transform.parent);
					playerGridScroll.name = "PlayerScroll";
					playerGrid.GetComponent < RectMask2D > ().enabled = true;
					ScrollRect playerScrollRect = playerGrid.AddComponent < ScrollRect > ();
					playerGrid.GetComponent < RectTransform > ().offsetMax = new Vector2(800f, playerGrid.GetComponent < RectTransform > ().offsetMax.y);
					playerGrid.GetComponent < RectTransform > ().anchoredPosition = new Vector2(0f, 1f);
					playerScrollRect.content = playerGrid.GetComponent < InventoryGrid > ().m_gridRoot;
					playerScrollRect.viewport = __instance.m_player.GetComponentInChildren < RectTransform > ();
					playerScrollRect.verticalScrollbar = playerGridScroll.GetComponent < Scrollbar > ();
					playerGrid.GetComponent < InventoryGrid > ().m_scrollbar = playerGridScroll.GetComponent < Scrollbar > ();

					playerScrollRect.horizontal = false;
					playerScrollRect.movementType = ScrollRect.MovementType.Clamped;
					playerScrollRect.scrollSensitivity = oneRowSize;
					playerScrollRect.inertia = false;
					playerScrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
					Scrollbar playerScrollbar = playerGridScroll.GetComponent < Scrollbar > ();
					lastValue = playerScrollbar.value;
				}
			}
		}
	}
}
