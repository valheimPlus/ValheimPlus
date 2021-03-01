using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    /// <summary>
    /// Alters datarate
    /// </summary>
    [HarmonyPatch(typeof(ZDOMan), "SendZDOs")]
    public static class ChangeDataRate
    {
        private static void Prefix(ref ZDOMan __instance, ref int ___m_dataPerSec)
        {
            // If we dont limit this to a reasonable value you will get packet pipe problems
            if (Configuration.Current.Server.dataRate > 512) Configuration.Current.Server.dataRate = 512;

            if (Configuration.Current.Server.IsEnabled && Configuration.Current.Server.dataRate >= 60)
            {
                ___m_dataPerSec = Configuration.Current.Server.dataRate * 1024;
            }
        }
    }
}
