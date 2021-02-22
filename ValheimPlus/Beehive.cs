using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(Beehive), "Awake")]
    public static class ApplyBeehiveChanges
    {
        private static bool Prefix(ref float ___m_secPerUnit, ref int ___m_maxHoney)
        {
            if (Configuration.Current.Beehive.IsEnabled)
            {
                ___m_secPerUnit = Configuration.Current.Beehive.honeyProductionSpeed;
                ___m_maxHoney = Configuration.Current.Beehive.maximumHoneyPerBeehive;
            }
            return true;
        }

    }
}
