using UnityEngine;
using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus.GameClasses
{
    class VersionInfo
    {
        /// <summary>
        /// Get version string and enforce mod if enabled
        /// </summary>
        [HarmonyPatch(typeof(Version), "GetVersionString")]
        public static class Version_GetVersionString_Patch
        {
            private static void Postfix(ref string __result)
            {
                if (Configuration.Current.Server.IsEnabled)
                {
                    if (Configuration.Current.Server.enforceMod)
                    {
                        __result = __result + "@" + ValheimPlusPlugin.version;
                    }
                }
            }
        }
    }
}
