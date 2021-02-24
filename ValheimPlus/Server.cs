using System;
using HarmonyLib;
using Steamworks;
using ValheimPlus.Configurations;
using UnityEngine;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(ZNet), "Awake")]
    public static class ChangeGameServerVariables
    {
        private static void Postfix(ref ZNet __instance)
        {
            if (Configuration.Current.Server.IsEnabled)
            {
                int maxPlayers = Configuration.Current.Server.maxPlayers;
                if (maxPlayers >= 1)
                {
                    // Set Server Instance Max Players
                    __instance.m_serverPlayerLimit = maxPlayers;
                }
            }

            if (Configuration.Current.Map.IsEnabled && Configuration.Current.Map.playerPositionPublicOnJoin)
            {
                // Set player position visibility to public by default on server join
                __instance.m_publicReferencePosition = true;
            }
        }
    }

    [HarmonyPatch(typeof(ZNet), "SetPublicReferencePosition")]
    public static class PreventPublicPositionToggle
    {
        private static void Postfix(ref bool pub, ref bool ___m_publicReferencePosition)
        {
            if (Configuration.Current.Map.IsEnabled && Configuration.Current.Map.preventPlayerFromTurningOffPublicPosition)
            {
                ___m_publicReferencePosition = true;
            }
        }
    }

    [HarmonyPatch(typeof(SteamGameServer), "SetMaxPlayerCount")]
    public static class ChangeSteamServerVariables
    {
        private static void Prefix(ref int cPlayersMax)
        {
            if (Configuration.Current.Server.IsEnabled)
            {
                int maxPlayers = Configuration.Current.Server.maxPlayers;
                if (maxPlayers >= 1)
                {
                    cPlayersMax = maxPlayers;
                }
            }
        }
    }

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

    [HarmonyPatch(typeof(FejdStartup), "IsPublicPasswordValid")]
    public static class ChangeServerPasswordBehavior
    {
        private static void Postfix(ref Boolean __result) // Set after awake function
        {
            if (Configuration.Current.Server.IsEnabled)
            {
                if (Configuration.Current.Server.disableServerPassword)
                {
                    __result = true;
                }
            }
        }
    }

    [HarmonyPatch(typeof(Game), "UpdateSaving")]
    public static class ChangeClientAndServerSaveInterval
    {
        private static Boolean Prefix(ref Game __instance, ref float dt)
        {
            if (Configuration.Current.Server.IsEnabled && Configuration.Current.Server.autoSaveInterval >= 10)
            {
                __instance.m_saveTimer += dt;
                if (__instance.m_saveTimer > Configuration.Current.Server.autoSaveInterval)
                {
                    __instance.m_saveTimer = 0f;
                    __instance.SavePlayerProfile(false);
                    if (ZNet.instance)
                    {
                        ZNet.instance.Save(false);
                    }
                    Debug.Log("Saving world data.");
                }
                return false;
            }
            return true;
        }
    }

}
