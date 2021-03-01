using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using ValheimPlus.Configurations;
using ValheimPlus.UI;

namespace ValheimPlus
{
    /// <summary>
    /// Set max player limit and disable server password 
    /// </summary>
    [HarmonyPatch(typeof(FejdStartup), "Awake")]
    public static class HookServerStart
    {
        private static void Postfix(ref FejdStartup __instance)
        {
            if (Configuration.Current.Server.IsEnabled && Configuration.Current.Server.disableServerPassword)
            {
                __instance.m_minimumPasswordLength = 0;
            }
            if (Configuration.Current.Server.IsEnabled)
            {
                __instance.m_serverPlayerLimit = Configuration.Current.Server.maxPlayers;
            }
        }
    }

    /// <summary>
    /// Adding V+ logo and version text
    /// </summary>
    [HarmonyPatch(typeof(FejdStartup), "SetupGui")]
    public static class HookGui
    {
        private static void Postfix(ref FejdStartup __instance)
        {
            // logo
            GameObject logo = GameObject.Find("LOGO");
            logo.GetComponent<Image>().sprite = VPlusMainMenu.VPlusLogoSprite;

            // version text for bottom right of startup
            __instance.m_versionLabel.fontSize = 14;
            string gameVersion = Version.CombineVersion(global::Version.m_major, global::Version.m_minor, global::Version.m_patch);
            __instance.m_versionLabel.text = "version " + gameVersion + "\n" + "ValheimPlus " + ValheimPlusPlugin.version;
        }
    }

    /// <summary>
    /// Alters public password requirements
    /// </summary>
    [HarmonyPatch(typeof(FejdStartup), "IsPublicPasswordValid")]
    public static class ChangeServerPasswordBehavior
    {
        private static bool Prefix(ref bool __result)
        {
            if (Configuration.Current.Server.IsEnabled && Configuration.Current.Server.disableServerPassword)
            {
                // return always true
                __result = true;
                return false;
            }

            // continue with default function
            return true;
        }
    }

    /// <summary>
    /// Override password error
    /// </summary>
    [HarmonyPatch(typeof(FejdStartup), "GetPublicPasswordError")]
    public static class RemovePublicPasswordError
    {
        private static bool Prefix(ref string __result)
        {
            if (Configuration.Current.Server.IsEnabled && Configuration.Current.Server.disableServerPassword)
            {
                __result = "";
                return false;
            }

            return true;
        }
    }
}