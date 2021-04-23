using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ValheimPlus.RPC
{
    public class VPlusMapPinSync
    {
        /// <summary>
		/// Sync Pin with clients via the server
        /// </summary>
        public static void RPC_VPlusMapPinSync(long sender, ZPackage mapPinPkg)
        {
            if (ZNet.m_isServer) //Server
            {
                if (sender == ZRoutedRpc.instance.GetServerPeerID()) return;

                if (mapPinPkg == null) return;

                ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.Everybody, "VPlusMapPinSync", new object[] { mapPinPkg });

                ZLog.Log($"Sent map pin to all clients");
                VPlusAck.SendAck(sender);
            }
            else //Client
            {
                if (sender != ZRoutedRpc.instance.GetServerPeerID()) return; //Only bother if it's from the server.

                if (mapPinPkg == null)
                {
                    ZLog.LogWarning("Warning: Got empty map pin package from server.");
                    return;
                }
                long pinSender = mapPinPkg.ReadLong();
                if (pinSender != ZRoutedRpc.instance.m_id)
                {
                    string senderName = ZRoutedRpc.instance.GetPeer(pinSender).m_playerName;
                    Vector3 pinPos = mapPinPkg.ReadVector3();
                    int pinType = mapPinPkg.ReadInt();
                    string pinName = mapPinPkg.ReadString();
                    if (!Minimap.instance.HaveSimilarPin(pinPos, (Minimap.PinType)pinType, pinName, true))
                    {
                        Minimap.PinData addedPin = Minimap.instance.AddPin(pinPos, (Minimap.PinType)pinType, pinName, true, false);
                        MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, $"Received map pin {pinName} from {senderName}!",
                            0, Minimap.instance.GetSprite((Minimap.PinType)pinType));
                        ZLog.Log($"I got pin named {pinName} from {senderName}!");
                    }
                }
                //Send Ack
                VPlusAck.SendAck(sender);
            }
        }

        /// <summary>
		/// Send the pin, attach client ID
        /// </summary>
        public static void SendMapPinToServer(Minimap.PinData pinData)
        {
            ZLog.Log("-------------------- SENDING VPLUS MapPin DATA");

            ZPackage pkg = new ZPackage();

            pkg.Write(ZRoutedRpc.instance.m_id); // Sender ID
            pkg.Write(pinData.m_pos); // Pin position
            pkg.Write((int)pinData.m_type); // Pin type
            pkg.Write(pinData.m_name); // Pin name

            ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.instance.GetServerPeerID(), "VPlusMapPinSync", new object[] { pkg });

            ZLog.Log($"Sent map pin {pinData.m_name} to the server");

        }
    }
}
