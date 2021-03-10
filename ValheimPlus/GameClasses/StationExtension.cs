using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(StationExtension), nameof(StationExtension.Awake))]
    public static class StationExtension_Awake_Patch
    {
        /// <summary>
        /// Tweaks the station attachment distance.
        /// </summary>
        [HarmonyPrefix]
        public static void Prefix(ref float ___m_maxStationDistance)
        {
            if (Configuration.Current.Workbench.IsEnabled)
            {
                ___m_maxStationDistance = Configuration.Current.Workbench.workbenchAttachmentRange;
            }
        }
    }
}
