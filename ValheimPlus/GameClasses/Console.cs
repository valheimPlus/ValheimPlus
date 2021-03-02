using UnityEngine;
using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    class ConsoleInfo
    {
        /// <summary>
        /// Adding version data to console
        /// </summary>
        [HarmonyPatch(typeof(Console), "Awake")]
        public static class HookConsole
        {
            private static void Postfix(ref Console __instance)
            {
                __instance.AddString("ValheimPlus [" + ValheimPlusPlugin.version + "] is loaded.");
                if (!ValheimPlusPlugin.isUpToDate && ValheimPlusPlugin.newestVersion != "Unknown")
                {
                    __instance.AddString("ValheimPlus [" + ValheimPlusPlugin.version + "] is outdated, version [" + ValheimPlusPlugin.newestVersion + "] is available.");
                    __instance.AddString("Please visit " + ValheimPlusPlugin.Repository + ".");
                }
                else
                {
                    __instance.AddString("ValheimPlus [" + ValheimPlusPlugin.version + "] is up to date.");
                }

                __instance.AddString("");
            }
        }

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
                        Debug.Log($"Version generated with enforced mod : {__result}");
                        __result = gameVersion + "@" + ValheimPlusPlugin.version;
                        return false;
                    }
                }

                Debug.Log($"Version generated : {__result}");
                return false;
            }
        }
    }
}
