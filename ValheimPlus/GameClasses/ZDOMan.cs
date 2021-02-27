using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus.GameClasses
{
    /// <summary>
    /// Alters datarate
    /// </summary>
    [HarmonyPatch(typeof(ZDOMan), "SendZDOs")]
    public static class ChangeDataRate
    {
        private static void Prefix(ref ZDOMan __instance, ref int ___m_dataPerSec)
        {
            if (Configuration.Current.Server.IsEnabled && Configuration.Current.Server.dataRate >= 60)
            {
                ___m_dataPerSec = Configuration.Current.Server.dataRate * 1024;
            }
        }
    }
}
