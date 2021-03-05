using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using ValheimPlus.Configurations;

// ToDo add packet system to convey map markers
namespace ValheimPlus
{
    [HarmonyPatch(typeof(ZNet))]
    public class HookZNet
    {
        /// <summary>
        /// Hook base GetOtherPublicPlayer method
        /// </summary>
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(ZNet), "GetOtherPublicPlayers", new Type[] { typeof(List<ZNet.PlayerInfo>) })]
        public static void GetOtherPublicPlayers(object instance, List<ZNet.PlayerInfo> playerList) => throw new NotImplementedException();
    }

    /// <summary>
    /// Alter server player limit
    /// </summary>
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
        }
    }

    /// <summary>
    /// Sync server client configuration
    /// </summary>
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

    /// <summary>
    /// Load settngs from server instance
    /// </summary>
    [HarmonyPatch(typeof(ZNet), "Shutdown")]
    public static class OnErrorLoadOwnIni
    {
        private static void Prefix(ref ZNet __instance)
        {
            if (!__instance.IsServer())
            {
                // Load the client config file on server ZNet instance exit (server disconnect)
                if (ConfigurationExtra.LoadSettings() != true)
                {
                    Debug.LogError("Error while loading configuration file.");
                }
            }
        }
    }

    /// <summary>
    /// Force player public reference position on
    /// </summary>
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
}
