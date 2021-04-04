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
    /// Replace the existing SendZDOs function. This should be rewritten as a transpiler, but testing functionality first.
    /// </summary>
    [HarmonyPatch(typeof(ZDOMan), "SendZDOs")]
    public class ZDOManSendZDOs
    {
        private static bool Prefix(ref ZDOMan __instance, ref bool __result, ZDOMan.ZDOPeer peer, bool flush)
        {
            int numberOfBytesWeCanBuffer = 10240;
            if(peer.m_peer.m_socket is ZSteamSocket)
            {
                ZSteamSocket peerSocket = (ZSteamSocket)peer.m_peer.m_socket;
                //If the next send is outside our current timeslice we should wait so higher priority data can get buffered
                long microSecondsTillNextSend = peerSocket.GetEstimatedMicroSecondDelayTillNextTransfer();
                long microSecondsBetweenChunks = (long)(1000000.0f * (1.0f/ (float)ZDOMan.m_sendFPS)); //Currently hardcoded to 0.05f or 1/20th of a second.
                if (microSecondsTillNextSend > microSecondsBetweenChunks)
                {
                    //We do not expect that we can send data this chunk
                    __result = false; return false;
                }
                else
                {
                    numberOfBytesWeCanBuffer = (int)(peerSocket.GetEstimatedSendCapacity() / ZDOMan.m_sendFPS);
                }
            }
            else
            {
                //Original behavior.
                int sendQueueSize = peer.m_peer.m_socket.GetSendQueueSize();
                if (!flush && sendQueueSize > 10240)
                {
                    __result = false; return false;
                }
                numberOfBytesWeCanBuffer = 10240 - sendQueueSize;
                if (numberOfBytesWeCanBuffer < 2048)
                {
                    __result = false; return false;
                }
            }		
            __instance.m_tempToSync.Clear();
            __instance.CreateSyncList(peer, __instance.m_tempToSync);
            if (__instance.m_tempToSync.Count == 0 && peer.m_invalidSector.Count == 0)
            {
                __result = false; return false;
            }
            ZPackage outerDataWrapper = new ZPackage();
            bool haveInvalidSectorsToSend = false;
            if (peer.m_invalidSector.Count > 0)
            {
                haveInvalidSectorsToSend = true;
                outerDataWrapper.Write(peer.m_invalidSector.Count);
                foreach (ZDOID id in peer.m_invalidSector)
                {
                    outerDataWrapper.Write(id);
                }
                peer.m_invalidSector.Clear();
            }
            else
            {
                outerDataWrapper.Write(0);
            }
            float time = Time.time;
            ZPackage innerSerializer = new ZPackage();
            bool havePackageDataToSend = false;
            int toSyncIdx = 0;
            while (toSyncIdx < __instance.m_tempToSync.Count && outerDataWrapper.Size() <= numberOfBytesWeCanBuffer)
            {
                ZDO zdo = __instance.m_tempToSync[toSyncIdx];
                peer.m_forceSend.Remove(zdo.m_uid);
                if (!ZNet.instance.IsServer())
                {
                    __instance.m_clientChangeQueue.Remove(zdo.m_uid);
                }
                if (peer.ShouldSend(zdo))
                {
                    outerDataWrapper.Write(zdo.m_uid);
                    outerDataWrapper.Write(zdo.m_ownerRevision);
                    outerDataWrapper.Write(zdo.m_dataRevision);
                    outerDataWrapper.Write(zdo.m_owner);
                    outerDataWrapper.Write(zdo.GetPosition());
                    innerSerializer.Clear();
                    zdo.Serialize(innerSerializer);
                    outerDataWrapper.Write(innerSerializer);
                    peer.m_zdos[zdo.m_uid] = new ZDOMan.ZDOPeer.PeerZDOInfo(zdo.m_dataRevision, zdo.m_ownerRevision, time);
                    havePackageDataToSend = true;
                    __instance.m_zdosSent++;
                }
                toSyncIdx++;
            }
            outerDataWrapper.Write(ZDOID.None);
            if (havePackageDataToSend || haveInvalidSectorsToSend)
            {
                peer.m_peer.m_rpc.Invoke("ZDOData", new object[]
                {
                outerDataWrapper
                });
            }
            __result = havePackageDataToSend || haveInvalidSectorsToSend;
            return false;
        }
    }




}
