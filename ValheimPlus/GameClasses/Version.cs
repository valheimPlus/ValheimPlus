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
                Debug.Log($"Version generator started.");
                if (Configuration.Current.Server.IsEnabled)
                {
                    if (Configuration.Current.Server.enforceMod)
                    {
                        __result = __result + "@" + ValheimPlusPlugin.version;
                        Debug.Log($"Version generated with enforced mod : {__result}");
                    }
                }
                else
                {
                    Debug.Log($"Version generated : {__result}");
                }
            }
        }
    }
}
