using HarmonyLib;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using ValheimPlus.Configurations;
using ValheimPlus.RPC;
using ValheimPlus.Utility;


namespace ValheimPlus.GameClasses
{



    /// <summary>
    /// Extensions for the ZSteamSocket class that support better adaptive transfer mechanisms
    /// </summary>
    public static class ZSteamSocketExtensions
    {
        public static long totalCompressedSize = 0L;
        public static long totalUncompressedSize = 0L;

        private static int estimatedCapacityTotal = 0;
        private static int estimatedCapacityCount = 0;
 
        public static int GetEstimatedSendCapacity(this ZSteamSocket zsteamsocket)
        {
            if (!zsteamsocket.IsConnected())
            {
                return 0;
            }

            int mySendRate = 0;
            int myEstimatedThroughput = 0;
            int totalSendRate = 0;
            //Compute total send rate
            for(int i = 0; i < ZSteamSocket.m_sockets.Count; i++)
            {
                SteamNetworkingQuickConnectionStatus steamNetworkingQuickConnectionStatus;
                if (SteamNetworkingSockets.GetQuickConnectionStatus(ZSteamSocket.m_sockets[i].m_con, out steamNetworkingQuickConnectionStatus))
                {
                    if (ZSteamSocket.m_sockets[i] == zsteamsocket)
                    {
                        mySendRate = (int)steamNetworkingQuickConnectionStatus.m_flOutBytesPerSec;
                        myEstimatedThroughput = steamNetworkingQuickConnectionStatus.m_nSendRateBytesPerSecond;
                    }
                    totalSendRate += (int)steamNetworkingQuickConnectionStatus.m_flOutBytesPerSec;
                    if (!ZNet.m_isServer)
                    {
                        if(estimatedCapacityCount > ZDOMan.m_sendFPS*2)
                        {
                            VPlusNetworkStatusManager.SendNetworkStatus(ZRoutedRpc.instance.GetServerPeerID(), new VPlusNetworkStatus(Configuration.Local.NetworkConfiguration.enableCompression, estimatedCapacityTotal / estimatedCapacityCount));
                            estimatedCapacityCount = 1;
                            estimatedCapacityTotal = steamNetworkingQuickConnectionStatus.m_nSendRateBytesPerSecond;
                        }
                        else
                        {
                            estimatedCapacityCount += 1;
                            estimatedCapacityTotal += steamNetworkingQuickConnectionStatus.m_nSendRateBytesPerSecond;
                        }
                    }

                }

            }
            int myRemainingSendCapacity = myEstimatedThroughput - mySendRate;
            int globalRemainingSendCapacity = Configuration.Local.NetworkConfiguration.maxSendRateOverall_kbps * 1024 - totalSendRate;

            int remainingCapacity = Math.Min(myRemainingSendCapacity, globalRemainingSendCapacity);

            return (int)(remainingCapacity * ZSteamSocketExtensions.EstimateOverallCompressionRatio());
        }
        public static long GetEstimatedMicroSecondDelayTillNextTransfer(this ZSteamSocket zsteamsocket)
        {
            if (!zsteamsocket.IsConnected())
            {
                return -1;
            }

            SteamNetworkingQuickConnectionStatus steamNetworkingQuickConnectionStatus;
            if (SteamNetworkingSockets.GetQuickConnectionStatus(zsteamsocket.m_con, out steamNetworkingQuickConnectionStatus))
            {
                return steamNetworkingQuickConnectionStatus.m_usecQueueTime.m_SteamNetworkingMicroseconds;
            }
            return -1;
        }

        public static void UpdateOverallCompressionRatio(int uncompressedSize, int compressedSize)
        {
            totalCompressedSize += compressedSize;
            totalUncompressedSize += uncompressedSize;
        }

        public static float EstimateOverallCompressionRatio()
        {
            if (! Configuration.Local.NetworkConfiguration.enableCompression)
            {
                //If compression is turned off, ratio is 1:1
                return 1f;
            }
            if(totalUncompressedSize < 1024*128 || totalCompressedSize < 1024 * 128)
            {
                //Assume 1:1 ratio until at least 128KB has been transfered compressed
                return 1f;
            }
            return (float)totalUncompressedSize / (float)totalCompressedSize;
        }

        
        public static bool IsCompressed(byte[] data)
        {
            if (data.Length < sizeof(int))
            {
                return false;
            }
            return BitConverter.ToInt32(data, 0) == int.MinValue;
        }
        public static byte[] MarkAsCompressed(byte[] input)
        {
            byte[] flag = BitConverter.GetBytes(int.MinValue);
            byte[] output = new byte[flag.Length + input.Length];
            flag.CopyTo(output, 0);
            input.CopyTo(output, flag.Length);
            return output;
        }
    }



    /// <summary>
    /// After RegisterGlobalCallbacks runs, set the maximum send rate per connection to v+ configured value
    /// </summary>
    [HarmonyPatch(typeof(ZSteamSocket), "RegisterGlobalCallbacks")]
    public class ZSteamSocketRegisterGlobalCallbacks
    {
        private static void Postfix()
        {
            if (ZSteamSocket.m_statusChanged == null)
            {
                int maxSendRate = Configuration.Local.NetworkConfiguration.maxSendRatePerConnection_kbps * 1200;
                GCHandle maxSendRateHandle = GCHandle.Alloc(maxSendRate, GCHandleType.Pinned);
                SteamNetworkingUtils.SetConfigValue(ESteamNetworkingConfigValue.k_ESteamNetworkingConfig_SendRateMax, ESteamNetworkingConfigScope.k_ESteamNetworkingConfig_Global, IntPtr.Zero, ESteamNetworkingConfigDataType.k_ESteamNetworkingConfig_Int32, maxSendRateHandle.AddrOfPinnedObject());
                maxSendRateHandle.Free();
            }
        }
    }


    /// <summary>
    /// Alter Send to compress if remote side supports compression.
    /// </summary>
    [HarmonyPatch(typeof(ZSteamSocket), "SendQueuedPackages")]
    public class ZSteamSocketSendQueuedPackages
    {
        ///We are re-writing each of the outgoing byte arrays as a compressed package if necessary.
        private static void Prefix(ref ZSteamSocket __instance)
        {
            if ( Configuration.Local == null ||
                 Configuration.Local.NetworkConfiguration == null ||
                 ! Configuration.Local.NetworkConfiguration.enableCompression ||
                 ! VPlusNetworkStatusManager.SteamPeerSupportsCompression(__instance.GetPeerID())) {
                ///Don't do anything if compression isn't enabled.
                return; 
            }
            Queue<byte[]> compressedData = new Queue<byte[]>();
            while (__instance.m_sendQueue.Count > 0)
            {
                byte[] uncompressed = __instance.m_sendQueue.Dequeue();
                if (ZSteamSocketExtensions.IsCompressed(uncompressed))
                {
                    compressedData.Enqueue(uncompressed);
                }
                else
                {
                    byte[] compressed = LZ4.LZ4Codec.Wrap(uncompressed);
                    ZSteamSocketExtensions.UpdateOverallCompressionRatio(uncompressed.Length, compressed.Length);
                    compressedData.Enqueue(ZSteamSocketExtensions.MarkAsCompressed(compressed));
                }
            } 
        }
    }

    /// <summary>
    /// Alter Recv to decompress if required
    /// </summary>
    [HarmonyPatch(typeof(ZSteamSocket), "Recv")]
    public class ZSteamSocketRecv
    {
        ///This should written as a transpiler, but thats more effort than I care to put into this right now.
        private static bool Prefix(ref ZSteamSocket __instance, ref ZPackage __result)
        {
            if (!__instance.IsConnected())
            {
                __result = null;
                return true;
            }
            IntPtr[] array = new IntPtr[1];
            if (SteamNetworkingSockets.ReceiveMessagesOnConnection(__instance.m_con, array, 1) == 1)
            {
                SteamNetworkingMessage_t steamNetworkingMessage_t = (SteamNetworkingMessage_t)Marshal.PtrToStructure(array[0],typeof(SteamNetworkingMessage_t));
                byte[] rawInputData = new byte[steamNetworkingMessage_t.m_cbSize];
                Marshal.Copy(steamNetworkingMessage_t.m_pData, rawInputData, 0, steamNetworkingMessage_t.m_cbSize);
                //We want to unconditionally check for compression and decompress even if we don't have compression enabled for output.
                byte[] uncompressedData = rawInputData;
                if (ZSteamSocketExtensions.IsCompressed(rawInputData))
                {
                    uncompressedData = LZ4.LZ4Codec.Unwrap(rawInputData, sizeof(int));
                }
                ZPackage zpackage = new ZPackage(uncompressedData);
                steamNetworkingMessage_t.m_pfnRelease = array[0];
                steamNetworkingMessage_t.Release();
                __instance.m_totalRecv += zpackage.Size();
                __instance.m_gotData = true;
                __result = zpackage;
                return true;
            }
            __result = null;
            return true;
        }
    }




}
