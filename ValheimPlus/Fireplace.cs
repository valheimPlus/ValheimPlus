using HarmonyLib;
using ValheimPlus.Configurations;
using UnityEngine;

namespace ValheimPlus 
{
    class FireplaceFuel 
    {
        [HarmonyPatch(typeof(Fireplace), "UpdateFireplace")]
        public static class TorchesNoFuel 
        {
			private static void Postfix(Fireplace __instance, ref ZNetView ___m_nview) 
			{
				if (Configuration.Current.Fireplace.IsEnabled)
				{
					if (Configuration.Current.Fireplace.onlyTorches)
					{
						if (__instance.GetHoverText().Contains("torch") || __instance.GetHoverText().Contains("Sconce") || __instance.name.Contains("brazier")) 
						{
							___m_nview.GetZDO().Set("fuel", 1f);
						}  
					} 
					else
					{
						___m_nview.GetZDO().Set("fuel", 3f);
					}
				}
			}
        }
    }
}
