using UnityEngine;
using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    class VersionInfo
    {
        /// <summary>
        /// Get version string and enforce mod if enabled
        /// </summary>
        [HarmonyPatch(typeof(Version), "GetVersionString")]
        public static class VersionServerControl
        {
            private static bool Prefix(ref string __result)
            {
                string gameVersion = Version.CombineVersion(global::Version.m_major, global::Version.m_minor, global::Version.m_patch);
                __result = gameVersion;

                Debug.Log($"Version generator started.");
                if (Configuration.Current.Server.IsEnabled)
                {
                    if (Configuration.Current.Server.enforceMod)
                    {
                        __result = gameVersion + "@" + ValheimPlusPlugin.version;
                        Debug.Log($"Version generated with enforced mod : {__result}");
                        return false;
                    }
                }

                Debug.Log($"Version generated : {__result}");
                return false;
            }
        }
    }
}
