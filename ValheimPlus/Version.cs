using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using Unity;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Runtime;
using IniParser;
using IniParser.Model;
using HarmonyLib;
using System.Globalization;
using Steamworks;
using ValheimPlus;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    class VersionInfo
    {
        
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
                else {
                    __instance.AddString("ValheimPlus [" + ValheimPlusPlugin.version + "] is up to date.");
                }
                __instance.AddString("");
            }

        }

        [HarmonyPatch(typeof(FejdStartup), "SetupGui")]
        public static class HookGui
        {
            private static void Postfix(ref FejdStartup __instance)
            {
                __instance.m_versionLabel.fontSize = 14;
                string gameVersion = Version.CombineVersion(global::Version.m_major, global::Version.m_minor, global::Version.m_patch);
                __instance.m_versionLabel.text = "version " + gameVersion + "\n" +"ValheimPlus " + ValheimPlusPlugin.version;
            }

        }


        [HarmonyPatch(typeof(Version), "GetVersionString")]
        public static class VersionServerControl
        {
            private static Boolean Prefix(ref string __result)
            {
                string gameVersion = Version.CombineVersion(global::Version.m_major, global::Version.m_minor, global::Version.m_patch);
                __result = gameVersion;

                Debug.Log($"Version generator started.");
                if (Configuration.Current.Server.IsEnabled)
                {
                    if (Configuration.Current.Server.enforceConfiguration && Configuration.Current.Server.enforceMod)
                    {
                        __result = gameVersion + "@" + ValheimPlusPlugin.version + "@" + ConfigurationExtra.GetServerHashFor(Configuration.Current);
                        Debug.Log($"Version generated with enforced mod and config hash : {__result}");
                        return false;
                    }

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
