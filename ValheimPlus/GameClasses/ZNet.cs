using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using ValheimPlus.Configurations;
using ValheimPlus.RPC;
using ValheimPlus.Utility;

// ToDo add packet system to convey map markers
namespace ValheimPlus.GameClasses
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
    /// Send queued RPCs
    /// </summary>
    [HarmonyPatch(typeof(ZNet), "SendPeriodicData")]
    public static class PeriodicDataHandler
    {
        private static void Postfix()
        {
            RpcQueue.SendNextRpc();
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
                ValheimPlusPlugin.harmony.UnpatchSelf();

                // Load the client config file on server ZNet instance exit (server disconnect)
                if (ConfigurationExtra.LoadSettings() != true)
                {
                    Debug.LogError("Error while loading configuration file.");
                }

                ValheimPlusPlugin.harmony.PatchAll();

                //We left the server, so reset our map sync check.
                if (Configuration.Current.Map.IsEnabled && Configuration.Current.Map.shareMapProgression)
                    VPlusMapSync.ShouldSyncOnSpawn = true;
            }
            else
            {
                //Save map data to disk
                if (Configuration.Current.Map.IsEnabled && Configuration.Current.Map.shareMapProgression)
                    VPlusMapSync.SaveMapDataToDisk();
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

    [HarmonyPatch(typeof(ZNet), "RPC_RefPos")]
    public static class PlayerPositionWatcher
    {
        private static void Postfix(ref ZNet __instance, ZRpc rpc, Vector3 pos, bool publicRefPos)
        {
            if (!__instance.IsServer()) return;

            if (Configuration.Current.Map.IsEnabled && Configuration.Current.Map.shareMapProgression)
            {
                Minimap.instance.WorldToPixel(pos, out int pixelX, out int pixelY);

                int radiusPixels =
                    (int) Mathf.Ceil(Configuration.Current.Map.exploreRadius / Minimap.instance.m_pixelSize);

                for (int y = pixelY - radiusPixels; y <= pixelY + radiusPixels; ++y)
                {
                    for (int x = pixelX - radiusPixels; x <= pixelX + radiusPixels; ++x)
                    {
                        if (x >= 0 && y >= 0 &&
                            (x < Minimap.instance.m_textureSize && y < Minimap.instance.m_textureSize) &&
                            ((double) new Vector2((float) (x - pixelX), (float) (y - pixelY)).magnitude <=
                             (double) radiusPixels))
                        {
                            VPlusMapSync.ServerMapData[y * Minimap.instance.m_textureSize + x] = true;
                        }
                    }
                }
            }
        }
    }
}
