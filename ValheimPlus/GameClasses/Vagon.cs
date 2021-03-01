using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    class WagonModifications
	{
        [HarmonyPatch(typeof(Vagon), "UpdateMass")]
        public static class ModifyWagonMass
		{
			// "Vagon" is from base game
            private static bool Prefix(ref Vagon __instance)
			{
				if (!__instance.m_nview.IsOwner())
				{
					return false;
				}
				if (__instance.m_container == null)
				{
					return false;
				}

				float totalWeight = 0;

				if (Configuration.Current.Wagon.IsEnabled)
					totalWeight = Helper.applyModifierValue(__instance.m_container.GetInventory().GetTotalWeight(), Configuration.Current.Wagon.wagonExtraMassFromItems);
				else
					totalWeight = __instance.m_container.GetInventory().GetTotalWeight();
				
				if (Configuration.Current.Wagon.IsEnabled)
					__instance.m_baseMass = Configuration.Current.Wagon.wagonBaseMass;
				else
					__instance.m_baseMass = 20;

				float mass = __instance.m_baseMass + totalWeight * __instance.m_itemWeightMassFactor;

				__instance.SetMass(mass);

				return false;
			}
        }
    }
}
