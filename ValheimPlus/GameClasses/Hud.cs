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
