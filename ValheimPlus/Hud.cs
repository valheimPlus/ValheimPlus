using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using ValheimPlus.Configurations;

namespace ValheimPlus {
    class HudModification {
        
		[HarmonyPatch(typeof(InventoryGui), "SetupRequirement")]
        public static class AddCurrentItemsAvailable {
            private static Boolean Prefix(Transform elementRoot, Piece.Requirement req, Player player, bool craft, int quality, ref Boolean __result)
            {
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
					component3.text = num + " / " + amount.ToString() ;
					if (num < amount)
					{
						component3.color = ((Mathf.Sin(Time.time * 10f) > 0f) ? Color.red : Color.white);
					}
					else
					{
						component3.color = Color.white;
					}
				}
				__result = true;
				return false;
			}
        }

		[HarmonyPatch(typeof(Skills), "RaiseSkill")]
		public static class AddExpGainedDisplay
		{
			private static void Postfix(Skills __instance, Skills.SkillType skillType, float factor = 1f)
			{
				if (Configuration.Current.Hud.IsEnabled && Configuration.Current.Hud.experienceGainedNotifications)
				{
					Skills.Skill skill = __instance.GetSkill(skillType);
					float percent = skill.m_accumulator / (skill.GetNextLevelRequirement() / 100);
					__instance.m_player.Message(MessageHud.MessageType.TopLeft, skill.m_info.m_skill + " [" + Helper.tFloat(skill.m_accumulator, 2) + "/" + Helper.tFloat(skill.GetNextLevelRequirement(), 2) + "] (" + Helper.tFloat(percent, 0) + "%)", 0, skill.m_info.m_icon);
				}
			}

		}

		[HarmonyPatch(typeof(Hud), "UpdateStamina")]
		public static class AddStaminaDisplay
		{
			private static GameObject stamina;
			private static void Postfix(Hud __instance, Player player)
			{
				if (Configuration.Current.Hud.IsEnabled && Configuration.Current.Hud.staminaText)
				{
					Text staminaText;
					if (stamina == null)
					{
						var healthText = __instance.m_healthText;
						stamina = new GameObject();
						stamina.transform.SetParent(__instance.m_staminaBar.transform.parent);
						var rec = stamina.AddComponent<RectTransform>();
						var oldRec = __instance.m_staminaBar.GetComponent<RectTransform>();
						rec.position = new Vector3(Screen.width / 2, __instance.m_staminaBar2Root.transform.position.y, 0);

						staminaText = stamina.AddComponent<Text>();
						staminaText.color = healthText.color;
						staminaText.font = healthText.font;
						staminaText.fontSize = healthText.fontSize;
						staminaText.alignment = healthText.alignment;
						staminaText.enabled = true;
						staminaText.alignment = TextAnchor.MiddleCenter;
					}
					else staminaText = stamina.GetComponent<Text>();
					staminaText.text = Mathf.CeilToInt(player.m_stamina).ToString() + "/" + Mathf.CeilToInt(player.m_maxStamina).ToString();
					stamina.SetActive(__instance.m_staminaAnimator.GetBool("Visible"));
				}
			}
		}
	}
}
