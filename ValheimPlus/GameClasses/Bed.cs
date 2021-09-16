using HarmonyLib;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus.GameClasses
{
	/// <summary>
	/// Allow to Sleep without setting spawn point
	/// </summary>
	[HarmonyPatch(typeof(Bed), "GetHoverText")]

	public static class Bed_GetHoverText_Patch
	{
		private static void Postfix(Bed __instance, ref string __result, ZNetView ___m_nview)
		{
			if (Configuration.Current.Bed.IsEnabled && Configuration.Current.Bed.sleepWithoutSpawn && Configuration.Current.Bed.unclaimedBedsOnly)
			{
				bool ownerName = (___m_nview.GetZDO().GetLong("owner", 0L) != 0L) || Traverse.Create(__instance).Method("IsCurrent", new object[0]).GetValue<bool>();
				if (!ownerName)
				{
					__result += Localization.instance.Localize("\n[" + "LShift" + "+<color=yellow><b>$KEY_Use</b></color>] $piece_bed_sleep");
				}
			}
			else if (Configuration.Current.Bed.IsEnabled && Configuration.Current.Bed.sleepWithoutSpawn && !Configuration.Current.Bed.unclaimedBedsOnly)
			{
				bool current = Traverse.Create(__instance).Method("IsCurrent", new object[0]).GetValue<bool>();
				if (current)
				{
					return;
				}
				__result += Localization.instance.Localize("\n[LShift+<color=yellow><b>$KEY_Use</b></color>] $piece_bed_sleep");
			}
		}
	}

	[HarmonyPatch(typeof(Bed), "Interact")]
	public static class Bed_Interact_Patch
	{
		private static void Postfix(Bed __instance, Humanoid human, bool repeat, ZNetView ___m_nview)
		{
			if (Configuration.Current.Bed.IsEnabled && Configuration.Current.Bed.sleepWithoutSpawn)
			{
				if (Input.GetKey(KeyCode.LeftShift))
				{
					if (Input.GetKeyDown(KeyCode.E))
					{
						bool flag = Traverse.Create(__instance).Method("IsCurrent", new object[0]).GetValue<bool>() || repeat || Configuration.Current.Bed.unclaimedBedsOnly && ___m_nview.GetZDO().GetLong("owner", 0L) != 0L;
						if (!flag)
						{
							Player humanPlayer = human as Player;
							if (!EnvMan.instance.IsAfternoon() && !EnvMan.instance.IsNight())
							{
								human.Message(MessageHud.MessageType.Center, "$msg_cantsleep", 0, null);
								return;
							}
							if (!__instance.CheckEnemies(humanPlayer))
							{
								return;
							}
							if (!__instance.CheckExposure(humanPlayer))
							{
								return;
							}
							if (!__instance.CheckFire(humanPlayer))
							{
								return;
							}
							if (!__instance.CheckWet(humanPlayer))
							{
								return;
							}
							human.AttachStart(__instance.m_spawnPoint, __instance.gameObject, true, true, false, "attach_bed", new Vector3(0f, 0.5f, 0f));
							return;
						}
					}
				}
			}
		}
	}
	[HarmonyPatch(typeof(Bed), "Interact")]
	public static class Bed_InteractIgnoreStandaloneE_Patch
	{
		private static bool Prefix(Bed __instance, Humanoid human)
		{
			if (Input.GetKey(KeyCode.E))
			{
				if (Input.GetKey(KeyCode.LeftShift))
				{
					return false;
				}
			}
			return true;
		}
	}
}