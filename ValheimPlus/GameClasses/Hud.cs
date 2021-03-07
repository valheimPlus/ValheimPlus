using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using ValheimPlus.Configurations;

namespace ValheimPlus {
	/// <summary>
	/// Shows Stamina Numerical Value
	/// </summary>
	[HarmonyPatch(typeof(Hud), "UpdateStamina")]
	public static class AddStaminaDisplay
	{
		private static GameObject stamina;
		private static void Postfix(Hud __instance, Player player)
		{
			if (Configuration.Current.Hud.IsEnabled && Configuration.Current.Hud.displayStaminaValue)
			{
				Text staminaText;
				if (stamina == null)
				{
					var healthText = __instance.m_healthText;
					stamina = new GameObject();
					stamina.transform.SetParent(__instance.m_staminaBar.transform.parent);
					stamina.AddComponent<RectTransform>();

					staminaText = stamina.AddComponent<Text>();
					staminaText.color = healthText.color;
					staminaText.font = healthText.font;
					staminaText.fontSize = healthText.fontSize;
					staminaText.enabled = true;
					staminaText.alignment = TextAnchor.MiddleCenter;
				}
				else staminaText = stamina.GetComponent<Text>();
				staminaText.text = Mathf.CeilToInt(player.m_stamina).ToString() + "/" + Mathf.CeilToInt(player.m_maxStamina).ToString();
				var staminaBarRec = __instance.m_staminaBar2Root.transform as RectTransform;
				stamina.GetComponent<RectTransform>().position = new Vector2(staminaBarRec.position.x, staminaBarRec.position.y);
				stamina.SetActive(__instance.m_staminaAnimator.GetBool("Visible"));
			}
		}
	}

	/// <summary>
	/// Removes damage flash
	/// </summary>
	[HarmonyPatch(typeof(Hud), "DamageFlash")]
	public static class Hud_DamageFlash_Patch
	{
		private static void Postfix(Hud __instance)
		{
			if (Configuration.Current.Hud.IsEnabled && Configuration.Current.Hud.removeDamageFlash)
				__instance.m_damageScreen.gameObject.SetActive(false);
		}
	}

}
