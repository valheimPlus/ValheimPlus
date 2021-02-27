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

            if (Configuration.Current.Map.IsEnabled && Configuration.Current.MapServer.playerPositionPublicOnJoin)
            {
                // Set player position visibility to public by default on server join
                __instance.m_publicReferencePosition = true;
            }


        }
    }

    [HarmonyPatch(typeof(ZNet), "RPC_PeerInfo")]
    public static class ConfigServerSync
    {
        private static void Postfix(ref ZNet __instance)
        {
            if (!ZNet.m_isServer)
            {
                ZLog.Log("-------------------- SENDING VPLUGCONFIGSYNC REQUEST");
                ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.instance.GetServerPeerID(), "VPlusConfigSync", new object[] { new ZPackage() });
            }
        }
    }


    [HarmonyPatch(typeof(ZNet), "SetPublicReferencePosition")]
    public static class PreventPublicPositionToggle
    {
        private static void Postfix(ref bool pub, ref bool ___m_publicReferencePosition)
        {
            if (Configuration.Current.Map.IsEnabled && Configuration.Current.MapServer.preventPlayerFromTurningOffPublicPosition)
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
        private static Boolean Prefix(ref Boolean __result)
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

    [HarmonyPatch(typeof(FejdStartup), "GetPublicPasswordError")]
    public static class RemovePublicPasswordError
    {
        private static Boolean Prefix(ref string __result) 
        {
            if (Configuration.Current.Server.IsEnabled && Configuration.Current.Server.disableServerPassword)
            {
                __result = "";
                return false;
            }
            return true;
        }
    }


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

    [HarmonyPatch(typeof(Game), "UpdateSaving")]
    public static class ChangeClientAndServerSaveInterval
    {
        private static Boolean Prefix(ref Game __instance, ref float dt)
        {
            if (Configuration.Current.Server.IsEnabled && Configuration.Current.Server.autoSaveInterval >= 10 && ZNet.instance.IsServer())
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
